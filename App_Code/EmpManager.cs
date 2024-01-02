using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Data.SqlClient;
using System.IO;
using  Dorjibari;
using autouniv;

/// <summary>
/// Summary description for EmpManager
/// </summary>
/// 
namespace Dorjibari
{
    public class EmpManager
    {
        public static void CreateEmp(Emp emp)
        {
            String connectionString = DataManager.OraConnString();
            SqlConnection sqlCon = new SqlConnection(connectionString);
            if (emp.PenssionAmount == "")
            {
                emp.PenssionAmount = "0";
            }

            emp.EmpNo = "DB" + IdManager.GiftItemSerialNo("", "replace(EMP_NO,'DB','')", "dbo.PMIS_PERSONNEL");

            string variables = "emp_no,f_name,m_name,l_name,join_date,job_status,join_desig_code, confirm_date,emp_status,emp_type,emp_cat,pf_no,emp_birth_dt,sex,place_of_birth,marital_status_code,blood_group,spouse_name,nationality,fh_name,mh_name,religion_code,per_loc,per_dist_code,per_thana_code, zip_area_code,mail_loc,mail_dist_code,mail_thana_code,e_mail,res_ph_no,mobile,bank_acc_no,emp_insured_dt, spouse_ins_dt,senr_sl_no,prst_desig_code,prst_post_br,lpr_date,pass_no,driv_lic_no,national_id,personeel_file_no,tin_no,GlCoaCode,CountryID,BasicSalary,BANK_NO,BranchID,RetirmentDate,PenssionAmount,[LocalUploadBranchID] ";
            string values = " '" + emp.EmpNo + "',  '" + emp.FName + "',  '" + emp.MName + "',   '" + emp.LName + "', " +
                    "  convert(datetime,nullif('" + emp.JoinDate + "',''),103),  '" + emp.JobStatus + "',   '" + emp.JoinDesigCode + "', " +
                    "  convert(datetime,nullif('" + emp.ConfirmDate + "',''),103), '" + emp.EmpStatus + "', '" + emp.EmpType + "', " +
                    "  '" + emp.EmpCat + "','" + emp.PfNo + "', convert(datetime,nullif('" + emp.EmpBirthDt + "',''),103), " +
                    "  '" + emp.Sex + "', '" + emp.PlaceOfBirth + "', '" + emp.MaritalStatusCode + "', '" + emp.BloodGroup + "', " +
                    "  '" + emp.SpouseName + "', '" + emp.Nationality + "', '" + emp.FhName + "', '" + emp.MhName + "', " +
                    "  '" + emp.ReligionCode + "', '" + emp.PerLoc + "', '" + emp.PerDistCode + "', '" + emp.PerThanaCode + "', " +
                    "  '" + emp.ZipAreaCode + "', '" + emp.MailLoc + "', '" + emp.MailDistCode + "', '" + emp.MailThanaCode + "', " +
                    "  '" + emp.EMail + "', '" + emp.ResPhNo + "', '" + emp.Mobile + "', '" + emp.BankAccNo + "', " +
                    "  convert(datetime,nullif('" + emp.EmpInsuredDt + "',''),103), convert(datetime,nullif('" + emp.SpouseInsDt + "',''),103), convert(numeric,nullif('" + emp.SenrSlNo + "','')), " +
                    "  '" + emp.PrstDesigCode + "', '" + emp.PrstPostBr + "', " +
                    "  convert(datetime,nullif('" + emp.LprDate + "',''),103), '" + emp.PassNo + "', '" + emp.DrivLicNo + "', '" + emp.NationalId + "', '" + emp.PersoneelFileNo + "', '" + emp.TinNo + "','" + emp.GlCoaCode + "','" + emp.County + "','" + emp.BasicSalary + "','" + emp.BankBranchNo + "','" + emp.BranchID + "' ,Convert(date,'" + emp.RetirmentDate + "',103),'" + emp.PenssionAmount + "','0,' ";
            string query = "";

            if (emp.EmpPhoto != null)
            {
                if (emp.EmpPhoto.Length > 0)
                {
                    variables = variables + ",emp_photo";
                    values = values + ",@img";
                }
            }
            if (emp.SpecSigna != null)
            {
                if (emp.SpecSigna.Length > 0)
                {
                    variables = variables + ",spec_signa";
                    values = values + ",@sig";
                }
            }

            query = " insert into pmis_personnel (" + variables + ")  values ( " + values + " )";
            SqlCommand cmnd;
            cmnd = new SqlCommand(query, sqlCon);
            SqlParameter img = new SqlParameter();
            img.SqlDbType = SqlDbType.VarBinary;
            img.ParameterName = "img";
            img.Value = emp.EmpPhoto;
            cmnd.Parameters.Add(img);
            if (emp.EmpPhoto == null)
            {
                cmnd.Parameters.Remove(img);
            }
            else
            {
                if (emp.EmpPhoto.Length == 0)
                {
                    cmnd.Parameters.Remove(img);
                }
            }

            img = new SqlParameter();
            img.SqlDbType = SqlDbType.VarBinary;
            img.ParameterName = "sig";
            img.Value = emp.SpecSigna;
            cmnd.Parameters.Add(img);
            if (emp.SpecSigna == null)
            {
                cmnd.Parameters.Remove(img);
            }
            else
            {
                if (emp.SpecSigna.Length == 0)
                {
                    cmnd.Parameters.Remove(img);
                }
            }
            sqlCon.Open();
            cmnd.ExecuteNonQuery();
            sqlCon.Close();
        }
        public static void UpdateEmp(Emp emp)
        {
            String connectionString = DataManager.OraConnString();
            SqlConnection sqlCon = new SqlConnection(connectionString);

            int prvBranchID = IdManager.GetShowSingleValueInt("isnull(BranchID,0)", "pmis_personnel where emp_no='" + emp.EmpNo + "'");
           
                emp.PrvBranch = prvBranchID.ToString();
                if (emp.PenssionAmount == "")
                {
                    emp.PenssionAmount = "0";
                }
            string variables = "f_name= '" + emp.FName + "', m_name= '" + emp.MName + "',  l_name= '" + emp.LName + "', " +
                      " join_date= convert(datetime,nullif('" + emp.JoinDate + "',''),103), job_status= '" + emp.JobStatus + "',  join_desig_code= '" + emp.JoinDesigCode + "', " +
                      " confirm_date= convert(datetime,nullif('" + emp.ConfirmDate + "',''),103),emp_status= '" + emp.EmpStatus + "',emp_type= '" + emp.EmpType + "', " +
                      " emp_cat= '" + emp.EmpCat + "',pf_no= '" + emp.PfNo + "',[GlCoaCode]='"+emp.GlCoaCode+"',emp_birth_dt= convert(datetime,nullif('" + emp.EmpBirthDt + "',''),103), " +
                      " sex= '" + emp.Sex + "',place_of_birth= '" + emp.PlaceOfBirth + "',marital_status_code= '" + emp.MaritalStatusCode + "',blood_group= '" + emp.BloodGroup + "', " +
                      " spouse_name= '" + emp.SpouseName + "',nationality= '" + emp.Nationality + "',fh_name= '" + emp.FhName + "',mh_name= '" + emp.MhName + "', " +
                      " religion_code= '" + emp.ReligionCode + "',per_loc= '" + emp.PerLoc + "',per_dist_code= '" + emp.PerDistCode + "',per_thana_code= '" + emp.PerThanaCode + "', " +
                      " zip_area_code= '" + emp.ZipAreaCode + "',mail_loc= '" + emp.MailLoc + "',mail_dist_code= '" + emp.MailDistCode + "',mail_thana_code= '" + emp.MailThanaCode + "', " +
                      " e_mail= '" + emp.EMail + "',res_ph_no= '" + emp.ResPhNo + "',mobile= '" + emp.Mobile + "',bank_acc_no= '" + emp.BankAccNo + "', " +
                      " emp_insured_dt= convert(datetime,nullif('" + emp.EmpInsuredDt + "',''),103),spouse_ins_dt= convert(datetime,nullif('" + emp.SpouseInsDt + "',''),103),senr_sl_no= convert(numeric,nullif('" + emp.SenrSlNo + "','')), " +
                      " prst_desig_code= '" + emp.PrstDesigCode + "',prst_post_br= '" + emp.PrstPostBr + "',RetirmentDate=Convert(date,'"+emp.RetirmentDate+"',103),PenssionAmount='"+emp.PenssionAmount+"'," +
                      " lpr_date= convert(datetime,nullif('" + emp.LprDate + "',''),103),pass_no= '" + emp.PassNo + "',[LocalUploadBranchID]='0,', PrvBranchID='" + emp.PrvBranch + "'  ,BranchID='" + emp.BranchID + "',driv_lic_no= '" + emp.DrivLicNo + "',national_id= '" + emp.NationalId + "',CountryID='" + emp.County + "',LocalUpload=2,BasicSalary='" + emp.BasicSalary + "',BANK_NO='" + emp.BankBranchNo + "' ";
            string query = "";

            if (emp.EmpPhoto != null)
            {
                if (emp.EmpPhoto.Length > 0)
                {
                    variables = variables + ",emp_photo=@img";
                }
            }

            if (emp.SpecSigna != null)
            {
                if (emp.SpecSigna.Length > 0)
                {
                    variables = variables + ",spec_signa=@sig"; 
                }
            }

            query = " update pmis_personnel set " + variables + " where emp_no='" + emp.EmpNo + "' ";

            SqlCommand cmnd;
            cmnd = new SqlCommand(query, sqlCon);
            SqlParameter img = new SqlParameter();
            img.SqlDbType = SqlDbType.VarBinary;
            img.ParameterName = "img";
            img.Value = emp.EmpPhoto;
            cmnd.Parameters.Add(img);
            if (emp.EmpPhoto == null)
            {
                cmnd.Parameters.Remove(img);
            }
            else
            {
                if (emp.EmpPhoto.Length == 0)
                {
                    cmnd.Parameters.Remove(img);
                }
            }

            img = new SqlParameter();
            img.SqlDbType = SqlDbType.VarBinary;
            img.ParameterName = "sig";
            img.Value = emp.SpecSigna;
            cmnd.Parameters.Add(img);
            if (emp.SpecSigna == null)
            {
                cmnd.Parameters.Remove(img);
            }
            else
            {
                if (emp.SpecSigna.Length == 0)
                {
                    cmnd.Parameters.Remove(img);
                }
            }
            sqlCon.Open();
            cmnd.ExecuteNonQuery();
            sqlCon.Close();
            
            //**********************************//
            //String connectionString = DataManager.OraConnString();            
            //using (SqlConnection sqlCon = new SqlConnection(connectionString))
            //{
            //    string query = "";
            //    if (emp.EmpPhoto != null)
            //    {
            //        if (emp.SpecSigna != null)
            //        {
            //            query = " update pmis_personnel set f_name= '" + emp.FName + "', m_name= '" + emp.MName + "',  l_name= '" + emp.LName + "', " +
            //          " join_date= convert(datetime,nullif('" + emp.JoinDate + "',''),103), job_status= '" + emp.JobStatus + "',  join_desig_code= '" + emp.JoinDesigCode + "', " +
            //          " confirm_date= convert(datetime,nullif('" + emp.ConfirmDate + "',''),103),emp_status= '" + emp.EmpStatus + "',emp_type= '" + emp.EmpType + "', " +
            //          " emp_cat= '" + emp.EmpCat + "',pf_no= '" + emp.PfNo + "',emp_birth_dt= convert(datetime,nullif('" + emp.EmpBirthDt + "',''),103), " +
            //          " sex= '" + emp.Sex + "',place_of_birth= '" + emp.PlaceOfBirth + "',marital_status_code= '" + emp.MaritalStatusCode + "',blood_group= '" + emp.BloodGroup + "', " +
            //          " spouse_name= '" + emp.SpouseName + "',nationality= '" + emp.Nationality + "',fh_name= '" + emp.FhName + "',mh_name= '" + emp.MhName + "', " +
            //          " religion_code= '" + emp.ReligionCode + "',per_loc= '" + emp.PerLoc + "',per_dist_code= '" + emp.PerDistCode + "',per_thana_code= '" + emp.PerThanaCode + "', " +
            //          " zip_area_code= '" + emp.ZipAreaCode + "',mail_loc= '" + emp.MailLoc + "',mail_dist_code= '" + emp.MailDistCode + "',mail_thana_code= '" + emp.MailThanaCode + "', " +
            //          " e_mail= '" + emp.EMail + "',res_ph_no= '" + emp.ResPhNo + "',mobile= '" + emp.Mobile + "',bank_acc_no= '" + emp.BankAccNo + "', " +
            //          " emp_insured_dt= convert(datetime,nullif('" + emp.EmpInsuredDt + "',''),103),spouse_ins_dt= convert(datetime,nullif('" + emp.SpouseInsDt + "',''),103),senr_sl_no= convert(numeric,nullif('" + emp.SenrSlNo + "','')), " +
            //          " prst_desig_code= '" + emp.PrstDesigCode + "',prst_post_br= '" + emp.PrstPostBr + "', emp_photo=@img,spec_signa=@sig, " +
            //          " lpr_date= convert(datetime,nullif('" + emp.LprDate + "',''),103),pass_no= '" + emp.PassNo + "',driv_lic_no= '" + emp.DrivLicNo + "',national_id= '" + emp.NationalId + "',personeel_file_no= '" + emp.PersoneelFileNo + "',tin_no= '" + emp.TinNo + "',CountryID='"+emp.County+"' where emp_no='" + emp.EmpNo + "'";
            //        }
            //        else
            //        {
            //            query = " update pmis_personnel set f_name= '" + emp.FName + "', m_name= '" + emp.MName + "',  l_name= '" + emp.LName + "', " +
            //     " join_date= convert(datetime,nullif('" + emp.JoinDate + "',''),103), job_status= '" + emp.JobStatus + "',  join_desig_code= '" + emp.JoinDesigCode + "', " +
            //     " confirm_date= convert(datetime,nullif('" + emp.ConfirmDate + "',''),103),emp_status= '" + emp.EmpStatus + "',emp_type= '" + emp.EmpType + "', " +
            //     " emp_cat= '" + emp.EmpCat + "',pf_no= '" + emp.PfNo + "',emp_birth_dt= convert(datetime,nullif('" + emp.EmpBirthDt + "',''),103), " +
            //     " sex= '" + emp.Sex + "',place_of_birth= '" + emp.PlaceOfBirth + "',marital_status_code= '" + emp.MaritalStatusCode + "',blood_group= '" + emp.BloodGroup + "', " +
            //     " spouse_name= '" + emp.SpouseName + "',nationality= '" + emp.Nationality + "',fh_name= '" + emp.FhName + "',mh_name= '" + emp.MhName + "', " +
            //     " religion_code= '" + emp.ReligionCode + "',per_loc= '" + emp.PerLoc + "',per_dist_code= '" + emp.PerDistCode + "',per_thana_code= '" + emp.PerThanaCode + "', " +
            //     " zip_area_code= '" + emp.ZipAreaCode + "',mail_loc= '" + emp.MailLoc + "',mail_dist_code= '" + emp.MailDistCode + "',mail_thana_code= '" + emp.MailThanaCode + "', " +
            //     " e_mail= '" + emp.EMail + "',res_ph_no= '" + emp.ResPhNo + "',mobile= '" + emp.Mobile + "',bank_acc_no= '" + emp.BankAccNo + "', " +
            //     " emp_insured_dt= convert(datetime,nullif('" + emp.EmpInsuredDt + "',''),103),spouse_ins_dt= convert(datetime,nullif('" + emp.SpouseInsDt + "',''),103),senr_sl_no= convert(numeric,nullif('" + emp.SenrSlNo + "','')), " +
            //     " prst_desig_code= '" + emp.PrstDesigCode + "',prst_post_br= '" + emp.PrstPostBr + "', emp_photo=@img,spec_signa=null, " +
            //     " lpr_date= convert(datetime,nullif('" + emp.LprDate + "',''),103),pass_no= '" + emp.PassNo + "',driv_lic_no= '" + emp.DrivLicNo + "',national_id= '" + emp.NationalId + "',personeel_file_no= '" + emp.PersoneelFileNo + "',tin_no= '" + emp.TinNo + "',CountryID='" + emp.County + "' where emp_no='" + emp.EmpNo + "'";
            //        }
            //    }
            //    else
            //    {
            //        query = " update pmis_personnel set f_name= '" + emp.FName + "', m_name= '" + emp.MName + "',  l_name= '" + emp.LName + "', " +
            //     " join_date= convert(datetime,nullif('" + emp.JoinDate + "',''),103), job_status= '" + emp.JobStatus + "',  join_desig_code= '" + emp.JoinDesigCode + "', " +
            //     " confirm_date= convert(datetime,nullif('" + emp.ConfirmDate + "',''),103),emp_status= '" + emp.EmpStatus + "',emp_type= '" + emp.EmpType + "', " +
            //     " emp_cat= '" + emp.EmpCat + "',pf_no= '" + emp.PfNo + "',emp_birth_dt= convert(datetime,nullif('" + emp.EmpBirthDt + "',''),103), " +
            //     " sex= '" + emp.Sex + "',place_of_birth= '" + emp.PlaceOfBirth + "',marital_status_code= '" + emp.MaritalStatusCode + "',blood_group= '" + emp.BloodGroup + "', " +
            //     " spouse_name= '" + emp.SpouseName + "',nationality= '" + emp.Nationality + "',fh_name= '" + emp.FhName + "',mh_name= '" + emp.MhName + "', " +
            //     " religion_code= '" + emp.ReligionCode + "',per_loc= '" + emp.PerLoc + "',per_dist_code= '" + emp.PerDistCode + "',per_thana_code= '" + emp.PerThanaCode + "', " +
            //     " zip_area_code= '" + emp.ZipAreaCode + "',mail_loc= '" + emp.MailLoc + "',mail_dist_code= '" + emp.MailDistCode + "',mail_thana_code= '" + emp.MailThanaCode + "', " +
            //     " e_mail= '" + emp.EMail + "',res_ph_no= '" + emp.ResPhNo + "',mobile= '" + emp.Mobile + "',bank_acc_no= '" + emp.BankAccNo + "', " +
            //     " emp_insured_dt= convert(datetime,nullif('" + emp.EmpInsuredDt + "',''),103),spouse_ins_dt= convert(datetime,nullif('" + emp.SpouseInsDt + "',''),103),senr_sl_no= convert(numeric,nullif('" + emp.SenrSlNo + "','')), " +
            //     " prst_desig_code= '" + emp.PrstDesigCode + "',prst_post_br= '" + emp.PrstPostBr + "', emp_photo=null,spec_signa=null, " +
            //     " lpr_date= convert(datetime,nullif('" + emp.LprDate + "',''),103),pass_no= '" + emp.PassNo + "',driv_lic_no= '" + emp.DrivLicNo + "',national_id= '" + emp.NationalId + "',personeel_file_no= '" + emp.PersoneelFileNo + "',tin_no= '" + emp.TinNo + "',CountryID='" + emp.County + "' where emp_no='" + emp.EmpNo + "'";
            //    }

            //    SqlParameter img = new SqlParameter();
            //    img.SqlDbType = SqlDbType.VarBinary;
            //    img.ParameterName = "img";
            //    img.Value = emp.EmpPhoto;
            //    SqlParameter sig = new SqlParameter();
            //    sig.SqlDbType = SqlDbType.VarBinary;
            //    sig.ParameterName = "sig";
            //    sig.Value = emp.SpecSigna;
            //    using (SqlCommand cmnd = new SqlCommand(query, sqlCon))
            //    {
            //        cmnd.Parameters.Add(img);
            //        cmnd.Parameters.Add(sig);
            //        if (emp.EmpPhoto == null)
            //        {
            //            cmnd.Parameters.Remove(img);
            //        }
            //        if (emp.SpecSigna == null)
            //        {
            //            cmnd.Parameters.Remove(sig);
            //        }
            //        sqlCon.Open();
            //        cmnd.ExecuteNonQuery();
            //    }


                //*********** Auto Coa generate off **********//
                //string CoaQuery = @"UPDATE [GL_SEG_COA] SET [SEG_COA_DESC] ='" + emp.FName + "'  WHERE [SEG_COA_CODE]='" + emp.GlCoaCode + "'";
                //DataManager.ExecuteNonQuery(connectionString, CoaQuery);
                //string glCoaQuery = @"UPDATE [GL_COA] SET [COA_DESC] ='SDL," + emp.descript + "' where [GL_COA_CODE]='1-" + emp.GlCoaCode + "'";
                //DataManager.ExecuteNonQuery(connectionString, glCoaQuery);

            //}
        }
        public static void DeleteEmp(Emp emp)
        {
            String connectionString = DataManager.OraConnString();
            string query = " delete from pmis_personnel where emp_no='" + emp.EmpNo + "'";
            DataManager.ExecuteNonQuery(connectionString, query);


            //*********** Auto Coa generate off **********//
            //string Query1 = @"DELETE FROM [GL_SEG_COA] WHERE [SEG_COA_CODE]='" + emp.GlCoaCode + "'";
            //DataManager.ExecuteNonQuery(connectionString, Query1);

            //string Query2 = @"delete from [GL_COA] where [COA_NATURAL_CODE]='" + emp.GlCoaCode + "'";
            //DataManager.ExecuteNonQuery(connectionString, Query2);
        }
        public static Emp getEmp(string empno)
        {
            String connectionString = DataManager.OraConnString();
            string query = "select EMP_NO, PF_NO, F_NAME, M_NAME, L_NAME, FH_NAME, MH_NAME, HIGH_EDU, PER_LOC, PER_DIST_CODE, PER_THANA_CODE, MAIL_LOC, " +
            " MAIL_DIST_CODE, MAIL_THANA_CODE, ZIP_AREA_CODE, E_MAIL, OFF_PH_NO, RES_PH_NO, convert(varchar,EMP_BIRTH_DT,103) emp_birth_dt, " +
            " PLACE_OF_BIRTH, NATIONALITY, SEX, BLOOD_GROUP, RELIGION_CODE, MARITAL_STATUS_CODE, SPOUSE_NAME, NO_CHILD, EMP_TYPE, " +
            " CONCT_PERSON, SPEC_SIGNA, convert(varchar,JOIN_DATE,103) join_date, APP_LETT_NO, PROV_PERIOD, " +
            " convert(varchar,CONFIRM_DATE,103) confirm_date , JOIN_DESIG_CODE, DISC_CODE, convert(varchar,LPR_DATE,103)lpr_date, " +
            " ACC_NO, BANK_ACC_NO, PRST_DESIG_CODE, convert(varchar,LAST_PROM_DATE,103) last_prom_date, PAY_SCALE, IMME_PREV_BR, " +
            " PRST_POST_BR, convert(varchar,PRST_POST_DT,103)prst_post_dt, convert(varchar,LAST_INCR_DT,103) last_incr_dt, SENR_SL_NO, " +
            " EMP_STATUS, SUSP_SALARY,EMP_PHOTO, BASIC_AMT, EMP_INSURED_AMT, convert(varchar,EMP_INSURED_DT,103)emp_insured_dt, " +
            " SPOUSE_INS_AMT, convert(varchar,SPOUSE_INS_DT,103) spouse_ins_dt, LOAN_AMT, TI_NO, MOBILE, EMP_CAT, JOB_STATUS, " +
            " PASS_NO, DRIV_LIC_NO, PHOTO_PATH, SPOUSE_PROFESSION, SPOUSE_QUALIFI, TIN_NO,PERSONEEL_FILE_NO, PER_POST_OFFICE, " +
            " MAIL_POST_OFFICE, MAIL_ZIP_CODE, NATIONAL_ID, PER_PHONE_NO,ENTRY_USER, convert(varchar,ENTRY_DATE,103)entry_date, " +
            " UPDATE_USER, convert(varchar,UPDATE_DATE,103) update_date, MAIL_POST_CODE, PAY_SCALE_ID,Convert(nvarchar,RetirmentDate,103) as RetirmentDate,PenssionAmount, " +
            " AUTHO_USER, convert(varchar,AUTHO_DATE,103) autho_date, AUTHO_STATUS,personeel_file_no,tin_no,GlCoaCode,CountryID,ISNULL(BasicSalary,0) AS BasicSalary,BANK_NO,BranchID from pmis_personnel where ID='" + empno + "'";
            DataTable dt = DataManager.ExecuteQuery(connectionString, query, "Employee");
            if (dt.Rows.Count == 0)
            {
                return null;
            }
            return new Emp(dt.Rows[0]);
        }
        public static DataTable getEmps(string empno, string name, string dob)
        {
            String connectionString = DataManager.OraConnString();
            string query = "select emp_no,dbo.initcap(rtrim(ltrim(rtrim(f_name))+' '+ ltrim(rtrim(m_name))+' '+ltrim(rtrim(l_name)))) name, "+
                " convert(varchar,emp_birth_dt,103) dob, "+
                " (select top 1 BranchName from BranchInfo where BranchKey=a.BranchKey) BranchName, "+
                " (select top 1 desig_name from pmis_desig_code where desig_code=a.prst_desig_code) desig "+
                " from pmis_personnel a where emp_no like '%" + empno + "%' and lower(rtrim(ltrim(rtrim(f_name))+' '+ " +
                " ltrim(rtrim(m_name))+' '+ltrim(rtrim(l_name)))) like '%" + name.ToLower() + "%' and "+
                " convert(datetime,nullif(emp_birth_dt,''),103) between convert(datetime,coalesce(nullif('" + dob + "',''),emp_birth_dt),103)-30 and convert(datetime,coalesce(nullif('" + dob + "',''),emp_birth_dt),103)+30 ";
            DataTable dt = DataManager.ExecuteQuery(connectionString, query, "Employee");
            return dt;
        }

        public static DataTable getEmpsByBranch(string branch)
        {
            String connectionString = DataManager.OraConnString();
            string query = "select emp_no,dbo.initcap(rtrim(ltrim(rtrim(f_name))+' '+ ltrim(rtrim(m_name))+' '+ltrim(rtrim(l_name)))) name, " +
                " convert(varchar,emp_birth_dt,103) dob, " +
                " (select top 1 desig_name from pmis_desig_code where desig_code=a.prst_desig_code) desig " +
                " from pmis_personnel a where branchKey='" + branch + "' ";
            DataTable dt = DataManager.ExecuteQuery(connectionString, query, "Employee");
            return dt;
        }

        public static DataTable getEmpForPayroll(string empno, string name)
        {
            String connectionString = DataManager.OraConnString();
            string query = "select emp_no,dbo.initcap(rtrim(ltrim(rtrim(f_name))+' '+ ltrim(rtrim(m_name))+' '+ltrim(rtrim(l_name)))) name, " +
                " convert(varchar,emp_birth_dt,103) dob, " +
                " (select BranchName from BranchInfo where BranchKey=a.BranchKey and rownum=1) branch, " +
                " (select desig_name from pmis_desig_code where desig_code=a.prst_desig_code and rownum=1) desig " +
                " from pmis_personnel a where emp_no like '%" + empno + "%' and lower(rtrim(ltrim(rtrim(f_name))+' '+ " +
                " ltrim(rtrim(m_name))+' '+ltrim(rtrim(l_name)))) like '%" + name.ToLower() + "%' "+
                " and emp_no not in (select employee_id from pay_employee)";
            DataTable dt = DataManager.ExecuteQuery(connectionString, query, "Employee");
            return dt;
        }
        public static DataTable getEmployees()
        {
            String connectionString = DataManager.OraConnString();
            string query = "select emp_no,dbo.initcap(rtrim(ltrim(rtrim(f_name))+' '+ ltrim(rtrim(m_name))+' '+ltrim(rtrim(l_name)))) name, " +
                " convert(varchar,emp_birth_dt,103) dob, " +
                " (select BranchName from BranchInfo where BranchKey=a.BranchKey and rownum=1) branch, " +
                " (select desig_name from pmis_desig_code where desig_code=a.prst_desig_code and rownum=1) desig " +
                " from pmis_personnel a where emp_no in (select top 100 emp_no from (select emp_no from pmis_personnel order by dbms_random.value) as t1) ";
            DataTable dt = DataManager.ExecuteQuery(connectionString, query, "Employee");
            return dt;
        }
        public static DataTable getEmployeeRpt(string criteria)
        {
            String connectionString = DataManager.OraConnString();
            string query = "select emp_no,dbo.initcap(rtrim(ltrim(rtrim(f_name))+' '+ ltrim(rtrim(m_name))+' '+ltrim(rtrim(l_name)))) name, " +
                " convert(varchar,emp_birth_dt,103) dob, " +
                " (select top 1 BranchName from BranchInfo where BranchKey=a.BranchKey ) branch, " +
                " (select top 1 desig_name from pmis_desig_code where desig_code=a.prst_desig_code ) desig " +
                " from pmis_personnel a ";
            if (criteria != "")
            {
                if (criteria.Trim().StartsWith("and"))
                {
                    query = query + " where " + criteria.Trim().Remove(0, 3);
                }
                else
                {
                    query = query + " where " + criteria;
                }
            }
            DataTable dt = DataManager.ExecuteQuery(connectionString, query, "Employee");
            return dt;
        }
        public static string ConvertBytesToString(byte[] bytes)
        {
            string output = String.Empty;
            MemoryStream stream = new MemoryStream(bytes);
            stream.Position = 0;
            using (StreamReader reader = new StreamReader(stream))
            {
                output = reader.ReadToEnd();
            }
            return output;
        }
        public static byte[] ConvertStringToBytes(string input)
        {
            MemoryStream stream = new MemoryStream();
            using (StreamWriter writer = new StreamWriter(stream))
            {
                writer.Write(input);
                writer.Flush();
            }
            return stream.ToArray();
        }
        public static byte[] ConvertImageToByteArray(System.Drawing.Image imageToConvert,
                                       System.Drawing.Imaging.ImageFormat formatOfImage)
        {
            byte[] Ret;
            try
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    imageToConvert.Save(ms, formatOfImage);
                    Ret = ms.ToArray();
                }
            }
            catch (Exception) { throw; }
            return Ret;
        }
        public static System.Drawing.Bitmap BytesToBitmap(byte[] byteArray)
        {
            using (MemoryStream ms = new MemoryStream(byteArray))
            {
                System.Drawing.Bitmap img = (System.Drawing.Bitmap)System.Drawing.Image.FromStream(ms);
                return img;
            }
        }
        public static System.Drawing.Image byteArrayToImage(byte[] byteArrayIn)
        {
            MemoryStream ms;
            System.Drawing.Image returnImage;
            FileStream fs = new FileStream(HttpContext.Current.Server.MapPath("~/img/noimage.jpg"), FileMode.Open, FileAccess.Read);
            BinaryReader br = new BinaryReader(fs);
            byte[] bt = br.ReadBytes((int)fs.Length);
            if (byteArrayIn.Length == 0)
            {
                byteArrayIn = bt;
            }
            ms = new MemoryStream(byteArrayIn);
            returnImage = System.Drawing.Image.FromStream(ms);
            return returnImage;
        }
        public static DataTable getEmpPhoto(string empno)
        {
            String connectionString = DataManager.OraConnString();
            string query = "select emp_photo from pmis_personnel where emp_no= " + empno + "";
            DataTable dt = DataManager.ExecuteQuery(connectionString, query, "Employee");
            return dt;
        }
        public static DataTable getEmpSign(string empno)
        {
            String connectionString = DataManager.OraConnString();
            string query = "select spec_signa from pmis_personnel where emp_no= " + empno + "";
            DataTable dt = DataManager.ExecuteQuery(connectionString, query, "Employee");
            return dt;
        }
        public enum ResizeOptions
        {
            // Use fixed width & height without keeping the proportions
            ExactWidthAndHeight,

            // Use maximum width (as defined) and keeping the proportions
            MaxWidth,

            // Use maximum height (as defined) and keeping the proportions
            MaxHeight,

            // Use maximum width or height (the biggest) and keeping the proportions
            MaxWidthAndHeight
        }
        public static System.Drawing.Bitmap DoResize(System.Drawing.Bitmap originalImg, int widthInPixels, int heightInPixels)
        {
            System.Drawing.Bitmap bitmap;
            try
            {
                bitmap = new System.Drawing.Bitmap(widthInPixels, heightInPixels);
                using (System.Drawing.Graphics graphic = System.Drawing.Graphics.FromImage(bitmap))
                {
                    // Quality properties
                    graphic.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                    graphic.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                    graphic.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
                    graphic.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;

                    graphic.DrawImage(originalImg, 0, 0, widthInPixels, heightInPixels);
                    return bitmap;
                }
            }
            finally
            {
                if (originalImg != null)
                {
                    originalImg.Dispose();
                }
            }
        }
        public static System.Drawing.Bitmap ResizeImage(System.Drawing.Bitmap image, int width, int height, ResizeOptions resizeOptions)
        {
            float f_width;
            float f_height;
            float dim;
            switch (resizeOptions)
            {
                case ResizeOptions.ExactWidthAndHeight:
                    return DoResize(image, width, height);

                case ResizeOptions.MaxHeight:
                    f_width = image.Width;
                    f_height = image.Height;

                    if (f_height <= height)
                        return DoResize(image, (int)f_width, (int)f_height);

                    dim = f_width / f_height;
                    width = (int)((float)(height) * dim);
                    return DoResize(image, width, height);

                case ResizeOptions.MaxWidth:
                    f_width = image.Width;
                    f_height = image.Height;

                    if (f_width <= width)
                        return DoResize(image, (int)f_width, (int)f_height);

                    dim = f_width / f_height;
                    height = (int)((float)(width) / dim);
                    return DoResize(image, width, height);

                case ResizeOptions.MaxWidthAndHeight:
                    int tmpHeight = height;
                    int tmpWidth = width;
                    f_width = image.Width;
                    f_height = image.Height;

                    if (f_width <= width && f_height <= height)
                        return DoResize(image, (int)f_width, (int)f_height);

                    dim = f_width / f_height;

                    // Check if the width is ok
                    if (f_width < width)
                        width = (int)f_width;
                    height = (int)((float)(width) / dim);
                    // The width is too width
                    if (height > tmpHeight)
                    {
                        if (f_height < tmpHeight)
                            height = (int)f_height;
                        else
                            height = tmpHeight;
                        width = (int)((float)(height) * dim);
                    }
                    return DoResize(image, width, height);
                default:
                    return image;
            }
        }
        public static string getEmployeeName(string cc)
        {
            String ConnectionString = DataManager.OraConnString();
            SqlConnection myConnection = new SqlConnection(ConnectionString);
            string Query = "select f_name+' '+m_name+' '+l_name from pmis_personnel where emp_no='" + cc + "'";
            myConnection.Open();
            SqlCommand myCommand = new SqlCommand(Query, myConnection);
            object maxValue = myCommand.ExecuteScalar();
            myConnection.Close();
            return maxValue.ToString();
        }
        public static DataTable getEmpRpt(string empno)
        {
            String connectionString = DataManager.OraConnString();
            //string query = "SELECT a.Emp_No,dbo.initcap(a.f_Name+' '+a.m_name+' '+a.l_name) name,case when a.sex='M' then 'Male' when a.sex='F' then 'Female' end sex, " +
            //" convert(varchar,emp_birth_dt,103)dob,dbo.initcap(a.FH_Name)fh_name,dbo.initcap(a.MH_NAME)mh_name, " +
            //" a.BLOOD_GROUP, case when a.MARITAL_STATUS_CODE='S' then 'Single' else 'Married' end marital_status, " +
            //" dbo.initcap(a.PER_LOC+', '+(select thana_name from pmis_thana_code where thana_code=a.per_thana_code )+', '+ " +
            //" (select district_name from pmis_district_code where district_code=a.per_dist_code )) per_address, " +
            //" dbo.initcap(a.mail_loc+', '+(select thana_name from pmis_thana_code where thana_code=a.mail_thana_code ) +', '+ " +
            //" (select district_name from pmis_district_code where district_code=a.mail_dist_code )) mail_address, " +
            //" (select dbo.initcap(BranchName) from BranchInfo where BranchKey=a.BranchKey) branch, " +
            //" (select dbo.initcap(desig_name) from pmis_desig_code where desig_code=a.prst_desig_code)desig " +
            //" from pmis_personnel a where emp_no='" + empno + "'";

            string query = @"SELECT t1.Emp_No,dbo.initcap(t1.f_Name+' '+t1.m_name+' '+t1.l_name) name,case when t1.sex='M' then 'Male' when t1.sex='F' then 'Female' end sex,  convert(varchar,emp_birth_dt,103)dob,dbo.initcap(t1.FH_Name)fh_name,dbo.initcap(t1.MH_NAME)mh_name,  t1.BLOOD_GROUP, case when t1.MARITAL_STATUS_CODE='S' then 'Single' else 'Married' end marital_status,dbo.initcap(IsNull(t1.PER_LOC,'')+', '+IsNull(t2.THANA_NAME,'')+', '+ IsNull(t3.DISTRICT_NAME,'')) per_address
,dbo.initcap(IsNULL(t1.mail_loc,'')+', '+IsNull(t4.THANA_NAME,'') +', '+  IsNull(t5.DISTRICT_NAME,'')) mail_address
,dbo.initcap(t6.BranchName) branch,dbo.initcap(t7.desig_name)desig,t1.E_MAIL,t1.RES_PH_NO,t1.MOBILE,(case when t1.RELIGION_CODE='1' then 'Islam' when t1.RELIGION_CODE='2' then 'Hindu' when t1.RELIGION_CODE='3' then 'Christian' when t1.RELIGION_CODE='4' then 'Buddhist' when t1.RELIGION_CODE='5' then 'Others' end) as religion,t1.NATIONAL_ID,t1.NATIONALITY
from pmis_personnel t1 
left join pmis_thana_code t2 on t1.PER_THANA_CODE=t2.THANA_CODE
left join pmis_district_code t3 on t1.PER_DIST_CODE=t3.DISTRICT_CODE
left join pmis_thana_code t4 on t1.MAIL_THANA_CODE=t4.THANA_CODE
left join pmis_district_code t5 on t1.MAIL_DIST_CODE=t5.DISTRICT_CODE
left join BranchInfo t6 on t1.BRANCH_CODE=t6.BranchKey
left join pmis_desig_code t7 on t1.JOIN_DESIG_CODE=t7.DESIG_CODE where t1.emp_no='" + empno + "'";
            DataTable dt = DataManager.ExecuteQuery(connectionString, query, "Employee");
            return dt;
        }
        public static DataTable getEmpDetailRpt(string empno)
        {
            string connectionString = DataManager.OraConnString();
            string query = "select emp_no,dbo.initcap(a.f_Name+' '+a.m_name+' '+a.l_name) name, convert(varchar,emp_birth_dt,103) dob,zip_area_code, " +
            " case when religion_code='1' then 'Muslim'  when religion_code='2' then 'Hindu' when religion_code='3' then 'Christian' when religion_code='4' then 'Buddist' when religion_code='5' then 'Others' end religion_code, " +
            " case when marital_status_code='1' then 'Married' when marital_status_code='2' then 'Single' when marital_status_code='3' then 'Divorced' when marital_status_code='4' then 'Separated' end marital_status_code, " +
            " national_id,pass_no,case when a.sex='M' then 'Male' when a.sex='F' then 'Female' end sex,bank_acc_no,pf_no,dbo.initcap(fh_name)fh_name, " +
            " dbo.initcap(mh_name)mh_name,driv_lic_no,tin_no,personeel_file_no,dbo.initcap(spouse_name)spouse_name,mail_loc, " +
            " (select district_name from pmis_district_code where district_code=a.mail_dist_code ) mail_dist_code, " +
            " (select thana_name from pmis_thana_code where thana_code=a.mail_thana_code ) mail_thana_code, " +
            " res_ph_no+' '+mobile contact_no,per_loc,place_of_birth,blood_group, " +
            " (select district_name from pmis_district_code where district_code=a.per_dist_code ) per_dist_code, " +
            " (select thana_name from pmis_thana_code where thana_code=a.per_thana_code ) per_thana_code, " +
            " (select dbo.initcap(BranchName) from BranchInfo where BranchKey=a.BranchKey) BranchKey, " +
            " (select dbo.initcap(desig_name) from pmis_desig_code where desig_code=a.prst_desig_code)desig_code " +
            " from pmis_personnel a where emp_no='" + empno + "'";
            DataTable dt = DataManager.ExecuteQuery(connectionString, query, "Employee");
            return dt;
        }
        public static DataTable getEmpSimpleRpt(string empno)
        {
            string connectionString = DataManager.OraConnString();
            string query = "select emp_no,edu,join_date,convert(varchar,convert(decimal(13,0),datediff(day,cur_div_date,getdate())/365.25)) div_year, "+
            " name,BranchName,cur_div_date,convert(varchar,convert(decimal(13,0),datediff(day,cur_div_date,getdate())/365.25*30)) div_month,dob, " +
            " desig_name,curr_desig_date,convert(varchar,convert(decimal(13,0),datediff(day,cur_div_date,getdate())/365.25*30*30)) div_day from ( " +
            " select emp_no,dbo.initcap(a.f_Name+' '+a.m_name+' '+a.l_name) name,convert(varchar,emp_birth_dt,103)dob, "+
            " (select dbo.initcap(BranchName) from BranchInfo where BranchKey=a.BranchKey) BranchName, "+
            " (select dbo.initcap(desig_name) from pmis_desig_code where desig_code=a.prst_desig_code)desig_name, "+
            " (select  exam_name from pmis_exam_code where exam_code=(select max(convert(numeric,exam_code)) from pmis_education where emp_no=a.emp_no)) edu, "+
            " convert(varchar,join_date,103)join_date,(select convert(varchar,max(joining_date),103) from pmis_promotion where emp_no=a.emp_no)curr_desig_date, "+
            " coalesce((select convert(varchar,min(joining_date),103) from pmis_promotion where joining_date =  "+
            " (select min(joining_date) from pmis_promotion where joining_branch=a.BranchKey and emp_no=a.emp_no)), "+
            " (select max(joining_date) from pmis_promotion where emp_no=a.emp_no)) cur_div_date "+
            " from pmis_personnel a ) as tot where emp_no='" + empno + "'";
            DataTable dt = DataManager.ExecuteQuery(connectionString, query, "Employee");
            return dt;
        }

        public static DataTable GetAllEmployeeInformation()
        {
            string connectionString = DataManager.OraConnString();
            SqlConnection con = new SqlConnection(connectionString);
            try
            {
                con.Open();
                string selectQuery = @"SELECT [PMIS_PERSONNEL].ID,[EMP_NO]      
     ,isnull([F_NAME],'')+' '+isnull([M_NAME],'')+' '+isnull([L_NAME],'') as[Emp_name]
      ,[FH_NAME]
      ,convert(nvarchar,[EMP_BIRTH_DT],103) as[EMP_BIRTH_DT]    
      ,case when [SEX]='M' then 'Male'
      when [SEX]='F' then 'Female'
       else '' end as[sex]        
      ,case when RELIGION_CODE='1' then 'Muslim'
      when RELIGION_CODE='2' then 'Hindu'
      when RELIGION_CODE='5' then 'Christian'
       else '' end as RELIGION_CODE
       ,t2.DESIG_NAME
	   ,t3.COUNTRY_DESC AS[Country]
  FROM [PMIS_PERSONNEL]
  left join PMIS_DESIG_CODE t2 on [PMIS_PERSONNEL].JOIN_DESIG_CODE=t2.DESIG_CODE
  left join COUNTRY_INFO t3 on t3.COUNTRY_CODE=[PMIS_PERSONNEL].CountryID order by EMP_NO,[PMIS_PERSONNEL].id desc";
                SqlDataAdapter da = new SqlDataAdapter(selectQuery,con);
                DataSet ds = new DataSet();
                da.Fill(ds, "PMIS_PERSONNEL");
                DataTable table = ds.Tables["PMIS_PERSONNEL"];
                return table;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                
            }
        }

        public static DataTable GetEmployeeInformationForSpecificEmployee(string empNo)
        {
            string connectionString = DataManager.OraConnString();
            SqlConnection con = new SqlConnection(connectionString);
            try
            {
                con.Open();
                string selectQuery = @"SELECT p.[EMP_NO]       
      ,p.[F_NAME] as[Emp_name],[GlCoaCode]
      ,p.[M_NAME]
      ,p.[L_NAME]
, convert(date,p.EMP_BIRTH_DT,103)  EMP_BIRTH_DT 
,p.JOIN_DESIG_CODE
,p.SEX ,Convert(nvarchar,p.RetirmentDate,103) as RetirmentDate,p.PenssionAmount
,de.DESIG_NAME ,p.BranchID   
  FROM [PMIS_PERSONNEL] p
left join PMIS_DESIG_CODE de on de.DESIG_CODE=p.JOIN_DESIG_CODE
WHERE p.[ID]='" + empNo + "'";
                SqlDataAdapter da = new SqlDataAdapter(selectQuery, con);
                DataSet ds = new DataSet();
                da.Fill(ds, "View_personal_info");
                DataTable table = ds.Tables["View_personal_info"];
                return table;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {

            }
        }

        public static DataTable GetAllEmployeeInformationForReport()
        {
            String connectionString = DataManager.OraConnString();
            string query = @"SELECT [EMP_NO] AS [ID]   
      ,[F_NAME] AS [Name of Employee] 
      ,(SELECT [DESIG_ABB] FROM [PMIS_DESIG_CODE] WHERE [DESIG_CODE]=[DISC_CODE]) AS [Designation Name]
      ,convert(NVARCHAR,JOIN_DATE,103) AS [Joining Date]
      ,[CONFIRM_DATE] AS [Confirmation Date]
      ,[LAST_PROM_DATE] AS [Last Promotion Date]
      ,[LAST_INCR_DT] AS [Last Increment Date]
      ,convert(NVARCHAR,EMP_BIRTH_DT,103) AS [Date Of Birth]      
      ,(CASE WHEN SUBSTRING(CONVERT(VARCHAR,CONVERT(DATETIME,ISNULL([JOIN_DATE],'1900-01-01')),111),0,5)='1900' THEN ' ' ELSE (((CONVERT(VARCHAR,(CONVERT(INT,SUBSTRING(CONVERT(VARCHAR,GETDATE(),111),0,5))-CONVERT(INT,SUBSTRING(CONVERT(VARCHAR,CONVERT(DATETIME,ISNULL([JOIN_DATE],'1900-01-01')),111),0,5))))+'Y-')+ CONVERT(VARCHAR,(CASE WHEN CONVERT(INT,SUBSTRING(CONVERT(VARCHAR,CONVERT(DATETIME,ISNULL([JOIN_DATE],'1900-01-01')),111),6,2))>CONVERT(INT,SUBSTRING(CONVERT(VARCHAR,GETDATE(),111),6,2)) THEN (CONVERT(INT,SUBSTRING(CONVERT(VARCHAR,GETDATE(),111),6,2))+12)-CONVERT(INT,SUBSTRING(CONVERT(VARCHAR,CONVERT(DATETIME,ISNULL([JOIN_DATE],'1900-01-01')),111),6,2)) ELSE CONVERT(INT,SUBSTRING(CONVERT(VARCHAR,GETDATE(),111),6,2))-CONVERT(INT,SUBSTRING(CONVERT(VARCHAR,CONVERT(DATETIME,ISNULL([JOIN_DATE],'1900-01-01')),111),6,2)) END)))+'M-'+CONVERT(VARCHAR,(CASE WHEN CONVERT(INT ,SUBSTRING(CONVERT(VARCHAR,CONVERT(DATETIME,ISNULL([JOIN_DATE],'1900-01-01')),111),9,2))> CONVERT(INT,SUBSTRING(CONVERT(VARCHAR,GETDATE(),111),9,2)) THEN (CONVERT(INT,SUBSTRING(CONVERT(VARCHAR,GETDATE(),111),9,2))+30)-CONVERT(INT ,SUBSTRING(CONVERT(VARCHAR,CONVERT(DATETIME,ISNULL([JOIN_DATE],'1900-01-01')),111),9,2)) ELSE CONVERT(INT,SUBSTRING(CONVERT(VARCHAR,GETDATE(),111),9,2)) - CONVERT(INT ,SUBSTRING(CONVERT(VARCHAR,CONVERT(DATETIME,ISNULL([JOIN_DATE],'1900-01-01')),111),9,2)) END))+'D') END) AS [Service Length]
      ,[EMP_TYPE] AS [Emp Type]        
  FROM [PMIS_PERSONNEL]";
            DataTable dt = DataManager.ExecuteQuery(connectionString, query, "PMIS_PERSONNEL");
            return dt;
        }

        public static DataTable GetAllEmployeeInformationForSpecificBrachReport(string BRANCHkEY)
        {
            String connectionString = DataManager.OraConnString();
            string query = @"SELECT [EMP_NO] AS [ID]   
      ,[F_NAME] AS [Name of Employee] 
      ,(SELECT [DESIG_ABB] FROM [PMIS_DESIG_CODE] WHERE [DESIG_CODE]=[DISC_CODE]) AS [Designation Name]
      ,convert(NVARCHAR,JOIN_DATE,103) AS [Joining Date]
      ,[CONFIRM_DATE] AS [Confirmation Date]
      ,[LAST_PROM_DATE] AS [Last Promotion Date]
      ,[LAST_INCR_DT] AS [Last Increment Date]
      ,convert(NVARCHAR,EMP_BIRTH_DT,103) AS [Date Of Birth]      
      ,(CASE WHEN SUBSTRING(CONVERT(VARCHAR,CONVERT(DATETIME,ISNULL([JOIN_DATE],'1900-01-01')),111),0,5)='1900' THEN ' ' ELSE (((CONVERT(VARCHAR,(CONVERT(INT,SUBSTRING(CONVERT(VARCHAR,GETDATE(),111),0,5))-CONVERT(INT,SUBSTRING(CONVERT(VARCHAR,CONVERT(DATETIME,ISNULL([JOIN_DATE],'1900-01-01')),111),0,5))))+'Y-')+ CONVERT(VARCHAR,(CASE WHEN CONVERT(INT,SUBSTRING(CONVERT(VARCHAR,CONVERT(DATETIME,ISNULL([JOIN_DATE],'1900-01-01')),111),6,2))>CONVERT(INT,SUBSTRING(CONVERT(VARCHAR,GETDATE(),111),6,2)) THEN (CONVERT(INT,SUBSTRING(CONVERT(VARCHAR,GETDATE(),111),6,2))+12)-CONVERT(INT,SUBSTRING(CONVERT(VARCHAR,CONVERT(DATETIME,ISNULL([JOIN_DATE],'1900-01-01')),111),6,2)) ELSE CONVERT(INT,SUBSTRING(CONVERT(VARCHAR,GETDATE(),111),6,2))-CONVERT(INT,SUBSTRING(CONVERT(VARCHAR,CONVERT(DATETIME,ISNULL([JOIN_DATE],'1900-01-01')),111),6,2)) END)))+'M-'+CONVERT(VARCHAR,(CASE WHEN CONVERT(INT ,SUBSTRING(CONVERT(VARCHAR,CONVERT(DATETIME,ISNULL([JOIN_DATE],'1900-01-01')),111),9,2))> CONVERT(INT,SUBSTRING(CONVERT(VARCHAR,GETDATE(),111),9,2)) THEN (CONVERT(INT,SUBSTRING(CONVERT(VARCHAR,GETDATE(),111),9,2))+30)-CONVERT(INT ,SUBSTRING(CONVERT(VARCHAR,CONVERT(DATETIME,ISNULL([JOIN_DATE],'1900-01-01')),111),9,2)) ELSE CONVERT(INT,SUBSTRING(CONVERT(VARCHAR,GETDATE(),111),9,2)) - CONVERT(INT ,SUBSTRING(CONVERT(VARCHAR,CONVERT(DATETIME,ISNULL([JOIN_DATE],'1900-01-01')),111),9,2)) END))+'D') END) AS [Service Length]
      ,[EMP_TYPE] AS [Emp Type]        
  FROM [PMIS_PERSONNEL] WHERE [BranchKey]='" + BRANCHkEY + "' ";
            DataTable dt = DataManager.ExecuteQuery(connectionString, query, "PMIS_PERSONNEL");
            return dt;
        }

        public static DataTable GetAllEmployeeInformationForSpecificDesignationReport(string designation)
        {
            String connectionString = DataManager.OraConnString();
            string query = @"SELECT [EMP_NO] AS [ID]   
      ,[F_NAME] AS [Name of Employee] 
      ,(SELECT [DESIG_ABB] FROM [PMIS_DESIG_CODE] WHERE [DESIG_CODE]=[DISC_CODE]) AS [Designation Name]
      ,convert(NVARCHAR,JOIN_DATE,103) AS [Joining Date]
      ,[CONFIRM_DATE] AS [Confirmation Date]
      ,[LAST_PROM_DATE] AS [Last Promotion Date]
      ,[LAST_INCR_DT] AS [Last Increment Date]
      ,convert(NVARCHAR,EMP_BIRTH_DT,103) AS [Date Of Birth]      
      ,(CASE WHEN SUBSTRING(CONVERT(VARCHAR,CONVERT(DATETIME,ISNULL([JOIN_DATE],'1900-01-01')),111),0,5)='1900' THEN ' ' ELSE (((CONVERT(VARCHAR,(CONVERT(INT,SUBSTRING(CONVERT(VARCHAR,GETDATE(),111),0,5))-CONVERT(INT,SUBSTRING(CONVERT(VARCHAR,CONVERT(DATETIME,ISNULL([JOIN_DATE],'1900-01-01')),111),0,5))))+'Y-')+ CONVERT(VARCHAR,(CASE WHEN CONVERT(INT,SUBSTRING(CONVERT(VARCHAR,CONVERT(DATETIME,ISNULL([JOIN_DATE],'1900-01-01')),111),6,2))>CONVERT(INT,SUBSTRING(CONVERT(VARCHAR,GETDATE(),111),6,2)) THEN (CONVERT(INT,SUBSTRING(CONVERT(VARCHAR,GETDATE(),111),6,2))+12)-CONVERT(INT,SUBSTRING(CONVERT(VARCHAR,CONVERT(DATETIME,ISNULL([JOIN_DATE],'1900-01-01')),111),6,2)) ELSE CONVERT(INT,SUBSTRING(CONVERT(VARCHAR,GETDATE(),111),6,2))-CONVERT(INT,SUBSTRING(CONVERT(VARCHAR,CONVERT(DATETIME,ISNULL([JOIN_DATE],'1900-01-01')),111),6,2)) END)))+'M-'+CONVERT(VARCHAR,(CASE WHEN CONVERT(INT ,SUBSTRING(CONVERT(VARCHAR,CONVERT(DATETIME,ISNULL([JOIN_DATE],'1900-01-01')),111),9,2))> CONVERT(INT,SUBSTRING(CONVERT(VARCHAR,GETDATE(),111),9,2)) THEN (CONVERT(INT,SUBSTRING(CONVERT(VARCHAR,GETDATE(),111),9,2))+30)-CONVERT(INT ,SUBSTRING(CONVERT(VARCHAR,CONVERT(DATETIME,ISNULL([JOIN_DATE],'1900-01-01')),111),9,2)) ELSE CONVERT(INT,SUBSTRING(CONVERT(VARCHAR,GETDATE(),111),9,2)) - CONVERT(INT ,SUBSTRING(CONVERT(VARCHAR,CONVERT(DATETIME,ISNULL([JOIN_DATE],'1900-01-01')),111),9,2)) END))+'D') END) AS [Service Length]
      ,[EMP_TYPE] AS [Emp Type]        
  FROM [PMIS_PERSONNEL] WHERE [DISC_CODE]='" + designation + "' ";
            DataTable dt = DataManager.ExecuteQuery(connectionString, query, "PMIS_PERSONNEL");
            return dt;
        }

        public static DataTable GetAllEmployeeInformationForSpecificEmployeeTypeReport(string empType)
        {
            String connectionString = DataManager.OraConnString();
            string query = @"SELECT [EMP_NO] AS [ID]   
      ,[F_NAME] AS [Name of Employee] 
      ,(SELECT [DESIG_ABB] FROM [PMIS_DESIG_CODE] WHERE [DESIG_CODE]=[DISC_CODE]) AS [Designation Name]
      ,convert(NVARCHAR,JOIN_DATE,103) AS [Joining Date]
      ,[CONFIRM_DATE] AS [Confirmation Date]
      ,[LAST_PROM_DATE] AS [Last Promotion Date]
      ,[LAST_INCR_DT] AS [Last Increment Date]
      ,convert(NVARCHAR,EMP_BIRTH_DT,103) AS [Date Of Birth]      
      ,(CASE WHEN SUBSTRING(CONVERT(VARCHAR,CONVERT(DATETIME,ISNULL([JOIN_DATE],'1900-01-01')),111),0,5)='1900' THEN ' ' ELSE (((CONVERT(VARCHAR,(CONVERT(INT,SUBSTRING(CONVERT(VARCHAR,GETDATE(),111),0,5))-CONVERT(INT,SUBSTRING(CONVERT(VARCHAR,CONVERT(DATETIME,ISNULL([JOIN_DATE],'1900-01-01')),111),0,5))))+'Y-')+ CONVERT(VARCHAR,(CASE WHEN CONVERT(INT,SUBSTRING(CONVERT(VARCHAR,CONVERT(DATETIME,ISNULL([JOIN_DATE],'1900-01-01')),111),6,2))>CONVERT(INT,SUBSTRING(CONVERT(VARCHAR,GETDATE(),111),6,2)) THEN (CONVERT(INT,SUBSTRING(CONVERT(VARCHAR,GETDATE(),111),6,2))+12)-CONVERT(INT,SUBSTRING(CONVERT(VARCHAR,CONVERT(DATETIME,ISNULL([JOIN_DATE],'1900-01-01')),111),6,2)) ELSE CONVERT(INT,SUBSTRING(CONVERT(VARCHAR,GETDATE(),111),6,2))-CONVERT(INT,SUBSTRING(CONVERT(VARCHAR,CONVERT(DATETIME,ISNULL([JOIN_DATE],'1900-01-01')),111),6,2)) END)))+'M-'+CONVERT(VARCHAR,(CASE WHEN CONVERT(INT ,SUBSTRING(CONVERT(VARCHAR,CONVERT(DATETIME,ISNULL([JOIN_DATE],'1900-01-01')),111),9,2))> CONVERT(INT,SUBSTRING(CONVERT(VARCHAR,GETDATE(),111),9,2)) THEN (CONVERT(INT,SUBSTRING(CONVERT(VARCHAR,GETDATE(),111),9,2))+30)-CONVERT(INT ,SUBSTRING(CONVERT(VARCHAR,CONVERT(DATETIME,ISNULL([JOIN_DATE],'1900-01-01')),111),9,2)) ELSE CONVERT(INT,SUBSTRING(CONVERT(VARCHAR,GETDATE(),111),9,2)) - CONVERT(INT ,SUBSTRING(CONVERT(VARCHAR,CONVERT(DATETIME,ISNULL([JOIN_DATE],'1900-01-01')),111),9,2)) END))+'D') END) AS [Service Length]
      ,[EMP_TYPE] AS [Emp Type]        
  FROM [PMIS_PERSONNEL] WHERE [EMP_TYPE]='" + empType + "' ";
            DataTable dt = DataManager.ExecuteQuery(connectionString, query, "PMIS_PERSONNEL");
            return dt;
        }

        public static DataTable GetAllEmployeeInformationForSpecificJoiningDateReport(string startDate, string endDate)
        {
            String connectionString = DataManager.OraConnString();
            string query = @"SELECT [EMP_NO] AS [ID]   
      ,[F_NAME] AS [Name of Employee] 
      ,(SELECT [DESIG_ABB] FROM [PMIS_DESIG_CODE] WHERE [DESIG_CODE]=[DISC_CODE]) AS [Designation Name]
      ,convert(NVARCHAR,JOIN_DATE,103) AS [Joining Date]
      ,[CONFIRM_DATE] AS [Confirmation Date]
      ,[LAST_PROM_DATE] AS [Last Promotion Date]
      ,[LAST_INCR_DT] AS [Last Increment Date]
      ,convert(NVARCHAR,EMP_BIRTH_DT,103) AS [Date Of Birth]      
      ,(CASE WHEN SUBSTRING(CONVERT(VARCHAR,CONVERT(DATETIME,ISNULL([JOIN_DATE],'1900-01-01')),111),0,5)='1900' THEN ' ' ELSE (((CONVERT(VARCHAR,(CONVERT(INT,SUBSTRING(CONVERT(VARCHAR,GETDATE(),111),0,5))-CONVERT(INT,SUBSTRING(CONVERT(VARCHAR,CONVERT(DATETIME,ISNULL([JOIN_DATE],'1900-01-01')),111),0,5))))+'Y-')+ CONVERT(VARCHAR,(CASE WHEN CONVERT(INT,SUBSTRING(CONVERT(VARCHAR,CONVERT(DATETIME,ISNULL([JOIN_DATE],'1900-01-01')),111),6,2))>CONVERT(INT,SUBSTRING(CONVERT(VARCHAR,GETDATE(),111),6,2)) THEN (CONVERT(INT,SUBSTRING(CONVERT(VARCHAR,GETDATE(),111),6,2))+12)-CONVERT(INT,SUBSTRING(CONVERT(VARCHAR,CONVERT(DATETIME,ISNULL([JOIN_DATE],'1900-01-01')),111),6,2)) ELSE CONVERT(INT,SUBSTRING(CONVERT(VARCHAR,GETDATE(),111),6,2))-CONVERT(INT,SUBSTRING(CONVERT(VARCHAR,CONVERT(DATETIME,ISNULL([JOIN_DATE],'1900-01-01')),111),6,2)) END)))+'M-'+CONVERT(VARCHAR,(CASE WHEN CONVERT(INT ,SUBSTRING(CONVERT(VARCHAR,CONVERT(DATETIME,ISNULL([JOIN_DATE],'1900-01-01')),111),9,2))> CONVERT(INT,SUBSTRING(CONVERT(VARCHAR,GETDATE(),111),9,2)) THEN (CONVERT(INT,SUBSTRING(CONVERT(VARCHAR,GETDATE(),111),9,2))+30)-CONVERT(INT ,SUBSTRING(CONVERT(VARCHAR,CONVERT(DATETIME,ISNULL([JOIN_DATE],'1900-01-01')),111),9,2)) ELSE CONVERT(INT,SUBSTRING(CONVERT(VARCHAR,GETDATE(),111),9,2)) - CONVERT(INT ,SUBSTRING(CONVERT(VARCHAR,CONVERT(DATETIME,ISNULL([JOIN_DATE],'1900-01-01')),111),9,2)) END))+'D') END) AS [Service Length]
      ,[EMP_TYPE] AS [Emp Type]        
  FROM [PMIS_PERSONNEL] WHERE CONVERT(DATETIME,[JOIN_DATE])>='" + Convert.ToDateTime(startDate) + "' AND CONVERT(DATETIME,[JOIN_DATE])<='" + Convert.ToDateTime(endDate) + "' ";
            DataTable dt = DataManager.ExecuteQuery(connectionString, query, "PMIS_PERSONNEL");
            return dt;
        }

        public static DataTable GetAllEmployeeInformationForOthers(string branchKey, string desgnationKey, string employeeType, string startDate, string endDate)
        {
            String connectionString = DataManager.OraConnString();
            string query = @"SELECT [EMP_NO] AS [ID]   
      ,[F_NAME] AS [Name of Employee] 
      ,(SELECT [DESIG_ABB] FROM [PMIS_DESIG_CODE] WHERE [DESIG_CODE]=[DISC_CODE]) AS [Designation Name]
      ,convert(NVARCHAR,JOIN_DATE,103) AS [Joining Date]
      ,[CONFIRM_DATE] AS [Confirmation Date]
      ,[LAST_PROM_DATE] AS [Last Promotion Date]
      ,[LAST_INCR_DT] AS [Last Increment Date]
      ,convert(NVARCHAR,EMP_BIRTH_DT,103) AS [Date Of Birth]      
      ,(CASE WHEN SUBSTRING(CONVERT(VARCHAR,CONVERT(DATETIME,ISNULL([JOIN_DATE],'1900-01-01')),111),0,5)='1900' THEN ' ' ELSE (((CONVERT(VARCHAR,(CONVERT(INT,SUBSTRING(CONVERT(VARCHAR,GETDATE(),111),0,5))-CONVERT(INT,SUBSTRING(CONVERT(VARCHAR,CONVERT(DATETIME,ISNULL([JOIN_DATE],'1900-01-01')),111),0,5))))+'Y-')+ CONVERT(VARCHAR,(CASE WHEN CONVERT(INT,SUBSTRING(CONVERT(VARCHAR,CONVERT(DATETIME,ISNULL([JOIN_DATE],'1900-01-01')),111),6,2))>CONVERT(INT,SUBSTRING(CONVERT(VARCHAR,GETDATE(),111),6,2)) THEN (CONVERT(INT,SUBSTRING(CONVERT(VARCHAR,GETDATE(),111),6,2))+12)-CONVERT(INT,SUBSTRING(CONVERT(VARCHAR,CONVERT(DATETIME,ISNULL([JOIN_DATE],'1900-01-01')),111),6,2)) ELSE CONVERT(INT,SUBSTRING(CONVERT(VARCHAR,GETDATE(),111),6,2))-CONVERT(INT,SUBSTRING(CONVERT(VARCHAR,CONVERT(DATETIME,ISNULL([JOIN_DATE],'1900-01-01')),111),6,2)) END)))+'M-'+CONVERT(VARCHAR,(CASE WHEN CONVERT(INT ,SUBSTRING(CONVERT(VARCHAR,CONVERT(DATETIME,ISNULL([JOIN_DATE],'1900-01-01')),111),9,2))> CONVERT(INT,SUBSTRING(CONVERT(VARCHAR,GETDATE(),111),9,2)) THEN (CONVERT(INT,SUBSTRING(CONVERT(VARCHAR,GETDATE(),111),9,2))+30)-CONVERT(INT ,SUBSTRING(CONVERT(VARCHAR,CONVERT(DATETIME,ISNULL([JOIN_DATE],'1900-01-01')),111),9,2)) ELSE CONVERT(INT,SUBSTRING(CONVERT(VARCHAR,GETDATE(),111),9,2)) - CONVERT(INT ,SUBSTRING(CONVERT(VARCHAR,CONVERT(DATETIME,ISNULL([JOIN_DATE],'1900-01-01')),111),9,2)) END))+'D') END) AS [Service Length]
      ,[EMP_TYPE] AS [Emp Type]        
  FROM [PMIS_PERSONNEL] WHERE [BranchKey] = '" + branchKey + "' AND [DISC_CODE] = '" + desgnationKey + "'  AND [EMP_CAT] = '" + employeeType + "' ";
            DataTable dt = DataManager.ExecuteQuery(connectionString, query, "PMIS_PERSONNEL");
            return dt;
        }

        public static DataTable GetShowAllEmployee(string Emp_No, string Designation, string DepartMent)
        {
            String connectionString = DataManager.OraConnString();
            string query = @"SELECT *
  FROM [View_EmployeeInformation] ";
            DataTable dt = DataManager.ExecuteQuery(connectionString, query, "PMIS_PERSONNEL");
            return dt;
        }
    }
}