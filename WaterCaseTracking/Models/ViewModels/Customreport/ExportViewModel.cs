﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WaterCaseTracking.Models.ViewModels.Customreport
{
    public class ExportViewModel 
    {
      
        public string ddlSYM { get; set; }
        public string ddlEYM { get; set; }
        public string ddlFileColumns { get; set; }
        public string ddlUntil { get; set; }
        public string ddlArea { get; set; }
      
        /// 考核組別
        /// </summary>
        public string Ass_GroupName { get; set; }
        /// <summary>
        /// 區域中心
        /// </summary>
        public string AreacenterName { get; set; }
        /// <summary>
        /// 副總轄管區
        /// </summary>
        public string Gen_JurisName { get; set; }
        public string FileColumns { get; set; }
        public string tablename { get; set; }
        /// <summary>



    }
}