using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WaterCaseTracking.Models
{
    public class LoginErrorTimesModel
    {
        public string AccountID { get; set; }
        public int ErrorTimes { get; set; }
        public DateTime LoginTime { get; set; }
    }
}