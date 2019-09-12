using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace WaterCaseTracking.Models.ViewModels.MCAsk
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
        public string AskDate { get; set; }
        public string Area { get; set; }
        public string MemberName { get; set; }
        public string Inquiry { get; set; }
        string _HandlingSituation;
        public string HandlingSituation { get { return _HandlingSituation; } set { _HandlingSituation = Regex.Replace(value, "<.*?>", string.Empty); } }
        public string DetailHandlingSituation { get; set; }
        public string Organizer { get; set; }
        public string OrganizerMan { get; set; }
        public string sStatus { get; set; }
        public string CreateUserName { get; set; }
        public string CreateDate { get; set; }
        public string UpdateUserName { get; set; }
        public string UpdateDate { get; set; }
    }
}