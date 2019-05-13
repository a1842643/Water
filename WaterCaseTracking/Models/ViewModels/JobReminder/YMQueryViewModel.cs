using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WaterCaseTracking.Models.ViewModels.JobReminder {
    public class YMQueryViewModel {
        /// <summary>
        /// 群組代碼
        /// </summary>
        public string GroupsCode { get; set; }
        /// <summary>
        /// 年月
        /// </summary>
        public string YM { get; set; }
    }
}