
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WaterCaseTracking.Models.ViewModels.Accounts
{
    public class SearchListViewModel : _BaseRequestViewModel
    {
        public List<SearchItemViewModel> data { get; set; }
    }

    public class SearchItemViewModel
    {
        /// <summary>
        /// 帳號
        /// </summary>
        public string AccountID { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        public string SecurityMena { get; set; }
        /// <summary>
        /// 角色
        /// </summary>
        public string RoleName { get; set; }
        /// <summary>
        /// 科室
        /// </summary>
        public string Organizer { get; set; }
        /// <summary>
        /// 停用啟用
        /// </summary>
        public bool IsEnable { get; set; }
        /// <summary>
        /// 新增人員
        /// </summary>
        public string CreateUserName { get; set; }

        /// <summary>
        /// 新增時間
        /// </summary>
        public string CreateDate { get; set; }
        /// <summary>
        /// 修改人員
        /// </summary>
        public string UpdateUserName { get; set; }
        /// <summary>
        /// 修改時間
        /// </summary>
        public string UpdateDate { get; set; }
    }
}