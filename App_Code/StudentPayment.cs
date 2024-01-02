using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for StudentAdmissionFee
/// </summary>
public class StudentPayment
{

    public string Id { get; set; }

    public string StudentName { get; set; }

    public string Class { get; set; }

    public string Roll { get; set; }

    public string Year { get; set; }

    public string Section { get; set; }

    public string PaymentId { get; set; }

    public double TotalAmount { get; set; }

    public double PaidAmount { get; set; }

    public double DueAmount { get; set; }

    public string HeadId { get; set; }

    public string MoneyReciptNo { get; set; }

    public string BookPageNo { get; set; }

    public string BankName { get; set; }

    public DateTime Date { get; set; }

    public string Remarks { get; set; }

    public string PaymentName { get; set; }

    public double TotalpayAmount { get; set; }

    public double TotalLatefee { get; set; }

    public double GrandTotal { get; set; }

    public double PaidByCash { get; set; }

    public double PaidByBank { get; set; }

    public double TotalPaidAmount { get; set; }

    public double TotalDueAmount { get; set; }

    public string BranchName { get; set; }

    public string AccountNo { get; set; }

    public string PaymentMod { get; set; }

    public string ChequeNo { get; set; }

    public DateTime ChequePostingDate { get; set; }

    public double Transportfee { get; set; }
}