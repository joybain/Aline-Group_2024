using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using autouniv;
using iTextSharp.text;
using iTextSharp.text.pdf;
using OldColor;
using iTextSharp.text.pdf.draw;

public partial class frmReturnProduct : System.Web.UI.Page
{
    private ProjectCostingManager _projectCostManager = new ProjectCostingManager();
    private ProjectStockCostmodel _projectCostModel = new ProjectStockCostmodel();
    private static Permis per;

   ProjectStockReturn _projectReturn = new ProjectStockReturn();
    ProjectReturnCostingManager _projectMng = new ProjectReturnCostingManager();
 

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
        ddlProjectName.DataSource = MeterialRequistitonManager.GetProject();
        ddlProjectName.DataTextField = "ProjectName";
        ddlProjectName.DataValueField = "ID";
        ddlProjectName.DataBind();
        ddlProjectName.Items.Insert(0, "");


        ddlProjectNameSerch.DataSource = MeterialRequistitonManager.GetProject();
        ddlProjectNameSerch.DataTextField = "ProjectName";
        ddlProjectNameSerch.DataValueField = "ID";
        ddlProjectNameSerch.DataBind();
        ddlProjectNameSerch.Items.Insert(0, "");

        txtInvoiceNo.Text = IdManager.GetDateTimeWiseSerial("Rt-Cost-", "ID", "[ProjectStockReturnCostMst]");
        txtCostDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
        DataTable dtgetEmty = getEmptyDtl();
        dgPODetailsDtl.DataSource = dtgetEmty;
        ViewState["ItemDtl"] = dtgetEmty;
        dgPODetailsDtl.DataBind();
        lblQuantity.Text = "";
        txtSite.Text = "";

        txtsearchDatefrom.Text = "";
        txtsearchDateTo.Text = "";

        DataTable dt = _projectMng.Getdata();
        dgHistory.DataSource = dt;
        dgHistory.DataBind();
        btnUpdate.Visible = false;

    }

    private DataTable getEmptyDtl()
    {
        //dgPODetailsDtl.Visible = true;
        DataTable dtDtlGrid = new DataTable();
        dtDtlGrid.Columns.Add("ID", typeof(string));
        dtDtlGrid.Columns.Add("ItemId", typeof(string));
        dtDtlGrid.Columns.Add("Name", typeof(string));
        dtDtlGrid.Columns.Add("Code", typeof(string));
        dtDtlGrid.Columns.Add("Uom", typeof(string));
        dtDtlGrid.Columns.Add("qnty", typeof(string));
        dtDtlGrid.Columns.Add("UomId", typeof(string));
        dtDtlGrid.Columns.Add("Remarks", typeof(string));
        dtDtlGrid.Columns.Add("Costqnty", typeof(string));

        DataRow dr = dtDtlGrid.NewRow();
        dtDtlGrid.Rows.Add(dr);
        return dtDtlGrid;

    }
    protected void txtItemSearch_TextChanged(object sender, EventArgs e)
    {
        try
        {
            //string ItemCode = "";
            //DataTable dtl = IdManager.GetShowDataTable("select t1.Code as item_code from (select t1.ItemId,t2.Code,t2.Name, sum(t1.Quentity) TotalQty,t3.ProjectId from ProjectStockCostdtl t1 Left outer join tbl_ProjectItemStock t3 on t1.ItemId=t3.Id inner join item t2 on t2.ID=t3.ItemID and t2.Code=t3.item_code where t3.ProjectId='" + ddlProjectName.SelectedValue + "' group by t1.ItemId,t2.Name,t2.Code,t3.ProjectId) t1 WHERE t1.ProjectId='" + ddlProjectName.SelectedValue + "' AND UPPER(t1.Code + '-' + t1.Name + '-' + ISNULL(CONVERT(NVARCHAR(50), t1.TotalQty), '0')) LIKE upper('%" + txtItemSearch.Text + "%')");

            //if (dtl.Rows.Count > 0)
            //{
            //    ItemCode = dtl.Rows[0]["item_code"].ToString();
            //}
            //else
            //{
            //    txtItemSearch.Text = "";
            //    txtItemSearch.Focus();
            //    return;
            //}

          //  DataTable dt = (DataTable)ViewState["ItemDtl"];
            DataTable dtdtl = ProjectCostingManager.GetItemsForProjectReturn(txtItemSearch.Text, ddlProjectName.SelectedValue);
            decimal Quantity = Convert.ToDecimal(dtdtl.Rows[0]["Costqnty"].ToString());
            if (Quantity > 0)
            {
                if (dtdtl.Rows.Count > 0)
                {

                    string find = "";
                    string flag = "";

                    //foreach (DataRow data in dtdtl.Rows)
                    //{
                    //    if (data["ID"].ToString() == dtdtl.Rows[0]["Id"].ToString() && data["Code"].ToString() == dtdtl.Rows[0]["Code"].ToString() && data["Name"].ToString() == dtdtl.Rows[0]["Name"].ToString())
                    //    {
                    //        if (Quantity > Convert.ToDecimal(data["qnty"].ToString()))
                    //        {
                    //            flag = "Y";
                    //            ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('Allready Add This Items...!!');", true);
                    //        }


                    //    }

                    //    if (data["ID"].ToString() == "" && data["Name"].ToString() == "")
                    //    {
                    //        find = "Y";

                    //    }
                    //}
                    if (find == "Y")
                    {
                        dtdtl.Rows.Remove(dtdtl.Rows[0]);

                    }
                    if (flag != "Y")
                    {
                        DataRow dr = dtdtl.NewRow();

                        dr["ID"] = dtdtl.Rows[0]["Id"].ToString();
                        dr["ItemId"] = dtdtl.Rows[0]["ItemId"].ToString();
                        dr["Name"] = dtdtl.Rows[0]["Name"].ToString();
                        dr["Code"] = dtdtl.Rows[0]["Code"].ToString();
                        dr["qnty"] = "0";
                        dr["Costqnty"] = dtdtl.Rows[0]["Costqnty"].ToString();
                        dr["Uom"] = dtdtl.Rows[0]["Uom"].ToString();
                        dr["UomId"] = dtdtl.Rows[0]["UomId"].ToString();
                        dr["Remarks"] = "";
                        //dr["check"] = 1;   
                        dtdtl.Rows.Add(dr);
                        dtdtl.Rows.Remove(dtdtl.Rows[0]);
                    }
                    txtItemSearch.Text = "";
                    txtItemSearch.Focus();
                    dgPODetailsDtl.DataSource = dtdtl;
                    ViewState["ItemDtl"] = dtdtl;
                    dgPODetailsDtl.DataBind();
                }
            }

            else
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('Item Received Quantity Is Over...!!');", true);
                txtItemSearch.Text = "";
                txtItemSearch.Focus();
                return;
            }
           // txtItemSearch.Focus();
            ShowFooterTotal(dtdtl, dgPODetailsDtl);
            //UP4.Update();
            //UP44.Update();
           // txtItemSearch.Focus();
        }
        catch (Exception ex)
        {


            ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "jsAlert('" + ex.Message + "','red',0);", true);
        }
    }
    protected void ddlProjectName_SelectedIndexChanged(object sender, EventArgs e)
    {
        var ProjectId = ddlProjectName.SelectedValue;
        Session["projectid"] = ProjectId;
        txtItemSearch.Enabled = true;
        txtSite.Text = IdManager.GetShowSingleValueString("Address", "ID", "Project_Setup_Tbl", ddlProjectName.SelectedValue);
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

            for (int i = 1; i < j; i++)
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

            lblQuantity.Text = GetQuantity(dt).ToString();
            //cell = new TableCell();
            //priceDr = GetTotal();


            //cell.HorizontalAlign = HorizontalAlign.Right;
            //row.Cells.Add(cell);

            //cell = new TableCell();
            ////priceDr = GetTotal();
            //cell.Text = GetTotalSales(dt).ToString("N2");

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
                if (drp["ID"].ToString() != "" && drp["qnty"].ToString() != "")
                {
                    ctot += decimal.Parse(drp["qnty"].ToString());
                }
            }
        }

        return ctot;
    }
    protected void dgPODetailsDtl_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow | e.Row.RowType == DataControlRowType.Header | e.Row.RowType == DataControlRowType.Footer)
        {
            e.Row.Cells[1].Attributes.Add("style", "display:none");
            e.Row.Cells[2].Attributes.Add("style", "display:none");
            e.Row.Cells[6].Attributes.Add("style", "display:none");
        }
    }

    protected void dgPODetailsDtl_RowDataBoundHistory(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow | e.Row.RowType == DataControlRowType.Header | e.Row.RowType == DataControlRowType.Footer) 
        {
            e.Row.Cells[1].Attributes.Add("style", "display:none");
        }
    }


    protected void txtQnty_TextChanged(object sender, EventArgs e)
    {
        try
        {
            GridViewRow gvr = (GridViewRow)((TextBox)sender).NamingContainer;
            DataTable dt = (DataTable)ViewState["ItemDtl"];


            if (dt.Rows.Count > 0)
            {
                DataRow dr = dt.Rows[gvr.DataItemIndex];
                //int remining = IdManager.GetShowSingleValueInt("isnull(TotalQty,0)", "(select t1.ItemId,t2.Code,t2.Name, sum(t1.Quentity) TotalQty,t3.ProjectId from ProjectStockCostdtl t1 Left outer join tbl_ProjectItemStock t3 on t1.ItemId=t3.Id inner join item t2 on t2.ID=t3.ItemID and t2.Code=t3.item_code where t3.ProjectId='" + ddlProjectName.SelectedValue + "' group by t1.ItemId,t2.Name,t2.Code,t3.ProjectId) t1 WHERE t1.ProjectId='" + ddlProjectName.SelectedValue + "' and t1.ItemId='" + dr["ID"].ToString() + "'");


                //var receivedqnt = dt.Rows[0]["Costqnty"].ToString();
                //decimal received = Convert.ToDecimal(receivedqnt);

                //if (Convert.ToDecimal(((TextBox)gvr.FindControl("txtQnty")).Text) <= received)
                //{
                    dr["qnty"] = ((TextBox)gvr.FindControl("txtQnty")).Text;
                //}

                //else
                //{
                //    ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('Item  Quantity Is Over...!!');", true);
                    dgPODetailsDtl.DataSource = dt;
                    ViewState["ItemDtl"] = dt;
                    dgPODetailsDtl.DataBind();
                    ShowFooterTotal(dt, dgPODetailsDtl);
                    return;
                }


            //}
            txtItemSearch.Text = "";
            dgPODetailsDtl.DataSource = dt;
            ViewState["ItemDtl"] = dt;
            dgPODetailsDtl.DataBind();

            ShowFooterTotal(dt, dgPODetailsDtl);
            txtItemSearch.Focus();
            //UP4.Update();
            //UP44.Update();
        }
        catch (Exception ex)
        {


            ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "jsAlert('" + ex.Message + "','red',0);", true);

        }
    }

    protected void btnSearchOnclick(object sender, EventArgs e)
    {



        DataTable dt = _projectMng.getDataSerch(txtsearchDatefrom.Text, txtsearchDateTo.Text, ddlProjectNameSerch.SelectedValue);
            dgHistory.DataSource = dt;
            dgHistory.DataBind();
            Refresh();
        
    }



    public void btnSaveReturn_Click(object sender, EventArgs e) 
    {

        try 
        { 
              if (string.IsNullOrEmpty(ddlProjectName.SelectedValue))
                 {
                  ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('Please Select Project Name...!!');", true);
                   return;
                  }
            
            // get propertics value 
              _projectReturn.CostCode = txtInvoiceNo.Text;
              _projectReturn.ProjectID = int.Parse(ddlProjectName.SelectedValue);
              _projectReturn.sight_Name = txtSite.Text.ToString();
              _projectReturn.Remarks = txtRemarks.Text;
              _projectReturn.Addby =Session["user"].ToString();
              _projectReturn.costingDate =DateTime.Now.ToString();
              _projectReturn.TotalQnitity = int.Parse(lblQuantity.Text);

            /// get Girdview values 
              DataTable dtDtlGrid1 = (DataTable)ViewState["ItemDtl"];


              ProjectReturnCostingManager.ProjectReturnCostingSave(dtDtlGrid1, _projectReturn);
             
             ClientScript.RegisterStartupScript(this.GetType(), "Warning", "alert('Record has been Save successfully...!!');", true);
             Refresh();

        }
        catch(Exception ex)
        {
            throw new Exception(ex.Message);
        }
        

    }

    protected void dgPODetailsDtl_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            if (ViewState["ItemDtl"] != null)
            {
                DataTable dtDtlGrid = (DataTable)ViewState["ItemDtl"];
                //DataTable dtDtlGrid = (DataTable)Session["purdtl"];
                dtDtlGrid.Rows.RemoveAt(dgPODetailsDtl.Rows[e.RowIndex].DataItemIndex);
                if (dtDtlGrid.Rows.Count > 0)
                {


                    dgPODetailsDtl.DataSource = dtDtlGrid;
                    dgPODetailsDtl.DataBind();

                    ShowFooterTotal(dtDtlGrid, dgPODetailsDtl);
                    ViewState["ItemDtl"] = dtDtlGrid;

                }
                else
                {
                    dgPODetailsDtl.DataSource = getEmptyDtl();
                    dgPODetailsDtl.DataBind();
                    ViewState["ItemDtl"] = getEmptyDtl();
                    ShowFooterTotal(getEmptyDtl(), dgPODetailsDtl);
                }

                // ShowFooterTotal();

            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('Your session is over. Try it again!!');", true);

                // ClientScript.RegisterStartupScript(this.GetType(), "ale", "alert('Your session is over. Try it again!!');", true);
            }
        }
        catch (Exception ex)
        {


            ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "jsAlert('" + ex.Message + "','red',0);", true);

        }
    }


    protected void dgHistory_SelectedIndexChanged(object sender, EventArgs e)
    {
        DgHistoryId.Value = dgHistory.SelectedRow.Cells[1].Text;
       
        DataTable dt = _projectMng.GetMstID(DgHistoryId.Value);

        txtInvoiceNo.Text = dt.Rows[0]["CostCode"].ToString();
        txtCostDate.Text = dt.Rows[0]["CostingDate"].ToString();
        txtSite.Text = dt.Rows[0]["Address"].ToString();
        lblQuantity.Text = dt.Rows[0]["TotalQuntity"].ToString();
        txtRemarks.Text = dt.Rows[0]["remarks"].ToString();
        ddlProjectName.SelectedValue = dt.Rows[0]["ProjectId"].ToString();
        txtID.Text = DgHistoryId.Value.ToString();

        DataTable dt1 = _projectMng.GetDtlId(DgHistoryId.Value);

        dgPODetailsDtl.DataSource = dt1;
        dgPODetailsDtl.DataBind();
        ShowFooterTotal(dt1, dgPODetailsDtl);
        //lblQuantity.Text = GetQuantity(dt1).ToString();
        pnl.ActiveTabIndex = 0;

        txtItemSearch.Enabled = true;
        string Itemcode = txtItemSearch.Text.ToString();
        int projectId = int.Parse(ddlProjectName.SelectedValue);
        Session["projectId"] = projectId;
        ItemUpdateSearch(Itemcode, projectId);
        btnUpdate.Visible = true;

    }


    private void ItemUpdateSearch(string Item_code, int id)
    {

        try
        {
            string ItemCode = "";
            DataTable dtl = IdManager.GetShowDataTable("select item_code from tbl_ProjectItemStock t1 inner join Item t2 on t1.ItemID=t2.ID where  UPPER(t1.item_code + '-' + t2.Name + '-' + ISNULL(CONVERT(NVARCHAR(50), t1.Quntity), '0'))=UPPER ('" + txtItemSearch.Text + "')  and ProjectId='" + ddlProjectName.SelectedValue + "'");

            if (dtl.Rows.Count > 0)
            {
                ItemCode = dtl.Rows[0]["item_code"].ToString();
            }
            else
            {
                txtItemSearch.Text = "";
                txtItemSearch.Focus();
                return;
            }

            DataTable dt = (DataTable)ViewState["ItemDtl"];
            DataTable dtdtl = ProjectCostingManager.GetItemsForProjectcosting(ItemCode, ddlProjectName.SelectedValue);
            decimal Quantity = Convert.ToDecimal(dtdtl.Rows[0]["qnty"].ToString());
            if (Quantity > 0)
            {
                if (dtdtl.Rows.Count > 0)
                {

                    string find = "";
                    string flag = "";

                    foreach (DataRow data in dt.Rows)
                    {
                        if (data["ID"].ToString() == dtdtl.Rows[0]["Id"].ToString() && data["Code"].ToString() == dtdtl.Rows[0]["Code"].ToString() && data["Name"].ToString() == dtdtl.Rows[0]["Name"].ToString())
                        {
                            if (Quantity > Convert.ToDecimal(data["qnty"].ToString()))
                            {
                                flag = "Y";
                                ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('Allready Add This Items...!!');", true);
                            }


                        }

                        if (data["ID"].ToString() == "" && data["Name"].ToString() == "")
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
                        dr["Name"] = dtdtl.Rows[0]["Name"].ToString();
                        dr["Code"] = dtdtl.Rows[0]["Code"].ToString();
                        dr["qnty"] = "1";
                        dr["Uom"] = dtdtl.Rows[0]["Uom"].ToString();
                        dr["UomId"] = dtdtl.Rows[0]["UomId"].ToString();

                        dt.Rows.Add(dr);
                    }
                    txtItemSearch.Text = "";
                    txtItemSearch.Focus();
                    dgPODetailsDtl.DataSource = dt;
                    ViewState["ItemDtl"] = dt;
                    dgPODetailsDtl.DataBind();
                }
            }

            else
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('Item Received Quantity Is Over...!!');", true);
                txtItemSearch.Text = "";
                txtItemSearch.Focus();
                return;
            }
            txtItemSearch.Focus();
            ShowFooterTotal(dt, dgPODetailsDtl);
            //UP4.Update();
            //UP44.Update();
            txtItemSearch.Focus();
        }
        catch (Exception ex)
        {


            ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "jsAlert('" + ex.Message + "','red',0);", true);
        }

    }



    protected void btnView_Click(object sender, EventArgs e)
    {

        getReturnProjectcost();

    }



    public void getReturnProjectcost()
    {
        string filename = "Return_" + txtInvoiceNo.Text + DateTime.Now.ToString();
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
        cell = new PdfPCell(new Phrase("Project Stock Return ", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 10, iTextSharp.text.Font.BOLD)));
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
        cell = new PdfPCell(FormatHeaderPhrase("Return   Code "));
        cell.BorderWidth = 0f;
        cell.FixedHeight = 20f;
        pdtclient.AddCell(cell);
        cell = new PdfPCell(FormatPhrase(": " + txtInvoiceNo.Text));
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
       
        PdfPTable pdtpur = new PdfPTable(2);
        pdtpur.WidthPercentage = 100;
        cell = new PdfPCell(FormatHeaderPhrase("Project site "));
        cell.BorderWidth = 0f;
        cell.FixedHeight = 20f;
        pdtpur.AddCell(cell);
        cell = new PdfPCell(FormatPhrase(": " + txtSite.Text));
        cell.BorderWidth = 0f;
        cell.FixedHeight = 20f;
        pdtpur.AddCell(cell);
        cell = new PdfPCell(FormatHeaderPhrase("Return Date "));
        cell.BorderWidth = 0f;
        cell.FixedHeight = 20f;
        pdtpur.AddCell(cell);
        cell = new PdfPCell(FormatPhrase(": " + txtCostDate.Text));
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


        //document.Add(pdtpur);       
        float[] widthdtl = new float[6] { 12, 25, 50,20, 20,20};
        PdfPTable pdtdtl = new PdfPTable(widthdtl);
        pdtdtl.WidthPercentage = 100;

        cell = new PdfPCell(FormatHeaderPhrase("Serial"));
        cell.HorizontalAlignment = 1;
        cell.VerticalAlignment = 1;
        cell.FixedHeight = 20f;
        cell.BorderColor = BaseColor.LIGHT_GRAY;
        pdtdtl.AddCell(cell);
        cell = new PdfPCell(FormatHeaderPhrase("Return Item Code"));
        cell.HorizontalAlignment = 1;
        cell.VerticalAlignment = 1;
        cell.FixedHeight = 20f;
        cell.BorderColor = BaseColor.LIGHT_GRAY;
        pdtdtl.AddCell(cell);
        cell = new PdfPCell(FormatHeaderPhrase("Item Name"));
        cell.HorizontalAlignment = 0;
        cell.VerticalAlignment = 1;
        cell.FixedHeight = 20f;
        cell.BorderColor = BaseColor.LIGHT_GRAY;
        pdtdtl.AddCell(cell);

        //cell = new PdfPCell(FormatHeaderPhrase("Project name "));
        //cell.HorizontalAlignment = 1;
        //cell.VerticalAlignment = 1;
        //cell.FixedHeight = 20f;
        //cell.BorderColor = BaseColor.LIGHT_GRAY;
        //pdtdtl.AddCell(cell);

        cell = new PdfPCell(FormatHeaderPhrase("UOM"));
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
        cell = new PdfPCell(FormatHeaderPhrase("Return Qnty"));
        cell.HorizontalAlignment = 1;
        cell.VerticalAlignment = 1;
        cell.FixedHeight = 20f;
        cell.BorderColor = BaseColor.LIGHT_GRAY;
        pdtdtl.AddCell(cell);

        cell = new PdfPCell(FormatHeaderPhrase("Remarks"));
        cell.HorizontalAlignment = 1;
        cell.VerticalAlignment = 1;
        cell.FixedHeight = 20f;
        cell.BorderColor = BaseColor.LIGHT_GRAY;
        pdtdtl.AddCell(cell);

        DgHistoryId.Value = dgHistory.SelectedRow.Cells[1].Text;

        DataTable dt = _projectMng.GetreturnItemDetlise(DgHistoryId.Value);
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

                cell = new PdfPCell(FormatPhrase(dr["Code"].ToString()));
                cell.HorizontalAlignment = 1;
                cell.VerticalAlignment = 1;
                cell.BorderColor = BaseColor.LIGHT_GRAY;
                pdtdtl.AddCell(cell);

                cell = new PdfPCell(FormatPhrase(dr["Name"].ToString()));
                cell.HorizontalAlignment = 0;
                cell.VerticalAlignment = 1;
                cell.BorderColor = BaseColor.LIGHT_GRAY;
                pdtdtl.AddCell(cell);

                //cell = new PdfPCell(FormatPhrase(dr["projectID"].ToString()));
                //cell.HorizontalAlignment = 1;
                //cell.VerticalAlignment = 1;
                //cell.BorderColor = BaseColor.LIGHT_GRAY;
                //pdtdtl.AddCell(cell);


                cell = new PdfPCell(FormatPhrase(dr["Uom"].ToString()));
                cell.HorizontalAlignment = 1;
                cell.VerticalAlignment = 1;
                cell.BorderColor = BaseColor.LIGHT_GRAY;
                pdtdtl.AddCell(cell);

                //cell = new PdfPCell(FormatPhrase(dr["item_rate"].ToString()));
                //cell.HorizontalAlignment = 1;
                //cell.VerticalAlignment = 1;
                //cell.BorderColor = BaseColor.LIGHT_GRAY;
                //pdtdtl.AddCell(cell);

                cell = new PdfPCell(FormatPhrase(dr["Quentity"].ToString()));
                //cell.HorizontalAlignment = Element.ALIGN_CENTER;
                //cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = 1;
                cell.VerticalAlignment = 1;
                cell.BorderColor = BaseColor.LIGHT_GRAY;
                pdtdtl.AddCell(cell);


                cell = new PdfPCell(FormatPhrase(dr["Remarks"].ToString()));
                //cell.HorizontalAlignment = Element.ALIGN_CENTER;
                //cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = 1;
                cell.VerticalAlignment = 1;
                cell.BorderColor = BaseColor.LIGHT_GRAY;
                pdtdtl.AddCell(cell);

           



                //if (Convert.ToDecimal(dr["ID"]) != 0)
                //{
                tot += Convert.ToDecimal(dr["Quentity"]);
                //}
                //if (Convert.ToDecimal(dr["ID"]) != 0)
                //{
                //  totQty += Convert.ToDecimal(dr["qnty"]);
                //}
            }
        }

        cell = new PdfPCell(FormatPhrase("Total "));
        cell.FixedHeight = 20f;
        cell.HorizontalAlignment = 2;
        cell.VerticalAlignment = 1;
        cell.BorderColor = BaseColor.LIGHT_GRAY;
        cell.Colspan = 4;
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
        //cell.HorizontalAlignment = Element.ALIGN_CENTER;
        //cell.VerticalAlignment = Element.ALIGN_MIDDLE;
        cell.FixedHeight = 20f;
        cell.HorizontalAlignment = 1;
        cell.VerticalAlignment = 1;
        cell.BorderColor = BaseColor.LIGHT_GRAY;
        pdtdtl.AddCell(cell);

        cell = new PdfPCell(FormatPhrase(" "));
        //cell.BorderWidth = 0f;
        //cell.HorizontalAlignment = Element.ALIGN_CENTER;
        //cell.VerticalAlignment = Element.ALIGN_MIDDLE;
        cell.FixedHeight = 20f;
        cell.HorizontalAlignment = 0;
        cell.VerticalAlignment = 1;
        cell.BorderColor = BaseColor.LIGHT_GRAY;
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


    protected void txtRemarks_TextChanged(object sender, EventArgs e)
    {

        GridViewRow gvr = (GridViewRow)((TextBox)sender).NamingContainer;
        DataTable dt = (DataTable)ViewState["ItemDtl"];
        if (dt.Rows.Count > 0)
        {
            DataRow dr = dt.Rows[gvr.DataItemIndex];
            dr["ID"] = dr["ID"].ToString();
            dr["Name"] = dr["Name"].ToString();
            dr["Code"] = dr["Code"].ToString();
            dr["Uom"] = dr["Uom"].ToString();
            dr["qnty"] = dr["qnty"].ToString();
            dr["UomId"] = dr["UomId"].ToString();
            dr["Remarks"] = ((TextBox)gvr.FindControl("txtRemarks")).Text;

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
        dgPODetailsDtl.DataSource = dt;
        dgPODetailsDtl.DataBind();
        //ShowFooterTotal();
        // ((TextBox)gvr.FindControl("txtItemRate")).Focus();        
        ((TextBox)dgPODetailsDtl.Rows[dgPODetailsDtl.Rows.Count - 1].FindControl("txtRemarks")).Focus();
        //MRIesms_UP.Update();
    }





    protected void btnUpdateReturn_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(ddlProjectName.SelectedValue))
        {
            ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('Please Select Project Name...!!');", true);
            return;
        }

        _projectReturn.id = txtID.Text.ToString();
        _projectReturn.CostCode = txtInvoiceNo.Text;
        _projectReturn.ProjectID = Convert.ToInt32(ddlProjectName.SelectedValue);
        _projectReturn.sight_Name = txtSite.Text.Trim();
        _projectReturn.costingDate = txtCostDate.Text;
        _projectReturn.Remarks = txtRemarks.Text;


        _projectReturn.Addby = Session["user"].ToString();
        _projectReturn.TotalQnitity = int.Parse(lblQuantity.Text);


        //DataTable dt = (DataTable)Session["purdtl"];
        ////_projectCostManager.ProjectcostingSave(dt, _projectCostModel);

        DataTable dtDtlGrid = (DataTable)ViewState["ItemDtl"];

        ProjectCostingManager.ProjectcostingUpdate(dtDtlGrid, _projectCostModel, DgHistoryId.Value);
        ClientScript.RegisterStartupScript(this.GetType(), "Warning", "alert('Record has been Save successfully...!!');", true);
        Refresh();
    }
    protected void btnClearLost_Click(object sender, EventArgs e)
    {
        Refresh();
    }
}