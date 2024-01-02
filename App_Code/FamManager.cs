using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Data.SqlClient;
using  Dorjibari;
using autouniv;

/// <summary>
/// Summary description for FamManager
/// </summary>
/// 
namespace Dorjibari
{
    public class FamManager
    {
        public static void CreateFam(Fam fam)
        {
            String connectionString = DataManager.OraConnString();
            string query = " insert into pmis_family_dtl (emp_no,rel_name,relation,birth_dt,age,occupation) values (" +
                "  '" + fam.EmpNo + "', '" + fam.RelName + "', '" + fam.Relation + "', convert(datetime,nullif('" + fam.BirthDt + "',''),103), " +
             "  convert(numeric,nullif('" + fam.Age + "','')), '" + fam.Occupation + "')";
            DataManager.ExecuteNonQuery(connectionString, query);
        }
        public static void UpdateFam(Fam fam)
        {
            String connectionString = DataManager.OraConnString();
            string query = " update pmis_family_dtl set rel_name= '" + fam.RelName + "',relation= '" + fam.Relation + "',birth_dt= convert(datetime,nullif('" + fam.BirthDt + "',''),103), " +
             " age= convert(numeric,nullif('" + fam.Age + "','')),occupation= '" + fam.Occupation + "' where emp_no='" + fam.EmpNo + "' and rtrim(rel_name)=rtrim('" + fam.RelName + "')";
            DataManager.ExecuteNonQuery(connectionString, query);
        }
        public static void DeleteFam(string emp)
        {
            String connectionString = DataManager.OraConnString();
            string query = " delete from pmis_family_dtl where emp_no='" + emp + "' ";
            DataManager.ExecuteNonQuery(connectionString, query);
        }
        public static Edu getFam(string empno, string rel)
        {
            String connectionString = DataManager.OraConnString();
            string query = "select EMP_NO, REL_NAME, RELATION, convert(varchar,BIRTH_DT,103)birth_dt, convert(varchar,AGE)age, OCCUPATION from pmis_family_dtl where emp_no='" + empno + "' and rtrim(rel_name)=rtrim('" + rel + "') ";
            DataTable dt = DataManager.ExecuteQuery(connectionString, query, "Family");
            if (dt.Rows.Count == 0)
            {
                return null;
            }
            return new Edu(dt.Rows[0]);
        }
        public static DataTable getFams(string empno)
        {
            String connectionString = DataManager.OraConnString();
            string query = "select EMP_NO, REL_NAME, RELATION, convert(varchar,BIRTH_DT,103)birth_dt, convert(varchar,AGE)age, OCCUPATION from pmis_family_dtl where emp_no='" + empno + "' ";
            DataTable dt = DataManager.ExecuteQuery(connectionString, query, "Family");
            return dt;
        }
        public static DataTable getFamily(string empno)
        {
            String connectionString = DataManager.OraConnString();
            string query = "select EMP_NO, dbo.initcap(REL_NAME)rel_name, RELATION, convert(varchar,BIRTH_DT,103)birth_dt, convert(varchar,AGE)age, dbo.initcap(OCCUPATION)occupation from pmis_family_dtl where emp_no='" + empno + "' ";
            DataTable dt = DataManager.ExecuteQuery(connectionString, query, "Family");
            return dt;
        }
        public static DataTable getFamilyRpt(string criteria)
        {
            String connectionString = DataManager.OraConnString();
            string query = "select EMP_NO, dbo.initcap(REL_NAME)rel_name, RELATION, convert(varchar,BIRTH_DT,103)birth_dt, convert(varchar,AGE)age, dbo.initcap(OCCUPATION)occupation from pmis_family_dtl ";
            if (criteria.Length > 0)
            {
                query += " where " + criteria;
            }
            DataTable dt = DataManager.ExecuteQuery(connectionString, query, "Family");
            return dt;
        }
        public static DataTable getFamRpt(string empno)
        {
            String connectionString = DataManager.OraConnString();
            string query = "select dbo.initcap(REL_NAME)rel_name, RELATION, convert(varchar,BIRTH_DT,103)birth_dt, convert(varchar,AGE)age, dbo.initcap(OCCUPATION)occupation from pmis_family_dtl where emp_no='" + empno + "' ";
            DataTable dt = DataManager.ExecuteQuery(connectionString, query, "Family");
            return dt;
        }
    }
}