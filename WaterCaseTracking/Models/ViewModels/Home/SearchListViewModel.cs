using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WaterCaseTracking.Models.ViewModels.Home
{
    public class SearchListViewModel : _BaseRequestViewModel
    {
        public bool success { get; set; }

        public string responseText { get; set; }

        public List<SearchItemViewModel> data { get; set; }

        public List<SearchUserViewModel> UserModel { get; set; }        
    }

    public class SearchItemViewModel
    {
        public int Seq { get; set; }

        public int ID { get; set; }

        public string Title { get; set; }

        public string Contents { get; set; }

        public string AttachedFile { get; set; }

        public DateTime ReleaseDate { get; set; }

        public string ReleaseDateStr { get; set; }

        //public DateTime StopRelease { get; set; }

        public string StopReleaseStr { get; set; }

        //public DateTime CreationDate { get; set; }

        public string CreationDateStr { get; set; }

        public string CreationUser { get; set; }

        public DateTime MODIFY_DATE { get; set; }

        public string MODIFY_USER { get; set; }
        
        public int StautsCode { get; set; }

        public string Stauts { get; set; }

    }

    public class SearchUserViewModel
    {
        public string PID { get; set; }

        public string PROID { get; set; }
    }

}