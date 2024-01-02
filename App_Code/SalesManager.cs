using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using autouniv;

/// <summary>
/// Summary description for SalesManager
/// </summary>
public class SalesManager
{
	public SalesManager()
	{
		//
		// TODO: Add constructor logic here
		//
	}

    public static DataTable GetShowItemsInformation(string ID)
    {
        String connectionString = DataManager.OraConnString();
        string query = @"  SELECT t1.[ID]
      ,t1.[Code]
      ,t1.[Name]             
      ,isnull(Rate,0) AS Tax
      ,t1.[DiscountAmount] 
      ,t1.[UnitPrice] AS SPrice
      ,'1' AS Qty ,'1' AS PackSize ,(select 'SQM '+CONVERT(nvarchar,tt.Quantity)+'Qty' from ItemSaleOrderDtl tt where tt.ItemOrderMstId='" + ID + "' and tt.ItemId=t1.ItemId) as OrderQty   ,t1.ClosingStock,'C' AS msr_unit_code,SalesClosingStock,isnull(SqmClosingStock,0) as SqmClosingStock,isnull(SftClosingStock,0) as SftClosingStock ,convert(decimal(18,2),((ISNULL(t1.UnitPrice,0)*1)-(ISNULL(t1.UnitPrice,0)*(t1.DiscountAmount/100)))) AS Total   FROM View_ManufactureStockForSales t1 where t1.[Active]='1' and t1.Code='" + ID + "' and t1.ClosingStock>0 ";
        DataTable dt = DataManager.ExecuteQuery(connectionString, query, "Item");
        return dt;
    }


    public static DataTable GetShowItemsInformationSalesOrder(string SalesOrderID)
    {
        String connectionString = DataManager.OraConnString();
//        string query = @"  SELECT t1.[ID]
//      ,t1.[Code]
//      ,t1.[Name]             
//      ,isnull(Rate,0) AS Tax
//      ,t1.[DiscountAmount] 
//      ,t1.[UnitPrice] AS SPrice
//      ,'1' AS Qty ,'1' AS PackSize, (select 'SQM '+CONVERT(nvarchar,tt.Quantity)+'Qty' from ItemSaleOrderDtl tt where tt.ItemOrderMstId='" + SalesOrderID + "' and tt.ItemId=t1.ItemId) as OrderQty     ,t1.ClosingStock,'C' AS msr_unit_code,SalesClosingStock,isnull(SqmClosingStock,0) as SqmClosingStock,isnull(SftClosingStock,0) as SftClosingStock    ,convert(decimal(18,2),((ISNULL(t1.UnitPrice,0)*1)-(ISNULL(t1.UnitPrice,0)*(t1.DiscountAmount/100)))) AS Total   FROM View_ManufactureStockForSales t1 where t1.[Active]='1' and t1.ItemId in (select ItemId from ItemSaleOrderDtl where ItemOrderMstId='" + SalesOrderID + "') and t1.ClosingStock>0 ";


        string query = @"  SELECT t1.[ID]
      ,t1.[Code]
      ,t1.[Name]             
      ,isnull(Rate,0) AS Tax
      ,t1.[DiscountAmount] 
      ,t1.[UnitPrice] AS SPrice
      ,'1' AS Qty ,'1' AS PackSize, (select 'SQM '+CONVERT(nvarchar,tt.Quantity)+'Qty' from ItemSaleOrderDtl tt where tt.ItemOrderMstId='" + SalesOrderID + "' and tt.ItemId=t1.ItemId) as OrderQty     ,t1.ClosingStock,'C' AS msr_unit_code,SalesClosingStock,isnull(SqmClosingStock,0) as SqmClosingStock,isnull(SftClosingStock,0) as SftClosingStock    ,convert(decimal(18,2),((ISNULL(t1.UnitPrice,0)*1)-(ISNULL(t1.UnitPrice,0)*(t1.DiscountAmount/100)))) AS Total   FROM View_ManufactureStockForSales t1 where  " +
                       "t1.Id in (select  MIN(t1.Id) as Id from View_ManufactureStockForSales t1 where t1.[Active]='1' and t1.ItemId in (select ItemId from ItemSaleOrderDtl where ItemOrderMstId='"+SalesOrderID+"') and ClosingStock>0 group by t1.ItemId) ";

        DataTable dt = DataManager.ExecuteQuery(connectionString, query, "Item");
        return dt;
    }

    public static Sales GetShowSalesInfo(string ID)
    {
        string connectionString = DataManager.OraConnString();
        SqlConnection sqlCon = new SqlConnection(connectionString);
        string query = @"SELECT t1.[ID],t1.[CompanyId],t1.[InvoiceNo],convert(decimal(18,2),t1.[SubTotal]) AS SubTotal,convert(decimal(18,2),t1.[TaxAmount]) 
AS TaxAmount,convert(decimal(18,2),t1.[DiscountAmount]) AS DiscountAmount,
((convert(decimal(18,2),t1.[SubTotal])+((convert(decimal(18,2),t1.[SubTotal])*convert(decimal(18,2),t1.[TaxAmount]))/100))-convert(decimal(18,2),t1.[DiscountAmount])) as GrandTotal
,convert(decimal(18,2),t1.[CashReceived]) AS CashReceived,t1.[CashRefund],convert(nvarchar,t1.[OrderDate],103) AS OrderDate,t1.[PaymentMethodID],t2.ChequeNo AS [PaymentMethodNumber],t2.Bank_id AS [BankId],convert(nvarchar,t2.[ChequeDate],103) AS ChequeDate,convert(decimal(18,2),t1.[ChequeAmount]) AS ChequeAmount,t1.[CustomerID],t1.[OrderStatusID],t1.[CreatedBy],t1.[CreatedDate] ,t1.[ModifiedBy],t1.[ModifiedDate],
(((convert(decimal(18,2),t1.[SubTotal])+((convert(decimal(18,2),t1.[SubTotal])*convert(decimal(18,2),t1.[TaxAmount]))/100))-convert(decimal(18,2),t1.[DiscountAmount]))-ISNULL(t1.CashReceived,0)) AS Due,t1.[DeliveryStatus],convert(nvarchar,t1.[DeliveryDate],103) AS DeliveryDate,t1.[Remark],t1.CustomerID,t2.Chk_Status,t1.DriverName,t1.DriverPhoneNo,t1.CarNo,t1.OutTime FROM [Order] t1 left join CustomerPaymentReceive t2 on t2.Customer_id=t1.ID where t1.ID='" + ID + "'";
        DataTable dt = DataManager.ExecuteQuery(connectionString, query, "Order");
        if (dt.Rows.Count == 0)
        {
            return null;
        }
        return new Sales(dt.Rows[0]);
    }

    public static Sales GetShowSalesOrderInfo(string ID)
    {
        string connectionString = DataManager.OraConnString();
        SqlConnection sqlCon = new SqlConnection(connectionString);
        string query = @"SELECT t1.[ID],t1.[CompanyId],t1.[InvoiceNo],convert(decimal(18,2),t1.[SubTotal]) AS SubTotal,convert(decimal(18,2),t1.[TaxAmount]) 
AS TaxAmount,convert(decimal(18,2),t1.[DiscountAmount]) AS DiscountAmount,
((convert(decimal(18,2),t1.[SubTotal])+((convert(decimal(18,2),t1.[SubTotal])*convert(decimal(18,2),t1.[TaxAmount]))/100))-convert(decimal(18,2),t1.[DiscountAmount])) as GrandTotal
,convert(decimal(18,2),t1.[CashReceived]) AS CashReceived,t1.[CashRefund],convert(nvarchar,t1.[OrderDate],103) AS OrderDate,t1.[PaymentMethodID],t2.ChequeNo AS [PaymentMethodNumber],t2.Bank_id AS [BankId],convert(nvarchar,t2.[ChequeDate],103) AS ChequeDate,convert(decimal(18,2),t1.[ChequeAmount]) AS ChequeAmount,t1.[CustomerID],t1.[OrderStatusID],t1.[CreatedBy],t1.[CreatedDate] ,t1.[ModifiedBy],t1.[ModifiedDate],
(((convert(decimal(18,2),t1.[SubTotal])+((convert(decimal(18,2),t1.[SubTotal])*convert(decimal(18,2),t1.[TaxAmount]))/100))-convert(decimal(18,2),t1.[DiscountAmount]))-ISNULL(t1.CashReceived,0)) AS Due,t1.[DeliveryStatus],convert(nvarchar,t1.[DeliveryDate],103) AS DeliveryDate,t1.[Remark],t1.CustomerID,t2.Chk_Status,t1.DriverName,t1.DriverPhoneNo,t1.CarNo,t1.OutTime FROM SalesOrder t1 left join CustomerPaymentReceive t2 on t2.Customer_id=t1.ID where t1.ID='" + ID + "'";
        DataTable dt = DataManager.ExecuteQuery(connectionString, query, "Order");
        if (dt.Rows.Count == 0)
        {
            return null;
        }
        return new Sales(dt.Rows[0]);
    }

    public static void SaveSalesInfo(Sales aSales, DataTable dt)
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

            command.CommandText = @"INSERT INTO [Order]
           ([InvoiceNo],[SubTotal],[TaxAmount],[DiscountAmount],[GrandTotal],[CashReceived],[CashRefund],[OrderDate],[CustomerID],[Due],[DeliveryStatus],[DeliveryDate],[Remark],[CreatedBy],[CreatedDate],DriverName,DriverPhoneNo,CarNo,OutTime)
     VALUES
           ('" + aSales.Invoice + "','" + aSales.Total + "','" + aSales.Tax + "','" + aSales.Disount + "','" + aSales.GTotal + "','" + aSales.CReceive + "','0',convert(datetime, nullif( '" + aSales.Date + "',''), 103),'" + aSales.Customer + "','" + aSales.Due + "','" + aSales.DvStatus + "',convert(datetime, nullif( '" + aSales.DvDate + "',''), 103),'" + aSales.Remarks + "','" + aSales.LoginBy + "',GETDATE(),'" + aSales.DriverName + "','" + aSales.DriverPhoneNo + "','" + aSales.CarNo + "','" + aSales.OutTime + "')";
            command.ExecuteNonQuery();






         



            command.CommandText = @"SELECT top(1) [ID]  FROM [Order] order by ID desc";
            string OrderMstID = command.ExecuteScalar().ToString();


            //*************************** Customer Payment ******************//
            decimal totPay = 0;
            if (aSales.PMethod == "C" && Convert.ToDouble(aSales.CReceive) > 0)
            {
                command.CommandText = @"INSERT INTO [CustomerPaymentReceive]
           ([Date],[Customer_id],[Invoice],[PayAmt],[PayMethod],[Bank_id],[ChequeNo],[ChequeDate],[entry_by],[entry_date],Payment_Type)
     VALUES
           (CONVERT(DATE,'" + aSales.Date + "',103),'" + aSales.Customer + "','" + OrderMstID + "','" + aSales.CReceive + "','" + aSales.PMethod + "','" + aSales.BankId + "','" + aSales.PMNumber + "',CONVERT(DATE,'" + aSales.ChequeDate + "',103),'" + aSales.LoginBy + "',GETDATE(),'IV')";
                command.ExecuteNonQuery();
            }

            if (aSales.PMethod == "Q")
            {
                if (Convert.ToDecimal(aSales.CReceive) == 0) { totPay = Convert.ToDecimal(aSales.GTotal); } else { totPay = Convert.ToDecimal(aSales.CReceive); }
                command.CommandText = @"INSERT INTO [CustomerPaymentReceive]
           ([Date],[Customer_id],[Invoice],[PayAmt],[PayMethod],[Bank_id],[ChequeNo],[ChequeDate],[entry_by],[entry_date],Chk_Status,Payment_Type)
     VALUES
           (CONVERT(DATE,'" + aSales.Date + "',103),'" + aSales.Customer + "','" + OrderMstID + "','" + totPay + "','" + aSales.PMethod + "','" + aSales.BankId + "','" + aSales.PMNumber + "',CONVERT(DATE,'" + aSales.ChequeDate + "',103),'" + aSales.LoginBy + "',GETDATE(),'" + aSales.Chk_Status + "','IV')";
                command.ExecuteNonQuery();

            }
            if (Convert.ToDecimal(aSales.CReceive) > 0 && aSales.PMethod == "CR")
            {
                command.CommandText = @"INSERT INTO [CustomerPaymentReceive]
           ([Date],[Customer_id],[Invoice],[PayAmt],[PayMethod],[Bank_id],[ChequeNo],[ChequeDate],[entry_by],[entry_date],Payment_Type)
     VALUES
           (CONVERT(DATE,'" + aSales.Date + "',103),'" + aSales.Customer + "','" + OrderMstID + "','" + aSales.CReceive + "','" + aSales.PMethod + "','" + aSales.BankId + "','" + aSales.PMNumber + "',CONVERT(DATE,'" + aSales.ChequeDate + "',103),'" + aSales.LoginBy + "',GETDATE(),'IV')";
                command.ExecuteNonQuery();
            }
            //***************************  ********************************// 
            foreach (DataRow dr in dt.Rows)
            {
                if (dr["Code"].ToString() != "")
                {
                    command.CommandText = @"INSERT INTO [OrderDetail]
           ([OrderID],[ItemID],[UnitPrice]  ,[TaxRate] ,[DiscountAmount] ,[SalePrice] ,[Quantity],PackSize,RollOrCftFlag ,[TotalPrice] ,[CreatedBy],[CreatedDate])
     VALUES
           ('" + OrderMstID + "','" + dr["ID"].ToString() + "','" + dr["SPrice"].ToString().Replace(",", "") + "','" + dr["Tax"].ToString().Replace(",", "") + "','" + dr["DiscountAmount"].ToString().Replace(",", "") + "','" + dr["SPrice"].ToString().Replace(",", "") + "','" + dr["Qty"].ToString().Replace(",", "") + "','" + dr["PackSize"].ToString().Replace(",", "") + "','" + dr["msr_unit_code"].ToString().Replace(",", "") + "','" + dr["Total"].ToString().Replace(",", "") + "','" + aSales.LoginBy + "',GETDATE())";
                    command.ExecuteNonQuery();







                    //New 

                    if (dr["msr_unit_code"].ToString().Replace(",", "") == "C")
                    {
                        command.CommandText = "UPDATE Manufacturing_Stock SET ClosingStock = (ClosingStock - (" +
                                              dr["Qty"].ToString().Replace(",", "") +
                                              "/isnull(CFT_Per_Role,0))), ClosingAmount = (ClosingAmount -((" +
                                              dr["Qty"].ToString().Replace(",", "") +
                                              "/isnull(CFT_Per_Role,0))* " + dr["Total"].ToString().Replace(",", "") + ")) WHERE ID='" + dr["ID"].ToString() +"'";
                        command.ExecuteNonQuery();
                    }
                    else if (dr["msr_unit_code"].ToString().Replace(",", "") == "SQ")
                    {
                        

                        command.CommandText = "UPDATE Manufacturing_Stock SET ClosingStock = (ClosingStock - (" +
                                             dr["Qty"].ToString().Replace(",", "") +
                                             "/isnull(SQM_Per_Role,0))), ClosingAmount = (ClosingAmount -((" +
                                             dr["Qty"].ToString().Replace(",", "") +
                                             "/isnull(SQM_Per_Role,0))* " + dr["Total"].ToString().Replace(",", "") + ")) WHERE ID='" + dr["ID"].ToString() + "'";

                        command.ExecuteNonQuery();
                    }

                    else if (dr["msr_unit_code"].ToString().Replace(",", "") == "SF")
                    {

                        command.CommandText = "UPDATE Manufacturing_Stock SET ClosingStock = (ClosingStock - (" +
                                         dr["Qty"].ToString().Replace(",", "") +
                                         "/isnull(SFT_Per_Role,0))), ClosingAmount = (ClosingAmount -((" +
                                         dr["Qty"].ToString().Replace(",", "") +
                                         "/isnull(SFT_Per_Role,0))* " + dr["Total"].ToString().Replace(",", "") + ")) WHERE ID='" + dr["ID"].ToString() + "'";



                        command.ExecuteNonQuery();
                    }

                    else if (dr["msr_unit_code"].ToString().Replace(",", "") == "R")
                    {
                        command.CommandText = "UPDATE Manufacturing_Stock SET ClosingStock = (ClosingStock - (" +
                                             dr["Qty"].ToString().Replace(",", "") +
                                             ")), ClosingAmount = (ClosingAmount -(" +
                                             dr["Qty"].ToString().Replace(",", "") +
                                             ")* " + dr["Total"].ToString().Replace(",", "") + ") WHERE ID='" + dr["ID"].ToString() +
                                             "'";
                        command.ExecuteNonQuery();
                    }




                    //








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

      public static void SaveSalesInfo(Sales aSales, DataTable dt,string salesOrderId)
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

            command.CommandText = @"INSERT INTO [Order]
           ([InvoiceNo],[SubTotal],[TaxAmount],[DiscountAmount],[GrandTotal],[CashReceived],[CashRefund],[OrderDate],[CustomerID],[Due],[DeliveryStatus],[DeliveryDate],[Remark],[CreatedBy],[CreatedDate],DriverName,DriverPhoneNo,CarNo,OutTime)
     VALUES
           ('" + aSales.Invoice + "','" + aSales.Total + "','" + aSales.Tax + "','" + aSales.Disount + "','" + aSales.GTotal + "','" + aSales.CReceive + "','0',convert(datetime, nullif( '" + aSales.Date + "',''), 103),'" + aSales.Customer + "','" + aSales.Due + "','" + aSales.DvStatus + "',convert(datetime, nullif( '" + aSales.DvDate + "',''), 103),'" + aSales.Remarks + "','" + aSales.LoginBy + "',GETDATE(),'" + aSales.DriverName + "','" + aSales.DriverPhoneNo + "','" + aSales.CarNo + "','" + aSales.OutTime + "')";
            command.ExecuteNonQuery();






            command.CommandText = @"Update ItemSalesOrderMst set OrderStatus='C' where Id='" + salesOrderId + "'";
            command.ExecuteNonQuery();



            command.CommandText = @"SELECT top(1) [ID]  FROM [Order] order by ID desc";
            string OrderMstID = command.ExecuteScalar().ToString();


            //*************************** Customer Payment ******************//
            decimal totPay = 0;
            if (aSales.PMethod == "C" && Convert.ToDouble(aSales.CReceive) > 0)
            {
                command.CommandText = @"INSERT INTO [CustomerPaymentReceive]
           ([Date],[Customer_id],[Invoice],[PayAmt],[PayMethod],[Bank_id],[ChequeNo],[ChequeDate],[entry_by],[entry_date],Payment_Type)
     VALUES
           (CONVERT(DATE,'" + aSales.Date + "',103),'" + aSales.Customer + "','" + OrderMstID + "','" + aSales.CReceive + "','" + aSales.PMethod + "','" + aSales.BankId + "','" + aSales.PMNumber + "',CONVERT(DATE,'" + aSales.ChequeDate + "',103),'" + aSales.LoginBy + "',GETDATE(),'IV')";
                command.ExecuteNonQuery();
            }

            if (aSales.PMethod == "Q")
            {
                if (Convert.ToDecimal(aSales.CReceive) == 0) { totPay = Convert.ToDecimal(aSales.GTotal); } else { totPay = Convert.ToDecimal(aSales.CReceive); }
                command.CommandText = @"INSERT INTO [CustomerPaymentReceive]
           ([Date],[Customer_id],[Invoice],[PayAmt],[PayMethod],[Bank_id],[ChequeNo],[ChequeDate],[entry_by],[entry_date],Chk_Status,Payment_Type)
     VALUES
           (CONVERT(DATE,'" + aSales.Date + "',103),'" + aSales.Customer + "','" + OrderMstID + "','" + totPay + "','" + aSales.PMethod + "','" + aSales.BankId + "','" + aSales.PMNumber + "',CONVERT(DATE,'" + aSales.ChequeDate + "',103),'" + aSales.LoginBy + "',GETDATE(),'" + aSales.Chk_Status + "','IV')";
                command.ExecuteNonQuery();

            }
            if (Convert.ToDecimal(aSales.CReceive) > 0 && aSales.PMethod == "CR")
            {
                command.CommandText = @"INSERT INTO [CustomerPaymentReceive]
           ([Date],[Customer_id],[Invoice],[PayAmt],[PayMethod],[Bank_id],[ChequeNo],[ChequeDate],[entry_by],[entry_date],Payment_Type)
     VALUES
           (CONVERT(DATE,'" + aSales.Date + "',103),'" + aSales.Customer + "','" + OrderMstID + "','" + aSales.CReceive + "','" + aSales.PMethod + "','" + aSales.BankId + "','" + aSales.PMNumber + "',CONVERT(DATE,'" + aSales.ChequeDate + "',103),'" + aSales.LoginBy + "',GETDATE(),'IV')";
                command.ExecuteNonQuery();
            }
            //***************************  ********************************// 
            foreach (DataRow dr in dt.Rows)
            {
                if (dr["Code"].ToString() != "")
                {
                    command.CommandText = @"INSERT INTO [OrderDetail]
           ([OrderID],[ItemID],[UnitPrice]  ,[TaxRate] ,[DiscountAmount] ,[SalePrice] ,[Quantity],PackSize,RollOrCftFlag ,[TotalPrice] ,[CreatedBy],[CreatedDate])
     VALUES
           ('" + OrderMstID + "','" + dr["ID"].ToString() + "','" + dr["SPrice"].ToString().Replace(",", "") + "','" + dr["Tax"].ToString().Replace(",", "") + "','" + dr["DiscountAmount"].ToString().Replace(",", "") + "','" + dr["SPrice"].ToString().Replace(",", "") + "','" + dr["Qty"].ToString().Replace(",", "") + "','" + dr["PackSize"].ToString().Replace(",", "") + "','" + dr["msr_unit_code"].ToString().Replace(",", "") + "','" + dr["Total"].ToString().Replace(",", "") + "','" + aSales.LoginBy + "',GETDATE())";
                    command.ExecuteNonQuery();







                    //New 

                    if (dr["msr_unit_code"].ToString().Replace(",", "") == "C")
                    {
                        command.CommandText = "UPDATE Manufacturing_Stock SET ClosingStock = (ClosingStock - (" +
                                              dr["Qty"].ToString().Replace(",", "") +
                                              "/isnull(CFT_Per_Role,0))), ClosingAmount = (ClosingAmount -((" +
                                              dr["Qty"].ToString().Replace(",", "") +
                                              "/isnull(CFT_Per_Role,0))* " + dr["Total"].ToString().Replace(",", "") + ")) WHERE ID='" + dr["ID"].ToString() +"'";
                        command.ExecuteNonQuery();
                    }
                    else if (dr["msr_unit_code"].ToString().Replace(",", "") == "SQ")
                    {
                        

                        command.CommandText = "UPDATE Manufacturing_Stock SET ClosingStock = (ClosingStock - (" +
                                             dr["Qty"].ToString().Replace(",", "") +
                                             "/isnull(SQM_Per_Role,0))), ClosingAmount = (ClosingAmount -((" +
                                             dr["Qty"].ToString().Replace(",", "") +
                                             "/isnull(SQM_Per_Role,0))* " + dr["Total"].ToString().Replace(",", "") + ")) WHERE ID='" + dr["ID"].ToString() + "'";

                        command.ExecuteNonQuery();
                    }

                    else if (dr["msr_unit_code"].ToString().Replace(",", "") == "SF")
                    {

                        command.CommandText = "UPDATE Manufacturing_Stock SET ClosingStock = (ClosingStock - (" +
                                         dr["Qty"].ToString().Replace(",", "") +
                                         "/isnull(SFT_Per_Role,0))), ClosingAmount = (ClosingAmount -((" +
                                         dr["Qty"].ToString().Replace(",", "") +
                                         "/isnull(SFT_Per_Role,0))* " + dr["Total"].ToString().Replace(",", "") + ")) WHERE ID='" + dr["ID"].ToString() + "'";



                        command.ExecuteNonQuery();
                    }

                    else if (dr["msr_unit_code"].ToString().Replace(",", "") == "R")
                    {
                        command.CommandText = "UPDATE Manufacturing_Stock SET ClosingStock = (ClosingStock - (" +
                                             dr["Qty"].ToString().Replace(",", "") +
                                             ")), ClosingAmount = (ClosingAmount -(" +
                                             dr["Qty"].ToString().Replace(",", "") +
                                             ")* " + dr["Total"].ToString().Replace(",", "") + ") WHERE ID='" + dr["ID"].ToString() +
                                             "'";
                        command.ExecuteNonQuery();
                    }




                    //








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

    public static void SaveSalesOrderInfo(Sales aSales, DataTable dt)
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

            command.CommandText = @"INSERT INTO [SalesOrder]
           ([InvoiceNo],[SubTotal],[TaxAmount],[DiscountAmount],[GrandTotal],[CashReceived],[CashRefund],[OrderDate],[CustomerID],[Due],[DeliveryStatus],[DeliveryDate],[Remark],[CreatedBy],[CreatedDate],DriverName,DriverPhoneNo,CarNo,OutTime)
     VALUES
           ('" + aSales.Invoice + "','" + aSales.Total + "','" + aSales.Tax + "','" + aSales.Disount + "','" + aSales.GTotal + "','" + aSales.CReceive + "','0',convert(datetime, nullif( '" + aSales.Date + "',''), 103),'" + aSales.Customer + "','" + aSales.Due + "','" + aSales.DvStatus + "',convert(datetime, nullif( '" + aSales.DvDate + "',''), 103),'" + aSales.Remarks + "','" + aSales.LoginBy + "',GETDATE(),'" + aSales.DriverName + "','" + aSales.DriverPhoneNo + "','" + aSales.CarNo + "','" + aSales.OutTime + "')";
            command.ExecuteNonQuery();

            command.CommandText = @"SELECT top(1) [ID]  FROM [SalesOrder] order by ID desc";
            string OrderMstID = command.ExecuteScalar().ToString();


           
            foreach (DataRow dr in dt.Rows)
            {
                if (dr["Code"].ToString() != "")
                {
                    command.CommandText = @"INSERT INTO [SalseOrderDetail]
           ([OrderID],[ItemID],[UnitPrice]  ,[TaxRate] ,[DiscountAmount] ,[SalePrice] ,[Quantity],PackSize,RollOrCftFlag ,[TotalPrice] ,[CreatedBy],[CreatedDate])
     VALUES
           ('" + OrderMstID + "','" + dr["ID"].ToString() + "','" + dr["SPrice"].ToString().Replace(",", "") + "','" + dr["Tax"].ToString().Replace(",", "") + "','" + dr["DiscountAmount"].ToString().Replace(",", "") + "','" + dr["SPrice"].ToString().Replace(",", "") + "','" + dr["Qty"].ToString().Replace(",", "") + "','" + dr["PackSize"].ToString().Replace(",", "") + "','" + dr["msr_unit_code"].ToString().Replace(",", "") + "','" + dr["Total"].ToString().Replace(",", "") + "','" + aSales.LoginBy + "',GETDATE())";
                    command.ExecuteNonQuery();
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
    public static void UpdateSalesInfo(Sales aSales, DataTable dt)
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

            command.CommandText = @"update [Order] set
           [SubTotal]='" + aSales.Total + "',[TaxAmount]='" + aSales.Tax + "',[DiscountAmount]='" + aSales.Disount + "',[GrandTotal]='" + aSales.GTotal + "',[CashReceived]='" + aSales.CReceive + "',[CashRefund]='0',[OrderDate]=convert(date,'" + aSales.Date + "',103),[CustomerID]='" + aSales.Customer + "',[Due]='" + aSales.Due + "',[DeliveryStatus]='" + aSales.DvStatus + "',[DeliveryDate]=convert(date,'" + aSales.DvDate + "',103),[Remark]='" + aSales.Remarks + "',[ModifiedBy]='" + aSales.LoginBy + "',[ModifiedDate]=GETDATE(),DriverName='"+aSales.DriverName+"',DriverPhoneNo='"+aSales.DriverPhoneNo+"',CarNo='"+aSales.CarNo+"' where ID='" + aSales.ID + "'";    
            command.ExecuteNonQuery();



          string  Query = @"select  ItemId,Quantity ,TotalPrice,RollOrCftFlag FROM [OrderDetail] WHERE OrderID='" + aSales.ID + "'";
            DataTable dt1 = DataManager.ExecuteQuery(DataManager.OraConnString(), Query, "ItemPurchaseMst");


            if (dt1.Rows.Count > 0)
            {
                foreach (DataRow dr in dt1.Rows)
                {
                    var a = dr["ItemId"].ToString();
                    if (dr["RollOrCftFlag"].ToString() == "C")
                    {
                        command.CommandText = @"UPDATE Manufacturing_Stock SET ClosingStock = (ClosingStock + (" + dr["Quantity"].ToString() + "/isnull(CFT_Per_Role,0))), ClosingAmount = (ClosingAmount +(" + dr["Quantity"].ToString() + "/isnull(CFT_Per_Role,0))* " + dr["TotalPrice"].ToString() + ") WHERE ID = '" + dr["ItemId"].ToString() + "'";
                        command.ExecuteNonQuery();
                    }

                    else if (dr["RollOrCftFlag"].ToString() == "SF")
                    {
                        command.CommandText = @"UPDATE Manufacturing_Stock SET ClosingStock = (ClosingStock + (" + dr["Quantity"].ToString() + "/isnull(SFT_Per_Role,0))), ClosingAmount = (ClosingAmount +(" + dr["Quantity"].ToString() + "/isnull(SFT_Per_Role,0))* " + dr["TotalPrice"].ToString() + ") WHERE ID = '" + dr["ItemId"].ToString() + "'";
                        command.ExecuteNonQuery();
                    }

                    else if (dr["RollOrCftFlag"].ToString() == "SQ")
                    {
                        command.CommandText = @"UPDATE Manufacturing_Stock SET ClosingStock = (ClosingStock + (" + dr["Quantity"].ToString() + "/isnull(SQM_Per_Role,0))), ClosingAmount = (ClosingAmount +(" + dr["Quantity"].ToString() + "/isnull(SQM_Per_Role,0))* " + dr["TotalPrice"].ToString() + ") WHERE ID = '" + dr["ItemId"].ToString() + "'";
                        command.ExecuteNonQuery();
                    }
                    else if (dr["RollOrCftFlag"].ToString() == "R")
                    {
                        command.CommandText = @"UPDATE Manufacturing_Stock SET ClosingStock = (ClosingStock + (" + dr["Quantity"].ToString() + ")), ClosingAmount = (ClosingAmount +(" + dr["Quantity"].ToString() + ")* " + dr["TotalPrice"].ToString() + ") WHERE ID = '" + dr["ItemId"].ToString() + "'";
                        command.ExecuteNonQuery();
                    }


                }
            }












            command.CommandText = @"delete from [OrderDetail] where OrderID='" + aSales.ID + "'";
            command.ExecuteNonQuery();
           
            command.CommandText = @"UPDATE [CustomerPaymentReceive]
   SET [Date] =CONVERT(DATE,'" + aSales.Date + "',103),[PayAmt] ='" + aSales.Total + "' ,[ChequeNo] ='" + aSales.PMNumber + "' ,[ChequeDate] = CONVERT(DATE,'" + aSales.ChequeDate + "',103),[update_by] ='" + aSales.LoginBy + "',[update_date] =GETDATE()  WHERE Invoice='" + aSales.ID + "' ";
            command.ExecuteNonQuery();

            foreach (DataRow dr in dt.Rows)
            {
                if (dr["Code"].ToString() != "")
                {
                    command.CommandText = @"INSERT INTO [OrderDetail]
           ([OrderID],[ItemID],[UnitPrice]  ,[TaxRate] ,[DiscountAmount] ,[SalePrice] ,[Quantity],PackSize,RollOrCftFlag ,[TotalPrice] ,[CreatedBy],[CreatedDate])
     VALUES
           ('" + aSales.ID + "','" + dr["ID"].ToString() + "','" + dr["SPrice"].ToString().Replace(",", "") + "','" + dr["Tax"].ToString().Replace(",", "") + "','" + dr["DiscountAmount"].ToString().Replace(",", "") + "','" + dr["SPrice"].ToString().Replace(",", "") + "','" + dr["Qty"].ToString().Replace(",", "") + "','" + dr["PackSize"].ToString().Replace(",", "") + "','" + dr["msr_unit_code"].ToString().Replace(",", "") + "','" + dr["Total"].ToString().Replace(",", "") + "','" + aSales.LoginBy + "',GETDATE())";
                    command.ExecuteNonQuery();
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

    public static void UpdateSalesOrderInfo(Sales aSales, DataTable dt)
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

            command.CommandText = @"update [SalesOrder] set
           [SubTotal]='" + aSales.Total + "',[TaxAmount]='" + aSales.Tax + "',[DiscountAmount]='" + aSales.Disount + "',[GrandTotal]='" + aSales.GTotal + "',[CashReceived]='" + aSales.CReceive + "',[CashRefund]='0',[OrderDate]=convert(date,'" + aSales.Date + "',103),[CustomerID]='" + aSales.Customer + "',[Due]='" + aSales.Due + "',[DeliveryStatus]='" + aSales.DvStatus + "',[DeliveryDate]=convert(date,'" + aSales.DvDate + "',103),[Remark]='" + aSales.Remarks + "',[ModifiedBy]='" + aSales.LoginBy + "',[ModifiedDate]=GETDATE(),DriverName='" + aSales.DriverName + "',DriverPhoneNo='" + aSales.DriverPhoneNo + "',CarNo='" + aSales.CarNo + "' where ID='" + aSales.ID + "'";
            command.ExecuteNonQuery();

            command.CommandText = @"delete from [SalseOrderDetail] where OrderID='" + aSales.ID + "'";
            command.ExecuteNonQuery();

//            command.CommandText = @"UPDATE [CustomerPaymentReceive]
//   SET [Date] =CONVERT(DATE,'" + aSales.Date + "',103),[PayAmt] ='" + aSales.Total + "' ,[ChequeNo] ='" + aSales.PMNumber + "' ,[ChequeDate] = CONVERT(DATE,'" + aSales.ChequeDate + "',103),[update_by] ='" + aSales.LoginBy + "',[update_date] =GETDATE()  WHERE Invoice='" + aSales.ID + "' ";
//            command.ExecuteNonQuery();

            foreach (DataRow dr in dt.Rows)
            {
                if (dr["Code"].ToString() != "")
                {
                    command.CommandText = @"INSERT INTO [SalseOrderDetail]
           ([OrderID],[ItemID],[UnitPrice]  ,[TaxRate] ,[DiscountAmount] ,[SalePrice] ,[Quantity],PackSize,RollOrCftFlag ,[TotalPrice] ,[CreatedBy],[CreatedDate])
     VALUES
           ('" + aSales.ID + "','" + dr["ID"].ToString() + "','" + dr["SPrice"].ToString().Replace(",", "") + "','" + dr["Tax"].ToString().Replace(",", "") + "','" + dr["DiscountAmount"].ToString().Replace(",", "") + "','" + dr["SPrice"].ToString().Replace(",", "") + "','" + dr["Qty"].ToString().Replace(",", "") + "','" + dr["PackSize"].ToString().Replace(",", "") + "','" + dr["msr_unit_code"].ToString().Replace(",", "") + "','" + dr["Total"].ToString().Replace(",", "") + "','" + aSales.LoginBy + "',GETDATE())";
                    command.ExecuteNonQuery();
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

    public static DataTable GetShowSalesDetails()
    {
        String connectionString = DataManager.OraConnString();
        string query = @"select t1.ID,t1.InvoiceNo,t2.ContactName as [CustomerName],CONVERT(nvarchar,t1.OrderDate,103)OrderDate,DeliveryStatus AS [Status],convert(decimal(18,2),t1.CashReceived) AS [CashReceived]  from [Order] t1 left join Customer t2 on t2.ID=t1.CustomerID order by t1.ID desc ";
        DataTable dt = DataManager.ExecuteQuery(connectionString, query, "Item");
        return dt;
    }



    public static DataTable GetShowSalesOrderDetails()
    {
        String connectionString = DataManager.OraConnString();
        string query = @"select t1.ID,t1.InvoiceNo,t2.ContactName as [CustomerName],CONVERT(nvarchar,t1.OrderDate,103)OrderDate,DeliveryStatus AS [Status],convert(decimal(18,2),t1.CashReceived) AS [CashReceived]  from [SalesOrder] t1 left join Customer t2 on t2.ID=t1.CustomerID order by t1.ID desc ";
        DataTable dt = DataManager.ExecuteQuery(connectionString, query, "Item");
        return dt;
    }

    public static DataTable GetSalesDetails(string OrderMstId)
    {
        String connectionString = DataManager.OraConnString();
        string query = @"select st.ID,st.Code,t2.Name,convert(decimal(18,2),t1.TaxRate) AS Tax,convert(decimal(18,2),t1.DiscountAmount) AS DiscountAmount,
convert(decimal(18,2),t1.UnitPrice) AS SPrice,convert(decimal(18,2),t1.Quantity)  AS Qty,convert(decimal(18,2),t1.PackSize) as PackSize,
 CONVERT(decimal(18, 2), st.ClosingStock) AS  ClosingStock,
 CONVERT(decimal(18, 4), st.CFT_Per_Role * CONVERT(decimal(18, 4), st.ClosingStock)) AS SalesClosingStock,
   CONVERT(decimal(18, 4), isnull(st.SQM_Per_Role,0) * CONVERT(decimal(18, 4), st.ClosingStock)) as SqmClosingStock,
						   CONVERT(decimal(18, 4), isnull(st.SFT_Per_Role,0) * CONVERT(decimal(18, 4), st.ClosingStock)) as SftClosingStock,
 convert(decimal(18,2),TotalPrice) AS Total,t3.BrandName,case when t1.RollOrCftFlag='C' then 'C'  when t1.RollOrCftFlag='SQ' THEN 'SQ' when t1.RollOrCftFlag='SF' THEN 'SF'  else 'R' end as msr_unit_code  
--'C' as msr_unit_code
,T5.Name as UOM  ,'' as OrderQty 
from OrderDetail t1 inner join Manufacturing_Stock st on st.ID=t1.ItemID left join Item t2 on t2.ID=st.ItemID left join Brand t3 on t3.ID=t2.Brand left join UOM  t5 on t5.Id=t2.uomId  LEFT OUTER JOIN
 dbo.ProductionLength AS l ON l.ID = st.lengthID LEFT OUTER JOIN
                         dbo.ProductionWidth AS w ON w.ID = st.widthID LEFT OUTER JOIN
                         dbo.ProductionThikness AS t ON t.ID = st.thiknessID where t1.OrderID='" + OrderMstId + "'";
        DataTable dt = DataManager.ExecuteQuery(connectionString, query, "OrderDetail");
        return dt;
    }

    public static void DeleteSalesVoucher(Sales aSales)
    {
        string connectionString = DataManager.OraConnString();
        SqlConnection connection = new SqlConnection(DataManager.OraConnString());
        SqlTransaction transaction;
        try
        {
            connection.Open();

            transaction = connection.BeginTransaction();
            SqlCommand command = new SqlCommand();
            command.Connection = connection;
            command.Transaction = transaction;

            string Query = @"select t1.VCH_SYS_NO  from [GL_TRANS_MST] t1 where SERIAL_NO='" + aSales.Invoice + "'";
            DataTable dt = DataManager.ExecuteQuery(connectionString, Query, "ItemPurchaseMst");

            command.CommandText = @"DELETE FROM [GL_TRANS_MST]  WHERE SERIAL_NO='" + aSales.Invoice + "'";
            command.ExecuteNonQuery();

            foreach (DataRow dr in dt.Rows)
            {
                command.CommandText = @"DELETE FROM [GL_TRANS_DTL]  WHERE VCH_SYS_NO='" + dr["VCH_SYS_NO"].ToString() + "'";
                command.ExecuteNonQuery();
            }

            command.CommandText = @"DELETE FROM [Order] WHERE  ID='" + aSales.ID + "'";
            command.ExecuteNonQuery();


            //triger not working so implement this code


            Query = @"select  ItemId,Quantity ,TotalPrice,RollOrCftFlag FROM [OrderDetail] WHERE OrderID='" + aSales.ID + "'";
            DataTable dt1 = DataManager.ExecuteQuery(connectionString, Query, "ItemPurchaseMst");


            if (dt1.Rows.Count > 0)
            {
                foreach (DataRow dr in dt1.Rows)
                {
                    var a = dr["ItemId"].ToString();
                    if (dr["RollOrCftFlag"].ToString() == "C")
                    {
                        command.CommandText = @"UPDATE Manufacturing_Stock SET ClosingStock = (ClosingStock + (" + dr["Quantity"].ToString() + "/isnull(CFT_Per_Role,0))), ClosingAmount = (ClosingAmount +(" + dr["Quantity"].ToString() + "/isnull(CFT_Per_Role,0))* " + dr["TotalPrice"].ToString() + ") WHERE ID = '" + dr["ItemId"].ToString() + "'";
                        command.ExecuteNonQuery();
                    }

                    else if (dr["RollOrCftFlag"].ToString() == "SF")
                    {
                        command.CommandText = @"UPDATE Manufacturing_Stock SET ClosingStock = (ClosingStock + (" + dr["Quantity"].ToString() + "/isnull(SFT_Per_Role,0))), ClosingAmount = (ClosingAmount +(" + dr["Quantity"].ToString() + "/isnull(SFT_Per_Role,0))* " + dr["TotalPrice"].ToString() + ") WHERE ID = '" + dr["ItemId"].ToString() + "'";
                        command.ExecuteNonQuery();
                    }

                    else if (dr["RollOrCftFlag"].ToString() == "SQ")
                    {
                        command.CommandText = @"UPDATE Manufacturing_Stock SET ClosingStock = (ClosingStock + (" + dr["Quantity"].ToString() + "/isnull(SQM_Per_Role,0))), ClosingAmount = (ClosingAmount +(" + dr["Quantity"].ToString() + "/isnull(SQM_Per_Role,0))* " + dr["TotalPrice"].ToString() + ") WHERE ID = '" + dr["ItemId"].ToString() + "'";
                        command.ExecuteNonQuery();
                    }
                    else if (dr["RollOrCftFlag"].ToString() == "R")
                    {
                        command.CommandText = @"UPDATE Manufacturing_Stock SET ClosingStock = (ClosingStock + (" + dr["Quantity"].ToString() + ")), ClosingAmount = (ClosingAmount +(" + dr["Quantity"].ToString() + ")* " + dr["TotalPrice"].ToString() + ") WHERE ID = '" + dr["ItemId"].ToString() + "'";
                        command.ExecuteNonQuery();
                    }


                }
            }




            //

           



            command.CommandText = @"DELETE FROM [OrderDetail] WHERE OrderID='" + aSales.ID + "'";
            command.ExecuteNonQuery();

            command.CommandText = @"DELETE FROM [CustomerPaymentReceive] WHERE Invoice='" + aSales.ID + "' AND Payment_Type ='IV'";
            command.ExecuteNonQuery();

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

    public static void DeleteSalesOrderVoucher(Sales aSales)
    {
        string connectionString = DataManager.OraConnString();
        SqlConnection connection = new SqlConnection(DataManager.OraConnString());
        SqlTransaction transaction;
        try
        {
            connection.Open();

            transaction = connection.BeginTransaction();
            SqlCommand command = new SqlCommand();
            command.Connection = connection;
            command.Transaction = transaction;




            command.CommandText = @"DELETE FROM [SalesOrder] WHERE  ID='" + aSales.ID + "'";
            command.ExecuteNonQuery();

            command.CommandText = @"DELETE FROM [SalseOrderDetail] WHERE OrderID='" + aSales.ID + "'";
            command.ExecuteNonQuery();

         

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

    public static void UpdatePrintStatus(string ID, int Type)
    {
        string connectionString = DataManager.OraConnString();
        SqlConnection oracon = new SqlConnection(connectionString);
        string query = @"update [FixGlCoaCode] set [PrintOrderID]=" + ID + ",PrintType=" + Type + "";
        DataManager.ExecuteNonQuery(connectionString, query);
    }
}