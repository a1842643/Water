﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WaterCaseTracking.Models.ViewModels.BusinessUT
{
    public class ExportViewModel
    {
        /// <summary>
        /// 地區別
        /// </summary>
        public string ddlArea { get; set; }
        /// <summary>
        /// 分行
        /// </summary>
        public string ddlUntil { get; set; }
        /// <summary>
        /// 考核組別
        /// </summary>
        public string ddlAssessmentGroup { get; set; }
        /// <summary>
        /// 區域中心
        /// </summary>
        public string ddlAreacenterName { get; set; }
        /// <summary>
        /// 副總轄管區
        /// </summary>
        public string ddlGen_JurisName { get; set; }
        /// <summary>
        /// 行舍
        /// </summary>
        public string ddlDormName { get; set; }
        /// <summary>
        /// 副檔名
        /// </summary>
        public string fileExtension { get; set; }
    }
}