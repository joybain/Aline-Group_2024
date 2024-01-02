using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KHSC.Manager.Others;
using KHSC.DAO.Others;

 
    public partial class EmployeeDesignationEntryUI : System.Web.UI.Page
    {
        DesignationManager aDesignationManagerObj = new DesignationManager();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack != true)
            {
                RefreshAll();
            }
        }

        private void RefreshAll()
        {
            DesignationIdTextBox.Text = "";
            DesignationNameTextBox.Text = "";
            DesignationGridview.DataSource = aDesignationManagerObj.GetAllDesignationInformation();
            DesignationGridview.DataBind();
            DesignationIdTextBox.Text = aDesignationManagerObj.GetDesignationAutoId();

            DeleteButton.Visible = false;
            UpdateButton.Visible = false;
            DesignationSaveButton.Visible = true;
        }

        protected void CloseButton_Click(object sender, EventArgs e)
        {
            RefreshAll();
        }

        protected void DesignationSaveButton_Click(object sender, EventArgs e)
        {
            try
            {
                Designation aDesignationObj = new Designation();
                aDesignationObj.Id = DesignationIdTextBox.Text;
                aDesignationObj.Name = DesignationNameTextBox.Text;
                aDesignationManagerObj.SaveTheDesignationInformation(aDesignationObj);
                RefreshAll();

                ConfiramationLabel.Text = "Information Have Been Saved Sucessfully";
                ConfiramationLabel.ForeColor = System.Drawing.Color.Green;
                ConfiramationLabel.Font.Bold = true;

            }
            catch (Exception ex)
            {

                ConfiramationLabel.Text = ex.Message;
                ConfiramationLabel.ForeColor = System.Drawing.Color.Red;
                ConfiramationLabel.Font.Bold = true;
            }
            
        }
        protected void UpdateButton_Click(object sender, EventArgs e)
        {
            try
            {
                Designation aDesignationObj = new Designation();
                aDesignationObj.Id = DesignationIdTextBox.Text;
                aDesignationObj.Name = DesignationNameTextBox.Text;
                aDesignationManagerObj.UpdateTheDesig(aDesignationObj);
                RefreshAll();
                ConfiramationLabel.Text = "Information Have Been Udated Sucessfully";
                ConfiramationLabel.ForeColor = System.Drawing.Color.Green;
                ConfiramationLabel.Font.Bold = true;

            }
            catch (Exception ex)
            {

                ConfiramationLabel.Text = ex.Message;
                ConfiramationLabel.ForeColor = System.Drawing.Color.Red;
                ConfiramationLabel.Font.Bold = true;
            }

        }
        protected void DeleteButton_Click(object sender, EventArgs e)
        {
            try
            {
                Designation aDesignationObj = new Designation();
                aDesignationObj.Id = DesignationIdTextBox.Text;
                aDesignationManagerObj.DeleteTheDesig(aDesignationObj);
                RefreshAll();
                ConfiramationLabel.Text = "Information Have Been Deleted Sucessfully";
                ConfiramationLabel.ForeColor = System.Drawing.Color.Green;
                ConfiramationLabel.Font.Bold = true;
            }
            catch (Exception ex)
            {
                ConfiramationLabel.Text = ex.Message;
                ConfiramationLabel.ForeColor = System.Drawing.Color.Red;
                ConfiramationLabel.Font.Bold = true;
            }
        }
        protected void DesignationGridview_SelectedIndexChanged(object sender, EventArgs e)
        {
            DesignationIdTextBox.Text = DesignationGridview.SelectedRow.Cells[0].Text;
            DesignationNameTextBox.Text = DesignationGridview.SelectedRow.Cells[1].Text;
            DeleteButton.Visible = true;
            UpdateButton.Visible = true;
            DesignationSaveButton.Visible = false;
        }
}
 