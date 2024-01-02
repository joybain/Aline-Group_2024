using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using autouniv;
using System.Data.SqlClient;

public partial class FrmBtoBTransfer : System.Web.UI.Page
{
    BranchToBranchManager _BranchToBranch = new BranchToBranchManager();
    ProjecctToProject _projectObj = new ProjecctToProject();
    private static Permis per;

    protected void Page_Load(object sender, EventArgs e)
    {
        //if (Session["user"] == null)
        //{
        //    if (Session.SessionID != "" | Session.SessionID != null)
        //    {
        //        clsSession ses = clsSessionManager.getSession(Session.SessionID);
        //        if (ses != null)
        //        {
        //            Session["user"] = ses.UserId;
        //            Session["book"] = "AMB";
        //            string connectionString = DataManager.OraConnString();
        //            SqlDataReader dReader;
        //            SqlConnection conn = new SqlConnection();
        //            conn.ConnectionString = connectionString;
        //            SqlCommand cmd = new SqlCommand();
        //            cmd.Connection = conn;
        //            cmd.CommandType = CommandType.Text;
        //            cmd.CommandText = "Select ID,user_grp,description from utl_userinfo where upper(user_name)=upper('" + Session["user"].ToString().ToUpper() + "') and status='A'";
        //            conn.Open();
        //            dReader = cmd.ExecuteReader();
        //            string wnot = "";
        //            if (dReader.HasRows == true)
        //            {
        //                while (dReader.Read())
        //                {
        //                    Session["userlevel"] = int.Parse(dReader["user_grp"].ToString());
        //                    //Session["dept"] = dReader["dept"].ToString();
        //                    wnot = dReader["description"].ToString();
        //                    Session["userID"] = dReader["ID"].ToString();
        //                }
        //                Session["wnote"] = wnot;

        //                cmd = new SqlCommand();
        //                cmd.Connection = conn;
        //                cmd.CommandType = CommandType.Text;
        //                cmd.CommandText = "Select book_desc,company_address1,company_address2,separator_type,ShortName from gl_set_of_books where book_name='" + Session["book"].ToString() + "' ";
        //                if (dReader.IsClosed == false)
        //                {
        //                    dReader.Close();
        //                }
        //                dReader = cmd.ExecuteReader();
        //                if (dReader.HasRows == true)
        //                {
        //                    while (dReader.Read())
        //                    {
        //                        Session["septype"] = dReader["separator_type"].ToString();
        //                        Session["org"] = dReader["book_desc"].ToString();
        //                        Session["add1"] = dReader["company_address1"].ToString();
        //                        Session["add2"] = dReader["company_address2"].ToString();
        //                        Session["ShotName"] = dReader["ShortName"].ToString();
        //                    }
        //                }
        //            }
        //            dReader.Close();
        //            conn.Close();
        //        }
        //    }
        //}

        //try
        //{
        //    ClientScript.RegisterStartupScript(this.GetType(), "ale", "alert('" + Session.SessionID + "');", true);
        //    string pageName = DataManager.GetCurrentPageName();
        //    string modid = PermisManager.getModuleId(pageName);
        //    per = PermisManager.getUsrPermis(Session["user"].ToString().Trim().ToUpper(), modid);
        //    if (per != null & per.AllowView == "Y")
        //    {
        //        ((Label)Page.Master.FindControl("lblLogin")).Text = Session["wnote"].ToString();
        //        ((LinkButton)Page.Master.FindControl("lbLogout")).Visible = true;
        //    }
        //    else
        //    {
        //        Response.Redirect("Home.aspx?sid=sam");
        //    }
        //}
        //catch
        //{
        //    Response.Redirect("Default.aspx?sid=sam");
        //}

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

       ddlProjectNameTo.DataSource = MeterialRequistitonManager.GetProject();
       ddlProjectNameTo.DataTextField = "ProjectName";
       ddlProjectNameTo.DataValueField = "ID";
       ddlProjectNameTo.DataBind();
       ddlProjectNameTo.Items.Insert(0, "");

       DataTable dtgetEmty = getEmptyDtl();
       dgPODetailsDtl.DataSource = dtgetEmty;
       ViewState["ItemDtl"] = dtgetEmty;
       dgPODetailsDtl.DataBind();
       txtTranstDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
       txtTransCode.Text = IdManager.GetDateTimeWiseSerial("PToP-", "ID", "[ProjectToProjectTransMst]");
       lblQuantity.Text = "";

       DataTable dt = _BranchToBranch.Getdata();
       dgHistory.DataSource = dt;
       dgHistory.DataBind();
       btnUpdate.Visible = false;

   }

    private DataTable getEmptyDtl()
    {
        //dgPODetailsDtl.Visible = true;
        DataTable dtDtlGrid = new DataTable();
        dtDtlGrid.Columns.Add("ID", typeof(string));
        dtDtlGrid.Columns.Add("Name", typeof(string));
        dtDtlGrid.Columns.Add("Code", typeof(string));
        dtDtlGrid.Columns.Add("Uom", typeof(string));
        dtDtlGrid.Columns.Add("qnty", typeof(string));
        dtDtlGrid.Columns.Add("UomId", typeof(string));
        dtDtlGrid.Columns.Add("Remarks", typeof(string));


        DataRow dr = dtDtlGrid.NewRow();
        dtDtlGrid.Rows.Add(dr);
        return dtDtlGrid;

    }

    protected void dgPODetailsDtl_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow | e.Row.RowType == DataControlRowType.Header | e.Row.RowType == DataControlRowType.Footer)
        {
            e.Row.Cells[1].Attributes.Add("style", "display:none");
            e.Row.Cells[5].Attributes.Add("style", "display:none");
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
            cell.ColumnSpan = 2;
            cell.HorizontalAlign = HorizontalAlign.Right;
            row.Cells.Add(cell);


            cell = new TableCell();
            //priceDr = GetQuantity();
            cell.Text = GetQuantity(dt).ToString();
            cell.HorizontalAlign = HorizontalAlign.Center;
            row.Cells.Add(cell);

            lblQuantity.Text = GetQuantity(dt).ToString();


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

    protected void ddlProjectName_SelectedIndexChanged(object sender, EventArgs e)
    {
        var ProjectId = ddlProjectName.SelectedValue;
        Session["projectid"] = ProjectId;
        txtItemSearch.Enabled = true;
       
    }

    protected void txtItemSearch_TextChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlProjectName.SelectedValue == ddlProjectNameTo.SelectedValue) 
            {

                ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('Please Select Different Project ...!!');", true);
                return;
 
            }

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
                        dr["qnty"] = "0";
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
        try
        {
            GridViewRow gvr = (GridViewRow)((TextBox)sender).NamingContainer;
            DataTable dt = (DataTable)ViewState["ItemDtl"];


            if (dt.Rows.Count > 0)
            {
                DataRow dr = dt.Rows[gvr.DataItemIndex];
                int remining = IdManager.GetShowSingleValueInt("isnull(Quntity,0)", "dbo.tbl_ProjectItemStock where ID='" + dr["ID"].ToString() + "' and ProjectId='" + ddlProjectName.SelectedValue + "'");

                int receivedqnt = 0;


                if (Convert.ToDecimal(((TextBox)gvr.FindControl("txtQnty")).Text) <= remining)
                {
                    dr["qnty"] = ((TextBox)gvr.FindControl("txtQnty")).Text;
                }

                else
                {
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('Item  Quantity Is Over...!!');", true);
                    dgPODetailsDtl.DataSource = dt;
                    ViewState["ItemDtl"] = dt;
                    dgPODetailsDtl.DataBind();
                    return;
                }


            }
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





    protected void btnProjectranfer_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(ddlProjectName.SelectedValue))
        {
            ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('Please Select Project Name...!!');", true);
            return;
        }
        if (string.IsNullOrEmpty(ddlProjectNameTo.SelectedValue)) 
        {
            ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('Please Select Project Name...!!');", true);
            return;
        }

        if (ddlProjectName.SelectedValue == ddlProjectNameTo.SelectedValue) 
        {
            if (string.IsNullOrEmpty(ddlProjectNameTo.SelectedValue))
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('Please Select Different Project ...!!');", true);
                return;
            }

        }

        _projectObj.ProjectTransCode = txtTransCode.Text.ToString();
        _projectObj.transferDate = txtTranstDate.Text.ToString();
        _projectObj.formProjectID=int.Parse(ddlProjectName.SelectedValue);
        _projectObj.ToProjectID=int.Parse(ddlProjectNameTo.SelectedValue);
        _projectObj.Remarks=txtRemarks.Text.ToString();
        _projectObj.Addby=Session["user"].ToString();
        _projectObj.TotalQnty = int.Parse(lblQuantity.Text);

        DataTable dtDtlGrid = (DataTable)ViewState["ItemDtl"];

        //ProjectCostingManager.ProjectcostingUpdate(dtDtlGrid, _projectObj);

        _BranchToBranch.ProjectToprojectSave(dtDtlGrid, _projectObj);
        ClientScript.RegisterStartupScript(this.GetType(), "Warning", "alert('Record has been Save successfully...!!');", true);
        Refresh();

    }
    protected void btnClearLost_Click(object sender, EventArgs e)
    {
        Refresh();
    }


    protected void dgHistory_RowDataBound(object sender, GridViewRowEventArgs e)
    {


        if (e.Row.RowType == DataControlRowType.DataRow | e.Row.RowType == DataControlRowType.Header | e.Row.RowType == DataControlRowType.Footer)
        {
            e.Row.Cells[1].Attributes.Add("style", "display:none");

        }
    }


    protected void dgHistory_SelectedIndexChanged(object sender, EventArgs e)
    {
        DgHistoryId.Value = dgHistory.SelectedRow.Cells[1].Text;
        DataTable dt = _BranchToBranch.GetdataMst(DgHistoryId.Value);

        txtTransCode.Text = dt.Rows[0]["TransferCode"].ToString();
        txtTranstDate.Text = dt.Rows[0]["TransferDate"].ToString();
        ddlProjectName.SelectedValue = dt.Rows[0]["FromProjectid"].ToString();
        ddlProjectNameTo.SelectedValue = dt.Rows[0]["ToProjectid"].ToString();
      
        DataTable dt1 = _BranchToBranch.GetDataDtl(DgHistoryId.Value);
        dgPODetailsDtl.DataSource = dt1;
        dgPODetailsDtl.DataBind();
        ShowFooterTotal(dt1, dgPODetailsDtl);
        pnl.ActiveTabIndex = 0;
        txtItemSearch.Enabled = true;
        string Itemcode = txtItemSearch.Text.ToString();
       // int projectId = int.Parse(ddlProjectName.SelectedValue);
       // Session["projectId"] = projectId;
        //ItemUpdateSearch(Itemcode, projectId);
        btnUpdate.Visible = true;



    }

   
}