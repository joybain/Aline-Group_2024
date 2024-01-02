using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using KHSC.Gateway.Others;
using System.Data;
using KHSC.DAO.Others;

namespace KHSC.Manager.Others
{
    public class BankManager
    {
        BankGateway aBankGatewayObj = new BankGateway();

        public string GetBankAutoId()
        {
            return aBankGatewayObj.GetBankAutoId();
        }

        public DataTable GetAllBankInformation()
        {
            DataTable table = aBankGatewayObj.GetAllBankInformation();
            return table;
        }

        public void SaveTheBankInformation(Bank aBankObj)
        {
            aBankGatewayObj.SaveTheBankInformation(aBankObj);
        }

        public void DeleteTheBank(Bank aBankObj)
        {
            aBankGatewayObj.DeleteTheBank(aBankObj);
        }

        public void UpdateTheBank(Bank aBankObj)
        {
            aBankGatewayObj.UpdateTheOldBankInforation(aBankObj);
        }

        public  DataTable GetAllBankList(string BankKey)
        {
            DataTable table = aBankGatewayObj.GetAllBankList(BankKey);
            return table;
        }
    }
}