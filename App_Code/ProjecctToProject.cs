using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for ProjecctToProject
/// </summary>
public class ProjecctToProject
{
	public ProjecctToProject()
	{
		//
		// TODO: Add constructor logic here
		//
	}

    public string id { get; set; }
    public string ProjectTransCode { get; set; }
    public string transferDate { get; set; }
    public int formProjectID { get; set; }
    public int ToProjectID { get; set; }
    public string Remarks { get; set; }
    public string Addby { get; set; }
    public int TotalQnty { get; set; }
}