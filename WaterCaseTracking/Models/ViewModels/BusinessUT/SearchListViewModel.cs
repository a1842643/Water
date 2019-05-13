
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WaterCaseTracking.Models.ViewModels.BusinessUT
{
    public class SearchListViewModel : _BaseRequestViewModel
    {
        public List<SearchItemViewModel> data { get; set; }
    }

    public class SearchItemViewModel
    {
        /// <summary>
        /// 分行代號
        /// </summary>
        public string Branch_No { get; set; }
        /// <summary>
        /// 分行名稱
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 成立日期
        /// </summary>
        public string Create_Date { get; set; }
        /// <summary>
        /// 郵遞區號
        /// </summary>
        public string Zip_Code { get; set; }

        /// <summary>
        /// 住址
        /// </summary>
        public string Address { get; set; }
        /// <summary>
        /// 電話
        /// </summary>
        public string Telephone { get; set; }
        /// <summary>
        /// 地區
        /// </summary>
        public string AREA { get; set; }
        /// <summary>
        /// 傳真
        /// </summary>
        public string Fax { get; set; }
        /// <summary>
        /// 行舍
        /// </summary>
        public string DormName { get; set; }
        /// <summary>
        /// 經理
        /// </summary>
        public string Manager { get; set; }
        /// <summary>
        /// 到職日
        /// </summary>
        public string Join_Date { get; set; }
        /// <summary>
        /// 副理1
        /// </summary>
        public string Vice_Manager_1 { get; set; }
        /// <summary>
        /// 副理2
        /// </summary>
        public string Vice_Manager_2 { get; set; }
        /// <summary>
        /// 考核組別
        /// </summary>
        public string Ass_GroupName { get; set; }
        /// <summary>
        /// 區域中心
        /// </summary>
        public string AreacenterName { get; set; }
        /// <summary>
        /// 副總轄管區
        /// </summary>
        public string Gen_JurisName { get; set; }
        /// <summary>
        /// 備註
        /// </summary>
        public string Remark { get; set; }
    }
}