﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WaterCaseTracking.Models
{
    public class ExpectedProjectModel
    {
        [DisplayName("項次")]
        public string ID { get; set; }
        [Required]
        [StringLength(100)]
        [DisplayName("工程名稱")]
        public string ProjectName { get; set; }
        [StringLength(200)]
        [DisplayName("長官交辦日期")]
        public string ChiefExecutiveDate { get; set; }
        [StringLength(200)]
        [DisplayName("發包簽准預計日期")]
        public string SentAllowExpDate { get; set; }
        [StringLength(200)]
        [DisplayName("發包簽准實際日期")]
        public string SentAllowRelDate { get; set; }
        [StringLength(200)]
        [DisplayName("訂約日期")]
        public string ContractDate { get; set; }
        [DisplayName("前次辦理情形")]
        public string PreSituation { get; set; }
        [DisplayName("目前辦理情形")]
        public string HandlingSituation { get; set; }

        [StringLength(50)]
        [DisplayName("成案預計完成日期 ")]
        public string CrProExpDate { get; set; }
        [StringLength(50)]
        [DisplayName("成案實際完成日期")]
        public string CrProReaDate { get; set; }
        [StringLength(50)]
        [DisplayName("設計發包預計完成評選日期")]
        public string PlanExpDate { get; set; }
        [StringLength(50)]
        [DisplayName("設計發包實際完成評選日期")]
        public string PlanReaDate { get; set; }
        [StringLength(50)]
        [DisplayName("基本設計預計完成日期")]
        public string BasDesExpDate { get; set; }
        [StringLength(50)]
        [DisplayName("基本設計實際完成日期")]
        public string BasDesReaDate { get; set; }
        [StringLength(50)]
        [DisplayName("細部設計預計完成日期")]
        public string DetailDesExpDate { get; set; }
        [StringLength(50)]
        [DisplayName("細部設計實際完成日期")]
        public string DetailDesReaDate { get; set; }
        [StringLength(50)]
        [DisplayName("上網發包預計完成日期")]
        public string OnlineExpDate { get; set; }
        [StringLength(50)]
        [DisplayName("上網發包實際完成日期")]
        public string OnlineReaDate { get; set; }
        [StringLength(50)]
        [DisplayName("評選日期")]
        public string SelectionDate { get; set; }
        [StringLength(50)]
        [DisplayName("決標時間預計完成日期")]
        public string AwardExpDate { get; set; }
        [StringLength(50)]
        [DisplayName("決標時間實際完成日期")]
        public string AwardReaDate { get; set; }
        [Required]
        [StringLength(20)]
        [DisplayName("科室")]
        public string Organizer { get; set; }
        [StringLength(20)]
        [DisplayName("承辦人")]
        public string OrganizerMan { get; set; }
    }
}