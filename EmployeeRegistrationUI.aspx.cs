using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ACCWebApplication.DAO.Others;
using autouniv;
using KHSC.DAO.Employee;
using KHSC.Manager.EmployeeManager;
using KHSC.Manager.Others;

using KHSC.DAO.Others;

using System.Collections;
using System.Data;

using System.IO;
using KHSC;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.draw;
using System.Data.SqlClient;
using OldColor;
using RIITS_FES_Accounts_Apps.Manager;


public partial class EmployeeRegistrationUI : System.Web.UI.Page
    {
        DepartmentManager aDepartmentManager = new DepartmentManager();
        DesignationManager aDesignationManagerObj = new DesignationManager();
        BankManager aBankManagerObj = new BankManager();
        ExamTitleManager aExamTitleManagerObj = new ExamTitleManager();
        ControlManager aControlManagerObj = new ControlManager();

        ArrayList spesializedArrayList = new ArrayList();
        ArrayList selectedSpesializedArrayList = new ArrayList();
        SpecializedManager aSpecializedManagerObj = new SpecializedManager();
        EmployeeManager aEmployeeManagerObj = new EmployeeManager();
        public static Permis per;
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
                        cmd.CommandText = "Select user_grp,description from utl_userinfo where upper(user_name)=upper('" + Session["user"].ToString().ToUpper() + "') and status='A'";
                        conn.Open();
                        dReader = cmd.ExecuteReader();
                        string wnot = "";
                        if (dReader.HasRows == true)
                        {
                            while (dReader.Read())
                            {
                                Session["userlevel"] = int.Parse(dReader["user_grp"].ToString());
                                wnot = dReader["description"].ToString();
                            }
                            Session["wnote"] = wnot;

                            cmd = new SqlCommand();
                            cmd.Connection = conn;
                            cmd.CommandType = CommandType.Text;
                            cmd.CommandText = "Select book_desc,company_address1,company_address2,separator_type from gl_set_of_books where book_name='" + Session["book"].ToString() + "' ";
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
                string pageName = DataManager.GetCurrentPageName();
                string modid = PermisManager.getModuleId(pageName);
                per = PermisManager.getUsrPermis(Session["user"].ToString().Trim().ToUpper(), modid);
                if (per != null && per.AllowView == "Y")
                {
                    ((Label)Page.Master.FindControl("lblLogin")).Text = Session["wnote"].ToString();
                    ((LinkButton)Page.Master.FindControl("lbLogout")).Visible = true;
                }
                else
                {
                    Response.Redirect("Default.aspx?sid=sam");
                }
            }
            catch
            {
                Response.Redirect("Default.aspx?sid=sam");
            }
            if (IsPostBack != true)
            {
                try
                {
                    RefreshAll();
                    TabContainer1.ActiveTabIndex = 7;
                }
                catch (FormatException fex)
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "Warning", "alert('" + fex.Message + "');", true);
                }
                catch (Exception ex)
                {
                    if (ex.Message.Contains("Database"))
                        ClientScript.RegisterStartupScript(this.GetType(), "Warning", "alert('Database Maintain Error. Contact to the Software Provider..!!');", true);
                    else
                        ClientScript.RegisterStartupScript(this.GetType(), "Warning", "alert('There is some problem to do the task. Try again properly.!!');", true);
                }

            }
        }
        private void RefreshExamTitleInformation()
        {
            EducationGridView.DataSource = null;
            EducationGridView.DataBind();
            DataTable educationDT = new DataTable();
            educationDT.Columns.Add("Id", typeof(string));
            educationDT.Columns.Add("Name", typeof(string));
            educationDT.Columns.Add("Institute", typeof(string));
            educationDT.Columns.Add("GPA", typeof(string));
            educationDT.Columns.Add("Grade", typeof(string));
            educationDT.Columns.Add("Group", typeof(string));
            educationDT.Columns.Add("Board/University", typeof(string));
            educationDT.Columns.Add("Passing Year", typeof(string));
            DataRow dr = educationDT.NewRow();
            educationDT.Rows.Add(dr);

            Session["EducationDataTable"] = educationDT;

             
            EducationGridView.DataSource = educationDT;
            EducationGridView.DataBind();
            
        }

        private void RefreshBankInformation()
        {
            BankNameDrropDownList.Items.Clear();
            BankNameDrropDownList.SelectedIndex = -1;
            
            BankNameDrropDownList.DataSource = aBankManagerObj.GetAllBankInformation();
            BankNameDrropDownList.DataTextField = "bnk_name";
            BankNameDrropDownList.DataValueField = "bnk_id";
            BankNameDrropDownList.DataBind();
            BankNameDrropDownList.Items.Insert(0, "Select Bank");
        }

        private void RefreshDesignationInformation()
        {
            DesignationDrropDownList.Items.Clear();
            DesignationDrropDownList.SelectedIndex = -1;
            DesignationDrropDownList.DataSource = aDesignationManagerObj.GetAllDesignationInformation();
            DesignationDrropDownList.DataTextField = "desig_name";
            DesignationDrropDownList.DataValueField = "desig_id";
            DesignationDrropDownList.DataBind();
            DesignationDrropDownList.Items.Insert(0, "");
        }

        private void RefreshDepartmentInformation()
        {
             
            DepartmentDrropDownList.Items.Clear();
            DepartmentDrropDownList.SelectedIndex = -1;
            DepartmentDrropDownList.DataSource = aDepartmentManager.GetAllDepartmentInformation();
            DepartmentDrropDownList.DataTextField = "dept_name";
            DepartmentDrropDownList.DataValueField = "dept_id";
            DepartmentDrropDownList.DataBind();
            DepartmentDrropDownList.Items.Insert(0, "Select Department");
        }
        private void RefreshAll()
        {
            Session["isindex"] = false;
            //EmployeeIdTextBox.Text = aEmployeeManagerObj.GetEmployeeAutoId();
            EmployeeIdTextBox.Text = "";
            NIDTextBox.Text = "";
            FirstNameTextBox.Text = "";
            MiddleNameTextBox.Text = "";
            LastNameTextBox.Text = "";
            TeacherCheckBox.Checked = false;
            StaffCheckBox.Checked = false;
            DesignationDrropDownList.Items.Clear();
            DesignationDrropDownList.SelectedIndex = -1;
            DepartmentDrropDownList.Items.Clear();
            DepartmentDrropDownList.SelectedIndex = -1;
            RefreshDepartmentInformation();
            RefreshDesignationInformation();
            JoiningDateTextBox.Text = DateTime.Now.ToString("dd/MM/yyyy");
            GenderDrropDownList.SelectedIndex = -1;
            DateOfBirthTextBox.Text = DateTime.Now.ToString("dd/MM/yyyy");
            ReligionDrropDownList.SelectedIndex = -1;
            BloodGroupDrropDownList.SelectedIndex = -1;
            MaritalStatusDrropDownBox.SelectedIndex = -1;
            ServicePreiordTextBox.Text = "";
            BEdChechkBox.Checked = false;
            MEdCheckBox.Checked = false;
            NTRCACheckBox.Checked = false;
            NACheckBox.Checked = false;
            RefreshExamTitleInformation();
            ExtraCurriculamTextBox1.Text = "";
            ExtraCurriculamTextBox2.Text = "";
            ExtraCurriculamTextBox3.Text = "";
            Experience();
            PerHouseNoTextBox.Text = "";
            PerThanaTextBox.Text = ""; 
            PerDistrictTextBox.Text = "";
            PerZipCodeTextBox.Text = "";
            perContactNoTextBox.Text = "";
            MailHouseNoTextBox.Text = "";
            MailThanaTextBox.Text = "";
            MailZipCodeTextBox.Text = "";
            MailDistrictTextBox.Text = "";
            MailMobileNoTextBox.Text = "";
            MailEmailTextBox.Text = "";
            FirstNameTextBox.Text = "";
            MothersNameTextBox.Text = "";
            FathersNameTextBox.Text = "";
            MothersNameTextBox.Text = "";
            SpouseNameTextBox.Text = "";
            OccupationTextBox.Text = "";
            ChildInfo1TextBox.Text = "";
            ChildInfo2TextBox.Text = "";
            ChildInfo3TextBox.Text = "";            
            BranchNameTextBox.Text = "";
            AccountNoTextBox.Text = "";
            AccountTypetextBox.Text = "";
            TINTextBox.Text = "";
            PassportTextBox.Text = "";         
            RefreshBankInformation();          
            RefrenceInformation();
            //Session.Remove("Image");
            //Session.Remove("Sig");
            Image1.ImageUrl="images/noimage.jpeg";
            Image2.ImageUrl = "images/noimage.jpeg";

            ViewState["Image"] = "";
            ViewState["Sig"] = "";
            DataTable dt = aEmployeeManagerObj.GetAllEmployeeInformation(EmployeeIdTextBox.Text, DesignationDrropDownList.SelectedValue, FirstNameTextBox.Text,"");
            TeacherInformationDetails.DataSource = dt;
            Session["History"] = dt;
            TeacherInformationDetails.DataBind();

            BtnUpdate.Visible = false;
            BtnSave.Visible = true;
        }

        private void Experience()
        {
            ExperienceGridview.DataSource = null;
            ExperienceGridview.DataBind();
            List<EmployeeExperience> aEmployeeExperienceList = new List<EmployeeExperience>();
            for (int i = 1; i <= 5; i++)
            {
                EmployeeExperience aEmployeeExperienceObj = new EmployeeExperience();
                aEmployeeExperienceObj.Serial = i;
                aEmployeeExperienceList.Add(aEmployeeExperienceObj);
            }

            ExperienceGridview.DataSource = aEmployeeExperienceList;
            ExperienceGridview.DataBind();
        }

        private void RefrenceInformation()
        {
            RefrenceGridview.DataSource=null;
            RefrenceGridview.DataBind();
            List<RefrenceInfo> aRefrenceInfoList = new List<RefrenceInfo>();
            for (int i = 1; i <= 3; i++)
            {
                RefrenceInfo aRefrenceInfoObj = new RefrenceInfo();
                aRefrenceInfoObj.Serial = i;
                aRefrenceInfoList.Add(aRefrenceInfoObj);
            }
            RefrenceGridview.DataSource = aRefrenceInfoList;
            RefrenceGridview.DataBind();
        }
        protected void SaveButton_Click(object sender, EventArgs e)
        {
            try
            {
                EmployeeInformation aEmployeeInformation = new EmployeeInformation();
                aEmployeeInformation.EmployeeId = EmployeeIdTextBox.Text;
                aEmployeeInformation.NID = NIDTextBox.Text;
                aEmployeeInformation.FirstName = FirstNameTextBox.Text;
                aEmployeeInformation.MiddleName = MiddleNameTextBox.Text;
                aEmployeeInformation.LastName = LastNameTextBox.Text;
                if (StaffCheckBox.Checked == true)
                {
                    aEmployeeInformation.EmployeeCategory = StaffCheckBox.Text;
                }
                else if (TeacherCheckBox.Checked == true)
                {
                    aEmployeeInformation.EmployeeCategory = TeacherCheckBox.Text;
                }

                aEmployeeInformation.Designation = DesignationDrropDownList.SelectedValue;
                aEmployeeInformation.Section = DepartmentDrropDownList.SelectedValue;
                aEmployeeInformation.JoiningDate =  JoiningDateTextBox.Text;
                aEmployeeInformation.Sex = GenderDrropDownList.SelectedItem.Text;
                aEmployeeInformation.DateOfBirth =  DateOfBirthTextBox.Text;
                aEmployeeInformation.Religion = ReligionDrropDownList.SelectedItem.Text;
                aEmployeeInformation.BloodGroup = BloodGroupDrropDownList.SelectedItem.Text;
                aEmployeeInformation.MaritalStatus = MaritalStatusDrropDownBox.Text;
                aEmployeeInformation.ServicePeriod = ServicePreiordTextBox.Text;
                aEmployeeInformation.Jobstatus = JobStatusDrropDownList.SelectedItem.Text;
                aEmployeeInformation.TIN = TINTextBox.Text;
                aEmployeeInformation.Pasport = PassportTextBox.Text;
                if (BEdChechkBox.Checked == true)
                {
                    aEmployeeInformation.BEd = true;

                }
                else
                {
                    aEmployeeInformation.BEd = false;
                }

                if (MEdCheckBox.Checked == true)
                {
                    aEmployeeInformation.MEd = true;
                }
                else
                {
                    aEmployeeInformation.MEd = false;
                }

                if (NTRCACheckBox.Checked == true)
                {
                    aEmployeeInformation.NTRCA = true;
                }
                else
                {
                    aEmployeeInformation.NTRCA = false;
                }

                if (NACheckBox.Checked == true)
                {
                    aEmployeeInformation.NA = true;
                }
                else
                {
                    aEmployeeInformation.NA = false;
                }

                aEmployeeInformation.ExtraCurrlm1 = ExtraCurriculamTextBox1.Text;
                aEmployeeInformation.ExtraCurrlm2 = ExtraCurriculamTextBox2.Text;
                aEmployeeInformation.ExtraCurrlm3 = ExtraCurriculamTextBox3.Text;

                aEmployeeInformation.PerHouseNo = PerHouseNoTextBox.Text;
                aEmployeeInformation.PerThana = PerThanaTextBox.Text;
                aEmployeeInformation.PerDistrict = PerDistrictTextBox.Text;
                aEmployeeInformation.PerZipCode = PerZipCodeTextBox.Text;
                aEmployeeInformation.PerContact = perContactNoTextBox.Text;
                aEmployeeInformation.MailHouse = MailHouseNoTextBox.Text;
                aEmployeeInformation.MailThana = MailThanaTextBox.Text;
                aEmployeeInformation.MailDistrict = MailDistrictTextBox.Text;
                aEmployeeInformation.MailZipCode = MailZipCodeTextBox.Text;
                aEmployeeInformation.MailMobile = MailMobileNoTextBox.Text;
                aEmployeeInformation.MailEmail = MailEmailTextBox.Text;

                aEmployeeInformation.FathersName = FathersNameTextBox.Text;
                aEmployeeInformation.MothersName = MothersNameTextBox.Text;
                aEmployeeInformation.SpouseName = SpouseNameTextBox.Text;
                aEmployeeInformation.Occupation = OccupationTextBox.Text;
                aEmployeeInformation.Childe1 = ChildInfo1TextBox.Text;
                aEmployeeInformation.Childe2 = ChildInfo2TextBox.Text;
                aEmployeeInformation.Childe3 = ChildInfo3TextBox.Text;




                aEmployeeInformation.BankId = BankNameDrropDownList.SelectedValue;
                aEmployeeInformation.BranchName = BranchNameTextBox.Text;
                aEmployeeInformation.AccountNo = AccountNoTextBox.Text;
                aEmployeeInformation.AccountType = AccountTypetextBox.Text;
                //aEmployeeInformation.ExprencceYear = ExprenceTextBox.Text;
                //aEmployeeInformation.ExprenceDetails =ExprenceDetailsTextBox.Text;

                byte[] image = null;
                if (ViewState["Image"] != "")
                {
                    aEmployeeInformation.EmployeeImage = (byte[])ViewState["Image"];
                    image = (byte[])ViewState["Image"];
                }

                byte[] sig = null;
                if (ViewState["Sig"] != "")
                {
                    aEmployeeInformation.EmployeeSignature = (byte[])ViewState["Sig"];
                    sig = (byte[])ViewState["Sig"];
                }



                List<ExamTitle> etList = new List<ExamTitle>();
                for (int i = 0; i < EducationGridView.Rows.Count; i++)
                {
                    ExamTitle aExamTitle = new ExamTitle();
                    DropDownList examDl = EducationGridView.Rows[i].Cells[0].FindControl("ExamTitleComboBox") as DropDownList;
                    TextBox InistituteTextBox = EducationGridView.Rows[i].Cells[0].FindControl("InstituteTextBox") as TextBox;
                    TextBox GPATextBox = EducationGridView.Rows[i].Cells[0].FindControl("GPATextBox") as TextBox;
                    TextBox GradeTextBox = EducationGridView.Rows[i].Cells[0].FindControl("GradeTextBox") as TextBox;
                    TextBox GroupTextBox = EducationGridView.Rows[i].Cells[0].FindControl("GroupTextBox") as TextBox;
                    TextBox BoardTextBox = EducationGridView.Rows[i].Cells[0].FindControl("BoardUniversityTextBox") as TextBox;
                    TextBox PassingYearTextBox = EducationGridView.Rows[i].Cells[0].FindControl("PassingYearTextBox") as TextBox;
                    if (InistituteTextBox.Text.Length > 0)
                    {
                        aExamTitle.ExamId = examDl.SelectedItem.Value.ToString();
                        aExamTitle.Inistitute = InistituteTextBox.Text;
                        aExamTitle.GPA = GPATextBox.Text;
                        aExamTitle.Grade = GradeTextBox.Text;
                        aExamTitle.Group = GroupTextBox.Text;
                        aExamTitle.Board = BoardTextBox.Text;
                        aExamTitle.PassingYear = PassingYearTextBox.Text;
                        etList.Add(aExamTitle);

                    }
                }

                List<EmployeeExperience> EmpExperienceList = new List<EmployeeExperience>();
                for (int i = 0; i < ExperienceGridview.Rows.Count; i++)
                {
                    EmployeeExperience expObj = new EmployeeExperience();

                    TextBox OrgName = ExperienceGridview.Rows[i].Cells[0].FindControl("OrganizzationNameTextBox") as TextBox;
                    TextBox Position = ExperienceGridview.Rows[i].Cells[0].FindControl("PositionTextBox") as TextBox;
                    TextBox Duration = ExperienceGridview.Rows[i].Cells[0].FindControl("DurationTextBox") as TextBox;
                    TextBox ReasonForLeave = ExperienceGridview.Rows[i].Cells[0].FindControl("ReasonForLEaveTextBox") as TextBox;

                    if (OrgName.Text.Length > 0)
                    {
                        expObj.OrganizationName = OrgName.Text;
                        expObj.Position = Position.Text;
                        expObj.Duration = Duration.Text;
                        expObj.ReasonForLeave = ReasonForLeave.Text;

                        EmpExperienceList.Add(expObj);
                    }
                }

                List<RefrenceInfo> EmpRefrenceList = new List<RefrenceInfo>();
                for (int i = 0; i < RefrenceGridview.Rows.Count; i++)
                {
                    RefrenceInfo aRefrenceInfoObj = new RefrenceInfo();

                    TextBox Name = RefrenceGridview.Rows[i].Cells[0].FindControl("RfNameTextBox") as TextBox;
                    TextBox Designation = RefrenceGridview.Rows[i].Cells[0].FindControl("RfDescriptionTextBox") as TextBox;
                    TextBox Organization = RefrenceGridview.Rows[i].Cells[0].FindControl("RfOrganizationTextBox") as TextBox;
                    TextBox Contact = RefrenceGridview.Rows[i].Cells[0].FindControl("RfContactNoTextBox") as TextBox;

                    if (Name.Text.Length > 0)
                    {
                        aRefrenceInfoObj.Name = Name.Text;
                        aRefrenceInfoObj.Designation = Designation.Text;
                        aRefrenceInfoObj.Organization = Organization.Text;
                        aRefrenceInfoObj.Contact = Contact.Text;

                        EmpRefrenceList.Add(aRefrenceInfoObj);
                    }
                }


                aEmployeeManagerObj.SaveTheEmployeeInformation(aEmployeeInformation, etList, EmpExperienceList, EmpRefrenceList, image, sig);
                ClientScript.RegisterStartupScript(this.GetType(), "ale", "alert('Information Save Sucessfully');", true);
                RefreshAll();
            }
            catch (FormatException fex)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "Warning", "alert('" + fex.Message + "');", true);
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("Database"))
                    ClientScript.RegisterStartupScript(this.GetType(), "Warning", "alert('Database Maintain Error. Contact to the Software Provider..!!');", true);
                else
                    ClientScript.RegisterStartupScript(this.GetType(), "Warning", "alert('There is some problem to do the task. Try again properly.!!');", true);
            }
        }

        protected void EducationGridView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    DropDownList exTitleDrropDownList = e.Row.FindControl("ExamTitleComboBox") as DropDownList;
                    TextBox InistituteTextBox = e.Row.FindControl("InstituteTextBox") as TextBox;
                    TextBox GPATextBox = e.Row.FindControl("GPATextBox") as TextBox;
                    TextBox GradeTextBox = e.Row.FindControl("GradeTextBox") as TextBox;
                    TextBox GoupTextBox = e.Row.FindControl("GroupTextBox") as TextBox;
                    TextBox BoardTextBox = e.Row.FindControl("BoardUniversityTextBox") as TextBox;
                    TextBox PassingYearTextBox = e.Row.FindControl("PassingYearTextBox") as TextBox;

                    exTitleDrropDownList.DataSource = aExamTitleManagerObj.GetAllExamTitleInformation();
                    exTitleDrropDownList.DataTextField = "Name";
                    exTitleDrropDownList.DataValueField = "Id";
                    exTitleDrropDownList.DataBind();
                    exTitleDrropDownList.Items.Insert(0, "Select Exam Title");

                    //int up = Convert.ToInt32(Session["IsUpdate"]);

                    //if (up > 0)
                    //{
                    //    string empId = Session["empId"].ToString();
                    //    List<string> empExam = aExamTitleManagerObj.GetLastExamName(empId);
                    //    foreach (string str in empExam)
                    //    {
                    //        exTitleDrropDownList.SelectedIndex = aControlManagerObj.IndexCalCulation(exTitleDrropDownList, str);
                    //    }
                    //}
                    //bool isUpdate = Convert.ToBoolean(Session["isindex"]);
                    //if (isUpdate == true)
                    //{
                    //    if (e.Row.RowIndex > -1)
                    //    {
                    //        if (EducationGridView.Rows[e.Row.RowIndex].Cells[1].Text.Length > 0)
                    //        {
                    //            exTitleDrropDownList.SelectedValue = EducationGridView.Rows[e.Row.RowIndex].Cells[1].Text;
                    //            InistituteTextBox.Text = EducationGridView.Rows[e.Row.RowIndex].Cells[1].Text;
                    //            GPATextBox.Text = EducationGridView.Rows[e.Row.RowIndex].Cells[2].Text;
                    //            GradeTextBox.Text = EducationGridView.Rows[e.Row.RowIndex].Cells[3].Text;
                    //            GoupTextBox.Text = EducationGridView.Rows[e.Row.RowIndex].Cells[4].Text;
                    //            BoardTextBox.Text = EducationGridView.Rows[e.Row.RowIndex].Cells[5].Text;
                    //            PassingYearTextBox.Text = EducationGridView.Rows[e.Row.RowIndex].Cells[6].Text;
                    //        }
                    //    }
                    //}
                }
            }
            catch (FormatException fex)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "Warning", "alert('" + fex.Message + "');", true);
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("Database"))
                    ClientScript.RegisterStartupScript(this.GetType(), "Warning", "alert('Database Maintain Error. Contact to the Software Provider..!!');", true);
                else
                    ClientScript.RegisterStartupScript(this.GetType(), "Warning", "alert('There is some problem to do the task. Try again properly.!!');", true);
            }
        }

        

        protected void ExamTitleComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                int rowIndex = 0;

                if (Session["EducationDataTable"] != null)
                {

                    DataTable currentExamTable = Session["EducationDataTable"] as DataTable;
                    DataRow dr = null;
                    if (currentExamTable.Rows.Count > 0)
                    {
                        for (int i = 1; i <= currentExamTable.Rows.Count; i++)
                        {
                            ExamTitle aExamTitle = new ExamTitle();
                            DropDownList examDl = EducationGridView.Rows[rowIndex].FindControl("ExamTitleComboBox") as DropDownList;
                            TextBox InistituteTextBox = EducationGridView.Rows[rowIndex].FindControl("InstituteTextBox") as TextBox;
                            TextBox GPATextBox = EducationGridView.Rows[rowIndex].FindControl("GPATextBox") as TextBox;
                            TextBox GradeTextBox = EducationGridView.Rows[rowIndex].FindControl("GradeTextBox") as TextBox;
                            TextBox GoupTextBox = EducationGridView.Rows[rowIndex].FindControl("GroupTextBox") as TextBox;
                            TextBox BoardTextBox = EducationGridView.Rows[rowIndex].FindControl("BoardUniversityTextBox") as TextBox;
                            TextBox PassingYearTextBox = EducationGridView.Rows[rowIndex].FindControl("PassingYearTextBox") as TextBox;

                            dr = currentExamTable.NewRow();

                            currentExamTable.Rows[i - 1]["Id"] = examDl.SelectedValue;
                            //currentExamTable.Rows[i - 1]["Name"] = examDl.SelectedItem.Text;
                            currentExamTable.Rows[i - 1]["Institute"] = InistituteTextBox.Text;
                            currentExamTable.Rows[i - 1]["GPA"] = GPATextBox.Text;
                            currentExamTable.Rows[i - 1]["Grade"] = GradeTextBox.Text;
                            currentExamTable.Rows[i - 1]["Group"] = GoupTextBox.Text;
                            currentExamTable.Rows[i - 1]["Board/University"] = BoardTextBox.Text;
                            currentExamTable.Rows[i - 1]["Passing Year"] = PassingYearTextBox.Text;
                            dr = currentExamTable.NewRow();
                            rowIndex++;

                            //examDl.SelectedIndex = aControlManagerObj.IndexCalCulation(examDl, examDl.SelectedItem.Text);
                            //InistituteTextBox.Text = EducationGridView.Rows[i].Cells[1].Text;
                            //GPATextBox.Text = EducationGridView.Rows[i].Cells[2].Text;
                            //GradeTextBox.Text = EducationGridView.Rows[i].Cells[3].Text;
                        }
                        currentExamTable.Rows.Add(dr);
                        Session["EducationDataTable"] = currentExamTable;
                        EducationGridView.DataSource = currentExamTable;
                        EducationGridView.DataBind();

                    }



                }
                SetThePreviousData();
            }
            catch (FormatException fex)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "Warning", "alert('" + fex.Message + "');", true);
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("Database"))
                    ClientScript.RegisterStartupScript(this.GetType(), "Warning", "alert('Database Maintain Error. Contact to the Software Provider..!!');", true);
                else
                    ClientScript.RegisterStartupScript(this.GetType(), "Warning", "alert('There is some problem to do the task. Try again properly.!!');", true);
            }
        }

        private void SetThePreviousData()
        {
            int rowIndex = 0;
            if (Session["EducationDataTable"] != null)
            {
                DataTable table = Session["EducationDataTable"] as DataTable;

                if (table.Rows.Count > 0)
                {
                    for (int j = 0; j < table.Rows.Count; j++)
                    {
                        DropDownList examDl = EducationGridView.Rows[rowIndex].FindControl("ExamTitleComboBox") as DropDownList;
                        TextBox InistituteTextBox = EducationGridView.Rows[rowIndex].FindControl("InstituteTextBox") as TextBox;
                        TextBox GPATextBox = EducationGridView.Rows[rowIndex].FindControl("GPATextBox") as TextBox;
                        TextBox GradeTextBox = EducationGridView.Rows[rowIndex].FindControl("GradeTextBox") as TextBox;
                        TextBox GoupTextBox = EducationGridView.Rows[rowIndex].FindControl("GroupTextBox") as TextBox;
                        TextBox BoardTextBox = EducationGridView.Rows[rowIndex].FindControl("BoardUniversityTextBox") as TextBox;
                        TextBox PassingYearTextBox = EducationGridView.Rows[rowIndex].FindControl("PassingYearTextBox") as TextBox;

                        examDl.SelectedValue = table.Rows[j]["Id"].ToString();
                        InistituteTextBox.Text = table.Rows[j]["Institute"].ToString();
                        GPATextBox.Text = table.Rows[j]["GPA"].ToString();
                        GradeTextBox.Text = table.Rows[j]["Grade"].ToString();
                        GoupTextBox.Text = table.Rows[j]["Group"].ToString();
                        BoardTextBox.Text = table.Rows[j]["Board/University"].ToString();
                        PassingYearTextBox.Text = table.Rows[j]["Passing Year"].ToString();
                        rowIndex++;
                    }
                }
            }
        }

        protected void EducationGridView_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                SetRowData();
                if (Session["EducationDataTable"] != null)
                {
                    DataTable dt = (DataTable)Session["EducationDataTable"];
                    DataRow drCurrentRow = null;
                    int rowIndex = Convert.ToInt32(e.RowIndex);
                    if (dt.Rows.Count > 1)
                    {
                        dt.Rows.Remove(dt.Rows[rowIndex]);
                        drCurrentRow = dt.NewRow();
                        Session["EducationDataTable"] = dt;
                        EducationGridView.DataSource = dt;
                        EducationGridView.DataBind();
                        for (int i = 0; i < EducationGridView.Rows.Count - 1; i++)
                        {
                            EducationGridView.Rows[i].Cells[0].Text = Convert.ToString(i + 1);
                        }
                        SetThePreviousData();
                    }
                }
            }
            catch (FormatException fex)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "Warning", "alert('" + fex.Message + "');", true);
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("Database"))
                    ClientScript.RegisterStartupScript(this.GetType(), "Warning", "alert('Database Maintain Error. Contact to the Software Provider..!!');", true);
                else
                    ClientScript.RegisterStartupScript(this.GetType(), "Warning", "alert('There is some problem to do the task. Try again properly.!!');", true);
            }
        }

        private void SetRowData()
        {
            int rowIndex = 0;

            if (Session["EducationDataTable"] != null)
            {

                DataTable currentExamTable = Session["EducationDataTable"] as DataTable;
                DataRow dr = null;
                if (currentExamTable.Rows.Count > 0)
                {
                    for (int i = 1; i <= currentExamTable.Rows.Count; i++)
                    {
                        ExamTitle aExamTitle = new ExamTitle();
                        DropDownList examDl = EducationGridView.Rows[rowIndex].FindControl("ExamTitleComboBox") as DropDownList;
                        TextBox InistituteTextBox = EducationGridView.Rows[rowIndex].FindControl("InstituteTextBox") as TextBox;
                        TextBox GPATextBox = EducationGridView.Rows[rowIndex].FindControl("GPATextBox") as TextBox;
                        TextBox GradeTextBox = EducationGridView.Rows[rowIndex].FindControl("GradeTextBox") as TextBox;
                        TextBox GoupTextBox = EducationGridView.Rows[rowIndex].FindControl("GroupTextBox") as TextBox;
                        TextBox BoardTextBox = EducationGridView.Rows[rowIndex].FindControl("BoardUniversityTextBox") as TextBox;
                        TextBox PassingYearTextBox = EducationGridView.Rows[rowIndex].FindControl("PassingYearTextBox") as TextBox;

                        dr = currentExamTable.NewRow();

                        currentExamTable.Rows[i - 1]["Id"] = examDl.SelectedValue;
                        //currentExamTable.Rows[i - 1]["Name"] = examDl.SelectedItem.Text;
                        currentExamTable.Rows[i - 1]["Institute"] = InistituteTextBox.Text;
                        currentExamTable.Rows[i - 1]["GPA"] = GPATextBox.Text;
                        currentExamTable.Rows[i - 1]["Grade"] = GradeTextBox.Text;
                        currentExamTable.Rows[i - 1]["Group"] = GoupTextBox.Text;
                        currentExamTable.Rows[i - 1]["Board/University"] = BoardTextBox.Text;
                        currentExamTable.Rows[i - 1]["Passing Year"] = PassingYearTextBox.Text;
                        dr = currentExamTable.NewRow();
                        rowIndex++;

                        //examDl.SelectedIndex = aControlManagerObj.IndexCalCulation(examDl, examDl.SelectedItem.Text);
                        //InistituteTextBox.Text = EducationGridView.Rows[i].Cells[1].Text;
                        //GPATextBox.Text = EducationGridView.Rows[i].Cells[2].Text;
                        //GradeTextBox.Text = EducationGridView.Rows[i].Cells[3].Text;
                    }
                    //currentExamTable.Rows.Add(dr);
                    Session["EducationDataTable"] = currentExamTable;
                    //EducationGridView.DataSource = currentExamTable;
                    //EducationGridView.DataBind();

                }



            }
        }

        protected void ExperienceGridview_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            ////if (e.Row.RowType == DataControlRowType.DataRow)
            ////{
            ////    TextBox OrgName=e.Row.FindControl("OrganizzationNameTextBox") as TextBox;
            ////    TextBox Position=e.Row.FindControl("PositionTextBox") as TextBox;
            ////    TextBox Duration=e.Row.FindControl("DurationTextBox") as TextBox;
            ////    TextBox ReasonForLeave = e.Row.FindControl("ReasonForLEaveTextBox") as TextBox;
            ////    bool isUpdate = Convert.ToBoolean(Session["isindex"]);
            ////    if (isUpdate == true)
            ////    {
            ////        if (e.Row.RowIndex > -1)
            ////        {
            ////            OrgName.Text = ExperienceGridview.Rows[e.Row.RowIndex].Cells[1].Text;
            ////            Position.Text = ExperienceGridview.Rows[e.Row.RowIndex].Cells[2].Text;
            ////            Duration.Text = ExperienceGridview.Rows[e.Row.RowIndex].Cells[3].Text;
            ////            ReasonForLeave.Text = ExperienceGridview.Rows[e.Row.RowIndex].Cells[4].Text;
            ////        }
            ////    }

            ////}
            
        }

        protected void lbImgUpload_Click(object sender, EventArgs e)
        {
            try
            {
                if (FirstNameTextBox.Text != "" && imgUpload.HasFile)
                {
                    int width = 145;
                    int height = 165;
                    byte[] imgTe;                 

                    using (System.Drawing.Bitmap img = StudentManager.ResizeImage(new System.Drawing.Bitmap(imgUpload.PostedFile.InputStream), width, height, StudentManager.ResizeOptions.ExactWidthAndHeight))
                    {
                        imgUpload.PostedFile.InputStream.Close();
                        imgTe = StudentManager.ConvertImageToByteArray(img, System.Drawing.Imaging.ImageFormat.Png);

                        ViewState["Image"] = imgTe;
                       
                        img.Dispose();
                    }
                    string base64String = Convert.ToBase64String(imgTe, 0, imgTe.Length);
                    Image1.ImageUrl = "data:image/png;base64," + base64String;

                    //string base64String1 = Convert.ToBase64String(std_cur_photo, 0, std_cur_photo.Length);
                    //Image2.ImageUrl = "data:image/png;base64," + base64String1;
                }

                else
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "ale", "alert('Please input Employee First Name and then browse a photograph image!!');", true);
                }
            }
            catch (FormatException fex)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "Warning", "alert('" + fex.Message + "');", true);
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("Database"))
                    ClientScript.RegisterStartupScript(this.GetType(), "Warning", "alert('Database Maintain Error. Contact to the Software Provider..!!');", true);
                else
                    ClientScript.RegisterStartupScript(this.GetType(), "Warning", "alert('There is some problem to do the task. Try again properly.!!');", true);
            }
        }

        protected void lbImgUploadStdCur_Click(object sender, EventArgs e)
        {
            try
            {
                if (imgUploadStdCur.HasFile)
                {
                    int width = 145;
                    int height = 165;
                    byte[] Sig;
                    using (System.Drawing.Bitmap img = StudentManager.ResizeImage(new System.Drawing.Bitmap(imgUploadStdCur.PostedFile.InputStream), width, height, StudentManager.ResizeOptions.ExactWidthAndHeight))
                    {
                        imgUploadStdCur.PostedFile.InputStream.Close();
                        Sig = StudentManager.ConvertImageToByteArray(img, System.Drawing.Imaging.ImageFormat.Png);
                        ViewState["Sig"] = Sig;
                        img.Dispose();
                    }
                    string base64String = Convert.ToBase64String(Sig, 0, Sig.Length);
                    Image2.ImageUrl = "data:image/png;base64," + base64String;
                }
                else
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "ale", "alert('Please input Employee ID and then browse a photograph image!!');", true);
                }
            }
            catch (FormatException fex)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "Warning", "alert('" + fex.Message + "');", true);
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("Database"))
                    ClientScript.RegisterStartupScript(this.GetType(), "Warning", "alert('Database Maintain Error. Contact to the Software Provider..!!');", true);
                else
                    ClientScript.RegisterStartupScript(this.GetType(), "Warning", "alert('There is some problem to do the task. Try again properly.!!');", true);
            }
        }

        protected void ImagePreviewButton_Click(object sender, EventArgs e)
        {
            //try
            //{
            //    System.IO.Stream fs = FileUpload1.PostedFile.InputStream;
            //    System.IO.BinaryReader br = new System.IO.BinaryReader(fs);
            //    Byte[] bytes = br.ReadBytes((Int32)fs.Length);
            //    ViewState["Image"] = bytes;
            //    string base64String = Convert.ToBase64String(bytes, 0, bytes.Length);
            //    Image1.ImageUrl = "data:image/png;base64," + base64String;

            //    System.IO.Stream fs1 = FileUpload2.PostedFile.InputStream;
            //    System.IO.BinaryReader br1 = new System.IO.BinaryReader(fs1);
            //    Byte[] bytes1 = br1.ReadBytes((Int32)fs1.Length);
            //    ViewState["Sig"] = bytes1;
            //    string base64String1 = Convert.ToBase64String(bytes1, 0, bytes1.Length);
            //    Image2.ImageUrl = "data:image/png;base64," + base64String1;
            //}
            //catch (FormatException fex)
            //{
            //    ClientScript.RegisterStartupScript(this.GetType(), "Warning", "alert('" + fex.Message + "');", true);
            //}
            //catch (Exception ex)
            //{
            //    if (ex.Message.Contains("Database"))
            //        ClientScript.RegisterStartupScript(this.GetType(), "Warning", "alert('Database Maintain Error. Contact to the Software Provider..!!');", true);
            //    else
            //        ClientScript.RegisterStartupScript(this.GetType(), "Warning", "alert('There is some problem to do the task. Try again properly.!!');", true);
            //}
        }
        protected void SignatureButton_Click(object sender, EventArgs e)
        {
            
        }
        protected void EducationGridView_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        protected void RefreshButton_Click(object sender, EventArgs e)
        {
            try
            {
                var pageName = System.IO.Path.GetFileName(Request.Url.ToString());
                Response.Redirect(pageName);
            }
            catch (FormatException fex)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "Warning", "alert('" + fex.Message + "');", true);
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("Database"))
                    ClientScript.RegisterStartupScript(this.GetType(), "Warning", "alert('Database Maintain Error. Contact to the Software Provider..!!');", true);
                else
                    ClientScript.RegisterStartupScript(this.GetType(), "Warning", "alert('There is some problem to do the task. Try again properly.!!');", true);
            }
        }
        protected void StaffCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (StaffCheckBox.Checked == true)
            {
                StaffCheckBox.Checked = true;
                TeacherCheckBox.Checked = false;
            }
        }
        protected void TeacherCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (TeacherCheckBox.Checked == true)
            {
                StaffCheckBox.Checked = false;
                TeacherCheckBox.Checked = true;
            }
        }
        protected void TeacherInformationDetails_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                RefreshAll();
                RefreshBankInformation();
                RefreshDepartmentInformation();
                RefreshDesignationInformation();
                RefreshExamTitleInformation();

                Session["IsUpdate"] = 1;
                Session["isindex"] = true;
                EmployeeInformation aEmployeeObj = new EmployeeInformation();
                EmployeeIdTextBox.Text = TeacherInformationDetails.SelectedRow.Cells[1].Text;
                Session["empId"] = EmployeeIdTextBox.Text;
                aEmployeeObj = aEmployeeManagerObj.GetAllEmployeeInformationForSpecificEmployee(EmployeeIdTextBox.Text);
                NIDTextBox.Text = aEmployeeObj.NID;
                FirstNameTextBox.Text = aEmployeeObj.FirstName;
                MiddleNameTextBox.Text = aEmployeeObj.MiddleName;
                LastNameTextBox.Text = aEmployeeObj.LastName;
                if (aEmployeeObj.EmployeeCategory.ToUpper() == "STAFF")
                {
                    StaffCheckBox.Checked = true;
                    TeacherCheckBox.Checked = false;
                }
                else
                {
                    StaffCheckBox.Checked = false;
                    TeacherCheckBox.Checked = true;
                }
                if (aEmployeeObj.Designation != null && aEmployeeObj.Designation != "")
                {
                    DesignationDrropDownList.SelectedValue = aEmployeeObj.Designation;

                }
                if (aEmployeeObj.Section != null && aEmployeeObj.Section != "")
                {
                    DepartmentDrropDownList.SelectedValue = aEmployeeObj.Section;

                }

                JoiningDateTextBox.Text = aEmployeeObj.JoiningDate.ToString();
                GenderDrropDownList.SelectedValue = aEmployeeObj.Sex;
                DateOfBirthTextBox.Text = aEmployeeObj.DateOfBirth.ToString();
                ReligionDrropDownList.SelectedValue = aEmployeeObj.Religion;
                BloodGroupDrropDownList.SelectedValue = aEmployeeObj.BloodGroup;
                MaritalStatusDrropDownBox.SelectedValue = aEmployeeObj.MaritalStatus;
                ServicePreiordTextBox.Text = aEmployeeObj.ServicePeriod;
                JobStatusDrropDownList.SelectedValue = aEmployeeObj.Jobstatus;
                TINTextBox.Text = aEmployeeObj.TIN;
                PassportTextBox.Text = aEmployeeObj.Pasport;

                BEdChechkBox.Checked = aEmployeeObj.BEd;
                MEdCheckBox.Checked = aEmployeeObj.MEd;
                NTRCACheckBox.Checked = aEmployeeObj.NTRCA;

                NACheckBox.Checked = aEmployeeObj.NA;
                ExtraCurriculamTextBox1.Text = aEmployeeObj.ExtraCurrlm1;
                ExtraCurriculamTextBox2.Text = aEmployeeObj.ExtraCurrlm2;
                ExtraCurriculamTextBox3.Text = aEmployeeObj.ExtraCurrlm3;
                PerHouseNoTextBox.Text = aEmployeeObj.PerHouseNo;
                PerThanaTextBox.Text = aEmployeeObj.PerThana;
                PerDistrictTextBox.Text = aEmployeeObj.PerDistrict;
                PerZipCodeTextBox.Text = aEmployeeObj.PerZipCode;
                perContactNoTextBox.Text = aEmployeeObj.PerContact;
                MailHouseNoTextBox.Text = aEmployeeObj.MailHouse;
                MailThanaTextBox.Text = aEmployeeObj.MailThana;
                MailDistrictTextBox.Text = aEmployeeObj.MailThana;
                MailZipCodeTextBox.Text = aEmployeeObj.MailZipCode;
                MailMobileNoTextBox.Text = aEmployeeObj.MailMobile;
                MailEmailTextBox.Text = aEmployeeObj.MailEmail;
                FathersNameTextBox.Text = aEmployeeObj.FathersName;
                MothersNameTextBox.Text = aEmployeeObj.MothersName;
                SpouseNameTextBox.Text = aEmployeeObj.SpouseName;
                OccupationTextBox.Text = aEmployeeObj.Occupation;
                ChildInfo1TextBox.Text = aEmployeeObj.Childe1;
                ChildInfo2TextBox.Text = aEmployeeObj.Childe2;
                ChildInfo3TextBox.Text = aEmployeeObj.Childe3;

                if (aEmployeeObj.BankId != null && aEmployeeObj.BankId != "")
                {
                    BankNameDrropDownList.SelectedValue = aEmployeeObj.BankId;
                    AccountNoTextBox.Text = aEmployeeObj.AccountNo;
                    BranchNameTextBox.Text = aEmployeeObj.BranchName;
                    AccountTypetextBox.Text = aEmployeeObj.AccountType;
                }

                ViewState["Image"] = "";
                ViewState["Sig"] = "";

                byte[] image = aEmployeeManagerObj.GetEmployeeImageForSpecificEmployee(EmployeeIdTextBox.Text);
                byte[] sign = aEmployeeManagerObj.GetEmployeeSignImageForSpecificEmployee(EmployeeIdTextBox.Text);
                if (image.Length > 4)
                {
                    MemoryStream ms = new MemoryStream(image);
                    System.Drawing.Image img = System.Drawing.Image.FromStream(ms);
                    ViewState["Image"] = image;
                    string base64String = Convert.ToBase64String(image, 0, image.Length);
                    Image1.ImageUrl = "data:image/jpeg;base64," + base64String;
                }
                else
                {
                    Image1.ImageUrl = "images/noimage.jpeg";
                }


                if (sign.Length > 4)
                {
                    MemoryStream ms = new MemoryStream(image);
                    System.Drawing.Image img = System.Drawing.Image.FromStream(ms);
                    ViewState["Sig"] = sign;
                    string base64StringSign = Convert.ToBase64String(sign, 0, sign.Length);
                    Image2.ImageUrl = "data:image/jpeg;base64," + base64StringSign;
                }
                else
                {
                    Image2.ImageUrl = "images/noimage.jpeg";
                }

                EducationGridView.DataSource = aEmployeeManagerObj.GetAllExamTitleInformationForSpecifEmployee(EmployeeIdTextBox.Text);
                EducationGridView.DataBind();

                ExperienceGridview.DataSource = aEmployeeManagerObj.GetAllExperienceInformationForSpecificEmployee(EmployeeIdTextBox.Text);
                ExperienceGridview.DataBind();

                RefrenceGridview.DataSource = aEmployeeManagerObj.GetAllRefrenceInformationForSpecifcEmployee(EmployeeIdTextBox.Text);
                RefrenceGridview.DataBind();

                BtnUpdate.Visible = true;
                BtnSave.Visible = false;
            }
            catch (FormatException fex)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "Warning", "alert('" + fex.Message + "');", true);
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("Database"))
                    ClientScript.RegisterStartupScript(this.GetType(), "Warning", "alert('Database Maintain Error. Contact to the Software Provider..!!');", true);
                else
                    ClientScript.RegisterStartupScript(this.GetType(), "Warning", "alert('There is some problem to do the task. Try again properly.!!');", true);
            }
        }

        private void Find()
        {
            Session["IsUpdate"] = 1;
            Session["isindex"] = true;

            EmployeeInformation aEmployeeObj = new EmployeeInformation();
            //EmployeeIdTextBox.Text = TeacherInformationDetails.SelectedRow.Cells[0].Text;
            Session["empId"] = EmployeeIdTextBox.Text;
            aEmployeeObj = aEmployeeManagerObj.GetAllEmployeeInformationForSpecificEmployee(EmployeeIdTextBox.Text);
            NIDTextBox.Text = aEmployeeObj.NID;
            FirstNameTextBox.Text = aEmployeeObj.FirstName;
            MiddleNameTextBox.Text = aEmployeeObj.MiddleName;
            LastNameTextBox.Text = aEmployeeObj.LastName;
            if (aEmployeeObj.EmployeeCategory.ToUpper() == "STAFF")
            {
                StaffCheckBox.Checked = true;
                TeacherCheckBox.Checked = false;
            }
            else
            {
                StaffCheckBox.Checked = false;
                TeacherCheckBox.Checked = true;
            }
            if (aEmployeeObj.Designation != null && aEmployeeObj.Designation != "")
            {
                DesignationDrropDownList.SelectedValue = aEmployeeObj.Designation;

            }
            if (aEmployeeObj.Section != null && aEmployeeObj.Section != "")
            {
                DepartmentDrropDownList.SelectedValue = aEmployeeObj.Section;

            }

            JoiningDateTextBox.Text = aEmployeeObj.JoiningDate.ToString();
            GenderDrropDownList.SelectedValue = aEmployeeObj.Sex;
            DateOfBirthTextBox.Text = aEmployeeObj.DateOfBirth.ToString();
            ReligionDrropDownList.SelectedValue = aEmployeeObj.Religion;
            BloodGroupDrropDownList.SelectedValue = aEmployeeObj.BloodGroup;
            MaritalStatusDrropDownBox.SelectedValue = aEmployeeObj.MaritalStatus;
            ServicePreiordTextBox.Text = aEmployeeObj.ServicePeriod;
            JobStatusDrropDownList.SelectedValue = aEmployeeObj.Jobstatus;
            TINTextBox.Text = aEmployeeObj.TIN;
            PassportTextBox.Text = aEmployeeObj.Pasport;

            BEdChechkBox.Checked = aEmployeeObj.BEd;
            MEdCheckBox.Checked = aEmployeeObj.MEd;
            NTRCACheckBox.Checked = aEmployeeObj.NTRCA;

            NACheckBox.Checked = aEmployeeObj.NA;
            ExtraCurriculamTextBox1.Text = aEmployeeObj.ExtraCurrlm1;
            ExtraCurriculamTextBox2.Text = aEmployeeObj.ExtraCurrlm2;
            ExtraCurriculamTextBox3.Text = aEmployeeObj.ExtraCurrlm3;
            PerHouseNoTextBox.Text = aEmployeeObj.PerHouseNo;
            PerThanaTextBox.Text = aEmployeeObj.PerThana;
            PerDistrictTextBox.Text = aEmployeeObj.PerDistrict;
            PerZipCodeTextBox.Text = aEmployeeObj.PerZipCode;
            perContactNoTextBox.Text = aEmployeeObj.PerContact;
            MailHouseNoTextBox.Text = aEmployeeObj.MailHouse;
            MailThanaTextBox.Text = aEmployeeObj.MailThana;
            MailDistrictTextBox.Text = aEmployeeObj.MailThana;
            MailZipCodeTextBox.Text = aEmployeeObj.MailZipCode;
            MailMobileNoTextBox.Text = aEmployeeObj.MailZipCode;
            MailEmailTextBox.Text = aEmployeeObj.MailEmail;
            FathersNameTextBox.Text = aEmployeeObj.FathersName;
            MothersNameTextBox.Text = aEmployeeObj.MothersName;
            SpouseNameTextBox.Text = aEmployeeObj.SpouseName;
            OccupationTextBox.Text = aEmployeeObj.Occupation;
            ChildInfo1TextBox.Text = aEmployeeObj.Childe1;
            ChildInfo2TextBox.Text = aEmployeeObj.Childe2;
            ChildInfo3TextBox.Text = aEmployeeObj.Childe3;

            if (aEmployeeObj.BankId != null && aEmployeeObj.BankId != "")
            {
                BankNameDrropDownList.SelectedValue = aEmployeeObj.BankId;
                AccountNoTextBox.Text = aEmployeeObj.AccountNo;
                BranchNameTextBox.Text = aEmployeeObj.BranchName;
                AccountTypetextBox.Text = aEmployeeObj.AccountType;

            }

            ViewState["Image"] = "";
            ViewState["Sig"] = "";

            byte[] image = aEmployeeManagerObj.GetEmployeeImageForSpecificEmployee(EmployeeIdTextBox.Text);
            byte[] sign = aEmployeeManagerObj.GetEmployeeSignImageForSpecificEmployee(EmployeeIdTextBox.Text);
            if (image.Length > 4)
            {
                MemoryStream ms = new MemoryStream(image);
                System.Drawing.Image img = System.Drawing.Image.FromStream(ms);
                ViewState["Image"] = image;
                string base64String = Convert.ToBase64String(image, 0, image.Length);
                Image1.ImageUrl = "data:image/jpeg;base64," + base64String;
            }
            else
            {
                Image1.ImageUrl = "images/noimage.jpeg";
            }


            if (sign.Length > 4)
            {
                MemoryStream ms = new MemoryStream(image);
                System.Drawing.Image img = System.Drawing.Image.FromStream(ms);
                ViewState["Sig"] = sign;
                string base64StringSign = Convert.ToBase64String(sign, 0, sign.Length);
                Image2.ImageUrl = "data:image/jpeg;base64," + base64StringSign;
            }
            else
            {
                Image2.ImageUrl = "images/noimage.jpeg";
            }

            EducationGridView.DataSource = aEmployeeManagerObj.GetAllExamTitleInformationForSpecifEmployee(EmployeeIdTextBox.Text);
            EducationGridView.DataBind();

            ExperienceGridview.DataSource = aEmployeeManagerObj.GetAllExperienceInformationForSpecificEmployee(EmployeeIdTextBox.Text);
            ExperienceGridview.DataBind();

            RefrenceGridview.DataSource = aEmployeeManagerObj.GetAllRefrenceInformationForSpecifcEmployee(EmployeeIdTextBox.Text);
            RefrenceGridview.DataBind();

            BtnUpdate.Visible = true;
            BtnSave.Visible = false;
        }
        protected void RefrenceGridview_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //TextBox name = e.Row.FindControl("RfNameTextBox") as TextBox;
                //TextBox designation = e.Row.FindControl("RfDescriptionTextBox") as TextBox;
                //TextBox organiztion = e.Row.FindControl("RfOrganizationTextBox") as TextBox;
                //TextBox contactNo = e.Row.FindControl("RfContactNoTextBox") as TextBox;
                //bool isUpdate = Convert.ToBoolean(Session["isindex"]);
                //if (isUpdate == true)
                //{
                //    if (e.Row.RowIndex > -1)
                //    {
                //        name.Text = RefrenceGridview.Rows[e.Row.RowIndex].Cells[0].Text;
                //        designation.Text = RefrenceGridview.Rows[e.Row.RowIndex].Cells[1].Text;
                //        organiztion.Text = RefrenceGridview.Rows[e.Row.RowIndex].Cells[2].Text;
                //        contactNo.Text = RefrenceGridview.Rows[e.Row.RowIndex].Cells[3].Text;
                //    }
                //}
            }
        }
        protected void UpdateButton_Click(object sender, EventArgs e)
        {
            try
            {
                EmployeeInformation aEmployeeInformation = new EmployeeInformation();
                aEmployeeInformation.EmployeeId = EmployeeIdTextBox.Text;
                aEmployeeInformation.NID = NIDTextBox.Text;
                aEmployeeInformation.FirstName = FirstNameTextBox.Text;
                aEmployeeInformation.MiddleName = MiddleNameTextBox.Text;
                aEmployeeInformation.LastName = LastNameTextBox.Text;
                if (StaffCheckBox.Checked == true)
                {
                    aEmployeeInformation.EmployeeCategory = StaffCheckBox.Text;
                }
                else if (TeacherCheckBox.Checked == true)
                {
                    aEmployeeInformation.EmployeeCategory = TeacherCheckBox.Text;
                }

                aEmployeeInformation.Designation = DesignationDrropDownList.SelectedValue;
                aEmployeeInformation.Section = DepartmentDrropDownList.SelectedValue;
                aEmployeeInformation.JoiningDate =  JoiningDateTextBox.Text;
                aEmployeeInformation.Sex = GenderDrropDownList.SelectedItem.Text;
                aEmployeeInformation.DateOfBirth =  DateOfBirthTextBox.Text;
                aEmployeeInformation.Religion = ReligionDrropDownList.SelectedItem.Text;
                aEmployeeInformation.BloodGroup = BloodGroupDrropDownList.SelectedItem.Text;
                aEmployeeInformation.MaritalStatus = MaritalStatusDrropDownBox.Text;
                aEmployeeInformation.ServicePeriod = ServicePreiordTextBox.Text;
                aEmployeeInformation.Jobstatus = JobStatusDrropDownList.SelectedItem.Text;
                aEmployeeInformation.TIN = TINTextBox.Text;
                aEmployeeInformation.Pasport = PassportTextBox.Text;
                if (BEdChechkBox.Checked == true)
                { aEmployeeInformation.BEd = true; }
                else
                { aEmployeeInformation.BEd = false;  }

                if (MEdCheckBox.Checked == true)
                { aEmployeeInformation.MEd = true; }
                else
                { aEmployeeInformation.MEd = false; }

                if (NTRCACheckBox.Checked == true)
                { aEmployeeInformation.NTRCA = true;}
                else
                {
                    aEmployeeInformation.NTRCA = false;
                }

                if (NACheckBox.Checked == true)
                {
                    aEmployeeInformation.NA = true;
                }
                else
                {
                    aEmployeeInformation.NA = false;
                }

                aEmployeeInformation.ExtraCurrlm1 = ExtraCurriculamTextBox1.Text;
                aEmployeeInformation.ExtraCurrlm2 = ExtraCurriculamTextBox2.Text;
                aEmployeeInformation.ExtraCurrlm3 = ExtraCurriculamTextBox3.Text;

                aEmployeeInformation.PerHouseNo = PerHouseNoTextBox.Text;
                aEmployeeInformation.PerThana = PerThanaTextBox.Text;
                aEmployeeInformation.PerDistrict = PerDistrictTextBox.Text;
                aEmployeeInformation.PerZipCode = PerZipCodeTextBox.Text;
                aEmployeeInformation.PerContact = perContactNoTextBox.Text;
                aEmployeeInformation.MailHouse = MailHouseNoTextBox.Text;
                aEmployeeInformation.MailThana = MailThanaTextBox.Text;
                aEmployeeInformation.MailDistrict = MailDistrictTextBox.Text;
                aEmployeeInformation.MailZipCode = MailZipCodeTextBox.Text;
                aEmployeeInformation.MailMobile = MailMobileNoTextBox.Text;
                aEmployeeInformation.MailEmail = MailEmailTextBox.Text;

                aEmployeeInformation.FathersName = FathersNameTextBox.Text;
                aEmployeeInformation.MothersName = MothersNameTextBox.Text;
                aEmployeeInformation.SpouseName = SpouseNameTextBox.Text;
                aEmployeeInformation.Occupation = OccupationTextBox.Text;
                aEmployeeInformation.Childe1 = ChildInfo1TextBox.Text;
                aEmployeeInformation.Childe2 = ChildInfo2TextBox.Text;
                aEmployeeInformation.Childe3 = ChildInfo3TextBox.Text;




                aEmployeeInformation.BankId = BankNameDrropDownList.SelectedValue;
                aEmployeeInformation.BranchName = BranchNameTextBox.Text;
                aEmployeeInformation.AccountNo = AccountNoTextBox.Text;
                aEmployeeInformation.AccountType = AccountTypetextBox.Text;
                //aEmployeeInformation.ExprencceYear = ExprenceTextBox.Text;
                //aEmployeeInformation.ExprenceDetails =ExprenceDetailsTextBox.Text;




                byte[] image = null;
                if (ViewState["Image"] != " ")
                {
                    aEmployeeInformation.EmployeeImage = (byte[])ViewState["Image"];
                    image = (byte[])ViewState["Image"];
                }
                byte[] sig = null;
                if (ViewState["Sig"] != " ")
                {
                    aEmployeeInformation.EmployeeSignature = (byte[])ViewState["Sig"];
                    sig = (byte[])ViewState["Sig"];
                }



                List<ExamTitle> etList = new List<ExamTitle>();
                for (int i = 0; i < EducationGridView.Rows.Count; i++)
                {
                    ExamTitle aExamTitle = new ExamTitle();
                    DropDownList examDl = EducationGridView.Rows[i].Cells[0].FindControl("ExamTitleComboBox") as DropDownList;
                    TextBox InistituteTextBox = EducationGridView.Rows[i].Cells[0].FindControl("InstituteTextBox") as TextBox;
                    TextBox GPATextBox = EducationGridView.Rows[i].Cells[0].FindControl("GPATextBox") as TextBox;
                    TextBox GradeTextBox = EducationGridView.Rows[i].Cells[0].FindControl("GradeTextBox") as TextBox;
                    TextBox GroupTextBox = EducationGridView.Rows[i].Cells[0].FindControl("GroupTextBox") as TextBox;
                    TextBox BoardTextBox = EducationGridView.Rows[i].Cells[0].FindControl("BoardUniversityTextBox") as TextBox;
                    TextBox PassingYearTextBox = EducationGridView.Rows[i].Cells[0].FindControl("PassingYearTextBox") as TextBox;
                    if (InistituteTextBox.Text.Length > 0)
                    {
                        aExamTitle.ExamId = examDl.SelectedItem.Value.ToString();
                        aExamTitle.Inistitute = InistituteTextBox.Text;
                        aExamTitle.GPA = GPATextBox.Text;
                        aExamTitle.Grade = GradeTextBox.Text;
                        aExamTitle.Group = GroupTextBox.Text;
                        aExamTitle.Board = BoardTextBox.Text;
                        aExamTitle.PassingYear = PassingYearTextBox.Text;
                        etList.Add(aExamTitle);
                    }
                }

                List<EmployeeExperience> EmpExperienceList = new List<EmployeeExperience>();
                for (int i = 0; i < ExperienceGridview.Rows.Count; i++)
                {
                    EmployeeExperience expObj = new EmployeeExperience();

                    TextBox OrgName = ExperienceGridview.Rows[i].Cells[0].FindControl("OrganizzationNameTextBox") as TextBox;
                    TextBox Position = ExperienceGridview.Rows[i].Cells[0].FindControl("PositionTextBox") as TextBox;
                    TextBox Duration = ExperienceGridview.Rows[i].Cells[0].FindControl("DurationTextBox") as TextBox;
                    TextBox ReasonForLeave = ExperienceGridview.Rows[i].Cells[0].FindControl("ReasonForLEaveTextBox") as TextBox;

                    if (OrgName.Text.Length > 0)
                    {
                        expObj.OrganizationName = OrgName.Text;
                        expObj.Position = Position.Text;
                        expObj.Duration = Duration.Text;
                        expObj.ReasonForLeave = ReasonForLeave.Text;

                        EmpExperienceList.Add(expObj);
                    }
                }

                List<RefrenceInfo> EmpRefrenceList = new List<RefrenceInfo>();
                for (int i = 0; i < RefrenceGridview.Rows.Count; i++)
                {
                    RefrenceInfo aRefrenceInfoObj = new RefrenceInfo();

                    TextBox Name = RefrenceGridview.Rows[i].Cells[0].FindControl("RfNameTextBox") as TextBox;
                    TextBox Designation = RefrenceGridview.Rows[i].Cells[0].FindControl("RfDescriptionTextBox") as TextBox;
                    TextBox Organization = RefrenceGridview.Rows[i].Cells[0].FindControl("RfOrganizationTextBox") as TextBox;
                    TextBox Contact = RefrenceGridview.Rows[i].Cells[0].FindControl("RfContactNoTextBox") as TextBox;

                    if (Name.Text.Length > 0)
                    {
                        aRefrenceInfoObj.Name = Name.Text;
                        aRefrenceInfoObj.Designation = Designation.Text;
                        aRefrenceInfoObj.Organization = Organization.Text;
                        aRefrenceInfoObj.Contact = Contact.Text;

                        EmpRefrenceList.Add(aRefrenceInfoObj);
                    }
                }
                aEmployeeManagerObj.UpdateTheEmployeeInformation(aEmployeeInformation, etList, EmpExperienceList, EmpRefrenceList, image, sig);
                ClientScript.RegisterStartupScript(this.GetType(), "ale", "alert('Information Updated Sucessfully');", true);
                
            }
            catch (FormatException fex)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "Warning", "alert('" + fex.Message + "');", true);
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("Database"))
                    ClientScript.RegisterStartupScript(this.GetType(), "Warning", "alert('Database Maintain Error. Contact to the Software Provider..!!');", true);
                else
                    ClientScript.RegisterStartupScript(this.GetType(), "Warning", "alert('There is some problem to do the task. Try again properly.!!');", true);
            }
            RefreshAll();
            RefreshBankInformation();
            RefreshDepartmentInformation();
            RefreshDesignationInformation();
            RefreshExamTitleInformation();
            EmployeeIdTextBox.Focus();
            
        }
        protected void DeleteButton_Click(object sender, EventArgs e)
        {
            try
            {
                aEmployeeManagerObj.DeleteTheEmployeeInformation(EmployeeIdTextBox.Text);
                ClientScript.RegisterStartupScript(this.GetType(), "ale", "alert('Information Deleted Sucessfully');", true);
                RefreshAll();
            }
            catch (FormatException fex)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "Warning", "alert('" + fex.Message + "');", true);
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("Database"))
                    ClientScript.RegisterStartupScript(this.GetType(), "Warning", "alert('Database Maintain Error. Contact to the Software Provider..!!');", true);
                else
                    ClientScript.RegisterStartupScript(this.GetType(), "Warning", "alert('There is some problem to do the task. Try again properly.!!');", true);
            }
        }

        protected void TeacherInformationDetails_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                TeacherInformationDetails.PageIndex = e.NewPageIndex;
                TeacherInformationDetails.DataSource = Session["History"];
                TeacherInformationDetails.DataBind();
            }
            catch (FormatException fex)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "Warning", "alert('" + fex.Message + "');", true);
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("Database"))
                    ClientScript.RegisterStartupScript(this.GetType(), "Warning", "alert('Database Maintain Error. Contact to the Software Provider..!!');", true);
                else
                    ClientScript.RegisterStartupScript(this.GetType(), "Warning", "alert('There is some problem to do the task. Try again properly.!!');", true);
            }
        }
        protected void BtnFind_Click(object sender, EventArgs e)
        {
            try
            {
                string TeacherOfStaff = "";
                if (TeacherCheckBox.Checked)
                { TeacherOfStaff = "Teacher"; }
                else if (StaffCheckBox.Checked)
                { TeacherOfStaff = "Staff"; }
                else
                { TeacherOfStaff = ""; }
                DataTable dt = aEmployeeManagerObj.GetAllEmployeeInformation(EmployeeIdTextBox.Text, DesignationDrropDownList.SelectedValue, FirstNameTextBox.Text, TeacherOfStaff);
                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows.Count == 1)
                    {
                        EmployeeIdTextBox.Text = dt.Rows[0][0].ToString();
                        Find();
                    }
                    else
                    {
                        TeacherInformationDetails.DataSource = dt;
                        Session["History"] = dt;
                        TeacherInformationDetails.DataBind();
                    }
                }
                else
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "Warning", "alert('NO Search Student');", true);
                }
            }
            catch (FormatException fex)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "Warning", "alert('" + fex.Message + "');", true);
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("Database"))
                    ClientScript.RegisterStartupScript(this.GetType(), "Warning", "alert('Database Maintain Error. Contact to the Software Provider..!!');", true);
                else
                    ClientScript.RegisterStartupScript(this.GetType(), "Warning", "alert('There is some problem to do the task. Try again properly.!!');", true);
            }
        }
        protected void TeacherInformationDetails_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow || e.Row.RowType == DataControlRowType.Header || e.Row.RowType == DataControlRowType.Footer)
                {
                    e.Row.Cells[3].Attributes.Add("style", "display:none");
                   // e.Row.Cells[4].Attributes.Add("style", "display:none");
                }               
            }
            catch (FormatException fex)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "Warning", "alert('" + fex.Message + "');", true);
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("Database"))
                    ClientScript.RegisterStartupScript(this.GetType(), "Warning", "alert('Database Maintain Error. Contact to the Software Provider..!!');", true);
                else
                    ClientScript.RegisterStartupScript(this.GetType(), "Warning", "alert('There is some problem to do the task. Try again properly.!!');", true);
            }
        }
        protected void btnPrint_Click(object sender, EventArgs e)
        {
            Response.Clear();
            Response.ContentType = "application/pdf";
            Response.AddHeader("content-disposition", "attachment; filename='StudentInformation'.pdf");
            Document document = new Document();
            document = new Document(PageSize.A4.Rotate());
            MemoryStream ms = new MemoryStream();
            PdfWriter writer = PdfWriter.GetInstance(document, Response.OutputStream);
            pdfPage page = new pdfPage();
            writer.PageEvent = page;
            document.Open();

            byte[] logo = GlBookManager.GetGlLogo(Session["book"].ToString());
            iTextSharp.text.Image gif = iTextSharp.text.Image.GetInstance(logo);
            gif.Alignment = iTextSharp.text.Image.MIDDLE_ALIGN;
            gif.ScalePercent(35f);
            float[] titwidth = new float[2] { 5, 200 };
            PdfPCell cell;
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
            cell.BorderWidth = 0f;
            //cell.FixedHeight = 20f;
            dth.AddCell(cell);
            cell = new PdfPCell(new Phrase(Session["add1"].ToString(), FontFactory.GetFont(FontFactory.TIMES_ROMAN, 10, iTextSharp.text.Font.NORMAL)));
            cell.HorizontalAlignment = 1;
            cell.VerticalAlignment = 1;
            cell.BorderWidth = 0f;
            //cell.FixedHeight = 20f;
            dth.AddCell(cell);
            cell = new PdfPCell(new Phrase(Session["add2"].ToString(), FontFactory.GetFont(FontFactory.TIMES_ROMAN, 10, iTextSharp.text.Font.NORMAL)));
            cell.HorizontalAlignment = 1;
            cell.VerticalAlignment = 1;
            cell.BorderWidth = 0f;
            //cell.FixedHeight = 20f;
            dth.AddCell(cell);
          
            cell = new PdfPCell(new Phrase("Total Employee Information", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 11, iTextSharp.text.Font.BOLD)));
            cell.HorizontalAlignment = 1;
            cell.VerticalAlignment = 1;
            cell.BorderWidth = 0f;
            dth.AddCell(cell);
            document.Add(dth);

            LineSeparator line = new LineSeparator(0f, 100, null, Element.ALIGN_CENTER, -2);
            document.Add(line);
            PdfPTable dtempty = new PdfPTable(1);
            cell.BorderWidth = 0f;
            cell.FixedHeight = 5f;
            dtempty.AddCell(cell);
            document.Add(dtempty);

            float[] width = new float[9] { 8, 17, 30, 20, 25, 10, 15, 15, 15 };
            PdfPTable pdtc = new PdfPTable(width);
            pdtc.WidthPercentage = 100;
            //pdtc.HeaderRows =3;       
            pdtc.HeaderRows = 1;
            //cell = new PdfPCell(FormatHeaderPhrase(""));
            //cell.HorizontalAlignment = 1;
            //cell.VerticalAlignment = 1;
            //cell.FixedHeight = 20f;
            //cell.Colspan = 9;
            //pdtc.AddCell(cell);

            int ii=1;
            cell = new PdfPCell(FormatHeaderPhrase("Serial"));
            cell.HorizontalAlignment = 1;
            cell.VerticalAlignment = 1;
            pdtc.AddCell(cell);

            cell = new PdfPCell(FormatHeaderPhrase("Employee ID"));
            cell.HorizontalAlignment = 1;
            cell.VerticalAlignment = 1;
            pdtc.AddCell(cell);
            cell = new PdfPCell(FormatHeaderPhrase("Employee Name"));
            cell.HorizontalAlignment = 1;
            cell.VerticalAlignment = 1;
            pdtc.AddCell(cell);
            cell = new PdfPCell(FormatHeaderPhrase("Department"));
            cell.HorizontalAlignment = 1;
            cell.VerticalAlignment = 1;
            pdtc.AddCell(cell);
            cell = new PdfPCell(FormatHeaderPhrase("Designation"));
            cell.HorizontalAlignment = 1;
            cell.VerticalAlignment = 1;
            pdtc.AddCell(cell);
            cell = new PdfPCell(FormatHeaderPhrase("Gender"));
            cell.HorizontalAlignment = 1;
            cell.VerticalAlignment = 1;
            pdtc.AddCell(cell);
            cell = new PdfPCell(FormatHeaderPhrase("Blood Group"));
            cell.HorizontalAlignment = 1;
            cell.VerticalAlignment = 1;
            pdtc.AddCell(cell);
            cell = new PdfPCell(FormatHeaderPhrase("Date Of Birth"));
            cell.HorizontalAlignment = 1;
            cell.VerticalAlignment = 1;
            pdtc.AddCell(cell);
            cell = new PdfPCell(FormatHeaderPhrase("Phone Number"));
            cell.HorizontalAlignment = 1;
            cell.VerticalAlignment = 1;
            pdtc.AddCell(cell);

            DataTable dt =(DataTable)Session["History"];
            foreach (DataRow row in dt.Rows)
            {

                cell = new PdfPCell(FormatPhrase(ii.ToString()));
                cell.HorizontalAlignment = 1;
                cell.VerticalAlignment = 1;
                //cell.FixedHeight = 20f;
                //cell.BorderColor = BaseColor.LIGHT_GRAY;
                pdtc.AddCell(cell);
                ii++;

                cell = new PdfPCell(FormatPhrase(row["emp_employee_id"].ToString()));
                cell.HorizontalAlignment = 1;
                cell.VerticalAlignment = 1;
                //cell.FixedHeight = 20f;
                //cell.BorderColor = BaseColor.LIGHT_GRAY;
                pdtc.AddCell(cell);
                cell = new PdfPCell(FormatPhrase(row["emp_name"].ToString()));
                cell.HorizontalAlignment = 0;
                cell.VerticalAlignment = 1;
                //cell.FixedHeight = 20f;
                //cell.BorderColor = BaseColor.LIGHT_GRAY;
                pdtc.AddCell(cell);

                cell = new PdfPCell(FormatPhrase(row["dept_name"].ToString()));
                cell.HorizontalAlignment = 0;
                cell.VerticalAlignment = 1;
                //cell.FixedHeight = 20f;
                //cell.BorderColor = BaseColor.LIGHT_GRAY;
                pdtc.AddCell(cell);

                cell = new PdfPCell(FormatPhrase(row["desig_name"].ToString()));
                cell.HorizontalAlignment = 0;
                cell.VerticalAlignment = 1;
                //cell.FixedHeight = 20f;
                //cell.BorderColor = BaseColor.LIGHT_GRAY;
                pdtc.AddCell(cell);

                cell = new PdfPCell(FormatPhrase(row["emp_gender"].ToString()));
                cell.HorizontalAlignment = 1;
                cell.VerticalAlignment = 1;
                //cell.FixedHeight = 20f;
                //cell.BorderColor = BaseColor.LIGHT_GRAY;
                pdtc.AddCell(cell);

                cell = new PdfPCell(FormatPhrase(row["emp_blood"].ToString()));
                cell.HorizontalAlignment = 1;
                cell.VerticalAlignment = 1;
                //cell.FixedHeight = 20f;
                //cell.BorderColor = BaseColor.LIGHT_GRAY;
                pdtc.AddCell(cell);
                cell = new PdfPCell(FormatPhrase(row["emp_dateof_birth"].ToString()));
                cell.HorizontalAlignment = 1;
                cell.VerticalAlignment = 1;
                //cell.FixedHeight = 20f;
                //cell.BorderColor = BaseColor.LIGHT_GRAY;
                pdtc.AddCell(cell);
                cell = new PdfPCell(FormatPhrase(row["emp_mail_mobile"].ToString()));
                cell.HorizontalAlignment = 1;
                cell.VerticalAlignment = 1;
                //cell.FixedHeight = 20f;
                //cell.BorderColor = BaseColor.LIGHT_GRAY;
                pdtc.AddCell(cell);               

            }
            document.Add(pdtc);
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
