using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for ProjectStockReturn
/// </summary>
public class ProjectStockReturn
{
	public ProjectStockReturn()
	{
		//
		// TODO: Add constructor logic here
		//
	}
    public string id { get; set; }
    public string CostCode { get; set; }
    public int ProjectID { get; set; }
    public string sight_Name{ get; set; }
    public string Remarks { get; set; }
    public string Addby { get; set; }
    public string costingDate { get; set; }
    public int TotalQnitity { get; set; }
}