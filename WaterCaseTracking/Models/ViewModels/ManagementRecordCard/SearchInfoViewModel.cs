using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WaterCaseTracking.Models.ViewModels.ManagementRecordCard
{
    public class SearchInfoViewModel : DataTables.Parameters
    {
        public string Report { get; set; }

        public string Branch_No { get; set; }

        public string YeartMonth { get; set; }
        public string Area { get; set; }
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
        /// <summary>
    }
}