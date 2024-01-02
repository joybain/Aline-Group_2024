using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using autouniv;
using KHSC.DAO.Others;

namespace KHSC.Gateway.Others
{
    public class ExamTitleGateway
    {
        SqlConnection connection = new SqlConnection(DataManager.OraConnString());

        internal string GetExamTitleAutoId()
        {
            try
            {
                connection.Open();
                string selectQuery = @"SELECT '0' + RIGHT('000000'+CONVERT(VARCHAR,ISNULL(MAX(CONVERT(INTEGER,RIGHT([exti_id],6))),0)+1),6) FROM [tbl_exam_title_information]";
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

        internal DataTable GetAllExamTitleInformation()
        {
            try
            {
                connection.Open();
                string selectQuery = @"SELECT [exti_id] As [Id]
      ,[exti_name] As [Name]
      ,'' As[Institute]
      ,'' As [GPA]
      ,'' As [Grade]
  FROM [tbl_exam_title_information]";
                SqlDataAdapter da = new SqlDataAdapter(selectQuery, connection);
                DataSet ds = new DataSet();
                da.Fill(ds, "tbl_exam_title_information");
                DataTable table = ds.Tables["tbl_exam_title_information"];
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

        internal void SaveTheExamInformation(ExamTitle aExamTitleObj)
        {
            try
            {
                connection.Open();
                string selectQuery = @"INSERT INTO [tbl_exam_title_information]
           ([exti_id]
           ,[exti_name])
     VALUES
           ('" + aExamTitleObj.Id + "','" + aExamTitleObj.Name+ "')";
                SqlCommand command = new SqlCommand(selectQuery, connection);
                command.ExecuteNonQuery();
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

        internal List<ExamTitle> GetAllExamTitleInformationForSpecificEmployee(string employeeId)
        {
            try
            {
                connection.Open();
                string selectQuery = @"SELECT (SELECT  [exti_name]  FROM [tbl_exam_title_information] WHERE [exti_id]=[edi_exam_title_id])
      ,[edi_institute_name]
      ,[edi_gpa]
      ,[edi_grade]
  FROM [tbl_employee_education_info] WHERE [edi_employee_id]='" + employeeId + "'";
                SqlCommand command = new SqlCommand(selectQuery, connection);
                SqlDataReader reader = command.ExecuteReader();
                List<ExamTitle> axamTitlelIST = new List<ExamTitle>();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        ExamTitle aExamTitleObj = new ExamTitle();
                        aExamTitleObj.ExamId = reader[0].ToString();
                        aExamTitleObj.Inistitute = reader[1].ToString();
                        aExamTitleObj.GPA = reader[2].ToString();
                        aExamTitleObj.Grade = reader[3].ToString();

                        axamTitlelIST.Add(aExamTitleObj);
                    }
                }
                return axamTitlelIST;
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

        internal void DeleteTheExam(ExamTitle aExamTitleObj)
        {
            try
            {
                connection.Open();
                string selectQuery = @"DELETE FROM [tbl_exam_title_information] WHERE [exti_id] ='" + aExamTitleObj.Id + "'  ";
                SqlCommand command = new SqlCommand(selectQuery, connection);
                command.ExecuteNonQuery();
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

        internal void UpdateTheOldExamInforation(ExamTitle aExamTitleObj)
        {
            try
            {
                connection.Open();
                string selectQuery = @"UPDATE [tbl_exam_title_information]
   SET[exti_name] ='" + aExamTitleObj.Name + "' WHERE [exti_id] ='" + aExamTitleObj.Id + "'  ";
                SqlCommand command = new SqlCommand(selectQuery, connection);
                command.ExecuteNonQuery();
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

        internal List<string> GetLastExamName(string empId)
        {
             try
             {
                 connection.Open();
                 string selectQuery = @"SELECT  (SELECT  [exti_name] FROM [tbl_exam_title_information] WHERE [exti_id]=[edi_exam_title_id])  FROM [tbl_employee_education_info]WHERE [edi_employee_id]='" + empId + "'  ";
                 SqlCommand command = new SqlCommand(selectQuery, connection);
                 SqlDataReader reader = command.ExecuteReader();
                 List<string> strLis = new List<string>();
                 if (reader.HasRows)
                 {
                     while (reader.Read())
                     {
                         strLis.Add(reader[0].ToString());
                     }
                 }
                 return strLis;
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

        internal DataTable GetAllExamTitleInexamInfoInformation()
        {
            try
            {
                connection.Open();
                string selectQuery = @"SELECT [exam_title_id]
      ,[exam_title]
  FROM  [tbl_Exam_Title]";
                SqlDataAdapter da = new SqlDataAdapter(selectQuery, connection);
                DataSet ds = new DataSet();
                da.Fill(ds, "tbl_Exam_Title");
                DataTable table = ds.Tables["tbl_Exam_Title"];
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