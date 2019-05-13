using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WaterCaseTracking.Models.ViewModels.KPI_Busines
{
    public class SearchListViewModel : _BaseRequestViewModel
    {
        public float ManageRatio { get; set; }

        public float PerformanceRatio { get; set; }

        public float ManageDistribution { get; set; }

        public float ManageDeduction { get; set; }


        public List<SearchItemViewModel> data { get; set; }
    }

    public class SearchItemViewModel
    {
        public string UUID { get; set; }

        public string GROUPS { get; set; }

        public string SUBJECT { get; set; }

        public string UPID { get; set; }

        public int LEVELS { get; set; }

        public int YEART { get; set; }

        public float DISBUTION { get; set; }

        public string MODIFY_USER { get; set; }

        public DateTime MODIFY_DATE { get; set; }
    }
    public class SearchItem
    {
        public string UUID { get; set; }

        public float DISBUTION { get; set; }
    }
}