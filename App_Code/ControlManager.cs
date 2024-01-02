using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace RIITS_FES_Accounts_Apps.Manager
{
    public class ControlManager
    {
        public int IndexCalCulation(DropDownList ComboBoxName, string text)
        {
            int i;
            for (i = 0; i < ComboBoxName.Items.Count; i++)
            {
                if (ComboBoxName.Items[i].Text.ToUpper() == text.ToUpper())
                {                     
                   
                    break;
                    
                }
                
            }
            return i;
        }

    }
}