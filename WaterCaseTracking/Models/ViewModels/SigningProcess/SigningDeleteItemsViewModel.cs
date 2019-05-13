using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WaterCaseTracking.Models.ViewModels.SigningProcess {
    public class SigningDeleteItemsViewModel : _BaseRequestViewModel {
        /// <summary>
        /// 申請單編號(yyyyMMddHHmmss + 員編id)
        /// </summary>
        public string ProID { get; set; }
        /// <summary>
        /// 登入者ID
        /// </summary>
        public string ID { get; set; }
        /// <summary>
        /// 登入者名稱
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 簽核狀態
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// 簽核主管編號
        /// </summary>
        public string SupervisorID { get; set; }
        /// <summary>
        /// 簽核主管姓名
        /// </summary>
        public string SupervisorName { get; set; }
        /// <summary>
        /// 簽核主管註記
        /// </summary>
        public string MsgSupervisor { get; set; }
        /// <summary>
        /// 覆核主管註記
        /// </summary>
        public string MsgReviewSupervisor { get; set; }
    }
}