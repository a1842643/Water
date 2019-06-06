using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WaterCaseTracking.Models.ViewModels.ProjectControll
{
    public class SearchInfoViewModel : _BaseRequestViewModel
    {
        /// <summary>
        /// 工程名稱
        /// </summary>
        public string txtProjectName { get; set; }
        /// <summary>
        /// 科室
        /// </summary>
        public string ddlOrganizer { get; set; }
    }
}