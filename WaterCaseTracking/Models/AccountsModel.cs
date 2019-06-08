using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WaterCaseTracking.Models
{
    public class AccountsModel
    {
        public string AccountID { get; set; }
        public string Password { get; set; }
        public string AccountName { get; set; }
        public string Role { get; set; }
        public string Organizer { get; set; }
        public bool IsDefault { get; set; }

        public string CreateUserName { get; set; }
        public string CreateDate { get; set; }
        public string UpdateUserName { get; set; }
        public string UpdateDate { get; set; }
    }
}