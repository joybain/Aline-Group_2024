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
    public class BankGateway
    {
        SqlConnection connection = new SqlConnection(DataManager.OraConnString());

        public string GetBankAutoId()
        {
            try
            {
                connection.Open();
                string selectQuery = @"SELECT 'Bank-' + RIGHT('000000'+CONVERT(VARCHAR,ISNULL(MAX(CONVERT(INTEGER,RIGHT([bnk_id],6))),0)+1),6) FROM [tbl_bank_information]";
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

        public DataTable GetAllBankInformation()
        {
            try
            {
                connection.Open();
                string selectQuery = @"SELECT [bnk_id]
      ,[bnk_name]
 FROM [tbl_bank_information]";
                SqlDataAdapter da = new SqlDataAdapter(selectQuery, connection);
                DataSet ds = new DataSet();
                da.Fill(ds, "tbl_bank_information");
                DataTable table = ds.Tables["tbl_bank_information"];
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

        public void SaveTheBankInformation(Bank aBankObj)
        {
            try
            {
                connection.Open();
                string selectQuery = @"INSERT INTO [tbl_bank_information]
           ([bnk_id]
           ,[bnk_name])
     VALUES
           ('" + aBankObj.Id + "','" + aBankObj.Name + "')";
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

        internal void UpdateTheOldBankInforation(Bank aBankObj)
        {
            try
            {
                connection.Open();
                string selectQuery = @"UPDATE [tbl_bank_information]
   SET[bnk_name] ='" + aBankObj.Name + "' WHERE [bnk_id] ='" + aBankObj.Id + "'  ";
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

        internal void DeleteTheBank(Bank aBankObj)
        {
            try
            {
                connection.Open();
                string selectQuery = @"DELETE FROM [tbl_bank_information] WHERE [bnk_id] ='" + aBankObj.Id + "'  ";
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

        internal DataTable GetAllBankList(string BankKey)
        {
            try
            {
                connection.Open();
                string selectQuery = @"SELECT [BANK_ID]
      ,[BRANCH_ID]
      ,[BRANCH_NAME]
      ,[ADDR1]
      ,[ADDR2]
      ,[ADDR3]
      ,[PHONE]
  FROM [BANK_BRANCH] WHERE BANK_ID='" + BankKey + "' ";
                SqlDataAdapter da = new SqlDataAdapter(selectQuery, connection);
                DataSet ds = new DataSet();
                da.Fill(ds, "BANK_BRANCH");
                DataTable table = ds.Tables["BANK_BRANCH"];
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