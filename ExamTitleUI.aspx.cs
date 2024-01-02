using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KHSC.Manager.Others;
using KHSC.DAO.Others;

    public partial class ExamTitleUI : System.Web.UI.Page
    {
        ExamTitleManager aExamTitleManagerObj = new ExamTitleManager();

        protected void Page_Load(object sender, EventArgs e)
        {
            if(IsPostBack !=true)
            {
                RefreshAll();

            }
        }

        private void RefreshAll()
        {
            ExamTitleIdTextBox.Text = "";
            ExamTitleNameTextBox.Text = "";
            ExamTitleIdTextBox.Text = aExamTitleManagerObj.GetExamTitleAutoId();
            ExamTitleGridView.DataSource = aExamTitleManagerObj.GetAllExamTitleInformation();
            ExamTitleGridView.DataBind();

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
            ExamTitle aExamTitleObj = new ExamTitle();
            aExamTitleObj.Id = ExamTitleIdTextBox.Text;
            aExamTitleObj.Name = ExamTitleNameTextBox.Text;
            aExamTitleManagerObj.SaveTheExamInformation(aExamTitleObj);
            RefreshAll();
        }
        protected void ExamTitleGridView_SelectedIndexChanged(object sender, EventArgs e)
        {
            
            
            ExamTitleIdTextBox.Text = ExamTitleGridView.SelectedRow.Cells[0].Text;
            ExamTitleNameTextBox.Text = ExamTitleGridView.SelectedRow.Cells[1].Text;
            DeleteButton.Visible = true;
            UpdateButton.Visible = true;
            SaveButton.Visible = false;
        }
        protected void UpdateButton_Click(object sender, EventArgs e)
        {
            try
            {
                ExamTitle aExamTitleObj=new ExamTitle();
                aExamTitleObj.Id = ExamTitleIdTextBox.Text;
                aExamTitleObj.Name = ExamTitleNameTextBox.Text;
                aExamTitleManagerObj.UpdateTheExam(aExamTitleObj);
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
                ExamTitle aExamTitleObj = new ExamTitle();
                aExamTitleObj.Id = ExamTitleIdTextBox.Text;
                aExamTitleManagerObj.DeleteTheExam(aExamTitleObj);
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

       
}
