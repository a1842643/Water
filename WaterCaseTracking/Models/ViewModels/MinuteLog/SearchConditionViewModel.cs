using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WaterCaseTracking.Models.ViewModels.MinuteLog {
    public class SearchConditionViewModel : _BaseRequestViewModel {
        //操作人員下拉選單
        public DropDownListViewModel ddlUserName { get; set; }
        //功能下拉選單
        public DropDownListViewModel ddlFuncName { get; set; }
        //狀態下拉選單
        public DropDownListViewModel ddlStatus { get; set; }
        //紀錄時間(起)
        public DateTime ddlloggedStart { get; set; }
        //紀錄時間(尾)
        public DateTime ddlloggedEnd { get; set; }
    }
}