using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KHSC.Manager.Others;
using KHSC.DAO.Others;

 
    public partial class BankNameEntryUI : System.Web.UI.Page
    {
        BankManager aBankManagerObj = new BankManager();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack != true)
            {
                RefreshAll();
            }
        }

        private void RefreshAll()
        {
            BankIdTextBox.Text = "";
            BankNameTextBox.Text = "";
            BankGridView.DataSource = aBankManagerObj.GetAllBankInformation();
            BankGridView.DataBind();
            BankIdTextBox.Text = aBankManagerObj.GetBankAutoId();
            DeleteButton.Visible = false;
            UpdateButton.Visible = false;
            SaveButton.Visible = true;
        }

        protected void CloseButton_Click(object sender, EventArgs e)
        {
            RefreshAll();
        }

        protected void DeptSaveButton_Click(object sender, EventArgs e)
        {
            try
            {
                Bank aBankObj = new Bank();
                aBankObj.Id = BankIdTextBox.Text;
                aBankObj.Name = BankNameTextBox.Text;
                aBankManagerObj.SaveTheBankInformation(aBankObj);
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
                Bank aBankObj = new Bank();
                aBankObj.Id = BankIdTextBox.Text;
                aBankObj.Name = BankNameTextBox.Text;
                aBankManagerObj.UpdateTheBank(aBankObj);
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
                Bank aBankObj = new Bank();
                aBankObj.Id = BankIdTextBox.Text;
                aBankManagerObj.DeleteTheBank(aBankObj);
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
        protected void BankGridView_SelectedIndexChanged(object sender, EventArgs e)
        {
            BankIdTextBox.Text = BankGridView.SelectedRow.Cells[0].Text;
            BankNameTextBox.Text = BankGridView.SelectedRow.Cells[1].Text;
            DeleteButton.Visible = true;
            UpdateButton.Visible = true;
            SaveButton.Visible = false;
        }
       
}
 