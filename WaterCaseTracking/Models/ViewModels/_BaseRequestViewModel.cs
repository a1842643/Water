using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WaterCaseTracking.Models.ViewModels
{
    public class _BaseRequestViewModel
    {
        public string db_Result { get; set; }

        public int draw { get; set; }
        public int recordsTotal { get; set; }
        public int recordsFiltered { get; set; }

        private string _sSortDir_0;
        public int sEcho { get; set; }
        public int iColumns { get; set; }
        public string sColumns { get; set; }
        public int iDisplayStart { get; set; }
        public int iDisplayLength { get; set; }
        public int iSortCol_0 { get; set; }
        public string sSortDir_0
        {
            get { return string.IsNullOrEmpty(_sSortDir_0) || _sSortDir_0.ToLower() == "desc" ? "desc" : "asc"; }
            set { _sSortDir_0 = value; }
        }

    }
}