using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WaterCaseTracking.Models.ViewModels.KPI_Busines
{
    public class SearchInfoViewModel : DataTables.Parameters
    {
        public float ManageRatio { get; set; }

        public float PerformanceRatio { get; set; }

        public float ManageDistribution { get; set; }

        public float ManageDeduction { get; set; }

        public string SUBJECT { get; set; }

        public int LEVELS { get; set; }

        public string GROUPS { get; set; }

        public string UUID { get; set; }

        public int Year { get; set; }
    }
}