using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WaterCaseTracking.Models.ViewModels.MinuteLog {
    public class QueryGetoTableViewModel : _BaseRequestViewModel {
        /// <summary>
        /// 操作人員下拉選單
        /// </summary>
        public string ddlUserName { get; set; }
        /// <summary>
        /// 功能下拉選單
        /// </summary>
        public string ddlFuncName { get; set; }
        /// <summary>
        /// 狀態下拉選單
        /// </summary>
        public string ddlStatus { get; set; }
        /// <summary>
        /// 紀錄時間(起)
        /// </summary>
        public string ddlloggedStart { get; set; }
        /// <summary>
        /// 紀錄時間(尾)
        /// </summary>
        public string ddlloggedEnd { get; set; }
        
    }
}