using System;
//using System.Activities.Statements;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using autouniv;

public partial class frmDistributionRecived : System.Web.UI.Page
{
    private static Permis per;
    DistrubationModel _distrubationMdl = new DistrubationModel();
    private static DataTable dtmsr = new DataTable();
    private distrubationManager _distrubation = new distrubationManager();
    distrubationRecivedManager _distrubationRecived=new distrubationRecivedManager();
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
        DataTable dt = _distrubationRecived.getProjectStock();
        gdiProjectStockHistory.DataSource = dt;
        ViewState["STK"] = dt;
        gdiProjectStockHistory.DataBind();
        ShowFooterProjectStockTotal();

        ddlProjectName.DataSource = MeterialRequistitonManager.GetProject();
        ddlProjectName.DataTextField = "ProjectName";
        ddlProjectName.DataValueField = "ID";
        ddlProjectName.DataBind();
        ddlProjectName.Items.Insert(0, "");

        ddlSerchProjectName.DataSource = MeterialRequistitonManager.GetProject();
        ddlSerchProjectName.DataTextField = "ProjectName";
        ddlSerchProjectName.DataValueField = "ID";
        ddlSerchProjectName.DataBind();
        ddlSerchProjectName.Items.Insert(0, "");

        Label1.Visible = HistoryFildSet.Visible=false;
       
      txtRecivedDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
      TabContainer1.ActiveTabIndex = 2;

        if (TabContainer1.ActiveTabIndex == 2)
        {
            //DataTable dt1 = _distrubation.RecivedMstall();
           //dgHistory.DataSource = dt1;
           //dgHistory.DataBind();
            Label1.Visible = true;
            HistoryFildSet.Visible = true;
        }
    }

    protected void btnSerch_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(ddlProjectName.SelectedValue))
        {
            ClientScript.RegisterStartupScript(this.GetType(), "Warning", "alert('Input Project Name.!!');", true);
            return;
        }

        DataTable dt = _distrubation.RecivedMst(ddlProjectName.SelectedValue);
        dgHistory.DataSource = dt;
        dgHistory.DataBind();
        Label1.Visible = true;
        HistoryFildSet.Visible = true;
    }
    protected void ddlProjectName_SelectedIndexChanged(object sender, EventArgs e)
    {
        txtSiteName.Text = IdManager.GetShowSingleValueString("Address", "ID", "Project_Setup_Tbl", ddlProjectName.SelectedValue);  
    }

    protected void dgHistory_SelectedIndexChanged(object sender, EventArgs e)
    {
        hidMstId.Value = dgHistory.SelectedRow.Cells[1].Text;
        DataTable dt1 = distrubationRecivedManager.MasterRecivedData(hidMstId.Value);
        if (dt1 != null)
        {

            DataRow drd = dt1.NewRow();
            dt1.Rows.Add(drd);
            dgDistributionRecived.DataSource = dt1;
            ViewState["purdtl"] = dt1;
            Session["purdtl"] = dt1;
            dgDistributionRecived.DataBind();
            ShowFooterTotal();
            TabContainer1.ActiveTabIndex = 1;

            DataTable dt2 = distrubationRecivedManager.MasterRecivedvalu(hidMstId.Value);
            if (dt2 != null)
            {
                txtMatrialNo.Text = dt2.Rows[0]["Requisition_Code"].ToString();
                txtOrderNumber.Text = dt2.Rows[0]["PO"].ToString();
                txtTotalDueRequ.Text = dt2.Rows[0]["DueQnty"].ToString();
                txtTotalTotalRequ.Text = dt2.Rows[0]["TotalRequisition"].ToString(); 
            }
           
        }
    }

    protected void btnExcel0_Click(object sender, EventArgs e)
    {
        Response.Redirect("frmDistributionRecived.aspx?mno=0.10");
    }
    private void ShowFooterTotal()
    {
        try
        {
            decimal ctot = decimal.Zero;
            decimal totAddi = 0;
            decimal totalStock = 0;
            decimal TotalRequrment = 0;
            decimal totItemsP = 0;
            decimal totQty = 0;
            decimal Total = 0;

            if (Session["purdtl"] != null)
            {
                DataTable dt = (DataTable)Session["purdtl"];
                foreach (DataRow drp in dt.Rows)
                {
                    if (drp["item_desc"].ToString() != "" && drp["item_code"].ToString() != "")
                    {
                        //totalStock += decimal.Parse(drp["total_Stock"].ToString());
                        totQty += decimal.Parse(drp["present_qnty"].ToString());
                        //totQty += decimal.Parse(drp["TransferQty"].ToString());
                        // totItemsP += decimal.Parse(drp["item_rate"].ToString()) * decimal.Parse(drp["qnty"].ToString());
                        // totA += decimal.Parse(drp["ExpireDate"].ToString());

                        //totAddi += (totItemsP * decimal.Parse(drp["Additional"].ToString())) / 100;
                        // Total += decimal.Parse(drp["item_rate"].ToString()) * decimal.Parse(drp["qnty"].ToString()); ;
                    }
                }

            }
            GridViewRow row = new GridViewRow(0, 0, DataControlRowType.Footer, DataControlRowState.Normal);
            TableCell cell;
            cell = new TableCell();
            cell.Text = "Total";
            cell.ColumnSpan = 2;
            cell.HorizontalAlign = HorizontalAlign.Right;
            row.Cells.Add(cell);
            //cell = new TableCell();
            //cell.Text = "";
            //cell.HorizontalAlign = HorizontalAlign.Right;
            //row.Cells.Add(cell);
            cell = new TableCell();

            cell.Text = totQty.ToString("N2");
            cell.HorizontalAlign = HorizontalAlign.Right;
            row.Cells.Add(cell);

            cell = new TableCell();

            //cell.Text = totalStock.ToString("N0");
            //cell.HorizontalAlign = HorizontalAlign.Right;
            //row.Cells.Add(cell);
            //lblQuantity.Text = totalStock.ToString("N0");
            ////cell = new TableCell();
            ////cell.Text = totItemsP.ToString("N0");
            ////cell.HorizontalAlign = HorizontalAlign.Right;
            ////row.Cells.Add(cell);
            ////cell = new TableCell();
            ////cell.Text = totA.ToString("N2");
            ////cell.HorizontalAlign = HorizontalAlign.Right;
            ////row.Cells.Add(cell);
            //cell = new TableCell();
            //// cell.ColumnSpan = 2;
           

           ;
            // cell.ColumnSpan = 2;
            cell.Text = "";
            cell.HorizontalAlign = HorizontalAlign.Right;
            row.Cells.Add(cell);
            row.Font.Bold = true;
            row.BackColor = System.Drawing.Color.LightGray;
            if (dgDistributionRecived.Rows.Count > 0)
            {
                dgDistributionRecived.Controls[0].Controls.Add(row);
            }

        }

        catch (FormatException fex)
        {
            ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "jsAlert('" + fex.Message + "','red',0);", true);

        }
        catch (Exception ex)
        {
            if (ex.Message.Contains("Database"))
                ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "jsAlert('Database Maintain Error. Contact to the Software Provider..!!','red',0);", true);
            else ;
            ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "jsAlert('There is some problem to do the task. Try again properly.!!','red',0);", true);
        }

    }
    public DataTable PopulateMeasure()
    {
        dtmsr = MeterialRequistitonManager.GetMeasure();
        DataRow dr = dtmsr.NewRow();
        dtmsr.Rows.InsertAt(dr, 0);
        return dtmsr;
    }
    private void GetRequisition()
    {
        dgDistributionRecived.Visible = true;
        DataTable dtDtlGrid = new DataTable();
        dtDtlGrid.Columns.Add("ID", typeof(string));
        dtDtlGrid.Columns.Add("item_desc", typeof(string));
        dtDtlGrid.Columns.Add("check", typeof(int));
        dtDtlGrid.Columns.Add("item_code", typeof(string));
        dtDtlGrid.Columns.Add("msr_unit_code", typeof(string));
        dtDtlGrid.Columns.Add("present_qnty", typeof(string));
        dtDtlGrid.Columns.Add("Due_qnty", typeof(string));
        dtDtlGrid.Columns.Add("Remarks", typeof(string));

        DataRow dr = dtDtlGrid.NewRow();
        dtDtlGrid.Rows.Add(dr);
        dgDistributionRecived.DataSource = dtDtlGrid;
        Session["purdtl"] = dtDtlGrid;
        dgDistributionRecived.DataBind();
    }
    protected void dgDistributionRecived_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow | e.Row.RowType == DataControlRowType.Header | e.Row.RowType == DataControlRowType.Footer)
        {
            e.Row.Cells[1].Attributes.Add("style", "display:none");
            //  e.Row.Cells[7].Attributes.Add("style", "display:none");
        }
    }
    protected void dgHistory_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow | e.Row.RowType == DataControlRowType.Header | e.Row.RowType == DataControlRowType.Footer)
        {
            e.Row.Cells[1].Attributes.Add("style", "display:none");
            if (e.Row.Cells[7].Text.Equals("Not Received"))
            {
                e.Row.Cells[7].ForeColor = System.Drawing.Color.Red;

            }
            else
            {
                e.Row.Cells[7].ForeColor = System.Drawing.Color.Green;
            }
        }

    }
    protected void btnRecive_Click(object sender, EventArgs e)
    {
        DataTable data = _distrubationRecived.AllredyRecived(hidMstId.Value);
        if (data.Rows.Count > 0)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "Warning", "alert('Allready Recived This Items.!!');", true);
            return;
        }
        
        if (String.IsNullOrEmpty(ddlProjectName.SelectedValue))
        {
            ClientScript.RegisterStartupScript(this.GetType(), "Warning", "alert('Please Selected Project Name');", true);
            return;
        }
        else
        {
            _distrubationMdl.ProjectId = Convert.ToInt32(ddlProjectName.SelectedValue);
        }
        DataTable Code = _distrubationRecived.TranseferCode(hidMstId.Value);
        _distrubationMdl.TranseferCode = Code.Rows[0]["Code"].ToString();

        _distrubationMdl.RequisitionID = hidMstId.Value;
        _distrubationMdl.LoginBy = Session["userID"].ToString();
        _distrubationMdl.ReciveDate = txtRecivedDate.Text;
        DataTable dt = (DataTable)ViewState["purdtl"];
        distrubationRecivedManager.distrubationSave(dt, _distrubationMdl);
        ClientScript.RegisterStartupScript(this.GetType(), "Warning", "alert('Record has been Save successfully...!!');", true);
        Refresh();
        Response.Redirect("frmDistributionRecived.aspx?mno=0.10");
    }
    protected void btnSerchProjectStock_Click(object sender, EventArgs e)
    {
        DataTable dt = _distrubationRecived.getProjectStockSerch(ddlSerchProjectName.SelectedValue);
        gdiProjectStockHistory.DataSource = dt;
        ViewState["STK"] = dt;
        gdiProjectStockHistory.DataBind();
        ShowFooterProjectStockTotal();
    }
    protected void btnProjectStockclear_Click(object sender, EventArgs e)
    {
        DataTable dt = _distrubationRecived.getProjectStock();
        gdiProjectStockHistory.DataSource = dt;
        ViewState["STK"] = dt;
        gdiProjectStockHistory.DataBind();
        ShowFooterProjectStockTotal();

        ddlSerchProjectName.DataSource = MeterialRequistitonManager.GetProject();
        ddlSerchProjectName.DataTextField = "ProjectName";
        ddlSerchProjectName.DataValueField = "ID";
        ddlSerchProjectName.DataBind();
        ddlSerchProjectName.Items.Insert(0, "");
      
        TabContainer1.ActiveTabIndex = 2;
    }
    private void ShowFooterProjectStockTotal()
    {
        try
        {
            if (gdiProjectStockHistory.Rows.Count > 0)
            {
                GridViewRow row = new GridViewRow(0, 0, DataControlRowType.Footer, DataControlRowState.Normal);
                TableCell cell;
                DataTable dtdtl = (DataTable)ViewState["STK"];
                double totQty = 0, totPrice = 0, TotSFT = 0, TotSQM = 0;
                foreach (DataRow dr in dtdtl.Rows)
                {
                    if (!string.IsNullOrEmpty(dr["Qnty"].ToString()))
                    {
                        totQty += Convert.ToDouble(dr["Qnty"]);//Convert.ToDouble(dr["ClosingStock"]);

                        //TotSFT += Convert.ToDouble(dr["SFTClosingStock"]);
                        //TotSQM += Convert.ToDouble(dr["SQMClosingStock"]);
                    }
                }
                cell = new TableCell();
                cell.Text = "Total";
                cell.ColumnSpan = 2;
                cell.HorizontalAlign = HorizontalAlign.Right;
                row.Cells.Add(cell);

                cell = new TableCell();
                cell.Text = totQty.ToString("N2");
                cell.HorizontalAlign = HorizontalAlign.Center;
                row.Cells.Add(cell);
                row.Font.Bold = true;

                //cell = new TableCell();
                //cell.Text = TotSFT.ToString("N2");
                //cell.HorizontalAlign = HorizontalAlign.Center;
                //row.Cells.Add(cell);
                //row.Font.Bold = true;

                //cell = new TableCell();
                //cell.Text = TotSQM.ToString("N2");
                //cell.HorizontalAlign = HorizontalAlign.Center;
                //row.Cells.Add(cell);
                //row.Font.Bold = true;

                //cell = new TableCell();
                //cell.Text = "";
                //cell.HorizontalAlign = HorizontalAlign.Right;
                //row.Cells.Add(cell);
                //cell.ColumnSpan = 2;
                //row.Font.Bold = true;
                gdiProjectStockHistory.Controls[0].Controls.Add(row);
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
    protected void BtnReset_Click(object sender, EventArgs e)
    {
        Response.Redirect("frmDistributionRecived.aspx?mno=0.10");
    }
    protected void gdiProjectStockHistory_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow | e.Row.RowType == DataControlRowType.Header | e.Row.RowType == DataControlRowType.Footer)
        {
            e.Row.Cells[0].Attributes.Add("style", "display:none");
            //  e.Row.Cells[7].Attributes.Add("style", "display:none");
        }
    }

    protected void gdiProjectStockHistory_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gdiProjectStockHistory.DataSource = ViewState["STK"];
        gdiProjectStockHistory.PageIndex = e.NewPageIndex;
        gdiProjectStockHistory.DataBind();

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

            dgDistributionRecived.DataSource = dtdtl;
            dgDistributionRecived.DataBind();
        }
        catch (Exception ex)
        {


            ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "jsAlert('" + ex.Message + "','red',0);", true);

        }
    }
}