using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WaterCaseTracking.Models {
    public class ManualUploadModel {
        /// <summary>
        /// 申請單號 yyyyMMddHHmmss(14) +人員編號(6) +預留(2)
        /// </summary>
        [Required]
        [StringLength(22)]
        [DisplayName("申請單號")]
        public string ID { get; set; }

        /// <summary>
        /// 分行代碼
        /// </summary>
        [Required]
        [StringLength(6)]
        [DisplayName("分行代碼")]
        public string Branch_No { get; set; }

        /// <summary>
        /// 年月
        /// </summary>
        [Required]
        [StringLength(6)]
        [DisplayName("年月")]
        public string YM { get; set; }

        /// <summary>
        /// 單別(選擇的是哪個Excel匯入)
        /// </summary>
        [Required]
        [StringLength(2)]
        [DisplayName("單別(選擇的是哪個Excel匯入)")]
        public string Category { get; set; }
        
        /// <summary>
        /// 業務部-目標分配-總存款
        /// </summary>
        [Required]
        [StringLength(10)]
        [DisplayName("業務部-目標分配-總存款")]
        public string A01 { get; set; }

        /// <summary>
        /// 業務部-目標分配-活期性存款
        /// </summary>
        [Required]
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(13, 0)")]
        [DisplayName("業務部-目標分配-活期性存款")]
        public string A02 { get; set; }

        /// <summary>
        /// 業務部-目標分配-自有資金放款
        /// </summary>
        [Required]
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(13, 0)")]
        [DisplayName("業務部-目標分配-自有資金放款")]
        public string A03 { get; set; }
        /// <summary>
        /// 業務部-目標分配-外匯承作量
        /// </summary>
        [Required]
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(13, 0)")]
        [DisplayName("業務部-目標分配-外匯承作量")]
        public string A04 { get; set; }
        /// <summary>
        /// 業務部-目標分配-盈餘
        /// </summary>
        [Required]
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(13, 0)")]
        [DisplayName("業務部-目標分配-盈餘")]
        public string A05 { get; set; }
        /// <summary>
        /// 業務部-目標分配-外匯存款-DBU
        /// </summary>
        [Required]
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(13, 0)")]
        [DisplayName("業務部-目標分配-外匯存款-DBU")]
        public string A06 { get; set; }
        /// <summary>
        /// 業務部-目標分配-外匯存款-代辦OBU
        /// </summary>
        [Required]
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(13, 0)")]
        [DisplayName("業務部-目標分配-外匯存款-代辦OBU")]
        public string A07 { get; set; }
        /// <summary>
        /// 業務部-目標分配-外幣授信-DBU
        /// </summary>
        [Required]
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(13, 0)")]
        [DisplayName("業務部-目標分配-外幣授信-DBU")]
        public string A08 { get; set; }
        /// <summary>
        /// 業務部-目標分配-外幣授信-代辦OBU
        /// </summary>
        [Required]
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(13, 0)")]
        [DisplayName("業務部-目標分配-外幣授信-代辦OBU")]
        public string A09 { get; set; }
        /// <summary>
        /// 會計處-存放利率(當月)-(存款)利率
        /// </summary>
        [Required]
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(13, 2)")]
        [DisplayName("會計處-存放利率(當月)-(存款)利率")]
        public string A10 { get; set; }
        /// <summary>
        /// 會計處-存放利率(當月)-(放款)利率
        /// </summary>
        [Required]
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(13, 2)")]
        [DisplayName("會計處-存放利率(當月)-(放款)利率")]
        public string A11 { get; set; }
        /// <summary>
        /// 會計處-存放利率(當月)-利差
        /// </summary>
        [Required]
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(13, 2)")]
        [DisplayName("會計處-存放利率(當月)-利差")]
        public string A12 { get; set; }
        /// <summary>
        /// 會計處-存放利率(累計)-利差
        /// </summary>
        [Required]
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(13, 2)")]
        [DisplayName("會計處-存放利率(累計)-利差")]
        public string A13 { get; set; }
        /// <summary>
        /// 會計處-存放利率(累計)-(存款)利率
        /// </summary>
        [Required]
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(13, 2)")]
        [DisplayName("會計處-存放利率(累計)-(存款)利率")]
        public string A14 { get; set; }
        /// <summary>
        /// 會計處-存放利率(累計)-(放款)利率
        /// </summary>
        [Required]
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(13, 2)")]
        [DisplayName("會計處-存放利率(累計)-(放款)利率")]
        public string A15 { get; set; }

        /// <summary>
        /// 財管部-手續費收入營運概況表-受託投資有價證券業務(本月)
        /// </summary>
        [Required]
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(13, 0)")]
        [DisplayName("財管部-手續費收入營運概況表-受託投資有價證券業務(本月)")]
        public string A16 { get; set; }
        /// <summary>
        /// 財管部-手續費收入營運概況表-銀行保險業務(本月)
        /// </summary>
        [Required]
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(13, 0)")]
        [DisplayName("財管部-手續費收入營運概況表-銀行保險業務(本月)")]
        public string A17 { get; set; }
        /// <summary>
        /// 財管部-手續費收入營運概況表-合計(本月)
        /// </summary>
        [Required]
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(13, 0)")]
        [DisplayName("財管部-手續費收入營運概況表-合計(本月)")]
        public string A18 { get; set; }
        /// <summary>
        /// 財管部-手續費收入營運概況表-受託投資有價證券業務(累計)
        /// </summary>
        [Required]
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(13, 0)")]
        [DisplayName("財管部-手續費收入營運概況表-受託投資有價證券業務(累計)")]
        public string A19 { get; set; }
        /// <summary>
        /// 財管部-手續費收入營運概況表-受託投資有價證券業務(累計)
        /// </summary>
        [Required]
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(13, 0)")]
        [DisplayName("財管部-手續費收入營運概況表-銀行保險業務(累計)")]
        public string A20 { get; set; }
        /// <summary>
        /// 財管部-手續費收入營運概況表-受託投資有價證券業務(累計)
        /// </summary>
        [Required]
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(13, 0)")]
        [DisplayName("財管部-手續費收入營運概況表-合計(累計)")]
        public string A21 { get; set; }
        /// <summary>
        /// 財管部-手續費收入營運概況表累計達成率
        /// </summary>
        [Required]
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(13, 2)")]
        [DisplayName("財管部-手續費收入營運概況表累計達成率")]
        public string A22 { get; set; }
        /// <summary>
        /// 財管部-手續費收入營運概況表(Y-1年M月)受託投資有價證券業務
        /// </summary>
        [Required]
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(13, 0)")]
        [DisplayName("財管部-手續費收入營運概況表(Y-1年M月)受託投資有價證券業務")]
        public string A23 { get; set; }
        /// <summary>
        /// 財管部-手續費收入營運概況表(Y-1年M月)銀行保險業務
        /// </summary>
        [Required]
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(13, 0)")]
        [DisplayName("財管部-手續費收入營運概況表(Y-1年M月)銀行保險業務")]
        public string A24 { get; set; }
        /// <summary>
        /// 財管部-手續費收入營運概況表(Y-1年M月)合計
        /// </summary>
        [Required]
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(13, 0)")]
        [DisplayName("財管部-手續費收入營運概況表(Y-1年M月)合計")]
        public string A25 { get; set; }
        /// <summary>
        /// 財管部-手續費收入營運概況表(與去年同期比較金額)受託投資有價證券業務
        /// </summary>
        [Required]
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(13, 0)")]
        [DisplayName("財管部-手續費收入營運概況表(與去年同期比較金額)受託投資有價證券業務")]
        public string A26 { get; set; }
        /// <summary>
        /// 財管部-手續費收入營運概況表(與去年同期比較金額)銀行保險業務
        /// </summary>
        [Required]
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(13, 0)")]
        [DisplayName("財管部-手續費收入營運概況表(與去年同期比較金額)銀行保險業務")]
        public string A27 { get; set; }
        /// <summary>
        /// 財管部-手續費收入營運概況表(與去年同期比較金額)合計
        /// </summary>
        [Required]
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(13, 0)")]
        [DisplayName("財管部-手續費收入營運概況表(與去年同期比較金額)合計")]
        public string A28 { get; set; }
        /// <summary>
        /// 財管部-手續費收入營運概況表(與去年同期比較比率)受託投資有價證券業務
        /// </summary>
        [Required]
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(13, 2)")]
        [DisplayName("財管部-手續費收入營運概況表(與去年同期比較比率)受託投資有價證券業務")]
        public string A29 { get; set; }
        /// <summary>
        /// 財管部-手續費收入營運概況表(與去年同期比較比率)銀行保險業務
        /// </summary>
        [Required]
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(13, 2)")]
        [DisplayName("財管部-手續費收入營運概況表(與去年同期比較比率)銀行保險業務")]
        public string A30 { get; set; }
        /// <summary>
        /// 財管部-手續費收入營運概況表(與去年同期比較比率)合計
        /// </summary>
        [Required]
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(13, 2)")]
        [DisplayName("財管部-手續費收入營運概況表(與去年同期比較比率)合計")]
        public string A31 { get; set; }
        /// <summary>
        /// 信託部-聯行貼補手續費收入-受託投資有價證券業務(本月)
        /// </summary>
        [Required]
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(13, 0)")]
        [DisplayName("信託部-聯行貼補手續費收入-受託投資有價證券業務(本月)")]
        public string A32 { get; set; }
        /// <summary>
        /// 信託部-聯行貼補手續費收入-受託投資有價證券業務(本年度累計)
        /// </summary>
        [Required]
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(13, 0)")]
        [DisplayName("信託部-聯行貼補手續費收入-受託投資有價證券業務(本年度累計)")]
        public string A33 { get; set; }
        /// <summary>
        /// 信託部-聯行貼補手續費收入-不動產信託業務(本月)
        /// </summary>
        [Required]
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(13, 0)")]
        [DisplayName("信託部-聯行貼補手續費收入-不動產信託業務(本月)")]
        public string A34 { get; set; }
        /// <summary>
        /// 信託部-聯行貼補手續費收入-不動產信託業務(本年度累計)
        /// </summary>
        [Required]
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(13, 0)")]
        [DisplayName("信託部-聯行貼補手續費收入-不動產信託業務(本年度累計)")]
        public string A35 { get; set; }
        /// <summary>
        /// 信託部-聯行貼補手續費收入-租賃權信託業務(本月)
        /// </summary>
        [Required]
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(13, 0)")]
        [DisplayName("信託部-聯行貼補手續費收入-租賃權信託業務(本月)")]
        public string A36 { get; set; }
        /// <summary>
        /// 信託部-聯行貼補手續費收入-租賃權信託業務(本年度累計)
        /// </summary>
        [Required]
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(13, 0)")]
        [DisplayName("信託部-聯行貼補手續費收入-租賃權信託業務(本年度累計)")]
        public string A37 { get; set; }
        /// <summary>
        /// 信託部-聯行貼補手續費收入-簽證業務(本月)
        /// </summary>
        [Required]
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(13, 0)")]
        [DisplayName("信託部-聯行貼補手續費收入-簽證業務(本月)")]
        public string A38 { get; set; }
        /// <summary>
        /// 信託部-聯行貼補手續費收入-簽證業務(本年度累計)
        /// </summary>
        [Required]
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(13, 0)")]
        [DisplayName("信託部-聯行貼補手續費收入-簽證業務(本年度累計)")]
        public string A39 { get; set; }
        /// <summary>
        /// 信託部-聯行貼補手續費收入-地上權信託業務(本月)
        /// </summary>
        [Required]
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(13, 0)")]
        [DisplayName("信託部-聯行貼補手續費收入-地上權信託業務(本月)")]
        public string A40 { get; set; }
        /// <summary>
        /// 信託部-聯行貼補手續費收入-地上權信託業務(本年度累計)
        /// </summary>
        [Required]
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(13, 0)")]
        [DisplayName("信託部-聯行貼補手續費收入-地上權信託業務(本年度累計)")]
        public string A41 { get; set; }
        /// <summary>
        /// 信託部-聯行貼補手續費收入-公司債發行受託人(本月)
        /// </summary>
        [Required]
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(13, 0)")]
        [DisplayName("信託部-聯行貼補手續費收入-公司債發行受託人(本月)")]
        public string A42 { get; set; }
        /// <summary>
        /// 信託部-聯行貼補手續費收入-公司債發行受託人(本年度累計)
        /// </summary>
        [Required]
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(13, 0)")]
        [DisplayName("信託部-聯行貼補手續費收入-公司債發行受託人(本年度累計)")]
        public string A43 { get; set; }
        /// <summary>
        /// 信託部-聯行貼補手續費收入-外國債及結構債券(本月)
        /// </summary>
        [Required]
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(13, 0)")]
        [DisplayName("信託部-聯行貼補手續費收入-外國債及結構債券(本月)")]
        public string A44 { get; set; }
        /// <summary>
        /// 信託部-聯行貼補手續費收入-外國債及結構債券(本年度累計)
        /// </summary>
        [Required]
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(13, 0)")]
        [DisplayName("信託部-聯行貼補手續費收入-外國債及結構債券(本年度累計)")]
        public string A45 { get; set; }
        /// <summary>
        /// 信託部-聯行貼補手續費收入-財產信託業務(本月)
        /// </summary>
        [Required]
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(13, 0)")]
        [DisplayName("信託部-聯行貼補手續費收入-財產信託業務(本月)")]
        public string A46 { get; set; }
        /// <summary>
        /// 信託部-聯行貼補手續費收入-財產信託業務(本年度累計)
        /// </summary>
        [Required]
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(13, 0)")]
        [DisplayName("信託部-聯行貼補手續費收入-財產信託業務(本年度累計)")]
        public string A47 { get; set; }
        /// <summary>
        /// 信託部-聯行貼補手續費收入-合　　計(本月)
        /// </summary>
        [Required]
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(13, 0)")]
        [DisplayName("信託部-聯行貼補手續費收入-合　　計(本月)")]
        public string A48 { get; set; }
        /// <summary>
        /// 信託部-聯行貼補手續費收入-合　　計(本年度累計)
        /// </summary>
        [Required]
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(13, 0)")]
        [DisplayName("信託部-聯行貼補手續費收入-合　　計(本年度累計)")]
        public string A49 { get; set; }
        /// <summary>
        /// 業務部- 活期存款調整數
        /// </summary>
        [Required]
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(13, 0)")]
        [DisplayName("業務部-存款調整數- 活期存款")]
        public string A50 { get; set; }
        /// <summary>
        /// 業務部- 定期存款調整數
        /// </summary>
        [Required]
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(13, 0)")]
        [DisplayName("業務部-存款調整數- 定期存款")]
        public string A51 { get; set; }
        /// <summary>
        /// 業務部- 公庫存款調整數
        /// </summary>
        [Required]
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(13, 0)")]
        [DisplayName("業務部-存款調整數- 公庫存款")]
        public string A52 { get; set; }
        /// <summary>
        /// 業務部- 總存款調整數
        /// </summary>
        [Required]
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(13, 0)")]
        [DisplayName("業務部-存款調整數- 總存款")]
        public string A53 { get; set; }
        /// <summary>
        /// 債管部-應提備抵呆帳數-本月
        /// </summary>
        [Required]
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(13, 0)")]
        [DisplayName("債管部-應提備抵呆帳數-本月")]
        public string A54 { get; set; }
        /// <summary>
        /// 債管部-應提備抵呆帳數-截至X月底累計
        /// </summary>
        [Required]
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(13, 0)")]
        [DisplayName("債管部-應提備抵呆帳數-截至X月底累計")]
        public string A55 { get; set; }
        /// <summary>
        /// 債管部-應提備抵呆帳數-代辦OBU業務備呆
        /// </summary>
        [Required]
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(13, 0)")]
        [DisplayName("債管部-應提備抵呆帳數-代辦OBU業務備呆")]
        public string A56 { get; set; }
        /// <summary>
        /// 債管部-追債收回統計表-追索債權本金新增累計(B)
        /// </summary>
        [Required]
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(13, 0)")]
        [DisplayName("債管部-追債收回統計表-追索債權本金新增累計(B)")]
        public string A57 { get; set; }
        /// <summary>
        /// 債管部-追債收回統計表-本月份截止累計達成額(J)
        /// </summary>
        [Required]
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(13, 0)")]
        [DisplayName("債管部-追債收回統計表-本月份截止累計達成額(J)")]
        public string A58 { get; set; }
        /// <summary>
        /// 信託部-各項手收合計數-手續費收入合計數(含證券經紀及承銷收入
        /// </summary>
        [Required]
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(13, 0)")]
        [DisplayName("信託部-各項手收合計數-手續費收入合計數(含證券經紀及承銷收入")]
        public string A59 { get; set; }
        /// <summary>
        /// 風管部-資本成本-資本成本
        /// </summary>
        [Required]
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(13, 0)")]
        [DisplayName("風管部-資本成本-資本成本")]
        public string A60 { get; set; }
        /// <summary>
        /// 國外部-代辦OBU盈餘調整數-合計
        /// </summary>
        [Required]
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(13, 0)")]
        [DisplayName("國外部-代辦OBU盈餘調整數-合計")]
        public string A61 { get; set; }
        /// <summary>
        /// 債管部-呆帳息-F欄
        /// </summary>
        [Required]
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(13, 0)")]
        [DisplayName("債管部-呆帳息-F欄")]
        public string A62 { get; set; }
        /// <summary>
        /// 財管部-手續費收入營運概況表本年度目標
        /// </summary>
        [Required]
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(13, 0)")]
        [DisplayName("財管部-手續費收入營運概況表本年度目標")]
        public string A63 { get; set; }
        /// <summary>
        /// 總存法定目標
        /// </summary>
        [Required]
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(13, 0)")]
        [DisplayName("總存法定目標")]
        public string A64 { get; set; }
        /// <summary>
        /// 活存法定目標
        /// </summary>
        [Required]
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(13, 0)")]
        [DisplayName("活存法定目標")]
        public string A65 { get; set; }
        /// <summary>
        /// 活存法定目標(含公庫)
        /// </summary>
        [Required]
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(13, 0)")]
        [DisplayName("活存法定目標(含公庫)")]
        public string A66 { get; set; }
        /// <summary>
        /// 公庫法定目標
        /// </summary>
        [Required]
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(13, 0)")]
        [DisplayName("公庫法定目標")]
        public string A67 { get; set; }
        /// <summary>
        /// 自有資金法定目標
        /// </summary>
        [Required]
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(13, 0)")]
        [DisplayName("自有資金法定目標")]
        public string A68 { get; set; }
        /// <summary>
        /// 外匯法定目標
        /// </summary>
        [Required]
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(13, 0)")]
        [DisplayName("外匯法定目標")]
        public string A69 { get; set; }
        /// <summary>
        /// 盈餘法定目標
        /// </summary>
        [Required]
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(13, 0)")]
        [DisplayName("盈餘法定目標")]
        public string A70 { get; set; }
        /// <summary>
        /// 本月份底止逾期放款 金額 (甲+乙)類（列報+列管）
        /// </summary>
        [Required]
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(13, 0)")]
        [DisplayName("本月份底止逾期放款 金額 (甲+乙)類（列報+列管）")]
        public string A71 { get; set; }
        /// <summary>
        /// 本月份底止逾期放款 比率 (甲+乙)類（列報+列管）
        /// </summary>
        [Required]
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(13, 0)")]
        [DisplayName("本月份底止逾期放款 比率 (甲+乙)類（列報+列管）")]
        public string A72 { get; set; }
        /// <summary>
        /// 本外匯承作額--本月止累計外匯(國內分行)
        /// </summary>
        [Required]
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(13, 0)")]
        [DisplayName("外匯承作額--本月止累計外匯(國內分行)")]
        public string A73 { get; set; }
    }
}