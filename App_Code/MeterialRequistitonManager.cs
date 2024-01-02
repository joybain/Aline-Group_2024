using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using autouniv;

/// <summary>
/// Summary description for MeterialRequistitonManager
/// </summary>
public class MeterialRequistitonManager
{
	public MeterialRequistitonManager()
	{
		//
		// TODO: Add constructor logic here
		//
	}


    public static DataTable GetMeasure()
    {
        string connectionString = DataManager.OraConnString();
        SqlConnection sqlCon = new SqlConnection(connectionString);
        string query = "select ID,Name from UOM";
        DataTable dt = DataManager.ExecuteQuery(connectionString, query, "Measure");
        return dt;
    }
    public static DataTable GetMaterialItem(string Item,string projectId)
    {
        string connectionString = DataManager.OraConnString();
        SqlConnection sqlCon = new SqlConnection(connectionString);
        //string query = @" SELECT t1.[ID],t1.[Code] AS [item_code],t1.[Name] AS [item_desc],t1.[UOMID] AS [msr_unit_code],t1.[UnitPrice],t1.[Currency],t2.Name AS[UMO] ,t3.BrandName FROM [Item] t1 left join UOM t2 on t2.ID=t1.UOMID left join Brand t3 on t3.ID=t1.Brand  where  upper (t1.Code+ ' - '+t1.Name) = upper('" + criteria + "') and  t1.[Active]=1";
       // string query = @"select ID,Name as item_desc,Code as item_Code,UOMID as msr_unit_code,(SELECT ISNULL(SUM(t2.distribution_qty), 0) FROM ItemDistributionMst t1 INNER JOIN ItemDistributionDtl t2 ON t1.id = t2.MstId WHERE t1.ProjectId = '"+projectId+"' AND t2.item_code + '-' + t2.item_desc = '" + Item + "')  as Previous, '0' as Total,'0' as SupplidQnt,'0' as PresentStock,'0' as PSRequirement,'' as Remarks from Item where Code+'-'+ Name='" + Item + "'";

        string query = @"select ID,Name as item_desc,Code as item_Code,UOMID as msr_unit_code,(SELECT ISNULL(SUM(t2.distribution_qty), 0) FROM ItemDistributionMst t1 INNER JOIN ItemDistributionDtl t2 ON t1.id = t2.MstId WHERE t1.ProjectId = '" + projectId + "' AND t2.item_code + '-' + t2.item_desc = '" + Item + "')  as Previous, '0' as Total,'0' as SupplidQnt, (select ISNULL(Sum(t1.Quntity),0) as TotalStock from tbl_ProjectItemStock t1 inner join Item t2 on t1.ItemID=t2.ID where upper (t2.Code+ '-'+t2.Name)=upper('" + Item + "') and  t2.[Active]=1 and t1.ProjectId='" + projectId + "') as PresentStock,'0' as PSRequirement,'' as Remarks from Item where  upper (Code+ '-'+Name)=upper('" + Item + "')";

        DataTable dt = DataManager.ExecuteQuery(connectionString, query, "Item");
        return dt;
    }



    public static object GetProject()
    {
        string connectionString = DataManager.OraConnString();
        SqlConnection sqlCon = new SqlConnection(connectionString);
        string query = @"select * from [dbo].[Project_Setup_Tbl]";
        DataTable dt = DataManager.ExecuteQuery(connectionString, query, "Item");
        return dt;
    }



    public static void Save(MeterialRequistitonModel _meterialMdl, DataTable dt)
    {
        SqlConnection connection = new SqlConnection(DataManager.OraConnString());
        SqlTransaction transaction;
        try
        {
            connection.Open();

            transaction = connection.BeginTransaction();
            SqlCommand command = new SqlCommand();
            command.Connection = connection;
            command.Transaction = transaction;

            command.CommandText = @"insert into [dbo].[MaterialRequisitionMst] (RequisitionNo,RequisitionDate,ProjectId,Address,Recoment,DistributionStatus,RequisitionStatus,ApproveStatus) 
VALUES ('" +_meterialMdl.RequisitionNo+"',convert(date,'" + _meterialMdl.Date + "',103),'"+_meterialMdl.ProjectId+"','"+_meterialMdl.Address+"','"+_meterialMdl.Recoment+"','P','P','U')";
            command.ExecuteNonQuery();

            command.CommandText = @"SELECT top(1) [ID]  FROM [MaterialRequisitionMst] order by ID desc";
            string MaterialMstId = command.ExecuteScalar().ToString();

            foreach (DataRow dr in dt.Rows)
            {
                if (dr["ID"].ToString() != "")
                {
                    var PresentStock = "0";
                    command.CommandText = @" SELECT sum(Quntity) PresentStock  FROM tbl_ProjectItemStock where ItemID='" + dr["Id"].ToString() + "' and ProjectId='" + _meterialMdl.ProjectId + "'";
                     PresentStock = command.ExecuteScalar().ToString();
                    if (string.IsNullOrEmpty(PresentStock))
                    {
                        PresentStock = "0";
                    }
                    //var
                    var SupplidQnt = dr["SupplidQnt"].ToString();
                    var Previous = dr["Previous"].ToString();
                    var thisTimeRe = dr["This_time_Requisition"].ToString();

                    command.CommandText = @"Insert into [dbo].[MaterialRequisitionDtl] (MstId,ItemId,item_desc,item_Code,msr_unit_code,Previous,This_time_Requisition,Total_Requisition,
Supplid_Requisition,Present_Stock,Present_Site_Requiremnt,Remarks) values ('" + MaterialMstId + "','" + dr["Id"].ToString() + "','" + dr["item_desc"].ToString() + "','" + dr["item_Code"] + "','" + dr["msr_unit_code"].ToString() + "','" + dr["Previous"].ToString() + "','" + dr["This_time_Requisition"].ToString() + "'," + Convert.ToDouble(dr["Previous"].ToString()) + "+" + Convert.ToDouble(dr["This_time_Requisition"].ToString()) + ",'" + dr["SupplidQnt"].ToString() + "','" + PresentStock + "',(" + (Convert.ToDouble(Previous) + " + " + Convert.ToDouble(thisTimeRe) + ") - (" + Convert.ToDouble(SupplidQnt) + " + " + Convert.ToDouble(PresentStock)) + "),'" + dr["Remarks"].ToString() + "')";
                    command.ExecuteNonQuery();
                }
            }

            transaction.Commit();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        finally
        {
            if (connection.State == ConnectionState.Open)
                connection.Close();
        }
    }

    public object GateData()
    {
        string connectionString = DataManager.OraConnString();
        SqlConnection sqlCon = new SqlConnection(connectionString);
        string query = @"select '0'  as [check],uu.USER_NAME as Approve, t1.Id,RequisitionNo,RequisitionDate,t2.ProjectName,t1.Address,t3.ContactName as ClientName,CASE WHEN t1.AutherizStatus IS NULL then 'U' else 'A' end as Authorize,t1.ApproveStatus from [dbo].[MaterialRequisitionMST] as t1 inner join 
Project_Setup_Tbl as t2 on t1.ProjectId=t2.Id inner join [dbo].[Customer] as t3 on t2.ClientId=t3.Id left join UTL_USERINFO uu on uu.id=t1.Approve";
        DataTable dt = DataManager.ExecuteQuery(connectionString, query, "Item");
        return dt;
    }





    public static DataTable GetPurchaseMaterialMst(string ID)
    {
        string connectionString = DataManager.OraConnString();
        SqlConnection sqlCon = new SqlConnection(connectionString);
        string query = @"Select * from MaterialRequisitionMST where ID='" + ID + "'";
        DataTable dt = DataManager.ExecuteQuery(connectionString, query, "Item");
        return dt;
    }

    public static DataTable GetMetrialsDetails(string MstId)
    {
        string connectionString = DataManager.OraConnString();
        SqlConnection sqlCon = new SqlConnection(connectionString);
        string query =
            @"Select '1' as [check], ItemId as Id,item_desc,item_Code,msr_unit_code,Previous,This_Time_Requisition,Total_Requisition,Supplid_Requisition as SupplidQnt,Present_Stock as PresentStock , Present_Site_Requiremnt,Remarks from [MaterialRequisitionDtl] where MstId='" + MstId + "'";
        DataTable dt = DataManager.ExecuteQuery(connectionString, query, "MaterialRequisitionDtl");
        return dt;
    }

    public static void Update(MeterialRequistitonModel _meterialMdl, DataTable dt)
    {
        SqlConnection connection = new SqlConnection(DataManager.OraConnString());
        SqlTransaction transaction;
        try
        {
            connection.Open();

            transaction = connection.BeginTransaction();
            SqlCommand command = new SqlCommand();
            command.Connection = connection;
            command.Transaction = transaction;

            command.CommandText = @"UPDATE [MaterialRequisitionMst]
   SET RequisitionNo= '" + _meterialMdl.RequisitionNo + "', RequisitionDate= '"+_meterialMdl.Date+"',ProjectId='" + _meterialMdl.ProjectId + "',Address='" + _meterialMdl.Address + "',Recoment='" + _meterialMdl.Recoment + "' WHERE Id='" + _meterialMdl.Id + "'";
            command.ExecuteNonQuery();

            command.CommandText = @"delete from MaterialRequisitionDtl where MstId='" + _meterialMdl.Id + "'";
            command.ExecuteNonQuery();

            foreach (DataRow dr in dt.Rows)
            {
                if (dr["ID"].ToString() != "")
                {
                    command.CommandText = @"Insert into [dbo].[MaterialRequisitionDtl] (MstId,ItemId,item_desc,item_Code,msr_unit_code,Previous,This_time_Requisition,Total_Requisition,
Supplid_Requisition,Present_Stock,Present_Site_Requiremnt,Remarks) values ('" + _meterialMdl.Id + "','" + dr["Id"].ToString() + "','" + dr["item_desc"].ToString() + "','" + dr["item_Code"] + "','" + dr["msr_unit_code"].ToString() + "','" + dr["Previous"].ToString() + "','" + dr["This_time_Requisition"].ToString() + "','" + Convert.ToDouble(dr["Previous"].ToString()) + Convert.ToDouble(dr["This_time_Requisition"].ToString()) + "','" + dr["SupplidQnt"].ToString() + "','" + dr["PresentStock"].ToString() + "','" + (Convert.ToDouble(dr["Previous"].ToString()) + Convert.ToDouble(dr["This_time_Requisition"].ToString()) - Convert.ToDouble(dr["SupplidQnt"].ToString()) + Convert.ToDouble(dr["PresentStock"].ToString())) + "','" + dr["Remarks"].ToString() + "')";
                    command.ExecuteNonQuery();

             
                }
            }

            transaction.Commit();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        finally
        {
            if (connection.State == ConnectionState.Open)
                connection.Close();
        }
    }

    public static DataTable GetMterialMst(string Id)
    {
        string connectionString = DataManager.OraConnString();
        SqlConnection sqlCon = new SqlConnection(connectionString);
        string query = @"select RequisitionNo,RequisitionDate,t2.ProjectName,t1.Address as Site,t1.Recoment from [dbo].[MaterialRequisitionMst] as t1 
inner join [dbo].[Project_Setup_Tbl] as t2 on t1.ProjectId=t2.Id where t1.Id='"+Id+"'";
        DataTable dt = DataManager.ExecuteQuery(connectionString, query, "MaterialRequisitionMst");
        return dt;
    }

    public static DataTable GetMterialDtl(string Id)
    {
        string connectionString = DataManager.OraConnString();
        SqlConnection sqlCon = new SqlConnection(connectionString);
        string query = @"Select item_desc,t2.Name as msr_unit_code,Previous,This_time_Requisition,Total_Requisition,
Supplid_Requisition,Present_Stock,Present_Site_Requiremnt,Remarks from [dbo].[MaterialRequisitionDtl] as t1 inner join [dbo].[UOM] as t2 on t1.msr_unit_code=t2.Id where MstId='"+Id+"'";
        DataTable dt = DataManager.ExecuteQuery(connectionString, query, "MaterialRequisitionDtl");
        return dt;
    }

    public void UpdateApproveStatusAuthorise(string ApproveBy,string MstId)
    {
        SqlConnection connection = new SqlConnection(DataManager.OraConnString());
        SqlTransaction transaction;
        try
        {
            connection.Open();

            transaction = connection.BeginTransaction();
            SqlCommand command = new SqlCommand();
            command.Connection = connection;
            command.Transaction = transaction;

            command.CommandText = @"UPDATE [MaterialRequisitionMst]
   SET Approve= '" + ApproveBy + "', ApproveDate= GetDate(),ApproveStatus='A' WHERE Id='" + MstId + "'";
            command.ExecuteNonQuery();

            transaction.Commit();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        finally
        {
            if (connection.State == ConnectionState.Open)
                connection.Close();
        }
    }
}