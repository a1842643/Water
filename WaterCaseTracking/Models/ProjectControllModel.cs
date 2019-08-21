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
        [StringLength(100)]
        [DisplayName("工程名稱")]
        public string ProjectName { get; set; }
        [DisplayName("契約金額")]
        [MaxLength(18)]
        [RegularExpression(@"\d+(\.\d{1,2})?", ErrorMessage = "請輸入金額")]
        public string SContractAmount { get; set; }
        public decimal? ContractAmount { get; set; }
        [StringLength(10)]
        [DisplayName("開工日期")]
        public string BeginDate { get; set; }
        [StringLength(10)]
        [DisplayName("預訂完工日期")]
        public string PlanFinishDate { get; set; }
        [StringLength(100)]
        [DisplayName("預定進度")]
        public string PlanScheduleExpDate { get; set; }
        [StringLength(100)]
        [DisplayName("實際進度")]
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