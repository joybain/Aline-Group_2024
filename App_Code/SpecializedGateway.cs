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
    public class SpecializedGateway
    {
        SqlConnection connection = new SqlConnection(DataManager.OraConnString());

        public string GetSpecializedAutoId()
        {
            try
            {
                connection.Open();
                string selectQuery = @"SELECT 'Specialized-' + RIGHT('000000'+CONVERT(VARCHAR,ISNULL(MAX(CONVERT(INTEGER,RIGHT([spz_id],6))),0)+1),6) FROM [tbl_specialized_information]";
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

        public DataTable GetAllSpecializedInformation()
        {
            try
            {
                connection.Open();
                string selectQuery = @"SELECT [spz_id]
      ,[spz_name]
  FROM [tbl_specialized_information]";
                SqlDataAdapter da = new SqlDataAdapter(selectQuery, connection);
                DataSet ds = new DataSet();
                da.Fill(ds, "tbl_specialized_information");
                DataTable table = ds.Tables["tbl_specialized_information"];
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

        public void SaveTheSpecializedInformation(Specialized aSpecializedObj)
        {
            try
            {
                connection.Open();
                string selectQuery = @"INSERT INTO [tbl_specialized_information]
           ([spz_id]
           ,[spz_name])
     VALUES
           ('"+aSpecializedObj.Id+"','"+aSpecializedObj.Name+"')";
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

        public Specialized GetAllSpecializedInformationIsNotSelected(Specialized aSpecializedObj)
        {
            try
            {
                connection.Open();
                string selectQuery = @"SELECT [spz_id]
      ,[spz_name]
  FROM [tbl_specialized_information] WHERE [spz_id] !='" + aSpecializedObj.Id + "'";
                SqlCommand command = new SqlCommand(selectQuery, connection);
                SqlDataReader reader = command.ExecuteReader();
                Specialized ASpecializedoBj = new Specialized();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {

                        ASpecializedoBj.Id = reader[0].ToString();
                        ASpecializedoBj.Name = reader[1].ToString();
                        
                    }
                }
                return ASpecializedoBj;
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

        public DataTable GetAllSpecializedInformationIsForSpecificEmployee(string employeeId)
        {
            try
            {
                connection.Open();
                string selectQuery = @"SELECT  [empsz_specializtion_id] AS [ID]
,(SELECT  [spz_name] FROM [tbl_specialized_information] WHERE [spz_id]=[empsz_specializtion_id]) AS [spz_name]
  FROM [tbl_employee_specialzation_information] WHERE [empsz_employee_id]='" + employeeId + "'";
                SqlDataAdapter da = new SqlDataAdapter(selectQuery, connection);
                DataSet ds = new DataSet();
                da.Fill(ds, "tbl_employee_specialzation_information");
                DataTable table = ds.Tables["tbl_employee_specialzation_information"];
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

        public int CountTotalSpacialiedRow(string employee)
        {
            try
            {
                connection.Open();
                string selectQuery = @"SELECT  COUNT(*) FROM [tbl_employee_specialzation_information] WHERE [empsz_employee_id]='" + employee + "'";
                SqlCommand cmd=new SqlCommand(selectQuery,connection);
                return Convert.ToInt32(cmd);

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

        internal List<Specialized> GetAllInfo(string employee)
        {
            try
            {
                connection.Open();
                string selectQuery = @"SELECT  [empsz_specializtion_id]
  FROM [tbl_employee_specialzation_information] WHERE [empsz_employee_id]='" + employee + "'";
                SqlCommand command = new SqlCommand(selectQuery, connection);
                SqlDataReader reader = command.ExecuteReader();
                List<Specialized> sl = new List<Specialized>();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Specialized ASpecializedObj = new Specialized();
                        ASpecializedObj.Id = reader[0].ToString();

                        sl.Add(ASpecializedObj);
                    }
                }
                return sl;
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

        internal void DeleteTheSpecialized(Specialized aSpecializedObj)
        {
            try
            {
                connection.Open();
                string selectQuery = @"DELETE FROM [tbl_specialized_information] WHERE [empsz_employee_id] ='" + aSpecializedObj.Id + "'  ";
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

        internal void UpdateTheOldSpAreaInforation(Specialized aSpecializedObj)
        {
            try
            {
                connection.Open();
                string selectQuery = @"UPDATE [tbl_specialized_information]
   SET[spz_name] ='" + aSpecializedObj.Name + "' WHERE [empsz_employee_id] ='" + aSpecializedObj.Id + "'  ";
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