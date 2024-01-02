using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KHSC.Manager.Others;
using KHSC.DAO.Others;

    public partial class SpacializedAreaEntryUI : System.Web.UI.Page
    {
        SpecializedManager aSpecializedManagerObj = new SpecializedManager();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack != true)
            {
                RefreshAll();
                
            }
        }

        private void RefreshAll()
        {
            SpecializedIdTextBox.Text = "";
            SpecializedAreaNameTextBox.Text = "";
            SpecializedAreaGridView.DataSource = aSpecializedManagerObj.GetAllSpecializedInformation();
            SpecializedAreaGridView.DataBind();
            SpecializedIdTextBox.Text = aSpecializedManagerObj.GetSpecializedAutoId();
            DeleteButton.Visible = false;
            UpdateButton.Visible = false;
            SaveButton.Visible = true;
        }

        protected void DeptSaveButton_Click(object sender, EventArgs e)
        {
            try
            {
                Specialized aSpecializedObj = new Specialized();
                aSpecializedObj.Id = SpecializedIdTextBox.Text;
                aSpecializedObj.Name = SpecializedAreaNameTextBox.Text;
                aSpecializedManagerObj.SaveTheSpecializedInformation(aSpecializedObj);
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

        protected void CloseButton_Click(object sender, EventArgs e)
        {
            RefreshAll();
        }

        
        protected void UpdateButton_Click(object sender, EventArgs e)
        {
            try
            {
                Specialized aSpecializedObj = new Specialized();
                aSpecializedObj.Id = SpecializedIdTextBox.Text;
                aSpecializedObj.Name = SpecializedAreaNameTextBox.Text;
                aSpecializedManagerObj.UpdateTheSpecialized(aSpecializedObj);
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
                Specialized aSpecializedObj = new Specialized();
                aSpecializedObj.Id = SpecializedIdTextBox.Text;
                aSpecializedManagerObj.DeleteTheSpecialized(aSpecializedObj);
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
        protected void SpecializedAreaGridView_SelectedIndexChanged(object sender, EventArgs e)
        {
            SpecializedIdTextBox.Text = SpecializedAreaGridView.SelectedRow.Cells[0].Text;
            SpecializedAreaNameTextBox.Text = SpecializedAreaGridView.SelectedRow.Cells[1].Text;
            DeleteButton.Visible = true;
            UpdateButton.Visible = true;
            SaveButton.Visible = false;
        }
}
