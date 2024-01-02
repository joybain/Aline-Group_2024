using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

/// <summary>
/// Summary description for PurchaseOrder
/// </summary>
public class PurchaseOrderInfo
{
    public string ID, PO, PODate, RequisitionId, RequisitionNo,CsNo, SupplierID, CustomerId, TermsOfDelivery, TermsOfPayment, OrderStatus, ExpDelDate, ProjectId, ProjectSite;
	public PurchaseOrderInfo()
	{
		//
		// TODO: Add constructor logic here
		//
	}
    public PurchaseOrderInfo(DataRow dr)
    {
        if (dr["ID"].ToString() != String.Empty) { this.ID = dr["ID"].ToString(); }
        if (dr["PO"].ToString() != String.Empty) { this.PO = dr["PO"].ToString(); }
        if (dr["PODate"].ToString() != String.Empty) { this.PODate = dr["PODate"].ToString(); }
        try {if (dr["SupplierID"].ToString() != String.Empty) { this.SupplierID = dr["SupplierID"].ToString(); }} catch  { }
        try { if (dr["CustomerId"].ToString() != String.Empty) { this.CustomerId = dr["CustomerId"].ToString(); }  } catch  { }

        try { if (dr["RequisitionId"].ToString() != String.Empty) { this.RequisitionId = dr["RequisitionId"].ToString(); } }catch { }
        try { if (dr["RequisitionNo"].ToString() != String.Empty) { this.RequisitionNo = dr["RequisitionNo"].ToString(); } }
        catch { }
        
     
        if (dr["TermsOfDelivery"].ToString() != String.Empty) { this.TermsOfDelivery = dr["TermsOfDelivery"].ToString(); }
        if (dr["TermsOfPayment"].ToString() != String.Empty) { this.TermsOfPayment = dr["TermsOfPayment"].ToString(); }
        if (dr["ExpDelDate"].ToString() != String.Empty) { this.ExpDelDate = dr["ExpDelDate"].ToString(); }
        if (dr["OrderStatus"].ToString() != String.Empty) { this.OrderStatus = dr["OrderStatus"].ToString(); }

        try { if (dr["ProjectId"].ToString() != String.Empty) { this.ProjectId = dr["ProjectId"].ToString(); } } catch { }
        try { if (dr["ProjectSite"].ToString() != String.Empty) { this.ProjectSite = dr["ProjectSite"].ToString(); } }catch { }
        try { if (dr["CsNo"].ToString() != String.Empty) { this.CsNo = dr["CsNo"].ToString(); } }catch { }
        
    }

    public string LoginBy { get; set; }

 
}