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
    public class DesignationGateway
    {
        SqlConnection connection = new SqlConnection(DataManager.OraConnString());

        public string GetDesignationAutoId()
        {
            try
            {
                connection.Open();
                string selectQuery = @"SELECT 'Designation-' + RIGHT('000000'+CONVERT(VARCHAR,ISNULL(MAX(CONVERT(INTEGER,RIGHT([desig_id],6))),0)+1),6) FROM [tbl_designation_information]";
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

        public DataTable GetAllDesignationInformation()
        {
            try
            {
                connection.Open();
                string selectQuery = @"SELECT [desig_id]
      ,[desig_name]
  FROM [tbl_designation_information]";
                SqlDataAdapter da = new SqlDataAdapter(selectQuery, connection);
                DataSet ds = new DataSet();
                da.Fill(ds, "tbl_designation_information");
                DataTable table = ds.Tables["tbl_designation_information"];
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

        public void SaveTheDesignationInformation(Designation aDesignationObj)
        {
            try
            {
                connection.Open();
                string selectQuery = @"INSERT INTO [tbl_designation_information]
           ([desig_id]
           ,[desig_name])
     VALUES
           ('"+aDesignationObj.Id+"','"+aDesignationObj.Name+"')";
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

        internal void UpdateTheOldDesigInforation(Designation aDesignationObj)
        {
            try
            {
                connection.Open();
                string selectQuery = @"UPDATE [tbl_designation_information]
   SET[desig_name] ='" + aDesignationObj.Name + "' WHERE [desig_id] ='" + aDesignationObj.Id + "'  ";
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

        internal void DeleteTheDesig(Designation aDesignationObj)
        {
            try
            {
                connection.Open();
                string selectQuery = @"DELETE FROM [tbl_designation_information] WHERE [desig_id] ='" + aDesignationObj.Id + "'  ";
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