using System;
using System.Collections.Generic;

namespace WaterCaseTracking.Models.ViewModels.BatchManage
{
    public class BatchLoggerModel : _BaseRequestViewModel
    {
        public string Log_Text { get; set; }
    }

    public class BatchLoggerSearchItemModel : _BaseRequestViewModel
    {
        public string YM { get; set; }
        public string JobID { get; set; }
        public string FuncN { get; set; }
    }
}