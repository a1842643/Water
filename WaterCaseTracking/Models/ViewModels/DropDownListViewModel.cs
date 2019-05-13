using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WaterCaseTracking.Models.ViewModels
{
    public class DropDownListViewModel
    {
        public List<DropDownListItem> DropDownListLT { get; set; }
    }
    public class DropDownListItem
    {
        public string Values { get; set; }
        public string Text { get; set; }
    }
}