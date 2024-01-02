using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using KHSC.Gateway.Others;
using System.Data;
using KHSC.DAO.Others;

namespace KHSC.Manager.Others
{
    public class SpecializedManager
    {
        SpecializedGateway aSpecializedGatewayObj = new SpecializedGateway();

        public string GetSpecializedAutoId()
        {
            return aSpecializedGatewayObj.GetSpecializedAutoId();
        }

        public DataTable GetAllSpecializedInformation()
        {
            DataTable table = aSpecializedGatewayObj.GetAllSpecializedInformation();
            return table;
        }

        public void SaveTheSpecializedInformation(Specialized aSpecializedObj)
        {
            aSpecializedGatewayObj.SaveTheSpecializedInformation(aSpecializedObj);
        }

        internal Specialized GetAllSpecializedInformationIsNotSelected(Specialized aSpecializedObj)
        {
            return aSpecializedGatewayObj.GetAllSpecializedInformationIsNotSelected(aSpecializedObj);
            
        }

        public DataTable GetAllSpecializedInformationIsForSpecificEmployee(string employeeId)
        {
            DataTable table = aSpecializedGatewayObj.GetAllSpecializedInformationIsForSpecificEmployee(employeeId);
            return table;
        }



        public List<Specialized> GetAllInfo(string employee)
        {
            return aSpecializedGatewayObj.GetAllInfo(employee);
        }

        public void UpdateTheSpecialized(Specialized aSpecializedObj)
        {
            aSpecializedGatewayObj.UpdateTheOldSpAreaInforation(aSpecializedObj);
        }

        public void DeleteTheSpecialized(Specialized aSpecializedObj)
        {
            aSpecializedGatewayObj.DeleteTheSpecialized(aSpecializedObj);
        }
    }
}