using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WaterCaseTracking.Models.ViewModels.KPI_Performance.Maintain
{
    public class SearchInfoViewMode : DataTables.Parameters
    {
        public string CmbItemId { get; set; }

        public int year { get; set; }

        public string Subject { get; set; }

        public string groups { get; set; }
    }
}