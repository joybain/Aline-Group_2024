using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

/// <summary>
/// Summary description for PaymentManageInfo
/// </summary>
public class PaymentManageInfo
{
    private System.Data.DataRow dataRow;

	public PaymentManageInfo()
	{
		//
		// TODO: Add constructor logic here
		//
	}

    public PaymentManageInfo(DataRow dr)
    {
        if (dr["ID"].ToString() != string.Empty)
        {
            this.Id = dr["ID"].ToString();
        }
        if (dr["MethodName"].ToString() != string.Empty)
        {
            this.Name = dr["MethodName"].ToString();
        }
        if (dr["PaymentID"].ToString() != string.Empty)
        {
            this.PaymentID = dr["PaymentID"].ToString();
        }
        if (dr["BranchID"].ToString() != string.Empty)
        {
            this.Name = dr["BranchID"].ToString();
        }
        
    }
    public string Id { get; set; }

    public string Status { get; set; }

    public string BranchID { get; set; }

    public string Name { get; set; }

    public string PaymentID { get; set; }

    public string LoginBy { get; set; }
}