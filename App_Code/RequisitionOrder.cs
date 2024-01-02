using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

/// <summary>
/// Summary description for RequisitionOrder
/// </summary>
public class RequisitionOrder
{
    private System.Data.DataRow dataRow;

	public RequisitionOrder()
	{
		//
		// TODO: Add constructor logic here
		//
	}

    public RequisitionOrder(DataRow dr)
    {
        if (dr["ID"].ToString() != String.Empty) { this.ID = dr["ID"].ToString(); }
        if (dr["Code"].ToString() != String.Empty) { this.Code = dr["Code"].ToString(); }
        if (dr["Date"].ToString() != String.Empty) { this.Date = dr["Date"].ToString(); }
        if (dr["RequisitionBy"].ToString() != String.Empty) { this.RequisitionBy = dr["RequisitionBy"].ToString(); }
        if (dr["Remarks"].ToString() != String.Empty) { this.Remarks = dr["Remarks"].ToString(); }
        if (dr["Status"].ToString() != String.Empty) { this.Status = dr["Status"].ToString(); }

        try
        {
            if (dr["AuthorizedDate"].ToString() != String.Empty) { this.AuthorizedDate = dr["AuthorizedDate"].ToString(); }
        }
        catch
        {

            this.AuthorizedDate = "";
        }
        
    }

    public string ID { get; set; }

    public string Code { get; set; }

    public string Date { get; set; }

    public string RequisitionBy { get; set; }

    public string Remarks { get; set; }

    public string LoginBy { get; set; }

    public string Status { get; set; }
    public string AuthorizedDate { get; set; }
}