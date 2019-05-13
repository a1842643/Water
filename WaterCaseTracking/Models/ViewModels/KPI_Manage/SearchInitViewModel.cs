using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WaterCaseTracking.Models.ViewModels.KPI_Manage
{
    public class SearchInitViewModel : _BaseRequestViewModel
    {
        /// <summary>
        /// 是否已鎖定 0未鎖定:1已鎖定
        /// </summary>
        public int LOCKED { get; set; }
        public DropDownListViewModel ddlArea { get; set; }

        public DropDownListViewModel ddlUntil { get; set; }

        public DropDownListViewModel ddlAssessmentGroup { get; set; }

        public DropDownListViewModel ddlAreacenterName { get; set; }

        public DropDownListViewModel ddlGen_JurisName { get; set; }

        public DropDownListViewModel ddlDormName { get; set; }
    }
}