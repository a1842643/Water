using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WaterCaseTracking.Models.ViewModels.KPI_Performance.Maintain
{
    public class SearchListViewMode : _BaseRequestViewModel
    {
        //public int draw { get; set; }
        //public int recordsTotal { get; set; }
        //public int recordsFiltered { get; set; }
        public int CmbItemId { get; set; }
        public List<SearchItemViewMode> data { get; set; }
        public List<ItemTreeViewMode> tree { get; set; }
    }

    public class SearchItemViewMode
    {
        public Guid UUID { get; set; }

        public string Subject { get; set; }

        public Guid? UpID { get; set; }

        public float? Distribution { get; set; }

        public string GROUPS { get; set; }

        public int LEVELS { get; set; }

        public int YEART { get; set; }

        public string user { get; set; }

        public List<SearchItemViewMode> Item { get; set; }
    }

    public class ItemTreeViewMode
    {
        public string id { get; set; }

        public string parent { get; set; }

        public string text { get; set; }

    }
}