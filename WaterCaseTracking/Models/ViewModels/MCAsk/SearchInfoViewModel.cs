using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WaterCaseTracking.Models.ViewModels.MCAsk
{
    public class SearchInfoViewModel : _BaseRequestViewModel
    {
        /// <summary>
        /// 詢問事項
        /// </summary>
        public string txtInquiry { get; set; }
        /// <summary>
        /// 辦理情形
        /// </summary>
        public string txtHandlingSituation { get; set; }
        /// <summary>
        /// 議員姓名
        /// </summary>
        public string txtMemberName { get; set; }
        /// <summary>
        /// 地區
        /// </summary>
        public string ddlArea { get; set; }
        /// <summary>
        /// 承辦單位
        /// </summary>
        public string ddlOrganizer { get; set; }
        /// <summary>
        /// 0:市政總質詢事項
        /// 1:議會案件
        /// </summary>
        public string Types { get; set; }
    }
}