using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WaterCaseTracking.Models
{
    public class AccountsModel
    {
        [Required]
        [MaxLength(20)]
        [DisplayName("帳號")]
        public string AccountID { get; set; }
        public string Password { get; set; }
        [Required]
        [MaxLength(100)]
        [DisplayName("帳號姓名")]
        public string AccountName { get; set; }
        [Required]
        [DisplayName("角色")]
        public string Role { get; set; }
        [Required]
        [DisplayName("科室")]
        public string Organizer { get; set; }
        public bool IsDefault { get; set; }
        public bool IsEnable { get; set; }

        public string CreateUserName { get; set; }
        public string CreateDate { get; set; }
        public string UpdateUserName { get; set; }
        public string UpdateDate { get; set; }
    }
}