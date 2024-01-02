using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class frmProjectSetup : System.Web.UI.Page
{
    ProjectModel _valu = new ProjectModel();
    ProjectManager _ProjectManager = new ProjectManager();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            lblTotalHistory.Text = "<h1>Total Items : " + dgProjectHistory.Rows.Count.ToString() + "</h1>";
            RefreshAll();
        }
    }

    private void RefreshAll()
    {

        ddlClient.DataSource = ProjectManager.GetCLient();
        ddlClient.DataTextField = "ContactName";
        ddlClient.DataValueField = "ID";
        ddlClient.DataBind();
        ddlClient.Items.Insert(0, "");
        txtProjectName.Text = txtAddress.Text = "";
        hidenId.Value = "";
        
        var data = _ProjectManager.GateData();
        lblTotalHistory.Text = "<h1>Total Items : " + dgProjectHistory.Rows.Count.ToString() + "</h1>";
        dgProjectHistory.DataSource = data;
        dgProjectHistory.DataBind();
        
    }


    protected void BtnSave_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(ddlClient.SelectedValue))
        {
            ClientScript.RegisterStartupScript(this.GetType(), "ale", "alert('Input  Client Name...!!');", true);
            return;
        }

        if (string.IsNullOrEmpty(hidenId.Value))
        {
            _valu.ClientId = Convert.ToInt32(ddlClient.SelectedValue);
            _valu.ProjectName = txtProjectName.Text.Replace("'", "");
            _valu.Address = txtAddress.Text.Replace("'", "");

            int count = _ProjectManager.save(_valu);
            if (count > 0)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "ale", "alert('Save Succes...!!');", true);
                RefreshAll();
            }
        }
        else
        {
            _valu.Id = Convert.ToInt32(hidenId.Value);
            _valu.ClientId = Convert.ToInt32(ddlClient.SelectedValue);
            _valu.ProjectName = txtProjectName.Text.Replace("'", "");
            _valu.Address = txtAddress.Text.Replace("'", "");

            int count = _ProjectManager.Update(_valu);
            if (count > 0)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "ale", "alert('Update Succes...!!');", true);
                RefreshAll();
            }
        }
    }
   
    protected void BtnDelete_Click(object sender, EventArgs e)
    {
        _valu.Id = Convert.ToInt32(hidenId.Value);

        int count = _ProjectManager.Delete(_valu);
        if (count > 0)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "ale", "alert('Delete Succes...!!');", true);
            RefreshAll();
        }
    }
    protected void dgProjectHistory_SelectedIndexChanged(object sender, EventArgs e)
    {
        hidenId.Value = dgProjectHistory.SelectedRow.Cells[1].Text.Trim();
        DataTable dt = ProjectManager.ShowData(hidenId.Value);
        if (dt != null)
        {
            ddlClient.SelectedValue = dt.Rows[0]["ClientId"].ToString();
            txtProjectName.Text = dt.Rows[0]["ProjectName"].ToString();
            txtAddress.Text = dt.Rows[0]["Address"].ToString();
        }
    }
    protected void dgProjectHistory_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow | e.Row.RowType == DataControlRowType.Header | e.Row.RowType == DataControlRowType.Footer)
        {
            e.Row.Cells[1].Attributes.Add("style", "display:none");
        }
    }
    protected void BtnReset_Click(object sender, EventArgs e)
    {
        RefreshAll();
    }
}