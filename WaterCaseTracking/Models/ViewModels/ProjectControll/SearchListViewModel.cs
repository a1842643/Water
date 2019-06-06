using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WaterCaseTracking.Models.ViewModels.ExpectedProject
{
    public class SearchListViewModel : _BaseRequestViewModel
    {
        public List<SearchItemViewModel> data { get; set; }
    }

    public class SearchItemViewModel
    {
        public string ID { get; set; }
        /// <summary>
        /// 項次+Guid
        /// </summary>
        public string HID { get; set; }
        public string ProjectName { get; set; }
        public string CrProExpDate { get; set; }
        public string CrProReaDate { get; set; }
        public string PlanExpDate { get; set; }
        public string PlanReaDate { get; set; }
        public string BasDesExpDate { get; set; }
        public string BasDesReaDate { get; set; }
        public string DetailDesExpDate { get; set; }
        public string DetailDesReaDate { get; set; }
        public string OnlineExpDate { get; set; }
        public string OnlineReaDate { get; set; }
        public string SelectionExpDate { get; set; }
        public string SelectionReaDate { get; set; }
        public string AwardExpDate { get; set; }
        public string AwardReaDate { get; set; }
        public string Organizer { get; set; }
        public string OrganizerMan { get; set; }

        public string CreateUserName { get; set; }
        public string CreateDate { get; set; }
        public string UpdateUserName { get; set; }
        public string UpdateDate { get; set; }
    }
}