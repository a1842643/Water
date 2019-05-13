using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WaterCaseTracking.Models.ViewModels.SigningProcess {
    public class SigningQueryItemViewModel : _BaseRequestViewModel {
        public List<SearchItemViewModel> data { get; set; }
    }

    public class SearchItemViewModel {
        /// <summary>
        /// 申請單編號(yyyyMMddHHmmss + 員編id)
        /// </summary>
        public string ProID { get; set; }
        /// <summary>
        /// 申請項目ID
        /// </summary>
        public string PID { get; set; }
        /// <summary>
        /// 申請項目Name
        /// </summary>
        public string PName { get; set; }
        /// <summary>
        /// 申請者ID
        /// </summary>
        public string UserID { get; set; }
        /// <summary>
        /// 申請者名稱
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 年月
        /// </summary>
        public string YM { get; set; }
        /// <summary>
        /// 申請上傳檔案名稱
        /// </summary>
        public string UploadName { get; set; }
        /// <summary>
        /// 申請上傳檔案路徑
        /// </summary>
        public string UploadPath { get; set; }
        /// <summary>
        /// 簽核主管編號
        /// </summary>
        public string SupervisorID { get; set; }
        /// <summary>
        /// 簽核主管姓名
        /// </summary>
        public string SupervisorName { get; set; }
        /// <summary>
        /// 覆核主管編號
        /// </summary>
        public string ReviewSupervisorID { get; set; }
        /// <summary>
        /// 覆核主管姓名
        /// </summary>
        public string ReviewSupervisorName { get; set; }
        /// <summary>
        /// 申請者註記
        /// </summary>
        public string MsgUser { get; set; }
        /// <summary>
        /// 簽核主管註記
        /// </summary>
        public string MsgSupervisor { get; set; }
        /// <summary>
        /// 覆核主管註記
        /// </summary>
        public string MsgReviewSupervisor { get; set; }
        /// <summary>
        /// 簽核狀態 - 因為匯出在SQL轉文字,故改成字串格式
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// 簽核狀態 - 因為匯出在SQL轉文字,故改成字串格式
        /// </summary>
        public string StatusText { get; set; }
        /// <summary>
        /// 作業區間(起)
        /// </summary>
        public string WorkTimeStart { get; set; }
        /// <summary>
        /// 作業區間(尾)
        /// </summary>
        public string WorkTimeEnd { get; set; }
        /// <summary>
        /// 有沒刪除申請單 1:開單,0:關單
        /// </summary>
        public bool NoDelete { get; set; }
    }
}