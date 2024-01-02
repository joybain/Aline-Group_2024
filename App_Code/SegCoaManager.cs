﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using Dorjibari;
using autouniv;


/// <summary>
/// Summary description for SegCoaManager
/// </summary>
/// 
namespace Dorjibari
{
    public class SegCoaManager
    {
        
        public static void CreateSegCoa(SegCoa segcoa, string CheckType)
        {
            SqlConnection connection = new SqlConnection(DataManager.OraConnString());
            SqlTransaction transaction;
            try
            {
                connection.Open();
                transaction = connection.BeginTransaction();
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;
                if (segcoa.ParentCode == "" || segcoa.ParentCode == null)
                {
                    command.CommandText = " insert into gl_seg_coa (seg_coa_code,lvl_code,seg_coa_desc,bud_allowed, " +
                       " post_allowed,acc_type,open_date,rootleaf,status,taxable,book_name,entry_user,entry_date) values ( " +
                       " '" + segcoa.GlSegCode + "',  '" + segcoa.LvlCode + "',  '" + segcoa.SegCoaDesc + "', " +
                       " '" + segcoa.BudAllowed + "',  '" + segcoa.PostAllowed + "', " +
                        "  '" + segcoa.AccType + "', convert(datetime,case '" + segcoa.OpenDate + "' when '' then null else '" + segcoa.OpenDate + "' end,103),  '" + segcoa.RootLeaf + "', " +
                        "  '" + segcoa.Status + "',  '" + segcoa.Taxable + "',  '" + segcoa.BookName + "','" + segcoa.EntryUser + "',convert(datetime,'" + segcoa.EntryDate + "',103))";
                    command.ExecuteNonQuery();


                }
                else
                {
                    command.CommandText = "insert into gl_seg_coa (seg_coa_code,lvl_code,seg_coa_desc,parent_code,bud_allowed, " +
                       " post_allowed,acc_type,open_date,rootleaf,status,taxable,book_name,entry_user,entry_date) values ( " +
                       " '" + segcoa.GlSegCode + "',  '" + segcoa.LvlCode + "',  '" + segcoa.SegCoaDesc + "', " +
                       "  '" + segcoa.ParentCode + "',  '" + segcoa.BudAllowed + "',  '" + segcoa.PostAllowed + "', " +
                        "  '" + segcoa.AccType + "', convert(datetime,case '" + segcoa.OpenDate + "' when '' then null else '" + segcoa.OpenDate + "' end,103),  '" + segcoa.RootLeaf + "', " +
                        "  '" + segcoa.Status + "',  '" + segcoa.Taxable + "',  '" + segcoa.BookName + "','" + segcoa.EntryUser + "',convert(datetime,'" + segcoa.EntryDate + "',103))";
                    command.ExecuteNonQuery();
                }

                if (!string.IsNullOrEmpty(CheckType))
                {
                    if (CheckType.Equals("C"))
                    {
                        command.CommandText = "select COUNT(*) from Customer where [Gl_CoaCode]='" + segcoa.GlSegCode + "'";
                        int cheksegcodes = Convert.ToInt32(command.ExecuteScalar());
                        if (cheksegcodes > 0)
                        {
                            command.CommandText = "UPDATE [Customer] SET [ContactName] ='" + segcoa.SegCoaDesc +
                                                  "',[ModifiedBy] ='" + segcoa.EntryUser + "',[ModifiedDate]=GETDATE()  WHERE [Gl_CoaCode]='" + segcoa.GlSegCode +
                                                  "' ";
                            command.ExecuteNonQuery();

                        }
                        else
                        {
                            //int SegCode = IdManager.GetNextID("Customer", "Code");
                            command.CommandText =
                                "INSERT INTO [Customer] ([Code],[ContactName],[CreatedBy],[CreatedDate],[Gl_CoaCode],Active) values  ('0000'+convert(nvarchar,(convert(int,(select MAX(code) from Customer))+1)),'" +
                                segcoa.SegCoaDesc + "', '" + segcoa.EntryUser + "',GETDATE(),'1-'+'" + segcoa.GlSegCode + "','True')";
                            command.ExecuteNonQuery();

                        }
                    }
                }

                // end code
                transaction.Commit();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
                transaction.Rollback();
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
            }
        }
         public static void CreateSegCoaForBank(SegCoa segcoa, string CheckType)
        {
            SqlConnection connection = new SqlConnection(DataManager.OraConnString());
            SqlTransaction transaction;
            try
            {
                connection.Open();
                transaction = connection.BeginTransaction();
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;
                if (segcoa.ParentCode == "" || segcoa.ParentCode == null)
                {
                    command.CommandText = " insert into gl_seg_coa (seg_coa_code,lvl_code,seg_coa_desc,bud_allowed, " +
                       " post_allowed,acc_type,open_date,rootleaf,status,taxable,book_name,entry_user,entry_date,AccountStatus) values ( " +
                       " '" + segcoa.GlSegCode + "',  '" + segcoa.LvlCode + "',  '" + segcoa.SegCoaDesc + "', " +
                       " '" + segcoa.BudAllowed + "',  '" + segcoa.PostAllowed + "', " +
                        "  '" + segcoa.AccType + "', convert(datetime,case '" + segcoa.OpenDate + "' when '' then null else '" + segcoa.OpenDate + "' end,103),  '" + segcoa.RootLeaf + "', " +
                        "  '" + segcoa.Status + "',  '" + segcoa.Taxable + "',  '" + segcoa.BookName + "','" + segcoa.EntryUser + "',convert(datetime,'" + segcoa.EntryDate + "',103),'"+segcoa.AccountType+"')";
                    command.ExecuteNonQuery();


                }
                else
                {
                    command.CommandText = "insert into gl_seg_coa (seg_coa_code,lvl_code,seg_coa_desc,parent_code,bud_allowed, " +
                       " post_allowed,acc_type,open_date,rootleaf,status,taxable,book_name,entry_user,entry_date) values ( " +
                       " '" + segcoa.GlSegCode + "',  '" + segcoa.LvlCode + "',  '" + segcoa.SegCoaDesc + "', " +
                       "  '" + segcoa.ParentCode + "',  '" + segcoa.BudAllowed + "',  '" + segcoa.PostAllowed + "', " +
                        "  '" + segcoa.AccType + "', convert(datetime,case '" + segcoa.OpenDate + "' when '' then null else '" + segcoa.OpenDate + "' end,103),  '" + segcoa.RootLeaf + "', " +
                        "  '" + segcoa.Status + "',  '" + segcoa.Taxable + "',  '" + segcoa.BookName + "','" + segcoa.EntryUser + "',convert(datetime,'" + segcoa.EntryDate + "',103))";
                    command.ExecuteNonQuery();
                }

                if (!string.IsNullOrEmpty(CheckType))
                {
                    if (CheckType.Equals("C"))
                    {
                        command.CommandText = "select COUNT(*) from Customer where [Gl_CoaCode]='" + segcoa.GlSegCode + "'";
                        int cheksegcodes = Convert.ToInt32(command.ExecuteScalar());
                        if (cheksegcodes > 0)
                        {
                            command.CommandText = "UPDATE [Customer] SET [ContactName] ='" + segcoa.SegCoaDesc +
                                                  "',[ModifiedBy] ='" + segcoa.EntryUser + "',[ModifiedDate]=GETDATE()  WHERE [Gl_CoaCode]='" + segcoa.GlSegCode +
                                                  "' ";
                            command.ExecuteNonQuery();

                        }
                        else
                        {
                            //int SegCode = IdManager.GetNextID("Customer", "Code");
                            command.CommandText =
                                "INSERT INTO [Customer] ([Code],[ContactName],[CreatedBy],[CreatedDate],[Gl_CoaCode],Active) values  ('0000'+convert(nvarchar,(convert(int,(select MAX(code) from Customer))+1)),'" +
                                segcoa.SegCoaDesc + "', '" + segcoa.EntryUser + "',GETDATE(),'1-'+'" + segcoa.GlSegCode + "','True')";
                            command.ExecuteNonQuery();

                        }
                    }
                }

                // end code
                transaction.Commit();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
                transaction.Rollback();
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
            }
        }
        public static void UpdateSegCoa(SegCoa segcoa)
        {
            String connectionString = DataManager.OraConnString();
            SqlConnection sqlCon = new SqlConnection(connectionString);

            string query = " update gl_seg_coa set lvl_code='" + segcoa.LvlCode + "', seg_coa_desc='" + segcoa.SegCoaDesc + "', " +
                   " parent_code= '" + segcoa.ParentCode + "', bud_allowed= '" + segcoa.BudAllowed + "', post_allowed= '" + segcoa.PostAllowed + "', " +
                    " acc_type= '" + segcoa.AccType + "', open_date= convert(datetime,case '" + segcoa.OpenDate + "' when '' then null else '" + segcoa.OpenDate + "' end,103), rootleaf= '" + segcoa.RootLeaf + "', " +
                    " taxable= '" + segcoa.Taxable + "',[AccountStatus]='"+segcoa.AccountType+"' where seg_coa_code='" + segcoa.GlSegCode + "'";
            DataManager.ExecuteNonQuery(connectionString, query);

            string Update = @"UPDATE [GL_COA]
   SET [COA_DESC] ='SDL," + segcoa.SegCoaDesc + "' WHERE [COA_NATURAL_CODE]='" + segcoa.GlSegCode + "'";
            DataManager.ExecuteNonQuery(connectionString, Update);


        }
        public static void UpdateSegCoaStatus(SegCoa segcoa)
        {
            String connectionString = DataManager.OraConnString();
            SqlConnection sqlCon = new SqlConnection(connectionString);

            string query = " update gl_seg_coa set status='" + segcoa.Status + "', autho_user='" + segcoa.AuthoUser + "', " +
                   " autho_date= convert(datetime,'" + segcoa.AuthoDate + "',103) where seg_coa_code='" + segcoa.GlSegCode + "'";

            DataManager.ExecuteNonQuery(connectionString, query);
        }

        public static void DeleteSegCoa(SegCoa segcoa)
        {
            String connectionString = DataManager.OraConnString();
            SqlConnection sqlCon = new SqlConnection(connectionString);

            string query = " delete from gl_seg_coa where seg_coa_code='"+segcoa.GlSegCode+"' ";
            DataManager.ExecuteNonQuery(connectionString, query);

            string query1 = " DELETE FROM [GL_COA] WHERE COA_NATURAL_CODE='" + segcoa.GlSegCode + "' ";
            DataManager.ExecuteNonQuery(connectionString, query1);
        }

        public static SegCoa getSegCoa(string segcoacode)
        {
            String connectionString = DataManager.OraConnString();
            SqlConnection sqlCon = new SqlConnection(connectionString);

            string query = "select seg_coa_code,lvl_code,seg_coa_desc,parent_code,bud_allowed,post_allowed,acc_type, "+
                " convert(varchar,open_date,103) open_date,rootleaf,status,taxable,book_name,entry_user,convert(varchar,entry_date,103) entry_date,autho_user,convert(varchar,autho_date,103) autho_date from gl_seg_coa where seg_coa_code='" + segcoacode + "'";
            DataTable dt= DataManager.ExecuteQuery(connectionString, query, "Gl_Seg_Coa");
            if (dt.Rows.Count == 0)
            {
                return null;
            }
            return new SegCoa(dt.Rows[0]);
        }

        public static DataTable GetSegCoas(string criteria)
        {
            string connectionString = DataManager.OraConnString();
            SqlConnection sqlCon = new SqlConnection(connectionString);
            string query = "SELECT SEG_COA_CODE,seg_coa_desc,acc_type FROM GL_SEG_COA a";
            if (criteria != "")
            {
                query = query + " where " + criteria +" order by seg_coa_code";
            }
            DataTable dt = DataManager.ExecuteQuery(connectionString, query, "SegCoa");
            return dt;
        }
        public static DataTable GetSegCoass(string query)
        {
            string connectionString = DataManager.OraConnString();
            SqlConnection sqlCon = new SqlConnection(connectionString);
            DataTable dt = DataManager.ExecuteQuery(connectionString, query, "SegCoa");
            return dt;
        }
        public static string GetLvlCode(string segcode)
        {
            String ConnectionString = DataManager.OraConnString();
            SqlConnection myConnection = new SqlConnection(ConnectionString);
            string Query = "select lvl_code from gl_seg_coa where seg_coa_code='" + segcode + "'";
            myConnection.Open();
            SqlCommand myCommand = new SqlCommand(Query, myConnection);
            object maxValue = myCommand.ExecuteScalar();
            myConnection.Close();
            return maxValue.ToString();
        }
        public static string GetSegCoaDesc(string segcode)
        {
            String ConnectionString = DataManager.OraConnString();
            SqlConnection myConnection = new SqlConnection(ConnectionString);
            string Query = "select seg_coa_desc from gl_seg_coa where seg_coa_code='" + segcode + "'";
            myConnection.Open();
            SqlCommand myCommand = new SqlCommand(Query, myConnection);
            object maxValue = myCommand.ExecuteScalar();
            myConnection.Close();
            return maxValue.ToString();
        }
        public static DataTable GetSegCoaAll()
        {
            string connectionString = DataManager.OraConnString();
            SqlConnection sqlCon = new SqlConnection(connectionString);
            string query = "SELECT SEG_COA_CODE,seg_coa_desc,parent_code,rootleaf FROM GL_SEG_COA where seg_coa_code like '0%' and parent_code is null and lvl_code in (select lvl_code from gl_level_type where lvl_enabled='Y') order by convert(varchar,lvl_code)";            
            DataTable dt = DataManager.ExecuteQuery(connectionString, query, "SegCoa");
            return dt;
        }

        public static DataTable GetSegCoaChild(string criteria)
        {
            string connectionString = DataManager.OraConnString();
            SqlConnection sqlCon = new SqlConnection(connectionString);
            string query = "SELECT SEG_COA_CODE,seg_coa_desc,parent_code,rootleaf FROM GL_SEG_COA";
            if (criteria != "")
            {
                query = query + " where " + criteria;
            }
            DataTable dt = DataManager.ExecuteQuery(connectionString, query, "SegCoa");
            return dt;
        }
        public static int getChild(string glcode)
        {
            int val;
            string connectionString = DataManager.OraConnString();
            SqlConnection sqlCon = new SqlConnection(connectionString);
            string query = "select count(*) cnt from gl_seg_coa where parent_code='"+glcode+"'";
            sqlCon.Open();
            SqlCommand myCommand = new SqlCommand(query, sqlCon);
            object maxValue = myCommand.ExecuteScalar();
            sqlCon.Close();
            if (maxValue == DBNull.Value) return 0;
            val = int.Parse(maxValue.ToString());
            return val;
        }
        public static DataTable getChildSegs(string book,string seg)
        {
            string connectionString = DataManager.OraConnString();
            SqlConnection sqlCon = new SqlConnection(connectionString);
            string query = "SELECT SEG_COA_CODE FROM GL_SEG_COA S WHERE BOOK_NAME = '" + book.ToUpper() + "' " +
                 " AND ROOTLEAF = 'L' CONNECT BY PRIOR SEG_COA_CODE=parent_code  AND book_name='" + book.ToUpper() + "' " +
                 " START WITH SEG_COA_CODE = '" + seg + "' AND book_name='" + book.ToUpper() + "' ";
            DataTable dt = DataManager.ExecuteQuery(connectionString, query, "SegCoa");
            return dt;
        }
        public static string getMainSeg(string segcoa)
        {
            string mainseg="";
            string connectionString = DataManager.OraConnString();
            SqlDataReader dReader;
            SqlConnection conn = new SqlConnection();
            conn.ConnectionString = connectionString;
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "select lvl_seg_type from gl_level_type where lvl_code=(select lvl_code from gl_seg_coa where seg_coa_code='"+segcoa+"')";
            if (conn.State != ConnectionState.Open)
            {
                conn.Open();
            }
            dReader = cmd.ExecuteReader();
            if (dReader.HasRows == true)
            {
                while (dReader.Read())
                {
                    mainseg = dReader["lvl_seg_type"].ToString();
                }
            }
            cmd.Dispose(); dReader.Close();
            if (conn.State == ConnectionState.Open)
            {
                conn.Close();
                SqlConnection.ClearPool(conn);
            }
            return mainseg;
        }

        public static DataTable GetShowBankOrClientInformation(string p)
        {
            SqlConnection connection = new SqlConnection(DataManager.OraConnString());
            connection.Open();
            string selectQuery = @"SELECT COUNT(*) FROM [CLIENT_INFO] where [gl_coa_code]='"+p+"'";
            SqlCommand command=new SqlCommand(selectQuery,connection);
            int count = Convert.ToInt32(command.ExecuteScalar());
            if (count > 0)
            {
                string connectionString = DataManager.OraConnString();
                SqlConnection sqlCon = new SqlConnection(connectionString);
                string query = @"SELECT [client_id]
                                ,[client_name]
                                ,'Client' AS[Check]
                                FROM [CLIENT_INFO] where [gl_coa_code]='" + p + "'";
                DataTable dt = DataManager.ExecuteQuery(connectionString, query, "SegCoa");
                return dt;
            }
            else
            {
                string connectionString = DataManager.OraConnString();
                SqlConnection sqlCon = new SqlConnection(connectionString);
                string query = @"SELECT [bank_id]
                                  ,[bank_name]
                                  ,'Bank' AS[Check]
                              FROM [bank_info] where [gl_coa_code]='" + p + "'";
                DataTable dt = DataManager.ExecuteQuery(connectionString, query, "SegCoa");
                return dt;
            }
        }

        public static int GetShowCount(string p)
        {
            SqlConnection connection = new SqlConnection(DataManager.OraConnString());
            connection.Open();
            string selectQuery = @"SELECT COUNT(*)  FROM [GL_TRANS_DTL] t where t.GL_COA_CODE='1-" + p + "'";
            SqlCommand command = new SqlCommand(selectQuery,connection);
            return Convert.ToInt32(command.ExecuteScalar());

        }
    }
}