using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WaterCaseTracking.Models.ViewModels.KPI_Performance
{
    public class SearchInitViewModel : _BaseRequestViewModel
    {
        /// <summary>
        /// 是否已鎖定 0未鎖定:1已鎖定
        /// </summary>
        public int LOCKED { get; set; }
        public DropDownListViewModel ddlKPIPer { get; set; }
    }
}