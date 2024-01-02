using System;
using System.Data;
using System.Configuration;
using System.Linq;






using System.Xml.Linq;
using System.Data.SqlClient;
using autouniv;


/// <summary>
/// Summary description for clsMeasureManager
/// </summary>
/// 
namespace OldColor
{
    public class clsMeasureManager
    {
        public static DataTable GetMeasures()
        {
            string connectionString = DataManager.OraConnString();
            SqlConnection sqlCon = new SqlConnection(connectionString);
            string query = "select ID AS msr_unit_code, Name AS msr_unit_desc from UOM WHERE DeleteBy IS NULL order by ID DESC ";
            DataTable dt = DataManager.ExecuteQuery(connectionString, query, "UOM");
            return dt;
        }

        public static DataTable Getinfo(string showfiled,string table,string Parameter)
        {
            string connectionString = DataManager.OraConnString();
            SqlConnection sqlCon = new SqlConnection(connectionString);
            string query = "select " + showfiled + " from " + table + " " + Parameter;
            DataTable dt = DataManager.ExecuteQuery(connectionString, query, table);
            return dt;
        }
        public static void CreateMeasure(clsMeasure msr)
        {
            String connectionString = DataManager.OraConnString();
            SqlConnection sqlCon = new SqlConnection(connectionString);
            string query = " insert into UOM (Name,Active,[AddBy],[AddDate]) values ('" + msr.MsrUnitDesc + "','True','" + msr.LoginBy + "','" + Globals._localTime.ToString() + "')";
            DataManager.ExecuteNonQuery(connectionString, query);
            sqlCon.Close();
        }

        public static void Createlength(clsMeasure msr)
        {
            String connectionString = DataManager.OraConnString();
            SqlConnection sqlCon = new SqlConnection(connectionString);
            string query = " insert into dbo.ProductionLength (Length,[AddBy],[AddDate]) values ('" + msr.MsrUnitDesc + "','" + msr.LoginBy + "','" + Globals._localTime.ToString() + "')";
            DataManager.ExecuteNonQuery(connectionString, query);
            sqlCon.Close();
        }

        public static void CreateWidth(clsMeasure msr)
        {
            String connectionString = DataManager.OraConnString();
            SqlConnection sqlCon = new SqlConnection(connectionString);
            string query = " insert into dbo.ProductionWidth (Width,[AddBy],[AddDate]) values ('" + msr.MsrUnitDesc + "','" + msr.LoginBy + "','" + Globals._localTime.ToString() + "')";
            DataManager.ExecuteNonQuery(connectionString, query);
            sqlCon.Close();
        }


        public static void CreateThikness(clsMeasure msr)
        {
            String connectionString = DataManager.OraConnString();
            SqlConnection sqlCon = new SqlConnection(connectionString);
            string query = " insert into dbo.ProductionThikness (Thikness,[AddBy],[AddDate]) values ('" + msr.MsrUnitDesc + "','" + msr.LoginBy + "','" + Globals._localTime.ToString() + "')";
            DataManager.ExecuteNonQuery(connectionString, query);
            sqlCon.Close();
        }

        public static void UpdateMeasure(clsMeasure msr)
        {
            String connectionString = DataManager.OraConnString();
            SqlConnection sqlCon = new SqlConnection(connectionString);
            string query = " update UOM set Name='" + msr.MsrUnitDesc + "',[UpdateBy]='" + msr.LoginBy + "',[UpdateDate]='" + Globals._localTime.ToString() + "' where ID='" + msr.MsrUnitCode + "'";
            DataManager.ExecuteNonQuery(connectionString, query);
            sqlCon.Close();
        }
        public static void Updatelength(clsMeasure msr)
        {
            String connectionString = DataManager.OraConnString();
            SqlConnection sqlCon = new SqlConnection(connectionString);
            string query = " update ProductionLength set Length='" + msr.MsrUnitDesc + "',[UpdateBy]='" + msr.LoginBy + "',[UpdateDate]='" + Globals._localTime.ToString() + "' where ID='" + msr.MsrUnitCode + "'";
            DataManager.ExecuteNonQuery(connectionString, query);
            sqlCon.Close();
        }

        public static void Updatewidth(clsMeasure msr)
        {
            String connectionString = DataManager.OraConnString();
            SqlConnection sqlCon = new SqlConnection(connectionString);
            string query = " update ProductionWidth set Width='" + msr.MsrUnitDesc + "',[UpdateBy]='" + msr.LoginBy + "',[UpdateDate]='" + Globals._localTime.ToString() + "' where ID='" + msr.MsrUnitCode + "'";
            DataManager.ExecuteNonQuery(connectionString, query);
            sqlCon.Close();
        }
        public static void UpdateThilness(clsMeasure msr)
        {
            String connectionString = DataManager.OraConnString();
            SqlConnection sqlCon = new SqlConnection(connectionString);
            string query = " update ProductionThikness set Thikness='" + msr.MsrUnitDesc + "',[UpdateBy]='" + msr.LoginBy + "',[UpdateDate]='" + Globals._localTime.ToString() + "' where ID='" + msr.MsrUnitCode + "'";
            DataManager.ExecuteNonQuery(connectionString, query);
            sqlCon.Close();
        }



        public static void DeleteMeasure(clsMeasure msr)
        {
            String connectionString = DataManager.OraConnString();
            SqlConnection sqlCon = new SqlConnection(connectionString);
            string query = " update UOM set [DeleteBy]='" + msr.LoginBy + "',[DeleteDate]='" + Globals._localTime.ToString() + "' where ID='" + msr.MsrUnitCode + "'";
            DataManager.ExecuteNonQuery(connectionString, query);
            sqlCon.Close();
        }
        public static void Deleteinfo(string table, string ID,string userID)
        {
            String connectionString = DataManager.OraConnString();
            SqlConnection sqlCon = new SqlConnection(connectionString);
            string query = " update " + table + " set [DeleteBy]='" + userID + "',[DeleteDate]=GetDate() where ID='" + ID + "'";
            DataManager.ExecuteNonQuery(connectionString, query);
            sqlCon.Close();
        }

        public static clsMeasure GetMeasure(System.String msr)
        {
            string connectionString = DataManager.OraConnString();
            SqlConnection sqlCon = new SqlConnection(connectionString);
            string query = "select * from UOM where ID = '" + msr + "'";
            DataTable dt = DataManager.ExecuteQuery(connectionString, query, "UOM");
            sqlCon.Close();
            if (dt.Rows.Count == 0)
            {
                return null;
            }
            return new clsMeasure(dt.Rows[0]);
        }
    }
}