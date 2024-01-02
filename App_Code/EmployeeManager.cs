using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ACCWebApplication.DAO.Others;
using KHSC.DAO.Employee;
using KHSC.DAO.Others;

using System.Data;
using KHSC.Gateway.EmloyeeGateway;


namespace KHSC.Manager.EmployeeManager
{
    public class EmployeeManager
    {
        EmployeeGateway aEmployeeGatewayObj = new EmployeeGateway();

        public void SaveTheEmployeeInformation(EmployeeInformation aEmployeeInformationObj, List<ExamTitle> etList, List<EmployeeExperience> EmpExperienceList, List<RefrenceInfo> EmpRefrenceList, byte[] image, byte[] sig)
        {
            aEmployeeGatewayObj.SaveTheEmployeeInformation(aEmployeeInformationObj, etList, EmpExperienceList, EmpRefrenceList, image, sig);
        }

        public string GetEmployeeAutoId()
        {
            return aEmployeeGatewayObj.GetEmployeeAutoId();
        }

        public DataTable GetAllEmployeeInformation(string EmpId, string DesignationID, string Name, string TeacherOfStaff)
        {
            DataTable table = aEmployeeGatewayObj.GetAllEmployeeInformation(EmpId, DesignationID, Name, TeacherOfStaff);
            return table;
            
        }

        public EmployeeInformation GetAllEmployeeInformationForSpecificEmployee(string empId)
        {
            return aEmployeeGatewayObj.GetAllEmployeeInformationForSpecificEmployee(empId);
        }

        //public void UpdateTheEmployeeInformation(EmployeeInformation aEmployeeInformationObj, List<ExamTitle> etList, List<Specialized> aSpecializedList)
        //{
        //    aEmployeeGatewayObj.UpdateTheEmployeeInformation(aEmployeeInformationObj, etList, aSpecializedList);
        //}

        //public void DeleteTheEmployeeInformation(EmployeeInformation aEmployeeInformationObj)
        //{
        //    aEmployeeGatewayObj.DeleteTheEmployeeInformation(aEmployeeInformationObj);
        //}

        public byte[] GetEmployeeImageForSpecificEmployee(string employeeId)
        {
           return aEmployeeGatewayObj.GetEmployeeImageForSpecificEmployee(employeeId);
        }
        public byte[] GetEmployeeSignImageForSpecificEmployee(string employeeId)
        {
            return aEmployeeGatewayObj.GetEmployeeSignImageForSpecificEmployee(employeeId);
        }

        public DataTable GetAllExamTitleInformationForSpecifEmployee(string employeeId)
        {
            DataTable table = aEmployeeGatewayObj.GetAllExamTitleInformationForSpecifEmployee(employeeId);
            return table;
        }

        public DataTable GetAllExperienceInformationForSpecificEmployee(string employeeId)
        {
            DataTable table = aEmployeeGatewayObj.GetAllExperienceInformationForSpecificEmployee(employeeId);
            return table;
        }

        public DataTable GetAllRefrenceInformationForSpecifcEmployee(string employeeId)
        {
            DataTable table = aEmployeeGatewayObj.GetAllRefrenceInformationForSpecifcEmployee(employeeId);
            return table;
        }

        public void UpdateTheEmployeeInformation(EmployeeInformation aEmployeeInformation, List<ExamTitle> etList, List<EmployeeExperience> EmpExperienceList, List<RefrenceInfo> EmpRefrenceList, byte[] image, byte[] sig)
        {
            aEmployeeGatewayObj.UpdateTheEmployeeInformation(aEmployeeInformation,etList,EmpExperienceList,EmpRefrenceList,image,sig);
        }

        public void DeleteTheEmployeeInformation(string employeeId)
        {
            aEmployeeGatewayObj.DeleteTheEmployeeInformation(employeeId);
        }



        public DataTable GetAllEmployeeOnTeacherInformation()
        {
            DataTable table = aEmployeeGatewayObj.GetShowEmployeeOnTeacher();
            return table;
        }
    }
}