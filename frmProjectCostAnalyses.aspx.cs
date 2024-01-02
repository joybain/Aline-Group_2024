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


public partial class frmProjectCostAnalyses : System.Web.UI.Page
{
    private static Permis per;
    private static DataTable dtmsr = new DataTable();
    ProposeCostPriceManager _proposeCostPriceManager=new ProposeCostPriceManager();
    ProjectCostMdl _CostMdl =new ProjectCostMdl();

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
                    cmd.CommandText =
                        "Select ID,user_grp,description from utl_userinfo where upper(user_name)=upper('" +
                        Session["user"].ToString().ToUpper() + "') and status='A'";
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
                        cmd.CommandText =
                            "Select book_desc,company_address1,company_address2,separator_type,ShortName from gl_set_of_books where book_name='" +
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
                ((Label) Page.Master.FindControl("lblLogin")).Text = Session["wnote"].ToString();
                ((LinkButton) Page.Master.FindControl("lbLogout")).Visible = true;
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
        Session["Cost_Total"] = null;
        Session["purdtl"] = null; 
        btnSave.Visible = false;
        btnNew.Visible = true;
        var data = _proposeCostPriceManager.Gatedata();
        dgProposeCostHistory.DataSource = data;
        dgProposeCostHistory.DataBind();
        // txtCostPrice.Visible = false;
        //lblCost.Visible = false;
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
        dgProjectCost.Visible = true;
        DataTable dtDtlGrid = new DataTable();
        dtDtlGrid.Columns.Add("ID", typeof (string));
        dtDtlGrid.Columns.Add("item_desc", typeof (string));
        //dtDtlGrid.Columns.Add("ItemId", typeof(string));
        dtDtlGrid.Columns.Add("item_code", typeof (string));
        dtDtlGrid.Columns.Add("msr_unit_code", typeof (string));
        //dtDtlGrid.Columns.Add("this_time_requisition_qnty", typeof(string));
        dtDtlGrid.Columns.Add("ItemRate", typeof (string));
        dtDtlGrid.Columns.Add("Qnty", typeof (string));
        dtDtlGrid.Columns.Add("Total", typeof (string));
        //dtDtlGrid.Columns.Add("total_Stock", typeof(string));
        dtDtlGrid.Columns.Add("Remarksany", typeof (string));

        DataRow dr = dtDtlGrid.NewRow();
        dtDtlGrid.Rows.Add(dr);
        dgProjectCost.DataSource = dtDtlGrid;
        Session["purdtl"] = dtDtlGrid;
        dgProjectCost.DataBind();
    }

    protected void btnNew_Click(object sender, EventArgs e)
    {
        txtDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
        btnSave.Visible = true;
        btnNew.Visible = false;
        //txtCostPrice.Visible = true;
        //lblCost.Visible = true;
        dgProposeCostHistory.Visible = false;
        GetRequisition();

    }
    protected void dgProjectCost_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (((DataRowView) e.Row.DataItem)["Qnty"].ToString() != "" &&
                    ((DataRowView) e.Row.DataItem)["ItemRate"].ToString() != "")
                {
                    decimal total = decimal.Parse(((DataRowView) e.Row.DataItem)["ItemRate"].ToString())*
                                    decimal.Parse(((DataRowView) e.Row.DataItem)["qnty"].ToString());
                    ((Label) e.Row.FindControl("lblTotal")).Text = total.ToString("N2");

                }

                e.Row.Cells[8].Attributes.Add("style", "display:none");

            }
            else if (e.Row.RowType == DataControlRowType.Header)
            {

                e.Row.Cells[8].Attributes.Add("style", "display:none");

            }
            else if (e.Row.RowType == DataControlRowType.Footer)
            {

                e.Row.Cells[8].Attributes.Add("style", "display:none");

            }
        }

        catch (FormatException fex)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "Warning", "alert('" + fex.Message + "');", true);
        }
        catch (Exception ex)
        {
            if (ex.Message.Contains("Database"))
                ClientScript.RegisterStartupScript(this.GetType(), "Warning",
                    "alert('Database Maintain Error. Contact to the Software Provider..!!');", true);
            else
                ClientScript.RegisterStartupScript(this.GetType(), "Warning",
                    "alert('There is some problem to do the task. Try again properly.!!');", true);
        }
    }
    protected void txtItemName_TextChanged(object sender, EventArgs e)
    {
        try
        {
            GridViewRow gvr = (GridViewRow)((TextBox)sender).NamingContainer;
            DataTable dtdtl = (DataTable)Session["purdtl"];
            DataRow dr = dtdtl.Rows[gvr.DataItemIndex];
            DataTable dt = ProposeCostPriceManager.GetMaterialItem(((TextBox)gvr.FindControl("txtItemName")).Text);
            if (dt.Rows.Count > 0)
            {

                string flag = "";
                foreach (DataRow Dr2 in dtdtl.Rows)
                {
                    if (Dr2["ID"].ToString() == dt.Rows[0]["ID"].ToString())
                    {


                        flag = "Y";
                        ((TextBox)dgProjectCost.Rows[gvr.DataItemIndex].FindControl("txtItemName")).Focus();
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
                    dr["ItemRate"] = ((DataRow)dt.Rows[0])["ItemRate"].ToString();
                    dr["Qnty"] = ((DataRow)dt.Rows[0])["Qnty"].ToString();
                    dr["Total"] = ((DataRow)dt.Rows[0])["Total"].ToString();                   
                    dr["Remarksany"] = ((DataRow)dt.Rows[0])["Remarksany"].ToString();
                    dtdtl.Rows.InsertAt(dr, gvr.DataItemIndex);
                }
                dgProjectCost.DataSource = dtdtl;
                dgProjectCost.DataBind();
                //ShowFooterTotal();
                ((TextBox)dgProjectCost.Rows[gvr.DataItemIndex - 1].FindControl("txtRate")).Focus();
                MRIesms_UP.Update();
            }
        }

        catch (Exception ex)
        {
            ExceptionLogging.SendExcepToDB(ex);
        }
    }
    protected void txtRate_TextChanged(object sender, EventArgs e)
    {
        try
        {
            GridViewRow gvr = (GridViewRow)((TextBox)sender).NamingContainer;
            DataTable dt = (DataTable)Session["purdtl"];
            if (dt.Rows.Count > 0)
            {
                DataRow dr = dt.Rows[gvr.DataItemIndex];
                dr["ID"] = dr["ID"].ToString();
                dr["item_desc"] = dr["item_desc"].ToString();
                dr["item_code"] = dr["item_code"].ToString();
                dr["msr_unit_code"] = dr["msr_unit_code"].ToString();
                dr["ItemRate"] = ((TextBox)gvr.FindControl("txtRate")).Text;
                if (((TextBox)gvr.FindControl("txtqnty")).Text == "") { dr["qnty"] = "0"; }
                dr["Qnty"] = ((TextBox)gvr.FindControl("txtqnty")).Text;

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
            dgProjectCost.DataSource = dt;
            dgProjectCost.DataBind();
            ShowFooterTotal();
            ((TextBox)dgProjectCost.Rows[dgProjectCost.Rows.Count - 2].FindControl("txtqnty")).Focus();
        }
        catch (Exception ex)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "Warning", "alert('" + ex + "');", true);
        }
    }
    private void ShowFooterTotal()
    {
        decimal ctot = decimal.Zero;
        decimal totAddi = 0;
        decimal totRat = 0;
        decimal totQty = 0;
        decimal totItemsP = 0;
        decimal totA = 0;
        decimal Total = 0;

        if (Session["purdtl"] != null)
        {
            DataTable dt = (DataTable) Session["purdtl"];
            foreach (DataRow drp in dt.Rows)
            {
                if (drp["ID"].ToString() != "" && drp["ItemRate"].ToString() != "" && drp["Qnty"].ToString() != "")
                {
                    totRat += decimal.Parse(drp["ItemRate"].ToString());
                    totQty += decimal.Parse(drp["Qnty"].ToString());
                    totItemsP += decimal.Parse(drp["ItemRate"].ToString()) * decimal.Parse(drp["Qnty"].ToString());
                   

                    //totAddi += (totItemsP * decimal.Parse(drp["Additional"].ToString())) / 100;
                    Total += decimal.Parse(drp["ItemRate"].ToString()) * decimal.Parse(drp["Qnty"].ToString());
                    
                }
            }
                      
        }
        GridViewRow row = new GridViewRow(0, 0, DataControlRowType.Footer, DataControlRowState.Normal);
        TableCell cell;
        cell = new TableCell();
        cell.Text = "Total";
        cell.ColumnSpan = 4;
        cell.HorizontalAlign = HorizontalAlign.Right;
        row.Cells.Add(cell);
        cell = new TableCell();
        cell.Text = "";
        cell.HorizontalAlign = HorizontalAlign.Right;
        row.Cells.Add(cell);
        cell = new TableCell();
        cell.Text = totQty.ToString("N2");
        cell.HorizontalAlign = HorizontalAlign.Right;
        row.Cells.Add(cell);
        //cell = new TableCell();
        //cell.Text = totItemsP.ToString("N0");
        //cell.HorizontalAlign = HorizontalAlign.Right;
        //row.Cells.Add(cell);
        //cell = new TableCell();
        //cell.Text = totA.ToString("N2");
        //cell.HorizontalAlign = HorizontalAlign.Right;
        //row.Cells.Add(cell);
        cell = new TableCell();
       
        cell.Text = Total.ToString("N0");
        cell.HorizontalAlign = HorizontalAlign.Right;
        row.Cells.Add(cell);
        row.Font.Bold = true;
        row.BackColor = System.Drawing.Color.LightGray;
        if (dgProjectCost.Rows.Count > 0)
        {
            dgProjectCost.Controls[0].Controls.Add(row);
        }
        string totalvalu=Total.ToString("N0");
        var total = totalvalu;
        Session["Cost_Total"] = total;
        txtCostPrice.Text = total;




    }
    protected void txtqnty_TextChanged(object sender, EventArgs e)
    {
        try
        {
            GridViewRow gvr = (GridViewRow)((TextBox)sender).NamingContainer;
            DataTable dt = (DataTable)Session["purdtl"];
            if (dt.Rows.Count > 0)
            {
                DataRow dr = dt.Rows[gvr.DataItemIndex];
                dr["ID"] = dr["ID"].ToString();
                dr["item_desc"] = dr["item_desc"].ToString();
                dr["item_code"] = dr["item_code"].ToString();
                dr["msr_unit_code"] = dr["msr_unit_code"].ToString();
                dr["ItemRate"] = ((TextBox)gvr.FindControl("txtRate")).Text;
                if (((TextBox)gvr.FindControl("txtqnty")).Text == "")
                { dr["Qnty"] = "0"; }
                dr["Qnty"] = ((TextBox)gvr.FindControl("txtqnty")).Text;

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
            dgProjectCost.DataSource = dt;
            dgProjectCost.DataBind();
            ShowFooterTotal();
            ((TextBox)dgProjectCost.Rows[dgProjectCost.Rows.Count - 1].FindControl("txtItemName")).Focus();
        }
        catch (Exception ex)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "Warning", "alert('" + ex + "');", true);
        }
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(HiId.Value))
        {
            if (string.IsNullOrEmpty(txtDate.Text))
            {
                ClientScript.RegisterStartupScript(this.GetType(), "Warning", "alert('Input  Date.!!');", true);
                return;
            }


            _CostMdl.ProjectName = txtProjectName.Text.Replace("'", "’");
            _CostMdl.Date = txtDate.Text.Replace("'", "’");
            _CostMdl.CostPrice = Session["Cost_Total"].ToString();
            DataTable dt = (DataTable)Session["purdtl"];
            ProposeCostPriceManager.ProjectCostSave(dt, _CostMdl);
            ClientScript.RegisterStartupScript(this.GetType(), "Warning", "alert('Record has been Save successfully...!!');", true);
            Refresh();
        }
        else
        {
            _CostMdl.Id = Convert.ToInt32(HiId.Value);
            _CostMdl.ProjectName = txtProjectName.Text.Replace("'", "’");
            _CostMdl.Date = txtDate.Text;
           
            if (Session["Cost_Total"]==null)
            {
                _CostMdl.CostPrice = txtCostPrice.Text;
            }
            else
            {
                _CostMdl.CostPrice = Session["Cost_Total"].ToString();
            }
           
            DataTable dt = (DataTable)Session["purdtl"];
            ProposeCostPriceManager.ProjectCostUpdate(dt, _CostMdl);
            ClientScript.RegisterStartupScript(this.GetType(), "Warning", "alert('Record has been Update successfully...!!');", true);
            Refresh();
        }

       
    }
    protected void btnClear_Click(object sender, EventArgs e)
    {
        Response.Redirect("frmProjectCostAnalyses.aspx?mno=0.9");
    }
    protected void dgProposeCostHistory_SelectedIndexChanged(object sender, EventArgs e)
    {
        HiId.Value = dgProposeCostHistory.SelectedRow.Cells[1].Text;
        DataTable dt = ProposeCostPriceManager.GetProjectCostMst(HiId.Value);

        if (dt != null)
        {

            txtProjectName.Text = dt.Rows[0]["ProjectName"].ToString();
            txtDate.Text = dt.Rows[0]["Date"].ToString();
            txtCostPrice.Text = dt.Rows[0]["CostPrice"].ToString();
            btnSave.Visible = btnSave.Enabled = true;
            txtCostPrice.Visible = lblCost.Visible = true;
            DataTable dt1 = ProposeCostPriceManager.GetProjectCostDtl(HiId.Value);
            DataRow drd = dt1.NewRow();
            dt1.Rows.Add(drd);
            dgProjectCost.DataSource = dt1;
            ViewState["purdtl"] = dt1;
            Session["purdtl"] = dt1;
            dgProjectCost.DataBind();
            btnNew.Enabled = btnNew.Visible = false;
            dgProposeCostHistory.Visible = false;           
            dgProjectCost.Visible = true;
        }
    }
    protected void dgProposeCostHistory_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow | e.Row.RowType == DataControlRowType.Header | e.Row.RowType == DataControlRowType.Footer)
        {
            e.Row.Cells[1].Attributes.Add("style", "display:none");         
        }
    }
    protected void btnPrint_Click(object sender, EventArgs e)
    {
        getProjectCost();
    }
    public void getProjectCost()
    {
        string filename = "PC_" + txtProjectName.Text;
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
        cell = new PdfPCell(new Phrase("Propose Project Cost Analyses", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 10, iTextSharp.text.Font.BOLD)));
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
        cell = new PdfPCell(FormatHeaderPhrase("Project Name"));
        cell.BorderWidth = 0f;
        cell.FixedHeight = 20f;
        pdtclient.AddCell(cell);
        cell = new PdfPCell(FormatPhrase(": " + txtProjectName.Text));
        cell.BorderWidth = 0f;
        cell.FixedHeight = 20f;
        cell.Colspan = 3;
        pdtclient.AddCell(cell);
        cell = new PdfPCell(FormatHeaderPhrase(""));
        cell.BorderWidth = 0f;
        cell.FixedHeight = 20f;
        pdtclient.AddCell(cell);
        cell = new PdfPCell(FormatPhrase(""));
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
        cell = new PdfPCell(FormatHeaderPhrase("Date"));
        cell.BorderWidth = 0f;
        cell.FixedHeight = 20f;
        pdtpur.AddCell(cell);
        cell = new PdfPCell(FormatPhrase(": " + txtDate.Text));
        cell.BorderWidth = 0f;
        cell.FixedHeight = 20f;
        pdtpur.AddCell(cell);
        cell = new PdfPCell(FormatHeaderPhrase("Project Total Cost"));
        cell.BorderWidth = 0f;
        cell.FixedHeight = 20f;
        pdtpur.AddCell(cell);
        cell = new PdfPCell(FormatPhrase(": " + txtCostPrice.Text+ " " + "Taka Only"));
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
        float[] widthdtl = new float[6] { 12, 50, 15, 25, 25, 25 };
        PdfPTable pdtdtl = new PdfPTable(widthdtl);
        pdtdtl.WidthPercentage = 100;

        cell = new PdfPCell(FormatHeaderPhrase("Serial"));
        cell.HorizontalAlignment = 1;
        cell.VerticalAlignment = 1;
        cell.FixedHeight = 20f;
        cell.BorderColor = BaseColor.LIGHT_GRAY;
        pdtdtl.AddCell(cell);
        cell = new PdfPCell(FormatHeaderPhrase("Description"));
        cell.HorizontalAlignment = 1;
        cell.VerticalAlignment = 1;
        cell.FixedHeight = 20f;
        cell.BorderColor = BaseColor.LIGHT_GRAY;
        pdtdtl.AddCell(cell);
        cell = new PdfPCell(FormatHeaderPhrase("Unit"));
        cell.HorizontalAlignment =1 ;
        cell.VerticalAlignment = 1;
        cell.FixedHeight = 20f;
        cell.BorderColor = BaseColor.LIGHT_GRAY;
        pdtdtl.AddCell(cell);

        cell = new PdfPCell(FormatHeaderPhrase("Proposel Rate"));
        cell.HorizontalAlignment = 1;
        cell.VerticalAlignment = 1;
        cell.FixedHeight = 20f;
        cell.BorderColor = BaseColor.LIGHT_GRAY;
        pdtdtl.AddCell(cell);

        cell = new PdfPCell(FormatHeaderPhrase("Proposel Qnty"));
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
        cell = new PdfPCell(FormatHeaderPhrase("Total"));
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
        DataTable dt = ProposeCostPriceManager.GetProjectCostDtl(HiId.Value);
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

                cell = new PdfPCell(FormatPhrase(dr["item_desc"].ToString()));
                cell.HorizontalAlignment = 1;
                cell.VerticalAlignment = 1;
                cell.BorderColor = BaseColor.LIGHT_GRAY;
                pdtdtl.AddCell(cell);

                cell = new PdfPCell(FormatPhrase(dr["UOM"].ToString()));
                cell.HorizontalAlignment = 1;
                cell.VerticalAlignment = 1;
                cell.BorderColor = BaseColor.LIGHT_GRAY;
                pdtdtl.AddCell(cell);

                cell = new PdfPCell(FormatPhrase(dr["ItemRate"].ToString()));
                cell.HorizontalAlignment = 1;
                cell.VerticalAlignment = 1;
                cell.BorderColor = BaseColor.LIGHT_GRAY;
                pdtdtl.AddCell(cell);


                cell = new PdfPCell(FormatPhrase(dr["Qnty"].ToString()));
                cell.HorizontalAlignment = 1;
                cell.VerticalAlignment = 1;
                cell.BorderColor = BaseColor.LIGHT_GRAY;
                pdtdtl.AddCell(cell);

                //cell = new PdfPCell(FormatPhrase(dr["item_rate"].ToString()));
                //cell.HorizontalAlignment = 1;
                //cell.VerticalAlignment = 1;
                //cell.BorderColor = BaseColor.LIGHT_GRAY;
                //pdtdtl.AddCell(cell);

                cell = new PdfPCell(FormatPhrase(dr["Total"].ToString()));
                cell.HorizontalAlignment = 1;
                cell.VerticalAlignment = 1;
                cell.BorderColor = BaseColor.LIGHT_GRAY;
                pdtdtl.AddCell(cell);


                //if (Convert.ToDecimal(dr["ID"]) != 0)
                //{
                tot += Convert.ToDecimal(dr["Total"]);
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
        cell.Colspan = 5;
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

        cell = new PdfPCell(FormatPhrase(""));
        //cell.BorderWidth = 0f;
        cell.FixedHeight = 10f;
        cell.HorizontalAlignment = 0;
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
}