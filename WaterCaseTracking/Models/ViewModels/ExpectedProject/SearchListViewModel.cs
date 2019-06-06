﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WaterCaseTracking.Models.ViewModels.ProjectControll
{
    public class SearchListViewModel : _BaseRequestViewModel
    {
        public List<SearchItemViewModel> data { get; set; }
    }

    public class SearchItemViewModel
    {
        public string ID { get; set; }
        /// <summary>
        /// 項次+Guid
        /// </summary>
        public string HID { get; set; }
        public string ProjectName { get; set; }
        public string ContractAmount { get; set; }
        public string BeginDate { get; set; }
        public string PlanFinishDate { get; set; }
        public string PlanScheduleExpDate { get; set; }
        public string PlanScheduleReaDate { get; set; }
        public string Organizer { get; set; }
        public string OrganizerMan { get; set; }
        public string Remark { get; set; }
        public string CreateUserName { get; set; }
        public string CreateDate { get; set; }
        public string UpdateUserName { get; set; }
        public string UpdateDate { get; set; }
    }
}