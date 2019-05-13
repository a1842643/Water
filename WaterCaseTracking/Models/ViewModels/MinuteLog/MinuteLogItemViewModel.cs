using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WaterCaseTracking.Models.ViewModels.MinuteLog {
    public class MinuteLogItemViewModel : _BaseRequestViewModel {
        public List<SearchItemViewModel> data { get; set; }
    }

    public class SearchItemViewModel {
        /// <summary>
        /// 流水號
        /// </summary>
        public string ID { get; set; }
        /// <summary>
        /// 操作員ID
        /// </summary>
        public string UserID { get; set; }
        /// <summary>
        /// 操作員Name
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 動作名稱
        /// </summary>
        public string FuncName { get; set; }
        /// <summary>
        /// 狀態
        /// </summary>
        public string Status { get; set; }
        /// <summary>
        /// 內容
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// 紀錄時間
        /// </summary>
        public string logged { get; set; }
       
    }
}