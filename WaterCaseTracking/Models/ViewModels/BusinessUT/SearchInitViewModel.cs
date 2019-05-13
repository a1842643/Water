using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WaterCaseTracking.Models.ViewModels.BusinessUT
{
    public class SearchInitViewModel : _BaseRequestViewModel
    {
        public DropDownListViewModel ddlArea { get; set; }

        public DropDownListViewModel ddlUntil { get; set; }

        public DropDownListViewModel ddlAssessmentGroup { get; set; }

        public DropDownListViewModel ddlAreacenterName { get; set; }

        public DropDownListViewModel ddlGen_JurisName { get; set; }

        public DropDownListViewModel ddlDormName { get; set; }
    }
}