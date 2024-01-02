using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using autouniv;
using System.Data;

public partial class fmRequrimentAutherzation : System.Web.UI.Page
{
    private static Permis per;
    private readonly AuthenaticationRequirmentManager _AuthReqManager = new AuthenaticationRequirmentManager();
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
    public void Refresh() 
    {
        ddlProjectName.DataSource = MeterialRequistitonManager.GetProject();
        ddlProjectName.DataTextField = "ProjectName";
        ddlProjectName.DataValueField = "ID";
        ddlProjectName.DataBind();
        ddlProjectName.Items.Insert(0, "");
        txtItemSearch.Enabled = false;

        DataTable dtgetEmty = getEmptyDtl();
        dgPODetailsDtl.DataSource = dtgetEmty;
        ViewState["ItemDtl"] = dtgetEmty;
        dgPODetailsDtl.DataBind();
        lblQuantity.Text = "";
        txtSite.Text = "";

        dgHistory.DataSource = _AuthReqManager.Getmaterialrequization();
        dgHistory.DataBind();
        pnl.ActiveTabIndex = 1;





    }

  
    private DataTable getEmptyDtl()
    {
        //dgPODetailsDtl.Visible = true;
        DataTable dtDtlGrid = new DataTable();
        dtDtlGrid.Columns.Add("Id", typeof(string));
        dtDtlGrid.Columns.Add("item_Code", typeof(string));
        dtDtlGrid.Columns.Add("item_desc", typeof(string));
        //dtDtlGrid.Columns.Add("Uom", typeof(string));
        dtDtlGrid.Columns.Add("Total_Requisition", typeof(string));
        dtDtlGrid.Columns.Add("qnty", typeof(string));
        dtDtlGrid.Columns.Add("ItemId", typeof(string));
        dtDtlGrid.Columns.Add("Remarks", typeof(string));


        DataRow dr = dtDtlGrid.NewRow();
        dtDtlGrid.Rows.Add(dr);
        return dtDtlGrid;

    }

    protected void ddlProjectName_SelectedIndexChanged(object sender, EventArgs e)
    {

        var ProjectId = ddlProjectName.SelectedValue;
        Session["projectid"] = ProjectId;
        txtItemSearch.Enabled = true;
        txtSite.Text = IdManager.GetShowSingleValueString("Address", "ID", "Project_Setup_Tbl", ddlProjectName.SelectedValue);

    }

    protected void dgPODetailsDtl_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow | e.Row.RowType == DataControlRowType.Header | e.Row.RowType == DataControlRowType.Footer)
        {
            e.Row.Cells[1].Attributes.Add("style", "display:none");
            //e.Row.Cells[5].Attributes.Add("style", "display:none");
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
            cell.ColumnSpan = 3;
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

    protected void txtQnty_TextChanged(object sender, EventArgs e)
    {
        GridViewRow gvr = (GridViewRow)((TextBox)sender).NamingContainer;
        DataTable dt = (DataTable)ViewState["ItemDtl"];
        if (dt.Rows.Count > 0)
        {
            DataRow dr = dt.Rows[gvr.DataItemIndex];
            dr["ID"] = dr["ID"].ToString();
            dr["item_Code"] = dr["item_Code"].ToString();
            dr["item_desc"] = dr["item_desc"].ToString();
            dr["Total_Requisition"] = dr["Total_Requisition"].ToString();
            dr["Remarks"] = dr["Remarks"].ToString();
            //dr["ItemId"] = dr["ItemId"].ToString();
            dr["qnty"] = ((TextBox)gvr.FindControl("txtQnty")).Text;

        }
        //string found = "";
        //foreach (DataRow drd in dt.Rows)
        //{
        //    if (drd["ID"].ToString() == "" && drd["item_desc"].ToString() == "")
        //    {
        //        found = "Y";
        //    }
        //}
        //if (found == "")
        //{
        //    DataRow drd = dt.NewRow();
        //    dt.Rows.Add(drd);
        //}
        dgPODetailsDtl.DataSource = dt;
        dgPODetailsDtl.DataBind();
        ShowFooterTotal(dt, dgPODetailsDtl);
        // ((TextBox)gvr.FindControl("txtItemRate")).Focus();        
        ((TextBox)dgPODetailsDtl.Rows[dgPODetailsDtl.Rows.Count - 1].FindControl("txtQnty")).Focus();
        //MRIesms_UP.Update();
    }

    protected void txtRemarks_TextChanged(object sender, EventArgs e)
    {

        GridViewRow gvr = (GridViewRow)((TextBox)sender).NamingContainer;
        DataTable dt = (DataTable)ViewState["ItemDtl"];
        if (dt.Rows.Count > 0)
        {
            DataRow dr = dt.Rows[gvr.DataItemIndex];
            dr["ID"] = dr["ID"].ToString();
            dr["item_Code"] = dr["item_Code"].ToString();
            dr["item_desc"] = dr["item_desc"].ToString();
            dr["Total_Requisition"] = dr["Total_Requisition"].ToString();
            dr["qnty"] = dr["qnty"].ToString();
            //dr["ItemId"] = dr["ItemId"].ToString();
            dr["Remarks"] = ((TextBox)gvr.FindControl("txtRemarks")).Text;

        }
        //string found = "";
        //foreach (DataRow drd in dt.Rows)
        //{
        //    if (drd["ID"].ToString() == "" && drd["item_desc"].ToString() == "")
        //    {
        //        found = "Y";
        //    }
        //}
        //if (found == "")
        //{
        //    DataRow drd = dt.NewRow();
        //    dt.Rows.Add(drd);
        //}
        dgPODetailsDtl.DataSource = dt;
        dgPODetailsDtl.DataBind();
        //ShowFooterTotal();
        // ((TextBox)gvr.FindControl("txtItemRate")).Focus();        
        ((TextBox)dgPODetailsDtl.Rows[dgPODetailsDtl.Rows.Count - 1].FindControl("txtRemarks")).Focus();
        //MRIesms_UP.Update();
    }

    protected void txtItemSearch_TextChanged(object sender, EventArgs e)
    {
        Session["item_code"] = txtItemSearch.Text.ToString();
        try
        {
            string ItemCode = "";
            //DataTable dtl = IdManager.GetShowDataTable("select item_code from tbl_ProjectItemStock t1 inner join Item t2 on t1.ItemID=t2.ID where  UPPER(t1.item_code + '-' + t2.Name + '-' + ISNULL(CONVERT(NVARCHAR(50), t1.Quntity), '0'))=UPPER ('" + txtItemSearch.Text + "')  and ProjectId='" + ddlProjectName.SelectedValue + "'");
            DataTable dtl = IdManager.GetShowDataTable("select id as item_code from MaterialRequisitionMst where RequisitionNo='" + txtItemSearch.Text + "' and ProjectId='" + ddlProjectName.SelectedValue + "'");
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
            //DataTable dtdtl = ProjectCostingManager.GetItemsForProjectcosting(ItemCode, ddlProjectName.SelectedValue);
            DataTable dtdtl = _AuthReqManager.GetForRequimentAuthezatrion(ItemCode);
            dgPODetailsDtl.DataSource = dtdtl;
            ViewState["ItemDtl"] = dtdtl;
            dgPODetailsDtl.DataBind();
            txtItemSearch.Focus();
            txtItemSearch.Enabled = false;
            //decimal Quantity = Convert.ToDecimal(dtdtl.Rows[0]["qnty"].ToString());
            //if (Quantity > 0)
            //{
            //    if (dtdtl.Rows.Count > 0)
            //    {

            //        string find = "";
            //        string flag = "";

            //        foreach (DataRow data in dt.Rows)
            //        {
            //            if (data["ID"].ToString() == dtdtl.Rows[0]["Id"].ToString() && data["item_Code"].ToString() == dtdtl.Rows[0]["item_Code"].ToString() && data["item_desc"].ToString() == dtdtl.Rows[0]["item_desc"].ToString())
            //            {
            //                if (Quantity > Convert.ToDecimal(data["qnty"].ToString()))
            //                {
            //                    flag = "Y";
            //                    ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('Allready Add This Items...!!');", true);
            //                }


            //            }

            //            if (data["ID"].ToString() == "" && data["item_desc"].ToString() == "")
            //            {
            //                find = "Y";

            //            }
            //        }
            //        if (find == "Y")
            //        {
            //            dt.Rows.Remove(dt.Rows[0]);

            //        }
            //        if (flag != "Y")
            //        {
            //            //DataRow dr = dt.NewRow();

            //            //dr["ID"] = dtdtl.Rows[0]["Id"].ToString();
            //            //dr["item_Code"] = dtdtl.Rows[0]["item_Code"].ToString();
            //            //dr["item_desc"] = dtdtl.Rows[0]["item_desc"].ToString();
            //            //dr["qnty"] = "1";
            //            //dr["ItemId"] = dtdtl.Rows[0]["ItemId"].ToString();
            //            //dr["Total_Requisition"] = dtdtl.Rows[0]["Total_Requisition"].ToString();

            //            //dt.Rows.Add(dr);
            //        }
            //        txtItemSearch.Text = "";
            //        txtItemSearch.Focus();
            //        //dgPODetailsDtl.DataSource = dt;
            //        //ViewState["ItemDtl"] = dt;
            //        //dgPODetailsDtl.DataBind();
            //    }
            //}

            //else
            //{
            //    ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('Item Received Quantity Is Over...!!');", true);
            //    txtItemSearch.Text = "";
            //    txtItemSearch.Focus();
            //    return;
            //}
            txtItemSearch.Focus();
            ShowFooterTotal(dt, dgPODetailsDtl);
            ////UP4.Update();
            ////UP44.Update();
            //txtItemSearch.Focus();
        }
        catch (Exception ex)
        {


            ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "jsAlert('" + ex.Message + "','red',0);", true);
        }

    }
    protected void BtnReset_Click(object sender, EventArgs e)
    {
        Refresh();
        txtItemSearch.Enabled = false;
        txtItemSearch.Text = "";

        
    }
    protected void BtnSave_Click(object sender, EventArgs e)
    {
        //var item_code = Session["item_code"];
        //if (string.IsNullOrEmpty(item_code.ToString()))
        //{
        //    ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('Please Select Ihtem Code...!!');", true);
        //    return;
        //}

        DataTable dtDtlGrid = (DataTable)ViewState["ItemDtl"];
       _AuthReqManager.UpdateRequirimentItemstock(dtDtlGrid, hidMstId.Value);
        ClientScript.RegisterStartupScript(this.GetType(), "Warning", "alert('Record has been Autheriz successfully...!!');", true);
        Refresh();
        pnl.ActiveTabIndex = 1;

    }

    protected void dgHistory_RowDataBound(object sender, GridViewRowEventArgs e)
    {


        if (e.Row.RowType == DataControlRowType.DataRow | e.Row.RowType == DataControlRowType.Header | e.Row.RowType == DataControlRowType.Footer)
        {
            e.Row.Cells[1].Attributes.Add("style", "display:none");

        } 

    }




    protected void dghostoryChangeItem(object sender, EventArgs e)    
    {
        hidMstId.Value = dgHistory.SelectedRow.Cells[1].Text;
    
        DataTable dt = _AuthReqManager.getShowAllDatematerial(hidMstId.Value);
        txtSite.Text = dt.Rows[0]["Address"].ToString();
        txtItemSearch.Text = dt.Rows[0]["RequisitionNo"].ToString();
        ddlProjectName.SelectedValue = dt.Rows[0]["ProjectId"].ToString();



        DataTable dt1 = _AuthReqManager.UpdateMaterialValues(hidMstId.Value);
        dgPODetailsDtl.DataSource = dt1;
        dgPODetailsDtl.DataBind();
        pnl.ActiveTabIndex = 0;
        ShowFooterTotal(dt1, dgPODetailsDtl);
        ViewState["ItemDtl"] = dt1;
       

    }


    
}