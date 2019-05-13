using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WaterCaseTracking.Models
{
    public class UntilModel
    {
        /// <summary>
        /// 分行代號
        /// </summary>
        [Required]
        [StringLength(6)]
        [DisplayName("分行代號")]
        public string Branch_No { get; set; }
        /// <summary>
        /// 分行名稱
        /// </summary>
        [Required]
        [StringLength(50)]
        [DisplayName("分行名稱")]
        public string Name { get; set; }
        /// <summary>
        /// 成立日期
        /// </summary>
        [DisplayName("成立日期")]
        public string Create_Date { get; set; }
        /// <summary>
        /// 住址
        /// </summary>
        [StringLength(9)]
        [DisplayName("郵遞區號")]
        public string Zip_Code { get; set; }
        /// <summary>
        /// 住址
        /// </summary>
        [StringLength(255)]
        [DisplayName("住址")]
        public string Address { get; set; }
        /// <summary>
        /// 電話
        /// </summary>
        [Required]
        [StringLength(30)]
        [DisplayName("電話")]
        public string Telephone { get; set; }
        /// <summary>
        /// 地區
        /// </summary>
        [Required]
        [StringLength(20)]
        [DisplayName("地區")]
        public string AREA { get; set; }
        /// <summary>
        /// 傳真
        /// </summary>
        [StringLength(30)]
        [DisplayName("傳真")]
        public string Fax { get; set; }
        /// <summary>
        /// 行舍
        /// </summary>
        [Required]
        [StringLength(2)]
        [DisplayName("行舍")]
        public string DormName { get; set; }
        /// <summary>
        /// 經理
        /// </summary>
        [StringLength(30)]
        [DisplayName("經理")]
        public string Manager { get; set; }
        /// <summary>
        /// 到職日
        /// </summary>
        [DisplayName("到職日")]
        public string Join_Date { get; set; }
        /// <summary>
        /// 副理1
        /// </summary>
        [StringLength(10)]
        [DisplayName("副理1")]
        public string Vice_Manager_1 { get; set; }
        /// <summary>
        /// 副理2
        /// </summary>
        [StringLength(10)]
        [DisplayName("副理2")]
        public string Vice_Manager_2 { get; set; }
        /// <summary>
        /// 考核組別
        /// </summary>
        [Required]
        [StringLength(20)]
        [DisplayName("考核組別")]
        public string Ass_GroupName { get; set; }
        /// <summary>
        /// 區域中心
        /// </summary>
        [Required]
        [StringLength(20)]
        [DisplayName("區域中心")]
        public string AreacenterName { get; set; }
        /// <summary>
        /// 副總轄管區
        /// </summary>
        [Required]
        [StringLength(20)]
        [DisplayName("副總轄管區")]
        public string Gen_JurisName { get; set; }
        /// <summary>
        /// 備註
        /// </summary>
        [StringLength(255)]
        [DisplayName("備註")]
        public string Remark { get; set; }

    }
}