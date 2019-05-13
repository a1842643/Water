using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WaterCaseTracking.Models.ViewModels.ManagementRecordCard
{
    public class SearchListViewModel : _BaseRequestViewModel
    {        
        public List<SearchItemViewModel> data { get; set; }
    }

    public class SearchItemViewModel
    {
        public string A1 { get; set; }
        public string A2 { get; set; }
        public string A3 { get; set; }
        public string A4 { get; set; }
        public string A5 { get; set; }
        public string A6 { get; set; }
    }
}