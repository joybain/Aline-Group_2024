using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using autouniv;
using System.Data;
using System.Data.SqlClient;

/// <summary>
/// Summary description for ProjectManager
/// </summary>
public class ProjectManager
{
	public ProjectManager()
	{
		//
		// TODO: Add constructor logic here
		//
	}

    public static object GetCLient()
    {
        string connectionString = DataManager.OraConnString();
        SqlConnection sqlCon = new SqlConnection(connectionString);
        string query = "Select * from [Customer] ";

        DataTable dt = DataManager.ExecuteQuery(connectionString, query, "View_GetItemCatagory");
        return dt;
    }

    public int save(ProjectModel _valu)
    {
        String connectionString = DataManager.OraConnString();
        string InsertQuery = "insert into Project_Setup_Tbl(ClientId,ProjectName,Address) values ('" + _valu.ClientId + "','" +_valu.ProjectName + "','" + _valu.Address + "')";
        int Count = DataManager.SaveUpdateDelete(InsertQuery, connectionString);
        return Count;
    }

    public int Update(ProjectModel _valu)
    {
        String connectionString = DataManager.OraConnString();
        string InsertQuery = "Update Project_Setup_Tbl set ClientId='"+_valu.ClientId+"',ProjectName='"+_valu.ProjectName+"',Address='"+_valu.Address+"' where Id='"+_valu.Id+"'";
        int Count = DataManager.SaveUpdateDelete(InsertQuery, connectionString);
        return Count;
    }

    public object GateData()
    {

        String connectionString = DataManager.OraConnString();
        string query = "select t1.Id,t2.ContactName as ClientID,ProjectName,Address from Project_Setup_Tbl as t1 inner join [Customer] as t2 on t1.ClientId=t2.Id";
        DataTable dt = DataManager.ExecuteQuery(connectionString, query, "Student_Info_Table");
        return dt;
    }

    public static DataTable ShowData(string Id)
    {

        String connectionString = DataManager.OraConnString();
        string query = "Select * from Project_Setup_Tbl";
        DataTable dt = DataManager.ExecuteQuery(connectionString, query, "Student_Info_Table");
        return dt;
    }

    public int Delete(ProjectModel _valu)
    {
        String connectionString = DataManager.OraConnString();
        string InsertQuery = "delete from Project_Setup_Tbl where Id='"+_valu.Id+"'";
        int Count = DataManager.SaveUpdateDelete(InsertQuery, connectionString);
        return Count;
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

            command.CommandText = @"insert into ProposeProjectCostMst (ProjectName,Date,CostPrice) Values ('" + _CostMdl.ProjectName + "',convert(date,'" + _CostMdl.Date + "',103),'"+_CostMdl.CostPrice+"')";
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
        string query = @"select Id,ProjectName,Date,CostPrice from ProposeProjectCostMst where ID='" + ID + "'";
        DataTable dt = DataManager.ExecuteQuery(connectionString, query, "Item");
        return dt;
    }

    public static DataTable GetProjectCostDtl(string MstId)
    {
        string connectionString = DataManager.OraConnString();
        SqlConnection sqlCon = new SqlConnection(connectionString);
        string query = @"select  Id, item_code,item_desc, msr_unit_code,ItemRate,
Qnty,Total , Remarksany  from ProposeProjectCostDtl where MstId= '" + MstId + "'";
        DataTable dt = DataManager.ExecuteQuery(connectionString, query, "Item");
        return dt;
    }
}