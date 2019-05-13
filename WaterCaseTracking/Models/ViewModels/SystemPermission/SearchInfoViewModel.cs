using WaterCaseTracking.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WaterCaseTracking.Models.ViewModels.SystemPermission
{
    public class SearchInfoViewModel : DataTables.Parameters
    {
        public string MAIL { get; set; }

        public string ID { get; set; }

        public string PID { get; set; }

        public string NAME { get; set; }

        public string Programs { get; set; }

        public string PROID { get; set; }

        public DateTime MODIFY_DATE { get; set; }

        public string MODIFY_USER { get; set; }
    }
}