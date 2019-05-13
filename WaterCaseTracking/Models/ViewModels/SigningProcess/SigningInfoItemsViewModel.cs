using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WaterCaseTracking.Models.ViewModels.SigningProcess {
    public class SigningInfoItemsViewModel : _BaseRequestViewModel {
        /// <summary>
        /// 簽核狀態
        /// </summary>
        public int ddlStatus { get; set; }
        
        /// <summary>
        /// 編號
        /// </summary>
        public string _ID { get; set; }
    }
}