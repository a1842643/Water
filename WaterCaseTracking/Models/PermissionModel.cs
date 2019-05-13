using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WaterCaseTracking.Models
{
    public class PermissionModel
    {
        public int SID { get; set; }

        public string PID { get; set; }

        public string NAME { get; set; }
        
        public string MailAddress { get; set; }
    }
}