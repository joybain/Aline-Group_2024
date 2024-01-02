using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using autouniv;
using KHSC.DAO.Others;



namespace KHSC.Gateway.Others
{
    public class DepartmentGateway
    {
        SqlConnection connection = new SqlConnection(DataManager.OraConnString());

        public string GetDepartmentAutoId()
        {
            try
            {
                connection.Open();
                string selectQuery = @"SELECT 'Section-' + RIGHT('000000'+CONVERT(VARCHAR,ISNULL(MAX(CONVERT(INTEGER,RIGHT([dept_id],6))),0)+1),6) FROM [tbl_department_information]";
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

        public void SaveTheDepartmentInformation(Department aDepartmentObj)
        {
            try
            {
                connection.Open();
                string selectQuery = @"INSERT INTO [tbl_department_information]
           ([dept_id]
           ,[dept_name])
     VALUES
           ('" + aDepartmentObj.Id + "','" + aDepartmentObj.Name + "')";
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

        public DataTable GetAllDepartmentInformation()
        {
            try
            {
                connection.Open();
                string selectQuery = @"SELECT [dept_id]
      ,[dept_name]
  FROM [tbl_department_information]";
                SqlDataAdapter da = new SqlDataAdapter(selectQuery, connection);
                DataSet ds = new DataSet();
                da.Fill(ds, "tbl_department_information");
                DataTable table = ds.Tables["tbl_department_information"];
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

       

        internal void UpdateTheOldDeptInforation(Department aDepartmentObj)
        {
            try
            {
                connection.Open();
                string selectQuery = @"UPDATE [tbl_department_information]
   SET[dept_name] ='" + aDepartmentObj.Name + "' WHERE [dept_id] ='" + aDepartmentObj.Id + "'  ";
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

        internal void DeleteTheDept(Department aDepartmentObj)
        {
            try
            {
                connection.Open();
                string selectQuery = @"DELETE FROM [tbl_department_information] WHERE [dept_id] ='" + aDepartmentObj.Id + "'  ";
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
    }
}