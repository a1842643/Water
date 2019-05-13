using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WaterCaseTracking.Models.ViewModels.ParameterSettings
{
    public class SearchInfoViewModel : DataTables.Parameters
    {
        public string SqlAddress { get; set; }

        public string SqlAccount { get; set; }

        public string SqlPassword { get; set; }

        public string FtpAddress { get; set; }

        public string FtpAccount { get; set; }

        public string FtpPassword { get; set; }

        public string SSOAddress { get; set; }

        public string SSOAccount { get; set; }

        public string SSOPassword { get; set; }

        public string MailAddress { get; set; }

        public string MailAccount { get; set; }

        public string MailPassword { get; set; }
    }

}