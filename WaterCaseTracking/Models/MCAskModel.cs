using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace WaterCaseTracking.Models
{
    public class MCAskModel
    {
        [DisplayName("項次")]
        public int ID { get; set; }
        [DisplayName("詢問日期")]
        public string AskDate { get; set; }
        [DisplayName("地區")]
        public string Area { get; set; }
        [DisplayName("議員姓名")]
        public string MemberName { get; set; }
        [DisplayName("詢問事項")]
        public string Inquiry { get; set; }
        [DisplayName("辦理情形")]
        public string HandlingSituation { get; set; }
        [DisplayName("承辦單位")]
        public string Organizer { get; set; }
        [DisplayName("承辦人員")]
        public string OrganizerMan { get; set; }
        [DisplayName("狀態")]
        public string sStatus { get; set; }
        /// <summary>
        /// 0:市政總質詢事項
        /// 1:議會案件
        /// </summary>
        public string Types { get; set; }
    }
}