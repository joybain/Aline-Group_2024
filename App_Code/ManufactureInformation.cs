using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

/// <summary>
/// Summary description for ManufactureInformation
/// </summary>
public class ManufactureInformation
{
    private System.Data.DataRow dataRow;

	public ManufactureInformation()
	{
		//
		// TODO: Add constructor logic here
		//
	}

    public ManufactureInformation(DataRow dr)
    {
        if (dr["ID"].ToString() != string.Empty) { this.ID = dr["ID"].ToString(); }
        if (dr["GenerateCode"].ToString() != string.Empty) { this.GenerateCode = dr["GenerateCode"].ToString(); }
        if (dr["Remarks"].ToString() != string.Empty) { this.Remarks = dr["Remarks"].ToString(); }
        if (dr["ManufagchuredBy"].ToString() != string.Empty) { this.ManufagchuredBy = dr["ManufagchuredBy"].ToString(); }
        if (dr["ManufagchuredDate"].ToString() != string.Empty) { this.ManufagchuredDate = dr["ManufagchuredDate"].ToString(); }
        if (dr["ProductionCode"].ToString() != string.Empty) { this.ProductionCode = dr["ProductionCode"].ToString(); }
        if (dr["ProductionSerial"].ToString() != string.Empty) { this.ProductionSerial = dr["ProductionSerial"].ToString(); }
        if (dr["ProductionParentCode"].ToString() != string.Empty) { this.ProductionParentCode = dr["ProductionParentCode"].ToString(); }
        if (dr["ProductionID"].ToString() != string.Empty) { this.ProductionID = dr["ProductionID"].ToString(); }

    }

    public string ID { get; set; }

    public string GenerateCode { get; set; }

    public string Remarks { get; set; }

    public string ManufagchuredBy { get; set; }

    public string ManufagchuredDate { get; set; }

    public string ProductionCode { get; set; }

    public string ProductionSerial { get; set; }

    public string ProductionParentCode { get; set; }

    public string ProductionID { get; set; }

    public object AddBy { get; set; }

    public string ExpDeliveryDate { get; set; }
}