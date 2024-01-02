using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using autouniv;
using System.Data.SqlClient;

/// <summary>
/// Summary description for ItemDelieryinfoManager
/// </summary>
public class ItemDelieryinfoManager
{
	public ItemDelieryinfoManager()
	{
		//
		// TODO: Add constructor logic here
		//
	}

    public static itemDelieryinfo GetDeliveryInformation(string ID)
    {
        string connectionString = DataManager.OraConnString();
        SqlConnection sqlCon = new SqlConnection(connectionString);
        string query = @" SELECT [ID]
      ,[Code]
      ,CONVERT(NVARCHAR,[Date],103) as Date
      ,RequisitionID
      ,DeliveryBy
      ,RequisitionCode      
      ,[CreatedBy]
      ,[CreatedDate]
      ,[UpdateBy],Remarks
      ,[UpdateDate]
        FROM dbo.ItemDeliveryMst where ID='" + ID + "' and DeleteBy is null ";
        DataTable dt = DataManager.ExecuteQuery(connectionString, query, "ItemRequisitionMst");
        sqlCon.Close();
        if (dt.Rows.Count == 0)
        {
            return null;
        }
        return new itemDelieryinfo(dt.Rows[0]);
    }



    public static void UpdateRequisitionDeliuvery(itemDelieryinfo pomst, DataTable dt)
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

            command.CommandText = @"UPDATE [ItemDeliveryMst]
   SET [Date] =convert(date,'" + pomst.Date + "',103),RequisitionID='"+pomst.RequisitionID+"' ,RequisitionCode='"+pomst.RequisitionCode+"',[DeliveryBy] ='" + pomst.DeliveryBy + "',Remarks='" + pomst.Remarks + "',[UpdateBy] ='" + pomst.LoginBy + "' ,[UpdateDate] =GETDATE() WHERE ID='" + pomst.ID + "'";
            command.ExecuteNonQuery();

            command.CommandText = @"delete from dbo.ItemDeliveryDtl where MstID='" + pomst.ID + "'";
            command.ExecuteNonQuery();

            foreach (DataRow dr in dt.Rows)
            {
                if (dr["ID"].ToString() != "")
                {
                    command.CommandText = @"INSERT INTO [ItemDeliveryDtl]
           ([MstID],[ItemID],[UnitPrice],[Quantity],[Total],[CreatedBy],[CreatedDate],[MsrUnitCode])
     VALUES
           ('" + pomst.ID + "','" + dr["ID"].ToString() + "','" + dr["item_rate"].ToString() + "','" + dr["qnty"].ToString() + "','" + Convert.ToDouble(dr["item_rate"].ToString()) * Convert.ToDouble(dr["qnty"].ToString()) + "','" + pomst.LoginBy + "','" + Globals._localTime.ToString() + "','" + dr["msr_unit_code"].ToString() + "')";
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

    public static void SaveRequisitionDelivery(itemDelieryinfo pomst, DataTable dt)
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



            command.CommandText = @"Update ItemRequisitionMst set DeliveryFlag=1 where Id='" + pomst.RequisitionID+ "'";
            command.ExecuteNonQuery();

            command.CommandText = @"INSERT INTO ItemDeliveryMst
           ([Code],[Date],[RequisitionID],DeliveryBy,RequisitionCode,[Remarks],[CreatedBy],[CreatedDate])
     VALUES
           ('" + pomst.Code + "',convert(date,'" + pomst.Date + "',103),'" + pomst.RequisitionID + "','" + pomst.DeliveryBy + "','" + pomst.RequisitionCode + "','" + pomst.Remarks + "','" + pomst.LoginBy + "',GETDATE())";


            command.ExecuteNonQuery();

            command.CommandText = @"SELECT top(1) [ID]  FROM [ItemDeliveryMst] order by ID desc";
            string OrderMstID = command.ExecuteScalar().ToString();

            foreach (DataRow dr in dt.Rows)
            {
                if (dr["ID"].ToString() != "" && dr["ID"].ToString()!="0")
                {
                    command.CommandText = @"INSERT INTO [ItemDeliveryDtl]
           ([MstID],[ItemID],[UnitPrice],[Quantity],[Total],[CreatedBy],[CreatedDate],[MsrUnitCode])
     VALUES
           ('" + OrderMstID + "','" + dr["ID"].ToString() + "','" + dr["item_rate"].ToString() + "','" + dr["qnty"].ToString() + "','" + Convert.ToDouble(dr["item_rate"].ToString()) * Convert.ToDouble(dr["qnty"].ToString()) + "','" + pomst.LoginBy + "',GetDate(),'" + dr["msr_unit_code"].ToString() + "')";
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

    public static void DeleteRequisitionDelivery(itemDelieryinfo pomst)
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

            command.CommandText = @"UPDATE [ItemDeliveryMst]   SET [DeleteBy] = '" + pomst.LoginBy + "',[DeleteDate] ='" + Globals._localTime.ToString() + "' WHERE ID='" + pomst.ID + "'";
            command.ExecuteNonQuery();

            command.CommandText = @" UPDATE [ItemDeliveryDtl]  SET [DeleteBy] = '" + pomst.LoginBy + "',[DeleteDate] ='" + Globals._localTime.ToString() + "' WHERE  MstID='" + pomst.ID + "'";
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

    public static DataTable GetRequisitionDeliveryInfo(string p)
    {
        string connectionString = DataManager.OraConnString();
        SqlConnection sqlCon = new SqlConnection(connectionString);
        string query = @"SELECT t1.ID
      ,t1.Code
      ,CONVERT(NVARCHAR,t1.[Date],103) AS Date
      ,CONVERT(NVARCHAR,t2.F_name,103) AS  DeliveryBy    
      ,t1.Remarks ,t3.COde as RequisitionCode     
  FROM ItemDeliveryMst t1 inner join ItemRequisitionMst t3 on t3.ID=t1.RequisitionID  
   inner join PMIS_PERSONNEL t2 on t2.ID=t1.DeliveryBy where t1.DeleteBy is null ";
        DataTable dt = DataManager.ExecuteQuery(connectionString, query, "ItemDeliveryMst");
        return dt;
    }
    public static DataTable GetRequisitionOrderItemsDetailsfordelivery(string ID)
    {
        string connectionString = DataManager.OraConnString();
        SqlConnection sqlCon = new SqlConnection(connectionString);
        string query =
            @"SELECT t1.[ItemID] AS ID,t2.StyleNo AS item_code,t2.Name AS item_desc,CONVERT(nvarchar,t1.[UnitPrice]) AS item_rate,CONVERT(nvarchar,t1.[Quantity]) AS qnty,CONVERT(nvarchar,t1.[Total]) AS [Total],CONVERT(nvarchar,t1.[MsrUnitCode]) AS msr_unit_code,t3.Name AS UMO,'0' AS Additional,t4.BrandName
  FROM dbo.ItemRequisitionDtl t1 inner join Item t2 on t2.ID=t1.ItemID inner join UOM t3 on t3.ID=t1.MsrUnitCode left join Brand t4 on t4.ID=t2.Brand where t1.DeleteBy is null and t1.MstID='" + ID + "'  ";
        DataTable dt = DataManager.ExecuteQuery(connectionString, query, "ItemPurOrderDtl");
        return dt;
    }

    public static DataTable GetDeliveryInfoDetails(string ID)
    {
        string connectionString = DataManager.OraConnString();
        SqlConnection sqlCon = new SqlConnection(connectionString);
        string query =
            @"SELECT t1.[ItemID] AS ID,t2.StyleNo AS item_code,t2.Name AS item_desc,CONVERT(nvarchar,t1.[UnitPrice]) AS item_rate,CONVERT(nvarchar,t1.[Quantity]) AS qnty,CONVERT(nvarchar,t1.[Total]) AS [Total],CONVERT(nvarchar,t1.[MsrUnitCode]) AS msr_unit_code,t3.Name AS UMO,'0' AS Additional,t4.BrandName
  FROM dbo.ItemDeliveryDtl t1 inner join Item t2 on t2.ID=t1.ItemID inner join UOM t3 on t3.ID=t1.MsrUnitCode left join Brand t4 on t4.ID=t2.Brand where t1.DeleteBy is null and t1.MstID='" + ID + "'";
        DataTable dt = DataManager.ExecuteQuery(connectionString, query, "ItemPurOrderDtl");
        return dt;
    }
}