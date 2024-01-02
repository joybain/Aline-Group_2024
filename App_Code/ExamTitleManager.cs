using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using KHSC.Gateway.Others;
using System.Data;
using KHSC.DAO.Others;

namespace KHSC.Manager.Others
{
    public class ExamTitleManager
    {
        ExamTitleGateway aExamTitleGatewayObj = new ExamTitleGateway();

        public string GetExamTitleAutoId()
        {
            return aExamTitleGatewayObj.GetExamTitleAutoId();
        }

        public DataTable GetAllExamTitleInformation()
        {
            DataTable table = aExamTitleGatewayObj.GetAllExamTitleInformation();
            return table;
        }

        public void SaveTheExamInformation(ExamTitle aExamTitleObj)
        {
            aExamTitleGatewayObj.SaveTheExamInformation(aExamTitleObj);
        }

        public List<ExamTitle> GetAllExamTitleInformationForSpecificEmployee(string employeeId)
        {
            return aExamTitleGatewayObj.GetAllExamTitleInformationForSpecificEmployee(employeeId);
        }

        public void DeleteTheExam(ExamTitle aExamTitleObj)
        {
            aExamTitleGatewayObj.DeleteTheExam(aExamTitleObj);
        }

        public void UpdateTheExam(ExamTitle aExamTitleObj)
        {
            aExamTitleGatewayObj.UpdateTheOldExamInforation(aExamTitleObj);
        }

        public List<string> GetLastExamName(string empId)
        {
            return aExamTitleGatewayObj.GetLastExamName(empId);
        }

        public DataTable GetAllExamTitleInexamInfoInformation()
        {
            DataTable dt = aExamTitleGatewayObj.GetAllExamTitleInexamInfoInformation();
            return dt;
        }
    }
}