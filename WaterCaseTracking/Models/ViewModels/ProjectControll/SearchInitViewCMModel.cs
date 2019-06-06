using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WaterCaseTracking.Models.ViewModels.ExpectedProject
{
    public class SearchInitCMViewModel : _BaseRequestViewModel
    {
        public DropDownListViewModel ddlOrganizer { get; set; }
    }
}