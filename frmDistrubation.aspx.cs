using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using autouniv;
using OldColor;
using System.Data.SqlClient;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.draw;

public partial class frmDistrubation : System.Web.UI.Page
{
    private static Permis per;
    DistrubationModel _distrubationMdl=new DistrubationModel();
    distrubationManager _distrubationManager = new distrubationManager();
    private static DataTable dtmsr = new DataTable();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["user"] == null)
        {
            if (Session.SessionID != "" | Session.SessionID != null)
            {
                clsSession ses = clsSessionManager.getSession(Session.SessionID);
                if (ses != null)
                {
                    Session["user"] = ses.UserId;
                    Session["book"] = "AMB";
                    string connectionString = DataManager.OraConnString();
                    SqlDataReader dReader;
                    SqlConnection conn = new SqlConnection();
                    conn.ConnectionString = connectionString;
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = conn;
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "Select ID,user_grp,description from utl_userinfo where upper(user_name)=upper('" + Session["user"].ToString().ToUpper() + "') and status='A'";
                    conn.Open();
                    dReader = cmd.ExecuteReader();
                    string wnot = "";
                    if (dReader.HasRows == true)
                    {
                        while (dReader.Read())
                        {
                            Session["userlevel"] = int.Parse(dReader["user_grp"].ToString());
                            //Session["dept"] = dReader["dept"].ToString();
                            wnot = dReader["description"].ToString();
                            Session["userID"] = dReader["ID"].ToString();
                        }
                        Session["wnote"] = wnot;

                        cmd = new SqlCommand();
                        cmd.Connection = conn;
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "Select book_desc,company_address1,company_address2,separator_type,ShortName from gl_set_of_books where book_name='" + Session["book"].ToString() + "' ";
                        if (dReader.IsClosed == false)
                        {
                            dReader.Close();
                        }
                        dReader = cmd.ExecuteReader();
                        if (dReader.HasRows == true)
                        {
                            while (dReader.Read())
                            {
                                Session["septype"] = dReader["separator_type"].ToString();
                                Session["org"] = dReader["book_desc"].ToString();
                                Session["add1"] = dReader["company_address1"].ToString();
                                Session["add2"] = dReader["company_address2"].ToString();
                                Session["ShotName"] = dReader["ShortName"].ToString();
                            }
                        }
                    }
                    dReader.Close();
                    conn.Close();
                }
            }
        }
        try
        {
            //ClientScript.RegisterStartupScript(this.GetType(), "ale", "alert('" + Session.SessionID + "');", true);
            string pageName = DataManager.GetCurrentPageName();
            string modid = PermisManager.getModuleId(pageName);
            per = PermisManager.getUsrPermis(Session["user"].ToString().Trim().ToUpper(), modid);
            if (per != null & per.AllowView == "Y")
            {
                ((Label)Page.Master.FindControl("lblLogin")).Text = Session["wnote"].ToString();
                ((LinkButton)Page.Master.FindControl("lbLogout")).Visible = true;
            }
            else
            {
                Response.Redirect("Home.aspx?sid=sam");
            }
        }
        catch
        {
            Response.Redirect("Default.aspx?sid=sam");
        }

         if (!IsPostBack)
         {
             Refresh();
             btnSave.Visible = false;

         }
    }

private void Refresh()
{
    ddlProjectName.DataSource = MeterialRequistitonManager.GetProject();
    ddlProjectName.DataTextField = "ProjectName";
    ddlProjectName.DataValueField = "ID";
    ddlProjectName.DataBind();
    ddlProjectName.Items.Insert(0, "");

        ddlTransferProject.DataSource = MeterialRequistitonManager.GetProject();
        ddlTransferProject.DataTextField = "ProjectName";
        ddlTransferProject.DataValueField = "ID";
        ddlTransferProject.DataBind();
        ddlTransferProject.Items.Insert(0, "");

        txtRequisitionCode.Text = "";
    txtItemSearch.Text = "";
    lblReqNo.Text = "";
    txtRemark.Text = "";
    txtRequisitionCode.Enabled = false;
   // txtRequisitionNo.Enabled = false;
    txtChallanNo.Enabled = false;
    checkbox.Checked = false;
    ddlProjectName.Enabled = false;
    dgDistribution.DataSource = null;
    dgDistribution.DataBind();
    dgDistribution.Enabled = txtRequisitionNo.Enabled = true;
    Session["purdtl"] = null;
    dgDistribution.Visible = false;
    var Data = _distrubationManager.GateData();
    dgRequistionHistory.DataSource = Data;
    dgRequistionHistory.DataBind();
    txtSerchItem.Text = "";
    //lblQuantity.Text = "0";
    //dgRequistionHistory.EmptyDataTemplate;
}

 
    private void GetRequisition()
    {
        dgDistribution.Visible = true;
        DataTable dtDtlGrid = new DataTable();
        dtDtlGrid.Columns.Add("ID", typeof(string));
        dtDtlGrid.Columns.Add("item_desc", typeof(string));
        //dtDtlGrid.Columns.Add("ItemId", typeof(string));
        dtDtlGrid.Columns.Add("item_code", typeof(string));
        dtDtlGrid.Columns.Add("msr_unit_code", typeof(string));
        dtDtlGrid.Columns.Add("this_time_requisition_qnty", typeof(string));
        dtDtlGrid.Columns.Add("requisition_qnty", typeof(string));
        dtDtlGrid.Columns.Add("duerequisition_qnty", typeof(string));
        dtDtlGrid.Columns.Add("present_Stock", typeof(string));
        dtDtlGrid.Columns.Add("total_Stock", typeof(string));      
        dtDtlGrid.Columns.Add("Remarksany", typeof(string));

        DataRow dr = dtDtlGrid.NewRow();
        dtDtlGrid.Rows.Add(dr);
        dgDistribution.DataSource = dtDtlGrid;
        Session["purdtl"] = dtDtlGrid;
        dgDistribution.DataBind();
    }

    protected void btnNew_Click(object sender, EventArgs e)
    {
        txtRequisitionCode.Text = IdManager.GetDateTimeWiseSerial("TR-", "ID", "[ItemDistributionMst]");
        txtTfDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
        txtRequisitionCode.Enabled = true;
        //txtRequisitionNo.Enabled = true;
        if (checkbox.Text == "Fixed Assest Or Head Office Product")
        {
            lblSerchitem.Visible = txtSerchItem.Visible = true;      
        }
        else
        {
            txtRequisitionNo.Visible = lblSerchRequisitionNo.Visible = true;
            ddlTransferProject.Visible =Label1.Visible=Label2.Visible= false;
        }
        txtChallanNo.Enabled = true;
        ddlProjectName.Enabled = true;
        ddlTransferProject.Enabled = true;
        txtRequisitionNo.Enabled = true;
        btnSave.Visible = true;
        btnNew.Visible = false;
        dgRequistionHistory.Visible = false;
        GetRequisition();
    }
    protected void ddlProjectName_SelectedIndexChanged(object sender, EventArgs e)
    {
        var ProjectId = ddlProjectName.SelectedValue;
        Session["projectid"] = ProjectId;
        if (ProjectId == null)
        {
            txtRequisitionNo.Enabled = false;
        }
        else
        {
            txtRequisitionNo.Enabled = true;
        }
        txtItemSearch.Enabled = true;
    }
   
    protected void txtRequisitionNo_TextChanged1(object sender, EventArgs e)
    {

        //if (string.IsNullOrEmpty(ddlProjectName.SelectedValue))
        //{
        //    ClientScript.RegisterStartupScript(this.GetType(), "ale", "alert('Please At First Select Project!!');", true);
        //    return;
        //}

        DataTable dt = PurchaseOrderManager.PurchaseRequisition(txtRequisitionNo.Text, ddlProjectName.SelectedValue);
         if (dt.Rows.Count > 0)
        {
            lblReqNo.Text = dt.Rows[0]["ID"].ToString();
            Session["MatarialId"] = lblReqNo.Text;
            int OrderMstId = IdManager.GetLastMstId("Id", "ItemDistributionMst", lblReqNo.Text);
            if (OrderMstId == 0)
            {
                DataTable dt1 = distrubationManager.GetRqValu(lblReqNo.Text, ddlProjectName.SelectedValue);
                DataRow drd = dt1.NewRow();
                dt1.Rows.Add(drd);
                dgDistribution.DataSource = dt1;
                //ViewState["purdtl"] = dt1;
                Session["purdtl"] = dt1;
                dgDistribution.DataBind();
                PVI_UP.Update();

            }
            else
            {
                DataTable dt2 = distrubationManager.GetRqValu1(lblReqNo.Text, ddlProjectName.SelectedValue);
                DataRow drd = dt2.NewRow();
                dt2.Rows.Add(drd);
                dgDistribution.DataSource = dt2;
                Session["purdtl"] = dt2;
                dgDistribution.DataBind();
                PVI_UP.Update();       
            }

        }
        else
        {
            GetRequisition();
            //ShowFooterTotal();
            PVI_UP.Update();
            //PVIesms_UP.Update();
            txtRequisitionCode.Text = lblReqNo.Text = "";
            ClientScript.RegisterStartupScript(this.GetType(), "ale", "alert('This Requisition No is Not Found!!');", true);
        }
      
    }

    protected void txtItemName_TextChanged(object sender, EventArgs e)
    {
        try
        {
            GridViewRow gvr = (GridViewRow)((TextBox)sender).NamingContainer;
            DataTable dtdtl = (DataTable)Session["purdtl"];
            DataRow dr = dtdtl.Rows[gvr.DataItemIndex];
            DataTable dt = distrubationManager.GetMaterialItem(((TextBox)gvr.FindControl("txtItemName")).Text);
            if (dt.Rows.Count > 0)
            {

                string flag = "";
                foreach (DataRow Dr2 in dtdtl.Rows)
                {
                    if (Dr2["ID"].ToString() == dt.Rows[0]["ID"].ToString())
                    {


                        flag = "Y";
                        ((TextBox)dgDistribution.Rows[gvr.DataItemIndex].FindControl("txtItemName")).Focus();
                        //ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "alert('Already This Item Add...!!');", true);

                    }


                }
                if (flag != "Y")
                {
                    dtdtl.Rows.Remove(dr);
                    dr = dtdtl.NewRow();
                    dr["ID"] = ((DataRow)dt.Rows[0])["ID"].ToString();
                    dr["item_desc"] = ((DataRow)dt.Rows[0])["item_desc"].ToString();
                    dr["item_code"] = ((DataRow)dt.Rows[0])["item_code"].ToString();
                    dr["msr_unit_code"] = ((DataRow)dt.Rows[0])["msr_unit_code"].ToString();
                    dr["requisition_qnty"] = "0";
                    dr["duerequisition_qnty"] = ((DataRow)dt.Rows[0])["duerequisition_qnty"].ToString();
                    dr["present_Stock"] = ((DataRow)dt.Rows[0])["present_Stock"].ToString();
                    dr["total_Stock"] = ((DataRow)dt.Rows[0])["total_Stock"].ToString();
                    //dr["PSRequirement"] = ((DataRow)dt.Rows[0])["PSRequirement"].ToString();
                    dr["Remarksany"] = ((DataRow)dt.Rows[0])["Remarksany"].ToString();
                    dtdtl.Rows.InsertAt(dr, gvr.DataItemIndex);
                }
                dgDistribution.DataSource = dtdtl;
                dgDistribution.DataBind();
                //ShowFooterTotal();
                ((TextBox)dgDistribution.Rows[gvr.DataItemIndex-1].FindControl("txtPresentQty")).Focus();
                MRIesms_UP.Update();
            }
        }

catch (Exception ex)
        {
            ExceptionLogging.SendExcepToDB(ex);
        }
    }
    protected void dgDistribution_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (((DataRowView)e.Row.DataItem)["this_time_requisition_qnty"].ToString() != "")
                {
                    
                   // decimal total = decimal.Parse(((DataRowView)e.Row.DataItem)["present_Stock"].ToString()) +
                                  // decimal.Parse(((DataRowView)e.Row.DataItem)["this_time_requisition_qnty"].ToString());
                    decimal total = decimal.Parse(((DataRowView)e.Row.DataItem)["this_time_requisition_qnty"].ToString());
                    decimal TotalStock = decimal.Parse(((DataRowView)e.Row.DataItem)["present_Stock"].ToString()) -
                                  decimal.Parse(((DataRowView)e.Row.DataItem)["this_time_requisition_qnty"].ToString());

                    //decimal deuqnty = decimal.Parse(((DataRowView)e.Row.DataItem)["requisition_qnty"].ToString()) -
                    //            decimal.Parse(((DataRowView)e.Row.DataItem)["this_time_requisition_qnty"].ToString());
                    
                    ((TextBox)e.Row.FindControl("txtTotalQnty")).Text = total.ToString("N2");
                    ((TextBox)e.Row.FindControl("txtStockQuantity")).Text = TotalStock.ToString("N2");
                    //if (deuqnty > 0)
                    //{
                    //    ((TextBox)e.Row.FindControl("txtDueRequisitionQnty")).Text = deuqnty.ToString("N2");
                    //}
                    //else
                    //{
                    //    ((TextBox)e.Row.FindControl("txtDueRequisitionQnty")).Text ="0";
                    //}
                   
                    //decimal psr = decimal.Parse(((DataRowView)e.Row.DataItem)["Previous"].ToString()) +
                    //               decimal.Parse(((DataRowView)e.Row.DataItem)["This_time_Requisition"].ToString()) - (decimal.Parse(((DataRowView)e.Row.DataItem)["SupplidQnt"].ToString()) + decimal.Parse(((DataRowView)e.Row.DataItem)["PresentStock"].ToString()));

                    //((Label)e.Row.FindControl("lblPSRequirement")).Text = psr.ToString("N2");
                }
                //e.Row.Cells[1].Attributes.Add("style", "display:none");
                e.Row.Cells[8].Attributes.Add("style", "display:none");
                e.Row.Cells[10].Attributes.Add("style", "display:none");
            }
            else if (e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.Cells[8].Attributes.Add("style", "display:none");
                e.Row.Cells[10].Attributes.Add("style", "display:none");
            }
            else if (e.Row.RowType == DataControlRowType.Footer)
            {
                e.Row.Cells[8].Attributes.Add("style", "display:none");
                e.Row.Cells[10].Attributes.Add("style", "display:none");
            }
        }
        catch (FormatException fex)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "Warning", "alert('" + fex.Message + "');", true);
        }
        catch (Exception ex)
        {
            if (ex.Message.Contains("Database"))
                ClientScript.RegisterStartupScript(this.GetType(), "Warning", "alert('Database Maintain Error. Contact to the Software Provider..!!');", true);
            else
                ClientScript.RegisterStartupScript(this.GetType(), "Warning", "alert('There is some problem to do the task. Try again properly.!!');", true);
        }
    }
    //private void ShowFooterTotal()
    //{
    //    try
    //    {
    //        decimal ctot = decimal.Zero;
    //        decimal totAddi = 0;
    //        decimal totalStock = 0;
    //        decimal TotalRequrment = 0;
    //        decimal totItemsP = 0;
    //        decimal totQty = 0;
    //        decimal Total = 0;

    //        if (Session["purdtl"] != null)
    //        {
    //            DataTable dt = (DataTable)Session["purdtl"];
    //            foreach (DataRow drp in dt.Rows)
    //            {
    //                if (drp["ItemsID"].ToString() != "" && drp["Price"].ToString() != "")
    //                {
    //                    totalStock += decimal.Parse(drp["total_Stock"].ToString());
    //                    TotalRequrment += decimal.Parse(drp["requisition_qnty"].ToString());
    //                    //totQty += decimal.Parse(drp["TransferQty"].ToString());
    //                    // totItemsP += decimal.Parse(drp["item_rate"].ToString()) * decimal.Parse(drp["qnty"].ToString());
    //                    // totA += decimal.Parse(drp["ExpireDate"].ToString());

    //                    //totAddi += (totItemsP * decimal.Parse(drp["Additional"].ToString())) / 100;
    //                    // Total += decimal.Parse(drp["item_rate"].ToString()) * decimal.Parse(drp["qnty"].ToString()); ;
    //                }
    //            }
               
    //        }
    //        GridViewRow row = new GridViewRow(0, 0, DataControlRowType.Footer, DataControlRowState.Normal);
    //        TableCell cell;
    //        cell = new TableCell();
    //        cell.Text = "Total";
    //        cell.ColumnSpan = 5;
    //        cell.HorizontalAlign = HorizontalAlign.Right;
    //        row.Cells.Add(cell);
    //        //cell = new TableCell();
    //        //cell.Text = "";
    //        //cell.HorizontalAlign = HorizontalAlign.Right;
    //        //row.Cells.Add(cell);
    //        cell = new TableCell();

    //        cell.Text = TotalRequrment.ToString("N2");
    //        cell.HorizontalAlign = HorizontalAlign.Right;
    //        row.Cells.Add(cell);

    //        cell = new TableCell();

    //        cell.Text = totalStock.ToString("N0");
    //        cell.HorizontalAlign = HorizontalAlign.Right;
    //        row.Cells.Add(cell);
    //        //lblQuantity.Text = totalStock.ToString("N0");
    //        //cell = new TableCell();
    //        //cell.Text = totItemsP.ToString("N0");
    //        //cell.HorizontalAlign = HorizontalAlign.Right;
    //        //row.Cells.Add(cell);
    //        //cell = new TableCell();
    //        //cell.Text = totA.ToString("N2");
    //        //cell.HorizontalAlign = HorizontalAlign.Right;
    //        //row.Cells.Add(cell);
    //        cell = new TableCell();
    //        // cell.ColumnSpan = 2;
    //        cell.Text = totalStock.ToString("N0");
    //        cell.ColumnSpan = 1;
    //        cell.HorizontalAlign = HorizontalAlign.Right;
    //        row.Cells.Add(cell);

    //        cell = new TableCell();
    //        // cell.ColumnSpan = 2;
    //        cell.Text = "";
    //        cell.HorizontalAlign = HorizontalAlign.Right;
    //        row.Cells.Add(cell);
    //        row.Font.Bold = true;
    //        row.BackColor = System.Drawing.Color.LightGray;
    //        if (dgDistribution.Rows.Count > 0)
    //        {
    //            dgDistribution.Controls[0].Controls.Add(row);
    //        }

    //    }

    //    catch (FormatException fex)
    //    {
    //        ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "jsAlert('" + fex.Message + "','red',0);", true);

    //    }
    //    catch (Exception ex)
    //    {
    //        if (ex.Message.Contains("Database"))
    //            ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "jsAlert('Database Maintain Error. Contact to the Software Provider..!!','red',0);", true);
    //        else ;
    //        ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "jsAlert('There is some problem to do the task. Try again properly.!!','red',0);", true);
    //    }

    //}
    public DataTable PopulateMeasure()
    {
        dtmsr = MeterialRequistitonManager.GetMeasure();
        DataRow dr = dtmsr.NewRow();
        dtmsr.Rows.InsertAt(dr, 0);
        return dtmsr;
    }
    protected void txtPresentQty_TextChanged(object sender, EventArgs e)
    {
        GridViewRow gvr = (GridViewRow)((TextBox)sender).NamingContainer;
        //TextBox textBox = (TextBox)gvr.FindControl("txtCode");
        //string textBoxValue = textBox.Text;
        DataTable dt = (DataTable)Session["purdtl"]; //distrubationManager.GetThisValu(textBoxValue);
        if (dt.Rows.Count > 0)
        {
            //for (int i = 0; i < dt.Rows.Count; i++)
            //{
                
            //}
            DataRow dr = dt.Rows[gvr.DataItemIndex];
            dr["ID"] = dr["ID"].ToString();
            dr["item_desc"] = dr["item_desc"].ToString();
            dr["item_code"] = dr["item_code"].ToString();
            dr["msr_unit_code"] = dr["msr_unit_code"].ToString();
            dr["requisition_qnty"] = dr["requisition_qnty"].ToString();
            dr["duerequisition_qnty"] = dr["duerequisition_qnty"].ToString();
           // dr["present_Stock"] = ((DataRow)dt.Rows[0])["present_Stock"].ToString();
            dr["present_Stock"] = ((TextBox)gvr.FindControl("txtStockQuantity")).Text;
            dr["total_Stock"] = dr["total_Stock"].ToString();
            //dr["PSRequirement"] = ((DataRow)dt.Rows[0])["PSRequirement"].ToString();
            dr["Remarksany"] = dr["Remarksany"].ToString();
            dr["this_time_requisition_qnty"] = ((TextBox)gvr.FindControl("txtPresentQty")).Text;

            //var totalstock = ((TextBox)gvr.FindControl("present_Stock")).Text;
            //int total = Convert.ToInt32(totalstock);
            //var presentQnty =((TextBox) gvr.FindControl("txtPresentQty")).Text;
            //int present = Convert.ToInt32(presentQnty);
            //if (total < present)                     
            //{
            //    string Mgs = "Items Quantity Over This Closing Quantity. So 1 is Fxied\\n Tolat Closing Qiantity : (" +
            //                 dr["total_Stock"].ToString() + ")..!!";
            //    ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "jsAlert('" + Mgs + "','INDIANRED',2);", true);

            //     return;
            //}

        }
        string found = "";
        foreach (DataRow drd in dt.Rows)
        {
            if (drd["ID"].ToString() == "" && drd["item_desc"].ToString() == "")
            {
                found = "Y";
            }
            
        }
        if (found == "")
        {
            DataRow drd = dt.NewRow();
            dt.Rows.Add(drd);
        }
        dgDistribution.DataSource = dt;
        dgDistribution.DataBind();
        ShowFooterTotal(dt, dgDistribution);
        lblQuantity.Text = GetQuantity(dt).ToString();
        // ((TextBox)gvr.FindControl("txtItemRate")).Focus();        
        ((TextBox)dgDistribution.Rows[dgDistribution.Rows.Count - 1].FindControl("txtPresentQty")).Focus();
        MRIesms_UP.Update();
    }
    private void ShowFooterTotal(DataTable dt, GridView dgPODetailsDtl2)
    {
        try
        {
            GridViewRow row = new GridViewRow(0, 0, DataControlRowType.Footer, DataControlRowState.Normal);
            TableCell cell;
            int j = 0;
            if (dgPODetailsDtl2.Columns[0].Visible == true)
            {
                j = dgPODetailsDtl2.Columns.Count - 6;
            }
            else if (dgPODetailsDtl2.Columns[0].Visible == false)
            {
                j = dgPODetailsDtl2.Columns.Count - 7;
            }

            for (int i = 0; i < j; i++)
            {
                cell = new TableCell();
                cell.Text = "";
                row.Cells.Add(cell);
            }
            cell = new TableCell();
            cell.Text = "Total";
            cell.ColumnSpan = 2;
            cell.HorizontalAlign = HorizontalAlign.Right;
            row.Cells.Add(cell);


            cell = new TableCell();
            //priceDr = GetQuantity();
            cell.Text = GetQuantity(dt).ToString();
            cell.HorizontalAlign = HorizontalAlign.Center;
            row.Cells.Add(cell);

            cell.HorizontalAlign = HorizontalAlign.Right;
            row.Cells.Add(cell);
            row.Font.Bold = true;
            row.BackColor = System.Drawing.Color.LightGray;
            if (dgPODetailsDtl2.Rows.Count > 0)
            {
                dgPODetailsDtl2.Controls[0].Controls.Add(row);
            }
        }
        catch (Exception ex)
        {

            throw new Exception(ex.Message);

            ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "jsAlert('" + ex.Message + "','red',0);", true);

        }
    }

    private object GetQuantity(DataTable dt)
    {
        decimal ctot = decimal.Zero;
        if (dt != null)
        {
            //DataTable dt = (DataTable)ViewState[OrDerRcFg];
            foreach (DataRow drp in dt.Rows)
            {
                if (drp["ID"].ToString() != "" && drp["this_time_requisition_qnty"].ToString() != "")
                {
                    ctot += decimal.Parse(drp["this_time_requisition_qnty"].ToString());
                }
            }
        }

        return ctot;
    }
    protected void dgDistribution_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        if (Session["purdtl"] != null)
        {
            DataTable dtdtl = (DataTable)Session["purdtl"];
            dtdtl.Rows.RemoveAt(dgDistribution.Rows[e.RowIndex].RowIndex);
            if (dtdtl.Rows.Count > 0)
            {

                dgDistribution.DataSource = dtdtl;
                Session["purdtl"] = dtdtl;
                dgDistribution.DataBind();
            }
            else
            {
                GetRequisition();
            }
            //ShowFooterTotal();
        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(), "ale", "alert('Your session is over. Try it again!!');", true);
        }
    }



    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(txtTfDate.Text))
        {
            ClientScript.RegisterStartupScript(this.GetType(), "Warning", "alert('Input Distrubation Date.!!');", true);
            return;
        }

        if (checkbox.Text == "Fixed Assest Or Head Office Product")
        {
            if (string.IsNullOrEmpty(ddlTransferProject.SelectedItem.Text))
            {
                ClientScript.RegisterStartupScript(this.GetType(), "Warning", "alert('Select Project Name.!!');", true);
                return;
            } 
        }
        else
        {
            if (string.IsNullOrEmpty(ddlProjectName.SelectedItem.Text))
            {
                ClientScript.RegisterStartupScript(this.GetType(), "Warning", "alert('Select Project Name.!!');", true);
                return;
            } 
        }
       

        _distrubationMdl.Code = txtRequisitionCode.Text.Replace("'", "’"); 
        _distrubationMdl.Date = txtTfDate.Text.Replace("'", "’"); 
        
        if (checkbox.Text == "Fixed Assest Or Head Office Product")
        {
            _distrubationMdl.ProjectId = Convert.ToInt32(ddlTransferProject.SelectedValue);
            _distrubationMdl.ItemType = "1";
        }
        else
        {
            _distrubationMdl.ProjectId = Convert.ToInt32(ddlProjectName.SelectedValue);
            _distrubationMdl.ItemType = "0";
        }
        _distrubationMdl.Remark = txtRemark.Text.Replace("'", "’");
        _distrubationMdl.ChalanNo = txtChallanNo.Text.Replace("'", "’");
        _distrubationMdl.RequisitionID = lblReqNo.Text.Replace("'", "’");
        _distrubationMdl.RequisitionCode = txtRequisitionNo.Text.Replace("'", "’");
       _distrubationMdl.LoginBy= Session["userID"].ToString();

        DataTable dt = (DataTable)Session["purdtl"];
         distrubationManager.distrubationSave(dt, _distrubationMdl);
         ClientScript.RegisterStartupScript(this.GetType(), "Warning", "alert('Record has been Save successfully...!!');", true);
          Refresh();
            
    }
    protected void btnClear_Click(object sender, EventArgs e)
    {
        Response.Redirect("frmDistrubation.aspx?mno=0.8");
    }
    protected void dgRequistionHistory_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow | e.Row.RowType == DataControlRowType.Header | e.Row.RowType == DataControlRowType.Footer)
        {
            e.Row.Cells[1].Attributes.Add("style", "display:none");
            e.Row.Cells[7].Attributes.Add("style", "display:none");
            if (e.Row.Cells[8].Text.Equals("Not Received"))
            {
                e.Row.Cells[8].ForeColor = System.Drawing.Color.Red;

            }
            else
            {
                e.Row.Cells[8].ForeColor = System.Drawing.Color.Green;
            }
        }
    }
    protected void dgRequistionHistory_SelectedIndexChanged(object sender, EventArgs e)
    {
        txtID.Text = dgRequistionHistory.SelectedRow.Cells[1].Text;
        DataTable dt = distrubationManager.GetPurchaseMaterialMst(txtID.Text);

        if (dt != null)
        {

            txtRequisitionCode.Text = dt.Rows[0]["Code"].ToString();
            txtRequisitionNo.Text = dt.Rows[0]["Requisition_Code"].ToString();
            txtRemark.Text = dt.Rows[0]["Remarks"].ToString();
            txtChallanNo.Text = dt.Rows[0]["ChalanNo"].ToString();
            
            txtTfDate.Text = dt.Rows[0]["CreateDate"].ToString();
            if (dt.Rows[0]["HeadStatus"].ToString() == "1")
            {
                ddlTransferProject.SelectedValue = dt.Rows[0]["ProjectId"].ToString();
                checkbox.Checked = true;
            }
            else
            {
                ddlProjectName.SelectedValue = dt.Rows[0]["ProjectId"].ToString();
                checkbox.Checked = false;
            }
            btnSave.Visible = btnSave.Enabled = true;
            DataTable dt1 = distrubationManager.GetMetrialsDetails(txtID.Text);
            DataRow drd = dt1.NewRow();
            dt1.Rows.Add(drd);
            dgDistribution.DataSource = dt1;
            ViewState["purdtl"] = dt1;
            Session["purdtl"] = dt1;
            dgDistribution.DataBind();
            btnNew.Enabled = btnNew.Visible = false;
            dgRequistionHistory.Visible = false;
            ddlProjectName.Enabled = true;
            dgDistribution.Visible = true;
        }
    }
    protected void btnPrint_Click(object sender, EventArgs e)
    {
        getDistrubation();
    }

    public void getDistrubation()
    {
        string filename = "DO_" + txtRequisitionCode.Text;
        Response.Clear();
        Response.ContentType = "application/pdf";
        Response.AddHeader("content-disposition", "attachment; filename=" + filename + ".pdf");
        Document document = new Document(PageSize.A4, 10f, 10f, 10f, 30f);
        PdfWriter writer = PdfWriter.GetInstance(document, Response.OutputStream);
        document.Open();
        Rectangle page = document.PageSize;
        PdfPTable head = new PdfPTable(1);
        head.TotalWidth = page.Width - 50;
        Phrase phrase = new Phrase(Convert.ToDateTime(Session["date"]).ToString("dd/MM/yyyy"), new Font(Font.FontFamily.TIMES_ROMAN, 8));
        PdfPCell c = new PdfPCell(phrase);
        c.Border = Rectangle.NO_BORDER;
        c.VerticalAlignment = Element.ALIGN_BOTTOM;
        c.HorizontalAlignment = Element.ALIGN_RIGHT;
        head.AddCell(c);
        head.WriteSelectedRows(0, -1, 0, page.Height - document.TopMargin + head.TotalHeight + 20, writer.DirectContent);

        PdfPCell cell;
        byte[] logo = GlBookManager.GetGlLogo(Session["book"].ToString());
        iTextSharp.text.Image gif = iTextSharp.text.Image.GetInstance(logo);
        gif.Alignment = iTextSharp.text.Image.MIDDLE_ALIGN;
        gif.ScalePercent(5f);

        float[] titwidth = new float[2] { 10, 200 };
        PdfPTable dth = new PdfPTable(titwidth);
        dth.WidthPercentage = 100;

        cell = new PdfPCell(gif);
        cell.HorizontalAlignment = 1;
        cell.VerticalAlignment = 1;
        cell.Rowspan = 4;
        cell.BorderWidth = 0f;
        dth.AddCell(cell);
        cell = new PdfPCell(new Phrase(Session["org"].ToString(), FontFactory.GetFont(FontFactory.TIMES_ROMAN, 12, iTextSharp.text.Font.BOLD)));
        cell.HorizontalAlignment = 1;
        cell.VerticalAlignment = 1;
        cell.Colspan = 7;
        cell.BorderWidth = 0f;
        // cell.FixedHeight = 20f;
        dth.AddCell(cell);
        cell = new PdfPCell(new Phrase(Session["add1"].ToString(), FontFactory.GetFont(FontFactory.TIMES_ROMAN, 10, iTextSharp.text.Font.BOLD)));
        cell.HorizontalAlignment = 1;
        cell.VerticalAlignment = 1;
        cell.Colspan = 7;
        cell.BorderWidth = 0f;
        // cell.FixedHeight = 20f;
        dth.AddCell(cell);
        cell = new PdfPCell(new Phrase(Session["add2"].ToString(), FontFactory.GetFont(FontFactory.TIMES_ROMAN, 10, iTextSharp.text.Font.BOLD)));
        cell.HorizontalAlignment = 1;
        cell.VerticalAlignment = 1;
        cell.Colspan = 7;
        cell.BorderWidth = 0f;
        // cell.FixedHeight = 20f;
        dth.AddCell(cell);
        cell = new PdfPCell(new Phrase("Distrubation Product(DP)", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 10, iTextSharp.text.Font.BOLD)));
        cell.HorizontalAlignment = 1;
        cell.VerticalAlignment = 1;
        cell.Colspan = 7;
        cell.BorderWidth = 0f;
        // cell.FixedHeight = 30f;
        dth.AddCell(cell);
        document.Add(dth);
        LineSeparator line = new LineSeparator(1, 100, null, Element.ALIGN_CENTER, -2);
        document.Add(line);
        PdfPTable dtempty = new PdfPTable(1);
        cell = new PdfPCell(FormatHeaderPhrase(""));
        cell.BorderWidth = 0f;
        cell.FixedHeight = 20f;
        dtempty.AddCell(cell);
        document.Add(dtempty);

        float[] titW = new float[2] { 60, 40 };
        PdfPTable pdtm = new PdfPTable(titW);
        pdtm.WidthPercentage = 100;

        PdfPTable pdtclient = new PdfPTable(4);
        pdtclient.WidthPercentage = 100;
        cell = new PdfPCell(FormatHeaderPhrase("Distrubation Code "));
        cell.BorderWidth = 0f;
        cell.FixedHeight = 20f;
        pdtclient.AddCell(cell);
        cell = new PdfPCell(FormatPhrase(": " + txtRequisitionCode.Text));
        cell.BorderWidth = 0f;
        cell.FixedHeight = 20f;
        cell.Colspan = 3;
        pdtclient.AddCell(cell);
        cell = new PdfPCell(FormatHeaderPhrase("Project Name "));
        cell.BorderWidth = 0f;
        cell.FixedHeight = 20f;
        pdtclient.AddCell(cell);
        cell = new PdfPCell(FormatPhrase(": " + ddlProjectName.SelectedItem.Text));
        cell.BorderWidth = 0f;
        cell.FixedHeight = 20f;
        cell.Colspan = 3;
        pdtclient.AddCell(cell);
        //cell = new PdfPCell(FormatHeaderPhrase(""));
        //cell.BorderWidth = 0f;
        //cell.FixedHeight = 20f;
        ////cell.Colspan = 2;
        //pdtclient.AddCell(cell);
        //cell = new PdfPCell(FormatPhrase(""));
        //cell.BorderWidth = 0f;
        //cell.FixedHeight = 20f;
        //cell.Colspan = 3;
        //pdtclient.AddCell(cell);
        //cell = new PdfPCell(FormatHeaderPhrase(""));
        //cell.BorderWidth = 0f;
        //cell.FixedHeight = 20f;
        ////cell.Colspan = 4;
        //pdtclient.AddCell(cell);
        //cell = new PdfPCell(FormatPhrase("" ));
        //cell.BorderWidth = 0f;
        //cell.FixedHeight = 20f;
        //cell.Colspan = 3;
        //pdtclient.AddCell(cell);

        PdfPTable pdtpur = new PdfPTable(2);
        pdtpur.WidthPercentage = 100;
        cell = new PdfPCell(FormatHeaderPhrase("Requisition Code "));
        cell.BorderWidth = 0f;
        cell.FixedHeight = 20f;
        pdtpur.AddCell(cell);
        cell = new PdfPCell(FormatPhrase(": " + txtRequisitionNo.Text));
        cell.BorderWidth = 0f;
        cell.FixedHeight = 20f;
        pdtpur.AddCell(cell);
        cell = new PdfPCell(FormatHeaderPhrase("Distrubation  Date "));
        cell.BorderWidth = 0f;
        cell.FixedHeight = 20f;
        pdtpur.AddCell(cell);
        cell = new PdfPCell(FormatPhrase(": " + txtTfDate.Text));
        cell.BorderWidth = 0f;
        cell.FixedHeight = 20f;
        pdtpur.AddCell(cell);
        //cell = new PdfPCell(FormatHeaderPhrase(""));
        //cell.BorderWidth = 0f;
        //cell.FixedHeight = 20f;
        //pdtpur.AddCell(cell);
        //cell = new PdfPCell(FormatPhrase("" ));
        //cell.BorderWidth = 0f;
        //cell.FixedHeight = 20f;
        //pdtpur.AddCell(cell);
        //cell = new PdfPCell(FormatHeaderPhrase(""));
        //cell.BorderWidth = 0f;
        //cell.FixedHeight = 20f;
        //pdtpur.AddCell(cell);
        //cell = new PdfPCell(FormatPhrase(""));
        //cell.BorderWidth = 0f;
        //cell.FixedHeight = 20f;
        //pdtpur.AddCell(cell);


        cell = new PdfPCell(pdtclient);
        cell.BorderWidth = 0f;
        pdtm.AddCell(cell);
        cell = new PdfPCell(pdtpur);
        cell.BorderWidth = 0f;
        pdtm.AddCell(cell);
        document.Add(pdtm);
       

        //document.Add(pdtpur);       
        float[] widthdtl = new float[7] { 10, 15, 40, 20, 20, 20, 20 };
        PdfPTable pdtdtl = new PdfPTable(widthdtl);
        pdtdtl.WidthPercentage = 100;

        cell = new PdfPCell(FormatHeaderPhrase("Serial"));
        cell.HorizontalAlignment = 1;
        cell.VerticalAlignment = 1;
        cell.FixedHeight = 20f;
        cell.BorderColor = BaseColor.LIGHT_GRAY;
        pdtdtl.AddCell(cell);
        cell = new PdfPCell(FormatHeaderPhrase("Item Code"));
        cell.HorizontalAlignment = 1;
        cell.VerticalAlignment = 1;
        cell.FixedHeight = 20f;
        cell.BorderColor = BaseColor.LIGHT_GRAY;
        pdtdtl.AddCell(cell);
        cell = new PdfPCell(FormatHeaderPhrase("Description"));
        cell.HorizontalAlignment = 0;
        cell.VerticalAlignment = 1;
        cell.FixedHeight = 20f;
        cell.BorderColor = BaseColor.LIGHT_GRAY;
        pdtdtl.AddCell(cell);

        cell = new PdfPCell(FormatHeaderPhrase("Unit"));
        cell.HorizontalAlignment = 1;
        cell.VerticalAlignment = 1;
        cell.FixedHeight = 20f;
        cell.BorderColor = BaseColor.LIGHT_GRAY;
        pdtdtl.AddCell(cell);

        cell = new PdfPCell(FormatHeaderPhrase("Requisition Qnty"));
        cell.HorizontalAlignment = 1;
        cell.VerticalAlignment = 1;
        cell.FixedHeight = 20f;
        cell.BorderColor = BaseColor.LIGHT_GRAY;
        pdtdtl.AddCell(cell);

        cell = new PdfPCell(FormatHeaderPhrase("Due Requisition"));
        cell.HorizontalAlignment = 1;
        cell.VerticalAlignment = 1;
        cell.FixedHeight = 20f;
        cell.BorderColor = BaseColor.LIGHT_GRAY;
        pdtdtl.AddCell(cell);

      
        cell = new PdfPCell(FormatHeaderPhrase("Distrubation Qnty"));
        cell.HorizontalAlignment = 1;
        cell.VerticalAlignment = 1;
        cell.FixedHeight = 20f;
        cell.BorderColor = BaseColor.LIGHT_GRAY;
        pdtdtl.AddCell(cell);
        //cell = new PdfPCell(FormatHeaderPhrase(""));
        //cell.HorizontalAlignment = 2;
        //cell.VerticalAlignment = 1;
        //cell.FixedHeight = 20f;
        //cell.BorderColor = BaseColor.LIGHT_GRAY;
        //pdtdtl.AddCell(cell);
        //DataTable dt = (DataTable)Session["purdtl"];
        DataTable dt = distrubationManager.GetDistrubationItemsDetails(txtID.Text);
        int Serial = 1;
        decimal DueQty = 0;
        decimal tot = 0;
        foreach (DataRow dr in dt.Rows)
        {
            if (Convert.ToDecimal(dr["ID"]) != 0)
            {
                cell = new PdfPCell(FormatPhrase(Serial.ToString()));
                cell.HorizontalAlignment = 1;
                cell.VerticalAlignment = 1;
                cell.BorderColor = BaseColor.LIGHT_GRAY;
                pdtdtl.AddCell(cell);
                Serial++;

                cell = new PdfPCell(FormatPhrase(dr["item_code"].ToString()));
                cell.HorizontalAlignment = 1;
                cell.VerticalAlignment = 1;
                cell.BorderColor = BaseColor.LIGHT_GRAY;
                pdtdtl.AddCell(cell);

                cell = new PdfPCell(FormatPhrase(dr["item_desc"].ToString()));
                cell.HorizontalAlignment = 0;
                cell.VerticalAlignment = 1;
                cell.BorderColor = BaseColor.LIGHT_GRAY;
                pdtdtl.AddCell(cell);

                cell = new PdfPCell(FormatPhrase(dr["msr_unit_code"].ToString()));
                cell.HorizontalAlignment = 1;
                cell.VerticalAlignment = 1;
                cell.BorderColor = BaseColor.LIGHT_GRAY;
                pdtdtl.AddCell(cell);

                cell = new PdfPCell(FormatPhrase(dr["requisition_qnty"].ToString()));
                cell.HorizontalAlignment = 1;
                cell.VerticalAlignment = 1;
                cell.BorderColor = BaseColor.LIGHT_GRAY;
                pdtdtl.AddCell(cell);




                cell = new PdfPCell(FormatPhrase(dr["duerequisition_qnty"].ToString()));
                cell.HorizontalAlignment = 1;
                cell.VerticalAlignment = 1;
                cell.BorderColor = BaseColor.LIGHT_GRAY;
                pdtdtl.AddCell(cell);

                cell = new PdfPCell(FormatPhrase(dr["duerequisition_qnty"].ToString()));
                cell.HorizontalAlignment = 1;
                cell.VerticalAlignment = 1;
                cell.BorderColor = BaseColor.LIGHT_GRAY;
                pdtdtl.AddCell(cell);
                DueQty += Convert.ToDecimal(dr["duerequisition_qnty"]);


                cell = new PdfPCell(FormatPhrase(dr["distribution_qty"].ToString()));
                cell.HorizontalAlignment = 1;
                cell.VerticalAlignment = 1;
                cell.BorderColor = BaseColor.LIGHT_GRAY;
                pdtdtl.AddCell(cell);


              
                //if (Convert.ToDecimal(dr["ID"]) != 0)
                //{
                tot += Convert.ToDecimal(dr["distribution_qty"]);
                //}
                //if (Convert.ToDecimal(dr["ID"]) != 0)
                //{
              //  totQty += Convert.ToDecimal(dr["qnty"]);
                //}
            }
        }

        cell = new PdfPCell(FormatPhrase("Total"));
        cell.FixedHeight = 20f;
        cell.HorizontalAlignment = 2;
        cell.VerticalAlignment = 1;
        cell.BorderColor = BaseColor.LIGHT_GRAY;
        cell.Colspan = 6;
        pdtdtl.AddCell(cell);

        //cell = new PdfPCell(FormatPhrase(totQty.ToString("N2")));
        //// cell.BorderWidth = 0f;
        //cell.FixedHeight = 20f;
        //cell.HorizontalAlignment = 2;
        //cell.VerticalAlignment = 1;
        //cell.BorderColor = BaseColor.LIGHT_GRAY;
        //pdtdtl.AddCell(cell);

        cell = new PdfPCell(FormatPhrase(tot.ToString("N2")));
        //cell.BorderWidth = 0f;
        cell.FixedHeight = 20f;
        cell.HorizontalAlignment = 2;
        cell.VerticalAlignment = 1;
        cell.BorderColor = BaseColor.LIGHT_GRAY;
        pdtdtl.AddCell(cell);

        cell = new PdfPCell(FormatPhrase(DueQty.ToString("N2")));
        //cell.BorderWidth = 0f;
        cell.FixedHeight = 10f;
        cell.HorizontalAlignment = 1;
        cell.VerticalAlignment = 1;
        cell.Border = 0;
        cell.Colspan = 7;
        pdtdtl.AddCell(cell);


        //cell = new PdfPCell(FormatHeaderPhrase("Comments :"));
        //cell = new PdfPCell(FormatPhrase("In word: " + DataManager.GetLiteralAmt(tot.ToString()).Replace("  ", " ").Replace("  ", " ")));
        ////cell.BorderWidth = 0f;
        //cell.FixedHeight = 20f;
        //cell.HorizontalAlignment = 0;
        //cell.VerticalAlignment = 1;
        //cell.Border = 0;
        //cell.Colspan = 7;
        //pdtdtl.AddCell(cell);


        document.Add(pdtdtl);

        PdfPTable pdtsig = new PdfPTable(3);
        pdtsig.WidthPercentage = 100;
        cell = new PdfPCell(FormatPhrase(""));
        cell.HorizontalAlignment = 1;
        cell.VerticalAlignment = 1;
        cell.Border = 0;
        cell.Colspan = 3;
        cell.FixedHeight = 40f;
        cell.BorderColor = BaseColor.LIGHT_GRAY;
        pdtsig.AddCell(cell);
        cell = new PdfPCell(FormatPhrase("Received by"));
        cell.HorizontalAlignment = 1;
        cell.VerticalAlignment = 1;
        cell.Border = 1;
        cell.FixedHeight = 20f;
        cell.BorderColor = BaseColor.LIGHT_GRAY;
        pdtsig.AddCell(cell);
        cell = new PdfPCell(FormatPhrase(""));
        cell.HorizontalAlignment = 0;
        cell.VerticalAlignment = 1;
        cell.Border = 0;
        cell.FixedHeight = 20f;
        cell.BorderColor = BaseColor.LIGHT_GRAY;
        pdtsig.AddCell(cell);
        cell = new PdfPCell(FormatPhrase("Prepared by"));
        cell.HorizontalAlignment = 1;
        cell.VerticalAlignment = 1;
        cell.Border = 1;
        cell.FixedHeight = 20f;
        cell.BorderColor = BaseColor.LIGHT_GRAY;
        pdtsig.AddCell(cell);
        document.Add(pdtsig);

        document.Close();
        Response.Flush();
        Response.End();
    }
    private static Phrase FormatPhrase(string value)
    {
        return new Phrase(value, FontFactory.GetFont(FontFactory.TIMES_ROMAN, 9));
    }

    private static Phrase FormatHeaderPhrase(string value)
    {
        return new Phrase(value, FontFactory.GetFont(FontFactory.TIMES_ROMAN, 10, iTextSharp.text.Font.BOLD));
    }
    protected void dgDistribution_RowDataBound1(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow | e.Row.RowType == DataControlRowType.Header | e.Row.RowType == DataControlRowType.Footer)
        {
            e.Row.Cells[10].Attributes.Add("style", "display:none");
          //  e.Row.Cells[7].Attributes.Add("style", "display:none");

           
        }
    }


    protected void checkbox_CheckedChanged(object sender, EventArgs e)
    {
        if (checkbox.Checked == true)
        {
            checkbox.Text = "Fixed Assest Or Head Office Product";
            ddlProjectName.Visible = false;
            Label5.Visible = false;
            Label3.Visible = false;
            lblSerchitem.Visible = txtSerchItem.Visible = true;
            txtRequisitionNo.Visible = lblSerchRequisitionNo.Visible = false;
        }
        else
        {

            checkbox.Text = "Transfer Matrial";
            ddlProjectName.Visible = true;
            Label5.Visible = true;
            Label3.Visible = true;
            lblSerchitem.Visible = txtSerchItem.Visible = false;
            txtRequisitionNo.Visible = lblSerchRequisitionNo.Visible = true;
        }
    }
    protected void txtSerchItem_TextChanged(object sender, EventArgs e)
    {
        try
        {
            string ItemCode = "";
            DataTable dtl = IdManager.GetShowDataTable("select CODE from View_StockSearchInfo where upper(Items) like upper('%" + txtSerchItem.Text + "%')  and ProjectId is null ");

            if (dtl.Rows.Count > 0)
            {
                ItemCode = dtl.Rows[0]["Code"].ToString();
                txtSerchItem.Text = ItemCode;
            }
            else
            {
                txtSerchItem.Text = "";
                txtSerchItem.Focus();
                return;
            }

            DataTable dt = (DataTable)Session["purdtl"];
            DataTable dtdtl = distrubationManager.GetItemsForHeadOffice(ItemCode);
            decimal Quantity = Convert.ToDecimal(dtdtl.Rows[0]["present_Stock"].ToString());
            if (Quantity > 0)
            {
                if (dtdtl.Rows.Count > 0)
                {

                    string find = "";
                    string flag = "";

                    foreach (DataRow data in dt.Rows)
                    {
                        if (data["ID"].ToString() == dtdtl.Rows[0]["Id"].ToString() && data["item_code"].ToString() == dtdtl.Rows[0]["item_code"].ToString() && data["item_desc"].ToString() == dtdtl.Rows[0]["item_desc"].ToString())
                        {
                            if (Quantity > Convert.ToDecimal(data["present_Stock"].ToString()))
                            {
                                flag = "Y";
                                ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('Allready Add This Items...!!');", true);
                            }


                        }

                        if (data["ID"].ToString() == "" && data["item_desc"].ToString() == "")
                        {
                            find = "Y";

                        }
                    }
                    if (find == "Y")
                    {
                        dt.Rows.Remove(dt.Rows[0]);

                    }
                    if (flag != "Y")
                    {
                        DataRow dr = dt.NewRow();

                   
                        dr["ID"] = dtdtl.Rows[0]["Id"].ToString();
                        dr["item_desc"] = dtdtl.Rows[0]["item_desc"].ToString();
                        dr["item_code"] = dtdtl.Rows[0]["item_code"].ToString();
                        dr["msr_unit_code"] = dtdtl.Rows[0]["UOMID"].ToString();
                        dr["requisition_qnty"] = "0";
                        dr["duerequisition_qnty"] = dtdtl.Rows[0]["duerequisition_qnty"].ToString();
                        dr["present_Stock"] = dtdtl.Rows[0]["present_Stock"].ToString();
                        dr["total_Stock"] = dtdtl.Rows[0]["total_Stock"].ToString();
                        //dr["PSRequirement"] = ((DataRow)dt.Rows[0])["PSRequirement"].ToString();
                        dr["Remarksany"] = dtdtl.Rows[0]["Remarksany"].ToString();


                        dt.Rows.Add(dr);
                    }
                    txtSerchItem.Text = "";
                    txtSerchItem.Focus();
                    dgDistribution.DataSource = dt;                    
                    Session["purdtl"] = dt;
                    dgDistribution.DataBind();
                    
                }
            }

            else
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('Item Transfer Quantity Is Over...!!');", true);
                txtSerchItem.Text = "";
                txtSerchItem.Focus();
                return;
            }
            txtSerchItem.Focus();
            //ShowFooterTotal(dt, dgDistribution);
            //UP4.Update();
            //UP44.Update();
            txtSerchItem.Focus();
        }
        catch (Exception ex)
        {


            ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "jsAlert('" + ex.Message + "','red',0);", true);
        }
    }
}  
       