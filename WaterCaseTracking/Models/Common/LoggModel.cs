using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WaterCaseTracking.Models.Common
{
    public class LoggModel
    {
        public int ID { get; set;}
        public string UserID { get; set;}
        public string UserName { get; set;}
        public string FuncName { get; set;}
        public string Status { get; set;}
        public string Message { get; set;}
        public DateTime logged { get; set;}
    }
}