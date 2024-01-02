using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using autouniv;
using System.Data;

/// <summary>
/// Summary description for ProposeCostPriceManager
/// </summary>
public class ProposeCostPriceManager
{
	public ProposeCostPriceManager()
	{
		//
		// TODO: Add constructor logic here
		//
	}

    public static DataTable GetMaterialItem(string Item)
    {
        string connectionString = DataManager.OraConnString();
        SqlConnection sqlCon = new SqlConnection(connectionString);
        //string query = @" SELECT t1.[ID],t1.[Code] AS [item_code],t1.[Name] AS [item_desc],t1.[UOMID] AS [msr_unit_code],t1.[UnitPrice],t1.[Currency],t2.Name AS[UMO] ,t3.BrandName FROM [Item] t1 left join UOM t2 on t2.ID=t1.UOMID left join Brand t3 on t3.ID=t1.Brand  where  upper (t1.Code+ ' - '+t1.Name) = upper('" + criteria + "') and  t1.[Active]=1";
        string query =
            @"select t1.Id as Id, t1.Code as item_code,t1.name as item_desc,t1.UOMID as msr_unit_code,t1.UnitPrice as ItemRate,
'0.00' as Qnty,'0.00' as Total ,'' as Remarksany  from Item as t1 left  join ItemStock t2 on t1.ID=t2.ItemID 
inner join uom t3 on t1.UOMID=t3.ID where t1.Code+'-'+ t1.Name='" + Item + "'";

        DataTable dt = DataManager.ExecuteQuery(connectionString, query, "Item");
        return dt;
    }

    public object Gatedata()
    {
        String connectionString = DataManager.OraConnString();
        string query = "select Id,ProjectName,CONVERT(NVARCHAR,Date,103) as Date,CostPrice from ProposeProjectCostMst";
        DataTable dt = DataManager.ExecuteQuery(connectionString, query, "ProposeProjectCostMst");
        return dt;
    }


    public static void ProjectCostSave(DataTable dt, ProjectCostMdl _CostMdl)
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

            command.CommandText = @"insert into ProposeProjectCostMst (ProjectName,Date,CostPrice) Values ('" + _CostMdl.ProjectName + "',convert(date,'" + _CostMdl.Date + "',103),'" + _CostMdl.CostPrice + "')";
            command.ExecuteNonQuery();

            command.CommandText = @"SELECT top(1) [ID]  FROM [ProposeProjectCostMst] order by ID desc";
            string CostMstId = command.ExecuteScalar().ToString();

            foreach (DataRow dr in dt.Rows)
            {
                if (dr["ID"].ToString() != "")
                {
                    var ItemId = dr["ID"].ToString();
                    var Code = dr["item_code"].ToString();
                    var ItemName = dr["item_desc"].ToString();
                    var Uom = dr["msr_unit_code"].ToString();
                    var Remarks = dr["Remarksany"].ToString();
                    command.CommandText = @"insert into ProposeProjectCostDtl (MstId,item_code,item_desc,msr_unit_code,ItemRate,Qnty,Total,Remarksany) Values ('" + CostMstId + "','" + Code + "','" + ItemName + "','" + Uom + "','" + Convert.ToDouble(dr["ItemRate"].ToString()) + "','" + Convert.ToDouble(dr["Qnty"].ToString()) + "','" +
                                          Convert.ToDouble(dr["ItemRate"].ToString()) *
                                          Convert.ToDouble(dr["Qnty"].ToString()) + "','" + Remarks + "')";
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

    public static DataTable GetProjectCostMst(string ID)
    {
        string connectionString = DataManager.OraConnString();
        SqlConnection sqlCon = new SqlConnection(connectionString);
        string query = @"select Id,ProjectName,CONVERT(NVARCHAR,Date,103) as Date,CostPrice from ProposeProjectCostMst where ID='" + ID + "'";
        DataTable dt = DataManager.ExecuteQuery(connectionString, query, "Item");
        return dt;
    }

    public static DataTable GetProjectCostDtl(string MstId)
    {
        string connectionString = DataManager.OraConnString();
        SqlConnection sqlCon = new SqlConnection(connectionString);
        string query = @"select  t1.Id, item_code,item_desc, msr_unit_code,ItemRate,t2.Name as Uom,
Qnty,Total, Remarksany  from ProposeProjectCostDtl as t1  inner join UOM as t2 on t1.msr_unit_code=t2.Id where MstId= '" + MstId + "'";
        DataTable dt = DataManager.ExecuteQuery(connectionString, query, "Item");
        return dt;
    }

    public static void ProjectCostUpdate(DataTable dt, ProjectCostMdl _CostMdl)
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

            command.CommandText = @"UPDATE [ProposeProjectCostMst]
   SET ProjectName= '" + _CostMdl.ProjectName + "', Date= convert(date,'" + _CostMdl.Date + "',103),CostPrice='" + _CostMdl.CostPrice + "' WHERE Id='" + _CostMdl.Id + "'";
            command.ExecuteNonQuery();

            command.CommandText = @"delete from ProposeProjectCostDtl where MstId='" + _CostMdl.Id + "'";
            command.ExecuteNonQuery();

            foreach (DataRow dr in dt.Rows)
            {
                if (dr["ID"].ToString() != "")
                {
                    var ItemId = dr["ID"].ToString();
                    var Code = dr["item_code"].ToString();
                    var ItemName = dr["item_desc"].ToString();
                    var Uom = dr["msr_unit_code"].ToString();
                    var Remarks = dr["Remarksany"].ToString();
                    command.CommandText = @"insert into ProposeProjectCostDtl (MstId,item_code,item_desc,msr_unit_code,ItemRate,Qnty,Total,Remarksany) Values ('" + _CostMdl.Id + "','" + Code + "','" + ItemName + "','" + Uom + "','" + Convert.ToDouble(dr["ItemRate"].ToString()) + "','" + Convert.ToDouble(dr["Qnty"].ToString()) + "','" +
                                          Convert.ToDouble(dr["ItemRate"].ToString()) *
                                          Convert.ToDouble(dr["Qnty"].ToString()) + "','" + Remarks + "')";
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