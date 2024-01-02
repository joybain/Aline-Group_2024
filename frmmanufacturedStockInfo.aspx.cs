using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using autouniv;

public partial class frmmanufacturedStockInfo : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Populate();
            TreeView1.CollapseAll();
           dghistory.Visible = false;
           dghistory.DataSource = null;
           ViewState["manufacture"] = null;
           dghistory.DataBind();
        }
    }
    protected void TreeView1_SelectedNodeChanged(object sender, EventArgs e)
    {
        //lblreceivedFianl.Text = TreeView1.SelectedNode.Text + " (Fianl - Received) ";
        DataTable dtproduction = IdManager.GetShowDataTable("Select top(1) *  from ProductionSetup where Code='" + TreeView1.SelectedValue + "'  ");       
        DataTable dt = ManufactureInformationManager.GetManufactureStock("", TreeView1.SelectedValue);
        dghistory.DataSource = dt;
        ViewState["manufacture"] = dt;
        dghistory.Visible = true;
        dghistory.DataBind();
    }

    public void Populate()
    {
        DataTable List = IdManager.GetShowDataTable("Select SerialNo,ProductionStageName as Name,Code from ProductionSetup where Status=1 order by SerialNo");
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
}