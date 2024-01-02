using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using autouniv;
using System.Data.SqlClient;
using System.Data;

/// <summary>
/// Summary description for RequisitionOrderManager
/// </summary>
public class RequisitionOrderManager
{
	public RequisitionOrderManager()
	{
		//
		// TODO: Add constructor logic here
		//
	}

    public static RequisitionOrder GetPurchaseOrderMst(string ID)
    {
        string connectionString = DataManager.OraConnString();
        SqlConnection sqlCon = new SqlConnection(connectionString);
        string query = @"SELECT [ID]
      ,[Code]
      ,CONVERT(NVARCHAR,[Date],103) as Date
      ,[RequisitionBy]
      ,[Remarks]
      ,[Status]      
      ,[CreatedBy]
      ,[CreatedDate]
      ,[UpdateBy]
      ,[UpdateDate],AuthorizedDate
  FROM dbo.ItemRequisitionMst where ID='" + ID + "' and DeleteBy is null";
        DataTable dt = DataManager.ExecuteQuery(connectionString, query, "ItemRequisitionMst");
    
        sqlCon.Close();
        if (dt.Rows.Count == 0)
        {
            return null;
        }
        return new RequisitionOrder(dt.Rows[0]);
    }

    public static void SaveRequisitionOrder(RequisitionOrder pomst, DataTable dt)
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

            command.CommandText = @"INSERT INTO ItemRequisitionMst
           ([Code],[Date],[RequisitionBy],[Remarks],[CreatedBy],[CreatedDate])
     VALUES
           ('" + pomst.Code + "',convert(date,'" + pomst.Date + "',103),'" + pomst.RequisitionBy + "','" + pomst.Remarks + "','" + pomst.LoginBy + "',GETDATE())";
           
            
            command.ExecuteNonQuery();

            command.CommandText = @"SELECT top(1) [ID]  FROM [ItemRequisitionMst] order by ID desc";
            string OrderMstID = command.ExecuteScalar().ToString();

            foreach (DataRow dr in dt.Rows)
            {
                if (dr["ID"].ToString() != "" && dr["ID"].ToString() != "0")
                {
                    command.CommandText = @"INSERT INTO [ItemRequisitionDtl]
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

    public static void UpdateRequisitionOrder(RequisitionOrder pomst, DataTable dt)
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

            command.CommandText = @"UPDATE [ItemRequisitionMst]
   SET [Date] =convert(date,'" + pomst.Date + "',103) ,[RequisitionBy] ='" + pomst.RequisitionBy + "',Remarks='" + pomst.Remarks + "',[UpdateBy] ='" + pomst.LoginBy + "' ,[UpdateDate] =GETDATE() WHERE ID='" + pomst.ID + "'";
            command.ExecuteNonQuery();

            command.CommandText = @"delete from ItemRequisitionDtl where MstID='" + pomst.ID + "'";
            command.ExecuteNonQuery();

            foreach (DataRow dr in dt.Rows)
            {
                if (dr["ID"].ToString() != "" && dr["ID"].ToString() != "0")
                {
                    command.CommandText = @"INSERT INTO [ItemRequisitionDtl]
           ([MstID],[ItemID],[UnitPrice],[Quantity],[Total],[CreatedBy],[CreatedDate],[MsrUnitCode])
     VALUES
           ('" + pomst.ID + "','" + dr["ID"].ToString() + "','" + dr["item_rate"].ToString() + "','" + dr["qnty"].ToString() + "','" + Convert.ToDouble(dr["item_rate"].ToString()) * Convert.ToDouble(dr["qnty"].ToString()) + "','" + pomst.LoginBy + "',GETDATE(),'" + dr["msr_unit_code"].ToString() + "')";
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

    public static DataTable GetRequisitionInfo(string ID)
    {
        string connectionString = DataManager.OraConnString();
        SqlConnection sqlCon = new SqlConnection(connectionString);
        string query = @"SELECT t1.ID, case when Isnull(t1.[Status],'P')='P' then '0' else '1' end  [check]
      ,t1.Code
      ,CONVERT(NVARCHAR,t1.[Date],103) AS Date
      ,CONVERT(NVARCHAR,t2.F_name,103) AS  RequisitionBy    
      ,Remarks
      ,case when Isnull(t1.[Status],'P')='P' then 'Pending' 
       when t1.[Status]='A' then 'Authorized' 
       when t1.[Status]='D' then 'Delivered' else '' end AS [Status]
  FROM dbo.ItemRequisitionMst t1 inner join dbo.PMIS_PERSONNEL t2 on t2.ID=t1.RequisitionBy where t1.DeleteBy is null";
        DataTable dt = DataManager.ExecuteQuery(connectionString, query, "ItemPurOrderMst");
        return dt;     
    }

    public static DataTable GetDeliveryInfo(string ID)
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

    public static DataTable GetRequisitionInfoByPanding(string ID)
    {
        string connectionString = DataManager.OraConnString();
        SqlConnection sqlCon = new SqlConnection(connectionString);
        string query = @"SELECT t1.ID, case when Isnull(t1.[Status],'P')='P' then '0' else '1' end  [check]
      ,t1.Code
      ,CONVERT(NVARCHAR,t1.[Date],103) AS Date
      ,CONVERT(NVARCHAR,t2.F_name,103) AS  RequisitionBy    
      ,Remarks
      ,case when Isnull(t1.[Status],'P')='P' then 'Pending' 
       when t1.[Status]='A' then 'Authorized' 
       when t1.[Status]='D' then 'Delivered' else '' end AS [Status]
  FROM dbo.ItemRequisitionMst t1 inner join dbo.PMIS_PERSONNEL t2 on t2.ID=t1.RequisitionBy where t1.DeleteBy is null and Isnull(t1.[Status],'P')='P' ";
        DataTable dt = DataManager.ExecuteQuery(connectionString, query, "ItemPurOrderMst");
        return dt;
    }

    public static DataTable GetRequisitionOrderItemsDetails(string ID)
    {
        string connectionString = DataManager.OraConnString();
        SqlConnection sqlCon = new SqlConnection(connectionString);
        string query =
            @"SELECT t1.[ItemID] AS ID,t2.StyleNo AS item_code,t2.Name AS item_desc,CONVERT(nvarchar,t1.[UnitPrice]) AS item_rate,CONVERT(nvarchar,t1.[Quantity]) AS qnty,CONVERT(nvarchar,t1.[Total]) AS [Total],CONVERT(nvarchar,t1.[MsrUnitCode]) AS msr_unit_code,t3.Name AS UMO,'0' AS Additional,t4.BrandName
  FROM dbo.ItemRequisitionDtl t1 inner join Item t2 on t2.ID=t1.ItemID inner join UOM t3 on t3.ID=t1.MsrUnitCode left join Brand t4 on t4.ID=t2.Brand where t1.DeleteBy is null and t1.MstID='" + ID + "' union all select '','','','','','','','','','' ";
        DataTable dt = DataManager.ExecuteQuery(connectionString, query, "ItemPurOrderDtl");
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

    public static void DeleteRequisitionOrder(RequisitionOrder pomst)
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

            command.CommandText = @"UPDATE [ItemRequisitionMst]   SET [DeleteBy] = '" + pomst.LoginBy + "',[DeleteDate] ='" + Globals._localTime.ToString() + "' WHERE ID='" + pomst.ID + "'";
            command.ExecuteNonQuery();

            command.CommandText = @" UPDATE [ItemRequisitionDtl]  SET [DeleteBy] = '" + pomst.LoginBy + "',[DeleteDate] ='" + Globals._localTime.ToString() + "' WHERE  MstID='" + pomst.ID + "'";
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

    public static DataTable GetRequisitionOrderHistory(string PoNo, string SupplierID, string FromDate, string ToDate)
    {
        string connectionString = DataManager.OraConnString();
        SqlConnection sqlCon = new SqlConnection(connectionString);

        string Parameter = "";
        if (PoNo != "")
        {
            Parameter = Parameter + " and t1.[Code]='" + PoNo + "'";
        }
        else
        {
            //if (SupplierID != "")
            //{
            //    Parameter = Parameter + " and t2.ID=" + SupplierID + "";
            //}

            if (FromDate != "" && ToDate != "")
            {
                Parameter = Parameter + " and Convert(date,t1.[Date],103) between Convert(date,'" + FromDate + "',103) AND Convert(date,'" + ToDate + "',103) ";
            }
        }
        string query = @"SELECT t1.ID
      ,t1.[Code], case when Isnull(t1.[Status],'P')='P' then '0' else '1' end  as [check]
      ,CONVERT(NVARCHAR,t1.[Date],103) AS Date
      ,CONVERT(NVARCHAR,t2.F_Name,103) AS  Supplier_Name    
      
      ,case when Isnull(t1.[Status],'P')='P' then 'Pending' 
       when t1.[Status]='A' then 'Authorized' 
       when t1.[Status]='D' then 'Delivered' else '' end AS [Status]
  FROM [ItemRequisitionMst] t1 inner join dbo.PMIS_PERSONNEL t2 on t2.ID=t1.RequisitionBy where t1.DeleteBy is null " + Parameter + " order By t1.ID desc";


        DataTable dt = DataManager.ExecuteQuery(connectionString, query, "ItemPurOrderMst");
        return dt;
    }

    public static DataTable GetRequisitionOrderHistorybyPanding(string PoNo, string SupplierID, string FromDate, string ToDate)
    {
        string connectionString = DataManager.OraConnString();
        SqlConnection sqlCon = new SqlConnection(connectionString);

        string Parameter = "";
        if (PoNo != "")
        {
            Parameter = Parameter + " and t1.[Code]='" + PoNo + "'";
        }
        else
        {
            //if (SupplierID != "")
            //{
            //    Parameter = Parameter + " and t2.ID=" + SupplierID + "";
            //}

            if (FromDate != "" && ToDate != "")
            {
                Parameter = Parameter + " and Convert(date,t1.[Date],103) between Convert(date,'" + FromDate + "',103) AND Convert(date,'" + ToDate + "',103) ";
            }
        }
        string query = @"SELECT t1.ID
      ,t1.[Code], case when Isnull(t1.[Status],'P')='P' then '0' else '1' end  as [check]
      ,CONVERT(NVARCHAR,t1.[Date],103) AS Date
      ,CONVERT(NVARCHAR,t2.F_Name,103) AS  Supplier_Name    
      
      ,case when Isnull(t1.[Status],'P')='P' then 'Pending' 
       when t1.[Status]='A' then 'Authorized' 
       when t1.[Status]='D' then 'Delivered' else '' end AS [Status]
  FROM [ItemRequisitionMst] t1 inner join dbo.PMIS_PERSONNEL t2 on t2.ID=t1.RequisitionBy where t1.DeleteBy is null and  Isnull(t1.[Status],'P')='P' " + Parameter + " order By t1.ID desc";


        DataTable dt = DataManager.ExecuteQuery(connectionString, query, "ItemPurOrderMst");
        return dt;
    }

    public static void AuthorizedRequisition(DataTable dt, string UserID)
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

          
         
            foreach (DataRow dr in dt.Rows)
            {
                if (dr["ID"].ToString() != "" && dr["check"].ToString()=="1")
                {
                    command.CommandText = @"UPDATE [ItemRequisitionMst]
   SET [AuthorizedBy] ='" + UserID + "',Status='A' ,[AuthorizedDate] =GETDATE() WHERE ID='" + dr["ID"].ToString() + "'";
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
}