using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using autouniv;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.draw;
using OldColor;

public partial class frmBondingRequisition : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Populate();
            TreeView1.CollapseAll();
            pnlReceivedFinal.Visible = false;
           // pnlreceived.Visible = false;
            getEmptyDtl();
            getEmptyDtlfinal();
            getEmptyDtlConsumtion();

            dghistory.DataSource = null;
            ViewState["manufacture"] = null;
            dghistory.DataBind();
            string query = "Select null as ID,'' Name union select ID,F_Name+'-'+L_Name from dbo.PMIS_PERSONNEL order by 1";
            util.PopulationDropDownList(ddlManufacturedBy, "PMIS_PERSONNEL", query, "Name", "ID");

            string query22 = "SELECT  [ID] ,[Name] FROM [View_UOM]";
           // util.PopulationDropDownList(ddlUmo, "UOM", query22, "Name", "ID");

            string query1 = "select ID,Thikness from ProductionThikness order by 1";
            string query2 = "select ID,Width from ProductionWidth order by 1";
            string query3 = "select ID,Length from ProductionLength order by 1";
            //util.PopulationDropDownList(ddlThiknessConsumption, "ProductionThikness", query1, "Thikness", "ID");
           // util.PopulationDropDownList(ddlThiknessReceived, "ProductionThikness", query1, "Thikness", "ID");
            util.PopulationDropDownList(ddlThiknessReceivedFinal, "ProductionThikness", query1, "Thikness", "ID");

           // util.PopulationDropDownList(ddlWidthConsumption, "ProductionWidth", query2, "width", "ID");
           // util.PopulationDropDownList(ddlWidthReceived, "ProductionWidth", query2, "width", "ID");
            util.PopulationDropDownList(ddlWidthReceivedFinal, "ProductionWidth", query2, "width", "ID");


           // util.PopulationDropDownList(ddllenthConsumption, "ProductionLength", query3, "Length", "ID");
           // util.PopulationDropDownList(ddllenthReceived, "ProductionLength", query3, "Length", "ID");
            util.PopulationDropDownList(ddllenthReceivedFinal, "ProductionLength", query3, "Length", "ID");
            txtManufactureCode.Text = IdManager.GetDateTimeWiseSerial("Re", "ID", "[ItemBondingRequisitionMst]");
            txtExpDeliveryDate.Text = System.DateTime.Now.Date.ToString("dd/MM/yyyy");
            txtManufactureDate.Text = System.DateTime.Now.Date.ToString("dd/MM/yyyy");
        }

    }

    public void Populate()
    {
        //DataTable List = IdManager.GetShowDataTable("Select top(2) SerialNo,case when SerialNo=1 then 'Bonding' else 'Cutting' end as Name,Code from ProductionSetup where Status=1 order by SerialNo");
        DataTable List = IdManager.GetShowDataTable("Select SerialNo,ProductionStageName Name,Code  from ProductionSetup where Status=1 and  parentcode!=0  order by SerialNo");
        TreeNode newNode;
        foreach (DataRow row in List.Rows)
        {
            newNode = new TreeNode();
            newNode.Text = row["Code"].ToString() + " - " + row["Name"].ToString();
            newNode.Value = row["Code"].ToString();
            TreeView1.Nodes.Add(newNode);
            //if (row["rootleaf"].ToString() == "R")
            //{
            //    PopChild(row["seg_coa_code"].ToString(), newNode);
            //}
        }
    }
    public void PopChild(string segcode, TreeNode node)
    {
        //DataTable dt = SegCoaManager.GetSegCoaChild("parent_code='" + segcode + "' order by convert(numeric,nullif(seg_coa_code,''))");
        //TreeNode newNode;

        //foreach (DataRow dr in dt.Rows)
        //{
        //    newNode = new TreeNode();
        //    newNode.Text = dr["seg_coa_code"].ToString() + " - " + dr["seg_coa_desc"].ToString();
        //    newNode.Value = dr["seg_coa_code"].ToString();
        //    node.ChildNodes.Add(newNode);
        //    if (dr["rootleaf"].ToString() == "R")
        //    {
        //        PopChild(dr["seg_coa_code"].ToString(), newNode);
        //    }
        //}
    }
    protected void TreeView1_SelectedNodeChanged(object sender, EventArgs e)
    {
       // lblrecivedPnl.Text = TreeView1.SelectedNode.Text + " (Raw Materials) ";
       // lblConsumption.Text = TreeView1.SelectedNode.Text + " (Raw Materials) ";
        hfMstID.Value = "";
        lblreceivedFianl.Text = TreeView1.SelectedNode.Text ;
        DataTable dtproduction = IdManager.GetShowDataTable("Select top(1) *  from ProductionSetup where Code='" + TreeView1.SelectedValue + "'  ");
        Session["ParentCode"] = dtproduction.Rows[0]["ParentCode"].ToString();
        Session["ProductionCode"] = dtproduction.Rows[0]["Code"].ToString();
        Session["SerialNo"] = dtproduction.Rows[0]["Code"].ToString();
        Session["ReSerialNo"] = dtproduction.Rows[0]["SerialNo"].ToString();
        getEmptyDtlConsumtion();
        getEmptyDtlfinal();
        getEmptyDtl();
        if (TreeView1.SelectedValue == "001")
        {
            //lblThikness.Visible = false;
            //ddlThiknessConsumption.Visible = false;
            //LblLength.Visible = false;
            //ddllenthConsumption.Visible = false;
            //LblWidth.Visible = false;
            //ddlWidthConsumption.Visible = false;
            //pnlreceived.Visible = false;
            //pnlConsumption.Visible = true;

        }
        else
        {
            //lblThikness.Visible = true;
            //ddlThiknessConsumption.Visible = true;
            //LblLength.Visible = true;
            //ddllenthConsumption.Visible = true;
            //LblWidth.Visible = true;
            //ddlWidthConsumption.Visible = true;
            //pnlreceived.Visible = true;
            //pnlConsumption.Visible = false;
        }

        //if (dtproduction.Rows[0]["FinalStatge"].ToString() != "True" && dtproduction.Rows[0]["FinalStatge"].ToString() != "1" && dtproduction.Rows[0]["FinalStatge"].ToString()=="")
        //{
        //    pnlReceivedFinal.Visible = false;
        //    pnlreceived.Visible = pnlConsumption.Visible = true;


        //}
        //else
        {
            pnlReceivedFinal.Visible = true;

        }
        DataTable dt = BondingRequisitionManager.GetInformationHistory("", TreeView1.SelectedValue);
         
        dghistory.DataSource = dt;
        ViewState["manufacture"] = dt;
        dghistory.DataBind();

    }
    
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        try
        {
            //AddItemDetailsReceived(lblItemID, txtItemReceived, ddllenthReceived, ddlWidthReceived, ddlThiknessReceived, txtquantityReceived, "FNreceivedDetails", dgReceived);
            //txtItemReceived.Focus();
            //AddItemDetailsConsumption(lblItemID, txtItemReceived, ddllenthReceived, ddlWidthReceived, ddlThiknessReceived, txtquantityReceived, "FNreceivedDetails", dgReceived, lblItemProductionCode);
            //txtItemConsumption.Focus();
            // ShowFooterTotal();

        }
        catch (Exception ex)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "Warning", "alert('" + ex + "');", true);
        }
    }

    private void AddItemDetailsConsumption(Label ItemID, TextBox ItemConsumption, DropDownList lenthConsumption, DropDownList WidthConsumption, DropDownList ThiknessConsumption, TextBox quantityConsumption, string viewstate, GridView Consumption, Label lblItemProductionCode)
    {
        if (ItemID.Text == "")
        {
            ClientScript.RegisterStartupScript(this.GetType(), "ale", "alert('PLease Select Item...!!');", true);
            return;
        }
        if (string.IsNullOrEmpty(lenthConsumption.SelectedValue))
        {
            ClientScript.RegisterStartupScript(this.GetType(), "ale", "alert('PLease Select Length..!!');", true);
            return;
        }
        if (string.IsNullOrEmpty(WidthConsumption.SelectedValue))
        {
            ClientScript.RegisterStartupScript(this.GetType(), "ale", "alert('PLease Select Width..!!');", true);
            return;
        }

        if (string.IsNullOrEmpty(ThiknessConsumption.SelectedValue))
        {
            ClientScript.RegisterStartupScript(this.GetType(), "ale", "alert('PLease Select Thikness...!!');", true);
            return;
        }

        if (quantityConsumption.Text == "" || quantityConsumption.Text == "0")
        {
            ClientScript.RegisterStartupScript(this.GetType(), "ale", "alert('PLease Input Quantity ...!!');", true);
            return;
        }
        if (Session["ParentCode"] != null && Session["ParentCode"].ToString() != "0")
        {
            DataTable dtproduction = IdManager.GetShowDataTable("Select top(1) *  from ProductionSetup where Code='" + TreeView1.SelectedValue + "'  ");

            string serialNo = dtproduction.Rows[0]["SerialNo"].ToString();

            DataTable dt = (DataTable)ViewState[viewstate];
            DataTable dtitemstock = IdManager.GetShowDataTable("Select top(1) *  from Manufacturing_Stock where ItemID='" + ItemID.Text + "' and WidthID='" + WidthConsumption.Text + "' and LengthID='" + lenthConsumption.Text + "' and ThiknessID='" + ThiknessConsumption.Text + "' and  ProductionSerialNo<=Convert(int,'" + serialNo + "') ");
            if (dtitemstock.Rows.Count > 0)
            {
                if (Convert.ToDecimal(dtitemstock.Rows[0]["ClosingStock"].ToString()) >= Convert.ToDecimal(quantityConsumption.Text))
                {
                    string found = "";
                    foreach (DataRow drd in dt.Rows)
                    {
                        if (drd["ProductionCode"].ToString() == dtitemstock.Rows[0]["ProductionCode"].ToString() && drd["ItemID"].ToString() == ItemID.Text && drd["LengthID"].ToString() == lenthConsumption.SelectedValue && drd["WidthID"].ToString() == WidthConsumption.SelectedValue && drd["ThiknessID"].ToString() == ThiknessConsumption.SelectedValue)
                        {
                            drd["Quantity"] = quantityConsumption.Text;
                            found = "Y";
                        }
                    }
                    if (found == "")
                    {


                        DataRow drd = dt.NewRow();
                        drd["ProductionCode"] = dtitemstock.Rows[0]["ProductionCode"].ToString();
                        drd["ParentCode"] = dtitemstock.Rows[0]["ParentCode"].ToString();
                        drd["ProductionSerialNo"] = dtitemstock.Rows[0]["ProductionSerialNo"].ToString();
                        drd["ItemID"] = ItemID.Text;
                        drd["Name"] = ItemConsumption.Text;
                        drd["LengthID"] = lenthConsumption.SelectedValue;
                        drd["WidthID"] = WidthConsumption.SelectedValue;
                        drd["ThiknessID"] = ThiknessConsumption.SelectedValue;
                        drd["Length"] = lenthConsumption.SelectedItem.Text.Replace(",", "").Trim();
                        drd["Width"] = WidthConsumption.SelectedItem.Text.Replace(",", "").Trim();
                        drd["Thikness"] = ThiknessConsumption.SelectedItem.Text.Replace(",", "").Trim();
                        drd["Quantity"] = quantityConsumption.Text;
                        dt.Rows.Add(drd);
                    }
                    Consumption.DataSource = dt;
                    Consumption.DataBind();
                    ViewState[viewstate] = dt;
                    ItemID.Text = ItemConsumption.Text = "";
                    quantityConsumption.Text = "0";
                    lenthConsumption.SelectedIndex = WidthConsumption.SelectedIndex = ThiknessConsumption.SelectedIndex = -1;
                }
                else
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "ale", "alert('Quantity Over then Stock Quantity...!!');", true);
                    quantityConsumption.Focus();
                }
            }
            else
            {
                ClientScript.RegisterStartupScript(this.GetType(), "ale", "alert('Can't Find This Item...!!');", true);
                quantityConsumption.Focus();
            }

        }



    }

    private void AddItemDetailsReceived(Label ItemID, TextBox ItemReceived, DropDownList lenthReceived, DropDownList WidthReceived, DropDownList ThiknessReceived, TextBox quantityReceived, string viewstate, GridView Received)
    {
        try
        {
            if (ItemID.Text == "")
            {
                ClientScript.RegisterStartupScript(this.GetType(), "ale", "alert('PLease Select Item...!!');", true);
                return;
            }
            //if (lenthReceived.SelectedValue == null || lenthReceived.SelectedValue == "")
            //{
            //    ClientScript.RegisterStartupScript(this.GetType(), "ale", "alert('PLease Select Length..!!');", true);
            //    return;
            //}
            //if (WidthReceived.SelectedValue == null || WidthReceived.SelectedValue == "")
            //{
            //    ClientScript.RegisterStartupScript(this.GetType(), "ale", "alert('PLease Select Width..!!');", true);
            //    return;
            //}

            //if (ThiknessReceived.SelectedValue == null || ThiknessReceived.SelectedValue == "")
            //{
            //    ClientScript.RegisterStartupScript(this.GetType(), "ale", "alert('PLease Select Thikness...!!');", true);
            //    return;
            //}

            if (quantityReceived.Text == "" || quantityReceived.Text == "0")
            {
                ClientScript.RegisterStartupScript(this.GetType(), "ale", "alert('PLease Input Quantity ...!!');", true);
                return;
            }

            DataTable dtproduction = IdManager.GetShowDataTable("Select top(1) *  from ProductionSetup where Code='" + TreeView1.SelectedValue + "'  ");

            DataTable dtitemstock = IdManager.GetShowDataTable(@"Select top(1) tt.ID,tt.ItemID,tt.ClosingStock,tt.lengthID,tt.WidthID,tt.ThiknessID,ISNULL(l.Length, '') as Length, ISNULL(w.Width, '') as Width ,ISNULL(t.Thikness, '') as Thikness from Manufacturing_Stock tt LEFT OUTER JOIN
                         dbo.ProductionLength AS l ON l.ID = tt.lengthID LEFT OUTER JOIN
                         dbo.ProductionWidth AS w ON w.ID = tt.widthID LEFT OUTER JOIN
                         dbo.ProductionThikness AS t ON t.ID = tt.thiknessID where tt.ID='" + ItemID.Text + "' ");

            if (dtproduction.Rows.Count > 0)
            {
                if ((dtproduction.Rows[0]["FinalStatge"].ToString() == "True" || dtproduction.Rows[0]["FinalStatge"].ToString() == "1") && dtproduction.Rows[0]["FinalStatge"].ToString() != "")
                {

                    DataTable dt = (DataTable)ViewState[viewstate];

                    string found = "";
                    foreach (DataRow drd in dt.Rows)
                    {
                        if (drd["ItemID"].ToString() == ItemID.Text && drd["LengthID"].ToString() == lenthReceived.SelectedValue && drd["WidthID"].ToString() == WidthReceived.SelectedValue && drd["ThiknessID"].ToString() == ThiknessReceived.SelectedValue)
                        {
                            drd["Quantity"] = quantityReceived.Text;
                            found = "Y";
                        }
                    }
                    if (found == "")
                    {
                        DataRow drd = dt.NewRow();
                        drd["ProductionCode"] = TreeView1.SelectedValue;
                        drd["ParentCode"] = dtproduction.Rows[0]["ParentCode"].ToString();
                        drd["ProductionSerialNo"] = dtproduction.Rows[0]["SerialNo"].ToString();
                        drd["FinalStatge"] = "1";
                        drd["Goods_ItemID"] = ItemID.Text;
                        drd["ItemID"] = ItemID.Text;
                        drd["Name"] = ItemReceived.Text;
                        //drd["LengthID"] = lenthReceived.SelectedValue;
                        //drd["WidthID"] = WidthReceived.SelectedValue;
                        //drd["ThiknessID"] = ThiknessReceived.SelectedValue;
                        //drd["Length"] = lenthReceived.SelectedItem.Text;
                        //drd["Width"] = WidthReceived.SelectedItem.Text;
                        //drd["Thikness"] = ThiknessReceived.SelectedItem.Text;
                        if (!string.IsNullOrEmpty(dtitemstock.Rows[0]["lengthID"].ToString()))
                        {
                            drd["LengthID"] = dtitemstock.Rows[0]["lengthID"].ToString();

                        }
                        if (!string.IsNullOrEmpty(dtitemstock.Rows[0]["widthID"].ToString()))
                        {
                            drd["WidthID"] = dtitemstock.Rows[0]["widthID"].ToString();

                        }
                        if (!string.IsNullOrEmpty(dtitemstock.Rows[0]["thiknessID"].ToString()))
                        {
                            drd["ThiknessID"] = dtitemstock.Rows[0]["thiknessID"].ToString();

                        }

                        drd["Length"] = dtitemstock.Rows[0]["length"].ToString();
                        drd["Width"] = dtitemstock.Rows[0]["width"].ToString();
                        drd["Thikness"] = dtitemstock.Rows[0]["thikness"].ToString();
                       
                        drd["Quantity"] = quantityReceived.Text;
                        dt.Rows.Add(drd);
                    }
                    Received.DataSource = dt;
                    Received.DataBind();
                    ViewState[viewstate] = dt;
                    ItemID.Text = ItemReceived.Text = "";
                    quantityReceived.Text = "0";
                    lenthReceived.SelectedIndex = WidthReceived.SelectedIndex = ThiknessReceived.SelectedIndex = -1;
                }
                else
                {
                    DataTable dt = (DataTable)ViewState[viewstate];
                  
                    string found = "";
                    foreach (DataRow drd in dt.Rows)
                    {
                        if (drd["ItemID"].ToString() == ItemID.Text && drd["LengthID"].ToString() == lenthReceived.SelectedValue && drd["WidthID"].ToString() == WidthReceived.SelectedValue && drd["ThiknessID"].ToString() == ThiknessReceived.SelectedValue)
                        {
                            drd["Quantity"] = quantityReceived.Text;
                            found = "Y";
                        }
                    }
                    if (found == "")
                    {
                        DataRow drd = dt.NewRow();

                        drd["ProductionCode"] = TreeView1.SelectedValue;
                        drd["ParentCode"] = dtproduction.Rows[0]["ParentCode"].ToString();
                        drd["ProductionSerialNo"] = dtproduction.Rows[0]["SerialNo"].ToString();
                        drd["FinalStatge"] = "0";
                        drd["ItemID"] = ItemID.Text;
                        drd["Name"] = ItemReceived.Text;
                        //drd["LengthID"] = lenthReceived.SelectedValue;
                        //drd["WidthID"] = WidthReceived.SelectedValue;
                        //drd["ThiknessID"] = ThiknessReceived.SelectedValue;
                        //drd["Length"] = lenthReceived.SelectedItem.Text;
                        //drd["Width"] = WidthReceived.SelectedItem.Text;
                        //drd["Thikness"] = ThiknessReceived.SelectedItem.Text;
                        if (!string.IsNullOrEmpty(dtitemstock.Rows[0]["lengthID"].ToString()))
                        {
                            drd["LengthID"] = dtitemstock.Rows[0]["lengthID"].ToString();

                        }
                        if (!string.IsNullOrEmpty(dtitemstock.Rows[0]["widthID"].ToString()))
                        {
                            drd["WidthID"] = dtitemstock.Rows[0]["widthID"].ToString();

                        }
                        if (!string.IsNullOrEmpty(dtitemstock.Rows[0]["thiknessID"].ToString()))
                        {
                            drd["ThiknessID"] = dtitemstock.Rows[0]["thiknessID"].ToString();

                        }

                        drd["Length"] = dtitemstock.Rows[0]["length"].ToString();
                        drd["Width"] = dtitemstock.Rows[0]["width"].ToString();
                        drd["Thikness"] = dtitemstock.Rows[0]["thikness"].ToString();
                       
                        drd["Quantity"] = quantityReceived.Text;
                        dt.Rows.Add(drd);
                    }
                    Received.DataSource = dt;
                    Received.DataBind();
                    ViewState[viewstate] = dt;
                    ItemID.Text = ItemReceived.Text = "";
                    quantityReceived.Text = "0";
                    lenthReceived.SelectedIndex = WidthReceived.SelectedIndex = ThiknessReceived.SelectedIndex = -1;
                }
            }

        }
        catch
        {

        }
    }

    private void AddItemDetailsConsumption(Label ItemID, TextBox ItemConsumption, DropDownList lenthConsumption, DropDownList WidthConsumption, DropDownList ThiknessConsumption, TextBox quantityConsumption, string viewstate, GridView Consumption)
    {

        if (ItemID.Text == "")
        {
            ClientScript.RegisterStartupScript(this.GetType(), "ale", "alert('PLease Select Item...!!');", true);
            return;
        }
        if (string.IsNullOrEmpty(lenthConsumption.SelectedValue))
        {
            ClientScript.RegisterStartupScript(this.GetType(), "ale", "alert('PLease Select Length..!!');", true);
            return;
        }
        if (string.IsNullOrEmpty(WidthConsumption.SelectedValue))
        {
            ClientScript.RegisterStartupScript(this.GetType(), "ale", "alert('PLease Select Width..!!');", true);
            return;
        }

        if (string.IsNullOrEmpty(ThiknessConsumption.SelectedValue))
        {
            ClientScript.RegisterStartupScript(this.GetType(), "ale", "alert('PLease Select Thikness...!!');", true);
            return;
        }

        if (quantityConsumption.Text == "" || quantityConsumption.Text == "0")
        {
            ClientScript.RegisterStartupScript(this.GetType(), "ale", "alert('PLease Input Quantity ...!!');", true);
            return;
        }
        if (Session["ParentCode"] != null && Session["ParentCode"].ToString() != "0")
        {

            DataTable dt = (DataTable)ViewState[viewstate];
            DataTable dtitemstock = IdManager.GetShowDataTable("Select top(1) *  from Manufacturing_Stock where ItemID='" + ItemID.Text + "' and WidthID='" + WidthConsumption.Text + "' and LengthID='" + lenthConsumption.Text + "' and ThiknessID='" + ThiknessConsumption.Text + "' and  ProductionCode='" + TreeView1.SelectedValue + "' ");
            if (dtitemstock.Rows.Count > 0)
            {
                if (Convert.ToDecimal(dtitemstock.Rows[0]["ClosingStock"].ToString()) >= Convert.ToDecimal(quantityConsumption.Text))
                {
                    string found = "";
                    foreach (DataRow drd in dt.Rows)
                    {
                        if (drd["ProductionCode"].ToString() == dtitemstock.Rows[0]["ProductionCode"].ToString() && drd["ItemID"].ToString() == ItemID.Text && drd["LengthID"].ToString() == lenthConsumption.SelectedValue && drd["WidthID"].ToString() == WidthConsumption.SelectedValue && drd["ThiknessID"].ToString() == ThiknessConsumption.SelectedValue)
                        {
                            drd["Quantity"] = quantityConsumption.Text;
                            found = "Y";
                        }
                    }
                    if (found == "")
                    {


                        DataRow drd = dt.NewRow();

                        drd["ProductionCode"] = dtitemstock.Rows[0]["ProductionCode"].ToString();
                        drd["ParentCode"] = dtitemstock.Rows[0]["ParentCode"].ToString();
                        drd["ProductionSerialNo"] = dtitemstock.Rows[0]["ProductionSerialNo"].ToString();
                        drd["ItemID"] = ItemID.Text;
                        drd["Name"] = ItemConsumption.Text;
                        drd["LengthID"] = lenthConsumption.SelectedValue;
                        drd["WidthID"] = WidthConsumption.SelectedValue;
                        drd["ThiknessID"] = ThiknessConsumption.SelectedValue;
                        drd["Length"] = lenthConsumption.SelectedItem.Text;
                        drd["Width"] = WidthConsumption.SelectedItem.Text;
                        drd["Thikness"] = ThiknessConsumption.SelectedItem.Text;
                        drd["Quantity"] = quantityConsumption.Text;
                        dt.Rows.Add(drd);
                    }
                    Consumption.DataSource = dt;
                    Consumption.DataBind();
                    ViewState[viewstate] = dt;
                    ItemID.Text = ItemConsumption.Text = "";
                    quantityConsumption.Text = "0";
                    lenthConsumption.SelectedIndex = WidthConsumption.SelectedIndex = ThiknessConsumption.SelectedIndex = -1;
                }
                else
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "ale", "alert('Quantity Over then Stock Quantity...!!');", true);
                    quantityConsumption.Focus();
                }
            }
            else
            {
                ClientScript.RegisterStartupScript(this.GetType(), "ale", "alert('Can't Find This Item...!!');", true);
                quantityConsumption.Focus();
            }

        }

        else if (Session["ParentCode"].ToString() == "0")
        {
            DataTable dt = (DataTable)ViewState[viewstate];
            //  DataTable dtitemstock = IdManager.GetShowDataTable("Select top(1) *  from dbo.Item where ID='" + ItemID.Text + "' ");
            DataTable dtitemstock = IdManager.GetShowDataTable("Select top(1) *  from Manufacturing_Stock where ItemID='" + ItemID.Text + "'  and  ProductionCode='" + TreeView1.SelectedValue + "' ");

            if (dtitemstock.Rows.Count > 0)
            {
                if (Convert.ToDecimal(dtitemstock.Rows[0]["ClosingStock"].ToString()) >= Convert.ToDecimal(quantityConsumption.Text))
                {
                    string found = "";
                    foreach (DataRow drd in dt.Rows)
                    {
                        if (drd["ItemID"].ToString() == ItemID.Text)
                        {
                            drd["Quantity"] = quantityConsumption.Text;
                            found = "Y";
                        }
                    }
                    if (found == "")
                    {


                        DataRow drd = dt.NewRow();

                        drd["ProductionCode"] = dtitemstock.Rows[0]["ProductionCode"].ToString();
                        drd["ParentCode"] = dtitemstock.Rows[0]["ParentCode"].ToString();
                        drd["ProductionSerialNo"] = dtitemstock.Rows[0]["ProductionSerialNo"].ToString();
                        drd["ItemID"] = ItemID.Text;
                        drd["Name"] = ItemConsumption.Text;
                        drd["LengthID"] = lenthConsumption.SelectedValue;
                        drd["WidthID"] = WidthConsumption.SelectedValue;
                        drd["ThiknessID"] = ThiknessConsumption.SelectedValue;
                        drd["Length"] = lenthConsumption.SelectedItem.Text;
                        drd["Width"] = WidthConsumption.SelectedItem.Text;
                        drd["Thikness"] = ThiknessConsumption.SelectedItem.Text;
                        drd["Quantity"] = quantityConsumption.Text;
                        dt.Rows.Add(drd);
                    }
                    Consumption.DataSource = dt;
                    Consumption.DataBind();
                    ViewState[viewstate] = dt;
                    ItemID.Text = ItemConsumption.Text = "";
                    quantityConsumption.Text = "0";
                    lenthConsumption.SelectedIndex = WidthConsumption.SelectedIndex = ThiknessConsumption.SelectedIndex = -1;
                }
                else
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "ale", "alert('Quantity Over then Stock Quantity...!!');", true);
                    quantityConsumption.Focus();
                }
            }
            else
            {
                ClientScript.RegisterStartupScript(this.GetType(), "ale", "alert('Can't Find This Item...!!');", true);
                quantityConsumption.Focus();
            }
        }
    }

    protected void BtnReset_Click(object sender, EventArgs e)
    {

    }

    protected void dgReceived_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        if (ViewState["FNreceivedDetails"] != null)
        {
            DataTable dtDtlGrid = (DataTable)ViewState["FNreceivedDetails"];
           // dtDtlGrid.Rows.RemoveAt(dgReceived.Rows[e.RowIndex].DataItemIndex);
            if (dtDtlGrid.Rows.Count > 0)
            {
                string found = "";

               // dgReceived.DataSource = dtDtlGrid;
                ViewState["FNreceivedDetails"] = dtDtlGrid;
               // dgReceived.DataBind();
            }
            else
            {
                getEmptyDtl();
            }
            //ShowFooterTotal();
        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(), "ale", "alert('Your session is over. Try it again!!');", true);
        }

    }

    private void getEmptyDtl()
    {
        //dgReceived.Visible = true;
        DataTable dtDtlGrid = new DataTable();
        dtDtlGrid.Columns.Add("ID", typeof(string));
        dtDtlGrid.Columns.Add("ItemID", typeof(string));
        dtDtlGrid.Columns.Add("LengthID", typeof(string));
        dtDtlGrid.Columns.Add("WidthID", typeof(string));
        dtDtlGrid.Columns.Add("ThiknessID", typeof(string));
        dtDtlGrid.Columns.Add("Name", typeof(string));
        dtDtlGrid.Columns.Add("Length", typeof(string));
        dtDtlGrid.Columns.Add("Width", typeof(string));
        dtDtlGrid.Columns.Add("Thikness", typeof(string));
        dtDtlGrid.Columns.Add("Quantity", typeof(string));
        dtDtlGrid.Columns.Add("ProductionCode", typeof(string));
        dtDtlGrid.Columns.Add("ParentCode", typeof(string));
        dtDtlGrid.Columns.Add("ProductionSerialNo", typeof(string));

        //dgReceived.DataSource = dtDtlGrid;

        ViewState["FNreceivedDetails"] = dtDtlGrid;

        //dgReceived.DataBind();

    }

    private void getEmptyDtlfinal()
    {
       // dgReceived.Visible = true;
        DataTable dtDtlGrid = new DataTable();
        dtDtlGrid.Columns.Add("ID", typeof(string));
        dtDtlGrid.Columns.Add("ItemID", typeof(string));
        dtDtlGrid.Columns.Add("LengthID", typeof(string));
        dtDtlGrid.Columns.Add("WidthID", typeof(string));
        dtDtlGrid.Columns.Add("ThiknessID", typeof(string));
        dtDtlGrid.Columns.Add("Name", typeof(string));
        dtDtlGrid.Columns.Add("Length", typeof(string));
        dtDtlGrid.Columns.Add("Width", typeof(string));
        dtDtlGrid.Columns.Add("Thikness", typeof(string));
        dtDtlGrid.Columns.Add("Quantity", typeof(string));
        dtDtlGrid.Columns.Add("ProductionCode", typeof(string));
        dtDtlGrid.Columns.Add("ParentCode", typeof(string));
        dtDtlGrid.Columns.Add("ProductionSerialNo", typeof(string));
        dtDtlGrid.Columns.Add("FinalStatge", typeof(string));
        dtDtlGrid.Columns.Add("Goods_ItemID", typeof(string));

        dgReceivedFinal.DataSource = dtDtlGrid;
        ViewState["receivedDetailsFinal"] = dtDtlGrid;

        dgReceivedFinal.DataBind();
    }

    private void getEmptyDtlConsumtion()
    {
        //dgReceived.Visible = true;
        DataTable dtDtlGrid = new DataTable();
        dtDtlGrid.Columns.Add("ID", typeof(string));
        dtDtlGrid.Columns.Add("ItemID", typeof(string));
        dtDtlGrid.Columns.Add("LengthID", typeof(string));
        dtDtlGrid.Columns.Add("WidthID", typeof(string));
        dtDtlGrid.Columns.Add("ThiknessID", typeof(string));
        dtDtlGrid.Columns.Add("Name", typeof(string));
        dtDtlGrid.Columns.Add("Length", typeof(string));
        dtDtlGrid.Columns.Add("Width", typeof(string));
        dtDtlGrid.Columns.Add("Thikness", typeof(string));
        dtDtlGrid.Columns.Add("Quantity", typeof(string));
        dtDtlGrid.Columns.Add("ProductionCode", typeof(string));
        dtDtlGrid.Columns.Add("ParentCode", typeof(string));
        dtDtlGrid.Columns.Add("ProductionSerialNo", typeof(string));

       // dgConsumption.DataSource = dtDtlGrid;
        ViewState["ConsumptionDetails"] = dtDtlGrid;
       // dgConsumption.DataBind();
    }
    protected void BtnClearReceived_Click(object sender, EventArgs e)
    {
        //lblItemID.Text = txtItemReceived.Text = lblItemProductionCode.Text = "";
        //txtquantityReceived.Text = "0";
        //ddllenthReceived.SelectedIndex = ddlWidthReceived.SelectedIndex = ddlThiknessReceived.SelectedIndex = -1;

        //BtnClearReceived(lblItemID, txtItemReceived, ddllenthReceived, ddlWidthReceived, ddlThiknessReceived, txtquantityReceived, "FNreceivedDetails", dgReceived);
    }

    //private void BtnClearReceived(Label ItemID, TextBox ItemReceived, DropDownList lenthReceived, DropDownList WidthReceived, DropDownList ThiknessReceived, TextBox quantityReceived, string viewstate, GridView Received)
    //{
    //    ItemID.Text = ItemReceived.Text = "";
    //    quantityReceived.Text = "0";
    //    lenthReceived.SelectedIndex = WidthReceived.SelectedIndex = ThiknessReceived.SelectedIndex = -1;
    //   // getEmptyDtl();
    //}
    protected void BtnAddConsumption_Click(object sender, EventArgs e)
    {
        //AddItemDetailsConsumption(lblItemIDConsumption, txtItemConsumption, ddllenthConsumption, ddlWidthConsumption, ddlThiknessConsumption, txtquantityConsumption, "ConsumptionDetails", dgConsumption);
        //txtItemConsumption.Focus();

    }
    protected void txtItemReceived_TextChanged(object sender, EventArgs e)
    {
        if (Session["ParentCode"] != null)
        {
            //DataTable dt = IdManager.GetShowDataTable("Select *  from View_SearchManufactureStock where Upper(ItemName)=Upper('" + txtItemReceived.Text + "')  ");
            //if (dt.Rows.Count > 0)
            //{
            //    //txtItemReceived.Text = dt.Rows[0]["ItemName"].ToString();
            //    // lblItemID.Text = dt.Rows[0]["ID"].ToString();
            //    // lblItemProductionCode.Text = dt.Rows[0]["ProductionCode"].ToString();
            //    // ddllenthReceived.SelectedValue = dt.Rows[0]["lengthID"].ToString();
            //    //ddlThiknessReceived.SelectedValue = dt.Rows[0]["thiknessID"].ToString();
            //    //ddlWidthReceived.SelectedValue = dt.Rows[0]["widthID"].ToString();
            //    //ddllenthReceived.Focus();
            //    //  txtquantityReceived.Focus();
            //}
            //else
            //{
            //    //lblItemID.Text = "";
            //}
        }
        else
        {
            //lblItemID.Text = "";
        }
    }
    protected void txtItemConsumption_TextChanged(object sender, EventArgs e)
    {
        if (Session["ParentCode"] != null)
        {
            //DataTable dt = IdManager.GetShowDataTable("Select *  from View_ItemSearch where Upper(ItemName)=Upper('" + txtItemConsumption.Text + "')  ");
            //if (dt.Rows.Count > 0)
            //{
            //   // txtItemConsumption.Text = dt.Rows[0]["ItemName"].ToString();
            //   // lblItemIDConsumption.Text = dt.Rows[0]["ID"].ToString();

            //    //ddlUmo.SelectedValue = dt.Rows[0]["UOMID"].ToString();

            //    if (TreeView1.SelectedValue == "001")
            //    {
            //       // txtquantityConsumption.Focus();
            //    }
            //    else
            //    {
            //       // ddllenthConsumption.Focus();
            //    }
            //}
            //else
            //{
            //   //lblItemIDConsumption.Text = "";
            //}
        }
        else
        {
            //lblItemIDConsumption.Text = "";
        }
    }
    protected void BtnAddReceivedFinal_Click(object sender, EventArgs e)
    {
        try
        {
            AddItemDetailsReceived(lblItemIDFinal, txtItemReceivedFinal, ddllenthReceivedFinal, ddlWidthReceivedFinal, ddlThiknessReceivedFinal, txtquantityReceivedFinal, "receivedDetailsFinal", dgReceivedFinal);

            // ShowFooterTotal();

        }
        catch (Exception ex)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "Warning", "alert('" + ex + "');", true);
        }
    }
    protected void txtItemReceivedFinal_TextChanged(object sender, EventArgs e)
    {
        if (Session["ParentCode"] != null)
        {
            //DataTable dt = IdManager.GetShowDataTable("Select *  from View_ItemSearch where Upper(ItemName)=Upper('" + txtItemReceivedFinal.Text + "') and  ItemsType in (2) ");
            //DataTable dt = IdManager.GetShowDataTable("Select *  from View_SearchManufactureStock where Upper(ItemName)=Upper('" + txtItemReceivedFinal.Text + "')  ");
            DataTable dt = IdManager.GetShowDataTable("Select *  from View_SearchManufactureStock where Upper(SearchItemName)=Upper('" + txtItemReceivedFinal.Text + "')  ");
            if (dt.Rows.Count > 0)
            {
                txtItemReceivedFinal.Text = dt.Rows[0]["ItemName"].ToString();
                lblItemIDFinal.Text = dt.Rows[0]["ID"].ToString();


        
                //ddllenthReceivedFinal.SelectedValue = dt.Rows[0]["lengthID"].ToString();
                //ddlThiknessReceivedFinal.SelectedValue = dt.Rows[0]["thiknessID"].ToString();
                //ddlWidthReceivedFinal.SelectedValue = dt.Rows[0]["widthID"].ToString();
                if (!string.IsNullOrEmpty(dt.Rows[0]["lengthID"].ToString()))
                {
                    ddllenthReceivedFinal.SelectedValue = dt.Rows[0]["lengthID"].ToString();

                }
                if (!string.IsNullOrEmpty(dt.Rows[0]["widthID"].ToString()))
                {
                    ddlWidthReceivedFinal.SelectedValue = dt.Rows[0]["widthID"].ToString();

                }
                if (!string.IsNullOrEmpty(dt.Rows[0]["thiknessID"].ToString()))
                {
                    ddlThiknessReceivedFinal.SelectedValue = dt.Rows[0]["thiknessID"].ToString();

                }
                ddllenthReceivedFinal.Focus();
            }
            else
            {
                lblItemIDFinal.Text = "";
            }
        }
        else
        {
            lblItemIDFinal.Text = "";
        }
    }
    protected void BtnClearReceivedFinal_Click(object sender, EventArgs e)
    {
        lblItemIDFinal.Text = txtItemReceivedFinal.Text = "";
        txtquantityReceivedFinal.Text = "0";
        ddllenthReceivedFinal.SelectedIndex = ddlWidthReceivedFinal.SelectedIndex = ddlThiknessReceivedFinal.SelectedIndex = -1;

    }
    protected void BtnClearConsumption_Click(object sender, EventArgs e)
    {
       // lblItemIDConsumption.Text = txtItemConsumption.Text = "";
       // txtquantityConsumption.Text = "0";
        //ddllenthConsumption.SelectedIndex = ddlWidthConsumption.SelectedIndex = ddlThiknessConsumption.SelectedIndex = -1;

    }
    protected void dgReceivedFinal_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        if (ViewState["receivedDetailsFinal"] != null)
        {
            DataTable dtDtlGrid = (DataTable)ViewState["receivedDetailsFinal"];
            dtDtlGrid.Rows.RemoveAt(dgReceivedFinal.Rows[e.RowIndex].DataItemIndex);
            if (dtDtlGrid.Rows.Count > 0)
            {

                dgReceivedFinal.DataSource = dtDtlGrid;
                ViewState["receivedDetailsFinal"] = dtDtlGrid;
                dgReceivedFinal.DataBind();
            }
            else
            {
                getEmptyDtlfinal();
            }
            //ShowFooterTotal();
        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(), "ale", "alert('Your session is over. Try it again!!');", true);
        }
    }
    protected void dgConsumption_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        if (ViewState["ConsumptionDetails"] != null)
        {
            DataTable dtDtlGrid = (DataTable)ViewState["ConsumptionDetails"];
           // dtDtlGrid.Rows.RemoveAt(dgConsumption.Rows[e.RowIndex].DataItemIndex);
            if (dtDtlGrid.Rows.Count > 0)
            {

                //dgConsumption.DataSource = dtDtlGrid;
                ViewState["ConsumptionDetails"] = dtDtlGrid;
              //  dgConsumption.DataBind();
            }
            else
            {
                getEmptyDtlConsumtion();
            }
            //ShowFooterTotal();
        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(), "ale", "alert('Your session is over. Try it again!!');", true);
        }
    }

    protected void BtnSavess_Click(object sender, EventArgs e)
    {
        try
        {
            //if (string.IsNullOrEmpty(txtManufactureCode.Text))
            //{
            //    ClientScript.RegisterStartupScript(this.GetType(), "ale", "alert('Please Input Manufactured Code..!!');", true);
            //    return;
            //}
            if (string.IsNullOrEmpty(txtManufactureDate.Text))
            {
                ClientScript.RegisterStartupScript(this.GetType(), "ale", "alert('Please Input  Date..!!');", true);
                return;
            }

            if (string.IsNullOrEmpty(txtExpDeliveryDate.Text))
            {
                ClientScript.RegisterStartupScript(this.GetType(), "ale", "alert('Please Exp. Delivery Date..!!');", true);
                return;
            }

            if (string.IsNullOrEmpty(TreeView1.SelectedValue))
            {
                ClientScript.RegisterStartupScript(this.GetType(), "ale", "alert('Please Select Manufacture Option..!!');", true);
                return;
            }

            //if (string.IsNullOrEmpty(ddlManufacturedBy.SelectedValue))
            //{
            //    ClientScript.RegisterStartupScript(this.GetType(), "ale", "alert('Please select Manufactured By..!!');", true);
            //    return;
            //}

            ManufactureInformation aManufactureInformation;
           
            var data = BondingRequisitionManager.GetPurchaseOrderMst(hfMstID.Value);
            if (data.Rows.Count>0)
            {
                if (!string.IsNullOrEmpty(data.Rows[0]["AuthorizedDate"].ToString()))
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "ale", "alert('This Requisition Already Authorized..!!');", true);
                    return;
                }
                aManufactureInformation = new ManufactureInformation();
               // txtManufactureCode.Text = IdManager.GetDateTimeWiseSerial("ID", "ID", "[ItemBondingRequisitionMst]");
                aManufactureInformation.ID = hfMstID.Value;
                aManufactureInformation.GenerateCode = txtManufactureCode.Text;
                aManufactureInformation.Remarks = txtremarks.Text;
                aManufactureInformation.ManufagchuredBy = ddlManufacturedBy.SelectedValue;
                aManufactureInformation.ManufagchuredDate = txtManufactureDate.Text;
                aManufactureInformation.ExpDeliveryDate = txtExpDeliveryDate.Text;

                aManufactureInformation.ProductionSerial = Session["SerialNo"].ToString();

                try
                {

                    aManufactureInformation.AddBy = Session["userID"].ToString();
                }
                catch
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "ale", "alert('Session Out Please Login Again..!!');", true);
                    return;
                }
                string ManufactureCode = "", MstID = "";

            


                var DataDtlInfo = (DataTable)ViewState["receivedDetailsFinal"];
                //ManufactureInformationManager.SaveManufactureInformation(aManufactureInformation, Received, consumption, out ManufactureCode, out MstID);

                BondingRequisitionManager.UpdateRequisitionOrder(aManufactureInformation, DataDtlInfo);

                BtnSavess.Enabled = false;
                ClientScript.RegisterStartupScript(this.GetType(), "ale", "alert('Update Successfully..!!');", true);
              //  Refreshall();


            }




            else
            {

                aManufactureInformation = new ManufactureInformation();
                txtManufactureCode.Text = IdManager.GetDateTimeWiseSerial("Re", "ID", "[ItemBondingRequisitionMst]");
                aManufactureInformation.GenerateCode = txtManufactureCode.Text;
                aManufactureInformation.Remarks = txtremarks.Text;
                aManufactureInformation.ManufagchuredBy = ddlManufacturedBy.SelectedValue;
                aManufactureInformation.ManufagchuredDate = txtManufactureDate.Text;
                aManufactureInformation.ExpDeliveryDate = txtExpDeliveryDate.Text;

                aManufactureInformation.ProductionSerial = Session["SerialNo"].ToString();

                try
                {

                    aManufactureInformation.AddBy = Session["userID"].ToString();
                }
                catch
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "ale", "alert('Session Out Please Login Again..!!');", true);
                    return;
                }
                string ManufactureCode = "", MstID = "";
                var DataDtlInfo = (DataTable)ViewState["receivedDetailsFinal"];
                //ManufactureInformationManager.SaveManufactureInformation(aManufactureInformation, Received, consumption, out ManufactureCode, out MstID);

               MstID= BondingRequisitionManager.SaveManufactureInformation(aManufactureInformation, DataDtlInfo);

                txtManufactureCode.Text = ManufactureCode;
                hfMstID.Value = MstID;
                BtnSavess.Enabled = false;
                ClientScript.RegisterStartupScript(this.GetType(), "ale", "alert('Save Successfully..!!');", true);
               // Refreshall();



            }
        }
        catch (Exception ex)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "ale", "alert('" + ex.Message + "');", true);
        }

    }

    protected void BtnDeletess_Click(object sender, EventArgs e)
    {
        try
        {
            if (string.IsNullOrEmpty(hfMstID.Value))
            {
                ClientScript.RegisterStartupScript(this.GetType(), "ale", "alert('First Select Requistion No..!!');", true);
                return;
            }
               var data = BondingRequisitionManager.GetPurchaseOrderMst(hfMstID.Value);
            if (data.Rows.Count > 0)
            {
                if (!string.IsNullOrEmpty(data.Rows[0]["AuthorizedDate"].ToString()))
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "ale",
                        "alert('This Requisition Already Authorized..!!');", true);
                    return;
                }


                ManufactureInformation aManufactureInformation = new ManufactureInformation();
                // txtManufactureCode.Text = IdManager.GetDateTimeWiseSerial("ID", "ID", "[ItemBondingRequisitionMst]");
                aManufactureInformation.ID = hfMstID.Value;
                aManufactureInformation.GenerateCode = txtManufactureCode.Text;
                aManufactureInformation.Remarks = txtremarks.Text;
                aManufactureInformation.ManufagchuredBy = ddlManufacturedBy.SelectedValue;
                aManufactureInformation.ManufagchuredDate = txtManufactureDate.Text;
                aManufactureInformation.ExpDeliveryDate = txtExpDeliveryDate.Text;

                aManufactureInformation.ProductionSerial = Session["SerialNo"].ToString();

                try
                {

                    aManufactureInformation.AddBy = Session["userID"].ToString();
                }
                catch
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "ale", "alert('Session Out Please Login Again..!!');", true);
                    return;
                }
                string ManufactureCode = "", MstID = "";




                var DataDtlInfo = (DataTable)ViewState["receivedDetailsFinal"];
                //ManufactureInformationManager.SaveManufactureInformation(aManufactureInformation, Received, consumption, out ManufactureCode, out MstID);

                BondingRequisitionManager.DeleteRequisitionOrder(aManufactureInformation, DataDtlInfo);

                BtnSavess.Enabled = false;
                ClientScript.RegisterStartupScript(this.GetType(), "ale", "alert('Delete Successfully..!!');", true);
                //  Refreshall();

            }

        }
        catch (Exception ex)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "Warning", "alert('" + ex.Message + "');", true);

        }

    }
    protected void BtnReset_Click1(object sender, EventArgs e)
    {

        Refreshall();

    }

    private void Refreshall()
    {
        Session["SerialNo"] = "";
        Response.Redirect("frmBondingRequisition.aspx?mno=0.3");
       
    }

    private void Frefreshall()
    {
        ddlManufacturedBy.SelectedIndex = -1;
        txtManufactureCode.Text = txtManufactureCode.Text = "";

        getEmptyDtl();
        getEmptyDtlfinal();
        getEmptyDtlConsumtion();
    }
    protected void dghistory_SelectedIndexChanged(object sender, EventArgs e)
    {
        hfMstID.Value = dghistory.SelectedRow.Cells[1].Text;

        DataTable aManufactureInformation = BondingRequisitionManager.GetManufactureInformation(hfMstID.Value);
        if (aManufactureInformation != null)
        {

            foreach (TreeNode n in TreeView1.Nodes)
            {
                //if (n.Value == aManufactureInformation.ProductionCode)
                //{
                //    n.Select();
                //   // lblrecivedPnl.Text = n.Text + " (Raw Metarials) ";
                //   // lblConsumption.Text = n.Text + " (Raw Metarials) ";
                //    lblreceivedFianl.Text = n.Text + " (Finish Goods) ";
                //    break;
                //}

            }

            txtManufactureCode.Text = aManufactureInformation.Rows[0]["Code"].ToString();
            txtManufactureDate.Text = aManufactureInformation.Rows[0]["ManufagchuredDate"].ToString();
            txtExpDeliveryDate.Text = aManufactureInformation.Rows[0]["ExpDeliveryDate"].ToString();

            txtremarks.Text = aManufactureInformation.Rows[0]["Remarks"].ToString();

            try
            {
                ddlManufacturedBy.SelectedValue = aManufactureInformation.Rows[0]["RequisitionBy"].ToString();
            }
            catch 
            {

                ddlManufacturedBy.SelectedIndex = -1;
            }
            

            
            DataTable Received = BondingRequisitionManager.GetReceivedDetailsInfo(hfMstID.Value);

           
            pnlReceivedFinal.Visible = true;
            ViewState["receivedDetailsFinal"] = Received;
            dgReceivedFinal.DataSource = Received;
            dgReceivedFinal.DataBind();

        }
        else
        {
            hfMstID.Value = "";
        }
    }
    protected void dghistory_RowDataBound(object sender, GridViewRowEventArgs e)
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
    }

    protected void btnPrint_Click(object sender, EventArgs e)
    {
        try
        {
            if (!string.IsNullOrEmpty(hfMstID.Value))
            {
                getPurchaseOrder(); 
            }
            else
            {
                ClientScript.RegisterStartupScript(this.GetType(), "ale", "alert('First Select Requisition No..!!');", true);
                
            }
        }
        catch (Exception ex)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "Warning", "alert('" + ex.Message + "');", true);

        }
    }

    public void getPurchaseOrder()
    {
        string filename = "RE_" + TreeView1.SelectedNode.Text;
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
        cell = new PdfPCell(new Phrase("Requisition Order("+TreeView1.SelectedNode.Text+")", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 10, iTextSharp.text.Font.BOLD)));
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
        cell = new PdfPCell(FormatPhrase(": "));
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
        cell = new PdfPCell(FormatPhrase(": " + txtManufactureCode.Text));
        cell.BorderWidth = 0f;
        cell.FixedHeight = 20f;
        pdtpur.AddCell(cell);
        cell = new PdfPCell(FormatHeaderPhrase("Date"));
        cell.BorderWidth = 0f;
        cell.FixedHeight = 20f;
        pdtpur.AddCell(cell);
        cell = new PdfPCell(FormatPhrase(": " + txtManufactureDate.Text));
        cell.BorderWidth = 0f;
        cell.FixedHeight = 20f;
        pdtpur.AddCell(cell);

        cell = new PdfPCell(FormatHeaderPhrase(""));
        cell.BorderWidth = 0f;
        cell.FixedHeight = 20f;
        pdtpur.AddCell(cell);
        cell = new PdfPCell(FormatPhrase(""));
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
        float[] widthdtl = new float[8] { 12, 45, 10,20,20, 20, 20,20 };
        PdfPTable pdtdtl = new PdfPTable(widthdtl);
        pdtdtl.WidthPercentage = 100;

        cell = new PdfPCell(FormatHeaderPhrase("Serial"));
        cell.HorizontalAlignment = 1;
        cell.VerticalAlignment = 1;
        cell.FixedHeight = 20f;
        cell.BorderColor = BaseColor.LIGHT_GRAY;
        pdtdtl.AddCell(cell);
        cell = new PdfPCell(FormatHeaderPhrase("Item Name"));
        cell.HorizontalAlignment = 1;
        cell.VerticalAlignment = 1;
        cell.FixedHeight = 20f;
        cell.BorderColor = BaseColor.LIGHT_GRAY;
        pdtdtl.AddCell(cell);
        cell = new PdfPCell(FormatHeaderPhrase("UOM"));
        cell.HorizontalAlignment = 0;
        cell.VerticalAlignment = 1;
        cell.FixedHeight = 20f;
        cell.BorderColor = BaseColor.LIGHT_GRAY;
        pdtdtl.AddCell(cell);
        cell = new PdfPCell(FormatHeaderPhrase("Brand"));
        cell.HorizontalAlignment = 1;
        cell.VerticalAlignment = 1;
        cell.FixedHeight = 20f;
        cell.BorderColor = BaseColor.LIGHT_GRAY;
        pdtdtl.AddCell(cell);
        cell = new PdfPCell(FormatHeaderPhrase("Length"));
        cell.HorizontalAlignment = 1;
        cell.VerticalAlignment = 1;
        cell.FixedHeight = 20f;
        cell.BorderColor = BaseColor.LIGHT_GRAY;
        pdtdtl.AddCell(cell);
        cell = new PdfPCell(FormatHeaderPhrase("Width"));
        cell.HorizontalAlignment = 1;
        cell.VerticalAlignment = 1;
        cell.FixedHeight = 20f;
        cell.BorderColor = BaseColor.LIGHT_GRAY;
        pdtdtl.AddCell(cell);
        cell = new PdfPCell(FormatHeaderPhrase("Thikness"));
        cell.HorizontalAlignment = 2;
        cell.VerticalAlignment = 1;
        cell.FixedHeight = 20f;
        cell.BorderColor = BaseColor.LIGHT_GRAY;
        pdtdtl.AddCell(cell);
        cell = new PdfPCell(FormatHeaderPhrase("Qty"));
        cell.HorizontalAlignment = 2;
        cell.VerticalAlignment = 1;
        cell.FixedHeight = 20f;
        cell.BorderColor = BaseColor.LIGHT_GRAY;
        pdtdtl.AddCell(cell);
        //DataTable dt = (DataTable)ViewState["reqdtl"];
      //  DataTable dt = RequisitionOrderManager.GetRequisitionOrderItemsDetails(lbLId.Text);

        DataTable dt = BondingRequisitionManager.GetReceivedDetailsAllInfo(hfMstID.Value);
        int Serial = 1;
        decimal totLength = 0;
        decimal totWidth = 0, totThikness = 0, totQuantity=0;
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

                cell = new PdfPCell(FormatPhrase(dr["Name"].ToString()));
                cell.HorizontalAlignment = 1;
                cell.VerticalAlignment = 1;
                cell.BorderColor = BaseColor.LIGHT_GRAY;
                pdtdtl.AddCell(cell);

                cell = new PdfPCell(FormatPhrase(dr["UOM"].ToString()));
                cell.HorizontalAlignment = 0;
                cell.VerticalAlignment = 1;
                cell.BorderColor = BaseColor.LIGHT_GRAY;
                pdtdtl.AddCell(cell);

                cell = new PdfPCell(FormatPhrase(dr["BrandName"].ToString()));
                cell.HorizontalAlignment = 0;
                cell.VerticalAlignment = 1;
                cell.BorderColor = BaseColor.LIGHT_GRAY;
                pdtdtl.AddCell(cell);

                cell = new PdfPCell(FormatPhrase(dr["Length"].ToString()));
                cell.HorizontalAlignment = 1;
                cell.VerticalAlignment = 1;
                cell.BorderColor = BaseColor.LIGHT_GRAY;
                pdtdtl.AddCell(cell);

                cell = new PdfPCell(FormatPhrase(dr["Width"].ToString()));
                cell.HorizontalAlignment = 1;
                cell.VerticalAlignment = 1;
                cell.BorderColor = BaseColor.LIGHT_GRAY;
                pdtdtl.AddCell(cell);

                cell = new PdfPCell(FormatPhrase(dr["Thikness"].ToString()));
                cell.HorizontalAlignment =1;
                cell.VerticalAlignment = 1;
                cell.BorderColor = BaseColor.LIGHT_GRAY;
                pdtdtl.AddCell(cell);


                cell = new PdfPCell(FormatPhrase(dr["Quantity"].ToString()));
                cell.HorizontalAlignment =1;
                cell.VerticalAlignment = 1;
                cell.BorderColor = BaseColor.LIGHT_GRAY;
                pdtdtl.AddCell(cell);
                
                //if (Convert.ToDecimal(dr["ID"]) != 0)
                //{
                //tot += Convert.ToDecimal(dr["Total"]);
                //}
                //if (Convert.ToDecimal(dr["ID"]) != 0)
                //{
                //totLength += Convert.ToDecimal(dr["Length"]);
               // totWidth += Convert.ToDecimal(dr["Width"]);
                //totThikness += Convert.ToDecimal(dr["Thikness"]);
                totQuantity += Convert.ToDecimal(dr["Quantity"]);
                //}
            }
        }

        cell = new PdfPCell(FormatPhrase("Total"));
        cell.FixedHeight = 20f;
        cell.HorizontalAlignment = 2;
        cell.VerticalAlignment = 1;
        cell.BorderColor = BaseColor.LIGHT_GRAY;
        cell.Colspan = 7;
        pdtdtl.AddCell(cell);

        //cell = new PdfPCell(FormatPhrase(totLength.ToString("N2")));
        //// cell.BorderWidth = 0f;
        //cell.FixedHeight = 20f;
        //cell.HorizontalAlignment = 1;
        //cell.VerticalAlignment = 1;
        //cell.BorderColor = BaseColor.LIGHT_GRAY;
        //pdtdtl.AddCell(cell);

        //cell = new PdfPCell(FormatPhrase(totWidth.ToString("N2")));
        ////cell.BorderWidth = 0f;
        //cell.FixedHeight = 20f;
        //cell.HorizontalAlignment = 1;
        //cell.VerticalAlignment = 1;
        //cell.BorderColor = BaseColor.LIGHT_GRAY;
        //pdtdtl.AddCell(cell);


        //cell = new PdfPCell(FormatPhrase(totThikness.ToString("N2")));
        ////cell.BorderWidth = 0f;
        //cell.FixedHeight = 20f;
        //cell.HorizontalAlignment = 1;
        //cell.VerticalAlignment = 1;
        //cell.BorderColor = BaseColor.LIGHT_GRAY;
        //pdtdtl.AddCell(cell);

        cell = new PdfPCell(FormatPhrase(totQuantity.ToString("N2")));
        //cell.BorderWidth = 0f;
        cell.FixedHeight = 20f;
        cell.HorizontalAlignment = 1;
        cell.VerticalAlignment = 1;
        cell.BorderColor = BaseColor.LIGHT_GRAY;
        pdtdtl.AddCell(cell);

        cell = new PdfPCell(FormatPhrase(""));
        //cell.BorderWidth = 0f;
        cell.FixedHeight = 10f;
        cell.HorizontalAlignment = 0;
        cell.VerticalAlignment = 1;
        cell.Border = 0;
        cell.Colspan = 8;
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

  
}