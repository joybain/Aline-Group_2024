using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


using System.Data;
using KHSC.Gateway.Others;
using KHSC.DAO.Others;

namespace KHSC.Manager.Others
{
    public class DepartmentManager
    {
        DepartmentGateway aDepartmentGatewayObj = new DepartmentGateway();

        public string GetDepartmentAutoId()
        {
            return aDepartmentGatewayObj.GetDepartmentAutoId();
        }
        public void SaveTheDepartmentInformation(Department aDepartmentObj)
        {
            aDepartmentGatewayObj.SaveTheDepartmentInformation(aDepartmentObj);
        }

        public DataTable GetAllDepartmentInformation()
        {
            DataTable table = aDepartmentGatewayObj.GetAllDepartmentInformation();
            return table;
        }

        public void UpdateTheDept(Department aDepartmentObj)
        {
            aDepartmentGatewayObj.UpdateTheOldDeptInforation(aDepartmentObj);
        }

        public void DeleteTheDept(Department aDepartmentObj)
        {
            aDepartmentGatewayObj.DeleteTheDept(aDepartmentObj);
        }
    }
}