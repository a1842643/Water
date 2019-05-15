using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WaterCaseTracking.Models.ViewModels.MCAsk
{
    public class SearchInitCMViewModel : _BaseRequestViewModel
    {
        public DropDownListViewModel ddlArea { get; set; }
        public DropDownListViewModel ddlsStatus { get; set; }
        public DropDownListViewModel ddlOrganizer { get; set; }
    }
}