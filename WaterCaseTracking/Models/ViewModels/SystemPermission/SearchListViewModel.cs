using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WaterCaseTracking.Models.ViewModels.SystemPermission
{
    public class SearchListViewModel : _BaseRequestViewModel
    {
        public List<SearchItemViewModel> data { get; set; }

        public List<PermissionModel> list { get; set; }
    }

    public class SearchItemViewModel
    {
        public int ID { get; set; }

        public string Programs { get; set; }

        public string PROID { get; set; }

        public DateTime MODIFY_DATE { get; set; }

        public string MODIFY_USER { get; set; }
    }
}