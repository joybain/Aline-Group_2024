using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using OldColor;
using autouniv;
using Microsoft.Reporting.WebForms;

public partial class frmMaterialRequisition : System.Web.UI.Page
{
    public int userLvl = 0;
    private static Permis per;
    MeterialRequistitonManager _meterialMng = new MeterialRequistitonManager();
    MeterialRequistitonModel _meterialMdl = new MeterialRequistitonModel();
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
        }
    }

    private void Refresh()
    {
       
        ddlProject.DataSource = MeterialRequistitonManager.GetProject();
        ddlProject.DataTextField = "ProjectName";
        ddlProject.DataValueField = "ID";
        ddlProject.DataBind();
        ddlProject.Items.Insert(0, "");  
     
        Session["purdtl"] = null;
        btnNew.Enabled = true;
        btnSave.Enabled = false;
        btnSave.Visible = false;
        ddlProject.Focus();

        var Data = _meterialMng.GateData();
        dgRequistionHistory.DataSource = Data;
        dgRequistionHistory.DataBind();
        ViewState["History"] = Data;

        hidnId.Value = null;
        txtPresentDate.Text = txtRecomment.Text = txtRequisitionNo.Text = txtSite.Text = "";
      
    }

    public DataTable PopulateMeasure()
    {
        dtmsr = MeterialRequistitonManager.GetMeasure();
        DataRow dr = dtmsr.NewRow();
        dtmsr.Rows.InsertAt(dr, 0);
        return dtmsr;
    }
    protected void btnNew_Click(object sender, EventArgs e)
    {
        txtRecomment.Enabled = ddlProject.Enabled = txtPresentDate.Enabled = true; btnSave.Visible = btnSave.Enabled = true;
        lbAuth.Visible = false;
        txtRequisitionNo.Text = IdManager.GetDateTimeWiseSerial("MR-", "ID", "[MaterialRequisitionMst]");
       txtPresentDate.Text= Convert.ToDateTime(Session["date"]).ToString("dd/MM/yyyy");
       dgRequistionHistory.Visible = false;
       getRequisitionDtl();
        dgMaterial.Enabled = false;


    }
    protected void txtDesctiption_TextChanged(object sender, EventArgs e)
    {  GridViewRow gvr = (GridViewRow)((TextBox)sender).NamingContainer;      
        DataTable dtdtl = (DataTable)Session["purdtl"];
        DataRow dr = dtdtl.Rows[gvr.DataItemIndex];        
        DataTable dt = MeterialRequistitonManager.GetMaterialItem(((TextBox)gvr.FindControl("txtDesctiption")).Text,ddlProject.SelectedValue);
        if (dt.Rows.Count > 0)
        {
           
                string flag = "";
                foreach (DataRow Dr2 in dtdtl.Rows)
                {
                    if (Dr2["ID"].ToString() == dt.Rows[0]["ID"].ToString())
                    {


                        flag = "Y";
                        ((TextBox)dgMaterial.Rows[gvr.DataItemIndex].FindControl("txtDesctiption")).Focus();
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
                    dr["Previous"] = ((DataRow)dt.Rows[0])["Previous"].ToString();
                    dr["SupplidQnt"] = ((DataRow)dt.Rows[0])["SupplidQnt"].ToString();
                    dr["PresentStock"] = ((DataRow)dt.Rows[0])["PresentStock"].ToString();
                    //dr["PSRequirement"] = ((DataRow)dt.Rows[0])["PSRequirement"].ToString();
                    dr["Remarks"] = ((DataRow)dt.Rows[0])["Remarks"].ToString();
                    dtdtl.Rows.InsertAt(dr, gvr.DataItemIndex);
                }

                dgMaterial.DataSource = dtdtl;
                dgMaterial.DataBind();
                //ShowFooterTotal();
                ((TextBox)dgMaterial.Rows[gvr.DataItemIndex].FindControl("txtThisTimeRequisition")).Focus();
                MRIesms_UP.Update();
            }      
    }
    private void getRequisitionDtl()
    {
        dgMaterial.Visible = true;
        DataTable dtDtlGrid = new DataTable();
        dtDtlGrid.Columns.Add("ID", typeof(string));
        dtDtlGrid.Columns.Add("item_code", typeof(string));
        dtDtlGrid.Columns.Add("item_desc", typeof(string));
        dtDtlGrid.Columns.Add("msr_unit_code", typeof(string));     
        dtDtlGrid.Columns.Add("This_time_Requisition", typeof(string));
        dtDtlGrid.Columns.Add("Previous", typeof(string));
        //dtDtlGrid.Columns.Add("Total", typeof(string));
        dtDtlGrid.Columns.Add("SupplidQnt", typeof(string));
        dtDtlGrid.Columns.Add("PresentStock", typeof(string));
        //dtDtlGrid.Columns.Add("PSRequirement", typeof(string));
        dtDtlGrid.Columns.Add("Remarks", typeof(string));
        dtDtlGrid.Columns.Add("check", typeof(string));
 
        DataRow dr = dtDtlGrid.NewRow();
        dtDtlGrid.Rows.Add(dr);
        dgMaterial.DataSource = dtDtlGrid;
        Session["purdtl"] = dtDtlGrid;
        dgMaterial.DataBind();
    }
    protected void txtThisTimeRequisition_TextChanged(object sender, EventArgs e)
    {
        GridViewRow gvr = (GridViewRow)((TextBox)sender).NamingContainer;
        DataTable dtdtl = (DataTable)Session["purdtl"];
        if (dtdtl.Rows.Count > 0)
        {

            DataRow dr = dtdtl.Rows[gvr.DataItemIndex];
            dr["ID"] = dr["ID"].ToString();
            dr["item_desc"] = dr["item_desc"].ToString();
            dr["item_code"] = dr["item_code"].ToString();
            dr["msr_unit_code"] = dr["msr_unit_code"].ToString();
            dr["SupplidQnt"] = dr["SupplidQnt"].ToString();
            dr["PresentStock"] = dr["PresentStock"].ToString();            
            dr["Remarks"] = dr["Remarks"].ToString();
            dr["Previous"] = ((Label)gvr.FindControl("lblPreviousRequisition")).Text;
            dr["This_time_Requisition"] = ((TextBox)gvr.FindControl("txtThisTimeRequisition")).Text;

        }
        string found = "";
        foreach (DataRow drd in dtdtl.Rows)
        {
            if (drd["ID"].ToString() == "" && drd["item_desc"].ToString() == "")
            {
                found = "Y";
            }
        }
        if (found == "")
        {
            DataRow drd = dtdtl.NewRow();
            dtdtl.Rows.Add(drd);
        }
        dgMaterial.DataSource = dtdtl;
        dgMaterial.DataBind();
        //ShowFooterTotal();
        // ((TextBox)gvr.FindControl("txtItemRate")).Focus();        
        ((TextBox)dgMaterial.Rows[dgMaterial.Rows.Count - 1].FindControl("txtDesctiption")).Focus();
        MRIesms_UP.Update();
        
    }
    protected void dgMaterial_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (((DataRowView)e.Row.DataItem)["This_time_Requisition"].ToString() != "" && ((DataRowView)e.Row.DataItem)["Previous"].ToString() != "")
                {
                    decimal total = decimal.Parse(((DataRowView)e.Row.DataItem)["Previous"].ToString()) +
                                   decimal.Parse(((DataRowView)e.Row.DataItem)["This_time_Requisition"].ToString());

                    
                    ((Label)e.Row.FindControl("lblTotalRequisition")).Text = total.ToString("N2");
                    decimal psr = decimal.Parse(((DataRowView)e.Row.DataItem)["Previous"].ToString()) +
                                   decimal.Parse(((DataRowView)e.Row.DataItem)["This_time_Requisition"].ToString()) -(decimal.Parse(((DataRowView)e.Row.DataItem)["SupplidQnt"].ToString()) + decimal.Parse(((DataRowView)e.Row.DataItem)["PresentStock"].ToString()));

                    ((Label)e.Row.FindControl("lblPSRequirement")).Text = psr.ToString("N2");
                }   
                e.Row.Cells[2].Attributes.Add("style", "display:none");
                e.Row.Cells[1].Attributes.Add("style", "display:none");
                e.Row.Cells[12].Attributes.Add("style", "display:none");
            }
            else if (e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.Cells[1].Attributes.Add("style", "display:none");
               e.Row.Cells[2].Attributes.Add("style", "display:none");
                e.Row.Cells[12].Attributes.Add("style", "display:none");
            }
            else if (e.Row.RowType == DataControlRowType.Footer)
            {
                e.Row.Cells[1].Attributes.Add("style", "display:none");
                e.Row.Cells[2].Attributes.Add("style", "display:none");
                e.Row.Cells[12].Attributes.Add("style", "display:none");
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

    protected void dgMaterial_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        if (Session["purdtl"] != null)
        {
            DataTable dtdtl = (DataTable)Session["purdtl"];
            dtdtl.Rows.RemoveAt(dgMaterial.Rows[e.RowIndex].RowIndex);
            if (dtdtl.Rows.Count > 0)
            {
                
                dgMaterial.DataSource = dtdtl;
                Session["purdtl"] = dtdtl;
                dgMaterial.DataBind();
            }
            else
            {
                getRequisitionDtl();
            }
            //ShowFooterTotal();
        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(), "ale", "alert('Your session is over. Try it again!!');", true);
        }
    }
    protected void btnClear_Click(object sender, EventArgs e)
    {
        Response.Redirect("frmMaterialRequisition.aspx?mno=2.19");
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        
        if (string.IsNullOrEmpty(hidnId.Value))
        {
            if (string.IsNullOrEmpty(txtPresentDate.Text))
            {
                ClientScript.RegisterStartupScript(this.GetType(), "Warning", "alert('Input Order Date.!!');", true);
                return;
            }
            if (string.IsNullOrEmpty(ddlProject.SelectedItem.Text))
            {
                ClientScript.RegisterStartupScript(this.GetType(), "Warning", "alert('Select Project Name.!!');", true);
                return;
            }
            _meterialMdl.ProjectId = Convert.ToInt32(ddlProject.SelectedValue);
            _meterialMdl.RequisitionNo = txtRequisitionNo.Text;
            _meterialMdl.Address = txtSite.Text.Replace("'", "’");
            _meterialMdl.Date = txtPresentDate.Text.Replace("'", "’");
            _meterialMdl.Recoment = txtRecomment.Text.Replace("'", "’");
            DataTable dt = (DataTable)Session["purdtl"];
            MeterialRequistitonManager.Save(_meterialMdl, dt);
            ClientScript.RegisterStartupScript(this.GetType(), "Warning", "alert('Record has been saved successfully...!!');", true);
            Refresh();
          
        }
        else
        {
           
            _meterialMdl.Id = Convert.ToInt32(hidnId.Value);
            _meterialMdl.ProjectId = Convert.ToInt32(ddlProject.SelectedValue);
            _meterialMdl.RequisitionNo = txtRequisitionNo.Text;
            _meterialMdl.Address = txtSite.Text;
            _meterialMdl.Date = txtPresentDate.Text;
            _meterialMdl.Recoment = txtRecomment.Text.Replace("'", "’");
            DataTable dt = (DataTable)Session["purdtl"];
            MeterialRequistitonManager.Update(_meterialMdl, dt);
            ClientScript.RegisterStartupScript(this.GetType(), "Warning", "alert('Record has been Update successfully...!!');", true);      
            Refresh();
            Response.Redirect("frmMaterialRequisition.aspx?mno=0.0");
        }
    }
    protected void ddlProject_SelectedIndexChanged(object sender, EventArgs e)
    {     
        txtSite.Text = IdManager.GetShowSingleValueString("Address", "ID", "Project_Setup_Tbl", ddlProject.SelectedValue);
        dgMaterial.Enabled = true;
    }

    protected void dgRequistionHistory_SelectedIndexChanged(object sender, EventArgs e)
    {
         txtRecomment.Enabled = true;

        hidnId.Value = dgRequistionHistory.SelectedRow.Cells[2].Text.Trim();
        DataTable dt = MeterialRequistitonManager.GetPurchaseMaterialMst(hidnId.Value);

        if (dt != null)
        {

            txtRequisitionNo.Text = dt.Rows[0]["RequisitionNo"].ToString();
            txtPresentDate.Text = dt.Rows[0]["RequisitionDate"].ToString();
            txtSite.Text = dt.Rows[0]["Address"].ToString();
            txtRecomment.Text = dt.Rows[0]["Recoment"].ToString();
            ddlProject.SelectedValue = dt.Rows[0]["ProjectId"].ToString();
            btnSave.Visible = btnSave.Enabled = true;
            DataTable dt1 = MeterialRequistitonManager.GetMetrialsDetails(hidnId.Value);
            dgMaterial.DataSource = dt1;
            Session["purdtl"] = dt1;
            ViewState["purdtl"] = dt1;
            dgMaterial.DataBind();
            btnNew.Enabled = btnNew.Visible = false;
            dgRequistionHistory.Visible = false;
            ddlProject.Enabled = true;
        }
    }
    protected void dgRequistionHistory_RowDataBound(object sender, GridViewRowEventArgs e)
    {


        if (e.Row.RowType == DataControlRowType.DataRow )
        {
            CheckBox cb = e.Row.FindControl("chkSelect") as CheckBox;


            if (e.Row.Cells[9].Text.Equals("U"))
            {

                cb.Visible = true;
            }
            else
            {

                cb.Visible = false;
            }
            e.Row.Cells[2].Attributes.Add("style", "display:none");
            e.Row.Cells[9].Attributes.Add("style", "display:none");
            if (e.Row.Cells[10].Text.Equals("U"))
            {
                e.Row.Cells[10].ForeColor = System.Drawing.Color.Red;

            }
            else
            {
                e.Row.Cells[10].ForeColor = System.Drawing.Color.Green;
            }
        }
        
       else  if (e.Row.RowType == DataControlRowType.Header)
        {
            e.Row.Cells[2].Attributes.Add("style", "display:none");
            e.Row.Cells[9].Attributes.Add("style", "display:none");
            if (e.Row.Cells[10].Text.Equals("U"))
            {
                e.Row.Cells[10].ForeColor = System.Drawing.Color.Red;

            }
            else
            {
                e.Row.Cells[10].ForeColor = System.Drawing.Color.Green;
            }
        }
        else if (e.Row.RowType == DataControlRowType.Footer)
        {
            e.Row.Cells[2].Attributes.Add("style", "display:none");
            e.Row.Cells[9].Attributes.Add("style", "display:none");
            if (e.Row.Cells[10].Text.Equals("U"))
            {
                e.Row.Cells[10].ForeColor = System.Drawing.Color.Red;

            }
            else
            {
                e.Row.Cells[9].ForeColor = System.Drawing.Color.Green;
            }
        }
    }
    protected void btnPrint_Click(object sender, EventArgs e)
    {
        try
        {
                
                DataTable data = MeterialRequistitonManager.GetMterialMst(hidnId.Value);
                DataTable data1 = MeterialRequistitonManager.GetMterialDtl(hidnId.Value);

                if (data.Rows.Count > 0)
                {

                    RequisitionReport.LocalReport.DataSources.Clear();
                    ReportDataSource rds = new ReportDataSource("DataSet2", data);
                    ReportDataSource rds1 = new ReportDataSource("DataSet1", data1);
                    RequisitionReport.LocalReport.ReportPath = Server.MapPath("MaterialRequisitionReport.rdlc");
                    RequisitionReport.LocalReport.DataSources.Add(rds);
                    RequisitionReport.LocalReport.DataSources.Add(rds1);
                    RequisitionReport.LocalReport.Refresh();
                }
            }        
        catch
        {
            ClientScript.RegisterStartupScript(this.GetType(), "ale", "alert('No Data...!!');", true);
        }
    }
    protected void txtGrvRemarks_TextChanged(object sender, EventArgs e)
    {
        GridViewRow gvr = (GridViewRow)((TextBox)sender).NamingContainer;
        DataTable dtdtl = (DataTable)Session["purdtl"];
        if (dtdtl.Rows.Count > 0)
        {
            DataRow dr = dtdtl.Rows[gvr.DataItemIndex];
            dr["ID"] = dr["ID"].ToString();
            dr["item_desc"] = dr["item_desc"].ToString();
            dr["item_code"] = dr["item_code"].ToString();
            dr["msr_unit_code"] = dr["msr_unit_code"].ToString();
            dr["SupplidQnt"] = dr["SupplidQnt"].ToString();
            dr["PresentStock"] = dr["PresentStock"].ToString();

            dr["Previous"] = dr["Previous"].ToString();
            dr["This_time_Requisition"] = dr["This_time_Requisition"].ToString();
            dr["Remarks"] = ((TextBox)gvr.FindControl("txtGrvRemarks")).Text;

        }
        string found = "";
        foreach (DataRow drd in dtdtl.Rows)
        {
            if (drd["ID"].ToString() == "" && drd["item_desc"].ToString() == "")
            {
                found = "Y";
            }
        }
        if (found == "")
        {
            DataRow drd = dtdtl.NewRow();
            dtdtl.Rows.Add(drd);
        }
        dgMaterial.DataSource = dtdtl;
        dgMaterial.DataBind();
        //ShowFooterTotal();
        // ((TextBox)gvr.FindControl("txtItemRate")).Focus();        
        ((TextBox)dgMaterial.Rows[dgMaterial.Rows.Count - 1].FindControl("txtDesctiption")).Focus();
        MRIesms_UP.Update();
    }
    protected void chkSelect_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            CheckBox chk = (CheckBox)sender;
            GridViewRow gvr = (GridViewRow)chk.NamingContainer;
            DataTable dtdtl = (DataTable)ViewState["purdtl"];
            DataRow dr1 = dtdtl.Rows[gvr.DataItemIndex];
            dr1.BeginEdit();
            string Message = "";
            int Count = 0;

            //Count = IdManager.GetShowSingleValueInt("Count(*)", "OrderReceivedDtl Where StockReceivedStatus='Y' and ID='" + dr1["DtlsID"].ToString() + "' and DeleteBy is null");
            if (((CheckBox)gvr.FindControl("chkSelect")).Checked == true)
            {
                dr1["check"] = "1";

            }
            else
            {

                dr1["check"] = "0";

            }
            dtdtl.AcceptChanges();

            ViewState["purdtl"] = dtdtl;

            dgMaterial.DataSource = dtdtl;
            dgMaterial.DataBind();
        }
        catch (Exception ex)
        {


            ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "jsAlert('" + ex.Message + "','red',0);", true);

        }
    }
    protected void chkSelect_CheckedChanged1(object sender, EventArgs e)
    {
        if (ViewState["History"] != null)
        {
            CheckBox chk = (CheckBox)sender;
            GridViewRow gvr = (GridViewRow)chk.NamingContainer;
            DataTable dtdtl = (DataTable)ViewState["History"];
            DataRow dr1 = dtdtl.Rows[gvr.DataItemIndex];
            dr1.BeginEdit();

            if (((CheckBox)gvr.FindControl("chkSelect")).Checked == true)
            {
                dr1["check"] = "1";

            }
            else
            {



                dr1["check"] = "0";

            }
            dtdtl.AcceptChanges();

            ViewState["History"] = dtdtl;

            dgRequistionHistory.DataSource = dtdtl;
            dgRequistionHistory.DataBind();
        }
    }

    protected void LoginBtn_Click(object sender, EventArgs e)
    {
        string connectionString = DataManager.OraConnString();
        SqlDataReader dReader;
        SqlConnection conn = new SqlConnection();
        conn.ConnectionString = connectionString;
        SqlCommand cmd = new SqlCommand();
        cmd.Connection = conn;
        cmd.CommandType = CommandType.Text;
        cmd.CommandText =
            "Select ID,password,user_grp,description,EMP_NO,BranchID from [utl_userinfo] where upper(user_name)=upper('" +
            loginId.Text.Trim() + "') and status='A'";
        conn.Open();
        dReader = cmd.ExecuteReader();
        if (dReader.HasRows == true)
        {
            while (dReader.Read())

                if (pwd.Text != "" && pwd.Text.Trim() == dReader["password"].ToString())
                {
                    Session["POrderMstId"] = "";
                    Session["user"] = loginId.Text;
                    Session["pass"] = pwd.Text;
                    Session["userID"] = dReader["ID"].ToString();
                    Session["EMP_NO"] = dReader["EMP_NO"].ToString();
                    Session["EMPNO"] = dReader["EMP_NO"].ToString();
                    userLvl = int.Parse(dReader["user_grp"].ToString());
                    Session["userlevel"] = userLvl.ToString();
                    Session["book"] = "AMB";
                    Session["Branch"] = dReader["BranchID"].ToString();
                    ViewState["user_grp"] = dReader["user_grp"].ToString();
                    string wnote = dReader["description"].ToString();
                    Session["wnote"] = wnote;
                    cmd = new SqlCommand();
                    cmd.Connection = conn;
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText =
                        "Select book_desc,company_address1,company_address2,separator_type,ShortName from [gl_set_of_books] where book_name='" +
                        Session["book"].ToString() + "' ";
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
                    clsSession ses = clsSessionManager.getLoginSession(loginId.Text.ToUpper(), "");
                    if (ses == null)
                    {
                        ses = new clsSession();
                        ses.UserId = loginId.Text.ToUpper();
                        ses.SessionTime = System.DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss");
                        ses.SessionId = Session.SessionID;

                        ses.Mac = Server.HtmlEncode(Request.UserHostAddress);
                        clsSessionManager.CreateSession(ses);
                    }

                    if (ViewState["user_grp"].ToString() == "14" || ViewState["user_grp"].ToString() == "4" || ViewState["user_grp"].ToString() == "17")
                    {
                        DataTable dtdtl = (DataTable)ViewState["History"];
                        
                        foreach (DataRow dr in dtdtl.Rows)
                        {
                            if (dr["check"].ToString() == "1" && dr["ID"].ToString() != "")
                            {
                                _meterialMng.UpdateApproveStatusAuthorise(Session["userID"].ToString(), dr["ID"].ToString());
                            }
                            
                        }
                        ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert",
                       "alert('Approve Success....!!!!');", true);
                        Refresh();
                        }

                        
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert",
                        "alert('Incorrect Password....!!!!');", true);
                }
        }
        else
        {
            ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert",
                "alert('Incorrect User ID ...!!!!');", true);
            Session["user"] = "";
            Session["pass"] = "";
            loginId.Focus();
        }
    }
}