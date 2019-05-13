using System;
using System.Collections.Generic;
namespace WaterCaseTracking.Models.ViewModels.BatchManage
{
    public class BatchManageListViewModel : _BaseRequestViewModel
    {
        public List<BatchManageItemModel> data { get; set; }
    }

    public class BatchManageInitUIViewModel : _BaseRequestViewModel
    {
        public DropDownListViewModel ddlLog_FuncName { get; set; }
    }
    public class BatchManageItemModel : _BaseRequestViewModel
    {
        public string Log_YM
        {
            get; set;
        }

        public string Log_JobID
        {
            get; set;
        }
        public string Log_JobName
        {
            get; set;
        }

        public string Log_FuncName
        {
            get; set;
        }
        public string Log_Description
        {
            get; set;
        }
        public decimal Log_ExRate
        {
            get; set;
        }
        public string Log_StartTime
        {
            get; set;
        }

        public string Log_EndTime
        {
            get; set;
        }

        public string Log_UserID
        {
            get; set;
        }

        public string Log_Status
        {
            get; set;
        }
        public string Log_StatusName
        {
            get; set;
        }
        public string Log_Text
        {
            get; set;
        }
    }


    public class BatchManageSearchItemModel : _BaseRequestViewModel
    {
        public string txtLog_YM
        {
            get; set;
        }

        public string ddlLog_JobID
        {
            get; set;
        }
        public string ddlLog_FuncName
        {
            get; set;
        }
    }
}