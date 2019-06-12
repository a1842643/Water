using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WaterCaseTracking.Models.ViewModels.Accounts
{
    public class ChangePwViewModel
    {
        public string AccountID { get; set; }
        [MinLength(8)]
        [MaxLength(20)]
        [RegularExpression(@"(?=.{8,})((?=.*\d)(?=.*[a-z])(?=.*[A-Z])|(?=.*\d)(?=.*[a-zA-Z])(?=.*[\W_])|(?=.*[a-z])(?=.*[A-Z])(?=.*[\W_])).*",ErrorMessage = "密碼需包含英文大寫小寫、特殊符號或數字三種以上")]
        public string alienSecurity1 { get; set; }
        public string UpdateUserName { get; set; }
    }
}