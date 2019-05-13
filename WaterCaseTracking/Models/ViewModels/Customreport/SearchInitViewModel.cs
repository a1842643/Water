using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WaterCaseTracking.Models.ViewModels.Customreport
{
    public class SearchInitViewModel : _BaseRequestViewModel
    {
        public DropDownListViewModel ddlSYM { get; set; }
        public DropDownListViewModel ddlEYM { get; set; }
        public DropDownListViewModel ddlFileColumns { get; set; }
        public DropDownListViewModel ddlArea { get; set; }
        public DropDownListViewModel ddlUntil { get; set; }
        public DropDownListViewModel ddlAssessmentGroup { get; set; }

        public DropDownListViewModel ddlAreacenterName { get; set; }

        public DropDownListViewModel ddlGen_JurisName { get; set; }
        public DropDownListViewModel ddlGroup{ get; set; }
        public DropDownListViewModel ddlClass { get; set; }
    }
}