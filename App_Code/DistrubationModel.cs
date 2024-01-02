using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for DistrubationModel
/// </summary>
public class DistrubationModel
{
	public DistrubationModel()
	{
		//
		// TODO: Add constructor logic here
		//
	}

    public int Id { get; set; }
    public string Remark { get; set; }
    public string Date { get; set; }
    public int ProjectId { get; set; }
    public string ItemType { get; set; }
    public string Address { get; set; }
    public string Code { get; set; }
    public string TranseferCode { get; set; }
    public string RequisitionID { get; set; }
    public string RequisitionCode { get; set; }
    public string ChalanNo { get; set; }
    public string LoginBy { get; set; }
    public string ReciveDate { get; set; }

}