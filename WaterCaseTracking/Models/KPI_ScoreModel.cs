using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WaterCaseTracking.Models
{
    /// <summary>
    /// 步驟記錄檔
    /// </summary>
    public class KPI_ScoreModel
    {
        public Guid UUID { get; set; }
        public string Branch_No { get; set; }
        public int YEART { get; set; }
        public int MONTHS { get; set; }
        public decimal SCORE { get; set; }
        public string MODIFY_USER { get; set; }
        public DateTime MODIFY_DATE { get; set; }
        public int Sort { get; set; }
    }
}