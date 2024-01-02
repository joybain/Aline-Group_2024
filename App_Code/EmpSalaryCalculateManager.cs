using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
//using Delve;
using System.Data.SqlClient;
//using CMS;
using Dorjibari;
using autouniv;
using OldColor;
//using Delve;

/// <summary>
/// Summary description for EmpSalaryCalculateManager
/// </summary>
public class EmpSalaryCalculateManager
{
    SqlConnection connection = new SqlConnection(DataManager.OraConnString());
    SqlTransaction transaction;
	public EmpSalaryCalculateManager()
	{
		//
		// TODO: Add constructor logic here
		//
	}

    public static DataTable GetShowAllEmployee(string month, string Year ,string BranchID, string DesignationID, bool CheckOvt)
    {

        String connectionString = DataManager.OraConnString();
        SqlConnection connection = new SqlConnection(DataManager.OraConnString());
        DataTable tab = null;
        connection.Open();


        string query = @" SELECT     '1' as [check],  a.ID ,a.EMP_NO,a.F_NAME + ' ' + a.M_NAME + ' ' + a.L_NAME AS EMP_NAME,'0' as TotalPresent,'0' as TotalOff,'0' as TotalLeave,'0' as AbsentDay,Convert(nvarchar,a.JOIN_DATE,103)  as JOIN_DATE,a.MOBILE , a.FH_NAME , a.MH_NAME, a.PER_LOC, a.PER_DIST_CODE, a.BLOOD_GROUP AS Blood, 
				CONVERT(nvarchar, a.EMP_BIRTH_DT, 103) AS EMP_BIRTH_DT,a.BasicSalary as TotalPay, '0' as DayDeduction,'0' as HouseRant,'0' as SpecialPay,'0' as Absentdeduction,'0' as CasualAmount,'0' as OvertimeAmount,'0' as BonusAmount,'0' as CuttingDays,'0' as LateMin,'0' as CasualDays,'0' as OvertimeHour
                         ,CASE WHEN [SEX] = 'M' THEN 'Male' WHEN [SEX] = 'F' THEN 'Female' ELSE '' END AS sex, '0' as ACCOMFee,'0' as LateDeduction,'0' as NetPament ,'' as remarks,
                         CASE WHEN RELIGION_CODE = '1' THEN 'Islam' WHEN RELIGION_CODE = '2' THEN 'Hindu' WHEN RELIGION_CODE = '5' THEN 'Christian' ELSE '' END AS RELIGION_CODE,'0' as totPresentDays 
                         ,CASE WHEN JOB_STATUS = 'S' THEN 'In Service' WHEN JOB_STATUS = 'R' THEN 'Retired' WHEN JOB_STATUS = 'R1' THEN 'Resigned' WHEN JOB_STATUS = 'T' THEN 'Terminated' WHEN JOB_STATUS = 'T1' THEN 'Transferred'
                          WHEN JOB_STATUS = 'L' THEN 'Lay-off' END AS JOB_STATUS, t1.DESIG_NAME , a.MAIL_LOC, t2.DISTRICT_NAME AS mail_dist_code, a.JOIN_DESIG_CODE, t3.THANA_NAME AS mail_thana_code, br.BranchName, 
                         a.RES_PH_NO + ' ' + a.MOBILE AS contact_no, a.JOB_STATUS, a.JOIN_DESIG_CODE AS Expr1, a.PER_LOC , a.PLACE_OF_BIRTH, t4.DESIG_NAME AS DeaprtMent, t6.DISTRICT_NAME AS per_dist_Name, 
                         t5.THANA_NAME AS per_thana_Name, a.BasicSalary,isnull(t7.AdvaceAmount,0)-isnull(t8.AdvaceAmount,0) as AdvanceLoan, a.EMP_NO as Data1, a.EMP_PHOTO, a.EMP_PHOTO AS Barcode
FROM            dbo.PMIS_PERSONNEL AS a inner  JOIN
                         dbo.PMIS_DESIG_CODE AS t1 ON t1.DESIG_CODE = a.prst_desig_code LEFT OUTER JOIN
                         dbo.PMIS_DISTRICT_CODE AS t2 ON t2.DISTRICT_CODE = a.MAIL_DIST_CODE LEFT OUTER JOIN
                         dbo.PMIS_THANA_CODE AS t3 ON t3.THANA_CODE = a.MAIL_THANA_CODE LEFT OUTER JOIN
                         dbo.PMIS_DISTRICT_CODE AS t6 ON t6.DISTRICT_CODE = a.PER_DIST_CODE inner  JOIN
                         dbo.BranchInfo AS br ON br.ID = a.BranchID LEFT OUTER JOIN
                         dbo.PMIS_DESIG_CODE AS t4 ON t4.ID = CONVERT(int, a.JOIN_DESIG_CODE) LEFT OUTER JOIN
                         dbo.PMIS_THANA_CODE AS t5 ON t5.THANA_CODE = a.PER_THANA_CODE
						 left join (select Sum(isnull(AdvaceAmount,0)) as AdvaceAmount,EMP_ID,Emp_No from  [dbo].[PIMS_AdvanceLoan] group by EMP_ID,Emp_No) t7 on t7.Emp_ID=a.ID

						 left join (select Sum(isnull(AdvaceAmount,0)) as AdvaceAmount,EMP_ID,Emp_No from  [dbo].[PIMS_EMPLOYEESALARYGENERATE] group by EMP_ID,Emp_No) t8 on t8.Emp_ID=a.ID ";
        query = query + " Where a.ID not in (Select t2.EMP_ID from [dbo].[PIMS_EMPLOYEESALARYGENERATE_Mst] t1  inner join [dbo].[PIMS_EMPLOYEESALARYGENERATE] t2 on t2.[MstID]=t1.Code where Month(t1.[SalaryMonth])='" + month + "' and YEAR(t1.[SalaryMonth])='" + Year + "' )";
        
        if (!string.IsNullOrEmpty(DesignationID) && DesignationID!="0")
        {
            query = query + "and t1.DESIG_CODE=" + DesignationID + "";
        }
        if (!string.IsNullOrEmpty(BranchID) && BranchID != "0")
        {
            query = query + "and br.ID =" + BranchID + "";
        }


        

        DataTable dt = DataManager.ExecuteQuery(connectionString, query, "SP_Temp_Emp_Attandance_Salary");
        return dt;
    }   

    public void SaveEmployeeSalaryInformationMast(EmpSalaryCalculate aEmpSalaryCalculate, List<EmpSalaryCalculate> empList)
    {
        try
        {
            DataTable dtFixCode = VouchManager.GetAllFixGlCode("");
            
            connection.Open();

            transaction = connection.BeginTransaction();
           SqlCommand command = new SqlCommand();
            command.Connection = connection;
            command.Transaction = transaction;

            command.CommandText = @"INSERT INTO [dbo].[PIMS_EMPLOYEESALARYGENERATE_Mst]
           ([SalaryDate]
           ,[SalaryMonth]
           ,[TotalEmployee]
           ,[TotalSalary]
           ,[AddBy]
           ,[AddDate],[DesignationID],BranchID)
           VALUES
           (CONVERT(DATE,'" + aEmpSalaryCalculate.Date + "',103),CONVERT(DATE,'" + aEmpSalaryCalculate.SalaryMon +
                                  "',103),'" + empList.Count + "','" +
                                  aEmpSalaryCalculate.TotalSalary.Replace(",", "") + "','" + aEmpSalaryCalculate.EntryBy + "',GETDATE(),'"+aEmpSalaryCalculate.Designation+"','"+aEmpSalaryCalculate.BranchID+"')";
            command.ExecuteNonQuery();

            command.CommandText = @"Select Top(1) Code From PIMS_EMPLOYEESALARYGENERATE_Mst order by Code desc";
            string ID = command.ExecuteScalar().ToString();

            foreach (EmpSalaryCalculate EmpSalary in empList)
            {               

                command.CommandText = @"INSERT INTO [dbo].[PIMS_EMPLOYEESALARYGENERATE]
           (MstID,[EMP_ID] ,[EMP_NO],[EMP_NAME],[TotalPresent],[AbsentDay],[JOIN_DATE],[TotalPay],[DayDeduction],[AdvaceAmount],[Absentdeduction],[CasualAmount],[OvertimeAmount],[BonusAmount],[CuttingDays],[LateMin],[CasualDays],[OvertimeHour],[LateDeduction]
      ,[NetPament],[remarks],[totPresentDays],[DESIG_NAME],[AddBy],[AddDate])
            VALUES
           ('" + ID + "','" + EmpSalary.EmpId + "','" + EmpSalary.empNo + "','" + EmpSalary.EmpName + "','" + EmpSalary.PresentDay + "','" +
                                      EmpSalary.AbsendDay.Replace(",", "") + "','" +
                                      EmpSalary.Joined.Replace(",", "") + "','" +
                                      EmpSalary.TotalPay.Replace(",", "") + "','" +
                                      EmpSalary.CuttingDays.Replace(",", "")
                                      + "','" +
                                      EmpSalary.AdvanceLoan.Replace(",", "") + "','" +
                                      EmpSalary.AbsentAmn.Replace(",", "") + "','" +
                                      EmpSalary.CasualAmount.Replace(",", "") + "','" +
                                      EmpSalary.OvertimeAmount.Replace(",", "") + "','" + EmpSalary.BonusAmount + "','" +
                                      EmpSalary.CuttingDays + "','" + EmpSalary.LateMin + "','" +
                                      EmpSalary.CasualDays + "','" +
                                      EmpSalary.OvertimeHour + "','" + EmpSalary.LateDeduction + "','" + EmpSalary.NetPament.Replace(",", "") + "','" + EmpSalary.Remarks+ "','" + EmpSalary.WorkingDay + "','" + EmpSalary.Designation + "','" + aEmpSalaryCalculate.EntryBy + "',GETDATE())";
                command.ExecuteNonQuery();

                VouchMst vmst = new VouchMst();
                vmst.FinMon = FinYearManager.getFinMonthByDate(aEmpSalaryCalculate.SalaryMon);
                vmst.ValueDate = aEmpSalaryCalculate.SalaryMon;
                vmst.VchCode = "03";
                vmst.RefFileNo = "";
                vmst.VolumeNo = "";
                command.CommandText = @"Select Top(1) ID From PIMS_EMPLOYEESALARYGENERATE order by ID desc";              
                vmst.SerialNo = command.ExecuteScalar().ToString();
                vmst.Particulars = "Salary payment of Employee-" + EmpSalary.EmpName + "";
                vmst.ControlAmt = EmpSalary.NetPament;
                vmst.Payee = "Salary";
                vmst.CheckNo = "";
                vmst.CheqDate = "";
                vmst.CheqAmnt = "";
                vmst.MoneyRptNo = "";
                vmst.MoneyRptDate = "";
                vmst.TransType = "R";
                vmst.BookName = "AMB";
                vmst.EntryUser = aEmpSalaryCalculate.EntryBy;
                vmst.EntryDate = DateTime.Parse(DateTime.Now.ToString()).ToString("dd/MM/yyyy");

                vmst.VchSysNo = IdManager.GetNextID("gl_trans_mst", "vch_sys_no").ToString();
                vmst.VchRefNo = "JV-" + vmst.VchSysNo.ToString().PadLeft(10, '0');
                vmst.Status = "A";
                vmst.AuthoUserType = "4";
                VouchManager.CreateVouchMst(vmst);

                VouchDtl vdtl;
                for (int j = 0; j < 2; j++)
                {
                    //***** Jurnal ON Loan ****//
                    if (j == 0)
                    {
                        vdtl = new VouchDtl();
                        vdtl.VchSysNo = vmst.VchSysNo;
                        vdtl.ValueDate = aEmpSalaryCalculate.SalaryMon;
                        vdtl.LineNo = "1";
                        vdtl.GlCoaCode = dtFixCode.Rows[0]["SalaryExpanceCoa"].ToString();
                        vdtl.Particulars = "Salary Expance";
                        vdtl.AccType = "A";
                        vdtl.AmountDr = EmpSalary.NetPament;
                        vdtl.AmountCr = "0";
                        vdtl.Status = "A";
                        vdtl.BookName ="AMB";
                        VouchManager.CreateVouchDtl(vdtl);
                    }
                    else if (j == 1)
                    {
                        vdtl = new VouchDtl();
                        vdtl.VchSysNo = vmst.VchSysNo;
                        vdtl.ValueDate = aEmpSalaryCalculate.SalaryMon;
                        vdtl.LineNo = "1";
                        vdtl.GlCoaCode = "1-" + EmpSalary.CoaCode;
                        vdtl.Particulars = "Salary Payable To " +EmpSalary.EmpName + " ";
                        vdtl.AccType = "A";
                        vdtl.AmountDr = "0";
                        vdtl.AmountCr = EmpSalary.NetPament;
                        vdtl.Status = "A";
                        vdtl.BookName ="AMB";
                        VouchManager.CreateVouchDtl(vdtl);
                    }
                }
            }      

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
    public void UpdateEmployeeSalaryInformationMast(EmpSalaryCalculate aEmpSalaryCalculate, List<EmpSalaryCalculate> empList)
    {
        try
        {
            DataTable dtFixCode = VouchManager.GetAllFixGlCode("");
            
            connection.Open();
            transaction = connection.BeginTransaction();
            SqlCommand command = new SqlCommand();
            command.Connection = connection;
            command.Transaction = transaction;

            command.CommandText = @"UPDATE [PIMS_EMPLOYEESALARYGENERATE_Mst]
            SET [SalaryDate] =convert(date,'" + aEmpSalaryCalculate.Date + "',103),[TotalSalary] ='" +
                                  aEmpSalaryCalculate.TotalSalary.Replace(",", "") +
                                  "',[SalaryMonth]=CONVERT(DATE,'" + aEmpSalaryCalculate.SalaryMon + "',103) ,[DesignationID]='"+aEmpSalaryCalculate.Designation+"',BranchID='"+aEmpSalaryCalculate.BranchID+"' ,[TotalEmployee]='" + empList.Count + "',[UpdateDate] =GETDATE() ,[UpdateBy] ='" + aEmpSalaryCalculate.EntryBy +
                                  "'  WHERE [Code]='" + aEmpSalaryCalculate.Code + "'";
            command.ExecuteNonQuery();

            command.CommandText = @"DELETE FROM [PIMS_EMPLOYEESALARYGENERATE]  WHERE [MstID]='" + aEmpSalaryCalculate.Code + "'";
            command.ExecuteNonQuery();

            foreach (EmpSalaryCalculate EmpSalary in empList)
            {

                command.CommandText = @"INSERT INTO [dbo].[PIMS_EMPLOYEESALARYGENERATE]
           (MstID,[EMP_ID] ,[EMP_NO],[EMP_NAME],[TotalPresent],[AbsentDay],[JOIN_DATE],[TotalPay],[DayDeduction],[AdvaceAmount],[Absentdeduction],[CasualAmount],[OvertimeAmount],[BonusAmount],[CuttingDays],[LateMin],[CasualDays],[OvertimeHour],[LateDeduction]
      ,[NetPament],[remarks],[totPresentDays],[DESIG_NAME],[AddBy],[AddDate])
            VALUES
           ('" + aEmpSalaryCalculate.Code + "','" + EmpSalary.EmpId + "','" + EmpSalary.empNo + "','" + EmpSalary.EmpName + "','" + EmpSalary.PresentDay + "','" +
                                      EmpSalary.AbsendDay.Replace(",", "") + "','" +
                                      EmpSalary.Joined.Replace(",", "") + "','" +
                                      EmpSalary.TotalPay.Replace(",", "") + "','" +
                                      EmpSalary.CuttingDays.Replace(",", "")
                                      + "','" +
                                      EmpSalary.AdvanceLoan.Replace(",", "") + "','" +
                                      EmpSalary.AbsentAmn.Replace(",", "") + "','" +
                                      EmpSalary.CasualAmount.Replace(",", "") + "','" +
                                      EmpSalary.OvertimeAmount.Replace(",", "") + "','" + EmpSalary.BonusAmount + "','" +
                                      EmpSalary.CuttingDays + "','" + EmpSalary.LateMin + "','" +
                                      EmpSalary.CasualDays + "','" +
                                      EmpSalary.OvertimeHour + "','" + EmpSalary.LateDeduction + "','" + EmpSalary.NetPament.Replace(",", "") + "','" + EmpSalary.Remarks + "','" + EmpSalary.WorkingDay + "','" + EmpSalary.Designation + "','" + aEmpSalaryCalculate.EntryBy + "',GETDATE())";
                command.ExecuteNonQuery();

               

                string VCH_SYS_NO = IdManager.GetShowSingleValueString("VCH_SYS_NO",
                       "t1.PAYEE='Salary' and SUBSTRING(t1.VCH_REF_NO,1,2)='JV' and t1.SERIAL_NO", "GL_TRANS_MST t1",
                       EmpSalary.DtlsID);
                if (VCH_SYS_NO != "")
                {
                    command.CommandText = @"delete from gl_trans_dtl where vch_sys_no='" + VCH_SYS_NO + "' ";
                    command.ExecuteNonQuery();

                    command.CommandText = @"delete from GL_TRANS_MST where VCH_SYS_NO='" + VCH_SYS_NO + "'";
                    command.ExecuteNonQuery();
                }

                VouchMst vmst = new VouchMst();
                vmst.FinMon = FinYearManager.getFinMonthByDate(aEmpSalaryCalculate.SalaryMon);
                vmst.ValueDate = aEmpSalaryCalculate.SalaryMon;
                vmst.VchCode = "03";
                vmst.RefFileNo = "";
                vmst.VolumeNo = "";
                command.CommandText = @"Select Top(1) ID From PIMS_EMPLOYEESALARYGENERATE order by ID desc";
                vmst.SerialNo = command.ExecuteScalar().ToString();
                vmst.Particulars = "Salary payment of Employee-" + EmpSalary.EmpName + "";
                vmst.ControlAmt = EmpSalary.NetPament;
                vmst.Payee = "Salary";
                vmst.CheckNo = "";
                vmst.CheqDate = "";
                vmst.CheqAmnt = "";
                vmst.MoneyRptNo = "";
                vmst.MoneyRptDate = "";
                vmst.TransType = "R";
                vmst.BookName = "AMB";
                vmst.EntryUser = aEmpSalaryCalculate.EntryBy;
                vmst.EntryDate = DateTime.Parse(DateTime.Now.ToString()).ToString("dd/MM/yyyy");

                vmst.VchSysNo = IdManager.GetNextID("gl_trans_mst", "vch_sys_no").ToString();
                vmst.VchRefNo = "JV-" + vmst.VchSysNo.ToString().PadLeft(10, '0');
                vmst.Status = "A";
                vmst.AuthoUserType = "4";
                VouchManager.CreateVouchMst(vmst);

                VouchDtl vdtl;
                for (int j = 0; j < 2; j++)
                {
                    //***** Jurnal ON Loan ****//
                    if (j == 0)
                    {
                        vdtl = new VouchDtl();
                        vdtl.VchSysNo = vmst.VchSysNo;
                        vdtl.ValueDate = aEmpSalaryCalculate.SalaryMon;
                        vdtl.LineNo = "1";
                        vdtl.GlCoaCode = dtFixCode.Rows[0]["SalaryExpanceCoa"].ToString();
                        vdtl.Particulars = "Salary Expance";
                        vdtl.AccType = "A";
                        vdtl.AmountDr = EmpSalary.NetPament;
                        vdtl.AmountCr = "0";
                        vdtl.Status = "A";
                        vdtl.BookName = "AMB";
                        VouchManager.CreateVouchDtl(vdtl);
                    }
                    else if (j == 1)
                    {
                        vdtl = new VouchDtl();
                        vdtl.VchSysNo = vmst.VchSysNo;
                        vdtl.ValueDate = aEmpSalaryCalculate.SalaryMon;
                        vdtl.LineNo = "1";
                        vdtl.GlCoaCode = "1-" + EmpSalary.CoaCode;
                        vdtl.Particulars = "Salary Payable To " + EmpSalary.EmpName + " ";
                        vdtl.AccType = "A";
                        vdtl.AmountDr = "0";
                        vdtl.AmountCr = EmpSalary.NetPament;
                        vdtl.Status = "A";
                        vdtl.BookName = "AMB";
                        VouchManager.CreateVouchDtl(vdtl);
                    }
                }


            }
            transaction.Commit();
        }
        catch (Exception ex)
        {
            transaction.Rollback();
            connection.Close();
            throw new Exception(ex.Message);
        }
        finally
        {
            if (connection.State == ConnectionState.Open)
                connection.Close();
        }
    }
    public void DeleteEmployeeSalaryInformationMast(EmpSalaryCalculate aEmpSalaryCalculate)
    {
        try
        {
            connection.Open();

            transaction = connection.BeginTransaction();
            SqlCommand command = new SqlCommand();
            command.Connection = connection;
            command.Transaction = transaction;

            command.CommandText = @"UPDATE [PIMS_EMPLOYEESALARYGENERATE_Mst]
               SET [DeleteBy] ='" + aEmpSalaryCalculate.EntryBy + "' ,[DeleteDate] =GETDATE() WHERE Code='" + aEmpSalaryCalculate.Code + "'";
            command.ExecuteNonQuery();            

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
    public static object GetShowAllEmployeeDetails()
    {
        String connectionString = DataManager.OraConnString();
        string query = @"SELECT [Code] as[Id],convert(nvarchar,t.SalaryDate,103) as[Date],t.BranchID ,t2.BranchName , DATENAME(mm,t.SalaryMonth)+'-'+DATENAME(YYYY,t.SalaryMonth) as[Salary Month],t.TotalEmployee AS[Total Employee],t.TotalSalary AS[Total Salary] ,convert(nvarchar,t.AddDate,103) as[Entry date],t.AddBy AS[entry By] FROM [dbo].[PIMS_EMPLOYEESALARYGENERATE_Mst] t
 left join BranchInfo t2 on t2.ID=t.BranchID
 WHERE t.DeleteBy IS NULL ";
        DataTable dt = DataManager.ExecuteQuery(connectionString, query, "Emp_Salary_Mst");
        return dt;
    }

    public static DataTable GetShowEmployeeSalaryDetails(string ID)
    {
        String connectionString = DataManager.OraConnString();
        string query =
            @"SELECT     '1' as [check],tt.ID as dtlsID,  a.ID ,a.EMP_NO,a.F_NAME + ' ' + a.M_NAME + ' ' + a.L_NAME AS EMP_NAME,tt.TotalPresent as TotalPresent,tt.TotalOff as TotalOff,tt.TotalLeave as TotalLeave,tt.AbsentDay as AbsentDay,Convert(nvarchar,a.JOIN_DATE,103)  as JOIN_DATE,a.MOBILE , a.FH_NAME , a.MH_NAME, a.PER_LOC, a.PER_DIST_CODE, a.BLOOD_GROUP AS Blood, 
				CONVERT(nvarchar, a.EMP_BIRTH_DT, 103) AS EMP_BIRTH_DT,tt.TotalPay as TotalPay, tt.DayDeduction as DayDeduction,tt.HouseRant as HouseRant,tt.SpecialPay as SpecialPay,tt.Absentdeduction as Absentdeduction,tt.CasualAmount as CasualAmount,tt.OvertimeAmount as OvertimeAmount,tt.BonusAmount as BonusAmount,tt.CuttingDays as CuttingDays,tt.LateMin as LateMin,tt.CasualDays as CasualDays,tt.OvertimeHour as OvertimeHour
                         ,CASE WHEN [SEX] = 'M' THEN 'Male' WHEN [SEX] = 'F' THEN 'Female' ELSE '' END AS sex, tt.ACCOMFee as ACCOMFee,tt.LateDeduction as LateDeduction,tt.NetPament as NetPament ,tt.remarks as remarks,
                         CASE WHEN RELIGION_CODE = '1' THEN 'Islam' WHEN RELIGION_CODE = '2' THEN 'Hindu' WHEN RELIGION_CODE = '5' THEN 'Christian' ELSE '' END AS RELIGION_CODE,tt.totPresentDays as totPresentDays 
                         ,CASE WHEN a.JOB_STATUS = 'S' THEN 'In Service' WHEN a.JOB_STATUS = 'R' THEN 'Retired' WHEN a.JOB_STATUS = 'R1' THEN 'Resigned' WHEN a.JOB_STATUS = 'T' THEN 'Terminated' WHEN a.JOB_STATUS = 'T1' THEN 'Transferred'
                          WHEN a.JOB_STATUS = 'L' THEN 'Lay-off' END AS JOB_STATUS, t1.DESIG_NAME , a.MAIL_LOC, t2.DISTRICT_NAME AS mail_dist_code, a.JOIN_DESIG_CODE, t3.THANA_NAME AS mail_thana_code, br.BranchName, 
                         a.RES_PH_NO + ' ' + a.MOBILE AS contact_no, a.JOB_STATUS, a.JOIN_DESIG_CODE AS Expr1, a.PER_LOC , a.PLACE_OF_BIRTH, t4.DESIG_NAME AS DeaprtMent, t6.DISTRICT_NAME AS per_dist_Name, 
                         t5.THANA_NAME AS per_thana_Name, tt.BasicSalary,tt.AdvaceAmount as AdvanceLoan, a.EMP_NO as Data1, a.EMP_PHOTO, a.EMP_PHOTO AS Barcode
FROM        [dbo].[PIMS_EMPLOYEESALARYGENERATE_Mst] t inner join    [dbo].[PIMS_EMPLOYEESALARYGENERATE] tt on tt.MstID=t.Code inner join dbo.PMIS_PERSONNEL AS a on tt.EMP_NO=a.EMP_NO inner  JOIN
                         dbo.PMIS_DESIG_CODE AS t1 ON t1.DESIG_CODE = a.prst_desig_code LEFT OUTER JOIN
                         dbo.PMIS_DISTRICT_CODE AS t2 ON t2.DISTRICT_CODE = a.MAIL_DIST_CODE LEFT OUTER JOIN
                         dbo.PMIS_THANA_CODE AS t3 ON t3.THANA_CODE = a.MAIL_THANA_CODE LEFT OUTER JOIN
                         dbo.PMIS_DISTRICT_CODE AS t6 ON t6.DISTRICT_CODE = a.PER_DIST_CODE inner  JOIN
                         dbo.BranchInfo AS br ON br.ID = a.BranchID LEFT OUTER JOIN
                         dbo.PMIS_DESIG_CODE AS t4 ON t4.ID = CONVERT(int, a.JOIN_DESIG_CODE) LEFT OUTER JOIN
                         dbo.PMIS_THANA_CODE AS t5 ON t5.THANA_CODE = a.PER_THANA_CODE  where tt .[MstID]='" + ID + "' ";
        DataTable dt = DataManager.ExecuteQuery(connectionString, query, "Emp_Salary_Mst");
        return dt;
    }

    public static DataTable GetShowEmployeeSalaryDetails(string ID, string Designation, string WorkPlace,string Month,string Year)
    {
        String connectionString = DataManager.OraConnString();
        string parameter = "";
        if (!string.IsNullOrEmpty(ID) && string.IsNullOrEmpty(Designation) && string.IsNullOrEmpty(WorkPlace))
        {
            parameter = " WHERE month(t4.SalaryMonth)='" + Month + "' and  year(t4.SalaryMonth)='" + Year + "' and t1.EmpID ='" + ID + "' ";
        }
        else if (string.IsNullOrEmpty(ID) && !string.IsNullOrEmpty(Designation) && string.IsNullOrEmpty(WorkPlace))
        {
            parameter = " WHERE month(t4.SalaryMonth)='" + Month + "' and  year(t4.SalaryMonth)='" + Year + "' and t2.JOIN_DESIG_CODE ='" + Designation + "' ";
        }
        else if (string.IsNullOrEmpty(ID) && string.IsNullOrEmpty(Designation) && !string.IsNullOrEmpty(WorkPlace))
        {
            parameter = " WHERE month(t4.SalaryMonth)='" + Month + "' and  year(t4.SalaryMonth)='" + Year + "' and t2.WorkPlace ='" + WorkPlace + "' ";
        }
        else if (string.IsNullOrEmpty(ID) && !string.IsNullOrEmpty(Designation) && !string.IsNullOrEmpty(WorkPlace))
        {
            parameter = " WHERE month(t4.SalaryMonth)='" + Month + "' and  year(t4.SalaryMonth)='" + Year + "' and t2.JOIN_DESIG_CODE ='" + Designation + "' and t2.WorkPlace ='" + WorkPlace + "' ";
        }
        else if (string.IsNullOrEmpty(ID) && string.IsNullOrEmpty(Designation) && string.IsNullOrEmpty(WorkPlace))
        {
            parameter = " WHERE month(t4.SalaryMonth)='" + Month + "' and  year(t4.SalaryMonth)='" + Year + "' ";
        }
        string query =
            @"SELECT t1.[ID],t1.[MstID],1 AS [check],t2.JOB_STATUS AS[JOB_STATUS],t1.[EmpID] as [EMP_NO],t2.F_NAME+' '+t2.M_NAME+' '+t2.L_NAME AS[EMP_NAME],convert(nvarchar,t2.JOIN_DATE,103) AS JOIN_DATE,t3.DESIG_NAME,t1.IncrementYear,t1.WorkingMonth,t1.TotalPresent,t1.TotalOff,t1.TotalLeave,t1.AbsendDay AS[AbsentDay],t1.WorkingDay AS totPresentDays,t1.ConsolidatedSalary,t1.BasicSalary,t1.HouseRent AS[HouseRant],t1.SpecialPay,t1.TotalPay,t1.Absent AS[Absentdeduction],t1.AccomFee AS ACCOMFee,t1.AdvanceLoan,t1.OthersDeduction,t1.Total AS NetPament,t1.remarks  ,t1.DeductionFromBasic,t1.ConsolidatedSalary AS consolidated,t1.DutyTimeFix,t1.Joined,t1.Resigned,t1.IncrementStates,t2.WorkPlace AS[WorkPlaceID],t3.DESIG_CODE, 0 AS[JoinOrLeft] ,[dbo].[IN_WORDs](t1.Total) AS InWord

  FROM [Emp_Salary_Dtl] t1
  inner join [dbo].[Emp_Salary_Mst] t4 on t4.[Code]=t1.MstID
	  inner join [dbo].[PMIS_PERSONNEL] t2 on t2.EMP_NO=t1.EmpID
	  left join PMIS_DESIG_CODE t3 on t3.DESIG_CODE=t2.JOIN_DESIG_CODE " + parameter;
        DataTable dt = DataManager.ExecuteQuery(connectionString, query, "Emp_Salary_Mst");
        return dt;
    }

    public static int getCountSalaryGenerat(string p)
    {
        SqlConnection connection = new SqlConnection(DataManager.OraConnString());
        connection.Open();
        string selectQuery = @"SELECT COUNT(*) FROM [PIMS_EMPLOYEESALARYGENERATE] t1 where t1.[Code]='" + p + "' and t1.delete_status=1";
        SqlCommand command = new SqlCommand(selectQuery,connection);
        return Convert.ToInt32(command.ExecuteScalar());
    }

    public static void getCheckSalaryDeducation(string code)
    {
        String connectionString = DataManager.OraConnString();
        string query = @"UPDATE [Emp_Salary_Mst]
   SET [delete_status] ='1'  WHERE [Code]='" + code + "'";
        DataManager.ExecuteNonQuery(connectionString, query);
    }

    public static DataTable GetShowAllEmployeeDetails(string p)
    {
        String connectionString = DataManager.OraConnString();
        string query = @"SELECT [Code] as[Id],convert(nvarchar,[date],103) as[Date], DATENAME(mm,salary_month)+'-'+DATENAME(YYYY,salary_month) as[Salary_Month],[total_employee] AS[Total Employee],[total_salary] AS[Total_Salary] ,convert(nvarchar,[entry_date],103) as[Entry date],[entry_by] AS[entry By]  FROM [Emp_Salary_Mst] where Code='"+p+"'";
        DataTable dt = DataManager.ExecuteQuery(connectionString, query, "Emp_Salary_Mst");
        return dt;
    }

    public static DataTable GetShowAllEmployeeDetailsOnMonth(string p)
    {
        String connectionString = DataManager.OraConnString();
        string query = @"SELECT [Code] as[Id],convert(nvarchar,[date],103) as[Date], DATENAME(mm,salary_month)+'-'+DATENAME(YYYY,salary_month) as[Salary_Month],[total_employee] AS[Total Employee],[total_salary] AS[Total_Salary] ,convert(nvarchar,[entry_date],103) as[Entry date],[entry_by] AS[entry By]  FROM [Emp_Salary_Mst] where CONVERT(date,salary_month,103)='" + p + "'";
        DataTable dt = DataManager.ExecuteQuery(connectionString, query, "Emp_Salary_Mst");
        return dt;
    }

    public static DataTable GetShowEmployeeSalaryDetailsOnMonth(string p)
    {
        String connectionString = DataManager.OraConnString();
        string query = @"SELECT t2.F_NAME          
      ,t1.[salary] as[FEES]
      ,t1.[deducation] as[deductions]
      ,t1.[total_salary] as[total]
      ,t1.[remarks] as[remarks]
  FROM [Emp_Salary_Dtl] t1 inner join PMIS_PERSONNEL t2 on t2.EMP_NO=t1.[emp_id] where convert(date,salary_month,103)='" + p + "'";
        DataTable dt = DataManager.ExecuteQuery(connectionString, query, "Emp_Salary_Mst");
        return dt;
    }

    public string GetTransSl()
    {
        String connectionString = DataManager.OraConnString();
        SqlConnection connection = new SqlConnection(connectionString);
        try
        {
            connection.Open();
            string selectQuery = @"SELECT 'ESC-'+convert(nvarchar,YEAR(GETDATE()))+'-'+ RIGHT('00000'+Convert(varchar,ISNULL(Max(Convert(integer, RIGHT(Code, 5))),0)+ 1),5)FROM Emp_Salary_Mst";
            SqlCommand command = new SqlCommand(selectQuery, connection);
            string al_ledger_id = command.ExecuteScalar().ToString();
            return al_ledger_id.ToString();

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



    public static DataTable getShowAllSalaryInfo(string SalaryMonth,string designationID,string WorkPlaceID,string StudentID)
    {


        using (SqlConnection con = new SqlConnection(DataManager.OraConnString()))
        {
            try
            {
                DateTime ddt = DateTime.ParseExact(SalaryMonth, "dd/MM/yyyy", null);
                DateTime EndDt = ddt.AddMonths(1).AddDays(-1);
                DataSet ds = new DataSet();
                string connectionString = DataManager.OraConnString();
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    SqlCommand sqlComm = new SqlCommand("SP_Emp_Salary_Summry", conn);
                    sqlComm.Parameters.AddWithValue("@FromDate", ddt);
                    sqlComm.Parameters.AddWithValue("@ToDate", EndDt);
                    sqlComm.Parameters.AddWithValue("@SalaryMonth", ddt);

                    if (StudentID != "")
                    {
                        sqlComm.Parameters.AddWithValue("@EmpID", StudentID);

                    }
                    else
                    {
                        if (designationID != "0")
                        {
                            sqlComm.Parameters.AddWithValue("@DesignationId", designationID);

                        }
                        if (WorkPlaceID != "")
                        {
                            sqlComm.Parameters.AddWithValue("@Work_PlaceId", WorkPlaceID);
                        }
                    }
                    sqlComm.CommandType = CommandType.StoredProcedure;
                
                    SqlDataAdapter da = new SqlDataAdapter();
                    da.SelectCommand = sqlComm;
                    sqlComm.CommandTimeout = 600;

                    da.Fill(ds, "tableName");
                    return ds.Tables[0];
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                con.Close();
            }
        }


//        String connectionString = DataManager.OraConnString();
//        string Parameter="";
//        if (StudentID != "") { Parameter = "where CONVERT(DATE,t5.SalaryMonth,103)=CONVERT(DATE,'" + p + "',103) AND t2.Code='" + StudentID + "' order by  t1.[EmpID]"; }
//        if (designationID != "0" && WorkPlaceID == "" && StudentID == "") { Parameter = "where CONVERT(DATE,t5.SalaryMonth,103)=CONVERT(DATE,'" + p + "',103) AND t2.JOIN_DESIG_CODE='" + designationID + "' order by  t1.[EmpID] "; }
//        if (designationID != "0" && WorkPlaceID != "" && StudentID == "")
//        { Parameter = "where CONVERT(DATE,t5.SalaryMonth,103)=CONVERT(DATE,'" + p + "',103) AND t2.JOIN_DESIG_CODE='" + designationID + "' AND t2.WorkPlace='" + WorkPlaceID + "' order by  t1.[EmpID] "; }
//        string query = @"SELECT t1.[EmpID] AS EMP_NO
//      ,t2.Code+' - '+t2.F_NAME AS[EMP_NAME]
//      ,t3.DESIG_NAME
//      ,t4.Name AS [Department]
//      ,t1.[BasicSalary] AS BasicSalary
//      ,t1.[OverTimeRate] AS OverTimeRate
//      ,t1.[OverTimeHR] AS OverTimeHours
//      ,t1.[OverTHierRate] AS HairHoursRate
//      ,t1.[OverTHierHR] AS HairHours
//      ,t1.[OverTimeSalary] AS OvertimeSalary
//      ,t1.[OverHierSalary] AS OverTimeHairSalary
//      ,t1.Other AS Other
//      ,t1.[Deduction] AS deductions
//      ,t1.[Total] AS TotalSalary
//      ,t1.[Remarks] AS remarks      
//  FROM [Emp_Salary_Dtl] t1 INNER JOIN Emp_Salary_Mst t5 on t5.Code=t1.MstID INNER JOIN PMIS_PERSONNEL t2 on t2.EMP_NO=t1.EmpID INNER JOIN PMIS_DESIG_CODE t3 on t3.DESIG_CODE=t2.JOIN_DESIG_CODE  LEFT JOIN Work_Place t4 on t4.ID=t2.WorkPlace " + Parameter;
//        DataTable dt = DataManager.ExecuteQuery(connectionString, query, "Emp_Salary_Mst");
//        return dt;
    }



    public static DataTable GetsalaryInfo(string EmpCode, string Month,string Year)
    {
        String connectionString = DataManager.OraConnString();

        string query = @"select  MstID, Code,SalaryMonth, SalaryDate, EmpID, EMP_NAME, DESIG_NAME, JOIN_DATE, Increment, BasicSalary, HouseRant, NetBasePay, OthersPay, TotalPay, Remarks, OthersDeduction, Absent, AdvanceLoan, AccomFee, Deduction, TotalPayable,isnull(WorkingDay,0) as WorkingDay, dbo.InitCap(InWord) as InWord from dbo.View_SalaryInfoForReport where  Convert(date, SalaryMonth,103)=Convert(date,'01/" + Month + "/" + Year + "',103) ";
        if (EmpCode != "")
        {
            query = query + " and Code IN(" + EmpCode + ")";
        }
        DataTable dt = DataManager.ExecuteQuery(connectionString, query, "View_SalaryInfoForReport");
        return dt;
    }

    public static DataTable GetFixationOFPAy(string EmpID, string Month, string Year)
    {
        String connectionString = DataManager.OraConnString();
        string Paramiter="";
        if (EmpID!="")
        {
            Paramiter = " And t1.Emp_No='" + EmpID + "'";
        }
        string query = @"Select t1.Emp_No,t1.JOB_STATUS,t1.Code,Convert(nvarchar,t1.Code)+'-'+t1.F_Name as Name ,Convert(nvarchar,t1.Join_date,103) as JoinDate,isnull(t1.BasicSalary,0) as BasicSalary ,isnull(t1.Increment,0) as Increment,isnull(t1.SpecialPay,0) as SpecialPay,isnull(t1.Houserent,0) as Houserent,t2.DESIG_NAME ,convert(decimal(18,2),isnull(t1.BasicSalary,0)+isnull(t1.Increment,0)+isnull(t1.SpecialPay,0)+isnull(t1.Houserent,0)) as TotalPAy,0 as Increased
from dbo.PMIS_PERSONNEL t1 inner join  dbo.PMIS_DESIG_CODE t2 on t2.DESIG_CODE=t1.join_desig_Code Where t1.JOB_STATUS =1  " + Paramiter + " order by t1.Code";
        DataTable dt = DataManager.ExecuteQuery(connectionString, query, "PMIS_PERSONNEL");
        return dt;
    }

    public static DataTable GetPaysalaryInfo(string EmpCode, string month, string Year, string WorkPlaceID, string Designation)
    {
        using (SqlConnection con = new SqlConnection(DataManager.OraConnString()))
        {
            try
            {
               // DateTime ddt = DateTime.ParseExact(month, "dd/MM/yyyy", null);
                //DateTime EndDt = ddt.AddMonths(1).AddDays(-1);
                DataSet ds = new DataSet();
                string connectionString = DataManager.OraConnString();
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    SqlCommand sqlComm = new SqlCommand("SP_Emp_Increment", conn);
                    sqlComm.Parameters.AddWithValue("@SalaryMonth", Year+"-"+month+"-"+"01");


                    if (Designation != "" && Designation != "0")
                    {
                        sqlComm.Parameters.AddWithValue("@DesignationId", Designation);

                    }
                    else
                    {
                        sqlComm.Parameters.AddWithValue("@DesignationId", null);
                    }
                    

                    if (WorkPlaceID != "" && WorkPlaceID!="0")
                    {
                        sqlComm.Parameters.AddWithValue("@Work_PlaceId", WorkPlaceID);
                    }
                    else
                    {
                        sqlComm.Parameters.AddWithValue("@Work_PlaceId", null);
                    }
                    if (EmpCode != "")
                    {
                        sqlComm.Parameters.AddWithValue("@EmpCode", EmpCode);

                    }
                    else
                    {
                        sqlComm.Parameters.AddWithValue("@EmpCode", null);
                    }
                    sqlComm.CommandType = CommandType.StoredProcedure;

                    SqlDataAdapter da = new SqlDataAdapter();
                    da.SelectCommand = sqlComm;
                    sqlComm.CommandTimeout = 600;

                    da.Fill(ds, "tableName");
                    return ds.Tables[0];
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                con.Close();
            }
        }
    }
}