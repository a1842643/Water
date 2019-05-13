using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WaterCaseTracking.Models.ViewModels.ProductList
{
    public class detailViewModel
    {
        #region Properties											 
        /// <summary>
        /// 產品編號
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// 產品名稱
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 數量
        /// </summary>
        public int Qty { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// 類別編號
        /// </summary>
        public string CategoryId { get; set; }
        /// <summary>
        /// 價錢
        /// </summary>
        public decimal Price { get; set; }
        /// <summary>
        /// 建立日期
        /// </summary>
        public System.DateTime CreationDate { get; set; }
        /// <summary>
        /// 修改日期
        /// </summary>
        public System.DateTime ModifiedDate { get; set; }
        /// <summary>
        /// 狀態
        /// </summary>
        public bool Status { get; set; }
        #endregion
    }
}