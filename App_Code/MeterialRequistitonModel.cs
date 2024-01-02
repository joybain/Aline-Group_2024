using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for MeterialRequistitonModel
/// </summary>
public class MeterialRequistitonModel
{
    private System.Data.DataRow dataRow;


	public MeterialRequistitonModel()
	{
		//
		// TODO: Add constructor logic here
		//
	}

   
    public int Id { get; set; }
    public string RequisitionNo { get; set; }
    public string Date { get; set; }
    public int ProjectId { get; set; }
    public string Address { get; set; }
    public string Recoment { get; set; }
}