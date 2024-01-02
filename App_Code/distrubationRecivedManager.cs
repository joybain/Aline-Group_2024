using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using autouniv;
using Microsoft.ReportingServices.Interfaces;
using OldColor;
/// <summary>
/// Summary description for distrubationRecivedManager
/// </summary>
public class distrubationRecivedManager
{
	public distrubationRecivedManager()
	{
		//
		// TODO: Add constructor logic here
		//
	}

    public static DataTable MasterRecivedData(string Id)
    {
        string connectionString = DataManager.OraConnString();
        SqlConnection sqlCon = new SqlConnection(connectionString);
        //string query = @"select t1.ItemId as ID,t1.Code as item_code,t3.Name as item_desc,t1.UomId as msr_unit_code,t1.TransferQuantity as present_qnty,'' as  Remarks,'1' AS [check],(select (CONVERT(int,this_time_requisition)-t1.TransferQuantity) from MaterialRequisitionDtl tn inner join MaterialRequisitionMst tn2 on tn.mstId=tn2.Id where item_Code=t1.Code and tn2.ProjectId=t1.ToProjectId) Due_qnty from ProjectStockTransferDtl t1 inner join ProjectStockTransferMst t2 on t1.MstId=t2.id inner join Item t3 on t1.ItemId=t3.ID ";
        string query = @"select t1.ItemId as ID,t1.Code as item_code,t3.Name as item_desc,t1.UomId as msr_unit_code,t1.TransferQuantity as present_qnty,'' as  Remarks,'1' AS [check],(select Isnull(sum(requisition_qnty)-sum(distribution_qty),0) as Due_qnty from ItemDistributionDtl t1 inner join ItemDistributionMst t2 on t1.MstId=t2.ID where t2.RequisitionId='" + Id + "') Due_qnty from ProjectStockTransferDtl t1 inner join ProjectStockTransferMst t2 on t1.MstId=t2.id inner join Item t3 on t1.ItemId=t3.ID where MstId='" + Id + "'";
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

            if (_distrubationMdl.TranseferCode.Substring(1, 3) == "ToP")
            {
                command.CommandText = @"Update [ProjectToProjectTransMst] set RecivedDate=convert(date,'" + _distrubationMdl.ReciveDate + "',103),RecivedBy='" + _distrubationMdl.LoginBy + "',ReciveStatus='1' where TransferCode='" + _distrubationMdl.TranseferCode + "'";
                command.ExecuteNonQuery();
            }
            else
            {

                command.CommandText = @"Update [ItemDistributionMst] set RecivedDate=convert(date,'" + _distrubationMdl.ReciveDate + "',103),RecivedBy='" + _distrubationMdl.LoginBy + "',LocalUplode='1' where Code='" + _distrubationMdl.TranseferCode + "'";
                command.ExecuteNonQuery();
            }


            command.CommandText = @"Update [ProjectStockTransferMst] set ReceivedDate=convert(date,'" + _distrubationMdl.ReciveDate + "',103),Reciveby='" + _distrubationMdl.LoginBy + "',Status='1' where Code='" + _distrubationMdl.TranseferCode + "'";
            command.ExecuteNonQuery();
           

            foreach (DataRow dr in dt.Rows)
            {
                if (dr["ID"].ToString() != "" && dr["check"].ToString() =="1")
                {
                    var ItemId = dr["ID"].ToString();
                    var Code = dr["item_code"].ToString();
                    var ItemName = dr["item_desc"].ToString();
                    var Uom = dr["msr_unit_code"].ToString();
                    var Qty = dr["present_qnty"].ToString();
                    var Remarks = dr["Remarks"].ToString();

                    command.CommandText = @" select count(Id) id from tbl_ProjectItemStock where item_code='"+Code+"' and ProjectId='"+_distrubationMdl.ProjectId+"' and ItemID='"+ItemId+"'";
                    string Number = command.ExecuteScalar().ToString();
                    int Count = Convert.ToInt32(Number);

                    if (Count>0)
                    {
                        command.CommandText = @"update tbl_ProjectItemStock set Quntity=Quntity+'" + dr["present_qnty"].ToString() + "' where item_code='" + Code + "' and ProjectId='" + _distrubationMdl.ProjectId + "' and ItemID='" + ItemId + "'";
                        command.ExecuteNonQuery();
                    }
                    else
                    {
                        command.CommandText = @"insert into tbl_ProjectItemStock (item_code,ItemID,msr_unit_code,Quntity,ProjectId,Remarks)
values('" + Code + "','" + ItemId + "','" + Uom + "','" + Qty + "','" + _distrubationMdl.ProjectId + "','" + Remarks + "')";
                        command.ExecuteNonQuery();
                    }

                   
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

    public DataTable getProjectStock()
    {
        string connectionString = DataManager.OraConnString();
        SqlConnection sqlCon = new SqlConnection(connectionString);
        string query =
            @"select t1.Id,t1.item_code as Code,t2.Name as items_Desc,t1.Quntity as Qnty, t3.Name as UOM,t4.ProjectName as  ProjectName from tbl_ProjectItemStock t1 inner join Item t2 on t1.ItemID=t2.ID inner join UOM t3 on t1.msr_unit_code=t3.id inner join Project_Setup_Tbl t4 on t1.ProjectId=t4.Id where t1.Quntity > 0";
        DataTable dt = DataManager.ExecuteQuery(connectionString, query, "tbl_ProjectItemStock");
        return dt;
    }

    public DataTable AllredyRecived(string MstId)
    {
        string connectionString = DataManager.OraConnString();
        SqlConnection sqlCon = new SqlConnection(connectionString);
        string query = @"select * from ProjectStockTransferMst where id='"+MstId+"' and Status=1 and ReciveBy  is not null";
            
        DataTable dt = DataManager.ExecuteQuery(connectionString, query, "tbl_ProjectItemStock");
        return dt;
    }

    public DataTable getProjectStockSerch(string ProjectId)
    {
        string connectionString = DataManager.OraConnString();
        SqlConnection sqlCon = new SqlConnection(connectionString);
        string query = @"select t1.Id,t1.item_code as Code,t2.Name as items_Desc,t1.Quntity as Qnty, t3.Name as UOM,t4.ProjectName as  ProjectName from tbl_ProjectItemStock t1 inner join Item t2 on t1.ItemID=t2.ID inner join UOM t3 on t1.msr_unit_code=t3.id inner join Project_Setup_Tbl t4 on t1.ProjectId=t4.Id where t1.Quntity > 0 and t1.ProjectId='" + ProjectId + "'";

        DataTable dt = DataManager.ExecuteQuery(connectionString, query, "tbl_ProjectItemStock");
        return dt;
    }

    public DataTable TranseferCode(string MstId)
    {
        string connectionString = DataManager.OraConnString();
        SqlConnection sqlCon = new SqlConnection(connectionString);
        string query = @"select * from ProjectStockTransferMst where id='" + MstId + "'";

        DataTable dt = DataManager.ExecuteQuery(connectionString, query, "tbl_ProjectItemStock");
        return dt;
    }

    public static DataTable MasterRecivedvalu(string Id)
    {
        string connectionString = DataManager.OraConnString();
        SqlConnection sqlCon = new SqlConnection(connectionString);
        string query = @"select tn.PO,tn.Requisition_Code,isnull((tn.TotalRequisition-tn.TransferQuantity),0) as DueQnty,tn.TotalRequisition,RequisitionId from (select t4.PO,t3.Requisition_Code,(select sum(TransferQuantity) from ProjectStockTransferDtl where MstId In (t2.MstId)) as TransferQuantity,(select sum((CONVERT(int,this_time_requisition))) from MaterialRequisitionDtl where MstId in (t3.RequisitionId )) TotalRequisition,(select sum(AutherizQty) from MaterialRequisitionDtl where MstId in (t3.RequisitionId )) AutherizQty,t3.RequisitionId from ItemDistributionDtl t1 
inner join ProjectStockTransferDtl t2 on t1.MstId=t2.DtlID 
inner join ItemDistributionMst t3 on t1.mstid=t3.id 
inner join ItemPurOrderMst t4 on t3.RequisitionId=t4.RequisitionId where t2.MstId='"+Id+"' group by t4.PO,t3.Requisition_Code,t3.RequisitionId,t2.MstId) tn";
        DataTable dt = DataManager.ExecuteQuery(connectionString, query, "Item");
        return dt;
    }
}