using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WaterCaseTracking.Models.ViewModels.JobReminder {
    public class UpdateViewModel : IValidatableObject {
            /// <summary>
            /// 群組代碼
            /// </summary>
            [StringLength(16)]
            [DisplayName("群組代碼")]
            public string GroupsCode { get; set; }
            /// <summary>
            /// 功能代碼
            /// </summary>
            [StringLength(16)]
            [DisplayName("功能代碼")]
            public string ActionCode { get; set; }
            /// <summary>
            /// 功能名稱
            /// </summary>
            [StringLength(20)]
            [DisplayName("功能名稱")]
            public string ActionName { get; set; }
            /// <summary>
            /// 工作期間(起)(日)
            /// </summary>
            [Required]
            [Range(1, 28)]
            [DisplayName("工作期間(起)(日)")]
            public int WorkStart { get; set; }
            /// <summary>
            /// 工作期間(訖)(日)
            /// </summary>
            [Required]
            [Range(1, 28)]
            [DisplayName("工作期間(訖)(日)")]
            public int WorkEnd { get; set; }
            /// <summary>
            /// 註記
            /// </summary>
            [RegularExpression(@"^[\u4e00-\u9fa5_a-zA-Z0-9]+$", ErrorMessage = "不可輸入特殊字元!!")]
            [StringLength(20)]
            [DisplayName("註記")]
            public string WorkName { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext) {
            if (WorkStart > WorkEnd)
                yield return new ValidationResult("工作期間(起)(日)不可大於(訖)(日)!!", new string[] { "WorkStart", "WorkEnd" });
        }
    }
}