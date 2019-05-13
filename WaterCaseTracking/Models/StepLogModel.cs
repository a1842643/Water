using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WaterCaseTracking.Models
{
    /// <summary>
    /// 步驟記錄檔
    /// </summary>
    public class StepLogModel
    {
        /// <summary>
        /// 年份
        /// </summary>
        public int YEART { get; set; }
        /// <summary>
        /// 月份
        /// </summary>
        public int MONTH { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string GROUPS { get; set; }
        /// <summary>
        /// 是否已鎖定 0未鎖定:1已鎖定
        /// </summary>
        public int LOCKED { get; set; }
        /// <summary>
        /// 是否已上傳 0未上傳:1已上傳
        /// </summary>
        public int ISDOWN { get; set; }
    }
}