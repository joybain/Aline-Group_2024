using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for ProjectStockCostmodel
/// </summary>
public class ProjectStockCostmodel
{
	public ProjectStockCostmodel()
	{
		//
		// TODO: Add constructor logic here
		//
	}

    public int Id { get; set; }

    public string Code { get; set; }
    public int ProjectId { get; set; }
    public string SightName { get; set; }
    public string CostingDate { get; set; }
    public string AddDate { get; set; }    
    public string LoginBy { get; set; }
    public string Remarks { get; set; }
    public int TotalQuantity { get; set; }
}