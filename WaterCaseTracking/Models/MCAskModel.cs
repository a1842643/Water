using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WaterCaseTracking.Models
{
    public class MCAskModel
    {
        [DisplayName("項次")]
        public string ID { get; set; }
        [StringLength(10)]
        //[DataType(DataType.Date)]
        //[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DisplayName("詢問日期")]
        public string AskDate { get; set; }
        [Required]
        [DataType(DataType.MultilineText)]
        [DisplayName("地區")]
        public string Area { get; set; }
        [Required]
        [StringLength(100)]
        [DisplayName("議員姓名")]
        public string MemberName { get; set; }
        [Required]
        [DisplayName("詢問事項")]
        public string Inquiry { get; set; }
        [Required]
        [DataType(DataType.MultilineText)]
        [AllowHtml]
        [DisplayName("辦理情形")]
        public string HandlingSituation { get; set; }
        [Required]
        [StringLength(20)]
        [DisplayName("承辦單位")]
        public string Organizer { get; set; }
        [StringLength(20)]
        [DisplayName("承辦人員")]
        public string OrganizerMan { get; set; }
        [StringLength(10)]
        [DisplayName("狀態")]
        public string sStatus { get; set; }
        /// <summary>
        /// GUID匯出匯入用
        /// </summary>
        public string NGuid { get; set; }
        /// <summary>
        /// 0:市政總質詢事項
        /// 1:議會案件
        /// </summary>
        public string Types { get; set; }
    }
}