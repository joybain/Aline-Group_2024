using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
 
using System.Data.SqlClient;
using System.Data;
using autouniv;
using KHSC;
using KHSC.DAO.Employee;
using KHSC.DAO.Others;
using ACCWebApplication.DAO.Others;

namespace KHSC.Gateway.EmloyeeGateway
{
    public class EmployeeGateway
    {
        
        SqlConnection connection = new SqlConnection(DataManager.OraConnString());
        SqlTransaction transection;

        public void SaveTheEmployeeInformation(EmployeeInformation aEmployeeInformationObj, List<ExamTitle> etList, List<EmployeeExperience> EmpExperienceList, List<RefrenceInfo> EmpRefrenceList, byte[] image, byte[] sig)
        {
            try
            {
                connection.Open();
                transection = connection.BeginTransaction();
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transection;

                command.CommandText = @"SELECT  COUNT(*) FROM [tbl_employee_information] WHERE [emp_employee_id]='"+aEmployeeInformationObj.EmployeeId+"'";
                int count = Convert.ToInt32(command.ExecuteScalar());
                if (count > 0)
                {
                }
                else
                {
                    if (image != null)
                    {
                        command.CommandText = @"INSERT INTO [tbl_employee_information]
           ([emp_employee_id] ,[emp_nid] ,[emp_first_name],[emp_middle_name],[emp_last_name],[emp_catagory],[emp_designation_id],[emp_department_id],[emp_joining_date],[emp_gender],[emp_dateof_birth],[emp_religion],[emp_blood],[emp_marital_status],[emp_service_Preiord],[emp_job_status],[emp_tin],[emp_pasport],[emp_bed],[emp_med] ,[emp_ntrc],[emp_na],[emp_extra_currlm1] ,[emp_extra_currlm2],[emp_extra_currlm3],[emp_per_house],[emp_per_thana],[emp_per_District],[emp_per_zip_code],[emp_per_contact_no] ,[emp_mail_house],[emp_mail_thana],[emp_mail_district],[emp_mail_zip_code],[emp_mail_mobile],[emp_mail_email],[emp_fathers_name],[emp_mothers_name],[emp_spouse_name],[emp_occupation],[emp_childe_info1],[emp_childe_info2],[emp_childe_info3] ,[emp_bank_id],[emp_branch_name],[emp_account_no],[emp_account_type],[emp_image],[emp_signature])
     VALUES
           ('" + aEmployeeInformationObj.EmployeeId + "','" + aEmployeeInformationObj.NID + "','" + aEmployeeInformationObj.FirstName + "','" + aEmployeeInformationObj.MiddleName + "','" + aEmployeeInformationObj.LastName + "','" + aEmployeeInformationObj.EmployeeCategory + "','" + aEmployeeInformationObj.Designation + "','" + aEmployeeInformationObj.Section + "','" + aEmployeeInformationObj.JoiningDate + "','" + aEmployeeInformationObj.Sex + "','" + aEmployeeInformationObj.DateOfBirth + "','" + aEmployeeInformationObj.Religion + "','" + aEmployeeInformationObj.BloodGroup + "','" + aEmployeeInformationObj.MaritalStatus + "','" + aEmployeeInformationObj.ServicePeriod + "','" + aEmployeeInformationObj.Jobstatus + "','" + aEmployeeInformationObj.TIN + "','" + aEmployeeInformationObj.Pasport + "','" + aEmployeeInformationObj.BEd + "','" + aEmployeeInformationObj.MEd + "','" + aEmployeeInformationObj.NTRCA + "','" + aEmployeeInformationObj.NA + "','" + aEmployeeInformationObj.ExtraCurrlm1 + "','" + aEmployeeInformationObj.ExtraCurrlm2 + "','" + aEmployeeInformationObj.ExtraCurrlm3 + "','" + aEmployeeInformationObj.PerHouseNo + "','" + aEmployeeInformationObj.PerThana + "','" + aEmployeeInformationObj.PerDistrict + "','" + aEmployeeInformationObj.PerZipCode + "','" + aEmployeeInformationObj.PerContact + "','" + aEmployeeInformationObj.MailHouse + "','" + aEmployeeInformationObj.MailThana + "','" + aEmployeeInformationObj.MailDistrict + "','" + aEmployeeInformationObj.MailZipCode + "','" + aEmployeeInformationObj.MailMobile + "','" + aEmployeeInformationObj.MailEmail + "','" + aEmployeeInformationObj.FathersName + "','" + aEmployeeInformationObj.MothersName + "','" + aEmployeeInformationObj.SpouseName + "','" + aEmployeeInformationObj.Occupation + "','" + aEmployeeInformationObj.Childe1 + "','" + aEmployeeInformationObj.Childe2 + "','" + aEmployeeInformationObj.Childe3 + "','" + aEmployeeInformationObj.BankId + "','" + aEmployeeInformationObj.BranchName + "','" + aEmployeeInformationObj.AccountNo + "','" + aEmployeeInformationObj.AccountType + "',@image,@sig) SELECT @@IDENTITY";
                        command.Parameters.AddWithValue("@image", image);
                        command.Parameters.AddWithValue("@sig", sig);
                        command.ExecuteNonQuery();
                    }
                    else
                    {
                        command.CommandText = @"INSERT INTO [tbl_employee_information]
           ([emp_employee_id] ,[emp_nid] ,[emp_first_name],[emp_middle_name],[emp_last_name],[emp_catagory],[emp_designation_id],[emp_department_id],[emp_joining_date],[emp_gender],[emp_dateof_birth],[emp_religion],[emp_blood],[emp_marital_status],[emp_service_Preiord],[emp_job_status],[emp_tin],[emp_pasport],[emp_bed],[emp_med] ,[emp_ntrc],[emp_na],[emp_extra_currlm1] ,[emp_extra_currlm2],[emp_extra_currlm3],[emp_per_house],[emp_per_thana],[emp_per_District],[emp_per_zip_code],[emp_per_contact_no] ,[emp_mail_house],[emp_mail_thana],[emp_mail_district],[emp_mail_zip_code],[emp_mail_mobile],[emp_mail_email],[emp_fathers_name],[emp_mothers_name],[emp_spouse_name],[emp_occupation],[emp_childe_info1],[emp_childe_info2],[emp_childe_info3] ,[emp_bank_id],[emp_branch_name],[emp_account_no],[emp_account_type])
     VALUES
           ('" + aEmployeeInformationObj.EmployeeId + "','" + aEmployeeInformationObj.NID + "','" + aEmployeeInformationObj.FirstName + "','" + aEmployeeInformationObj.MiddleName + "','" + aEmployeeInformationObj.LastName + "','" + aEmployeeInformationObj.EmployeeCategory + "','" + aEmployeeInformationObj.Designation + "','" + aEmployeeInformationObj.Section + "','" + aEmployeeInformationObj.JoiningDate + "','" + aEmployeeInformationObj.Sex + "','" + aEmployeeInformationObj.DateOfBirth + "','" + aEmployeeInformationObj.Religion + "','" + aEmployeeInformationObj.BloodGroup + "','" + aEmployeeInformationObj.MaritalStatus + "','" + aEmployeeInformationObj.ServicePeriod + "','" + aEmployeeInformationObj.Jobstatus + "','" + aEmployeeInformationObj.TIN + "','" + aEmployeeInformationObj.Pasport + "','" + aEmployeeInformationObj.BEd + "','" + aEmployeeInformationObj.MEd + "','" + aEmployeeInformationObj.NTRCA + "','" + aEmployeeInformationObj.NA + "','" + aEmployeeInformationObj.ExtraCurrlm1 + "','" + aEmployeeInformationObj.ExtraCurrlm2 + "','" + aEmployeeInformationObj.ExtraCurrlm3 + "','" + aEmployeeInformationObj.PerHouseNo + "','" + aEmployeeInformationObj.PerThana + "','" + aEmployeeInformationObj.PerDistrict + "','" + aEmployeeInformationObj.PerZipCode + "','" + aEmployeeInformationObj.PerContact + "','" + aEmployeeInformationObj.MailHouse + "','" + aEmployeeInformationObj.MailThana + "','" + aEmployeeInformationObj.MailDistrict + "','" + aEmployeeInformationObj.MailZipCode + "','" + aEmployeeInformationObj.MailMobile + "','" + aEmployeeInformationObj.MailEmail + "','" + aEmployeeInformationObj.FathersName + "','" + aEmployeeInformationObj.MothersName + "','" + aEmployeeInformationObj.SpouseName + "','" + aEmployeeInformationObj.Occupation + "','" + aEmployeeInformationObj.Childe1 + "','" + aEmployeeInformationObj.Childe2 + "','" + aEmployeeInformationObj.Childe3 + "','" + aEmployeeInformationObj.BankId + "','" + aEmployeeInformationObj.BranchName + "','" + aEmployeeInformationObj.AccountNo + "','" + aEmployeeInformationObj.AccountType + "')";                      
                        command.ExecuteNonQuery();
                    }
                    

                    foreach (ExamTitle ext in etList)
                    {
                        command.CommandText = @"INSERT INTO [tbl_employee_education_info]
           ([edi_serial]
           ,[edi_employee_id]
           ,[edi_exam_title_id]
           ,[edi_institute_name]
           ,[edi_gpa]
           ,[edi_grade],[edi_group],[edi_board],[edi_passing_year])
     VALUES
           ((SELECT ISNULL(MAX([edi_serial]),0) FROM [tbl_employee_education_info])+1,'" + aEmployeeInformationObj.EmployeeId + "','" + ext.ExamId + "','" + ext.Inistitute + "','" + ext.GPA + "','" + ext.Grade + "','"+ext.Group+"','"+ext.Board+"','"+ext.PassingYear+"')";
                        command.ExecuteNonQuery();
                    }

                    

                    foreach (EmployeeExperience empExp in  EmpExperienceList)
                    {
                        command.CommandText = @"INSERT INTO [tbl_employee_experience_information]
           ([empexp_serial]
           ,[empexp_organization_name]
           ,[empexp_position]
           ,[empexp_duration]
           ,[empexp_employee_id],[empexp_reasn_for_leave])
     VALUES
           ((SELECT ISNULL(MAX([empexp_serial]),0) FROM [tbl_employee_experience_information])+1,'" + empExp.OrganizationName + "','" + empExp.Position + "','" + empExp.Duration + "','" + aEmployeeInformationObj.EmployeeId + "','"+empExp.ReasonForLeave+"')";
                        command.ExecuteNonQuery();
                    }

                    foreach (RefrenceInfo aRefrenceInfoObj in EmpRefrenceList)
                    {
                        command.CommandText = @"INSERT INTO [tbl_employee_refrence_information]
           ([empref_serial]
           ,[empref_name]
           ,[empref_designation]
           ,[empref_organization]
           ,[empref_contactno]
           ,[empref_emp_id])
     VALUES
           ((SELECT ISNULL(MAX([empref_serial]),0) FROM [tbl_employee_refrence_information])+1,'" + aRefrenceInfoObj.Name + "','" + aRefrenceInfoObj.Designation + "','" + aRefrenceInfoObj.Organization + "','" + aRefrenceInfoObj.Contact + "','"+aEmployeeInformationObj.EmployeeId+"')";
                        command.ExecuteNonQuery();
                    }
                }
                transection.Commit();

            }
            catch (Exception ex)
            {
                transection.Rollback();
                throw new Exception(ex.Message);
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
        }

        public string GetEmployeeAutoId()
        {
            try
            {
                connection.Open();
                string selectQuery = @"SELECT 'Employee-' + RIGHT('000000'+CONVERT(VARCHAR,ISNULL(MAX(CONVERT(INTEGER,RIGHT([emp_employee_id],6))),0)+1),6) FROM [tbl_employee_information]";
                SqlCommand command = new SqlCommand(selectQuery, connection);
                return command.ExecuteScalar().ToString();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
        }

        public DataTable GetAllEmployeeInformation(string EmpId, string DesignationID, string Name, string TeacherOfStaff)
        {
            try
            {
                connection.Open();
                string Parameter = "";
                if (EmpId != "" && DesignationID == "" && Name=="" && TeacherOfStaff=="")
                { Parameter = "WHERE [emp_employee_id]='" + EmpId + "' "; }
                else if (EmpId == "" && DesignationID != "" && Name != "" && TeacherOfStaff !="")
                { Parameter = "WHERE [emp_designation_id] ='" + DesignationID + "' AND ([emp_first_name]+ '' +[emp_middle_name]+''+[emp_last_name]) LIKE '%" + Name + "%' AND emp_catagory='" + TeacherOfStaff + "' "; }
                else if (EmpId == "" && DesignationID == "" && Name == "" && TeacherOfStaff != "")
                { Parameter = "WHERE  emp_catagory='" + TeacherOfStaff + "' "; }
                else if (EmpId == "" && DesignationID != "" && Name == "" && TeacherOfStaff != "")
                { Parameter = "WHERE  [emp_designation_id] ='" + DesignationID + "' AND emp_catagory='" + TeacherOfStaff + "' "; }
                else if (EmpId == "" && DesignationID != "" && Name == "" && TeacherOfStaff=="")
                { Parameter = "WHERE  [emp_designation_id] ='" + DesignationID + "' "; }
                else if (EmpId == "" && DesignationID == "" && Name != "" && TeacherOfStaff != "")
                { Parameter = "WHERE ([emp_first_name]+ '' +[emp_middle_name]+''+[emp_last_name]) LIKE '%" + Name + "%' AND emp_catagory='" + TeacherOfStaff + "' "; }
                else if (EmpId == "" && DesignationID == "" && Name != "" && TeacherOfStaff=="")
                { Parameter = "WHERE ([emp_first_name]+ '' +[emp_middle_name]+''+[emp_last_name]) LIKE '%" + Name + "%' "; }
                else
                { Parameter = ""; }
                string selectQuery = @"SELECT [emp_employee_id]
      ,([emp_first_name]+ '' +[emp_middle_name]+''+[emp_last_name]) As[emp_name]
      ,[emp_fathers_name]
      ,[emp_mothers_name]
      ,convert(nvarchar,[emp_dateof_birth],103)emp_dateof_birth
      ,convert(nvarchar,[emp_joining_date],103)emp_joining_date
      ,[emp_blood]
      ,[emp_religion]
      ,[emp_gender]
      ,(SELECT [dept_name] FROM [tbl_department_information] WHERE  [dept_id]=[emp_department_id]) AS [dept_name]
      ,(SELECT [desig_name] FROM [tbl_designation_information] WHERE  [desig_id]=[emp_designation_id]) AS [desig_name]
      ,[emp_job_status]   
      ,[emp_mail_mobile]   
  FROM [tbl_employee_information]  " + Parameter;
                SqlDataAdapter da = new SqlDataAdapter(selectQuery, connection);
                DataSet ds = new DataSet();
                da.Fill(ds, "tbl_employee_information");
                DataTable table = ds.Tables["tbl_employee_information"];
                return table;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);

            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();

                }
            }
        }

        public EmployeeInformation GetAllEmployeeInformationForSpecificEmployee(string empId)
        {
            try
            {
                connection.Open();
                string selectQuery = @"SELECT [emp_nid]
      ,[emp_first_name]
      ,[emp_middle_name]
      ,[emp_last_name]
      ,[emp_catagory]
      ,[emp_designation_id]
      ,[emp_department_id]
      ,convert(varchar,[emp_joining_date],103)[emp_joining_date]
      ,[emp_gender]
      ,convert(varchar,[emp_dateof_birth],103) [emp_dateof_birth]
      ,[emp_religion]
      ,[emp_blood]
      ,[emp_marital_status]
      ,[emp_service_Preiord]
      ,[emp_job_status]
      ,[emp_tin]
      ,[emp_pasport]
      , isnull([emp_bed],0) AS [emp_bed]
      ,isnull([emp_med],0) AS [emp_med]
      ,isnull([emp_ntrc],0) AS [emp_ntrc]
      ,isnull([emp_na],0) AS [emp_na]
      ,[emp_extra_currlm1]
      ,[emp_extra_currlm2]
      ,[emp_extra_currlm3]
      ,[emp_per_house]
      ,[emp_per_thana]
      ,[emp_per_District]
      ,[emp_per_zip_code]
      ,[emp_per_contact_no]
      ,[emp_mail_house]
      ,[emp_mail_thana]
      ,[emp_mail_district]
      ,[emp_mail_zip_code]
      ,[emp_mail_mobile]
      ,[emp_mail_email]
      ,[emp_fathers_name]
      ,[emp_mothers_name]
      ,[emp_spouse_name]
      ,[emp_occupation]
      ,[emp_childe_info1]
      ,[emp_childe_info2]
      ,[emp_childe_info3]
      ,[emp_bank_id]
      ,[emp_branch_name]
      ,[emp_account_no]
      ,[emp_account_type]      
  FROM [tbl_employee_information]  WHERE [emp_employee_id]='" + empId + "'";
                SqlCommand command = new SqlCommand(selectQuery, connection);
                SqlDataReader reader = command.ExecuteReader();
                EmployeeInformation aEmployeeInformationObj = new EmployeeInformation();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {

                        aEmployeeInformationObj.NID = reader[0].ToString();
                        aEmployeeInformationObj.FirstName = reader[1].ToString();
                        aEmployeeInformationObj.MiddleName = reader[2].ToString();
                        aEmployeeInformationObj.LastName = reader[3].ToString();
                        aEmployeeInformationObj.EmployeeCategory = reader[4].ToString();
                        aEmployeeInformationObj.Designation = reader[5].ToString();
                        aEmployeeInformationObj.Section = reader[6].ToString();
                        //aEmployeeInformationObj.JoiningDate =Convert.ToDateTime(reader[7]);
                        aEmployeeInformationObj.JoiningDate =  reader[7].ToString();
                        aEmployeeInformationObj.Sex = reader[8].ToString();
                        //aEmployeeInformationObj.DateOfBirth =Convert.ToDateTime(reader[9]);
                        aEmployeeInformationObj.DateOfBirth =  reader[9].ToString();
                        aEmployeeInformationObj.Religion = reader[10].ToString();
                        aEmployeeInformationObj.BloodGroup = reader[11].ToString();
                        aEmployeeInformationObj.MaritalStatus = reader[12].ToString();
                        aEmployeeInformationObj.ServicePeriod = reader[13].ToString();
                        aEmployeeInformationObj.Jobstatus = reader[14].ToString();
                        aEmployeeInformationObj.TIN = reader[15].ToString();
                        aEmployeeInformationObj.Pasport = reader[16].ToString();
                        aEmployeeInformationObj.BEd = Convert.ToBoolean(reader[17]);
                        aEmployeeInformationObj.MEd = Convert.ToBoolean(reader[18]);
                        aEmployeeInformationObj.NTRCA = Convert.ToBoolean(reader[19]);
                        aEmployeeInformationObj.NA = Convert.ToBoolean(reader[20]);
                        aEmployeeInformationObj.ExtraCurrlm1 = reader[21].ToString();
                        aEmployeeInformationObj.ExtraCurrlm2 = reader[22].ToString();
                        aEmployeeInformationObj.ExtraCurrlm3 = reader[23].ToString();
                        aEmployeeInformationObj.PerHouseNo = reader[24].ToString();
                        aEmployeeInformationObj.PerThana = reader[25].ToString();
                        aEmployeeInformationObj.PerDistrict = reader[26].ToString();
                        aEmployeeInformationObj.PerZipCode = reader[27].ToString();
                        aEmployeeInformationObj.PerContact = reader[28].ToString();
                        aEmployeeInformationObj.MailHouse = reader[29].ToString();
                        aEmployeeInformationObj.MailThana = reader[30].ToString();
                        aEmployeeInformationObj.MailDistrict = reader[31].ToString();
                        aEmployeeInformationObj.MailZipCode = reader[32].ToString();
                        aEmployeeInformationObj.MailMobile = reader[33].ToString();
                        aEmployeeInformationObj.MailEmail = reader[34].ToString();
                        aEmployeeInformationObj.FathersName = reader[35].ToString();
                        aEmployeeInformationObj.MothersName = reader[36].ToString();
                        aEmployeeInformationObj.SpouseName = reader[37].ToString();
                        aEmployeeInformationObj.Occupation = reader[38].ToString();
                        aEmployeeInformationObj.Childe1 = reader[39].ToString();
                        aEmployeeInformationObj.Childe2 = reader[40].ToString();
                        aEmployeeInformationObj.Childe3 = reader[41].ToString();
                        aEmployeeInformationObj.BankId = reader[42].ToString();
                        aEmployeeInformationObj.BranchName = reader[43].ToString();
                        aEmployeeInformationObj.AccountNo = reader[44].ToString();
                        aEmployeeInformationObj.AccountType = reader[45].ToString();
                    }
                }
                return aEmployeeInformationObj;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }

        }

         

        //public void DeleteTheEmployeeInformation(EmployeeInformation aEmployeeInformationObj)
        //{
        //    try
        //    {
        //        connection.Open();
        //        transection = connection.BeginTransaction();
        //        SqlCommand command = new SqlCommand();
        //        command.Connection = connection;
        //        command.Transaction = transection;


        //        command.CommandText = @"DELETE FROM [tbl_employee_information] WHERE [emp_employee_id] ='" + aEmployeeInformationObj.EmployeeId + "' ";
        //        command.ExecuteNonQuery();

        //        command.CommandText = @"DELETE FROM [tbl_employee_education_info] WHERE [edi_employee_id]='" + aEmployeeInformationObj.EmployeeId + "' ";
        //        command.ExecuteNonQuery();

        //        command.CommandText = @"DELETE FROM [tbl_employee_specialzation_information] WHERE [empsz_employee_id]='" + aEmployeeInformationObj.EmployeeId + "' ";
        //        command.ExecuteNonQuery();
                 

        //        transection.Commit();

        //    }
        //    catch (Exception ex)
        //    {
        //        transection.Rollback();
        //        throw new Exception(ex.Message);
        //    }
        //    finally
        //    {
        //        if (connection.State == ConnectionState.Open)
        //        {
        //            connection.Close();
        //        }
        //    }
        //}

        internal byte[] GetEmployeeImageForSpecificEmployee(string employeeId)
        {
            try
            {
                connection.Open();
                string selectQuery = @"SELECT  isnull([emp_image],0)      
  FROM [tbl_employee_information] WHERE [emp_employee_id]='" + employeeId + "'";
                SqlCommand command = new SqlCommand(selectQuery,connection);
                return (byte[]) command.ExecuteScalar();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
        }

        internal byte[] GetEmployeeSignImageForSpecificEmployee(string employeeId)
        {
            try
            {
                connection.Open();
                string selectQuery = @"SELECT isnull([emp_signature],0)
  FROM [tbl_employee_information] WHERE [emp_employee_id]='" + employeeId + "'";
                SqlCommand command = new SqlCommand(selectQuery, connection);
                return (byte[])command.ExecuteScalar();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
        }

        internal DataTable GetAllExamTitleInformationForSpecifEmployee(string employeeId)
        {
            try
            {
                connection.Open();
                string selectQuery = @"SELECT  [edi_exam_title_id] AS [Id]
      ,[edi_institute_name] AS [Institute]
      ,[edi_gpa] AS [GPA]
      ,[edi_grade] AS [Grade]
      ,[edi_group] AS [Group]
      ,[edi_board] AS [Board/University]
      ,[edi_passing_year] AS [Passing Year]
  FROM [tbl_employee_education_info] WHERE [edi_employee_id]='" +employeeId+"'";
                SqlDataAdapter da = new SqlDataAdapter(selectQuery, connection);
                DataSet ds = new DataSet();
                da.Fill(ds, "tbl_employee_education_info");
                DataTable table = ds.Tables["tbl_employee_education_info"];
                return table;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
        }

        internal DataTable GetAllExperienceInformationForSpecificEmployee(string employeeId)
        {
            try
            {
                connection.Open();
                string selectQuery = @"SELECT [empexp_serial] AS [Serial]
      ,[empexp_organization_name] AS [OrganizationName]
      ,[empexp_position] AS [Position]
      ,[empexp_duration] AS [Duration]     
      ,[empexp_reasn_for_leave] AS [ReasonForLeave]
  FROM [tbl_employee_experience_information] WHERE [empexp_employee_id]='" + employeeId + "'";
                SqlDataAdapter da = new SqlDataAdapter(selectQuery, connection);
                DataSet ds = new DataSet();
                da.Fill(ds, "tbl_employee_experience_information");
                DataTable table = ds.Tables["tbl_employee_experience_information"];
                return table;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
        }

        internal DataTable GetAllRefrenceInformationForSpecifcEmployee(string employeeId)
        {
            try
            {
                connection.Open();
                string selectQuery = @"SELECT [empref_serial] AS [Serial]
      ,[empref_name] AS [Name]
      ,[empref_designation] AS [Designation]
      ,[empref_organization] AS [Organization]
      ,[empref_contactno] AS [Contact]      
  FROM [tbl_employee_refrence_information] WHERE [empref_emp_id]='" + employeeId + "'";
                SqlDataAdapter da = new SqlDataAdapter(selectQuery, connection);
                DataSet ds = new DataSet();
                da.Fill(ds, "tbl_employee_refrence_information");
                DataTable table = ds.Tables["tbl_employee_refrence_information"];
                return table;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
        }

        internal void UpdateTheEmployeeInformation(EmployeeInformation aEmployeeInformationObj, List<ExamTitle> etList, List<EmployeeExperience> EmpExperienceList, List<RefrenceInfo> EmpRefrenceList, byte[] image, byte[] sig)
        {
            try
            {
                connection.Open();
                transection = connection.BeginTransaction();
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transection;


                command.CommandText = @"UPDATE [tbl_employee_information]
   SET [emp_nid] = '" + aEmployeeInformationObj.NID + "',[emp_first_name] = '" + aEmployeeInformationObj.FirstName + "' ,[emp_middle_name] ='" + aEmployeeInformationObj.MiddleName + "',[emp_last_name] = '" + aEmployeeInformationObj.LastName + "',[emp_catagory] ='" + aEmployeeInformationObj.EmployeeCategory + "',[emp_designation_id] ='" + aEmployeeInformationObj.Designation + "',[emp_department_id] ='" + aEmployeeInformationObj.Section + "',[emp_joining_date] ='" + aEmployeeInformationObj.JoiningDate + "',[emp_gender] ='" + aEmployeeInformationObj.Sex + "' ,[emp_dateof_birth] ='" + aEmployeeInformationObj.DateOfBirth + "',[emp_religion] = '" + aEmployeeInformationObj.Religion + "',[emp_blood] ='" + aEmployeeInformationObj.BloodGroup + "',[emp_marital_status] = '" + aEmployeeInformationObj.MaritalStatus + "',[emp_service_Preiord] = '" + aEmployeeInformationObj.ServicePeriod + "',[emp_job_status] ='" + aEmployeeInformationObj.Jobstatus + "',[emp_tin] ='" + aEmployeeInformationObj.TIN + "',[emp_pasport] ='" + aEmployeeInformationObj.Pasport + "',[emp_bed] ='" + aEmployeeInformationObj.BEd + "',[emp_med] ='" + aEmployeeInformationObj.MEd + "',[emp_ntrc] ='" + aEmployeeInformationObj.NTRCA + "',[emp_na] ='" + aEmployeeInformationObj.NA + "',[emp_extra_currlm1] ='" + aEmployeeInformationObj.ExtraCurrlm1 + "',[emp_extra_currlm2] = '" + aEmployeeInformationObj.ExtraCurrlm2 + "',[emp_extra_currlm3] ='" + aEmployeeInformationObj.ExtraCurrlm3 + "',[emp_per_house] ='" + aEmployeeInformationObj.PerHouseNo + "',[emp_per_thana] ='" + aEmployeeInformationObj.PerThana + "',[emp_per_District] ='" + aEmployeeInformationObj.PerDistrict + "',[emp_per_zip_code] ='" + aEmployeeInformationObj.PerZipCode + "',[emp_per_contact_no] ='" + aEmployeeInformationObj.PerContact + "',[emp_mail_house] ='" + aEmployeeInformationObj.MailHouse + "',[emp_mail_thana] ='" + aEmployeeInformationObj.MailThana + "',[emp_mail_district] ='" + aEmployeeInformationObj.MailDistrict + "',[emp_mail_zip_code] ='" + aEmployeeInformationObj.MailZipCode + "',[emp_mail_mobile] ='" + aEmployeeInformationObj.MailMobile + "',[emp_mail_email] ='" + aEmployeeInformationObj.MailEmail + "',[emp_fathers_name] ='" + aEmployeeInformationObj.FathersName + "',[emp_mothers_name] ='" + aEmployeeInformationObj.MothersName + "',[emp_spouse_name] ='" + aEmployeeInformationObj.SpouseName + "',[emp_occupation] ='" + aEmployeeInformationObj.Occupation + "',[emp_childe_info1] = '" + aEmployeeInformationObj.Childe1 + "',[emp_childe_info2] = '" + aEmployeeInformationObj.Childe2 + "',[emp_childe_info3] ='" + aEmployeeInformationObj.Childe3 + "',[emp_bank_id] ='" + aEmployeeInformationObj.BankId + "',[emp_branch_name] ='" + aEmployeeInformationObj.BranchName + "',[emp_account_no] = '" + aEmployeeInformationObj.AccountNo + "',[emp_account_type] ='" + aEmployeeInformationObj.AccountType + "',[emp_image] =@image,[emp_signature] =@sig WHERE  [emp_employee_id] = '" + aEmployeeInformationObj.EmployeeId + "' SELECT @@IDENTITY  ";
                command.Parameters.AddWithValue("@image", image);
                command.Parameters.AddWithValue("@sig", sig);
                command.ExecuteNonQuery();

                command.CommandText = @"DELETE FROM [tbl_employee_education_info] WHERE  [edi_employee_id]='" + aEmployeeInformationObj.EmployeeId + "' ";
                command.ExecuteNonQuery();

                command.CommandText = @"DELETE FROM [tbl_employee_experience_information] WHERE [empexp_employee_id]='" + aEmployeeInformationObj.EmployeeId + "' ";
                command.ExecuteNonQuery();

                command.CommandText = @"DELETE FROM [tbl_employee_refrence_information]  WHERE [empref_emp_id]='" + aEmployeeInformationObj.EmployeeId + "' ";
                command.ExecuteNonQuery();

                foreach (ExamTitle ext in etList)
                {
                    command.CommandText = @"INSERT INTO [tbl_employee_education_info]
           ([edi_serial]
           ,[edi_employee_id]
           ,[edi_exam_title_id]
           ,[edi_institute_name]
           ,[edi_gpa]
           ,[edi_grade],[edi_group],[edi_board],[edi_passing_year])
     VALUES
           ((SELECT ISNULL(MAX([edi_serial]),0) FROM [tbl_employee_education_info])+1,'" + aEmployeeInformationObj.EmployeeId + "','" + ext.ExamId + "','" + ext.Inistitute + "','" + ext.GPA + "','" + ext.Grade + "','" + ext.Group + "','" + ext.Board + "','" + ext.PassingYear + "')";
                    command.ExecuteNonQuery();
                }



                foreach (EmployeeExperience empExp in EmpExperienceList)
                {
                    command.CommandText = @"INSERT INTO [tbl_employee_experience_information]
           ([empexp_serial]
           ,[empexp_organization_name]
           ,[empexp_position]
           ,[empexp_duration]
           ,[empexp_employee_id],[empexp_reasn_for_leave])
     VALUES
           ((SELECT ISNULL(MAX([empexp_serial]),0) FROM [tbl_employee_experience_information])+1,'" + empExp.OrganizationName + "','" + empExp.Position + "','" + empExp.Duration + "','" + aEmployeeInformationObj.EmployeeId + "','" + empExp.ReasonForLeave + "')";
                    command.ExecuteNonQuery();
                }

                foreach (RefrenceInfo aRefrenceInfoObj in EmpRefrenceList)
                {
                    command.CommandText = @"INSERT INTO [tbl_employee_refrence_information]
           ([empref_serial]
           ,[empref_name]
           ,[empref_designation]
           ,[empref_organization]
           ,[empref_contactno]
           ,[empref_emp_id])
     VALUES
           ((SELECT ISNULL(MAX([empref_serial]),0) FROM [tbl_employee_refrence_information])+1,'" + aRefrenceInfoObj.Name + "','" + aRefrenceInfoObj.Designation + "','" + aRefrenceInfoObj.Organization + "','" + aRefrenceInfoObj.Contact + "','" + aEmployeeInformationObj.EmployeeId + "')";
                    command.ExecuteNonQuery();
                }

                transection.Commit();

            }
            catch (Exception ex)
            {
                transection.Rollback();
                throw new Exception(ex.Message);
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
        }

        internal void DeleteTheEmployeeInformation(string employeeId)
        {
            try
            {
                    connection.Open();
                    transection = connection.BeginTransaction();
                    SqlCommand command = new SqlCommand();
                    command.Connection = connection;
                    command.Transaction = transection;


                    command.CommandText = @"DELETE FROM [tbl_employee_information] WHERE [emp_employee_id] ='" + employeeId + "' ";
                    command.ExecuteNonQuery();

                    command.CommandText = @"DELETE FROM [tbl_employee_education_info] WHERE  [edi_employee_id]='" + employeeId + "' ";
                    command.ExecuteNonQuery();

                    command.CommandText = @"DELETE FROM [tbl_employee_experience_information] WHERE [empexp_employee_id]='" + employeeId + "' ";
                    command.ExecuteNonQuery();

                    command.CommandText = @"DELETE FROM [tbl_employee_refrence_information]  WHERE [empref_emp_id]='" + employeeId + "' ";
                    command.ExecuteNonQuery();


                    transection.Commit();

                }
                catch (Exception ex)
                {
                    transection.Rollback();
                    throw new Exception(ex.Message);
                }
                finally
                {
                    if (connection.State == ConnectionState.Open)
                    {
                        connection.Close();
                    }
                }
        }
        internal DataTable GetShowEmployeeOnTeacher()
        {
            try
            {
                connection.Open();
                string selectQuery = @"SELECT [emp_employee_id]
      ,([emp_first_name]+ '' +[emp_middle_name]+''+[emp_last_name]) As[emp_name]         
  FROM [tbl_employee_information] where (SELECT [desig_name] FROM [tbl_designation_information] WHERE [desig_id]=[emp_designation_id])='Teacher'";
                SqlDataAdapter da = new SqlDataAdapter(selectQuery, connection);
                DataSet ds = new DataSet();
                da.Fill(ds, "tbl_employee_information");
                DataTable table = ds.Tables["tbl_employee_information"];
                return table;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
        }
    }
}