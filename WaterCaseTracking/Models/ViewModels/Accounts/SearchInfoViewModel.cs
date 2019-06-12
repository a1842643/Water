using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WaterCaseTracking.Models.ViewModels.Accounts
{
    public class SearchInfoViewModel : _BaseRequestViewModel
    {
        /// <summary>
        /// 姓名
        /// </summary>
        public string txtSecurityMena { get; set; }
        /// <summary>
        /// 角色
        /// </summary>
        public string ddlRole { get; set; }
    }
}