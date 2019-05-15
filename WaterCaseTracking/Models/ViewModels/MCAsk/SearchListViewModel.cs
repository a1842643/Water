using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WaterCaseTracking.Models.ViewModels.MCAsk
{
    public class SearchListViewModel : _BaseRequestViewModel
    {
        public List<SearchItemViewModel> data { get; set; }
    }

    public class SearchItemViewModel
    {
        public int ID { get; set; }
        public string AskDate { get; set; }
        public string Area { get; set; }
        public string MemberName { get; set; }
        public string Inquiry { get; set; }
        public string HandlingSituation { get; set; }
        public string Organizer { get; set; }
        public string OrganizerMan { get; set; }
        public string sStatus { get; set; }
        public string CreateUserName { get; set; }
        public string CreateDate { get; set; }
        public string UpdateUserName { get; set; }
        public string UpdateDate { get; set; }
    }
}