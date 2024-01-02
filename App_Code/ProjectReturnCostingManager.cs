using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using autouniv;

/// <summary>
/// Summary description for ProjectReturnCostingManager
/// </summary>
public class ProjectReturnCostingManager
{
	public ProjectReturnCostingManager()
	{
		//
		// TODO: Add constructor logic here
		//
	}
    //

    public static void ProjectReturnCostingSave(DataTable dt, ProjectStockReturn _projectReturn)
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



            command.CommandText = @"insert into ProjectStockReturnCostMst(CostCode,ProjectId,Remarks,AddBy,AddDate,CostingDate,TotalQuntity,sightName)
         values('" + _projectReturn.CostCode + "','" + _projectReturn.ProjectID + "','" + _projectReturn.Remarks + "','" + _projectReturn.Addby + "',CONVERT(date,'" + _projectReturn.costingDate + "',101),CONVERT(date,'" + _projectReturn.costingDate + "',101),'" + _projectReturn.TotalQnitity + "','"+_projectReturn.sight_Name+"')";
            command.ExecuteNonQuery();

            command.CommandText = @"SELECT top(1) [ID]  FROM [ProjectStockReturnCostMst] order by ID desc";
            string DistributcostMstId = command.ExecuteScalar().ToString();

            foreach (DataRow dr in dt.Rows)
            {
                if (dr["ID"].ToString() != "")
                {
                    var CostMstID = dr["ID"].ToString();
                    var ItemId = dr["ItemId"].ToString();
                    var Code = dr["Code"].ToString();
                    var ItemName = dr["Name"].ToString();
                    var Uom = dr["UomId"].ToString();
                    var Qnty = Convert.ToDouble(dr["qnty"].ToString());
                    var txtRemark = dr["Remarks"].ToString();

                    if (Qnty != 0)
                    {
                        command.CommandText = @"insert into projectStockReturncostDtl(MstId,itemName,ItemId,Code,Uom,Quentity,Remarks)
                    values('" + DistributcostMstId + "','" + ItemName + "','" + ItemId + "','" + Code + "','" + Uom + "','" + Qnty + "','" + txtRemark + "')";
                        command.ExecuteNonQuery();

                        command.CommandText = @"update [tbl_ProjectItemStock] set Quntity=Quntity +" + Qnty + "  where ProjectId='" + _projectReturn.ProjectID + "' and ID='" + ItemId + "' and item_code='" + Code + "'";
                        command.ExecuteNonQuery();

                        command.CommandText = @"update [ProjectStockCostDtl] set ReturnQty="+ Qnty +"  where  MstId='" + CostMstID+"' and Code='" + Code + "' and ItemId='" + ItemId + "'";
                        command.ExecuteNonQuery();
                    }
                   

                    //command.CommandText = @"update ItemStock set Quantity=Quantity-'" + dr["this_time_requisition_qnty"].ToString() + "' where ItemID='" + ItemId + "' and MaterialId='" + _distrubationMdl.RequisitionID + "'";
                   
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
            string query = @"select t1.Id,CostCode,convert(nvarchar,CostingDate,103) as CostingDate ,TotalQuntity,t2.ProjectName as Projectid,remarks,AddBy  from ProjectStockReturnCostMst  t1 inner join Project_Setup_Tbl t2 on t1.ProjectId=t2.Id";
            DataTable dt = DataManager.ExecuteQuery(connectionString, query, "ProjectStockReturnCostMst");
            return dt;
           
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
     
        
    }

    public DataTable getDataSerch(string fromDAte, string Todate, string ProjectId)
    {
        string Parametar = "";
        if (!string.IsNullOrEmpty(fromDAte) && !string.IsNullOrEmpty(Todate) && string.IsNullOrEmpty(ProjectId))
        {
            Parametar = " where convert(date, CostingDate,103)   between convert(date,'" + fromDAte +
                        "',103) and convert(date,'" + Todate + "',103)";
        }
        else if (string.IsNullOrEmpty(fromDAte) && string.IsNullOrEmpty(Todate) && !string.IsNullOrEmpty(ProjectId))
        {
            Parametar = " where t1.ProjectId='" + ProjectId + "'";
        }
        else if (!string.IsNullOrEmpty(fromDAte) && !string.IsNullOrEmpty(Todate) && !string.IsNullOrEmpty(ProjectId))
        {
            Parametar = " where convert(date, CostingDate,103)   between convert(date,'" + fromDAte +
                        "',103) and convert(date,'" + Todate + "',103) and t1.ProjectId='" + ProjectId + "'";
        }

        string connectionString = DataManager.OraConnString();
        SqlConnection sqlCon = new SqlConnection(connectionString);
        //string query = @"select t1.Id,CostCode,convert(nvarchar,CostingDate,103) as CostingDate ,TotalQuntity,t2.ProjectName as Projectid,remarks,AddBy  from ProjectStockCostMst  t1 inner join Project_Setup_Tbl t2 on t1.ProjectId=t2.Id " + Parametar + "";
        string query = @"select t1.ID [ID],CostCode,convert(nvarchar,CostingDate,103) as CostingDate,TotalQuntity,t2.ProjectName [Projectid],AddBy,Remarks from ProjectStockReturnCostMst t1 inner join Project_Setup_Tbl t2 on t1.ProjectId=t2.Id " + Parametar + "";
        DataTable dt = DataManager.ExecuteQuery(connectionString, query, "ProjectStockCostMst");
        return dt;
    }

    
    // project Return master Table 

    public  DataTable GetMstID(string id)
    {
        try 
        { 
            string connectionString=DataManager.OraConnString();
            SqlConnection sqlcon = new SqlConnection(connectionString);
            string query = @"select t1.Id,CostCode,convert(nvarchar,CostingDate,103) as CostingDate,TotalQuntity,t1.Projectid,remarks,AddBy,t2.Address  from ProjectStockReturnCostMst  t1 inner join Project_Setup_Tbl t2 on t1.ProjectId=t2.Id where t1.Id='"+id+"'";
            DataTable dt = DataManager.ExecuteQuery(connectionString,query,"ProjectStockReturnCostMst");
            return dt;

        }
        catch(Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }


    // projectReturnDtl Table

    public DataTable GetDtlId(string id) 
    {
        try 
        {
            string connectionString = DataManager.OraConnString();
            SqlConnection sqlcon = new SqlConnection(connectionString);
            string query = @"select t1.Id,t1.ItemId,t1.itemName as Name,t1.Code,t3.Name as Uom,t3.Id as UomId,t1.Quentity as costqnty,ReturnQty as qnty,t1.MstId  as MstId, t1.Remarks as Remarks from projectStockReturncostDtl t1 inner join ProjectStockReturnCostMst t2 on t1.MstId=t2.Id inner join UOM t3 on t1.Uom=t3.id  where t1.MstId='" + id + "'";
            DataTable dt = DataManager.ExecuteQuery(connectionString, query, "projectStockReturncostDtl");
            return dt;

        }
        catch (Exception ex) 
        {
            throw new Exception(ex.Message);
        }
    }



    public  DataTable GetreturnItemDetlise(string id) 
    {
        try 
        {
            string connectionstring = DataManager.OraConnString();
            SqlConnection sqlcon = new SqlConnection(connectionstring);
            string query = @"select t1.Id [ID],t1.itemName as Name,t1.Code [Code] ,t3.Name as Uom,t3.Id as UomId,t1.Quentity [Quentity] ,t1.MstId  as MstId,(select ProjectName from Project_Setup_Tbl where Id=t2.ProjectId) [projectID], t1.Remarks as Remarks from projectStockReturncostDtl t1 inner join ProjectStockReturnCostMst t2 on t1.MstId=t2.Id inner join UOM t3 on t1.Uom=t3.id  where t1.MstId='"+id+"'";
            DataTable dt = DataManager.ExecuteQuery(connectionstring, query, "projectStockReturncostDtl");
            return dt;
        }
        catch(Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }


    public static void ProjectcostingUpdate(DataTable dt, ProjectStockReturn _projectReturn, string id)
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

            command.CommandText = @"Update ProjectStockReturnCostMst set CostCode='" + _projectReturn.CostCode + "',ProjectId='" + _projectReturn.ProjectID + "',Remarks='" + _projectReturn.Remarks + "' ,AddBy ='" + _projectReturn.Addby + "',CostingDate='" + _projectReturn.costingDate + "',TotalQuntity='" + _projectReturn.TotalQnitity + "',sightName='" + _projectReturn.sight_Name+ "' where Id='" + id + "'";
            command.ExecuteNonQuery();

            command.CommandText = @"DELETE FROM projectStockReturncostDtl WHERE MstId='" + id + "'";
            command.ExecuteNonQuery();

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
                    //var PresentQty = dr["present_Stock"].ToString();
                    //var ThistimeQty =(dr["this_time_requisition_qnty"].ToString());
                    //var TotalStock = dr["total_Stock"].ToString();
                    //var Remarks = dr["Remarksany"].ToString();
                    command.CommandText = @"insert into projectStockReturncostDtl(MstId,itemName,ItemId,Code,Uom,Quentity,Remarks)
                    values('" + id + "','" + ItemName + "','" + ItemId + "','" + Code + "','" + Uom + "','" + txtQnty + "','" + txtRemark + "')";
                    command.ExecuteNonQuery();

                    //command.CommandText = @"update ItemStock set Quantity=Quantity-'" + dr["this_time_requisition_qnty"].ToString() + "' where ItemID='" + ItemId + "' and MaterialId='" + _distrubationMdl.RequisitionID + "'";
                    command.CommandText = @"update [tbl_ProjectItemStock] set Quntity=Quntity-" + txtQnty + "  where ProjectId='" + _projectReturn.ProjectID + "' and ID='" + ItemId + "' and item_code='" + Code + "'";
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