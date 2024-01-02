using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for ProjectModel
/// </summary>
public class ProjectModel
{
	public ProjectModel()
	{
		//
		// TODO: Add constructor logic here
		//
	}
    public int Id { get; set; }
    public int ClientId { get; set; }
    public string ProjectName { get; set; }
    public string Address { get; set; }
    
}