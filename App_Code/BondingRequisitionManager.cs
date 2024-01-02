using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using autouniv;
using System.Data;
using DocumentFormat.OpenXml.Drawing.Charts;
using DataTable = System.Data.DataTable;


/// <summary>
/// Summary description for BondingRequisitionManager
/// </summary>
public class BondingRequisitionManager
{
	public BondingRequisitionManager()
	{
		//
		// TODO: Add constructor logic here
		//
	}

    public static  DataTable GetPurchaseOrderMst(string ID)
    {
        string connectionString = DataManager.OraConnString();
        SqlConnection sqlCon = new SqlConnection(connectionString);
        string query = @"SELECT [ID]
      ,[Code]
      ,CONVERT(NVARCHAR,[Date],103) as Date,CONVERT(NVARCHAR,ExpDeliveryDate,103) as Date 
      ,[RequisitionBy]
      ,[Remarks]
      ,[Status]      
      ,[CreatedBy]
      ,[CreatedDate]
      ,[UpdateBy]
      ,[UpdateDate],AuthorizedDate
  FROM [dbo].[ItemBondingRequisitionMst] where ID='" + ID + "' and DeleteBy is null";
        DataTable dt = DataManager.ExecuteQuery(connectionString, query, "ItemRequisitionMst");
        sqlCon.Close();
        return dt;
    }


    public static void UpdateRequisitionOrder(ManufactureInformation aManufactureInformation, DataTable dt)
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

            command.CommandText = @"UPDATE [ItemBondingRequisitionMst]
   SET [Date] =convert(date,'" + aManufactureInformation.ManufagchuredDate + "',103),[ExpDeliveryDate] =convert(date,'" + aManufactureInformation.ExpDeliveryDate + "',103) ,[RequisitionBy] ='" + aManufactureInformation.ManufagchuredBy + "',Remarks='" + aManufactureInformation.Remarks + "',[UpdateBy] ='" + aManufactureInformation.AddBy + "' ,[UpdateDate] =GETDATE() WHERE ID='" + aManufactureInformation.ID + "'";
            command.ExecuteNonQuery();

            command.CommandText = @"delete from ItemBondingRequisitionDtl where MstID='" + aManufactureInformation.ID + "'";
            command.ExecuteNonQuery();

            foreach (DataRow dr in dt.Rows)
            {
                if (dr["ItemId"].ToString() != "" && dr["ItemId"].ToString() != "0")
                {
                   
                    command.CommandText = @"INSERT INTO [ItemBondingRequisitionDtl] ([MstId],[ItemId],[LengthId],WidthId,ThikneesId,[Quantity]) VALUES ('" + aManufactureInformation.ID + "','" + dr["ItemId"].ToString() + "','" + dr["LengthId"].ToString() + "','" + dr["WidthId"].ToString() + "','" + dr["ThiknessId"].ToString() + "','" + dr["Quantity"].ToString() + "')";
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


    public static void DeleteRequisitionOrder(ManufactureInformation aManufactureInformation, DataTable dt)
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

            command.CommandText = @"UPDATE [ItemBondingRequisitionMst]
   SET [DeleteBy] ='" + aManufactureInformation.AddBy + "' ,[DeleteDate] =GETDATE() WHERE ID='" + aManufactureInformation.ID + "'";
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

    public static string SaveManufactureInformation(ManufactureInformation aManufactureInformation, DataTable DataDtlInfo)
    {
        string OrderMstID ="";
        SqlConnection connection = new SqlConnection(DataManager.OraConnString());
        SqlTransaction transaction;
        try
        {
            connection.Open();

            transaction = connection.BeginTransaction();
            SqlCommand command = new SqlCommand();
            command.Connection = connection;
            command.Transaction = transaction;

            command.CommandText = @"INSERT INTO ItemBondingRequisitionMst
           ([Code],[Date],ExpDeliveryDate,[RequisitionBy],[Remarks],[CreatedBy],[CreatedDate],ProductionId)
     VALUES
           ('" + aManufactureInformation.GenerateCode + "',convert(date,'" + aManufactureInformation.ManufagchuredDate + "',103),convert(date,'" + aManufactureInformation.ExpDeliveryDate + "',103),'" + aManufactureInformation.ManufagchuredBy + "','" + aManufactureInformation.Remarks + "','" + aManufactureInformation.AddBy + "',GETDATE(),'" + aManufactureInformation.ProductionSerial + "')";


            command.ExecuteNonQuery();

            command.CommandText = @"SELECT top(1) [ID]  FROM [ItemBondingRequisitionMst] order by ID desc";
            OrderMstID = command.ExecuteScalar().ToString();

            foreach (DataRow dr in DataDtlInfo.Rows)
            {
                if (dr["ItemId"].ToString() != "" && dr["ItemId"].ToString() != "0")
                {
                    command.CommandText = @"INSERT INTO [ItemBondingRequisitionDtl] ([MstId],[ItemId],[LengthId],WidthId,ThikneesId,[Quantity]) VALUES ('" + OrderMstID + "','" + dr["ItemId"].ToString() + "','" + dr["LengthId"].ToString() + "','" + dr["WidthId"].ToString() + "','" + dr["ThiknessId"].ToString() + "','" + dr["Quantity"].ToString() + "')";
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
        return OrderMstID;
    }


    public static DataTable GetManufactureInformation(string ID)
    {
        string connectionString = DataManager.OraConnString();
        SqlConnection sqlCon = new SqlConnection(connectionString);
        string query = @"SELECT * from [View_ItemBondingRequisition] Where ID='" + ID + "' ";
        DataTable dt = DataManager.ExecuteQuery(connectionString, query, "Manufacturing_Master");
        sqlCon.Close();
        if (dt.Rows.Count == 0)
        {
            return null;
        }
        return dt;
    }



    public static DataTable GetInformationHistory(string ID, string ProductionCode)
    {
        string connectionString = DataManager.OraConnString();
        SqlConnection sqlCon = new SqlConnection(connectionString);
        string parameter = "where t1.DeleteBy is null";
        if (ID != "")
        {
            parameter = parameter + " and  t1.ID ='" + ID + "' ";
        }
        else if (ProductionCode != "")
        {
            parameter = parameter + " and  t1.ProductionCode ='" + ProductionCode + "' ";
        }
        string query = @"SELECT * from [View_ItemBondingRequisition] t1  " + parameter;
        DataTable dt = DataManager.ExecuteQuery(connectionString, query, "[View_ItemBondingRequisition]");
        sqlCon.Close();
        return dt;
    }
    public static DataTable GetUnAuthorizationInformationHistory(string ID, string ProductionCode)
    {
        string connectionString = DataManager.OraConnString();
        SqlConnection sqlCon = new SqlConnection(connectionString);
        string parameter = "where t1.DeleteBy is null AND t1.AuthorizedDate is null";
        if (ID != "")
        {
            parameter = parameter + " and  t1.ID ='" + ID + "' ";
        }
        else if (ProductionCode != "")
        {
            parameter = parameter + " and  t1.ProductionCode ='" + ProductionCode + "' ";
        }
        string query = @"SELECT * from [View_ItemBondingRequisition] t1  " + parameter;
        DataTable dt = DataManager.ExecuteQuery(connectionString, query, "[View_ItemBondingRequisition]");
        sqlCon.Close();
        return dt;
    }


    public static DataTable GetReceivedDetailsInfo(string Id)
    {

        string query = @"select st.Id,it.Name ,t1.ItemId, t1.MstId,t1.LengthId,t4.Length , t1.WidthId,t3.Width,t1.ThikneesId,t1.ThikneesId as ThiknessId,t2.Thikness,t1.Quantity
from ItemBondingRequisitionDtl t1 inner join
Manufacturing_Stock st on st.ID=t1.ItemID
inner join Item It on st.ItemId=It.Id left join ProductionThikness t2 on t1.ThikneesId=t2.ID left join ProductionWidth t3 on t1.WidthId=t3.ID 
left join ProductionLength t4 on t1.LengthId=t4.ID where t1.MstId='" +Id+"'";
        DataTable dt = DataManager.ExecuteQuery(DataManager.OraConnString(), query, "ItemBondingRequisitionDtl");
        return dt;
    }

    public static DataTable GetReceivedDetailsAllInfo(string Id)
    {

        //string query = "select t1.Id,it.Name , t1.MstId,t1.LengthId,isnull(t4.Length,0) as Length , t1.WidthId,isnull(t3.Width,0) as Width ,t1.ThikneesId,isnull(t2.Thikness,0)as Thikness,t1.Quantity,t5.Name as UOM,isnull(t6.BrandName,'') as BrandName from ItemBondingRequisitionDtl t1 inner join Item It on t1.ItemId=It.Id left join ProductionThikness t2 on t1.ThikneesId=t2.ID left join ProductionWidth t3 on t1.WidthId=t3.ID left join ProductionLength t4 on t1.LengthId=t4.ID left join UOM  t5 on t5.Id=it.uomId left join Brand t6 on t6.Id=it.Brand where t1.MstId='" + Id + "'";

        string query = "select t1.Id,it.Name , t1.MstId,t1.LengthId,isnull(t4.Length,0) as Length , t1.WidthId,isnull(t3.Width,0) as Width ,t1.ThikneesId, isnull(t2.Thikness,0)as Thikness,t1.Quantity,t5.Name as UOM,isnull(t6.BrandName,'') as BrandName from ItemBondingRequisitionDtl t1 inner join  Manufacturing_Stock mnfs on t1.ItemId=mnfs.Id inner join Item It on mnfs.ItemId=It.Id left join ProductionThikness t2 on t1.ThikneesId=t2.ID left join ProductionWidth t3 on t1.WidthId=t3.ID left join ProductionLength t4 on t1.LengthId=t4.ID left join UOM  t5 on t5.Id=it.uomId left join Brand t6 on t6.Id=it.Brand where t1.MstId='" + Id + "'";

        DataTable dt = DataManager.ExecuteQuery(DataManager.OraConnString(), query, "ItemBondingRequisitionDtl");
        return dt;
    }

    public static DataTable GetReceivedDetailsInfoWithRequisition(string Code)
    {

        string query = "select '' Id, t1.ItemId,t1.LengthId, t1.WidthId,t1.ThikneesId as ThiknessID, it.Name,t4.Length ,t3.Width, t2.Thikness as Thikness, t1.Quantity, (CONVERT(nvarchar,(t5.Code))) as ProductionCode, (CONVERT(nvarchar,(t5.ParentCode))) AS ParentCode, (t5.SerialNo) AS ProductionSerialNo from ItemBondingRequisitionMst mst inner join   ItemBondingRequisitionDtl t1 on mst.ID=t1.MstId inner join Manufacturing_Stock st on st.ID=t1.ItemId inner join Item It on st.ItemId=It.Id left join ProductionThikness t2 on t1.ThikneesId=t2.ID left join ProductionWidth t3 on t1.WidthId=t3.ID left join ProductionLength t4 on t1.LengthId=t4.ID inner join ProductionSetup t5 on t5.code=mst.ProductionId  where mst.Code='" + Code + "'";
        DataTable dt = DataManager.ExecuteQuery(DataManager.OraConnString(), query, "ItemBondingRequisitionDtl");
        return dt;
    }






    public static void UpdateRequisitionOrderAuthorization(ManufactureInformation aManufactureInformation, DataTable dt)
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

            command.CommandText = @"UPDATE [ItemBondingRequisitionMst]
   SET [Date] =convert(date,'" + aManufactureInformation.ManufagchuredDate + "',103),[ExpDeliveryDate] =convert(date,'" + aManufactureInformation.ExpDeliveryDate + "',103) ,[RequisitionBy] ='" + aManufactureInformation.ManufagchuredBy + "',Remarks='" + aManufactureInformation.Remarks + "',[UpdateBy] ='" + aManufactureInformation.AddBy + "' ,[UpdateDate] =GETDATE(),AuthorizedB='" + aManufactureInformation.AddBy + "',AuthorizedDate=GetDate() WHERE ID='" + aManufactureInformation.ID + "'";
            command.ExecuteNonQuery();

            command.CommandText = @"delete from ItemBondingRequisitionDtl where MstID='" + aManufactureInformation.ID + "'";
            command.ExecuteNonQuery();

            foreach (DataRow dr in dt.Rows)
            {
                if (dr["ItemId"].ToString() != "" && dr["ItemId"].ToString() != "0")
                {
                    command.CommandText = @"INSERT INTO [ItemBondingRequisitionDtl] ([MstId],[ItemId],[LengthId],WidthId,ThikneesId,[Quantity]) VALUES ('" + aManufactureInformation.ID + "','" + dr["ItemId"].ToString() + "','" + dr["LengthId"].ToString() + "','" + dr["WidthId"].ToString() + "','" + dr["ThiknessId"].ToString() + "','" + dr["Quantity"].ToString() + "')";
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

    public static void UpdateRequisitionOrderAuthorization(ManufactureInformation aManufactureInformation)
    {
        var connectionString = DataManager.OraConnString();
        var connection = new SqlConnection(connectionString);
        connection.Open();
        string query = @"UPDATE [ItemBondingRequisitionMst]
   SET AuthorizedBy='" + aManufactureInformation.AddBy + "',AuthorizedDate=GetDate() WHERE ID='" + aManufactureInformation.ID + "'";
        var command = new SqlCommand(query, connection);
        command.ExecuteNonQuery();
        connection.Close();

    }
}