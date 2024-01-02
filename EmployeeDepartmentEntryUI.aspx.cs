using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KHSC.DAO.Others;
using KHSC.Manager.Others;


 
    public partial class EmployeeDepartmentEntryUI : System.Web.UI.Page
    {
        DepartmentManager aDepartmentManagerObj = new DepartmentManager();
       
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack != true)
            {
                RefreshAll();
            }
        }

        private void RefreshAll()
        {
            DepartmentIdTextBox.Text = "";
            DepartmentNameTextBox.Text = "";
            DepartmentGridview.DataSource = aDepartmentManagerObj.GetAllDepartmentInformation();
            DepartmentGridview.DataBind();
            DepartmentIdTextBox.Text = aDepartmentManagerObj.GetDepartmentAutoId();

            DeleteButton.Visible = false;
            UpdateButton.Visible = false;
            DeptSaveButton.Visible = true;
        }

        protected void DeptSaveButton_Click(object sender, EventArgs e)
        {
            try
            {
                Department aDepartmentObj = new Department();
                aDepartmentObj.Id = DepartmentIdTextBox.Text;
                aDepartmentObj.Name = DepartmentNameTextBox.Text;
                aDepartmentManagerObj.SaveTheDepartmentInformation(aDepartmentObj);
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

        protected void CloseButton_Click(object sender, EventArgs e)
        {
            RefreshAll();
        }
        protected void UpdateButton_Click(object sender, EventArgs e)
        {
            try
            {
                Department aDepartmentObj = new Department();
                aDepartmentObj.Id = DepartmentIdTextBox.Text;
                aDepartmentObj.Name = DepartmentNameTextBox.Text;
                aDepartmentManagerObj.UpdateTheDept(aDepartmentObj);
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
                Department aDepartmentObj = new Department();
                aDepartmentObj.Id = DepartmentIdTextBox.Text;
                aDepartmentManagerObj.DeleteTheDept(aDepartmentObj);
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
        protected void DepartmentGridview_SelectedIndexChanged(object sender, EventArgs e)
        {
            DepartmentIdTextBox.Text = DepartmentGridview.SelectedRow.Cells[0].Text;
            DepartmentNameTextBox.Text = DepartmentGridview.SelectedRow.Cells[1].Text;
            DeleteButton.Visible = true;
            UpdateButton.Visible = true;
            DeptSaveButton.Visible = false;
        }
}
 