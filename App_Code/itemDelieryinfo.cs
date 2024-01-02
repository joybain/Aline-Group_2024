using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

/// <summary>
/// Summary description for itemDelieryinfo
/// </summary>
public class itemDelieryinfo
{
	public itemDelieryinfo()
	{
		//
		// TODO: Add constructor logic here
		//
	}

    public itemDelieryinfo(DataRow dr)
    {
        if (dr["ID"].ToString() != String.Empty) { this.ID = dr["ID"].ToString(); }
        if (dr["Code"].ToString() != String.Empty) { this.Code = dr["Code"].ToString(); }
        if (dr["Date"].ToString() != String.Empty) { this.Date = dr["Date"].ToString(); }
        if (dr["RequisitionID"].ToString() != String.Empty) { this.RequisitionID = dr["RequisitionID"].ToString(); }
        if (dr["RequisitionCode"].ToString() != String.Empty) { this.RequisitionCode = dr["RequisitionCode"].ToString(); }
        if (dr["DeliveryBy"].ToString() != String.Empty) { this.DeliveryBy = dr["DeliveryBy"].ToString(); }
        if (dr["Remarks"].ToString() != String.Empty) { this.Remarks = dr["Remarks"].ToString(); }
        
    }

    public string ID { get; set; }

    public string Code { get; set; }

    public string Date { get; set; }

    public string DeliveryBy { get; set; }

    public string Remarks { get; set; }

    public string LoginBy { get; set; }

    public string Status { get; set; }

    public string RequisitionID { get; set; }

    public string RequisitionCode { get; set; }
}