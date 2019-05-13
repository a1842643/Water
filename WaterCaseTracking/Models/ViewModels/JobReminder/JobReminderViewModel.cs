using WaterCaseTracking.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WaterCaseTracking.Models.ViewModels.JobReminder {

    public class JobReminderViewModel : _BaseRequestViewModel {
        public List<SearchItemViewModel> data { get; set; }
    }
    //public class items {
    //    public string Text { get; set; }
    //    public string Value { get; set; }
    //}
    public class SearchItemViewModel {
        /// <summary>
        /// 群組代碼
        /// </summary>
        [StringLength(16)]
        [DisplayName("群組代碼")]
        public string GroupsCode { get; set; }
        /// <summary>
        /// 註記
        /// </summary>
        [RegularExpression(@"^[\u4e00-\u9fa5_a-zA-Z0-9]+$", ErrorMessage = "不可輸入特殊字元!!")]
        [StringLength(20)]
        [DisplayName("註記")]
        public string WorkName { get; set; }
        IEnumerable<SelectListItem> items = new List<SelectListItem>();
        public IEnumerable<SelectListItem> ActionGroups {
            get {
                items = new List<SelectListItem>()
                {
                new SelectListItem {Text="(人工上傳)信託部 - 各項手收合計數", Value="1"},
                new SelectListItem {Text="(人工上傳)信託部 - 聯行貼補手續費收入", Value="2"},
                new SelectListItem {Text="(人工上傳)風管部 - 資本成本", Value="3"},
                new SelectListItem {Text="(人工上傳)財管部 - 手續費收入營運概況表", Value="4"},
                new SelectListItem {Text="(人工上傳)國外部 - 代辦OBU盈餘調整數", Value="5"},
                new SelectListItem {Text="(人工上傳)國外部 - 營運概況表", Value="6"},
                new SelectListItem {Text="(人工上傳)債管部 - 呆帳息", Value="7"},            
                new SelectListItem {Text="(人工上傳)債管部 - 追債收回統計表", Value="8"},    
                new SelectListItem {Text="(人工上傳)債管部 - 應提備抵呆帳數", Value="9"},    
                new SelectListItem {Text="(人工上傳)債管部 - 營運概況表", Value="10"},        
                new SelectListItem {Text="(人工上傳)會計處 - 存放利率(累計)", Value="11"},    
                new SelectListItem {Text="(人工上傳)會計處 - 存放利率(當月)", Value="12"},    
                new SelectListItem {Text="(人工上傳)會計處 - 存放利率", Value="13"},          
                new SelectListItem {Text="(人工上傳)業務部 - 目標分配", Value="14"},          
                new SelectListItem {Text="(人工上傳)業務部 - 存款調整數", Value="15"},        
                new SelectListItem {Text="(管理考核)得分上傳", Value="16"},                   
                new SelectListItem {Text="(業績得分)上傳", Value="17"},                       
                new SelectListItem {Text="批次管理執行申請", Value="18"},                         
                };   
                return items;
            }
            set {
                items = value;
            } }
        public SelectList ActionGroupsitems { get; set; }

        /// <summary>
        /// 功能代碼
        /// </summary>
        [StringLength(16)]
        [DisplayName("功能代碼")]
        public string ActionCode { get; set; }
        /// <summary>
        /// 功能名稱
        /// </summary>
        [Required]
        [StringLength(20)]
        [DisplayName("功能名稱")]
        public string ActionName { get; set; }
        /// <summary>
        /// 年月
        /// </summary>
        [StringLength(5)]
        [DisplayName("年月")]
        public string YM { get; set; }
        /// <summary>
        /// 工作期間(起)(日)
        /// </summary>
        [Required]
        [Range(1, 28)]
        [DisplayName("工作期間(起)(日)")]
        public int WorkStart { get; set; }
        /// <summary>
        /// 工作期間(訖)(日)
        /// </summary>
        [Required]
        [Range(1,28)]
        [DisplayName("工作期間(訖)(日)")]
        public int WorkEnd { get; set; }
    }
}