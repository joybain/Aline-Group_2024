﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using autouniv;
using iTextSharp.text.pdf;
using iTextSharp.text;
using iTextSharp.text.pdf.draw;
using OldColor;
using System.Data.SqlClient;

public partial class frmManafactureRequisition : System.Web.UI.Page
{
    private static DataTable dtsup = new DataTable();
    private static DataTable dtmsr = new DataTable();
    public static decimal priceDr = 0;
    private static Permis per;

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
            txtOrderNo.Text = txtFromDate.Text = txtToDAte.Text = string.Empty;
            txtPONo.Enabled = txtPoDate.Enabled = ddlManufacturedBy.Enabled =   txtDelDate.Enabled =  true;
            getEmptyDtl();
            string query = "Select null as ID,'' Name union select ID,F_Name from dbo.PMIS_PERSONNEL order by 1";
            util.PopulationDropDownList(ddlManufacturedBy, "PMIS_PERSONNEL", query, "Name", "ID");
         
           
            DataTable dt = RequisitionOrderManager.GetRequisitionInfo("");
            ViewState["reor"] = dt;
            dgPOrderMst.DataSource = dt;
            dgPOrderMst.DataBind();
            btnNew.Visible = Panel1.Visible = true;
            ddlManufacturedBy.Focus();
            RefreshAll();
        }
    }
    private void getEmptyDtl()
    {
        dgPODetailsDtl.Visible = true;
        DataTable dtDtlGrid = new DataTable();
        dtDtlGrid.Columns.Add("ID", typeof(string));
        dtDtlGrid.Columns.Add("item_code", typeof(string));
        dtDtlGrid.Columns.Add("item_desc", typeof(string));
        dtDtlGrid.Columns.Add("msr_unit_code", typeof(string));
        dtDtlGrid.Columns.Add("item_rate", typeof(string));
        dtDtlGrid.Columns.Add("qnty", typeof(string));
        DataRow dr = dtDtlGrid.NewRow();
        dtDtlGrid.Rows.Add(dr);
        dgPODetailsDtl.DataSource = dtDtlGrid;
        ViewState["reqdtl"] = dtDtlGrid;
        dgPODetailsDtl.DataBind();
    }
    //************* Pv Items Details ******//
    protected void dgPurDtl_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (((DataRowView)e.Row.DataItem)["qnty"].ToString() != "" && ((DataRowView)e.Row.DataItem)["item_rate"].ToString() != "")
                {
                    decimal total = decimal.Parse(((DataRowView)e.Row.DataItem)["item_rate"].ToString()) *
                                   decimal.Parse(((DataRowView)e.Row.DataItem)["qnty"].ToString());
                    ((Label)e.Row.FindControl("lblTotal")).Text = total.ToString("N2");

                }
                e.Row.Cells[7].Attributes.Add("style", "display:none");

                e.Row.Cells[6].Attributes.Add("style", "display:none");
                e.Row.Cells[4].Attributes.Add("style", "display:none");
            }
            else if (e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.Cells[6].Attributes.Add("style", "display:none");
                e.Row.Cells[4].Attributes.Add("style", "display:none");
                e.Row.Cells[7].Attributes.Add("style", "display:none");
            }
            else if (e.Row.RowType == DataControlRowType.Footer)
            {
                e.Row.Cells[6].Attributes.Add("style", "display:none");
                e.Row.Cells[4].Attributes.Add("style", "display:none");
                e.Row.Cells[7].Attributes.Add("style", "display:none");
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
    protected void txtItemCode_TextChanged(object sender, EventArgs e)
    {

    }
    protected void txtItemDesc_TextChanged(object sender, EventArgs e)
    {
        GridViewRow gvr = (GridViewRow)((TextBox)sender).NamingContainer;
        DataTable dtdtl = (DataTable)ViewState["reqdtl"];
        DataRow dr = dtdtl.Rows[gvr.DataItemIndex];
        DataTable dt = ItemManager.GetItems(((TextBox)gvr.FindControl("txtItemDesc")).Text);
        if (dt.Rows.Count > 0)
        {

            string flag = "";
            foreach (DataRow Dr2 in dtdtl.Rows)
            {
                if (Dr2["ID"].ToString() == dt.Rows[0]["ID"].ToString())
                {


                    flag = "Y";
                    ((TextBox)dgPODetailsDtl.Rows[gvr.DataItemIndex].FindControl("txtItemDesc")).Focus();
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
                dr["item_rate"] = ((DataRow)dt.Rows[0])["UnitPrice"].ToString();
                dr["qnty"] = "0";
                dtdtl.Rows.InsertAt(dr, gvr.DataItemIndex);
            }

            dgPODetailsDtl.DataSource = dtdtl;
            dgPODetailsDtl.DataBind();
            ShowFooterTotal();
            ((TextBox)dgPODetailsDtl.Rows[gvr.DataItemIndex].FindControl("txtItemRate")).Focus();
            PVIesms_UP.Update();
        }

    }
    protected void dgPurDtl_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        if (ViewState["reqdtl"] != null)
        {
            DataTable dtDtlGrid = (DataTable)ViewState["reqdtl"];
            dtDtlGrid.Rows.RemoveAt(dgPODetailsDtl.Rows[e.RowIndex].DataItemIndex);
            if (dtDtlGrid.Rows.Count > 0)
            {
                string found = "";
                foreach (DataRow drf in dtDtlGrid.Rows)
                {
                    if (drf["ID"].ToString() == "" && drf["item_desc"].ToString() == "")
                    {
                        found = "Y";
                    }
                }
                if (found == "")
                {
                    DataRow dr = dtDtlGrid.NewRow();
                    dtDtlGrid.Rows.Add(dr);
                }
                dgPODetailsDtl.DataSource = dtDtlGrid;
                dgPODetailsDtl.DataBind();
            }
            else
            {
                getEmptyDtl();
            }
            ShowFooterTotal();
        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(), "ale", "alert('Your session is over. Try it again!!');", true);
        }
    }
    private void ShowFooterTotal()
    {
        GridViewRow row = new GridViewRow(0, 0, DataControlRowType.Footer, DataControlRowState.Normal);
        TableCell cell;
        int j = 0;
        if (dgPODetailsDtl.Columns[0].Visible == true)
        {
            j = dgPODetailsDtl.Columns.Count - 4;
        }
        else if (dgPODetailsDtl.Columns[0].Visible == false)
        {
            j = dgPODetailsDtl.Columns.Count - 5;
        }

        for (int i = 1; i < j; i++)
        {
            cell = new TableCell();
            cell.Text = "";
            row.Cells.Add(cell);
        }

        decimal ctot = decimal.Zero;
        decimal QntyTotal = decimal.Zero;
        if (ViewState["reqdtl"] != null)
        {
            DataTable dt = (DataTable)ViewState["reqdtl"];
            foreach (DataRow drp in dt.Rows)
            {
                if (drp["ID"].ToString() != "" && drp["item_rate"].ToString() != "" && drp["qnty"].ToString() != "")
                {
                    ctot += decimal.Parse(drp["item_rate"].ToString()) * decimal.Parse(drp["qnty"].ToString());
                    QntyTotal += decimal.Parse(drp["qnty"].ToString());
                }
            }
        }
        cell = new TableCell();
        cell.Text = "Total";
        row.Cells.Add(cell);
        cell = new TableCell();
        cell.Text = QntyTotal.ToString("N1");
        row.Cells.Add(cell);
        cell = new TableCell();
        priceDr = GetTotal();
        //cell.Text = ctot.ToString("N2");
        //cell.HorizontalAlign = HorizontalAlign.Right;
        //row.Cells.Add(cell);
        row.Font.Bold = true;
        row.BackColor = System.Drawing.Color.LightGray;
        if (dgPODetailsDtl.Rows.Count > 0)
        {
            dgPODetailsDtl.Controls[0].Controls.Add(row);
        }
    }
    private decimal GetTotal()
    {
        decimal ctot = decimal.Zero;
        decimal QntyTotal = decimal.Zero;
        if (ViewState["reqdtl"] != null)
        {
            DataTable dt = (DataTable)ViewState["reqdtl"];
            foreach (DataRow drp in dt.Rows)
            {
                if (drp["ID"].ToString() != "" && drp["item_rate"].ToString() != "" && drp["qnty"].ToString() != "")
                {
                    ctot += decimal.Parse(drp["item_rate"].ToString()) * decimal.Parse(drp["qnty"].ToString());
                    QntyTotal += decimal.Parse(drp["qnty"].ToString());
                }
            }
        }
        return ctot;
    }
    protected void txtQnty_TextChanged(object sender, EventArgs e)
    {
        GridViewRow gvr = (GridViewRow)((TextBox)sender).NamingContainer;
        DataTable dt = (DataTable)ViewState["reqdtl"];
        // DataTable dt = ItemManager.GetItems(((TextBox)gvr.FindControl("txtItemDesc")).Text);
        if (dt.Rows.Count > 0)
        {
            DataRow dr = dt.Rows[gvr.DataItemIndex];
            dr["ID"] = dr["ID"].ToString();
            dr["item_desc"] = dr["item_desc"].ToString();
            dr["item_code"] = dr["item_code"].ToString();
            dr["msr_unit_code"] = dr["msr_unit_code"].ToString();
            dr["item_rate"] = ((TextBox)gvr.FindControl("txtItemRate")).Text;
            dr["qnty"] = ((TextBox)gvr.FindControl("txtQnty")).Text;

        }
        string found = "";
        foreach (DataRow drd in dt.Rows)
        {
            if ((drd["ID"].ToString() == "" || drd["ID"].ToString() == "0") && drd["item_desc"].ToString() == "")
            {
                found = "Y";
            }
        }
        if (found == "")
        {
            DataRow drd = dt.NewRow();
            dt.Rows.Add(drd);
        }
        dgPODetailsDtl.DataSource = dt;
        dgPODetailsDtl.DataBind();
        ShowFooterTotal();
        // ((TextBox)gvr.FindControl("txtItemRate")).Focus();        
        ((TextBox)dgPODetailsDtl.Rows[dgPODetailsDtl.Rows.Count - 1].FindControl("txtItemDesc")).Focus();
        PVIesms_UP.Update();

    }
    public DataTable PopulateMeasure()
    {
        dtmsr = ItemManager.GetMeasure();
        DataRow dr = dtmsr.NewRow();
        dtmsr.Rows.InsertAt(dr, 0);
        return dtmsr;
    }
    protected void btnNew_Click(object sender, EventArgs e)
    {
        
        txtOrderNo.Text = "00000" + IdManager.GetNextID("ItemRequisitionMst", "Id");
        txtPoDate.Enabled = ddlManufacturedBy.Enabled =txtDelDate.Enabled =  true;
        txtPONo.Text =  "";
        ddlManufacturedBy.SelectedIndex = -1;
        lbLId.Text = "";
        dgPOrderMst.Visible = false;
        DataTable dt = RequisitionOrderManager.GetRequisitionInfo("");
        dgPOrderMst.DataSource = dt;
        dgPOrderMst.DataBind();
        getEmptyDtl();
        txtDelDate.Text = txtPoDate.Text = Convert.ToDateTime(Session["date"]).ToString("dd/MM/yyyy");
        btnNew.Visible = Panel1.Visible = false;
        ddlManufacturedBy.Focus();
        txtPONo.Text = IdManager.GetDateTimeWiseSerial("ID", "ID", "[ItemRequisitionMst]");
        btnSaveinf.Visible = btnSaveinf.Enabled = true;
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            if (string.IsNullOrEmpty(txtPoDate.Text))
            {
                ClientScript.RegisterStartupScript(this.GetType(), "Warning", "alert('Input  Date.!!');", true);
                return;
            }
            if (string.IsNullOrEmpty(ddlManufacturedBy.SelectedItem.Text))
            {
                ClientScript.RegisterStartupScript(this.GetType(), "Warning", "alert('Select Requisition By.!!');", true);
                return;
            }

            RequisitionOrder pomst = RequisitionOrderManager.GetPurchaseOrderMst(lbLId.Text);
            if (pomst != null)
            {
                //  if (per.AllowEdit == "Y")
                {
                    if (!string.IsNullOrEmpty(pomst.AuthorizedDate))
                    {
                        ClientScript.RegisterStartupScript(this.GetType(), "Warning", "alert('Manafacture  Requisition Already Authorized..!!');", true);
                        return;
                    }
                    pomst.Code = txtPONo.Text;
                    pomst.Date = txtPoDate.Text;
                    pomst.RequisitionBy = ddlManufacturedBy.SelectedValue;
                    try
                    {
                        pomst.LoginBy = Session["userID"].ToString();
                    }
                    catch
                    {
                        ClientScript.RegisterStartupScript(this.GetType(), "Warning", "alert('Session Out Please Login Again..!!');", true);
                        return;
                    }
                    DataTable dt = (DataTable)ViewState["reqdtl"];
                    RequisitionOrderManager.UpdateRequisitionOrder(pomst, dt);
                    ClientScript.RegisterStartupScript(this.GetType(), "Warning", "alert('Record has been update successfully...!!');", true);
                    RefreshAll();

                }
                //else
                //{
                //    ClientScript.RegisterStartupScript(this.GetType(), "ale", "alert('You are not Permitted this Step...!!');", true);
                //}

            }
            else
            {
                // if (per.AllowAdd == "Y")
                {
                    pomst = new RequisitionOrder();
                   
                    pomst.Date = txtPoDate.Text;
                    pomst.RequisitionBy = ddlManufacturedBy.SelectedValue;

                    txtPONo.Text = IdManager.GetDateTimeWiseSerial("ID", "ID", "[ItemRequisitionMst]");
                    pomst.Code = txtPONo.Text;
                    try
                    {
                        pomst.LoginBy = Session["userID"].ToString();
                    }
                    catch
                    {
                        ClientScript.RegisterStartupScript(this.GetType(), "Warning", "alert('Session Out Please Login Again..!!');", true);
                        return;
                    }
                    DataTable dt = (DataTable)ViewState["reqdtl"];

                    string flag = "Y";
                   
                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                             if (dr["item_desc"].ToString() != "")
                            {                                
                                flag = "N";
                            }

                        }
                    }
                    if (flag == "Y")
                    {
                        ClientScript.RegisterStartupScript(this.GetType(), "Warning", "alert('Please Select Item First...!!');", true);
                        return;

                    }

                    RequisitionOrderManager.SaveRequisitionOrder(pomst, dt);
                    ClientScript.RegisterStartupScript(this.GetType(), "Warning", "alert('Record has been saved successfully...!!');", true);
                    lbLId.Text = IdManager.GetShowSingleValueIntNotParameter("top(1)[ID]", "[ItemPurOrderMst] order by [ID] desc").ToString();
                    RefreshAll();
                    
                }
                //else
                //{
                //    ClientScript.RegisterStartupScript(this.GetType(), "ale", "alert('You are not Permitted this Step...!!');", true);
                //}
            }
        }
        catch (Exception ex)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "Warning", "alert('"+ex.Message+"');", true);

        }
        btnSaveinf.Enabled = true;
    }
    protected void Delete_Click(object sender, EventArgs e)
    {

        if (per.AllowDelete == "Y")
        {
            RequisitionOrder pomst = RequisitionOrderManager.GetPurchaseOrderMst(lbLId.Text);
            if (pomst != null)
            {
                if (!string.IsNullOrEmpty(pomst.AuthorizedDate))
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "Warning", "alert('Manafacture  Requisition Already Authorized..!!');", true);
                    return;
                }

                if (pomst.Status != "C")
                {
                    pomst.LoginBy = Session["userID"].ToString();

                    RequisitionOrderManager.DeleteRequisitionOrder(pomst);

                    ClientScript.RegisterStartupScript(this.GetType(), "ale", "alert('Record is/are delete Sucessfully.!!');", true);
                    RefreshAll();
                }
            }
        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(), "ale", "alert('You are not Permitted this Step...!!');", true);
        }
    }
    protected void Clear_Click(object sender, EventArgs e)
    {
        Response.Redirect("frmManafactureRequisition.aspx?mno=0.0");
    }

    private void RefreshAll()
    {
        txtOrderNo.Text ="";
        lbLId.Text = "";
        txtPONo.Enabled = txtPoDate.Enabled = ddlManufacturedBy.Enabled =  txtDelDate.Enabled =  true;
        txtPONo.Text = lbLId.Text = txtPoDate.Text = txtDelDate.Text = "";
        ddlManufacturedBy.SelectedIndex = -1;
        //btnSaveinf.Visible = false;
        dgPOrderMst.Visible = true;
        ViewState["reqdtl"] = null;
        dgPODetailsDtl.DataSource = null;
        dgPODetailsDtl.DataBind();
       // btnSaveinf.Enabled = false;
        btnNew.Visible = false;
        getEmptyDtl();
        DataTable dt = RequisitionOrderManager.GetRequisitionInfo("");
        dgPOrderMst.DataSource = dt;
        dgPOrderMst.DataBind();
        txtPONo.Text = IdManager.GetDateTimeWiseSerial("ID", "ID", "[ItemRequisitionMst]");
        ddlManufacturedBy.Focus();
        txtDelDate.Text = System.DateTime.Now.Date.ToString("dd/MM/yyyy");
        txtPoDate.Text = System.DateTime.Now.Date.ToString("dd/MM/yyyy");
    }
    protected void ddlSupplier_SelectedIndexChanged(object sender, EventArgs e)
    {
        //txtSupplierPhone.Text = IdManager.GetShowSingleValueString("Mobile", "ID", "Supplier", ddlManufacturedBy.SelectedValue);
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "Remarks()", true);
    }
    protected void dgPOrderMst_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow | e.Row.RowType == DataControlRowType.Header | e.Row.RowType == DataControlRowType.Footer)
            {
                e.Row.Cells[1].Attributes.Add("style", "display:none");
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
    protected void dgPOrderMst_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        dgPOrderMst.DataSource = ViewState["reor"];
        dgPOrderMst.PageIndex = e.NewPageIndex;
        dgPOrderMst.DataBind();
    }
    protected void dgPOrderMst_SelectedIndexChanged(object sender, EventArgs e)
    {
        
        txtPoDate.Enabled = ddlManufacturedBy.Enabled =  txtDelDate.Enabled =  true;
        RequisitionOrder pomst = RequisitionOrderManager.GetPurchaseOrderMst(dgPOrderMst.SelectedRow.Cells[1].Text);
        if (pomst != null)
        {
            lbLId.Text = pomst.ID;
            txtPONo.Text = pomst.Code;
            txtPoDate.Text = pomst.Date;
            ddlManufacturedBy.SelectedValue = pomst.RequisitionBy;
            //txtSupplierPhone.Text = IdManager.GetShowSingleValueString("Mobile", "ID", "Supplier", ddlManufacturedBy.SelectedValue);
            //txtTermsOfDelivery.Text = pomst.TermsOfDelivery;
            //txtTermsOfPayment.Text = pomst.TermsOfPayment;
            
            //ddlReqStatus.SelectedValue = pomst.OrderStatus;
          
            DataTable dt = RequisitionOrderManager.GetRequisitionOrderItemsDetails(dgPOrderMst.SelectedRow.Cells[1].Text);
            dgPODetailsDtl.DataSource = dt;
            ViewState["reqdtl"] = dt;
            dgPODetailsDtl.DataBind();
            ShowFooterTotal();
            //tabVch.Visible = true;
            dgPOrderMst.Visible = btnNew.Visible = false;
            //pnlVch.Visible = true;
            PVI_UP.Update();
            PVIesms_UP.Update();

            txtPoDate.Enabled = ddlManufacturedBy.Enabled = txtDelDate.Enabled = true;
          
            dgPOrderMst.Visible = false;
          
            //btnNew.Visible = Panel1.Visible = false;

            btnSaveinf.Visible = btnSaveinf.Enabled = true;
        }
    }
    //*************************  Check Popup  *******************//


    protected void btnClientSave_Click(object sender, EventArgs e)
    {

        if (txtvalue.Text == "")
        {
            ClientScript.RegisterStartupScript(this.GetType(), "ale", "alert('Enter Supplier Name..!!');", true);
            Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", "LoadModalDiv()", true);
            return;
        }
        if (ddlType.SelectedValue == "S")
        {
            string IdGlCoa = "";
            Supplier sup = new Supplier();
            sup.ComName = ""; sup.SupAddr1 = ""; sup.SupName = txtvalue.Text;
            sup.SupAddr2 = ""; sup.Designation = ""; sup.City = "";
            sup.SupMobile = txtMobile.Text; sup.State = ""; sup.SupPhone = "";
            sup.PostCode = ""; sup.Fax = ""; sup.Country = "";
            sup.Email = txtEmail.Text; sup.SupGroup = "3"; sup.Active = "True";
            sup.SupCode = IdManager.GetNextID("supplier", "Code").ToString().PadLeft(7, '0');
            sup.LoginBy = Session["userID"].ToString();
            //  IdGlCoa = IdManager.getAutoIdWithParameter("402", "GL_SEG_COA", "SEG_COA_CODE", "4020000", "0000", "4");
            sup.GlCoa = IdGlCoa;
            SupplierManager.CreateSupplier(sup);
            // Gl_COA(IdGlCoa);
            string queryLoc = "select '' ID,'' ContactName  union select ID ,ContactName from Supplier order by 1";
            util.PopulationDropDownList(ddlManufacturedBy, "Supplier", queryLoc, "ContactName", "ID");
            PVI_UP.Update();
        }
        txtvalue.Text = txtMobile.Text = txtEmail.Text = "";
        //ddlType.SelectedIndex = -1;
    }

    protected void btnPrint_Click(object sender, EventArgs e)
    {
        getPurchaseOrder();
    }

    public void getPurchaseOrder()
    {
        string filename = "RE_" + txtPONo.Text;
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
        gif.ScalePercent(30f);

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
        cell = new PdfPCell(new Phrase("Requisition Order(REO)", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 10, iTextSharp.text.Font.BOLD)));
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
        cell = new PdfPCell(FormatHeaderPhrase("Requisition By:"));
        cell.BorderWidth = 0f;
        cell.FixedHeight = 20f;
        pdtclient.AddCell(cell);
        cell = new PdfPCell(FormatPhrase(": " + ddlManufacturedBy.SelectedItem.Text));
        cell.BorderWidth = 0f;
        cell.FixedHeight = 20f;
        cell.Colspan = 3;
        pdtclient.AddCell(cell);
        cell = new PdfPCell(FormatHeaderPhrase("Phone Number :"));
        cell.BorderWidth = 0f;
        cell.FixedHeight = 20f;
        pdtclient.AddCell(cell);
        cell = new PdfPCell(FormatPhrase(": " ));
        cell.BorderWidth = 0f;
        cell.FixedHeight = 20f;
        cell.Colspan = 3;
        pdtclient.AddCell(cell);
        cell = new PdfPCell(FormatHeaderPhrase(""));
        // cell = new PdfPCell(FormatHeaderPhrase("Delivery Date"));
        cell.BorderWidth = 0f;
        cell.FixedHeight = 20f;
        pdtclient.AddCell(cell);
        //cell = new PdfPCell(FormatPhrase(": " + txtDelDate.Text));
        cell = new PdfPCell(FormatHeaderPhrase(""));
        cell.BorderWidth = 0f;
        cell.FixedHeight = 20f;
        cell.Colspan = 3;
        pdtclient.AddCell(cell);
      
        PdfPTable pdtpur = new PdfPTable(2);
        pdtpur.WidthPercentage = 100;
        cell = new PdfPCell(FormatHeaderPhrase("Requisition No."));
        cell.BorderWidth = 0f;
        cell.FixedHeight = 20f;
        pdtpur.AddCell(cell);
        cell = new PdfPCell(FormatPhrase(": " + txtPONo.Text));
        cell.BorderWidth = 0f;
        cell.FixedHeight = 20f;
        pdtpur.AddCell(cell);
        cell = new PdfPCell(FormatHeaderPhrase("Date"));
        cell.BorderWidth = 0f;
        cell.FixedHeight = 20f;
        pdtpur.AddCell(cell);
        cell = new PdfPCell(FormatPhrase(": " + txtPoDate.Text));
        cell.BorderWidth = 0f;
        cell.FixedHeight = 20f;
        pdtpur.AddCell(cell);
      
        cell = new PdfPCell(FormatHeaderPhrase(""));
        cell.BorderWidth = 0f;
        cell.FixedHeight = 20f;
        pdtpur.AddCell(cell);
        cell = new PdfPCell(FormatPhrase("" ));
        cell.BorderWidth = 0f;
        cell.FixedHeight = 20f;
        pdtpur.AddCell(cell);


        cell = new PdfPCell(pdtclient);
        cell.BorderWidth = 0f;
        pdtm.AddCell(cell);
        cell = new PdfPCell(pdtpur);
        cell.BorderWidth = 0f;
        pdtm.AddCell(cell);
        document.Add(pdtm);

        //document.Add(dtempty);       
        float[] widthdtl = new float[5] { 12, 25, 50,  20, 20 };
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
        //cell = new PdfPCell(FormatHeaderPhrase("Rate"));
        //cell.HorizontalAlignment = 1;
        //cell.VerticalAlignment = 1;
        //cell.FixedHeight = 20f;
        //cell.BorderColor = BaseColor.LIGHT_GRAY;
        //pdtdtl.AddCell(cell);
        cell = new PdfPCell(FormatHeaderPhrase("Quantity"));
        cell.HorizontalAlignment = 1;
        cell.VerticalAlignment = 1;
        cell.FixedHeight = 20f;
        cell.BorderColor = BaseColor.LIGHT_GRAY;
        pdtdtl.AddCell(cell);
        //cell = new PdfPCell(FormatHeaderPhrase("Amount"));
        //cell.HorizontalAlignment = 2;
        //cell.VerticalAlignment = 1;
        //cell.FixedHeight = 20f;
        //cell.BorderColor = BaseColor.LIGHT_GRAY;
        //pdtdtl.AddCell(cell);
        //DataTable dt = (DataTable)ViewState["reqdtl"];
        DataTable dt = RequisitionOrderManager.GetRequisitionOrderItemsDetails(lbLId.Text);
        int Serial = 1;
        decimal totQty = 0;
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

                cell = new PdfPCell(FormatPhrase(dr["UMO"].ToString()));
                cell.HorizontalAlignment = 1;
                cell.VerticalAlignment = 1;
                cell.BorderColor = BaseColor.LIGHT_GRAY;
                pdtdtl.AddCell(cell);

                //cell = new PdfPCell(FormatPhrase(dr["item_rate"].ToString()));
                //cell.HorizontalAlignment = 1;
                //cell.VerticalAlignment = 1;
                //cell.BorderColor = BaseColor.LIGHT_GRAY;
                //pdtdtl.AddCell(cell);

                cell = new PdfPCell(FormatPhrase(dr["qnty"].ToString()));
                cell.HorizontalAlignment = 2;
                cell.VerticalAlignment = 1;
                cell.BorderColor = BaseColor.LIGHT_GRAY;
                pdtdtl.AddCell(cell);

                //cell = new PdfPCell(FormatPhrase(dr["Total"].ToString()));
                //cell.HorizontalAlignment = 2;
                //cell.VerticalAlignment = 1;
                //cell.BorderColor = BaseColor.LIGHT_GRAY;
                //pdtdtl.AddCell(cell);
                //if (Convert.ToDecimal(dr["ID"]) != 0)
                //{
                //tot += Convert.ToDecimal(dr["Total"]);
                //}
                //if (Convert.ToDecimal(dr["ID"]) != 0)
                //{
                totQty += Convert.ToDecimal(dr["qnty"]);
                //}
            }
        }

        cell = new PdfPCell(FormatPhrase("Total"));
        cell.FixedHeight = 20f;
        cell.HorizontalAlignment = 2;
        cell.VerticalAlignment = 1;
        cell.BorderColor = BaseColor.LIGHT_GRAY;
        cell.Colspan = 4;
        pdtdtl.AddCell(cell);

        cell = new PdfPCell(FormatPhrase(totQty.ToString("N2")));
        // cell.BorderWidth = 0f;
        cell.FixedHeight = 20f;
        cell.HorizontalAlignment = 2;
        cell.VerticalAlignment = 1;
        cell.BorderColor = BaseColor.LIGHT_GRAY;
        pdtdtl.AddCell(cell);

        //cell = new PdfPCell(FormatPhrase(tot.ToString("N2")));
        ////cell.BorderWidth = 0f;
        //cell.FixedHeight = 20f;
        //cell.HorizontalAlignment = 2;
        //cell.VerticalAlignment = 1;
        //cell.BorderColor = BaseColor.LIGHT_GRAY;
        //pdtdtl.AddCell(cell);

        cell = new PdfPCell(FormatPhrase(""));
        //cell.BorderWidth = 0f;
        cell.FixedHeight = 10f;
        cell.HorizontalAlignment = 0;
        cell.VerticalAlignment = 1;
        cell.Border = 0;
        cell.Colspan = 5;
        pdtdtl.AddCell(cell);

        //cell = new PdfPCell(FormatPhrase("In word: " + DataManager.GetLiteralAmt(tot.ToString()).Replace("  ", " ").Replace("  ", " ")));
        ////cell = new PdfPCell(FormatHeaderPhrase("Comments :"));
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
        cell = new PdfPCell(FormatPhrase("Prepared by"));
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
        cell = new PdfPCell(FormatPhrase("Received by"));
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
    protected void lbSearch_Click(object sender, EventArgs e)
    {
        DataTable dt =RequisitionOrderManager.GetRequisitionOrderHistory(txtOrderNo.Text, "", txtFromDate.Text, txtToDAte.Text);
        if (dt.Rows.Count > 0)
        {
            dgPOrderMst.DataSource = dt;
            dgPOrderMst.DataBind();
        }
        else
        { dgPOrderMst.DataSource = null; dgPOrderMst.DataBind(); }
        PVI_UP.Update();
        PVIesms_UP.Update();
    }
    protected void lbClear_Click(object sender, EventArgs e)
    {
        txtOrderNo.Text =  txtFromDate.Text = txtToDAte.Text =  string.Empty;
        DataTable dt = RequisitionOrderManager.GetRequisitionInfo("");
        ViewState["reor"] = dt;
        dgPOrderMst.DataSource = dt;
        dgPOrderMst.DataBind();
        PVI_UP.Update();
        PVIesms_UP.Update();
    }
    protected void txtSearchSuplier_TextChanged(object sender, EventArgs e)
    {
        // int ID = IdManager.GetShowSingleValueInt("ID", " Supplier where Upper(Code+'-'+ContactName+'-'+Mobile)=upper('" + txtSearchSuplier.Text + "')");
        DataTable Dt = IdManager.GetShowDataTable("Select ID,Code,ContactName,Mobile from Supplier where Upper(Code+'-'+ContactName+'-'+Mobile)=upper('')");
        if (Dt.Rows.Count > 0)
        {
           // lblSupplierID.Text = Dt.Rows[0]["ID"].ToString();
          //  txtSearchSuplier.Text = Dt.Rows[0]["ContactName"].ToString();
        }
        else
        {
           // lblSupplierID.Text = "";
           // txtSearchSuplier.Text = "";
        }
        PVI_UP.Update();
        PVIesms_UP.Update();
    }
}