using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WaterCaseTracking.Models
{
    public class ProjectControllModel
    {
        [DisplayName("項次")]
        public string ID { get; set; }
        [Required]
        [MaxLength(100)]
        [DisplayName("工程名稱")]
        public string ProjectName { get; set; }
        [MaxLength(200)]
        [DisplayName("決標日")]
        public string AwardDate { get; set; }
        [MaxLength(200)]
        [DisplayName("訂約日")]
        public string ContractDate { get; set; }
        [MaxLength(200)]
        [DisplayName("進場施工時間")]
        public string ConstructionDate { get; set; }
        [MaxLength(200)]
        [DisplayName("原工期")]
        public string Duration { get; set; }
        [MaxLength(200)]
        [DisplayName("承商")]
        public string Company { get; set; }
        [MaxLength(200)]
        [DisplayName("施工落差％")]
        public string ConstructionGap { get; set; }
        [MaxLength(200)]
        [DisplayName("施工落後原因")]
        public string BehindReason { get; set; }
        [MaxLength(200)]
        [DisplayName("因應對策及預訂期程")]
        public string Countermeasures { get; set; }
        [MaxLength(200)]
        [DisplayName("展期工期次數(累計)")]
        public string ExtensionTimes { get; set; }
        [MaxLength(200)]
        [DisplayName("展期工期天數(累計)")]
        public string ExtensionDays { get; set; }
        [MaxLength(200)]
        [DisplayName("變更設計")]
        public string Changes { get; set; }
        [MaxLength(200)]
        [DisplayName("變更設計變更增減金額(千元)")]
        public string ChangeAmount { get; set; }
        [MaxLength(200)]
        [DisplayName("完工預定日期")]
        public string CompletedExpDate { get; set; }
        [MaxLength(200)]
        [DisplayName("完工實際日期")]
        public string CompletedRelDate { get; set; }
        [MaxLength(200)]
        [DisplayName("修正契約總價(千元)")]
        public string CorrectionAmount { get; set; }
        [MaxLength(200)]
        [DisplayName("累計估驗計價(千元)")]
        public string CumulativeValuation { get; set; }
        [MaxLength(200)]
        [DisplayName("估驗款執行率")]
        public string EstimateRate { get; set; }
        [MaxLength(200)]
        [DisplayName("估驗款落後%")]
        public string EstimateBehind { get; set; }
        [MaxLength(200)]
        [DisplayName("估驗款進度延遲因素分析")]
        public string EstimateBehindReason { get; set; }
        [MaxLength(200)]
        [DisplayName("估驗提報日期")]
        public string EstimateDate { get; set; }
        [DisplayName("目前辦理情形")]
        public string HandlingSituation { get; set; }

        [DisplayName("契約金額（千元）")]
        [MaxLength(50)]
        //[RegularExpression(@"\d+(\.\d{1,2})?", ErrorMessage = "請輸入金額")]
        public string SContractAmount { get; set; }
        public string ContractAmount { get; set; }
        [StringLength(50)]
        [DisplayName("開工日期")]
        public string BeginDate { get; set; }
        [StringLength(50)]
        [DisplayName("原預訂完工日期")]
        public string PlanFinishDate { get; set; }
        [StringLength(65536)]
        [DisplayName("預定進度％")]
        public string PlanScheduleExpDate { get; set; }
        [StringLength(65536)]
        [DisplayName("實際進度％")]
        public string PlanScheduleReaDate { get; set; }
        [Required]
        [StringLength(20)]
        [DisplayName("科室")]
        public string Organizer { get; set; }
        [StringLength(20)]
        [DisplayName("承辦人")]
        public string OrganizerMan { get; set; }
        [DisplayName("備註")]
        [MaxLength(65536)]
        public string Remark { get; set; }
    }
}