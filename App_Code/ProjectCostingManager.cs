using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using autouniv;
using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Wordprocessing;

/// <summary>
/// Summary description for ProjectCostingManager
/// </summary>
public class ProjectCostingManager
{
	public ProjectCostingManager()
	{
		//
		// TODO: Add constructor logic here
		//
	}

    public static DataTable GetItemsForProjectcosting(string ItemCode , string ProjectId)
    {
        string connectionString = DataManager.OraConnString();
        SqlConnection sqlCon = new SqlConnection(connectionString);
        string query = @"select t1.Id,t2.Name,item_code as Code,t1.msr_unit_code as UomId,t3.Name as Uom,Quntity as qnty from tbl_ProjectItemStock t1  inner join Item t2 on t1.ItemID=t2.ID inner join uom t3 on t1.msr_unit_code=t3.id where t1.item_code='" + ItemCode + "' and ProjectId='" + ProjectId + "' and Quntity>0";

      
        DataTable dt = DataManager.ExecuteQuery(connectionString, query, "tbl_ProjectItemStock");
       return dt;
    }





    public static void ProjectcostingSave(DataTable dt, ProjectStockCostmodel _projectCostModel)
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

            command.CommandText = @"insert into ProjectStockCostMst(CostCode,ProjectId,Remarks,AddDate,AddBy,TotalQuntity,CostingDate,Sight_name)
values('" + _projectCostModel.Code + "','" + _projectCostModel.ProjectId + "','" + _projectCostModel.Remarks + "',convert(date,'" + _projectCostModel.CostingDate + "',103),'" + _projectCostModel.LoginBy + "','" + _projectCostModel.TotalQuantity + "',convert(date,'" + _projectCostModel.CostingDate + "',103),'"+_projectCostModel.SightName+"')";
            command.ExecuteNonQuery();

            command.CommandText = @"SELECT top(1) [ID]  FROM [ProjectStockCostMst] order by ID desc";
            string DistributcostMstId = command.ExecuteScalar().ToString();

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
                    var Floor = dr["floor"].ToString();
                    var Stage = dr["stage"].ToString();

                    //var PresentQty = dr["present_Stock"].ToString();
                    //var ThistimeQty =(dr["this_time_requisition_qnty"].ToString());
                    //var TotalStock = dr["total_Stock"].ToString();
                    //var Remarks = dr["Remarksany"].ToString();
                    command.CommandText = @"insert into ProjectStockCostDtl(MstId,itemName,ItemId,Code,Uom,Quentity,Remarks,stage,floor)
                    values('" + DistributcostMstId + "','" + ItemName + "','" + ItemId + "','" + Code + "','" + Uom + "','" + txtQnty + "','" + txtRemark + "','" + Stage + "','" + Floor + "')";
                    command.ExecuteNonQuery();

          //command.CommandText = @"update ItemStock set Quantity=Quantity-'" + dr["this_time_requisition_qnty"].ToString() + "' where ItemID='" + ItemId + "' and MaterialId='" + _distrubationMdl.RequisitionID + "'";
                    command.CommandText = @"update [tbl_ProjectItemStock] set Quntity=Quntity-" + txtQnty + "  where ProjectId='" + _projectCostModel.ProjectId + "' and ID='" + ItemId + "' and item_code='" + Code + "'";
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
        string connectionString = DataManager.OraConnString();
        SqlConnection sqlCon = new SqlConnection(connectionString);
        string query = @"select t1.Id,CostCode,convert(nvarchar,CostingDate,103) as CostingDate ,TotalQuntity,t2.ProjectName as Projectid,remarks,AddBy  from ProjectStockCostMst  t1 inner join Project_Setup_Tbl t2 on t1.ProjectId=t2.Id";
        DataTable dt = DataManager.ExecuteQuery(connectionString, query, "ProjectStockCostMst");
        return dt;
    }
    public DataTable Getdataserch(string fromDAte, string Todate,string ProjectId)
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
        string query = @"select t1.Id,CostCode,convert(nvarchar,CostingDate,103) as CostingDate ,TotalQuntity,t2.ProjectName as Projectid,remarks,AddBy  from ProjectStockCostMst  t1 inner join Project_Setup_Tbl t2 on t1.ProjectId=t2.Id "+Parametar+"";
        DataTable dt = DataManager.ExecuteQuery(connectionString, query, "ProjectStockCostMst");
        return dt;
    }

    public DataTable GetdataMst(string Id)
    {
        string connectionString = DataManager.OraConnString();
        SqlConnection sqlCon = new SqlConnection(connectionString);
        string query = @"select t1.Id,CostCode,convert(nvarchar,CostingDate,103) as CostingDate,(select (Isnull(sum(t2.Quentity),0)-Isnull(sum(t2.ReturnQty),0)) as TotalQuntity from ProjectStockCostMst t1 inner join ProjectStockCostDtl t2  on t1.id=t2.MstId where t1.Id='" + Id + "') as TotalQuntity,t1.Projectid,remarks,AddBy,t2.Address  from ProjectStockCostMst  t1 inner join Project_Setup_Tbl t2 on t1.ProjectId=t2.Id where t1.Id='" + Id + "'";
        DataTable dt = DataManager.ExecuteQuery(connectionString, query, "ProjectStockCostMst");
        return dt;
    }

    public DataTable GetDataDtl(string MstId)
    {
        string connectionString = DataManager.OraConnString();
        SqlConnection sqlCon = new SqlConnection(connectionString);
        string query = @"
select t1.Id,t1.itemName as Name,t1.Code,t3.Name as Uom,t3.Id as UomId,(Isnull(sum(t1.Quentity),0)-Isnull(sum(t1.ReturnQty),0))  as qnty,t1.Remarks as Remarks,t1.[floor],t1.stage  from ProjectStockCostDtl t1 inner join ProjectStockCostMst t2 on t1.MstId=t2.Id inner join uom t3 on t1.Uom=t3.id  where t1.MstId='"+MstId+"' Group by t1.Id,t1.itemName,t1.Code,t3.Name,t3.Id,t1.Remarks,t1.[floor],t1.stage";
        DataTable dt = DataManager.ExecuteQuery(connectionString, query, "ProjectStockCostMst");
        return dt;
    }


    public DataTable GetCostDtlRept(string MstId) 
    {
        try
        {
            string connectionstring = DataManager.OraConnString();
            SqlConnection sqlcon = new SqlConnection(connectionstring);
            string query = @"select t1.Id [ID],t1.itemName as Name,t1.Code [Code] ,t3.Name as Uom,t3.Id as UomId,t1.Quentity [Quentity] ,t1.MstId  as MstId,(select ProjectName from Project_Setup_Tbl where Id=t2.ProjectId) [projectID], t1.Remarks as Remarks from ProjectStockCostDtl t1 inner join ProjectStockCostMst t2 on t1.MstId=t2.Id inner join UOM t3 on t1.Uom=t3.id  where t1.MstId='"+MstId+"'";
            DataTable dt = DataManager.ExecuteQuery(connectionstring, query, "ProjectStockCostDtl");
            return dt;
        }
        catch (Exception ex) 
        {
            throw new Exception(ex.Message);
        }
    }


    // project Costing Update 

    public static void ProjectcostingUpdate(DataTable dt, ProjectStockCostmodel _projectCostModel, string id)
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

            command.CommandText = @"Update ProjectStockCostMst set CostCode='" + _projectCostModel.Code + "',ProjectId='" + _projectCostModel.ProjectId + "',Remarks='" + _projectCostModel.Remarks + "' ,AddBy ='" + _projectCostModel.LoginBy + "',CostingDate='" + _projectCostModel.CostingDate + "',TotalQuntity='" + _projectCostModel.TotalQuantity + "' where Id='" + id + "'";
            command.ExecuteNonQuery();

            command.CommandText = @"DELETE FROM ProjectStockCostDtl WHERE MstId='"+id+"'";
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
                    command.CommandText = @"insert into ProjectStockCostDtl(MstId,itemName,ItemId,Code,Uom,Quentity,Remarks)
                    values('" + id + "','" + ItemName + "','" + ItemId + "','" + Code + "','" + Uom + "','" + txtQnty + "','" + txtRemark + "')";
                    command.ExecuteNonQuery();

                    //command.CommandText = @"update ItemStock set Quantity=Quantity-'" + dr["this_time_requisition_qnty"].ToString() + "' where ItemID='" + ItemId + "' and MaterialId='" + _distrubationMdl.RequisitionID + "'";
                    command.CommandText = @"update [tbl_ProjectItemStock] set Quntity=Quntity-" + txtQnty + "  where ProjectId='" + _projectCostModel.ProjectId + "' and ID='" + ItemId + "' and item_code='" + Code + "'";
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




    public static DataTable GetItemsForProjectReturn(string ItemCode, string ProjectId)
    {
        string connectionString = DataManager.OraConnString();
        SqlConnection sqlCon = new SqlConnection(connectionString);
        string query = @"Select t1.Id,t2.ItemId as ItemId,t2.itemName as Name,t2.Code as Code,t2.Uom as UomId,t3.Name as Uom,isnull(t2.Quentity,0) as Costqnty,t2.Remarks,'0' as qnty from ProjectStockCostMst t1 inner join ProjectStockCostDtl t2 on t1.Id=t2.MstId inner join UOM t3 on t2.Uom=t3.ID  where t1.CostCode='" + ItemCode + "' and ProjectId='" + ProjectId + "'";


        DataTable dt = DataManager.ExecuteQuery(connectionString, query, "tbl_ProjectItemStock");
        return dt;
       
    }
}