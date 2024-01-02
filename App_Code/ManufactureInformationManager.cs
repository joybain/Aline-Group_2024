using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using autouniv;
using System.Data.SqlClient;
using System.Data;

/// <summary>
/// Summary description for ManufactureInformationManager
/// </summary>
public class ManufactureInformationManager
{
	public ManufactureInformationManager()
	{
		//
		// TODO: Add constructor logic here
		//
	}

    public static ManufactureInformation GetManufactureInformation(string ID)
    {
        string connectionString = DataManager.OraConnString();
        SqlConnection sqlCon = new SqlConnection(connectionString);
        string query = @"SELECT [ID]
      ,[GenerateCode]   ,[Remarks] ,[ManufagchuredBy] ,Convert(nvarchar,[ManufagchuredDate],103) as ManufagchuredDate,[ProductionCode],[ProductionSerial],[ProductionParentCode],[ProductionID]
  FROM [Manufacturing_Master] t1  Where t1.ID='" + ID + "' ";
        DataTable dt = DataManager.ExecuteQuery(connectionString, query, "Manufacturing_Master");
        sqlCon.Close();
        if (dt.Rows.Count == 0)
        {
            return null;
        }
        return new ManufactureInformation(dt.Rows[0]);
    }

    public static string SaveManufactureInformation(ManufactureInformation aManufactureInformation, DataTable Received, DataTable consumption, out string ManufactureCode, out string MstID)
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


            List<SqlParameter> parameters = new List<SqlParameter>();


            parameters.Add(new SqlParameter("@ManufagchuredBy", aManufactureInformation.ManufagchuredBy));
            parameters.Add(new SqlParameter("@ManufagchuredDate", aManufactureInformation.ManufagchuredDate));
            parameters.Add(new SqlParameter("@Remarks", aManufactureInformation.Remarks));
            parameters.Add(new SqlParameter("@ProductionCode", aManufactureInformation.ProductionCode));
            parameters.Add(new SqlParameter("@ProductionParentCode", aManufactureInformation.ProductionParentCode));
            parameters.Add(new SqlParameter("@ProductionSerial", aManufactureInformation.ProductionSerial));
            parameters.Add(new SqlParameter("@ProductionID", aManufactureInformation.ProductionID));
            parameters.Add(new SqlParameter("@UserID", aManufactureInformation.AddBy));
            parameters.Add(new SqlParameter("@masterID", null));
            parameters.Add(new SqlParameter("@ReceivedType", Received));
            parameters.Add(new SqlParameter("@ConsumptionType", consumption));
            var Code = new SqlParameter("@GenerateCode", SqlDbType.VarChar, 50)
            {
                Direction = ParameterDirection.Output
            };

            var AID = new SqlParameter("@MstID", SqlDbType.VarChar, 50)
            {
                Direction = ParameterDirection.Output
            };
            parameters.Add(Code);
            parameters.Add(AID);

            int Record = DataManager.ExecuteNonQuery(DataManager.OraConnString(),"SP_InsertManufactureType", parameters);
            MstID = AID.Value.ToString();
            ManufactureCode = Code.Value.ToString();
            return MstID;

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

    public static void UpdateManufactureInformation(ManufactureInformation aManufactureInformation, DataTable Received, DataTable consumption, string MstID)
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


            List<SqlParameter> parameters = new List<SqlParameter>();


            parameters.Add(new SqlParameter("@ManufagchuredBy", aManufactureInformation.ManufagchuredBy));
            parameters.Add(new SqlParameter("@ManufagchuredDate", aManufactureInformation.ManufagchuredDate));
            parameters.Add(new SqlParameter("@Remarks", aManufactureInformation.Remarks));
            parameters.Add(new SqlParameter("@ProductionCode", aManufactureInformation.ProductionCode));
            parameters.Add(new SqlParameter("@ProductionParentCode", aManufactureInformation.ProductionParentCode));
            parameters.Add(new SqlParameter("@ProductionSerial", aManufactureInformation.ProductionSerial));
            parameters.Add(new SqlParameter("@ProductionID", aManufactureInformation.ProductionID));
            parameters.Add(new SqlParameter("@UserID", aManufactureInformation.AddBy));
            parameters.Add(new SqlParameter("@masterID", aManufactureInformation.ID));
            if (Received.Rows.Count > 0)
            {
                parameters.Add(new SqlParameter("@ReceivedType", Received));
            }
            else
            {
                parameters.Add(new SqlParameter("@ReceivedType", null));
            }
            parameters.Add(new SqlParameter("@ConsumptionType", consumption));
            var Code = new SqlParameter("@GenerateCode", SqlDbType.VarChar, 50)
            {
                Direction = ParameterDirection.Output
            };

            var AID = new SqlParameter("@MstID", SqlDbType.VarChar, 50)
            {
                Direction = ParameterDirection.Output
            };
            parameters.Add(Code);
            parameters.Add(AID);
            string ManufactureCode = "";
            int Record = DataManager.ExecuteNonQuery(DataManager.OraConnString(), "SP_InsertManufactureType", parameters);
            MstID = AID.Value.ToString();
            ManufactureCode = Code.Value.ToString();

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

    public static DataTable GetInformationHistory(string ID, string ProductionCode)
    {
        string connectionString = DataManager.OraConnString();
        SqlConnection sqlCon = new SqlConnection(connectionString);
        string parameter = "where t1.DeleteBy is null";
        if (ID != "")
        {
            parameter = parameter + " and  t1.ID ='" + ID + "' ";
        }
        else if (ProductionCode != "")
        {
            parameter = parameter + " and  t1.ProductionCode ='" + ProductionCode + "' ";
        }
        string query = @"SELECT * from View_ManufactureMasterInformation t1  " + parameter;
        DataTable dt = DataManager.ExecuteQuery(connectionString, query, "View_ManufactureMasterInformationt");
        sqlCon.Close();
        return dt;
    }

    public static DataTable GetReceivedDetailsInfo(string ID)
    {
        string connectionString = DataManager.OraConnString();
        SqlConnection sqlCon = new SqlConnection(connectionString);
        string parameter = "where t1.DeleteBy is null";
        if (ID != "")
        {
            parameter = parameter + " and  t1.ID ='" + ID + "' ";
        }

        string query = @"SELECT  t2.[ID]
      ,[RawMatarials_ItemID] as ItemID
      ,[lengthID] as LengthID
      ,[widthID] as WidthID
      ,[thiknessID] as ThiknessID      
      ,t1.Code+' - '+t1.Name as Name
      ,[length] as [Length]
      ,[width] as Width
      ,[thikness] as Thikness
      ,[Quantity] as Quantity 
      ,[ProductionCode] as ProductionCode
      ,[ProductionParentCode] as ParentCode
      ,[ProductionSerial] as ProductionSerialNo
      ,'' as FinalStatge
      ,[Goods_ItemID] as Goods_ItemID,Weight
      
  FROM [ManufacturedReceived] t2 inner join  dbo.Item AS t1 on t2.[RawMatarials_ItemID]=t1.ID
WHERE     t2.MasterID='" + ID + "'  ";
        DataTable dt = DataManager.ExecuteQuery(connectionString, query, "View_ManufactureMasterInformationt");
        sqlCon.Close();
        return dt;
    }


    public static DataTable GetReceivedDetailsAllInfo(string ID)
    {
        string connectionString = DataManager.OraConnString();
        SqlConnection sqlCon = new SqlConnection(connectionString);
        string parameter = "where t1.DeleteBy is null";
        if (ID != "")
        {
            parameter = parameter + " and  t1.ID ='" + ID + "' ";
        }

        string query = @"SELECT  t2.[ID] ,[RawMatarials_ItemID] as ItemID ,[lengthID] as LengthID ,[widthID] as WidthID ,[thiknessID] as ThiknessID ,t1.Code+' - '+t1.Name as Name ,isnull([length],0) as [Length] ,isnull([width],0) as Width ,isnull([thikness],0) as Thikness ,[Quantity] as Quantity ,[ProductionCode] as ProductionCode ,[ProductionParentCode] as ParentCode ,[ProductionSerial] as ProductionSerialNo ,'' as FinalStatge ,[Goods_ItemID] as Goods_ItemID ,t5.Name as UOM,isnull(t6.BrandName,'') as BrandName FROM [ManufacturedReceived] t2 inner join  dbo.Item AS t1 on t2.[RawMatarials_ItemID]=t1.ID left join UOM  t5 on t5.Id=t1.uomId left join Brand t6 on t6.Id=t1.Brand
WHERE     t2.MasterID='" + ID + "'  ";
        DataTable dt = DataManager.ExecuteQuery(connectionString, query, "View_ManufactureMasterInformationt");
        sqlCon.Close();
        return dt;
    }

    public static DataTable GetConsumptionDetailsInfo(string ID,string ProductionCode)
    {
        string connectionString = DataManager.OraConnString();
        SqlConnection sqlCon = new SqlConnection(connectionString);
        string parameter = "where t1.DeleteBy is null";
        if (ID != "")
        {
            parameter = parameter + " and  t1.ID ='" + ID + "' ";
        }
        string query = "";
        if(ProductionCode=="0")

            query = @"SELECT  t2.[ID]
      ,[RawMatarials_ItemID] as ItemID
      ,[lengthID] as LengthID
      ,[widthID] as WidthID
      ,[thiknessID] as ThiknessID      
      ,t1.Code+' - '+t1.Name as Name
      ,[length] as [Length]
      ,[width] as Width
      ,[thikness] as Thikness
      ,[Quantity] as Quantity 
      ,[ProductionCode] as ProductionCode
      ,[ProductionParentCode] as ParentCode
      ,[ProductionSerial] as ProductionSerialNo
      
  FROM [ManufacturedDetails] t2 inner join  dbo.Item AS t1 on t2.[RawMatarials_ItemID]=t1.ID
WHERE     t2.MasterID='" + ID + "'  ";

        else
             query = @"SELECT  t1.[ID] ,t1.[RawMatarials_ItemID] as ItemID ,t1.[lengthID] as LengthID ,t1.[widthID] as WidthID ,t1.[thiknessID] as ThiknessID ,t3.Code+' - '+t3.Name as Name ,t1.[length] as [Length] ,t1.[width] as Width ,t1.[thikness] as Thikness ,t1.[Quantity] as Quantity ,t1.[ProductionCode] as ProductionCode ,t1.[ProductionParentCode] as ParentCode ,t1.[ProductionSerial] as ProductionSerialNo FROM [ManufacturedDetails] t1 inner join Manufacturing_Stock t2 on t1.[RawMatarials_ItemID]=t2.Id inner join   dbo.Item AS t3 on t2.ItemId=t3.Id WHERE  t1.MasterID='" + ID + "'  ";

        DataTable dt = DataManager.ExecuteQuery(connectionString, query, "View_ManufactureMasterInformationt");
        sqlCon.Close();
        return dt;

    }



    public static DataTable GetConsumptionDetailsAllInfo(string ID)
    {
        string connectionString = DataManager.OraConnString();
        SqlConnection sqlCon = new SqlConnection(connectionString);
        string parameter = "where t1.DeleteBy is null";
        if (ID != "")
        {
            parameter = parameter + " and  t1.ID ='" + ID + "' ";
        }


//        string query = @"SELECT  t2.[ID] ,[RawMatarials_ItemID] as ItemID ,[lengthID] as LengthID ,[widthID] as WidthID ,[thiknessID] as ThiknessID ,t1.Code+' - '+t1.Name as Name ,isnull([length],0) as [Length] ,isnull([width],0) as Width ,isnull([thikness],0) as Thikness ,[Quantity] as Quantity ,[ProductionCode] as ProductionCode ,[ProductionParentCode] as ParentCode ,[ProductionSerial] as ProductionSerialNo ,t5.Name as UOM,isnull(t6.BrandName,'') as BrandName FROM [ManufacturedDetails] t2 inner join  dbo.Item AS t1 on t2.[RawMatarials_ItemID]=t1.ID left join UOM  t5 on t5.Id=t1.uomId left join Brand t6 on t6.Id=t1.Brand
//WHERE     t2.MasterID='" + ID + "'  ";
        string query = @"SELECT t2.[ID] ,[RawMatarials_ItemID] as ItemID ,T2.[lengthID] as LengthID ,T2.[widthID] as WidthID ,T2.[thiknessID] as ThiknessID ,t1.Code+' - '+t1.Name as Name ,isnull(T2.[length],0) as [Length] ,isnull(t2.[width],0) as Width ,isnull(t2.[thikness],0) as Thikness ,[Quantity] as Quantity ,t2.[ProductionCode] as ProductionCode ,[ProductionParentCode] as ParentCode , [ProductionSerial] as ProductionSerialNo ,t5.Name as UOM,isnull(t6.BrandName,'') as BrandName FROM [ManufacturedDetails] t2 inner join Manufacturing_Stock mnfs on t2.[RawMatarials_ItemID]=mnfs.Id inner join dbo.Item AS t1 on mnfs.ItemID=t1.ID left join UOM  t5 on t5.Id=t1.uomId left join Brand t6 on t6.Id=t1.Brand WHERE     t2.MasterID='" + ID + "'  ";
        DataTable dt = DataManager.ExecuteQuery(connectionString, query, "View_ManufactureMasterInformationt");
        sqlCon.Close();
        return dt;

    }

    public static DataTable GetManufactureStock(string parameter, string ProductionCode)
    {
        string connectionString = DataManager.OraConnString();
        SqlConnection sqlCon = new SqlConnection(connectionString);
        string query = @"SELECT * from dbo.View_ManuafactureItemSerach t1 where ProductionCode='" + ProductionCode + "' ";
        DataTable dt = DataManager.ExecuteQuery(connectionString, query, "View_ManuafactureItemSerach");
        sqlCon.Close();
        return dt;
    }


    public static void UpdateItemBondingRequisitionMst(string code)
    {
        var connction = new SqlConnection(DataManager.OraConnString());
        connction.Open();
        string query="update ItemBondingRequisitionMst set Status=1 where Code='"+code+
        "'";
        var command = new SqlCommand(query, connction);
        command.ExecuteNonQuery();
    }
}