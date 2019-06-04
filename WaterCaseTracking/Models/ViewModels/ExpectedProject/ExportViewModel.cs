using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WaterCaseTracking.Models.ViewModels.ExpectedProject
{
    public class ExportViewModel
    {
        /// <summary>
        /// 工程名稱
        /// </summary>
        public string txtProjectName { get; set; }
        /// <summary>
        /// 科室
        /// </summary>
        public string ddlOrganizer { get; set; }
        /// <summary>
        /// 副檔名
        /// </summary>
        public string fileExtension { get; set; }
    }
}