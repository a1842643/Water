using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WaterCaseTracking.Models.ViewModels.Home
{
    public class SearchInfoViewModel : DataTables.Parameters    
    {
        public string startDate { get; set; }

        public string endDate { get; set; }

        public string SearchStr { get; set; }

        public string ID { get; set; }

        public string user { get; set; }

        public string File { get; set; }     
        
        public int Cmbddlstatus { get; set; }

        public string SearchMode { get; set; }

    }
}