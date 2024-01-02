using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using autouniv;
using KHSC;
//using DBSC;

/// <summary>
/// Summary description for ExamTitleEntryManager
/// </summary>
public class ExamTitleEntryManager
{
	public ExamTitleEntryManager()
	{
		//
		// TODO: Add constructor logic here
		//
	}
    //SqlConnection connection = new SqlConnection(DataManager.OraConnString());
    SqlTransaction transaction;
    public void SaveExamInformation(ExxamTitleEntry aExxamTitleEntryObj)
    {
        string connectionString = DataManager.OraConnString();

        string insertQuery = @"INSERT INTO [tbl_Exam_Title]
           (id,[exam_title_id]
           ,[exam_title]
           ,[class_id])
     VALUES('" + aExxamTitleEntryObj.ExamId + "','" + aExxamTitleEntryObj.ExamId + "','" + aExxamTitleEntryObj.ExamName + "','" + aExxamTitleEntryObj.ClassNme + "')";
        DataManager.ExecuteNonQuery(connectionString, insertQuery);
    }

    public void UpdateExamInformation(ExxamTitleEntry aExxamTitleEntryObj, DataTable dtExamType, string LoginBy)
    {
        string connectionString = DataManager.OraConnString();

        //string UpdateQuery =
        //DataManager.ExecuteNonQuery(connectionString, UpdateQuery);

        connection.Open();
        transaction = connection.BeginTransaction();
        SqlCommand command = new SqlCommand();
        command.Connection = connection;
        command.Transaction = transaction;

        command.CommandText = @"UPDATE [tbl_Exam_Title]
   SET  [exam_title] = '" + aExxamTitleEntryObj.ExamName + "',[class_id] ='" + aExxamTitleEntryObj.ClassNme + "' WHERE [exam_title_id] ='" + aExxamTitleEntryObj.ExamId + "'";
        command.ExecuteNonQuery();

        foreach (DataRow drRow in dtExamType.Rows)
        {
            if (!string.IsNullOrEmpty(drRow["ExamTypeID"].ToString()))
            {
                if (drRow["check"].ToString().Equals("0"))
                {
                    command.CommandText = @"UPDATE [tbl_Exam_WiseExamTypeEntry]
                    SET [DeleteBy] = '" + LoginBy + "',[DeleteDate] =GETDATE()  WHERE ID='" + drRow["ExamTypeID"].ToString() + "'";
                    command.ExecuteNonQuery();
                }
            }
            else
            {
                if (!drRow["check"].ToString().Equals("0"))
                {
                    command.CommandText = @"INSERT INTO [tbl_Exam_WiseExamTypeEntry]
                       ([ExamTitleID],[ExamTypeID],[AddBy],[AddDate])
                 VALUES
                       ('" + aExxamTitleEntryObj.ExamId + "','" + drRow["ID"].ToString() + "','" + LoginBy + "',GETDATE())";
                    command.ExecuteNonQuery();
                }
            }
        }
        transaction.Commit();
        connection.Close();
    }
    public DataTable GetShowExamTypeDetails(string ExamTitleID, string Flag)
    {
        string connectionString = DataManager.OraConnString();
        string SelectQuery = "";
        if (Flag.Equals("1"))
        {
            SelectQuery = @"SELECT t1.ID,CASE when t2.ID IS NULL then 0 else 1 end AS[check]
      ,t1.[TypeName]
      ,convert(nvarchar,t2.ID) AS[ExamTypeID]
      FROM [ExamType] t1 
      left join [tbl_Exam_WiseExamTypeEntry] t2 on t2.[ExamTypeID]=t1.ID and t2.ExamTitleID='" + ExamTitleID + "' and t2.DeleteBy IS NULL ";
        }
        else
        {
            SelectQuery = @"SELECT t1.ID,CASE when t2.ID IS NULL then 0 else 1 end AS[check]
      ,t1.[TypeName]
      ,convert(nvarchar,t2.ID) AS[ExamTypeID]
      FROM [ExamType] t1 
      left join [Class_WiseExamType] t2 on t2.[ExamTypeID]=t1.ID and t2.ClassID='" + ExamTitleID + "' and t2.DeleteBy IS NULL ";
        }
        DataTable dt = DataManager.ExecuteQuery(connectionString, SelectQuery, "tbl_Exam_WiseExamTypeEntry");
        return dt;
    }
    public void DeleteExamInformation(ExxamTitleEntry aExxamTitleEntryObj)
    {
        string connectionString = DataManager.OraConnString();

        string deleteQuery = @"DELETE FROM [tbl_Exam_Title]
      WHERE [exam_title_id] ='" + aExxamTitleEntryObj.ExamId + "' ";
        DataManager.ExecuteNonQuery(connectionString, deleteQuery);
    }

    public DataTable GetExamTitleInfo(string ClassId)
    {
        string connectionString = DataManager.OraConnString();
        string Found = "";
        if (ClassId != "") { Found = "where t1.class_id='" + ClassId + "' order by t2.class_name "; } else { Found = "order by t2.class_name "; }
        string SelectQuery = @"SELECT t1.[exam_title_id]
      ,t1.[exam_title]
      ,t1.[class_id]
      ,t2.class_name
  FROM [tbl_Exam_Title] t1 inner join class_info t2 on t2.class_id=t1.class_id " + Found;
        DataTable dt = DataManager.ExecuteQuery(connectionString, SelectQuery, "tbl_Exam_Title");
        return dt;
    }

    public DataTable GetExamDetailsInfo(string p)
    {
        string connectionString = DataManager.OraConnString();

        string SelectQuery = @"SELECT [exam_title_id]
      ,[exam_title]
      ,[class_id]
  FROM [tbl_Exam_Title]  WHERE [exam_title_id] ='" + p + "'";
        DataTable dt = DataManager.ExecuteQuery(connectionString, SelectQuery, "tbl_Exam_Title");
        return dt;
    }

    SqlConnection connection = new SqlConnection(DataManager.OraConnString());
    public string GetAutoId()
    {
        
            connection.Open();
        try
        {
            //string selectQuery = @"SELECT 'EXM-' + RIGHT('000000'+CONVERT(VARCHAR,ISNULL(MAX(CONVERT(INTEGER,RIGHT([exam_title_id],6))),0)+1),6) FROM [tbl_Exam_Title]";
            string selectQuery = @"SELECT RIGHT('000'+CONVERT(VARCHAR,ISNULL(MAX(CONVERT(INTEGER,RIGHT([exam_title_id],3))),0)+1),3) FROM [tbl_Exam_Title]";
            SqlCommand command = new SqlCommand(selectQuery, connection);
            return command.ExecuteScalar().ToString();      
            }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        finally
        {
            connection.Close();
        }
        
    }

    public static DataTable GetShowExamInformation(string p)
    {
        string connectionString = DataManager.OraConnString();

        string SelectQuery = @"SELECT [exam_title],id      
  FROM [tbl_Exam_Title]  WHERE [class_id] ='" + p + "'";
        DataTable dt = DataManager.ExecuteQuery(connectionString, SelectQuery, "tbl_Exam_Title");
        return dt;
    }

    public static DataTable GetShowAdmitCardInfo(string Std_Id, string Class, string Section, string Verson, string Shift)
    {
        string connectionString = DataManager.OraConnString();
        string Condition = "";
        if (Std_Id != "" && Class == "" && Section == "" && Shift == "" && Verson == "") { Condition = "Where T1.student_id='" + Std_Id + "' order by CONVERT(int, T1a.std_roll)"; }

        else if (Std_Id != "" && Class != "" && Section != "" && Shift != "" && Verson != "") { Condition = "Where T1.student_id='" + Std_Id + "' order by CONVERT(int, T1a.std_roll) "; }

        else if (Std_Id != "" && Class != "" && Section == "" && Shift == "" && Verson == "") { Condition = "Where T1.student_id='" + Std_Id + "' And T1a.class_id ='" + Class + "'  order by CONVERT(int, T1a.std_roll) "; }

        else if (Std_Id == "" && Class != "" && Section == "" && Shift == "" && Verson == "") { Condition = "Where T1a.class_id ='" + Class + "' order by CONVERT(int, T1a.std_roll) "; }

        else if (Std_Id == "" && Class != "" && Section != "" && Shift != "" && Verson != "") { Condition = "Where T1a.class_id ='" + Class + "' and T1a.sect='" + Section + "' and T1a.shift='" + Shift + "' and T1a.version ='" + Verson + "' order by CONVERT(int, T1a.std_roll) "; }

        string SelectQuery = @"SELECT T1.student_id, t1c.class_name, CONVERT(int, T1a.std_roll) AS Roll,  t1s.sec_name,vi.version_name,si.shift_name ,T1a.class_year, dbo.InitCap(T1.f_name + ' ' + T1.m_name + ' ' + T1.l_name) AS StudentName,t1.std_photo
FROM  dbo.student_info AS T1 
INNER JOIN dbo.std_current_status AS T1a ON T1.ID = T1a.student_id AND T1.status = 1   
INNER JOIN dbo.class_info AS t1c ON T1a.class_id = t1c.class_id 
INNER JOIN dbo.section_info AS t1s ON T1a.sect = t1s.sec_id 
 Inner join version_info As vi on vi.version_id=T1a.version 
 Inner join shift_info As si on si.shift_id=T1a.shift " + Condition;
        DataTable dt = DataManager.ExecuteQuery(connectionString, SelectQuery, "tbl_Exam_Title");
        return dt;
    }
} 