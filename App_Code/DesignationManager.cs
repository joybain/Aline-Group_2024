using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using autouniv;
using KHSC.Gateway.Others;
using System.Data;
using System.Data.SqlClient;
using KHSC.DAO.Others;

namespace KHSC.Manager.Others
{
    public class DesignationManager
    {
        DesignationGateway aDesignationGatewayObj = new DesignationGateway();

        public string GetDesignationAutoId()
        {
            return aDesignationGatewayObj.GetDesignationAutoId();
        }

        public DataTable GetAllDesignationInformation()
        {
            DataTable table = aDesignationGatewayObj.GetAllDesignationInformation();
            return table;
        }

        public void SaveTheDesignationInformation(Designation aDesignationObj)
        {
            aDesignationGatewayObj.SaveTheDesignationInformation(aDesignationObj);
        }

        public void DeleteTheDesig(Designation aDesignationObj)
        {
            aDesignationGatewayObj.DeleteTheDesig(aDesignationObj);
        }

        public void UpdateTheDesig(Designation aDesignationObj)
        {
            aDesignationGatewayObj.UpdateTheOldDesigInforation(aDesignationObj);
        }

        public DataTable GetDesignationDesignation(string DesignationID, string Flag)
        {
            string query="",Parameter="";
            string connectionString = DataManager.OraConnString();
            if (!string.IsNullOrEmpty(DesignationID))
            {
                DesignationID = " where desig_id='" + DesignationID + "' ";
            }
            query = @"SELECT [desig_id],[desig_name],[Serial] FROM [dbo].[tbl_designation_information] " + DesignationID + " order by [Serial] asc ";

            DataTable dt = DataManager.ExecuteQuery(connectionString, query, "subject_info");
            return dt;  
        }
    }
}