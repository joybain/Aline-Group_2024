using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for ProjectCostMdl
/// </summary>
public class ProjectCostMdl
{
	public ProjectCostMdl()
	{
		//
		// TODO: Add constructor logic here
		//
	}
    public int Id { get; set; }
    public string Remarks { get; set; }
    public string Date { get; set; }
    public string ProjectName { get; set; }
    public string CostPrice { get; set; }
}