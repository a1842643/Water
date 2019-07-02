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
        string _txtInquiry;
        public string txtInquiry { get { return _txtInquiry; } set { _txtInquiry = HttpUtility.HtmlEncode(value); } }
        /// <summary>
        /// 辦理情形
        /// </summary>
        string _txtHandlingSituation;
        public string txtHandlingSituation { get { return _txtHandlingSituation; } set { _txtHandlingSituation = HttpUtility.HtmlEncode(value); } }
        /// <summary>
        /// 議員姓名
        /// </summary>
        string _txtMemberName;
        public string txtMemberName { get { return _txtMemberName; } set { _txtMemberName = HttpUtility.HtmlEncode(value); } }
        /// <summary>
        /// 地區
        /// </summary>
        string _ddlArea;
        public string ddlArea { get { return _ddlArea; } set { _ddlArea = HttpUtility.HtmlEncode(value); } }
        /// <summary>
        /// 承辦單位
        /// </summary>
        string _ddlOrganizer;
        public string ddlOrganizer { get { return _ddlOrganizer; } set { _ddlOrganizer = HttpUtility.HtmlEncode(value); } }
        /// <summary>
        /// 0:市政總質詢事項
        /// 1:議會案件
        /// </summary>
        string _Types;
        public string Types { get { return _Types; } set { _Types = HttpUtility.HtmlEncode(value); } }
    }
}