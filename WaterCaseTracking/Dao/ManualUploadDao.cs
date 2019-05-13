using Dapper;
using WaterCaseTracking.Models;
using WaterCaseTracking.Models.ViewModels.BusinessUT;
using WaterCaseTracking.Models.ViewModels.SigningProcess;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;

namespace WaterCaseTracking.Dao {
    public class ManualUploadDao : _BaseDao {

        #region 上傳資料
        internal int AddTempManualUpload(List<ManualUploadModel> listModel ,string FileValues ,out SqlConnection conn, out SqlTransaction trans) {

            #region 參數設定
            //組立SQL字串並連接資料庫
            conn = GetOpenConnection();
            trans = GetTransaction(conn);
            StringBuilder _sqlStr = new StringBuilder();
            #endregion
            #region 流程
            _sqlParamsList = new List<DynamicParameters>();
            #region swich case(_sqlStr)
            switch (FileValues) {
                case "1"://信託部 - 各項手收合計數
                         //N'信託部-各項手收合計數-手續費收入合計數(含證券經紀及承銷收入' , N'A59'
                    _sqlStr.Append(@"Insert Into Temp_ManualUpload ( 
                                    ID
                                    ,Branch_No                         --分行代號
                                    ,YM                                --年月
                                    ,Category                          --單別(選擇的是哪個Excel匯入)
                                    ,A59--信託部-各項手收合計數-手續費收入合計數(含證券經紀及承銷收入
                                )
                                Values(
                                    @ID
                                    ,@Branch_No             
                                    ,@YM
                                    ,@Category
                                    ,@A59
                                )
                        ");
                    break;
                case "2"://信託部 - 聯行貼補手續費收入
                         //N'信託部-聯行貼補手續費收入-受託投資有價證券業務(本月)' , N'A32'
                         //N'信託部-聯行貼補手續費收入-受託投資有價證券業務(本年度累計)' , N'A33'
                         //N'信託部-聯行貼補手續費收入-不動產信託業務(本月)' , N'A34'
                         //N'信託部-聯行貼補手續費收入-不動產信託業務(本年度累計)' , N'A35'
                         //N'信託部-聯行貼補手續費收入-租賃權信託業務(本月)' , N'A36'
                         //N'信託部-聯行貼補手續費收入-租賃權信託業務(本年度累計)' , N'A37'
                         //N'信託部-聯行貼補手續費收入-簽證業務(本月)' , N'A38'
                         //N'信託部-聯行貼補手續費收入-簽證業務(本年度累計)' , N'A39'
                         //N'信託部-聯行貼補手續費收入-地上權信託業務(本月)' , N'A40'
                         //N'信託部-聯行貼補手續費收入-地上權信託業務(本年度累計)' , N'A41'
                         //N'信託部-聯行貼補手續費收入-公司債發行受託人(本月)' , N'A42'
                         //N'信託部-聯行貼補手續費收入-公司債發行受託人(本年度累計)' , N'A43'
                         //N'信託部-聯行貼補手續費收入-外國債及結構債券(本月)' , N'A44'
                         //N'信託部-聯行貼補手續費收入-外國債及結構債券(本年度累計)' , N'A45'
                         //N'信託部-聯行貼補手續費收入-財產信託業務(本月)' , N'A46'
                         //N'信託部-聯行貼補手續費收入-財產信託業務(本年度累計)' , N'A47'
                         //N'信託部-聯行貼補手續費收入-合　　計(本月)' , N'A48'
                         //N'信託部-聯行貼補手續費收入-合　　計(本年度累計)' , N'A49'
                    _sqlStr.Append(@"Insert Into Temp_ManualUpload ( 
                                    ID
                                    ,Branch_No                         --分行代號
                                    ,YM                               --年月
                                    ,Category                          --單別(選擇的是哪個Excel匯入)
                                    ,A32--信託部-聯行貼補手續費收入-受託投資有價證券業務(本月)
                                    ,A33--信託部-聯行貼補手續費收入-受託投資有價證券業務(本年度累計)
                                    ,A34--信託部-聯行貼補手續費收入-不動產信託業務(本月)
                                    ,A35--信託部-聯行貼補手續費收入-不動產信託業務(本年度累計)
                                    ,A36--信託部-聯行貼補手續費收入-租賃權信託業務(本月)
                                    ,A37--信託部-聯行貼補手續費收入-租賃權信託業務(本年度累計)
                                    ,A38--信託部-聯行貼補手續費收入-簽證業務(本月)
                                    ,A39--信託部-聯行貼補手續費收入-簽證業務(本年度累計)
                                    ,A40--信託部-聯行貼補手續費收入-地上權信託業務(本月)
                                    ,A41--信託部-聯行貼補手續費收入-地上權信託業務(本年度累計)
                                    ,A42--信託部-聯行貼補手續費收入-公司債發行受託人(本月)
                                    ,A43--信託部-聯行貼補手續費收入-公司債發行受託人(本年度累計)
                                    ,A44--信託部-聯行貼補手續費收入-外國債及結構債券(本月)
                                    ,A45--信託部-聯行貼補手續費收入-外國債及結構債券(本年度累計)
                                    ,A46--信託部-聯行貼補手續費收入-財產信託業務(本月)
                                    ,A47--信託部-聯行貼補手續費收入-財產信託業務(本年度累計)
                                    ,A48--信託部-聯行貼補手續費收入-合　　計(本月)
                                    ,A49--信託部-聯行貼補手續費收入-合　　計(本年度累計)
                                )
                                Values(
                                    @ID
                                    ,@Branch_No             
                                    ,@YM
                                    ,@Category
                                    ,@A32
                                    ,@A33
                                    ,@A34
                                    ,@A35
                                    ,@A36
                                    ,@A37
                                    ,@A38
                                    ,@A39
                                    ,@A40
                                    ,@A41
                                    ,@A42
                                    ,@A43
                                    ,@A44
                                    ,@A45
                                    ,@A46
                                    ,@A47
                                    ,@A48
                                    ,@A49
                                )
                        ");
                    break;
                case "3"://風管部 - 資本成本
                         //N'風管部-資本成本-資本成本' , N'A60'
                    _sqlStr.Append(@"Insert Into Temp_ManualUpload ( 
                                    ID
                                    ,Branch_No                         --分行代號
                                    ,YM                               --年月
                                    ,Category                          --單別(選擇的是哪個Excel匯入)
                                    ,A60--風管部-資本成本-資本成本
                                )
                                Values(
                                    @ID
                                    ,@Branch_No             
                                    ,@YM
                                    ,@Category
                                    ,@A60
                                )
                        ");
                    break;
                case "4"://財管部 - 手續費收入營運概況表
                         //N'財管部-手續費收入營運概況表-受託投資有價證券業務(本月)' , N'A16'
                         //N'財管部-手續費收入營運概況表-銀行保險業務(本月)' , N'A17'
                         //N'財管部-手續費收入營運概況表-合計(本月)' , N'A18'
                         //N'財管部-手續費收入營運概況表-受託投資有價證券業務(累計)' , N'A19'
                         //N'財管部-手續費收入營運概況表-銀行保險業務(累計)' , N'A20'
                         //N'財管部-手續費收入營運概況表-合計(累計)' , N'A21'
                         //N'財管部-手續費收入營運概況表-累計達成率' , N'A22'
                         //N'財管部-手續費收入營運概況表-(Y-1年M月)受託投資有價證券業務' , N'A23'
                         //N'財管部-手續費收入營運概況表-(Y-1年M月)銀行保險業務' , N'A24'
                         //N'財管部-手續費收入營運概況表-(Y-1年M月)合計' , N'A25'
                         //N'財管部-手續費收入營運概況表-(與去年同期比較金額)受託投資有價證券業務' , N'A26'
                         //N'財管部-手續費收入營運概況表-(與去年同期比較金額)銀行保險業務' , N'A27'
                         //N'財管部-手續費收入營運概況表-(與去年同期比較金額)合計' , N'A28'
                         //N'財管部-手續費收入營運概況表-(與去年同期比較比率)受託投資有價證券業務' , N'A29'
                         //N'財管部-手續費收入營運概況表-(與去年同期比較比率)銀行保險業務' , N'A30'
                         //N'財管部-手續費收入營運概況表-(與去年同期比較比率)合計' , N'A31'
                         //N'財管部-手續費收入營運概況表-本年度目標' , N'A63'
                    _sqlStr.Append(@"Insert Into Temp_ManualUpload ( 
                                    ID
                                    ,Branch_No                         --分行代號
                                    ,YM                               --年月
                                    ,Category                          --單別(選擇的是哪個Excel匯入)
                                    ,A16--財管部-手續費收入營運概況表-受託投資有價證券業務(本月)
                                    ,A17--財管部-手續費收入營運概況表-銀行保險業務(本月)
                                    ,A18--財管部-手續費收入營運概況表-合計(本月)
                                    ,A19--財管部-手續費收入營運概況表-受託投資有價證券業務(累計)
                                    ,A20--財管部-手續費收入營運概況表-銀行保險業務(累計)
                                    ,A21--財管部-手續費收入營運概況表-合計(累計)
                                    ,A22--財管部-手續費收入營運概況表累計達成率
                                    ,A23--財管部-手續費收入營運概況表(Y-1年M月)受託投資有價證券業務
                                    ,A24--財管部-手續費收入營運概況表(Y-1年M月)銀行保險業務
                                    ,A25--財管部-手續費收入營運概況表(Y-1年M月)合計
                                    ,A26--財管部-手續費收入營運概況表(與去年同期比較金額)受託投資有價證券業務
                                    ,A27--財管部-手續費收入營運概況表(與去年同期比較金額)銀行保險業務
                                    ,A28--財管部-手續費收入營運概況表(與去年同期比較金額)合計
                                    ,A29--財管部-手續費收入營運概況表(與去年同期比較比率)受託投資有價證券業務
                                    ,A30--財管部-手續費收入營運概況表(與去年同期比較比率)銀行保險業務
                                    ,A31--財管部-手續費收入營運概況表(與去年同期比較比率)合計
                                    ,A63--財管部-手續費收入營運概況表-本年度目標
                                )
                                Values(
                                    @ID
                                    ,@Branch_No             
                                    ,@YM
                                    ,@Category
                                    ,@A16
                                    ,@A17
                                    ,@A18
                                    ,@A19
                                    ,@A20
                                    ,@A21
                                    ,@A22
                                    ,@A23
                                    ,@A24
                                    ,@A25
                                    ,@A26
                                    ,@A27
                                    ,@A28
                                    ,@A29
                                    ,@A30
                                    ,@A31
                                    ,@A63 
                                )
                        ");
                    break;
                case "5"://國外部 - 代辦OBU盈餘調整數
                         //N'國外部-代辦OBU盈餘調整數-合計' , N'A61'
                    _sqlStr.Append(@"Insert Into Temp_ManualUpload ( 
                                    ID
                                    ,Branch_No                         --分行代號
                                    ,YM                               --年月
                                    ,Category                          --單別(選擇的是哪個Excel匯入)
                                    ,A61--國外部-代辦OBU盈餘調整數-合計
                                )
                                Values(
                                    @ID
                                    ,@Branch_No             
                                    ,@YM
                                    ,@Category
                                    ,@A61
                                )
                        ");
                    break;
                case "6"://國外部 - 營運概況表
                    break;
                case "7"://債管部 - 呆帳息
                         //N'債管部-呆帳息-F欄' , N'A62'
                    _sqlStr.Append(@"Insert Into Temp_ManualUpload ( 
                                    ID
                                    ,Branch_No                         --分行代號
                                    ,YM                               --年月
                                    ,Category                          --單別(選擇的是哪個Excel匯入)
                                    ,A62--債管部-呆帳息-F欄
                                )
                                Values(
                                    @ID
                                    ,@Branch_No             
                                    ,@YM
                                    ,@Category
                                    ,@A62
                                )
                        ");
                    break;
                case "8"://債管部 - 追債收回統計表
                         //N'債管部-追債收回統計表-追索債權本金新增累計(B)' , N'A57'
                         //N'債管部-追債收回統計表-本月份截止累計達成額(J)' , N'A58'
                    _sqlStr.Append(@"Insert Into Temp_ManualUpload ( 
                                    ID
                                    ,Branch_No                         --分行代號
                                    ,YM                               --年月
                                    ,Category                          --單別(選擇的是哪個Excel匯入)
                                    ,A57--債管部-追債收回統計表-追索債權本金新增累計(B)
                                    ,A58--債管部-追債收回統計表-本月份截止累計達成額(J)
                                )
                                Values(
                                    @ID
                                    ,@Branch_No             
                                    ,@YM
                                    ,@Category
                                    ,@A57
                                    ,@A58
                                )
                        ");
                    break;
                case "9"://債管部 - 應提備抵呆帳數
                         //N'債管部-應提備抵呆帳數-本月' , N'A54'
                         //N'債管部-應提備抵呆帳數-截至X月底累計' , N'A55'
                         //N'債管部-應提備抵呆帳數-代辦OBU業務備呆' , N'A56'
                    _sqlStr.Append(@"Insert Into Temp_ManualUpload ( 
                                    ID
                                    ,Branch_No                         --分行代號
                                    ,YM                               --年月
                                    ,Category                          --單別(選擇的是哪個Excel匯入)
                                    ,A54--債管部-應提備抵呆帳數-本月
                                    ,A55--債管部-應提備抵呆帳數-截至X月底累計
                                    ,A56--債管部-應提備抵呆帳數-代辦OBU業務備呆
                                )
                                Values(
                                    @ID
                                    ,@Branch_No             
                                    ,@YM
                                    ,@Category
                                    ,@A54
                                    ,@A55
                                    ,@A56
                                )
                        ");
                    break;
                case "10"://債管部 - 營運概況表
                    break;
                case "11"://會計處 - 存放利率(累計)
                          //N'會計處-存放利率(累計)-利差' , N'A13'
                          //N'會計處-存放利率(累計)-(存款)利率' , N'A14'
                          //N'會計處-存放利率(累計)-(放款)利率' , N'A15'
                    _sqlStr.Append(@"Insert Into Temp_ManualUpload ( 
                                    ID
                                    ,Branch_No                         --分行代號
                                    ,YM                               --年月
                                    ,Category                          --單別(選擇的是哪個Excel匯入)
                                    ,A13--會計處-存放利率(累計)-利差
                                    ,A14--會計處-存放利率(累計)-(存款)利率
                                    ,A15--會計處-存放利率(累計)-(放款)利率
                                )
                                Values(
                                    @ID
                                    ,@Branch_No             
                                    ,@YM
                                    ,@Category
                                    ,@A13
                                    ,@A14
                                    ,@A15
                                )
                        ");
                    break;
                case "12"://會計處 - 存放利率(當月)
                          //N'會計處-存放利率(當月)-(存款)利率' , N'A10'
                          //N'會計處-存放利率(當月)-(放款)利率' , N'A11'
                          //N'會計處-存放利率(當月)-利差' , N'A12'
                    _sqlStr.Append(@"Insert Into Temp_ManualUpload ( 
                                    ID
                                    ,Branch_No                         --分行代號
                                    ,YM                               --年月
                                    ,Category                          --單別(選擇的是哪個Excel匯入)
                                    ,A10--會計處-存放利率(當月)-(存款)利率
                                    ,A11--會計處-存放利率(當月)-(放款)利率
                                    ,A12--會計處-存放利率(當月)-利差
                                )
                                Values(
                                    @ID
                                    ,@Branch_No             
                                    ,@YM
                                    ,@Category
                                    ,@A10
                                    ,@A11
                                    ,@A12
                                )
                        ");
                    break;
                case "13"://會計處 - 存放利率
                    break;
                case "14"://業務部 - 目標分配
                          //N'業務部-目標分配-總存款' , N'A01'
                          //N'業務部-目標分配-活期性存款' , N'A02'
                          //N'業務部-目標分配-自有資金放款' , N'A03'
                          //N'業務部-目標分配-外匯承作量' , N'A04'
                          //N'業務部-目標分配-盈餘' , N'A05'
                          //N'業務部-目標分配-外匯存款-DBU' , N'A06'
                          //N'業務部-目標分配-外匯存款-代辦OBU' , N'A07'
                          //N'業務部-目標分配-外幣授信-DBU' , N'A08'
                          //N'業務部-目標分配-外幣授信-代辦OBU' , N'A09'
                    _sqlStr.Append(@"Insert Into Temp_ManualUpload ( 
                                    ID
                                    ,Branch_No                         --分行代號
                                    ,YM                               --年月
                                    ,Category                          --單別(選擇的是哪個Excel匯入)
                                    ,A01--業務部-目標分配-總存款
                                    ,A02--業務部-目標分配-活期性存款
                                    ,A03--業務部-目標分配-自有資金放款
                                    ,A04--業務部-目標分配-外匯承作量
                                    ,A05--業務部-目標分配-盈餘
                                    ,A06--業務部-目標分配-外匯存款-DBU
                                    ,A07--業務部-目標分配-外匯存款-代辦OBU
                                    ,A08--業務部-目標分配-外幣授信-DBU
                                    ,A09--業務部-目標分配-外幣授信-代辦OBU
                                )
                                Values(
                                    @ID
                                    ,@Branch_No             
                                    ,@YM
                                    ,@Category
                                    ,@A01
                                    ,@A02
                                    ,@A03
                                    ,@A04
                                    ,@A05
                                    ,@A06
                                    ,@A07
                                    ,@A08
                                    ,@A09
                                )
                        ");
                    break;
                case "15"://業務部 - 存款調整數
                          //N'業務部-存款調整數- 活期存款' , N'A50'
                          //N'業務部-存款調整數- 定期存款' , N'A51'
                          //N'業務部-存款調整數- 公庫存款' , N'A52'
                          //N'業務部-存款調整數- 總存款' , N'A53'
                    _sqlStr.Append(@"Insert Into Temp_ManualUpload ( 
                                    ID
                                    ,Branch_No                         --分行代號
                                    ,YM                               --年月
                                    ,Category                          --單別(選擇的是哪個Excel匯入)
                                    ,A50--業務部-活期存款調整數
                                    ,A51--業務部-定期存款調整數
                                    ,A52--業務部-公庫存款調整數
                                    ,A53--業務部-總存款調整數
                                )
                                Values(
                                     @ID
                                    ,@Branch_No             
                                    ,@YM
                                    ,@Category
                                    ,@A50
                                    ,@A51
                                    ,@A52
                                    ,@A53
                                )
                        ");
                    break;
            }
            #endregion
            foreach (var model in listModel) {
                _sqlParams = new Dapper.DynamicParameters();
                #region 舊全欄位寫入
                //_sqlStr.Append(@"Insert Into ManualUpload ( 
                //                   Branch_No                         --分行代號
                //                   , YM                              --年月
                //                   ,A01--業務部-目標分配-總存款
                //                   ,A02--業務部-目標分配-活期性存款
                //                   ,A03--業務部-目標分配-自有資金放款
                //                   ,A04--業務部-目標分配-外匯承作量
                //                   ,A05--業務部-目標分配-盈餘
                //                   ,A06--業務部-目標分配-外匯存款-DBU
                //                   ,A07--業務部-目標分配-外匯存款-代辦OBU
                //                   ,A08--業務部-目標分配-外幣授信-DBU
                //                   ,A09--業務部-目標分配-外幣授信-代辦OBU
                //                   ,A10--會計處-存放利率(當月)-(存款)利率
                //                   ,A11--會計處-存放利率(當月)-(放款)利率
                //                   ,A12--會計處-存放利率(當月)-利差
                //                   ,A13--會計處-存放利率(累計)-利差
                //                   ,A14--會計處-存放利率(累計)-(存款)利率
                //                   ,A15--會計處-存放利率(累計)-(放款)利率

                //                   ,A63--財管部-手續費收入營運概況表-本年度目標
                //                   ,A16--財管部-手續費收入營運概況表-受託投資有價證券業務(本月)
                //                   ,A17--財管部-手續費收入營運概況表-銀行保險業務(本月)
                //                   ,A18--財管部-手續費收入營運概況表-合計(本月)
                //                   ,A19--財管部-手續費收入營運概況表-受託投資有價證券業務(累計)
                //                   ,A20--財管部-手續費收入營運概況表-銀行保險業務(累計)
                //                   ,A21--財管部-手續費收入營運概況表-合計(累計)
                //                   ,A22--財管部-手續費收入營運概況表累計達成率
                //                   ,A23--財管部-手續費收入營運概況表(Y-1年M月)受託投資有價證券業務
                //                   ,A24--財管部-手續費收入營運概況表(Y-1年M月)銀行保險業務
                //                   ,A25--財管部-手續費收入營運概況表(Y-1年M月)合計
                //                   ,A26--財管部-手續費收入營運概況表(與去年同期比較金額)受託投資有價證券業務
                //                   ,A27--財管部-手續費收入營運概況表(與去年同期比較金額)銀行保險業務
                //                   ,A28--財管部-手續費收入營運概況表(與去年同期比較金額)合計
                //                   ,A29--財管部-手續費收入營運概況表(與去年同期比較比率)受託投資有價證券業務
                //                   ,A30--財管部-手續費收入營運概況表(與去年同期比較比率)銀行保險業務
                //                   ,A31--財管部-手續費收入營運概況表(與去年同期比較比率)合計

                //                   ,A32--信託部-聯行貼補手續費收入-受託投資有價證券業務(本月)
                //                   ,A33--信託部-聯行貼補手續費收入-受託投資有價證券業務(本年度累計)
                //                   ,A34--信託部-聯行貼補手續費收入-不動產信託業務(本月)
                //                   ,A35--信託部-聯行貼補手續費收入-不動產信託業務(本年度累計)
                //                   ,A36--信託部-聯行貼補手續費收入-租賃權信託業務(本月)
                //                   ,A37--信託部-聯行貼補手續費收入-租賃權信託業務(本年度累計)
                //                   ,A38--信託部-聯行貼補手續費收入-簽證業務(本月)
                //                   ,A39--信託部-聯行貼補手續費收入-簽證業務(本年度累計)
                //                   ,A40--信託部-聯行貼補手續費收入-地上權信託業務(本月)
                //                   ,A41--信託部-聯行貼補手續費收入-地上權信託業務(本年度累計)
                //                   ,A42--信託部-聯行貼補手續費收入-公司債發行受託人(本月)
                //                   ,A43--信託部-聯行貼補手續費收入-公司債發行受託人(本年度累計)
                //                   ,A44--信託部-聯行貼補手續費收入-外國債及結構債券(本月)
                //                   ,A45--信託部-聯行貼補手續費收入-外國債及結構債券(本年度累計)
                //                   ,A46--信託部-聯行貼補手續費收入-財產信託業務(本月)
                //                   ,A47--信託部-聯行貼補手續費收入-財產信託業務(本年度累計)
                //                   ,A48--信託部-聯行貼補手續費收入-合　　計(本月)
                //                   ,A49--信託部-聯行貼補手續費收入-合　　計(本年度累計)

                //                   ,A50--業務部-活期存款調整數
                //                   ,A51--業務部-定期存款調整數
                //                   ,A52--業務部-公庫存款調整數
                //                   ,A53--業務部-總存款調整數
                //                   ,A54--債管部-應提備抵呆帳數-本月
                //                   ,A55--債管部-應提備抵呆帳數-截至X月底累計
                //                   ,A56--債管部-應提備抵呆帳數-代辦OBU業務備呆
                //                   ,A57--債管部-追債收回統計表-追索債權本金新增累計(B)
                //                   ,A58--債管部-追債收回統計表-本月份截止累計達成額(J)
                //                   ,A59--信託部-各項手收合計數-手續費收入合計數(含證券經紀及承銷收入
                //                   ,A60--風管部-資本成本-資本成本
                //                   ,A61--國外部-代辦OBU盈餘調整數-合計
                //                   ,A62--債管部-呆帳息-F欄
                //                   ,A64--總存法定目標
                //                   ,A65--活存法定目標
                //                   ,A66--活存法定目標(含公庫)
                //                   ,A67--公庫法定目標
                //                   ,A68--自有資金法定目標
                //                   ,A69--外匯法定目標
                //                   ,A70--盈餘法定目標
                //                   ,A71--本月份底止逾期放款 金額 (甲+乙)類（列報+列管）
                //                   ,A72--本月份底止逾期放款 比率 (甲+乙)類（列報+列管）
                //                   ,A73--外匯承作額--本月止累計外匯(國內分行)
                //               )
                //               Values(
                //                   @Branch_No             
                //                   ,@YM
                //                   ,@A01
                //                   ,@A02
                //                   ,@A03
                //                   ,@A04
                //                   ,@A05
                //                   ,@A06
                //                   ,@A07
                //                   ,@A08
                //                   ,@A09
                //                   ,@A10
                //                   ,@A11
                //                   ,@A12
                //                   ,@A13
                //                   ,@A14
                //                   ,@A15
                //                   ,@A63
                //                   ,@A16
                //                   ,@A17
                //                   ,@A18
                //                   ,@A19
                //                   ,@A20
                //                   ,@A21
                //                   ,@A22
                //                   ,@A23
                //                   ,@A24
                //                   ,@A25
                //                   ,@A26
                //                   ,@A27
                //                   ,@A28
                //                   ,@A29
                //                   ,@A30
                //                   ,@A31
                //                   ,@A32
                //                   ,@A33
                //                   ,@A34
                //                   ,@A35
                //                   ,@A36
                //                   ,@A37
                //                   ,@A38
                //                   ,@A39
                //                   ,@A40
                //                   ,@A41
                //                   ,@A42
                //                   ,@A43
                //                   ,@A44
                //                   ,@A45
                //                   ,@A46
                //                   ,@A47
                //                   ,@A48
                //                   ,@A49
                //                   ,@A50
                //                   ,@A51
                //                   ,@A52
                //                   ,@A53
                //                   ,@A54
                //                   ,@A55
                //                   ,@A56
                //                   ,@A57
                //                   ,@A58
                //                   ,@A59
                //                   ,@A60
                //                   ,@A61
                //                   ,@A62
                //                   ,@A64
                //                   ,@A65
                //                   ,@A66
                //                   ,@A67
                //                   ,@A68
                //                   ,@A69
                //                   ,@A70
                //                   ,@A71
                //                   ,@A72
                //                   ,@A73
                //               )
                //   ");
                #endregion
                #region swich case(_sqlParams)
                switch (FileValues) {
                case "1"://信託部 - 各項手收合計數
                         //N'信託部-各項手收合計數-手續費收入合計數(含證券經紀及承銷收入' , N'A59'
                        _sqlParams.Add("A59", model.A59);
                        break;
                case "2"://信託部 - 聯行貼補手續費收入
                         //N'信託部-聯行貼補手續費收入-受託投資有價證券業務(本月)' , N'A32'
                         //N'信託部-聯行貼補手續費收入-受託投資有價證券業務(本年度累計)' , N'A33'
                         //N'信託部-聯行貼補手續費收入-不動產信託業務(本月)' , N'A34'
                         //N'信託部-聯行貼補手續費收入-不動產信託業務(本年度累計)' , N'A35'
                         //N'信託部-聯行貼補手續費收入-租賃權信託業務(本月)' , N'A36'
                         //N'信託部-聯行貼補手續費收入-租賃權信託業務(本年度累計)' , N'A37'
                         //N'信託部-聯行貼補手續費收入-簽證業務(本月)' , N'A38'
                         //N'信託部-聯行貼補手續費收入-簽證業務(本年度累計)' , N'A39'
                         //N'信託部-聯行貼補手續費收入-地上權信託業務(本月)' , N'A40'
                         //N'信託部-聯行貼補手續費收入-地上權信託業務(本年度累計)' , N'A41'
                         //N'信託部-聯行貼補手續費收入-公司債發行受託人(本月)' , N'A42'
                         //N'信託部-聯行貼補手續費收入-公司債發行受託人(本年度累計)' , N'A43'
                         //N'信託部-聯行貼補手續費收入-外國債及結構債券(本月)' , N'A44'
                         //N'信託部-聯行貼補手續費收入-外國債及結構債券(本年度累計)' , N'A45'
                         //N'信託部-聯行貼補手續費收入-財產信託業務(本月)' , N'A46'
                         //N'信託部-聯行貼補手續費收入-財產信託業務(本年度累計)' , N'A47'
                         //N'信託部-聯行貼補手續費收入-合　　計(本月)' , N'A48'
                         //N'信託部-聯行貼補手續費收入-合　　計(本年度累計)' , N'A49'
                        _sqlParams.Add("A32", model.A32);
                        _sqlParams.Add("A33", model.A33);
                        _sqlParams.Add("A34", model.A34);
                        _sqlParams.Add("A35", model.A35);
                        _sqlParams.Add("A36", model.A36);
                        _sqlParams.Add("A37", model.A37);
                        _sqlParams.Add("A38", model.A38);
                        _sqlParams.Add("A39", model.A39);
                        _sqlParams.Add("A40", model.A40);
                        _sqlParams.Add("A41", model.A41);
                        _sqlParams.Add("A42", model.A42);
                        _sqlParams.Add("A43", model.A43);
                        _sqlParams.Add("A44", model.A44);
                        _sqlParams.Add("A45", model.A45);
                        _sqlParams.Add("A46", model.A46);
                        _sqlParams.Add("A47", model.A47);
                        _sqlParams.Add("A48", model.A48);
                        _sqlParams.Add("A49", model.A49);
                        break;
                case "3"://風管部 - 資本成本
                         //N'風管部-資本成本-資本成本' , N'A60'
                        _sqlParams.Add("A60", model.A60);
                        break;
                case "4"://財管部 - 手續費收入營運概況表
                         //N'財管部-手續費收入營運概況表-受託投資有價證券業務(本月)' , N'A16'
                         //N'財管部-手續費收入營運概況表-銀行保險業務(本月)' , N'A17'
                         //N'財管部-手續費收入營運概況表-合計(本月)' , N'A18'
                         //N'財管部-手續費收入營運概況表-受託投資有價證券業務(累計)' , N'A19'
                         //N'財管部-手續費收入營運概況表-銀行保險業務(累計)' , N'A20'
                         //N'財管部-手續費收入營運概況表-合計(累計)' , N'A21'
                         //N'財管部-手續費收入營運概況表-累計達成率' , N'A22'
                         //N'財管部-手續費收入營運概況表-(Y-1年M月)受託投資有價證券業務' , N'A23'
                         //N'財管部-手續費收入營運概況表-(Y-1年M月)銀行保險業務' , N'A24'
                         //N'財管部-手續費收入營運概況表-(Y-1年M月)合計' , N'A25'
                         //N'財管部-手續費收入營運概況表-(與去年同期比較金額)受託投資有價證券業務' , N'A26'
                         //N'財管部-手續費收入營運概況表-(與去年同期比較金額)銀行保險業務' , N'A27'
                         //N'財管部-手續費收入營運概況表-(與去年同期比較金額)合計' , N'A28'
                         //N'財管部-手續費收入營運概況表-(與去年同期比較比率)受託投資有價證券業務' , N'A29'
                         //N'財管部-手續費收入營運概況表-(與去年同期比較比率)銀行保險業務' , N'A30'
                         //N'財管部-手續費收入營運概況表-(與去年同期比較比率)合計' , N'A31'
                         //N'財管部-手續費收入營運概況表-本年度目標' , N'A63'
                        _sqlParams.Add("A16", model.A16);
                        _sqlParams.Add("A17", model.A17);
                        _sqlParams.Add("A18", model.A18);
                        _sqlParams.Add("A19", model.A19);
                        _sqlParams.Add("A20", model.A20);
                        _sqlParams.Add("A21", model.A21);
                        _sqlParams.Add("A22", model.A22);
                        _sqlParams.Add("A23", model.A23);
                        _sqlParams.Add("A24", model.A24);
                        _sqlParams.Add("A25", model.A25);
                        _sqlParams.Add("A26", model.A26);
                        _sqlParams.Add("A27", model.A27);
                        _sqlParams.Add("A28", model.A28);
                        _sqlParams.Add("A29", model.A29);
                        _sqlParams.Add("A30", model.A30);
                        _sqlParams.Add("A31", model.A31);
                        _sqlParams.Add("A63", model.A63);
                        break;
                case "5"://國外部 - 代辦OBU盈餘調整數
                         //N'國外部-代辦OBU盈餘調整數-合計' , N'A61'
                        _sqlParams.Add("A61", model.A61);
                        break;
                case "6"://國外部 - 營運概況表
                    break;
                case "7"://債管部 - 呆帳息
                         //N'債管部-呆帳息-F欄' , N'A62'
                        _sqlParams.Add("A62", model.A62);
                        break;
                case "8"://債管部 - 追債收回統計表
                         //N'債管部-追債收回統計表-追索債權本金新增累計(B)' , N'A57'
                         //N'債管部-追債收回統計表-本月份截止累計達成額(J)' , N'A58'
                        _sqlParams.Add("A57", model.A57);
                        _sqlParams.Add("A58", model.A58);
                        break;
                case "9"://債管部 - 應提備抵呆帳數
                         //N'債管部-應提備抵呆帳數-本月' , N'A54'
                         //N'債管部-應提備抵呆帳數-截至X月底累計' , N'A55'
                         //N'債管部-應提備抵呆帳數-代辦OBU業務備呆' , N'A56'
                        _sqlParams.Add("A54", model.A54);
                        _sqlParams.Add("A55", model.A55);
                        _sqlParams.Add("A56", model.A56);
                        break;
                case "10"://債管部 - 營運概況表
                    break;
                case "11"://會計處 - 存放利率(累計)
                          //N'會計處-存放利率(累計)-利差' , N'A13'
                          //N'會計處-存放利率(累計)-(存款)利率' , N'A14'
                          //N'會計處-存放利率(累計)-(放款)利率' , N'A15'
                        _sqlParams.Add("A13", model.A13);
                        _sqlParams.Add("A14", model.A14);
                        _sqlParams.Add("A15", model.A15);
                        break;
                case "12"://會計處 - 存放利率(當月)
                          //N'會計處-存放利率(當月)-(存款)利率' , N'A10'
                          //N'會計處-存放利率(當月)-(放款)利率' , N'A11'
                          //N'會計處-存放利率(當月)-利差' , N'A12'
                        _sqlParams.Add("A10", model.A10);
                        _sqlParams.Add("A11", model.A11);
                        _sqlParams.Add("A12", model.A12);
                        break;
                case "13"://會計處 - 存放利率
                    break;
                case "14"://業務部 - 目標分配
                          //N'業務部-目標分配-總存款' , N'A01'
                          //N'業務部-目標分配-活期性存款' , N'A02'
                          //N'業務部-目標分配-自有資金放款' , N'A03'
                          //N'業務部-目標分配-外匯承作量' , N'A04'
                          //N'業務部-目標分配-盈餘' , N'A05'
                          //N'業務部-目標分配-外匯存款-DBU' , N'A06'
                          //N'業務部-目標分配-外匯存款-代辦OBU' , N'A07'
                          //N'業務部-目標分配-外幣授信-DBU' , N'A08'
                          //N'業務部-目標分配-外幣授信-代辦OBU' , N'A09'
                        _sqlParams.Add("A01", model.A01);
                        _sqlParams.Add("A02", model.A02);
                        _sqlParams.Add("A03", model.A03);
                        _sqlParams.Add("A04", model.A04);
                        _sqlParams.Add("A05", model.A05);
                        _sqlParams.Add("A06", model.A06);
                        _sqlParams.Add("A07", model.A07);
                        _sqlParams.Add("A08", model.A08);
                        _sqlParams.Add("A09", model.A09);
                        break;
                case "15"://業務部 - 存款調整數
                          //N'業務部-存款調整數- 活期存款' , N'A50'
                          //N'業務部-存款調整數- 定期存款' , N'A51'
                          //N'業務部-存款調整數- 公庫存款' , N'A52'
                          //N'業務部-存款調整數- 總存款' , N'A53'
                        _sqlParams.Add("A50", model.A50);
                        _sqlParams.Add("A51", model.A51);
                        _sqlParams.Add("A52", model.A52);
                        _sqlParams.Add("A53", model.A53);
                        break;
            }
                #endregion
                _sqlParams.Add("ID", model.ID);
                _sqlParams.Add("Branch_No", model.Branch_No);
                _sqlParams.Add("YM", model.YM);
                _sqlParams.Add("Category", model.Category);

                //_sqlParams.Add("A64", model.A64);
                //_sqlParams.Add("A65", model.A65);
                //_sqlParams.Add("A66", model.A66);
                //_sqlParams.Add("A67", model.A67);
                //_sqlParams.Add("A68", model.A68);
                //_sqlParams.Add("A69", model.A69);
                //_sqlParams.Add("A70", model.A70);
                //_sqlParams.Add("A71", model.A71);
                //_sqlParams.Add("A72", model.A72);
                //_sqlParams.Add("A73", model.A73);

                _sqlParamsList.Add(_sqlParams);
            }

            try {
                var ExecResult = conn.Execute(_sqlStr.ToString(), _sqlParamsList, trans);
                TransactionCommit(trans);
                return ExecResult;
            }
            catch (Exception ex) {
                TransactionRollback(trans);
                GetCloseConnection(conn);
                throw ex;
            }
            finally {
                GetCloseConnection(conn);
            }
            #endregion

        }

        internal void DeleteTempManualUpload1(SigningDeleteItemsViewModel model, ref SqlConnection conn, ref SqlTransaction trans) {
            #region 參數設定
            //組立SQL字串並連接資料庫
            StringBuilder _sqlStr = new StringBuilder();
            #endregion

            #region 流程
            _sqlStr.Append(@"Delete from Temp_ManualUpload WHERE 1=1 ");
            _sqlStr.Append(@" AND ID = @ID ");

            _sqlParams = new Dapper.DynamicParameters();
            _sqlParams.Add("ID", model.ProID);

            try {
                var ExecResult = conn.Execute(_sqlStr.ToString(), _sqlParams, trans);
            }
            catch (Exception ex) {
                TransactionRollback(trans);
                GetCloseConnection(conn);
                throw ex;
            }
            #endregion
        }
        internal void DeleteTempManualUpload2(SigningDeleteItemsViewModel model, ref SqlConnection conn, ref SqlTransaction trans) {
            #region 參數設定
            //組立SQL字串並連接資料庫
            StringBuilder _sqlStr = new StringBuilder();
            #endregion

            #region 流程
            _sqlStr.Append(@"Delete from Temp_ManualUpload WHERE 1=1 ");
            _sqlStr.Append(@" AND ID = @ID ");

            _sqlParams = new Dapper.DynamicParameters();
            _sqlParams.Add("ID", model.ProID);

            try {
                var ExecResult = conn.Execute(_sqlStr.ToString(), _sqlParams, trans);
                TransactionCommit(trans);
            }
            catch (Exception ex) {
                TransactionRollback(trans);
                GetCloseConnection(conn);
                throw ex;
            }
            finally {
                GetCloseConnection(conn);
            }
            #endregion
        }

        internal void TemptoManualUpload(SigningUpdateItemsViewModel model, ref SqlConnection conn, ref SqlTransaction trans) {
            #region 參數設定
            //組立SQL字串並連接資料庫
            StringBuilder _sqlStr = new StringBuilder();
            StringBuilder _StrReserved = new StringBuilder();
            string _Str = string.Empty;
            #endregion
            //判斷Select...Insert...流程(將空的資料補齊)
            SelectInsertManualUpload(); //Select...Insert...空資料
            //Update...
            #region Select...Update...流程
            _sqlStr.Append(@"   UPDATE Table_A
                                SET Table_A.ID = Table_B.ID, ");
            #region swich case(產生_Str)
            switch (model.PID) {
                case "1"://信託部 - 各項手收合計數
                         //N'信託部-各項手收合計數-手續費收入合計數(含證券經紀及承銷收入' , N'A59'
                    _Str = @" Table_A.A59 = Table_B.A59 ";
                    break;
                case "2"://信託部 - 聯行貼補手續費收入
                         //N'信託部-聯行貼補手續費收入-受託投資有價證券業務(本月)' , N'A32'
                         //N'信託部-聯行貼補手續費收入-受託投資有價證券業務(本年度累計)' , N'A33'
                         //N'信託部-聯行貼補手續費收入-不動產信託業務(本月)' , N'A34'
                         //N'信託部-聯行貼補手續費收入-不動產信託業務(本年度累計)' , N'A35'
                         //N'信託部-聯行貼補手續費收入-租賃權信託業務(本月)' , N'A36'
                         //N'信託部-聯行貼補手續費收入-租賃權信託業務(本年度累計)' , N'A37'
                         //N'信託部-聯行貼補手續費收入-簽證業務(本月)' , N'A38'
                         //N'信託部-聯行貼補手續費收入-簽證業務(本年度累計)' , N'A39'
                         //N'信託部-聯行貼補手續費收入-地上權信託業務(本月)' , N'A40'
                         //N'信託部-聯行貼補手續費收入-地上權信託業務(本年度累計)' , N'A41'
                         //N'信託部-聯行貼補手續費收入-公司債發行受託人(本月)' , N'A42'
                         //N'信託部-聯行貼補手續費收入-公司債發行受託人(本年度累計)' , N'A43'
                         //N'信託部-聯行貼補手續費收入-外國債及結構債券(本月)' , N'A44'
                         //N'信託部-聯行貼補手續費收入-外國債及結構債券(本年度累計)' , N'A45'
                         //N'信託部-聯行貼補手續費收入-財產信託業務(本月)' , N'A46'
                         //N'信託部-聯行貼補手續費收入-財產信託業務(本年度累計)' , N'A47'
                         //N'信託部-聯行貼補手續費收入-合　　計(本月)' , N'A48'
                         //N'信託部-聯行貼補手續費收入-合　　計(本年度累計)' , N'A49'
                    _Str = @" Table_A.A32 = Table_B.A32, 
                              Table_A.A33 = Table_B.A33,
                              Table_A.A34 = Table_B.A34,
                              Table_A.A35 = Table_B.A35,
                              Table_A.A36 = Table_B.A36,
                              Table_A.A37 = Table_B.A37,
                              Table_A.A38 = Table_B.A38,
                              Table_A.A39 = Table_B.A39,
                              Table_A.A40 = Table_B.A40,
                              Table_A.A41 = Table_B.A41,
                              Table_A.A42 = Table_B.A42,
                              Table_A.A43 = Table_B.A43,
                              Table_A.A44 = Table_B.A44,
                              Table_A.A45 = Table_B.A45,
                              Table_A.A46 = Table_B.A46,
                              Table_A.A47 = Table_B.A47,
                              Table_A.A48 = Table_B.A48,
                              Table_A.A49 = Table_B.A49  ";
                    break;
                case "3"://風管部 - 資本成本
                         //N'風管部-資本成本-資本成本' , N'A60'
                    _Str = @" Table_A.A60 = Table_B.A60 ";
                    break;
                case "4"://財管部 - 手續費收入營運概況表
                         //N'財管部-手續費收入營運概況表-受託投資有價證券業務(本月)' , N'A16'
                         //N'財管部-手續費收入營運概況表-銀行保險業務(本月)' , N'A17'
                         //N'財管部-手續費收入營運概況表-合計(本月)' , N'A18'
                         //N'財管部-手續費收入營運概況表-受託投資有價證券業務(累計)' , N'A19'
                         //N'財管部-手續費收入營運概況表-銀行保險業務(累計)' , N'A20'
                         //N'財管部-手續費收入營運概況表-合計(累計)' , N'A21'
                         //N'財管部-手續費收入營運概況表-累計達成率' , N'A22'
                         //N'財管部-手續費收入營運概況表-(Y-1年M月)受託投資有價證券業務' , N'A23'
                         //N'財管部-手續費收入營運概況表-(Y-1年M月)銀行保險業務' , N'A24'
                         //N'財管部-手續費收入營運概況表-(Y-1年M月)合計' , N'A25'
                         //N'財管部-手續費收入營運概況表-(與去年同期比較金額)受託投資有價證券業務' , N'A26'
                         //N'財管部-手續費收入營運概況表-(與去年同期比較金額)銀行保險業務' , N'A27'
                         //N'財管部-手續費收入營運概況表-(與去年同期比較金額)合計' , N'A28'
                         //N'財管部-手續費收入營運概況表-(與去年同期比較比率)受託投資有價證券業務' , N'A29'
                         //N'財管部-手續費收入營運概況表-(與去年同期比較比率)銀行保險業務' , N'A30'
                         //N'財管部-手續費收入營運概況表-(與去年同期比較比率)合計' , N'A31'
                         //N'財管部-手續費收入營運概況表-本年度目標' , N'A63'
                    _Str = @" Table_A.A16 = Table_B.A16, 
                              Table_A.A17 = Table_B.A17,
                              Table_A.A18 = Table_B.A18,
                              Table_A.A19 = Table_B.A19,
                              Table_A.A20 = Table_B.A20,
                              Table_A.A21 = Table_B.A21,
                              Table_A.A22 = Table_B.A22,
                              Table_A.A23 = Table_B.A23,
                              Table_A.A24 = Table_B.A24,
                              Table_A.A25 = Table_B.A25,
                              Table_A.A26 = Table_B.A26,
                              Table_A.A27 = Table_B.A27,
                              Table_A.A28 = Table_B.A28,
                              Table_A.A29 = Table_B.A29,
                              Table_A.A30 = Table_B.A30,
                              Table_A.A31 = Table_B.A31,
                              Table_A.A63 = Table_B.A63 ";
                    break;
                case "5"://國外部 - 代辦OBU盈餘調整數
                         //N'國外部-代辦OBU盈餘調整數-合計' , N'A61'
                    _Str = @" Table_A.A61 = Table_B.A61 ";
                    break;
                case "6"://國外部 - 營運概況表
                    break;
                case "7"://債管部 - 呆帳息
                         //N'債管部-呆帳息-F欄' , N'A62'
                    _Str = @" Table_A.A62 = Table_B.A62 ";
                    break;
                case "8"://債管部 - 追債收回統計表
                         //N'債管部-追債收回統計表-追索債權本金新增累計(B)' , N'A57'
                         //N'債管部-追債收回統計表-本月份截止累計達成額(J)' , N'A58'
                    _Str = @" Table_A.A57 = Table_B.A57,
                              Table_A.A58 = Table_B.A58  ";
                    break;
                case "9"://債管部 - 應提備抵呆帳數
                         //N'債管部-應提備抵呆帳數-本月' , N'A54'
                         //N'債管部-應提備抵呆帳數-截至X月底累計' , N'A55'
                         //N'債管部-應提備抵呆帳數-代辦OBU業務備呆' , N'A56'
                    _Str = @" Table_A.A54 = Table_B.A54,
                              Table_A.A55 = Table_B.A55,
                              Table_A.A56 = Table_B.A56  ";
                    break;
                case "10"://債管部 - 營運概況表
                    break;
                case "11"://會計處 - 存放利率(累計)
                          //N'會計處-存放利率(累計)-利差' , N'A13'
                          //N'會計處-存放利率(累計)-(存款)利率' , N'A14'
                          //N'會計處-存放利率(累計)-(放款)利率' , N'A15'
                    _Str = @" Table_A.A13 = Table_B.A13,
                              Table_A.A14 = Table_B.A14,
                              Table_A.A15 = Table_B.A15  ";
                    break;
                case "12"://會計處 - 存放利率(當月)
                          //N'會計處-存放利率(當月)-(存款)利率' , N'A10'
                          //N'會計處-存放利率(當月)-(放款)利率' , N'A11'
                          //N'會計處-存放利率(當月)-利差' , N'A12'
                    _Str = @" Table_A.A10 = Table_B.A10,
                              Table_A.A11 = Table_B.A11,
                              Table_A.A12 = Table_B.A12  ";
                    break;
                case "13"://會計處 - 存放利率
                    break;
                case "14"://業務部 - 目標分配
                          //N'業務部-目標分配-總存款' , N'A01'
                          //N'業務部-目標分配-活期性存款' , N'A02'
                          //N'業務部-目標分配-自有資金放款' , N'A03'
                          //N'業務部-目標分配-外匯承作量' , N'A04'
                          //N'業務部-目標分配-盈餘' , N'A05'
                          //N'業務部-目標分配-外匯存款-DBU' , N'A06'
                          //N'業務部-目標分配-外匯存款-代辦OBU' , N'A07'
                          //N'業務部-目標分配-外幣授信-DBU' , N'A08'
                          //N'業務部-目標分配-外幣授信-代辦OBU' , N'A09'
                    _Str = @" Table_A.A01 = Table_B.A01, 
                              Table_A.A02 = Table_B.A02,
                              Table_A.A03 = Table_B.A03,
                              Table_A.A04 = Table_B.A04,
                              Table_A.A05 = Table_B.A05,
                              Table_A.A06 = Table_B.A06,
                              Table_A.A07 = Table_B.A07,
                              Table_A.A08 = Table_B.A08,
                              Table_A.A09 = Table_B.A09  ";
                    break;
                case "15"://業務部 - 存款調整數
                          //N'業務部-存款調整數- 活期存款' , N'A50'
                          //N'業務部-存款調整數- 定期存款' , N'A51'
                          //N'業務部-存款調整數- 公庫存款' , N'A52'
                          //N'業務部-存款調整數- 總存款' , N'A53'
                    _Str = @" Table_A.A50 = Table_B.A50,
                              Table_A.A51 = Table_B.A51,
                              Table_A.A52 = Table_B.A52,
                              Table_A.A53 = Table_B.A53  ";
                    break;
            }
            #endregion
            _sqlStr.Append(_Str); //將前面判斷的資料寫入SQL字串裡面
            _sqlStr.Append(@"   FROM ManualUpload AS Table_A
                                INNER JOIN Temp_ManualUpload AS Table_B
                                ON Table_A.Branch_No = Table_B.Branch_No and Table_A.YM = Table_B.YM  
                                WHERE 1=1 ");

            _sqlStr.Append(@"   AND Table_B.ID = @ID ");

            _sqlParams = new Dapper.DynamicParameters();
            _sqlParams.Add("ID", model.ProID);
            #endregion
            try {
                var ExecResult = conn.Execute(_sqlStr.ToString(), _sqlParams, trans);
            }
            catch (Exception ex) {
                TransactionRollback(trans);
                GetCloseConnection(conn);
                throw ex;
            }

        }
        #endregion

        /// <summary>
        /// 先Insert空資料結構,避免Update失敗
        /// </summary>
        /// <param name="model"></param>
        public void SelectInsertManualUpload() {

            #region 參數告宣
                //組立SQL字串並連接資料庫
                SearchListViewModel result = new SearchListViewModel();
                //查詢SQL
                StringBuilder _sqlStr = new StringBuilder();
                //查詢條件SQL
                StringBuilder _sqlParamStr = new StringBuilder();
            #endregion

            #region 取上個月民國年
                DateTime dt = DateTime.Now.AddMonths(-1);
                CultureInfo culture = new CultureInfo("zh-TW");
                culture.DateTimeFormat.Calendar = new TaiwanCalendar();
                string _YM = dt.ToString("yyyMM", culture); //民國年月 yyyMM
            #endregion

            #region 流程
                _sqlStr.Append(@" Insert into ManualUpload(Branch_No,YM) select Branch_No,@YM from Until ");

                _sqlParams = new Dapper.DynamicParameters();
                _sqlParams.Add("YM", _YM);
                try {
                    using (var cn = new SqlConnection(_dbConnPPP))//連接資料庫
                    {
                        cn.Open();
                        var ExecResult = cn.Execute(_sqlStr.ToString(), _sqlParams);
                        //...這裡要插操作紀錄,某User新增某xxxx申請單
                        //return ExecResult; //回報新增筆數
                    }
                }
                catch (Exception ex) {
                    //throw ex;
                }
            #endregion
        }
    }
}