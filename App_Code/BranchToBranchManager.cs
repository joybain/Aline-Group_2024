using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using autouniv;
using System.Data.SqlClient;
using System.Data;
using System.Web.UI.WebControls;
using System.Web.UI;

/// <summary>
/// Summary description for BranchToBranchManager
/// </summary>
public class BranchToBranchManager
{
	public BranchToBranchManager()
	{
		//
		// TODO: Add constructor logic here
		//
	}


    public  DataTable GetProjectName()
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

    public  void ProjectToprojectSave(DataTable dt, ProjecctToProject _projectObj)
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

            command.CommandText = @"INSERT INTO ProjectToProjectTransMst(TransferCode,TransferDate,FormProjectID,ToProjectID,Remarks,AddBy,TotalQuantity) values('" + _projectObj.ProjectTransCode +"',convert(date,'" + _projectObj.transferDate+"',103),'" + _projectObj.formProjectID +"','" + _projectObj.ToProjectID +"','" +_projectObj.Remarks +"','" + _projectObj.Addby +"','"+ _projectObj.TotalQnty +"')";
            command.ExecuteNonQuery();


            command.CommandText = @"insert into ProjectStockTransferMst (Code,TransferDate,TransferToProjectId,Remark,AddBy,TransferFromProjectId,Status) values('" + _projectObj.ProjectTransCode + "',convert(date,'" + _projectObj.transferDate + "',103),'" + _projectObj.ToProjectID + "','" + _projectObj.Remarks + "','" + _projectObj.Addby + "','" + _projectObj.formProjectID + "','0')";
            command.ExecuteNonQuery();

            command.CommandText = @"SELECT top(1) [ID]  FROM [ProjectStockTransferMst] order by ID desc";
            string TransferMstId = command.ExecuteScalar().ToString();


            command.CommandText = @"SELECT top(1) [ID]  FROM [ProjectToProjectTransMst] order by ID desc";
            string pMstId = command.ExecuteScalar().ToString();

            foreach (DataRow dr in dt.Rows)
            {
                if (dr["ID"].ToString() != "")
                {
                    var ItemId = dr["ID"].ToString();
                    var Code = dr["Code"].ToString();
                    var ItemName = dr["Name"].ToString();
                    var Uom = dr["UomId"].ToString();
                    var txtQnty = Convert.ToDouble(dr["qnty"].ToString());
                    var txtRemark = dr["Remarks"].ToString();
                    command.CommandText = @"INSERT INTO  ProjectToProjectTransDtl( MstId,ItemId,ItemName,Code,UOMID , Qunitity,Remarks) values('" + pMstId + "','"+ItemId+"','" + ItemName + "','" + Code + "',CONVERT(int,'"+Uom+"' ),'" + txtQnty + "','" + txtRemark + "')";
                    command.ExecuteNonQuery();

                    //*****************************Save ItemDistributionDtl Table***********************
                    command.CommandText = @" insert into ProjectStockTransferDtl (MstId,ItemId,TransferQuantity,Code,DtlId,ToProjectID,UomID)
values('" + TransferMstId + "','" + ItemId + "','" +
                                          dr["qnty"].ToString() + "','" + Code + "','" +
                                          pMstId +
                                          "','" + _projectObj.ToProjectID + "','"+Uom+"')";
                    command.ExecuteNonQuery();


                    command.CommandText = @"update [tbl_ProjectItemStock] set Quntity=Quntity-" + txtQnty + "  where ProjectId='" + _projectObj.formProjectID + "' and ID='" + ItemId + "' and item_code='" + Code + "'";
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




    public DataTable Getdata()
    {
        try
        {
            string connectionString = DataManager.OraConnString();
            SqlConnection sqlCon = new SqlConnection(connectionString);
            string query = @"select t1.Id, t1.TransferCode, convert(nvarchar,TransferDate,103) as TransferDate, TotalQuantity, Remarks, AddBy, t2.Projectname as ToProjectid, (select t3.ProjectName from  Project_Setup_Tbl t3 where t1.formProjectId=t3.id) as FromProjectid from ProjectToProjectTransMst t1 inner join Project_Setup_Tbl t2 on t1.ToProjectId=t2.id";
            DataTable dt = DataManager.ExecuteQuery(connectionString, query, "ProjectStockCostMst");
            return dt;
        }
        catch (Exception ex) 
        {
            throw new Exception(ex.Message);
        }
    }






    public DataTable GetDataDtl(string MstId)
    {
        try
        {
            string connectionString = DataManager.OraConnString();
            SqlConnection sqlCon = new SqlConnection(connectionString);
            string query = @"select ItemID as Id,ItemName Name,Code,t1.UOMID as UomId,t2.Name as Uom,Qunitity qnty,Remarks from ProjectToProjectTransdtl t1 inner join UOM t2 on t1.UOMID=t2.ID where t1.MstId='"+MstId+"'";
            DataTable dt = DataManager.ExecuteQuery(connectionString, query, "ProjectToProjectTransDtl");
            return dt;

        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public DataTable GetdataMst(string Id)
    {
        try
        {
            string connectionString = DataManager.OraConnString();
            SqlConnection sqlCon = new SqlConnection(connectionString);
            string query = "select t1.Id, t1.TransferCode, convert(nvarchar,TransferDate,103) as TransferDate, TotalQuantity, Remarks, AddBy, t1.ToProjectID as ToProjectid, t1.FormProjectID as FromProjectid from ProjectToProjectTransMst t1 inner join Project_Setup_Tbl t2 on t1.ToProjectId=t2.id where t1.ID='" + Id + "'";
            DataTable dt = DataManager.ExecuteQuery(connectionString, query, "ProjectToProjectTransMst");
            return dt;

        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
}