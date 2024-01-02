using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using autouniv;
using OldColor;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.draw;
using System.Text;

public partial class frmrequisitionOrderAuthorization : System.Web.UI.Page
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

            DataTable dt = RequisitionOrderManager.GetRequisitionInfoByPanding("");
            ViewState["reor"] = dt;
            dgPOrderMst.DataSource = dt;
            dgPOrderMst.DataBind();
            ViewState["Flag"] = "";
        }
    }
    protected void lbClear_Click(object sender, EventArgs e)
    {
        txtOrderNo.Text = txtFromDate.Text = txtToDAte.Text = string.Empty;
        DataTable dt = RequisitionOrderManager.GetRequisitionInfoByPanding("");
        ViewState["reor"] = dt;
        dgPOrderMst.DataSource = dt;
        dgPOrderMst.DataBind();
        PVI_UP.Update();
        
    }
    protected void lbSearch_Click(object sender, EventArgs e)
    {
        DataTable dt = RequisitionOrderManager.GetRequisitionOrderHistorybyPanding(txtOrderNo.Text, "", txtFromDate.Text, txtToDAte.Text);
        if (dt.Rows.Count > 0)
        {
            dgPOrderMst.DataSource = dt;
            dgPOrderMst.DataBind();
        }
        else
        { dgPOrderMst.DataSource = null; dgPOrderMst.DataBind(); }
        PVI_UP.Update();
    }
    protected void dgPOrderMst_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        dgPOrderMst.DataSource = ViewState["reor"];
        dgPOrderMst.PageIndex = e.NewPageIndex;
        dgPOrderMst.DataBind();
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


    private static Phrase FormatPhrase(string value)
    {
        return new Phrase(value, FontFactory.GetFont(FontFactory.TIMES_ROMAN, 9));
    }

    private static Phrase FormatHeaderPhrase(string value)
    {
        return new Phrase(value, FontFactory.GetFont(FontFactory.TIMES_ROMAN, 10, iTextSharp.text.Font.BOLD));
    }
    protected void dgPOrderMst_SelectedIndexChanged(object sender, EventArgs e)
    {
        RequisitionOrder pomst = RequisitionOrderManager.GetPurchaseOrderMst(dgPOrderMst.SelectedRow.Cells[1].Text);

        if ( pomst!= null)
        {
             //getPurchaseOrder(pomst);
            
             ModalPopupExtender1.Show();
             DataTable dt = RequisitionOrderManager.GetRequisitionOrderItemsDetails(dgPOrderMst.SelectedRow.Cells[1].Text);
             ViewState["reqdtl"] = dt;
             dgDetails.DataSource = dt;
             dgDetails.DataBind();
            //string filename = "PO_" + pomst.Code;
            //Response.Clear();
            //Response.ContentType = "application/pdf";
            //Response.AddHeader("content-disposition", "attachment; filename=" + filename + ".pdf");
            //Document document = new Document(PageSize.A4, 10f, 10f, 10f, 30f);
            //PdfWriter writer = PdfWriter.GetInstance(document, Response.OutputStream);
            //document.Open();
            //Rectangle page = document.PageSize;
            //PdfPTable head = new PdfPTable(1);
            //head.TotalWidth = page.Width - 50;
            //Phrase phrase = new Phrase(Convert.ToDateTime(Session["date"]).ToString("dd/MM/yyyy"), new Font(Font.FontFamily.TIMES_ROMAN, 8));
            //PdfPCell c = new PdfPCell(phrase);
            //c.Border = Rectangle.NO_BORDER;
            //c.VerticalAlignment = Element.ALIGN_BOTTOM;
            //c.HorizontalAlignment = Element.ALIGN_RIGHT;
            //head.AddCell(c);
            //head.WriteSelectedRows(0, -1, 0, page.Height - document.TopMargin + head.TotalHeight + 20, writer.DirectContent);

            //PdfPCell cell;
            //byte[] logo = GlBookManager.GetGlLogo(Session["book"].ToString());
            //iTextSharp.text.Image gif = iTextSharp.text.Image.GetInstance(logo);
            //gif.Alignment = iTextSharp.text.Image.MIDDLE_ALIGN;
            //gif.ScalePercent(30f);

            //float[] titwidth = new float[2] { 10, 200 };
            //PdfPTable dth = new PdfPTable(titwidth);
            //dth.WidthPercentage = 100;

            //cell = new PdfPCell(gif);
            //cell.HorizontalAlignment = 1;
            //cell.VerticalAlignment = 1;
            //cell.Rowspan = 4;
            //cell.BorderWidth = 0f;
            //dth.AddCell(cell);
            //cell = new PdfPCell(new Phrase(Session["org"].ToString(), FontFactory.GetFont(FontFactory.TIMES_ROMAN, 12, iTextSharp.text.Font.BOLD)));
            //cell.HorizontalAlignment = 1;
            //cell.VerticalAlignment = 1;
            //cell.Colspan = 7;
            //cell.BorderWidth = 0f;
            //// cell.FixedHeight = 20f;
            //dth.AddCell(cell);
            //cell = new PdfPCell(new Phrase(Session["add1"].ToString(), FontFactory.GetFont(FontFactory.TIMES_ROMAN, 10, iTextSharp.text.Font.BOLD)));
            //cell.HorizontalAlignment = 1;
            //cell.VerticalAlignment = 1;
            //cell.Colspan = 7;
            //cell.BorderWidth = 0f;
            //// cell.FixedHeight = 20f;
            //dth.AddCell(cell);
            //cell = new PdfPCell(new Phrase(Session["add2"].ToString(), FontFactory.GetFont(FontFactory.TIMES_ROMAN, 10, iTextSharp.text.Font.BOLD)));
            //cell.HorizontalAlignment = 1;
            //cell.VerticalAlignment = 1;
            //cell.Colspan = 7;
            //cell.BorderWidth = 0f;
            //// cell.FixedHeight = 20f;
            //dth.AddCell(cell);
            //cell = new PdfPCell(new Phrase("Manufactured Requisition", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 10, iTextSharp.text.Font.BOLD)));
            //cell.HorizontalAlignment = 1;
            //cell.VerticalAlignment = 1;
            //cell.Colspan = 7;
            //cell.BorderWidth = 0f;
            //// cell.FixedHeight = 30f;
            //dth.AddCell(cell);
            //document.Add(dth);
            //LineSeparator line = new LineSeparator(1, 100, null, Element.ALIGN_CENTER, -2);
            //document.Add(line);

            //PdfPTable dtempty = new PdfPTable(1);
            //cell = new PdfPCell(FormatHeaderPhrase(""));
            //cell.BorderWidth = 0f;
            //cell.FixedHeight = 20f;
            //dtempty.AddCell(cell);
            //document.Add(dtempty);


            //PdfPTable pdtclient = new PdfPTable(4);
            //pdtclient.WidthPercentage = 100;
            

            //PdfPTable pdtpur = new PdfPTable(2);
            //pdtpur.WidthPercentage = 100;
            //cell = new PdfPCell(FormatHeaderPhrase("Order No."));
            //cell.BorderWidth = 0f;
            //cell.FixedHeight = 20f;
            //pdtpur.AddCell(cell);
            //cell = new PdfPCell(FormatPhrase(": " + pomst.Code));
            //cell.BorderWidth = 0f;
            //cell.FixedHeight = 20f;
            //pdtpur.AddCell(cell);
            //cell = new PdfPCell(FormatHeaderPhrase("Date"));
            //cell.BorderWidth = 0f;
            //cell.FixedHeight = 20f;
            //pdtpur.AddCell(cell);
            //cell = new PdfPCell(FormatPhrase(": " + pomst.Date));
            //cell.BorderWidth = 0f;
            //cell.FixedHeight = 20f;
            //pdtpur.AddCell(cell);
            //cell = new PdfPCell(FormatHeaderPhrase("Status"));
            //cell.BorderWidth = 0f;
            //cell.FixedHeight = 20f;
            //pdtpur.AddCell(cell);
            //if (String.IsNullOrEmpty(pomst.Status) || pomst.Status == "P")
            //{
            //    cell = new PdfPCell(FormatPhrase(": Pending"));
            //    cell.BorderWidth = 0f;
            //    cell.FixedHeight = 20f;
            //    pdtpur.AddCell(cell);
            //}
            //else if (pomst.Status == "A")
            //{
            //    cell = new PdfPCell(FormatPhrase(": Authorized"));
            //    cell.BorderWidth = 0f;
            //    cell.FixedHeight = 20f;
            //    pdtpur.AddCell(cell);
            //}
            //else if (pomst.Status == "D")
            //{
            //    cell = new PdfPCell(FormatPhrase(": Delivered"));
            //    cell.BorderWidth = 0f;
            //    cell.FixedHeight = 20f;
            //    pdtpur.AddCell(cell);
            //}


            
            ////document.Add(dtempty);       
            //float[] widthdtl = new float[7] { 12, 25, 50, 20, 20, 20, 25 };
            //PdfPTable pdtdtl = new PdfPTable(widthdtl);
            //pdtdtl.WidthPercentage = 100;

            //cell = new PdfPCell(FormatHeaderPhrase("Serial"));
            //cell.HorizontalAlignment = 1;
            //cell.VerticalAlignment = 1;
            //cell.FixedHeight = 20f;
            //cell.BorderColor = BaseColor.LIGHT_GRAY;
            //pdtdtl.AddCell(cell);
            //cell = new PdfPCell(FormatHeaderPhrase("Item Code"));
            //cell.HorizontalAlignment = 1;
            //cell.VerticalAlignment = 1;
            //cell.FixedHeight = 20f;
            //cell.BorderColor = BaseColor.LIGHT_GRAY;
            //pdtdtl.AddCell(cell);
            //cell = new PdfPCell(FormatHeaderPhrase("Description"));
            //cell.HorizontalAlignment = 0;
            //cell.VerticalAlignment = 1;
            //cell.FixedHeight = 20f;
            //cell.BorderColor = BaseColor.LIGHT_GRAY;
            //pdtdtl.AddCell(cell);
            //cell = new PdfPCell(FormatHeaderPhrase("Unit"));
            //cell.HorizontalAlignment = 1;
            //cell.VerticalAlignment = 1;
            //cell.FixedHeight = 20f;
            //cell.BorderColor = BaseColor.LIGHT_GRAY;
            //pdtdtl.AddCell(cell);
            //cell = new PdfPCell(FormatHeaderPhrase("Rate"));
            //cell.HorizontalAlignment = 1;
            //cell.VerticalAlignment = 1;
            //cell.FixedHeight = 20f;
            //cell.BorderColor = BaseColor.LIGHT_GRAY;
            //pdtdtl.AddCell(cell);
            //cell = new PdfPCell(FormatHeaderPhrase("Quantity"));
            //cell.HorizontalAlignment = 1;
            //cell.VerticalAlignment = 1;
            //cell.FixedHeight = 20f;
            //cell.BorderColor = BaseColor.LIGHT_GRAY;
            //pdtdtl.AddCell(cell);
            //cell = new PdfPCell(FormatHeaderPhrase("Amount"));
            //cell.HorizontalAlignment = 2;
            //cell.VerticalAlignment = 1;
            //cell.FixedHeight = 20f;
            //cell.BorderColor = BaseColor.LIGHT_GRAY;
            //pdtdtl.AddCell(cell);
            ////DataTable dt = (DataTable)Session["purdtl"];
            //int Serial = 1;
            //decimal totQty = 0;
            //decimal tot = 0;
            //foreach (DataRow dr in dt.Rows)
            //{
            //    if (Convert.ToDecimal(dr["ID"]) != 0)
            //    {
            //        cell = new PdfPCell(FormatPhrase(Serial.ToString()));
            //        cell.HorizontalAlignment = 1;
            //        cell.VerticalAlignment = 1;
            //        cell.BorderColor = BaseColor.LIGHT_GRAY;
            //        pdtdtl.AddCell(cell);
            //        Serial++;

            //        cell = new PdfPCell(FormatPhrase(dr["item_code"].ToString()));
            //        cell.HorizontalAlignment = 1;
            //        cell.VerticalAlignment = 1;
            //        cell.BorderColor = BaseColor.LIGHT_GRAY;
            //        pdtdtl.AddCell(cell);

            //        cell = new PdfPCell(FormatPhrase(dr["item_desc"].ToString()));
            //        cell.HorizontalAlignment = 0;
            //        cell.VerticalAlignment = 1;
            //        cell.BorderColor = BaseColor.LIGHT_GRAY;
            //        pdtdtl.AddCell(cell);

            //        cell = new PdfPCell(FormatPhrase(dr["UMO"].ToString()));
            //        cell.HorizontalAlignment = 1;
            //        cell.VerticalAlignment = 1;
            //        cell.BorderColor = BaseColor.LIGHT_GRAY;
            //        pdtdtl.AddCell(cell);

            //        cell = new PdfPCell(FormatPhrase(dr["item_rate"].ToString()));
            //        cell.HorizontalAlignment = 1;
            //        cell.VerticalAlignment = 1;
            //        cell.BorderColor = BaseColor.LIGHT_GRAY;
            //        pdtdtl.AddCell(cell);

            //        cell = new PdfPCell(FormatPhrase(dr["qnty"].ToString()));
            //        cell.HorizontalAlignment = 2;
            //        cell.VerticalAlignment = 1;
            //        cell.BorderColor = BaseColor.LIGHT_GRAY;
            //        pdtdtl.AddCell(cell);

            //        cell = new PdfPCell(FormatPhrase(dr["Total"].ToString()));
            //        cell.HorizontalAlignment = 2;
            //        cell.VerticalAlignment = 1;
            //        cell.BorderColor = BaseColor.LIGHT_GRAY;
            //        pdtdtl.AddCell(cell);
            //        //if (Convert.ToDecimal(dr["ID"]) != 0)
            //        //{
            //        tot += Convert.ToDecimal(dr["Total"]);
            //        //}
            //        //if (Convert.ToDecimal(dr["ID"]) != 0)
            //        //{
            //        totQty += Convert.ToDecimal(dr["qnty"]);
            //        //}
            //    }
            //}

            //cell = new PdfPCell(FormatPhrase("Total"));
            //cell.FixedHeight = 20f;
            //cell.HorizontalAlignment = 2;
            //cell.VerticalAlignment = 1;
            //cell.BorderColor = BaseColor.LIGHT_GRAY;
            //cell.Colspan = 5;
            //pdtdtl.AddCell(cell);

            //cell = new PdfPCell(FormatPhrase(totQty.ToString("N2")));
            //// cell.BorderWidth = 0f;
            //cell.FixedHeight = 20f;
            //cell.HorizontalAlignment = 2;
            //cell.VerticalAlignment = 1;
            //cell.BorderColor = BaseColor.LIGHT_GRAY;
            //pdtdtl.AddCell(cell);

            //cell = new PdfPCell(FormatPhrase(tot.ToString("N2")));
            ////cell.BorderWidth = 0f;
            //cell.FixedHeight = 20f;
            //cell.HorizontalAlignment = 2;
            //cell.VerticalAlignment = 1;
            //cell.BorderColor = BaseColor.LIGHT_GRAY;
            //pdtdtl.AddCell(cell);

            //cell = new PdfPCell(FormatPhrase(""));
            ////cell.BorderWidth = 0f;
            //cell.FixedHeight = 10f;
            //cell.HorizontalAlignment = 0;
            //cell.VerticalAlignment = 1;
            //cell.Border = 0;
            //cell.Colspan = 7;
            //pdtdtl.AddCell(cell);
            //cell = new PdfPCell(FormatHeaderPhrase("Comments :"));
            ////cell.BorderWidth = 0f;
            //cell.FixedHeight = 20f;
            //cell.HorizontalAlignment = 0;
            //cell.VerticalAlignment = 1;
            //cell.Border = 0;
            //cell.Colspan = 7;
            //pdtdtl.AddCell(cell);

            //document.Add(pdtdtl);

            //PdfPTable pdtsig = new PdfPTable(3);
            //pdtsig.WidthPercentage = 100;
            //cell = new PdfPCell(FormatPhrase(""));
            //cell.HorizontalAlignment = 1;
            //cell.VerticalAlignment = 1;
            //cell.Border = 0;
            //cell.Colspan = 3;
            //cell.FixedHeight = 40f;
            //cell.BorderColor = BaseColor.LIGHT_GRAY;
            //pdtsig.AddCell(cell);
            //cell = new PdfPCell(FormatPhrase("Received by"));
            //cell.HorizontalAlignment = 1;
            //cell.VerticalAlignment = 1;
            //cell.Border = 1;
            //cell.FixedHeight = 20f;
            //cell.BorderColor = BaseColor.LIGHT_GRAY;
            //pdtsig.AddCell(cell);
            //cell = new PdfPCell(FormatPhrase(""));
            //cell.HorizontalAlignment = 0;
            //cell.VerticalAlignment = 1;
            //cell.Border = 0;
            //cell.FixedHeight = 20f;
            //cell.BorderColor = BaseColor.LIGHT_GRAY;
            //pdtsig.AddCell(cell);
            //cell = new PdfPCell(FormatPhrase("Prepared by"));
            //cell.HorizontalAlignment = 1;
            //cell.VerticalAlignment = 1;
            //cell.Border = 1;
            //cell.FixedHeight = 20f;
            //cell.BorderColor = BaseColor.LIGHT_GRAY;
            //pdtsig.AddCell(cell);
            //document.Add(pdtsig);

            //document.Close();
            //Response.Flush();
            //Response.End();
            if (ViewState["Flag"] == "")
            {
                ViewState["Flag"] = "1";
                dgPOrderMst_SelectedIndexChanged(sender, e);
            }
           

        }
    }
    protected void btnPrint_Click(object sender, EventArgs e)
    {
       
    }
    public void getPurchaseOrder(RequisitionOrder pomst)
    {
        string filename = "PO_" ;
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
        cell = new PdfPCell(new Phrase("Purchase Order(PO)", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 10, iTextSharp.text.Font.BOLD)));
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
        cell = new PdfPCell(FormatHeaderPhrase("Supplier Name :"));
        cell.BorderWidth = 0f;
        cell.FixedHeight = 20f;
        pdtclient.AddCell(cell);
        cell = new PdfPCell(FormatPhrase(": " ));
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
        cell = new PdfPCell(FormatHeaderPhrase("T. Of Delivery"));
        cell.BorderWidth = 0f;
        cell.FixedHeight = 20f;
        //cell.Colspan = 2;
        pdtclient.AddCell(cell);
        cell = new PdfPCell(FormatPhrase(": "));
        cell.BorderWidth = 0f;
        cell.FixedHeight = 20f;
        cell.Colspan = 3;
        pdtclient.AddCell(cell);
        cell = new PdfPCell(FormatHeaderPhrase("T. Of Payment"));
        cell.BorderWidth = 0f;
        cell.FixedHeight = 20f;
        //cell.Colspan = 4;
        pdtclient.AddCell(cell);
        cell = new PdfPCell(FormatPhrase(": " ));
        cell.BorderWidth = 0f;
        cell.FixedHeight = 20f;
        cell.Colspan = 3;
        pdtclient.AddCell(cell);

        PdfPTable pdtpur = new PdfPTable(2);
        pdtpur.WidthPercentage = 100;
        cell = new PdfPCell(FormatHeaderPhrase("Order No."));
        cell.BorderWidth = 0f;
        cell.FixedHeight = 20f;
        pdtpur.AddCell(cell);
        cell = new PdfPCell(FormatPhrase(": " + pomst.Code));
        cell.BorderWidth = 0f;
        cell.FixedHeight = 20f;
        pdtpur.AddCell(cell);
        cell = new PdfPCell(FormatHeaderPhrase("Date"));
        cell.BorderWidth = 0f;
        cell.FixedHeight = 20f;
        pdtpur.AddCell(cell);
        cell = new PdfPCell(FormatPhrase(": " + pomst.Date));
        cell.BorderWidth = 0f;
        cell.FixedHeight = 20f;
        pdtpur.AddCell(cell);
        cell = new PdfPCell(FormatHeaderPhrase("Delivery Date"));
        cell.BorderWidth = 0f;
        cell.FixedHeight = 20f;
        pdtpur.AddCell(cell);
        
        cell = new PdfPCell(FormatHeaderPhrase("Order Type"));
        cell.BorderWidth = 0f;
        cell.FixedHeight = 20f;
        pdtpur.AddCell(cell);
        if (String.IsNullOrEmpty(pomst.Status) || pomst.Status == "P")
        {
            cell = new PdfPCell(FormatPhrase(": Pending"));
            cell.BorderWidth = 0f;
            cell.FixedHeight = 20f;
            pdtpur.AddCell(cell);
        }
        else if (pomst.Status == "A")
        {
            cell = new PdfPCell(FormatPhrase(": Authorized"));
            cell.BorderWidth = 0f;
            cell.FixedHeight = 20f;
            pdtpur.AddCell(cell);
        }
        else if (pomst.Status == "D")
        {
            cell = new PdfPCell(FormatPhrase(": Delivered"));
            cell.BorderWidth = 0f;
            cell.FixedHeight = 20f;
            pdtpur.AddCell(cell);
        }
        cell = new PdfPCell(pdtclient);
        cell.BorderWidth = 0f;
        pdtm.AddCell(cell);
        cell = new PdfPCell(pdtpur);
        cell.BorderWidth = 0f;
        pdtm.AddCell(cell);
        document.Add(pdtm);

        //document.Add(dtempty);       
        float[] widthdtl = new float[7] { 12, 25, 50, 20, 20, 20, 25 };
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
        cell = new PdfPCell(FormatHeaderPhrase("Rate"));
        cell.HorizontalAlignment = 1;
        cell.VerticalAlignment = 1;
        cell.FixedHeight = 20f;
        cell.BorderColor = BaseColor.LIGHT_GRAY;
        pdtdtl.AddCell(cell);
        cell = new PdfPCell(FormatHeaderPhrase("Quantity"));
        cell.HorizontalAlignment = 1;
        cell.VerticalAlignment = 1;
        cell.FixedHeight = 20f;
        cell.BorderColor = BaseColor.LIGHT_GRAY;
        pdtdtl.AddCell(cell);
        cell = new PdfPCell(FormatHeaderPhrase("Amount"));
        cell.HorizontalAlignment = 2;
        cell.VerticalAlignment = 1;
        cell.FixedHeight = 20f;
        cell.BorderColor = BaseColor.LIGHT_GRAY;
        pdtdtl.AddCell(cell);
        //DataTable dt = (DataTable)Session["purdtl"];
        DataTable dt = PurchaseOrderManager.GetPurchaseOrderItemsDetails(pomst.ID);
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

                cell = new PdfPCell(FormatPhrase(dr["item_rate"].ToString()));
                cell.HorizontalAlignment = 1;
                cell.VerticalAlignment = 1;
                cell.BorderColor = BaseColor.LIGHT_GRAY;
                pdtdtl.AddCell(cell);

                cell = new PdfPCell(FormatPhrase(dr["qnty"].ToString()));
                cell.HorizontalAlignment = 2;
                cell.VerticalAlignment = 1;
                cell.BorderColor = BaseColor.LIGHT_GRAY;
                pdtdtl.AddCell(cell);

                cell = new PdfPCell(FormatPhrase(dr["Total"].ToString()));
                cell.HorizontalAlignment = 2;
                cell.VerticalAlignment = 1;
                cell.BorderColor = BaseColor.LIGHT_GRAY;
                pdtdtl.AddCell(cell);
                //if (Convert.ToDecimal(dr["ID"]) != 0)
                //{
                tot += Convert.ToDecimal(dr["Total"]);
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
        cell.Colspan = 5;
        pdtdtl.AddCell(cell);

        cell = new PdfPCell(FormatPhrase(totQty.ToString("N2")));
        // cell.BorderWidth = 0f;
        cell.FixedHeight = 20f;
        cell.HorizontalAlignment = 2;
        cell.VerticalAlignment = 1;
        cell.BorderColor = BaseColor.LIGHT_GRAY;
        pdtdtl.AddCell(cell);

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
        cell = new PdfPCell(FormatHeaderPhrase("Comments :"));
        //cell.BorderWidth = 0f;
        cell.FixedHeight = 20f;
        cell.HorizontalAlignment = 0;
        cell.VerticalAlignment = 1;
        cell.Border = 0;
        cell.Colspan = 7;
        pdtdtl.AddCell(cell);

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
    protected void chkSelect_CheckedChanged(object sender, EventArgs e)
    {
        if (ViewState["reor"] != null)
        {
            CheckBox chk = (CheckBox)sender;
            GridViewRow gvr = (GridViewRow)chk.NamingContainer;
            DataTable dtdtl = (DataTable)ViewState["reor"];
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
            ViewState["reor"] = dtdtl;
            dgPOrderMst.DataSource = dtdtl;
            dgPOrderMst.DataBind();
        }
    }
    protected void btnAuthorize_Click(object sender, EventArgs e)
    {
        try
        {
            DataTable dt = (DataTable)ViewState["reor"];
            string UserID= Session["userID"].ToString();
            RequisitionOrderManager.AuthorizedRequisition(dt,UserID);
            dt = RequisitionOrderManager.GetRequisitionInfoByPanding("");
            ViewState["reor"] = dt;
            dgPOrderMst.DataSource = dt;
            dgPOrderMst.DataBind();
            ClientScript.RegisterStartupScript(this.GetType(), "Warning", "alert('Authorized successfully...!!');", true);
        }
        catch (Exception ex)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "Warning", "alert('"+ex.Message+"');", true);
        }
    }
}