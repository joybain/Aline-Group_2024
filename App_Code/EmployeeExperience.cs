using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ACCWebApplication.DAO.Others
{
    public class EmployeeExperience
    {
        public int Serial { get; set; }

        public string OrganizationName { get; set; }

        public string Position { get; set; }

        public string Duration { get; set; }
        public string ReasonForLeave { get; set; }
    }
}