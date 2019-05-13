using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WaterCaseTracking.Models.ViewModels.AverageCount
{
    public class ExportViewModel 
    {
      
        public string ddlYM { get; set; }
        public string ddlArea { get; set; }
        public string ddlUntil { get; set; }
        public string Ass_GroupName { get; set; }
        /// <summary>
        /// 區域中心
        /// </summary>
        public string AreacenterName { get; set; }
        /// <summary>
        /// 副總轄管區
        /// </summary>
        public string Gen_JurisName { get; set; }
       



    }
}