using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

using autouniv;

namespace OldColor
{
    public class clsClientInfoManager
    {
        public static DataTable GetClientInfos()
        {
            string connectionString = DataManager.OraConnString();
            SqlConnection sqlCon = new SqlConnection(connectionString);
            string query = "select client_id,client_name,national_id,address1,address2,phone,mobile,fax,email,url,status from client_info order by client_id";
            DataTable dt = DataManager.ExecuteQuery(connectionString, query, "client_info");
            return dt;
        }
        public static DataTable GetClientInfosGrid()
        {
            string connectionString = DataManager.OraConnString();
            SqlConnection sqlCon = new SqlConnection(connectionString);
            string query = "select * from Customer a where a.DeleteBy IS NULL order by ID";
            DataTable dt = DataManager.ExecuteQuery(connectionString, query, "Customer");
            return dt;
        }
        public static void CreateClientInfo(clsClientInfo ci)
        {
            String connectionString = DataManager.OraConnString();
            SqlConnection sqlCon = new SqlConnection(connectionString);
            string query = @"INSERT INTO [Customer]
           ([Code]           
           ,[ContactName]
           ,[Email]
           ,[Mobile]
           ,[Phone]
           ,[Fax]
           ,[Address1]
           ,[Address2]
           ,[NationalId]          
           ,[PostalCode]
           ,[Country]
           ,[Active],CommonCus,[AddBy],[AddDate],Gl_CoaCode)
     VALUES
           ('" + ci.Code + "','" + ci.CustomerName + "','" + ci.Email + "','" + ci.Mobile + "','" + ci.Phone + "','" + ci.Fax + "','" + ci.Address1 + "','" + ci.Address2 + "','" + ci.NationalId + "','" + ci.PostalCode + "','" + ci.Country + "','" + ci.Active + "','" + ci.CommonCus + "','" + ci.LoginBy + "','" + Globals._localTime.ToString() + "','" + ci.GlCoa + "')";
            DataManager.ExecuteNonQuery(connectionString, query);
            sqlCon.Close();
        }

        public static void UpdateClientInfo(clsClientInfo ci)
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

                command.CommandText = @"update Customer set ContactName='" + ci.CustomerName + "',City='" +
                                      ci.NationalId + "', Address1= '" + ci.Address1 + "', Address2= '" + ci.Address2 +
                                      "', Phone= '" + ci.Phone + "', Mobile='" + ci.Mobile + "',Fax='" + ci.Fax +
                                      "', Active= '" + ci.Active + "' ,[UpdateBy]='" + ci.LoginBy +
                                      "',[UpdateDate]='" + Globals._localTime.ToString() + "',CommonCus='" + ci.CommonCus + "' where ID='" + ci.ID +
                                      "' ";
                command.ExecuteNonQuery();

                //*********** Auto Coa generate off **********//
                //command.CommandText = @"UPDATE [GL_SEG_COA] SET [SEG_COA_DESC] ='Accounts Receivable from-Customer-" + ci.CustomerName + "'  WHERE [SEG_COA_CODE]='" + ci.GlCoa + "'";
                //command.ExecuteNonQuery();

                //command.CommandText = @"UPDATE [GL_COA] SET [COA_DESC] ='" + ci.GlCoaDesc + "' where [GL_COA_CODE]='1-" + ci.GlCoa + "'";
                //command.ExecuteNonQuery();

                transaction.Commit();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
            } 

        }

        public static void DeleteClientInfo(clsClientInfo ci)
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

                command.CommandText = @"update Customer set [DeleteBy]='" + ci.LoginBy +
                                      "',[DeleteDate]='" + Globals._localTime.ToString() + "' where ID='" + ci.ID +
                                      "' ";
                command.ExecuteNonQuery();

                //*********** Auto Coa generate off **********//
                //command.CommandText = @"delete from GL_SEG_COA where SEG_COA_CODE='" + ci.GlCoa + "' ";
                //command.ExecuteNonQuery();

                //command.CommandText = @"delete from GL_COA where COA_NATURAL_CODE='" + ci.GlCoa + "' ";
                //command.ExecuteNonQuery();

                transaction.Commit();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
            }

        }

        public static clsClientInfo GetClientInfo(string ci)
        {
            string connectionString = DataManager.OraConnString();
            SqlConnection sqlCon = new SqlConnection(connectionString);
            string query = "select * from Customer where ID = '" + ci + "' ";
            DataTable dt = DataManager.ExecuteQuery(connectionString, query, "client_info");
            sqlCon.Close();
            if (dt.Rows.Count == 0)
            {
                return null;
            }
            return new clsClientInfo(dt.Rows[0]);
        }
        public static clsClientInfo GetClientInfoIdName(string ci)
        {
            string connectionString = DataManager.OraConnString();
            SqlConnection sqlCon = new SqlConnection(connectionString);
            string query = "select client_id,client_name,national_id,address1,address2,phone,mobile,fax,email,url,status from client_info where upper(client_id + ' - '+client_name) = upper('" + ci + "') ";
            DataTable dt = DataManager.ExecuteQuery(connectionString, query, "Client");
            sqlCon.Close();
            if (dt.Rows.Count == 0)
            {
                return null;
            }
            return new clsClientInfo(dt.Rows[0]);
        }
        public static clsClientInfo GetClientInfoPp(string ci,string pp)
        {
            string connectionString = DataManager.OraConnString();
            SqlConnection sqlCon = new SqlConnection(connectionString);
            string query = "select client_id,client_name,national_id,address1,address2,phone,mobile,fax,email,url,status from client_info where client_id = '" + ci + "' or passport='"+pp+"'";
            DataTable dt = DataManager.ExecuteQuery(connectionString, query, "Client");
            sqlCon.Close();
            if (dt.Rows.Count == 0)
            {
                return null;
            }
            return new clsClientInfo(dt.Rows[0]);
        }
        public static string getClientName(string cid)
        {
            String ConnectionString = DataManager.OraConnString();
            SqlConnection myConnection = new SqlConnection(ConnectionString);
            string Query = "select client_name from client_info where client_id='" + cid + "'";
            myConnection.Open();
            SqlCommand myCommand = new SqlCommand(Query, myConnection);
            object maxValue = myCommand.ExecuteScalar();
            myConnection.Close();
            if (maxValue != null)
            {
                return maxValue.ToString();
            }
            return "";
        }

        public static DataTable GetCommonClient()
        {
            string connectionString = DataManager.OraConnString();
            SqlConnection sqlCon = new SqlConnection(connectionString);
            string query = "select ID,ContactName,Code from Customer where CommonCus='1'";
            DataTable dt = DataManager.ExecuteQuery(connectionString, query, "Client");
            return dt;
        }
      
        public static DataTable GetShowSupplierOnPayment(string p)
        {
            String connectionString = DataManager.OraConnString();
            string query = @"SELECT ID,Code,[ContactName],Gl_CoaCode  FROM [Customer] where UPPER([Code]+'-'+[ContactName]+'-'+[Mobile])=UPPER('" + p + "') and Active='True'";
            DataTable dt = DataManager.ExecuteQuery(connectionString, query, "Customer");
            return dt;
        }
        public static int GetShowPaymentID()
        {
            SqlConnection connection = new SqlConnection(DataManager.OraConnString());
            try
            {
                connection.Open();
                string Query = @"SELECT top(1)[ID]  FROM [SupplierPayment] order by [ID] desc";
                SqlCommand command = new SqlCommand(Query, connection);
                return Convert.ToInt32(command.ExecuteScalar());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
            }
        }

        public static DataTable GetShowCustomerHistory(string P,string P_Type)
        {
            string connectionString = DataManager.OraConnString();
            SqlConnection sqlCon = new SqlConnection(connectionString);
            string Parameter = "";
            if (P != "") { Parameter = "Where t1.Chk_Status not in ('A') and t1.Customer_id='" + P + "' and Payment_Type='" + P_Type + "' order by ID desc"; } else { Parameter = "Where  t1.Chk_Status not in ('A') and  Payment_Type='" + P_Type + "' order by ID desc"; }
            string query = @"SELECT top(50) t1.[ID]
              ,t2.Code
              ,t2.ContactName
              ,CONVERT(nvarchar,t1.[Date],103) AS PmDate           
              ,t1.[PayAmt] 
              ,t1.ChequeNo
              ,CASE WHEN t1.Chk_Status='P' THEN 'Pending' WHEN t1.Chk_Status='A' THEN 'Approved' WHEN t1.Chk_Status='B' THEN 'Bounce' ELSE '' END AS[Chk_Status]
          FROM [CustomerPaymentReceive] t1 inner join Customer t2 on t2.ID=t1.Customer_id " + Parameter;
            DataTable dt = DataManager.ExecuteQuery(connectionString, query, "CustomerPaymentReceive");
            return dt;
        }
        public static DataTable GetShowCheckNubber(string ChkId)
        {
            string connectionString = DataManager.OraConnString();
            SqlConnection sqlCon = new SqlConnection(connectionString);
            string query = @"SELECT t1.[ID]
      ,convert(nvarchar,t1.Date,103) AS PmDate
      ,t1.Invoice
      ,t1.Customer_id
      ,t3.ContactName
      ,t3.Gl_CoaCode
      ,t1.[PayAmt]
      ,t1.[PayMethod]
      ,t1.[Bank_id]
      ,t1.[ChequeNo]
      ,convert(nvarchar,t1.[ChequeDate],103) AS [ChequeDate]      
      ,t1.Chk_Status
  FROM CustomerPaymentReceive t1 inner join [Order] t2 on t2.ID=t1.Invoice inner join Customer t3 on t3.ID=t2.CustomerID where t1.[ChequeNo]='" + ChkId + "'";
            DataTable dt = DataManager.ExecuteQuery(connectionString, query, "SupplierPayment");
            return dt;
        }

        public static DataTable GetShowPartyInfo(string p)
        {
            string connectionString = DataManager.OraConnString();
            SqlConnection sqlCon = new SqlConnection(connectionString);
            string query = @"SELECT  tt.ID,tt.ContactName FROM [Customer] tt where UPPER([ContactName]+' - '+[Mobile]) LIKE UPPER('%" + p + "%')";
            DataTable dt = DataManager.ExecuteQuery(connectionString, query, "SupplierPayment");
            return dt;
        }


        public DataTable GetPaymentDetailsSupplier(string ID)
        {
            string parameter = "";
            if (!string.IsNullOrEmpty(ID))
            {
                parameter = " where [ID]='" + ID + "' ";
            }

            string connectionString = DataManager.OraConnString();
            SqlConnection sqlCon = new SqlConnection(connectionString);

            string query =
                @" SELECT top(100) [ID],[Date],[Supplier_id],[ContactName],[Gl_CoaCode] ,[SearchName],[Mobile],[Phone],[Address1],GRN,[PayAmt],[PayMethod],[Bank_id],[ChequeNo],[ChequeDate],[Chk_Status]
      ,[Payment_Type],[Remarks]
  FROM [dbo].[View_SearchSupplierPayment] " + parameter + " order by id desc";
            DataTable dt = DataManager.ExecuteQuery(connectionString, query, "View_SearchSupplierPayment");
            return dt;
        }


        public double GetDueAmountSupplier(string CustomerID)
        {

            return IdManager.GetShowSingleValueCurrency(
                  "Select SUM(isnull(Due,0)-isnull(Payment,0)) as Due  from [dbo].[View_GetSupplierPaymentDue]  where [SupplierID]='" + CustomerID + "'  group by SupplierID ");
        }


        public void DeleteSupplierPayment(clsSupplierPaymentRec _aclsClientPaymentRec)
        {
            String connectionString = DataManager.OraConnString();
            SqlConnection sqlCon = new SqlConnection(connectionString);
            string Query = @"UPDATE [dbo].[SupplierPaymentReceive]
             SET [DeleteBy] ='" +
                           _aclsClientPaymentRec.LoginBy + "' ,[DeleteDate] =GETDATE() WHERE ID='" +
                           _aclsClientPaymentRec.ID + "'";
            DataManager.ExecuteNonQuery(connectionString, Query);
            sqlCon.Close();
        }


        public void SaveSupplierPayment(clsSupplierPaymentRec _aclsClientPaymentRec, VouchMst _aVouchMstCR)
        {
            SqlConnection connection = new SqlConnection(DataManager.OraConnString());
            SqlTransaction transaction;
            DataTable dtFixCode = VouchManager.GetAllFixGlCode("");
            connection.Open();
            transaction = connection.BeginTransaction();
            try
            {

                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                string Query = @"INSERT INTO [dbo].[SupplierPaymentReceive]
                ([Date],[Supplier_id],[PayAmt],[entry_by],[entry_date],[Remarks])
            VALUES
                (convert(date,'" + _aclsClientPaymentRec.Date + "'+' '+ substring(CONVERT(VARCHAR, GETDATE(), 108),0,6),103),'" + _aclsClientPaymentRec.Supplier_id +
                             "','" + _aclsClientPaymentRec.PayAmt.Replace(",", "") + "','" +
                             _aclsClientPaymentRec.LoginBy + "',GETDATE(),'" +
                             _aclsClientPaymentRec.Remarks.Replace(",", "") + "')";
                command.CommandText = Query;
                command.ExecuteNonQuery();

                //command.CommandText = @"SELECT top(1) [ID]  FROM [SupplierPaymentReceive] order by ID desc";
                //string MstID = command.ExecuteScalar().ToString();

                ////***************************  Devid Voucher ********************************// 
                //if (Convert.ToDecimal(_aclsClientPaymentRec.PayAmt) > 0)
                //{
                //    _aVouchMstCR.ControlAmt = _aclsClientPaymentRec.PayAmt.Replace("'", "").Replace(",", "");
                //    _aVouchMstCR.SerialNo = MstID;
                //    command.CommandText = VouchManager.SaveVoucherMst(_aVouchMstCR, 1);
                //    command.ExecuteNonQuery();
                //    VouchDtl vdtlCR;
                //    for (int j = 0; j < 2; j++)
                //    {
                //        if (j == 0)
                //        {
                //            //DataRow 
                //            vdtlCR = new VouchDtl();
                //            vdtlCR.VchSysNo = _aVouchMstCR.VchSysNo;
                //            vdtlCR.ValueDate = _aclsClientPaymentRec.Date;
                //            vdtlCR.LineNo = "1";
                //            vdtlCR.GlCoaCode = "1-" + _aclsClientPaymentRec.SupplierCoa;
                //            vdtlCR.Particulars = "Supplier Payment Received";
                //            vdtlCR.AccType = VouchManager.getAccType("1-" + _aclsClientPaymentRec.SupplierCoa);
                //            vdtlCR.AmountCr = "0";
                //            vdtlCR.AmountDr = _aVouchMstCR.ControlAmt.Replace(",", "");
                //            vdtlCR.Status = _aVouchMstCR.Status;
                //            vdtlCR.BookName = _aVouchMstCR.BookName; //*********** Convert Rate ********//

                //            vdtlCR.AUTHO_USER = _aVouchMstCR.EntryUser;
                //            VouchManager.CreateVouchDtlForAutoVoucher(_aVouchMstCR, vdtlCR, command);
                //        }
                //        else if (j == 1)
                //        {
                //            vdtlCR = new VouchDtl();
                //            vdtlCR.VchSysNo = _aVouchMstCR.VchSysNo;
                //            vdtlCR.ValueDate = _aclsClientPaymentRec.Date;
                //            vdtlCR.LineNo = "2";
                //            if (string.IsNullOrEmpty(_aclsClientPaymentRec.BankId))
                //            {
                //                vdtlCR.GlCoaCode = "1-" + dtFixCode.Rows[0]["CashInHand_BD"].ToString(); //**** SalesCode *******//
                //                vdtlCR.AccType =
                //                    VouchManager.getAccType("1-" + dtFixCode.Rows[0]["CashInHand_BD"]
                //                        .ToString()); //**** SalesCode *******//
                //                vdtlCR.Particulars = dtFixCode.Rows[0]["CashName_BD"].ToString();
                //            }
                //            else
                //            {
                //                //vdtlCR.GlCoaCode = "1-" + aSales.BankCoaCode; //**** SalesCode *******//
                //                //vdtlCR.AccType =
                //                //    VouchManager.getAccType("1-" + aSales.BankCoaCode); //**** SalesCode *******//
                //                //vdtlCR.Particulars = aSales.BankName;
                //            }

                //            vdtlCR.AmountCr = _aVouchMstCR.ControlAmt.Replace(",", "");
                //            vdtlCR.AmountDr = "0";
                //            vdtlCR.Status = _aVouchMstCR.Status;
                //            vdtlCR.BookName = _aVouchMstCR.BookName;
                //            vdtlCR.AUTHO_USER = _aVouchMstCR.EntryUser;
                //            //*********** Convert Rate ********//
                //            VouchManager.CreateVouchDtlForAutoVoucher(_aVouchMstCR, vdtlCR, command);
                //        }
                //    }
                //}
                transaction.Commit();
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw new Exception(ex.Message);
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
            }
        }



        public void DeleteCustomerPayment(clsClientPaymentRec _aclsClientPaymentRec)
        {
            String connectionString = DataManager.OraConnString();
            SqlConnection sqlCon = new SqlConnection(connectionString);
            string Query = @"UPDATE [dbo].[CustomerPaymentReceive]
             SET [DeleteBy] ='" +
                           _aclsClientPaymentRec.LoginBy + "' ,[DeleteDate] =GETDATE() WHERE ID='" +
                           _aclsClientPaymentRec.ID + "'";
            DataManager.ExecuteNonQuery(connectionString, Query);
            sqlCon.Close();
        }


        public DataTable GetBranchPaymentDetails(string ID, string BranchId)
        {
            string parameter = "where  BranchId='" + BranchId + "'";
            if (!string.IsNullOrEmpty(ID))
            {
                parameter = parameter + "  and   [ID]='" + ID + "' ";
            }


            string connectionString = DataManager.OraConnString();
            SqlConnection sqlCon = new SqlConnection(connectionString);

            string query =
                @"SELECT top(100) [ID],[Date],[Customer_id],[ContactName],[Gl_CoaCode] ,[SearchName],[Mobile],[Phone],[Address1],[Invoice],[PayAmt],[PayMethod],[Bank_id],[ChequeNo],[ChequeDate],[Chk_Status]
      ,[Payment_Type],[Remarks]
  FROM [dbo].[View_Search_Customer_Payment] " + parameter + " order by id desc";
            DataTable dt = DataManager.ExecuteQuery(connectionString, query, "View_Search_Customer_Payment");
            return dt;
        }

        public DataTable GetPaymentDetails(string ID)
        {
            string parameter = "";
            if (!string.IsNullOrEmpty(ID))
            {
                parameter =  " where  [ID]='" + ID + "' ";
            }


            string connectionString = DataManager.OraConnString();
            SqlConnection sqlCon = new SqlConnection(connectionString);

            string query =
                @"SELECT top(100) [ID],[Date],[Customer_id],[ContactName],[Gl_CoaCode] ,[SearchName],[Mobile],[Phone],[Address1],[Invoice],[PayAmt],[PayMethod],[Bank_id],[ChequeNo],[ChequeDate],[Chk_Status]
      ,[Payment_Type],[Remarks]
  FROM [dbo].[View_Search_Customer_Payment] " + parameter + " order by id desc";
            DataTable dt = DataManager.ExecuteQuery(connectionString, query, "View_Search_Customer_Payment");
            return dt;
        }
        public DataTable GetCustomerOnSearch(string SearchParameter, int flag)
        {
            string connectionString = DataManager.OraConnString();
            SqlConnection sqlCon = new SqlConnection(connectionString);
            string query = "";
            if (flag.Equals(0))
            {
                query = @"SELECT [ID],[ContactName],[Gl_CoaCode],[SearchName], Address1, Address2 FROM [View_SearchCustomer] " +
                               SearchParameter;
            }
            if (flag.Equals(5))
            {
                query = @"SELECT [ID],[ContactName],[Gl_CoaCode],[SearchName], Address1, Address2 FROM [View_SearchCustomer] where CommonCus=1  " + SearchParameter;
            }
            else
            {
                query = @"SELECT [ID],[ContactName],[Gl_CoaCode],[SearchName], Address1, Address2 FROM [View_SearchCustomer] " +
                        SearchParameter;
            }
            DataTable dt = DataManager.ExecuteQuery(connectionString, query, "View_SearchCustomer");
            return dt;
        }

        public double GetDueAmount(string CustomerID)
        {
            //return  IdManager.GetShowSingleValueCurrency(
            //      "select tab.DueAmt-isnull((SELECT isnull(t1.[PayAmt],0) FROM [dbo].[CustomerPaymentReceive] t1 where t1.[Customer_id]='" +
            //      CustomerID +
            //      "' and Invoice IS NULL),0) from (SELECT isnull(t1.[PayAmt],0) AS [DueAmt] FROM [dbo].[CustomerPaymentReceive] t1 where t1.[Customer_id]='" +
            //      CustomerID + "' and Invoice IS NOT NULL) tab");


            return IdManager.GetShowSingleValueCurrency(
                  "Select SUM(isnull(Due,0)-isnull(Payment,0)) as Due  from View_GetCustomerPaymentDue  where CustomerID='" + CustomerID + "'  group by CustomerID ");
        }

        public void DeleteBranchCustomerPayment(clsClientPaymentRec _aclsClientPaymentRec)
        {
            String connectionString = DataManager.OraConnString();
            SqlConnection sqlCon = new SqlConnection(connectionString);
            string Query = @"UPDATE [dbo].[CustomerPaymentReceive]
             SET [DeleteBy] ='" +
                           _aclsClientPaymentRec.LoginBy + "' ,[DeleteDate] =GETDATE() WHERE ID='" +
                           _aclsClientPaymentRec.ID + "' AND BranchId='" + _aclsClientPaymentRec.BranchId + "'";
            DataManager.ExecuteNonQuery(connectionString, Query);
            sqlCon.Close();
        }


        public double GetBranchDueAmount(string CustomerID, string BranchId)
        {
            //return  IdManager.GetShowSingleValueCurrency(
            //      "select tab.DueAmt-isnull((SELECT isnull(t1.[PayAmt],0) FROM [dbo].[CustomerPaymentReceive] t1 where t1.[Customer_id]='" +
            //      CustomerID +
            //      "' and Invoice IS NULL),0) from (SELECT isnull(t1.[PayAmt],0) AS [DueAmt] FROM [dbo].[CustomerPaymentReceive] t1 where t1.[Customer_id]='" +
            //      CustomerID + "' and Invoice IS NOT NULL) tab");


            return IdManager.GetShowSingleValueCurrency(
                "Select SUM(isnull(Due,0)-isnull(Payment,0)) as Due  from View_GetCustomerPaymentDue  where CustomerID='" + CustomerID + "' and BranchId='" + BranchId + "'  group by CustomerID ");
        }



        public void UpdateBranchCustomerPayment(clsClientPaymentRec _aclsClientPaymentRec, VouchMst _aVouchMstDV)
        {
            SqlConnection connection = new SqlConnection(DataManager.OraConnString());
            SqlTransaction transaction;
            DataTable dtFixCode = VouchManager.GetAllFixGlCode("");
            try
            {
                connection.Open();

                transaction = connection.BeginTransaction();
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;
                string Query = @"UPDATE [dbo].[CustomerPaymentReceive]
             SET [Date] = convert(date,'" + _aclsClientPaymentRec.Date + "'+' '+ substring(CONVERT(VARCHAR, GETDATE(), 108),0,6),103),[Customer_id] = '" +
                               _aclsClientPaymentRec.Customer_id +
                               "',[PayAmt] ='" + _aclsClientPaymentRec.PayAmt.Replace(",", "") + "' ,[update_by] ='" +
                               _aclsClientPaymentRec.LoginBy + "' ,[update_date] =GETDATE(),[Remarks] = '" +
                               _aclsClientPaymentRec.Remarks.Replace(",", "") + "' WHERE ID='" +
                               _aclsClientPaymentRec.ID + "' and BranchId='" + _aclsClientPaymentRec.BranchId + "'";
                command.CommandText = Query;
                command.ExecuteNonQuery();

                if (_aVouchMstDV.RefFileNo.Equals("New"))
                {
                    _aVouchMstDV.ControlAmt = _aclsClientPaymentRec.PayAmt.Replace("'", "").Replace(",", "");
                    command.CommandText = VouchManager.SaveVoucherMst(_aVouchMstDV, 1);
                    command.ExecuteNonQuery();
                    VouchDtl vdtlCR;
                    for (int j = 0; j < 2; j++)
                    {
                        if (j == 0)
                        {
                            //DataRow 
                            vdtlCR = new VouchDtl();
                            vdtlCR.VchSysNo = _aVouchMstDV.VchSysNo;
                            vdtlCR.ValueDate = _aclsClientPaymentRec.Date;
                            vdtlCR.LineNo = "1";
                            vdtlCR.GlCoaCode = "1-" + _aclsClientPaymentRec.CustomerCoa;
                            vdtlCR.Particulars = "Customer Payment Received";
                            vdtlCR.AccType = VouchManager.getAccType("1-" + _aclsClientPaymentRec.CustomerCoa);
                            vdtlCR.AmountDr = "0";
                            vdtlCR.AmountCr = _aVouchMstDV.ControlAmt.Replace(",", "");
                            vdtlCR.Status = _aVouchMstDV.Status;
                            vdtlCR.BookName = _aVouchMstDV.BookName; //*********** Convert Rate ********//

                            vdtlCR.AUTHO_USER = _aVouchMstDV.EntryUser;
                            VouchManager.CreateVouchDtlForAutoVoucher(_aVouchMstDV, vdtlCR, command);
                        }
                        else if (j == 1)
                        {
                            vdtlCR = new VouchDtl();
                            vdtlCR.VchSysNo = _aVouchMstDV.VchSysNo;
                            vdtlCR.ValueDate = _aclsClientPaymentRec.Date;
                            vdtlCR.LineNo = "2";
                            if (string.IsNullOrEmpty(_aclsClientPaymentRec.BankId))
                            {
                                vdtlCR.GlCoaCode = "1-" + dtFixCode.Rows[0]["CashInHand_BD"].ToString(); //**** SalesCode *******//
                                vdtlCR.AccType =
                                    VouchManager.getAccType("1-" + dtFixCode.Rows[0]["CashInHand_BD"]
                                        .ToString()); //**** SalesCode *******//
                                vdtlCR.Particulars = dtFixCode.Rows[0]["CashName_BD"].ToString();
                            }
                            else
                            {
                                //vdtlCR.GlCoaCode = "1-" + aSales.BankCoaCode; //**** SalesCode *******//
                                //vdtlCR.AccType =
                                //    VouchManager.getAccType("1-" + aSales.BankCoaCode); //**** SalesCode *******//
                                //vdtlCR.Particulars = aSales.BankName;
                            }

                            vdtlCR.AmountDr = _aVouchMstDV.ControlAmt.Replace(",", "");
                            vdtlCR.AmountCr = "0";
                            vdtlCR.Status = _aVouchMstDV.Status;
                            vdtlCR.BookName = _aVouchMstDV.BookName;
                            vdtlCR.AUTHO_USER = _aVouchMstDV.EntryUser;
                            //*********** Convert Rate ********//
                            VouchManager.CreateVouchDtlForAutoVoucher(_aVouchMstDV, vdtlCR, command);
                        }
                    }
                }
                else
                {
                    _aVouchMstDV.ControlAmt = _aclsClientPaymentRec.PayAmt.Replace("'", "").Replace(",", "");
                    command.CommandText = VouchManager.SaveVoucherMst(_aVouchMstDV, 2);
                    command.ExecuteNonQuery();

                    command.CommandText = @"delete from gl_trans_dtl where vch_sys_no=convert(numeric,'" +
                                          _aVouchMstDV.VchSysNo + "')";
                    command.ExecuteNonQuery();
                    VouchDtl vdtlCR;

                    for (int j = 0; j < 2; j++)
                    {
                        if (j == 0)
                        {
                            //DataRow 
                            vdtlCR = new VouchDtl();
                            vdtlCR.VchSysNo = _aVouchMstDV.VchSysNo;
                            vdtlCR.ValueDate = _aclsClientPaymentRec.Date;
                            vdtlCR.LineNo = "1";
                            vdtlCR.GlCoaCode = "1-" + _aclsClientPaymentRec.CustomerCoa;
                            vdtlCR.Particulars = "Customer Payment Received";
                            vdtlCR.AccType = VouchManager.getAccType("1-" + _aclsClientPaymentRec.CustomerCoa);
                            vdtlCR.AmountDr = "0";
                            vdtlCR.AmountCr = _aVouchMstDV.ControlAmt.Replace(",", "");
                            vdtlCR.Status = _aVouchMstDV.Status;
                            vdtlCR.BookName = _aVouchMstDV.BookName; //*********** Convert Rate ********//

                            vdtlCR.AUTHO_USER = _aVouchMstDV.EntryUser;
                            VouchManager.CreateVouchDtlForAutoVoucher(_aVouchMstDV, vdtlCR, command);
                        }
                        else if (j == 1)
                        {
                            vdtlCR = new VouchDtl();
                            vdtlCR.VchSysNo = _aVouchMstDV.VchSysNo;
                            vdtlCR.ValueDate = _aclsClientPaymentRec.Date;
                            vdtlCR.LineNo = "2";
                            if (string.IsNullOrEmpty(_aclsClientPaymentRec.BankId))
                            {
                                vdtlCR.GlCoaCode = "1-" + dtFixCode.Rows[0]["CashInHand_BD"].ToString(); //**** SalesCode *******//
                                vdtlCR.AccType =
                                    VouchManager.getAccType("1-" + dtFixCode.Rows[0]["CashInHand_BD"]
                                        .ToString()); //**** SalesCode *******//
                                vdtlCR.Particulars = dtFixCode.Rows[0]["CashName_BD"].ToString();
                            }
                            else
                            {
                                //vdtlCR.GlCoaCode = "1-" + aSales.BankCoaCode; //**** SalesCode *******//
                                //vdtlCR.AccType =
                                //    VouchManager.getAccType("1-" + aSales.BankCoaCode); //**** SalesCode *******//
                                //vdtlCR.Particulars = aSales.BankName;
                            }

                            vdtlCR.AmountCr = _aVouchMstDV.ControlAmt.Replace(",", "");
                            vdtlCR.AmountDr = "0";
                            vdtlCR.Status = _aVouchMstDV.Status;
                            vdtlCR.BookName = _aVouchMstDV.BookName;
                            vdtlCR.AUTHO_USER = _aVouchMstDV.EntryUser;
                            //*********** Convert Rate ********//
                            VouchManager.CreateVouchDtlForAutoVoucher(_aVouchMstDV, vdtlCR, command);
                        }
                    }
                }
                transaction.Commit();
            }

            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
            }
        }


        public void SaveBranchCustomerPayment(clsClientPaymentRec _aclsClientPaymentRec, VouchMst _aVouchMstCR)
        {
            SqlConnection connection = new SqlConnection(DataManager.OraConnString());
            SqlTransaction transaction;
            DataTable dtFixCode = VouchManager.GetAllFixGlCode("");
            connection.Open();
            transaction = connection.BeginTransaction();
            try
            {

                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                string Query = @"INSERT INTO [dbo].[CustomerPaymentReceive]
                (BranchId,[Date],[Customer_id],[PayAmt],[entry_by],[entry_date],[Remarks])
            VALUES
                ('" + _aclsClientPaymentRec.BranchId + "',convert(date,'" + _aclsClientPaymentRec.Date + "'+' '+ substring(CONVERT(VARCHAR, GETDATE(), 108),0,6),103),'" + _aclsClientPaymentRec.Customer_id +
                             "','" + _aclsClientPaymentRec.PayAmt.Replace(",", "") + "','" +
                             _aclsClientPaymentRec.LoginBy + "',GETDATE(),'" +
                             _aclsClientPaymentRec.Remarks.Replace(",", "") + "')";
                command.CommandText = Query;
                command.ExecuteNonQuery();

                ////***************************  Credit Voucher ********************************// 
                //command.CommandText = @"SELECT top(1) [ID]  FROM [CustomerPaymentReceive] where BranchId='" + _aclsClientPaymentRec.BranchId + "' order by ID desc";
                //string MstID = command.ExecuteScalar().ToString();

                ////***************************  Devid Voucher ********************************// 
                //if (Convert.ToDecimal(_aclsClientPaymentRec.PayAmt) > 0)
                //{
                //    _aVouchMstCR.ControlAmt = _aclsClientPaymentRec.PayAmt.Replace("'", "").Replace(",", "");
                //    _aVouchMstCR.SerialNo = MstID;
                //    command.CommandText = VouchManager.SaveVoucherMst(_aVouchMstCR, 1);
                //    command.ExecuteNonQuery();
                //    VouchDtl vdtlCR;
                //    for (int j = 0; j < 2; j++)
                //    {
                //        if (j == 0)
                //        {
                //            //DataRow 
                //            vdtlCR = new VouchDtl();
                //            vdtlCR.VchSysNo = _aVouchMstCR.VchSysNo;
                //            vdtlCR.ValueDate = _aclsClientPaymentRec.Date;
                //            vdtlCR.LineNo = "1";
                //            vdtlCR.GlCoaCode = "1-" + _aclsClientPaymentRec.CustomerCoa;
                //            vdtlCR.Particulars = "Customer Payment Received";
                //            vdtlCR.AccType = VouchManager.getAccType("1-" + _aclsClientPaymentRec.CustomerCoa);
                //            vdtlCR.AmountDr = "0";
                //            vdtlCR.AmountCr = _aVouchMstCR.ControlAmt.Replace(",", "");
                //            vdtlCR.Status = _aVouchMstCR.Status;
                //            vdtlCR.BookName = _aVouchMstCR.BookName; //*********** Convert Rate ********//

                //            vdtlCR.AUTHO_USER = _aVouchMstCR.EntryUser;
                //            VouchManager.CreateVouchDtlForAutoVoucher(_aVouchMstCR, vdtlCR, command);
                //        }
                //        else if (j == 1)
                //        {
                //            vdtlCR = new VouchDtl();
                //            vdtlCR.VchSysNo = _aVouchMstCR.VchSysNo;
                //            vdtlCR.ValueDate = _aclsClientPaymentRec.Date;
                //            vdtlCR.LineNo = "2";
                //            if (string.IsNullOrEmpty(_aclsClientPaymentRec.BankId))
                //            {
                //                vdtlCR.GlCoaCode = "1-" + dtFixCode.Rows[0]["CashInHand_BD"].ToString(); //**** SalesCode *******//
                //                vdtlCR.AccType =
                //                    VouchManager.getAccType("1-" + dtFixCode.Rows[0]["CashInHand_BD"]
                //                        .ToString()); //**** SalesCode *******//
                //                vdtlCR.Particulars = dtFixCode.Rows[0]["CashName_BD"].ToString();
                //            }
                //            else
                //            {
                //                //vdtlCR.GlCoaCode = "1-" + aSales.BankCoaCode; //**** SalesCode *******//
                //                //vdtlCR.AccType =
                //                //    VouchManager.getAccType("1-" + aSales.BankCoaCode); //**** SalesCode *******//
                //                //vdtlCR.Particulars = aSales.BankName;
                //            }

                //            vdtlCR.AmountDr = _aVouchMstCR.ControlAmt.Replace(",", "");
                //            vdtlCR.AmountCr = "0";
                //            vdtlCR.Status = _aVouchMstCR.Status;
                //            vdtlCR.BookName = _aVouchMstCR.BookName;
                //            vdtlCR.AUTHO_USER = _aVouchMstCR.EntryUser;
                //            //*********** Convert Rate ********//
                //            VouchManager.CreateVouchDtlForAutoVoucher(_aVouchMstCR, vdtlCR, command);
                //        }
                //    }
                //} 
                transaction.Commit();
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw new Exception(ex.Message);
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
            }
        }

        public void UpdateSupplierPayment(clsSupplierPaymentRec _aclsClientPaymentRec, VouchMst _aVouchMstDV)
        {
            SqlConnection connection = new SqlConnection(DataManager.OraConnString());
            SqlTransaction transaction;
            DataTable dtFixCode = VouchManager.GetAllFixGlCode("");
            try
            {
                connection.Open();

                transaction = connection.BeginTransaction();
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                string Query = @"UPDATE [dbo].[SupplierPaymentReceive]
             SET [Date] = convert(date,'" + _aclsClientPaymentRec.Date + "'+' '+ substring(CONVERT(VARCHAR, GETDATE(), 108),0,6),103),[Supplier_id] = '" +
                               _aclsClientPaymentRec.Supplier_id +
                               "',[PayAmt] ='" + _aclsClientPaymentRec.PayAmt.Replace(",", "") + "' ,[update_by] ='" +
                               _aclsClientPaymentRec.LoginBy + "' ,[update_date] =GETDATE(),[Remarks] = '" +
                               _aclsClientPaymentRec.Remarks.Replace(",", "") + "' WHERE ID='" +
                               _aclsClientPaymentRec.ID + "'";
                command.CommandText = Query;
                command.ExecuteNonQuery();

                //_aVouchMstDV.ControlAmt = _aclsClientPaymentRec.PayAmt.Replace("'", "").Replace(",", "");
                //command.CommandText = VouchManager.SaveVoucherMst(_aVouchMstDV, 2);
                //command.ExecuteNonQuery();

                //command.CommandText = @"delete from gl_trans_dtl where vch_sys_no=convert(numeric,'" +
                //                      _aVouchMstDV.VchSysNo + "')";
                //command.ExecuteNonQuery();
                //VouchDtl vdtlCR;
                //if (_aVouchMstDV.RefFileNo.Equals("New"))
                //{
                //    _aVouchMstDV.ControlAmt = _aclsClientPaymentRec.PayAmt.Replace("'", "").Replace(",", "");
                //    command.CommandText = VouchManager.SaveVoucherMst(_aVouchMstDV, 1);
                //    command.ExecuteNonQuery();
                //    for (int j = 0; j < 2; j++)
                //    {
                //        if (j == 0)
                //        {
                //            //DataRow 
                //            vdtlCR = new VouchDtl();
                //            vdtlCR.VchSysNo = _aVouchMstDV.VchSysNo;
                //            vdtlCR.ValueDate = _aclsClientPaymentRec.Date;
                //            vdtlCR.LineNo = "1";
                //            vdtlCR.GlCoaCode = "1-" + _aclsClientPaymentRec.SupplierCoa;
                //            vdtlCR.Particulars = "Supplier Payment Received";
                //            vdtlCR.AccType = VouchManager.getAccType("1-" + _aclsClientPaymentRec.SupplierCoa);
                //            vdtlCR.AmountCr = "0";
                //            vdtlCR.AmountDr = _aVouchMstDV.ControlAmt.Replace(",", "");
                //            vdtlCR.Status = _aVouchMstDV.Status;
                //            vdtlCR.BookName = _aVouchMstDV.BookName; //*********** Convert Rate ********//

                //            vdtlCR.AUTHO_USER = _aVouchMstDV.EntryUser;
                //            VouchManager.CreateVouchDtlForAutoVoucher(_aVouchMstDV, vdtlCR, command);
                //        }
                //        else if (j == 1)
                //        {
                //            vdtlCR = new VouchDtl();
                //            vdtlCR.VchSysNo = _aVouchMstDV.VchSysNo;
                //            vdtlCR.ValueDate = _aclsClientPaymentRec.Date;
                //            vdtlCR.LineNo = "2";
                //            if (string.IsNullOrEmpty(_aclsClientPaymentRec.BankId))
                //            {
                //                vdtlCR.GlCoaCode = "1-" + dtFixCode.Rows[0]["CashInHand_BD"].ToString(); //**** SalesCode *******//
                //                vdtlCR.AccType =
                //                    VouchManager.getAccType("1-" + dtFixCode.Rows[0]["CashInHand_BD"]
                //                        .ToString()); //**** SalesCode *******//
                //                vdtlCR.Particulars = dtFixCode.Rows[0]["CashName_BD"].ToString();
                //            }
                //            else
                //            {
                //                //vdtlCR.GlCoaCode = "1-" + aSales.BankCoaCode; //**** SalesCode *******//
                //                //vdtlCR.AccType =
                //                //    VouchManager.getAccType("1-" + aSales.BankCoaCode); //**** SalesCode *******//
                //                //vdtlCR.Particulars = aSales.BankName;
                //            }

                //            vdtlCR.AmountDr = _aVouchMstDV.ControlAmt.Replace(",", "");
                //            vdtlCR.AmountCr = "0";
                //            vdtlCR.Status = _aVouchMstDV.Status;
                //            vdtlCR.BookName = _aVouchMstDV.BookName;
                //            vdtlCR.AUTHO_USER = _aVouchMstDV.EntryUser;
                //            //*********** Convert Rate ********//
                //            VouchManager.CreateVouchDtlForAutoVoucher(_aVouchMstDV, vdtlCR, command);
                //        }
                //    }
                //}
                //else
                //{
                //    for (int j = 0; j < 2; j++)
                //    {
                //        if (j == 0)
                //        {
                //            //DataRow 
                //            vdtlCR = new VouchDtl();
                //            vdtlCR.VchSysNo = _aVouchMstDV.VchSysNo;
                //            vdtlCR.ValueDate = _aclsClientPaymentRec.Date;
                //            vdtlCR.LineNo = "1";
                //            vdtlCR.GlCoaCode = "1-" + _aclsClientPaymentRec.SupplierCoa;
                //            vdtlCR.Particulars = "Supplier Payment Received";
                //            vdtlCR.AccType = VouchManager.getAccType("1-" + _aclsClientPaymentRec.SupplierCoa);
                //            vdtlCR.AmountCr = "0";
                //            vdtlCR.AmountDr = _aVouchMstDV.ControlAmt.Replace(",", "");
                //            vdtlCR.Status = _aVouchMstDV.Status;
                //            vdtlCR.BookName = _aVouchMstDV.BookName; //*********** Convert Rate ********//

                //            vdtlCR.AUTHO_USER = _aVouchMstDV.EntryUser;
                //            VouchManager.CreateVouchDtlForAutoVoucher(_aVouchMstDV, vdtlCR, command);
                //        }
                //        else if (j == 1)
                //        {
                //            vdtlCR = new VouchDtl();
                //            vdtlCR.VchSysNo = _aVouchMstDV.VchSysNo;
                //            vdtlCR.ValueDate = _aclsClientPaymentRec.Date;
                //            vdtlCR.LineNo = "2";
                //            if (string.IsNullOrEmpty(_aclsClientPaymentRec.BankId))
                //            {
                //                vdtlCR.GlCoaCode = "1-" + dtFixCode.Rows[0]["CashInHand_BD"].ToString(); //**** SalesCode *******//
                //                vdtlCR.AccType =
                //                    VouchManager.getAccType("1-" + dtFixCode.Rows[0]["CashInHand_BD"]
                //                        .ToString()); //**** SalesCode *******//
                //                vdtlCR.Particulars = dtFixCode.Rows[0]["CashName_BD"].ToString();
                //            }
                //            else
                //            {
                //                //vdtlCR.GlCoaCode = "1-" + aSales.BankCoaCode; //**** SalesCode *******//
                //                //vdtlCR.AccType =
                //                //    VouchManager.getAccType("1-" + aSales.BankCoaCode); //**** SalesCode *******//
                //                //vdtlCR.Particulars = aSales.BankName;
                //            }

                //            vdtlCR.AmountCr = _aVouchMstDV.ControlAmt.Replace(",", "");
                //            vdtlCR.AmountDr = "0";
                //            vdtlCR.Status = _aVouchMstDV.Status;
                //            vdtlCR.BookName = _aVouchMstDV.BookName;
                //            vdtlCR.AUTHO_USER = _aVouchMstDV.EntryUser;
                //            //*********** Convert Rate ********//
                //            VouchManager.CreateVouchDtlForAutoVoucher(_aVouchMstDV, vdtlCR, command);
                //        }
                //    }
                //} 
                transaction.Commit();
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
            }
        }
       
    }
}
