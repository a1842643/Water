using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WaterCaseTracking.Models.ViewModels.Accounts
{
    public class SearchInitViewModel : _BaseRequestViewModel
    {
        public DropDownListViewModel ddlRole { get; set; }
        public DropDownListViewModel ddlOrganizer { get; set; }
    }
}