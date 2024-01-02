using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using autouniv;
using Microsoft.ReportingServices.Interfaces;
using OldColor;

/// <summary>
/// Summary description for distrubationManager
/// </summary>
public class distrubationManager
{
    public distrubationManager()
    {

    }

    public static DataTable GetRqValu(string RqId, string ProjectId)
    {
        string connectionString = DataManager.OraConnString();
        SqlConnection sqlCon = new SqlConnection(connectionString);
        string query = @"


select t2.ItemId Id, item_code,item_desc,t3.Id as UOMID,t3.ID as msr_unit_code,Total_Requisition as requisition_qnty,
isnull(sum(t1.Quantity),0) as present_Stock,'' as this_time_requisition_qnty,'' as total_Stock ,'' as Remarksany,'' as duerequisition_qnty   from MaterialRequisitionDtl t2 right join 
(select t1.Id as MstId,t1.ProjectId,t2.Quantity,t2.ItemID  from MaterialRequisitionMst t1 inner join ItemStock t2 on  t2.ProjectId=t1.ProjectId ) t1
on t1.MstId=t2.MstId and t1.ItemID=T2.ItemId inner join UOM t3 on t2.msr_unit_code=t3.ID where t1.MstId='" + RqId +
                       "' and ProjectId='" + ProjectId + "' group by t2.ItemId, item_code,item_desc,t3.Id,Total_Requisition";
        DataTable dt = DataManager.ExecuteQuery(connectionString, query, "Item");
        return dt;
    }

    public static DataTable GetRqValu1(string RqId)
    {
        string connectionString = DataManager.OraConnString();
        SqlConnection sqlCon = new SqlConnection(connectionString);
        string query = @"Select t1.ItemId Id, item_code,item_desc,msr_unit_code,Present_Site_Requiremnt as item_rate,Total_Requisition,
(ISNULL(t1.Present_Site_Requiremnt,0)-sum(ISNULL(t2.Quantity,0))) as qnty from [MaterialRequisitionDtl] as t1 
inner join ItemPurOrderDtl As t2 on t1.ItemId=t2.ItemID inner join [ItemPurOrderMst] as t3 on t2.ItemOrderMstID=t3.ID  
where MstId='" + RqId + "' and t3.RequisitionId='" + RqId +
                       "' group by t1.ItemId, item_code,item_desc,msr_unit_code,Present_Site_Requiremnt,Total_Requisition";
        DataTable dt = DataManager.ExecuteQuery(connectionString, query, "Item");
        return dt;
    }

    public static DataTable GetMaterialItem(string Item)
    {
        string connectionString = DataManager.OraConnString();
        SqlConnection sqlCon = new SqlConnection(connectionString);
        //string query = @" SELECT t1.[ID],t1.[Code] AS [item_code],t1.[Name] AS [item_desc],t1.[UOMID] AS [msr_unit_code],t1.[UnitPrice],t1.[Currency],t2.Name AS[UMO] ,t3.BrandName FROM [Item] t1 left join UOM t2 on t2.ID=t1.UOMID left join Brand t3 on t3.ID=t1.Brand  where  upper (t1.Code+ ' - '+t1.Name) = upper('" + criteria + "') and  t1.[Active]=1";
        string query =
            @"select t2.ItemId Id, t1.Code as item_code,t1.name as item_desc,t3.Name as msr_unit_code,'0' as requisition_qnty,
t2.Quantity as present_Stock,'' as this_time_requisition_qnty,'' as total_Stock ,'' as Remarksany  from Item as t1 inner  join ItemStock t2 on t1.ID=t2.ItemID inner join uom t3 on t1.UOMID=t3.ID where t1.Code+'-'+ t1.Name='" +
            Item + "'";

        DataTable dt = DataManager.ExecuteQuery(connectionString, query, "Item");
        return dt;
    }

    public static DataTable GetThisValu(string textBoxValue)
    {
        string connectionString = DataManager.OraConnString();
        SqlConnection sqlCon = new SqlConnection(connectionString);
        //string query = @" SELECT t1.[ID],t1.[Code] AS [item_code],t1.[Name] AS [item_desc],t1.[UOMID] AS [msr_unit_code],t1.[UnitPrice],t1.[Currency],t2.Name AS[UMO] ,t3.BrandName FROM [Item] t1 left join UOM t2 on t2.ID=t1.UOMID left join Brand t3 on t3.ID=t1.Brand  where  upper (t1.Code+ ' - '+t1.Name) = upper('" + criteria + "') and  t1.[Active]=1";
        string query =
            @"select t2.ItemId Id, t1.Code as item_code,t1.name as item_desc,t3.Name as msr_unit_code,'0' as requisition_qnty,
t2.Quantity as present_Stock,'' as this_time_requisition_qnty,'' as total_Stock ,'' as Remarksany  from Item as t1 inner  join ItemStock t2 on t1.ID=t2.ItemID inner join uom t3 on t1.UOMID=t3.ID where t1.Code='" +
            textBoxValue + "'";

        DataTable dt = DataManager.ExecuteQuery(connectionString, query, "Item");
        return dt;
    }

    


    public static void distrubationSave(DataTable dt, DistrubationModel _distrubationMdl)
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

            command.CommandText = @"insert into ItemDistributionMst(Code,ProjectId,CreateDate,ChalanNo,RequisitionId,Remarks,Requisition_by,Requisition_Code,TransferType,Status,HeadStatus)
values('" + _distrubationMdl.Code + "','" + _distrubationMdl.ProjectId + "',convert(date,'" + _distrubationMdl.Date + "',103),'" + _distrubationMdl.ChalanNo + "','" + _distrubationMdl.RequisitionID + "','" + _distrubationMdl.Remark + "','" + _distrubationMdl.LoginBy + "','" + _distrubationMdl.RequisitionCode + "','A','1','"+_distrubationMdl.ItemType+"')";
            command.ExecuteNonQuery();

            command.CommandText = @"insert into ProjectStockTransferMst (Code,TransferToProjectId,TransferDate,Remark,AddBy,TransferFromProjectId,Status)
values('" + _distrubationMdl.Code + "','" + _distrubationMdl.ProjectId + "',convert(date,'" + _distrubationMdl.Date + "',103),'" + _distrubationMdl.Remark + "','" + _distrubationMdl.LoginBy + "','" + _distrubationMdl.ProjectId + "','0')";
            command.ExecuteNonQuery();

            command.CommandText = @"SELECT top(1) [ID]  FROM [ItemDistributionMst] order by ID desc";
            string DistributMstId = command.ExecuteScalar().ToString();


            command.CommandText = @"SELECT top(1) [ID]  FROM [ProjectStockTransferMst] order by ID desc";
            string TransferMstId = command.ExecuteScalar().ToString();

            foreach (DataRow dr in dt.Rows)
            {
                if (dr["ID"].ToString() != "")
                {
                    var ItemId = dr["ID"].ToString();
                    var Code = dr["item_code"].ToString();
                    var ItemName = dr["item_desc"].ToString();
                    var Uom = dr["msr_unit_code"].ToString();
                    var RequisQty = Convert.ToDouble(dr["requisition_qnty"].ToString());
                    //var PresentQty = dr["present_Stock"].ToString();
                    //var ThistimeQty =(dr["this_time_requisition_qnty"].ToString());
                    var TotalStock = dr["total_Stock"].ToString();
                    var Remarks = dr["Remarksany"].ToString();
                    command.CommandText = @"insert into ItemDistributionDtl(MstId,item_desc,ItemId,item_code,msr_unit_code,requisition_qnty,this_time_requisition_qnty,present_Stock,distribution_qty,Remarksany,total_Stock)
values('" + DistributMstId + "','" + ItemName + "','" + ItemId + "','" + Code + "','" + Uom + "','" + RequisQty + "','" +
                                          dr["this_time_requisition_qnty"].ToString() + "','" +
                                          dr["this_time_requisition_qnty"].ToString() + "','" +
                                          dr["this_time_requisition_qnty"].ToString() + "','" + Remarks + "','" +
                                          dr["total_Stock"].ToString() +
                                          "')";
                    command.ExecuteNonQuery();
                    if (_distrubationMdl.ItemType=="1")
                    {
                        command.CommandText = @"update ItemStock set Quantity=Quantity-'" + dr["this_time_requisition_qnty"].ToString() + "' where ItemID='" + ItemId + "'";
                        command.ExecuteNonQuery();
                    }
                    else
                    {
                        command.CommandText = @"update ItemStock set Quantity=Quantity-'" + dr["this_time_requisition_qnty"].ToString() + "' where ItemID='" + ItemId + "' and MaterialId='" + _distrubationMdl.RequisitionID + "'";
                        command.ExecuteNonQuery();
                    }
                  
//*****************************Save ItemDistributionDtl Table***********************
                    command.CommandText = @" insert into ProjectStockTransferDtl (MstId,ItemId,TransferQuantity,Code,DtlId,ToProjectID,UomId)
values('" + TransferMstId + "','" + ItemId + "','" +
                                          dr["this_time_requisition_qnty"].ToString() + "','" + Code + "','" +
                                          DistributMstId +
                                          "','" + _distrubationMdl.ProjectId + "','"+Uom+"')";
                    command.ExecuteNonQuery();

                 }
            }

            command.CommandText = @"select sum(Qnty) As Qnty from 
(Select (ISNULL(t1.Present_Site_Requiremnt,0)-sum(ISNULL(t2.distribution_qty,0))) as qnty from [MaterialRequisitionDtl] as t1 
inner join ItemDistributionDtl As t2 on t1.ItemId=t2.ItemID inner join ItemDistributionMst as t3 on t2.MstId=t3.ID  where t3.RequisitionId='" + _distrubationMdl.RequisitionID + "' and t3.ProjectId='" + _distrubationMdl.ProjectId + "' group by t1.Present_Site_Requiremnt) as t1 ";
            string Distribustatus = command.ExecuteScalar().ToString();
            if (Distribustatus.Equals("0"))
            {
                command.CommandText = @"Update MaterialRequisitionMst set DistributionStatus='C' where Id='" +
                                      _distrubationMdl.RequisitionID + "' ";
                command.ExecuteNonQuery();
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
        string query = @"select t1.ID,Code,t2.ProjectName,Requisition_Code,t3.USER_NAME as  Requisition_by,CreateDate,RequisitionId, CASE WHEN t1.RecivedBy IS NULL then 'Not Received' else 'Received' end as [Status] from [ItemDistributionMst]  t1 inner join Project_Setup_Tbl t2 on t1.ProjectId=t2.Id inner join UTL_USERINFO t3 on t1.Requisition_by=t3.ID";
        DataTable dt = DataManager.ExecuteQuery(connectionString, query, "Item");
        return dt;
    }

    public static DataTable GetPurchaseMaterialMst(string ID)
    {
        string connectionString = DataManager.OraConnString();
        SqlConnection sqlCon = new SqlConnection(connectionString);
        string query = @"Select * from [ItemDistributionMst] where ID='" + ID + "'";
        DataTable dt = DataManager.ExecuteQuery(connectionString, query, "Item");
        return dt;
    }

    public static DataTable GetMetrialsDetails(string MstId)
    {
        string connectionString = DataManager.OraConnString();
        SqlConnection sqlCon = new SqlConnection(connectionString);
        string query =
            @"SELECT ID,item_desc,item_code,msr_unit_code,requisition_qnty,this_time_requisition_qnty,present_Stock,Remarksany,total_Stock,ISNULL(0,0) as duerequisition_qnty  FROM [ItemDistributionDtl]  where MstId='" + MstId + "'";
        DataTable dt = DataManager.ExecuteQuery(connectionString, query, "MaterialRequisitionDtl");
        return dt;
    }

    public static DataTable GetRqValu1(string RqId, string Projectid)
    {
        string connectionString = DataManager.OraConnString();
        SqlConnection sqlCon = new SqlConnection(connectionString);
        string query = @" select tt.ItemId Id,item_desc,item_Code,t2.msr_unit_code as UOMID,t2.msr_unit_code as msr_unit_code,Total_Requisition as requisition_qnty,Quantity as present_Stock,'' as this_time_requisition_qnty,'' as total_Stock,'' as Remarksany,Total_Requisition-DistributionQty as duerequisition_qnty from MaterialRequisitionMst t1 Inner join MaterialRequisitionDtl t2 on t1.Id=t2.MstId inner join ItemStock t3 on t1.Id=t3.MaterialId and t1.ProjectId=t3.ProjectId and t2.ItemId=t3.ItemID left join (select t1.RequisitionId,t2.ItemId,t1.ProjectId,sum(t2.distribution_qty) as DistributionQty from ItemDistributionMst t1 inner join ItemDistributionDtl t2 on t1.ID=t2.MstId where t1.RequisitionId='" + RqId + "'  group by t1.RequisitionId,t2.ItemId,t1.ProjectId) tt on t1.Id=tt.RequisitionId and t2.ItemId=tt.ItemId  and t1.ProjectId=tt.ProjectId where t1.Id='" + RqId + "' and t3.Quantity>0";
        DataTable dt = DataManager.ExecuteQuery(connectionString, query, "Item");
        return dt;
    }





//    public static int getMatarial(string MatarialId, string ProjectId)
//    {
//        String connectionString = DataManager.OraConnString();
//        string InsertQuery =  @"select Count((ISNULL(Total_Requisition,0))-
//(ISNULL(t4.present_Stock,0)))as requisition_qnty
// from MaterialRequisitionDtl t2 inner join 
//(select t1.Id as MstId,t1.ProjectId,t2.Quantity,t2.ItemID  from MaterialRequisitionMst t1 inner join ItemStock t2 on t1.Id=t2.MaterialId and t2.ProjectId=t1.ProjectId) t1
//on t1.MstId=t2.MstId and t1.ItemID=T2.ItemId inner join UOM t3 on t2.msr_unit_code=t3.ID inner join ItemDistributionDtl t4
//on t2.ItemId=t4.ItemId where t1.MstId='" + MatarialId + "' and ProjectId='" + ProjectId + "' and requisition_qnty<0";
//        int Count = DataManager.SaveUpdateDelete(InsertQuery, connectionString);
//        return Count;
//    }

    public static DataTable GetDistrubationItemsDetails(string MstId)
    {
        string connectionString = DataManager.OraConnString();
        SqlConnection sqlCon = new SqlConnection(connectionString);
        string query =
            @"SELECT t1.ID,item_desc,item_code,t2.Name msr_unit_code,requisition_qnty,this_time_requisition_qnty,distribution_qty,present_Stock,Remarksany,total_Stock,ISNULL(requisition_qnty,0)-ISNULL(this_time_requisition_qnty,0) as duerequisition_qnty  FROM [ItemDistributionDtl] t1 inner join UOM as t2 on t1.msr_unit_code=t2.ID where MstId='" + MstId + "'";
        DataTable dt = DataManager.ExecuteQuery(connectionString, query, "MaterialRequisitionDtl");
        return dt;
    }


    public static DataTable GetShowREportDistrubation(string ProjectId, string FromDate, string ToDate, string SupplierID, string StyleNo, string CetegoryID, string SubCetegoryID, string DesignNo, string Flag, string ReportType)
    {
        SqlConnection conn = new SqlConnection(DataManager.OraConnString());
        DataSet ds = new DataSet("ReportInformation");
        SqlCommand sqlComm = new SqlCommand("ReportInformation", conn);

        if (!string.IsNullOrEmpty(ProjectId))
        {
            sqlComm.Parameters.AddWithValue("@ItemID", ProjectId);
        }

        else
        {
            sqlComm.Parameters.AddWithValue("@ItemID", null);
        }

        if (FromDate == "" || ToDate == "")
        {
            sqlComm.Parameters.AddWithValue("@FromDate", null);
            sqlComm.Parameters.AddWithValue("@ToDate", null);
        }

        else
        {
            FromDate = DataManager.DateEncodestring(FromDate);
            ToDate = DataManager.DateEncodestring(ToDate);

            sqlComm.Parameters.AddWithValue("@FromDate", FromDate);
            sqlComm.Parameters.AddWithValue("@ToDate", ToDate);
        }
        if (!string.IsNullOrEmpty(StyleNo))
        {
            sqlComm.Parameters.AddWithValue("@StyleNo", StyleNo);
        }

        else
        {
            sqlComm.Parameters.AddWithValue("@StyleNo", null);
        }

        if (!string.IsNullOrEmpty(SupplierID) && SupplierID != "0")
        {
            sqlComm.Parameters.AddWithValue("@SupplierID", SupplierID);
        }

        else
        {
            sqlComm.Parameters.AddWithValue("@SupplierID", null);
        }

        if (!string.IsNullOrEmpty(CetegoryID))
        {
            sqlComm.Parameters.AddWithValue("@CetegoryID", CetegoryID);
        }

        else
        {
            sqlComm.Parameters.AddWithValue("@CetegoryID", null);
        }

        if (!string.IsNullOrEmpty(SubCetegoryID))
        {
            sqlComm.Parameters.AddWithValue("@SubCetegoryID", SubCetegoryID);
        }

        else
        {
            sqlComm.Parameters.AddWithValue("@SubCetegoryID", null);
        }

        if (!string.IsNullOrEmpty(DesignNo))
        {
            sqlComm.Parameters.AddWithValue("@DesignNo", DesignNo);
        }

        else
        {
            sqlComm.Parameters.AddWithValue("@DesignNo", null);
        }

        if (!string.IsNullOrEmpty(Flag))
        {
            sqlComm.Parameters.AddWithValue("@Flag", Flag);
        }

        if (!string.IsNullOrEmpty(ReportType))
        {
            sqlComm.Parameters.AddWithValue("@ReportType", ReportType);
        }

        else
        {
            sqlComm.Parameters.AddWithValue("@ReportType", null);
        }
        sqlComm.CommandType = CommandType.StoredProcedure;

        SqlDataAdapter da = new SqlDataAdapter();
        da.SelectCommand = sqlComm;

        da.Fill(ds);

        return ds.Tables[0];
    }



    public DataTable RecivedMst(string ProjectId)
    {
        string connectionString = DataManager.OraConnString();
        SqlConnection sqlCon = new SqlConnection(connectionString);
        // string query = @"select t1.ItemId as ID,t1.Code as item_code,t3.Name as item_desc,t1.UomId as msr_unit_code,t1.TransferQuantity as present_qnty,'' as  Remarks,'1' AS [check],(select Isnull(sum(requisition_qnty)-sum(distribution_qty),0) as Due_qnty from ItemDistributionDtl t1 inner join ItemDistributionMst t2 on t1.MstId=t2.ID where t2.RequisitionId='33') Due_qnty from ProjectStockTransferDtl t1 inner join ProjectStockTransferMst t2 on t1.MstId=t2.id inner join Item t3 on t1.ItemId=t3.ID where MstId='33'";

        string query = @"select t1.Id,Code,(select t3.ProjectName from  Project_Setup_Tbl t3 where t1.TransferFromProjectId=t3.id) as fromProjectName,t3.ProjectName as ToProjectName,convert(nvarchar,TransferDate,103) as TransferDate,Remark,CASE WHEN t1.Status = 0 then 'Not Received' else 'Received' end as [Status]  from ProjectStockTransferMst t1 inner join Project_Setup_Tbl t3 on t1.TransferToProjectId=t3.id WHERE t1.TransferTOProjectId = '" + ProjectId + "' order by Id,t1.Status Asc";
        DataTable dt = DataManager.ExecuteQuery(connectionString, query, "Item");
        return dt;
    }

    public DataTable RecivedMstall()
    {
        string connectionString = DataManager.OraConnString();
        SqlConnection sqlCon = new SqlConnection(connectionString);
        string query = @"Select t1.Id,Code,t2.ProjectName as ProjectName,convert(nvarchar,CreateDate,103) as TransferDate,t1.Remarks as Remark,CASE WHEN t1.LocalUplode IS NULL then 'Not Received' else 'Received' end as [Status]  from ItemDistributionMst t1 inner join Project_Setup_Tbl t2 on t1.ProjectId=t2.Id  order by Id,t1.LocalUplode desc";
        //string query = @"select t1.Id,Code,(select t3.ProjectName from  Project_Setup_Tbl t3 where t1.TransferFromProjectId=t3.id) as fromProjectName,t3.ProjectName as ToProjectName,convert(nvarchar,TransferDate,103) as TransferDate,Remark,CASE WHEN t1.Status = 0 then 'Not Received' else 'Received' end as [Status]  from ProjectStockTransferMst t1 inner join Project_Setup_Tbl t3 on t1.TransferToProjectId=t3.id order by Id,t1.Status Asc";
        DataTable dt = DataManager.ExecuteQuery(connectionString, query, "Item");
        return dt;
    }

    public static DataTable GetShowREportOprationSummery(string ProjectId)
    {
        SqlConnection conn = new SqlConnection(DataManager.OraConnString());
        DataSet ds = new DataSet("SP_OprationSummery");
        SqlCommand sqlComm = new SqlCommand("SP_OprationSummery", conn);

        if (!string.IsNullOrEmpty(ProjectId))
        {
            sqlComm.Parameters.AddWithValue("@ProjectId", ProjectId);
        }

        else
        {
            sqlComm.Parameters.AddWithValue("@ProjectId", null);
        }

       
        sqlComm.CommandType = CommandType.StoredProcedure;

        SqlDataAdapter da = new SqlDataAdapter();
        da.SelectCommand = sqlComm;

        da.Fill(ds);

        return ds.Tables[0];
    }

    public static DataTable GetItemsForHeadOffice(string ItemCode)
    {
        string connectionString = DataManager.OraConnString();
        SqlConnection sqlCon = new SqlConnection(connectionString);
        string query = @"select ItemID as Id, t1.Code as  item_code,t2.Name as item_desc,t2.UOMID,t2.UOMID as msr_unit_code,'' as requisition_qnty,isnull(t1.Quantity,0) as present_Stock,'' as this_time_requisition_qnty,'' as total_Stock,'' as Remarksany,'' as duerequisition_qnty  from ItemStock t1 inner join Item t2 on t1.ItemID=t2.ID where t2.Code='" + ItemCode + "'";


        DataTable dt = DataManager.ExecuteQuery(connectionString, query, "ItemStock");
        return dt;
    }
}