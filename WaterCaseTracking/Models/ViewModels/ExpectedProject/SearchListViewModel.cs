using System;
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
        public string AwardDate { get; set; }
        public string ContractDate { get; set; }
        public string ConstructionDate { get; set; }
        public string Duration { get; set; }
        public string Company { get; set; }
        public string ConstructionGap { get; set; }
        public string BehindReason { get; set; }
        public string Countermeasures { get; set; }
        public string ExtensionTimes { get; set; }
        public string ExtensionDays { get; set; }
        public string Changes { get; set; }
        public string ChangeAmount { get; set; }
        public string CompletedExpDate { get; set; }
        public string CompletedRelDate { get; set; }
        public string CorrectionAmount { get; set; }
        public string CumulativeValuation { get; set; }
        public string EstimateRate { get; set; }
        public string EstimateBehind { get; set; }
        public string EstimateBehindReason { get; set; }
        public string EstimateDate { get; set; }
        public string HandlingSituation { get; set; }
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