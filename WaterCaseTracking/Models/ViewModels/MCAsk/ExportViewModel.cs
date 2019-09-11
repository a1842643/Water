using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WaterCaseTracking.Models.ViewModels.MCAsk
{
    public class ExportViewModel
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
        /// 0:議會模擬問答事項
        /// 1:議員詢問事項
        /// </summary>
        public string Types { get; set; }
        /// <summary>
        /// 副檔名
        /// </summary>
        public string fileExtension { get; set; }
    }
}