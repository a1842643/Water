using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WaterCaseTracking.Models
{
    public class UseCodeModel
    {
        /// <summary>
        /// 群組代碼
        /// </summary>
        public string GROUP_CODE { get; set; }
        /// <summary>
        /// 項目代碼
        /// </summary>
        public string ITEM_CODE { get; set; }
        /// <summary>
        /// 項目名稱
        /// </summary>
        public string ITEM_NAME { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        public int SORT { get; set; }
        /// <summary>
        /// 備註
        /// </summary>
        public string REMARK { get; set; }
        /// <summary>
        /// 修改人員
        /// </summary>
        public string MODIFY_USER { get; set; }
        /// <summary>
        /// 修改日期
        /// </summary>
        public DateTime MODIFY_DATE { get; set; }
    }
}