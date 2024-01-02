using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for EmpSalaryCalculate
/// </summary>

[Serializable]
public class EmpSalaryCalculate
{
	public EmpSalaryCalculate()
	{
		//
		// TODO: Add constructor logic here
		//
	}

    public string EmpId { get; set; }

    public string EmpName { get; set; }

    public string Salary { get; set; }

    public string Deducation { get; set; }

    public string Total { get; set; }

    public string Remarks { get; set; }

    public string EntryBy { get; set; }

    public string Code { get; set; }

    public string Date { get; set; }

    public string SalaryMonth { get; set; }

    public int TotalEmp { get; set; }

    public string TotalSalary { get; set; }

    public string SalaryMon { get; set; }

    public string OverTimeRate { get; set; }

    public string OverTimeHR { get; set; }

    public string OverTHierRate { get; set; }

    public string OverTHierHR { get; set; }

    public string OverTimeSalary { get; set; }

    public string OverHierSalary { get; set; }

    public string Other { get; set; }

    public string HouseRent { get; set; }

    public string SpecialPay { get; set; }

    public string AccomFee { get; set; }

    public string Absent { get; set; }

    public string AdvanceLoan { get; set; }

    public string OthersDeducation { get; set; }

    public string AbsendDay { get; set; }

    public string AbsendHour { get; set; }

    public string ConsolidatedSalary { get; set; }

    public string DeductionFromBasic { get; set; }

    public string AccoDutyTimeFix { get; set; }

    public string WorkingDay { get; set; }

    public string DutyTimeFix { get; set; }

    public string Joined { get; set; }

    public string Resigned { get; set; }

    public string IncrementStates { get; set; }

    public string TotalPay { get; set; }

    public string LastIncrementYear { get; set; }

    public string TotalWarkingMonth { get; set; }

    public string PresentDay { get; set; }

    public string TotalWeeklyLeave { get; set; }

    public string txtTotalLeave { get; set; }



    public string DepartmentID { get; set; }

    public string empNo { get; set; }

    public string CuttingDays { get; set; }

    public string AbsentAmn { get; set; }

    public string Designation { get; set; }

    public string LateMin { get; set; }

    public string CasualDays { get; set; }

    public string OvertimeHour { get; set; }

    public string LateDeduction { get; set; }

    public string CasualAmount { get; set; }

    public string OvertimeAmount { get; set; }

    public string BonusAmount { get; set; }

    public string NetPament { get; set; }

    public string BranchID { get; set; }

    public string BasicSalary { get; set; }

    public string CoaCode { get; set; }

    public string DtlsID { get; set; }
}