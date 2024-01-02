using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using autouniv;
using System.Data.SqlClient;

/// <summary>
/// Summary description for AuthenaticationRequirmentManager
/// </summary>
public class AuthenaticationRequirmentManager
{

	public AuthenaticationRequirmentManager()
	{
		//
		// TODO: Add constructor logic here
		//
	}

    public DataTable GetProjectName()
    {
        try
        {
            string connectionString = DataManager.OraConnString();
            SqlConnection sqlCon = new SqlConnection(connectionString);
            string query = "select Id,ProjectName from Project_Setup_Tbl";
            DataTable dt = DataManager.ExecuteQuery(connectionString, query, "Project_Setup_Tbl");
            return dt;

        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }

    }

    public DataTable GetForRequimentAuthezatrion(string id) 
    {
        try 
        {
            string connectionstring = DataManager.OraConnString();
            SqlConnection sqlcon = new SqlConnection(connectionstring);
            string querey = @"select Id,item_Code,item_desc,ItemId,Total_Requisition , ISNULL(AutherizQty, 0) AS qnty, Remarks as Remarks from [MaterialRequisitionDtl] where MstId='" + id + "' and Total_Requisition > 0";
            DataTable dt = DataManager.ExecuteQuery(connectionstring, querey, "MaterialRequisitionDtl");
            return dt;
        }
        catch(Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public void UpdateRequirimentItemstock(DataTable dt,string id)
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

            var MstID = id;

            foreach (DataRow dr in dt.Rows)
            {
                if (dr["ID"].ToString() != "")
                {
                    var dtlId = dr["ID"].ToString();
                    var item_Code = dr["item_Code"].ToString();                                                       
                    var Qnty = Convert.ToDouble(dr["qnty"].ToString());
                    var txtRemark = dr["Remarks"].ToString();

                    command.CommandText = @"UPDATE MaterialRequisitionDtl SET AutherizQty ='" + Qnty + "', Remarks ='" + txtRemark + "'  WHERE  MstId='" + MstID + "' and Id='" + dtlId + "' and item_Code='" + item_Code + "'";
                    command.ExecuteNonQuery();
                                      
                }
            }
            command.CommandText = @"UPDATE MaterialRequisitionMst SET AutherizStatus ='1' WHERE Id='" + id + "'";
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

    public DataTable Getmaterialrequization() 
    {
        try 
        {
            string connectionstring = DataManager.OraConnString();
            SqlConnection sqlcon = new SqlConnection(connectionstring);
            string query = @"select t2.id [id],t2.RequisitionNo [RequisitionNo],CONVERT(date, t2.RequisitionDate,103) [RequisitionDate],t1.ProjectName [ProjectName],t1.Address [Address], CASE WHEN t2.AutherizStatus IS NULL then 'U' else 'A' end as [Status] from MaterialRequisitionMst t2  inner join Project_Setup_Tbl t1 on t1.Id=t2.ProjectId where AutherizStatus is Null and ApproveStatus ='A' order by Id desc";
            //string query = @"select t2.id [id],t2.RequisitionNo [RequisitionNo],CONVERT(varchar, t2.RequisitionDate,111) [RequisitionDate],t1.ProjectName [ProjectName],t1.Address [Address] from MaterialRequisitionMst t2  inner join Project_Setup_Tbl t1 on t1.Id=t2.ProjectId";
            DataTable dt = DataManager.ExecuteQuery(connectionstring, query, "MaterialRequisitionMst");
            return dt;
        }
        catch (Exception ex) 
        {
            throw new Exception(ex.Message);
        }
    }

    public DataTable getShowAllDatematerial(string id ) 
    {
        try
        {
            string connectionstring = DataManager.OraConnString();
            SqlConnection sqlcon = new SqlConnection(connectionstring);
            string query = @"select * from MaterialRequisitionMst where id='" + id + "'";
            DataTable dt = DataManager.ExecuteQuery(connectionstring, query, "MaterialRequisitionMst");
            return dt;

        }
        catch (Exception ex) 
        {
            throw new Exception(ex.Message);
        }
    }

    public DataTable GetProjectName(string id) 
    {
        try 
        {
            string connectionstring=DataManager.OraConnString();
            SqlConnection sqlcon = new SqlConnection(connectionstring);
            string query=@"select Id,projectName from Project_Setup_Tbl where Id='"+id+"'";
            DataTable dt =DataManager.ExecuteQuery(connectionstring,query,"Project_Setup_Tbl");
            return dt;
        }
        catch (Exception ex) 
        {
            throw new Exception(ex.Message);
        }
    }

    public DataTable UpdateMaterialValues(string Mstid) 
    {
        try
        {
            string connestionstring = DataManager.OraConnString();
            SqlConnection sqlcon = new SqlConnection(connestionstring);
            string querey = @"select id,item_Code,item_desc,This_time_Requisition Total_Requisition,This_time_Requisition [qnty],Remarks  from MaterialRequisitionDtl where MstId='" + Mstid + "'";
            DataTable dt = DataManager.ExecuteQuery(connestionstring, querey, "MaterialRequisitionDtl");
            return dt;
        }

        catch(Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }


  
   


  
}