using WaterCaseTracking.Dao;
using WaterCaseTracking.Models;
using Microsoft.VisualBasic;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Web;
using WaterCaseTracking.Models.ViewModels.SigningProcess;
using System.IO;
using WaterCaseTracking.Controllers;

namespace WaterCaseTracking.Service {
    public class ManualUploadService {
        /// <summary>
        /// 組立Model 寫入資料庫 Temp_ManualUpload
        /// </summary>
        /// <returns></returns>
        internal int doUpLoad(HttpPostedFileBase file,string FileValues, string FileText,string _YM, string _ID) {
            #region 參數宣告				
            ManualUploadModel manualUploadModel = new ManualUploadModel();
            ManualUploadDao manualUploadDao = new ManualUploadDao();
            int successQty = 0;
            #endregion

            #region 流程	
            //存Xls轉的DataTable
            DataTable orgDt = new DataTable();
            //判斷檔案室否有trans_date資料
            if (file.ContentLength > 0 || file != null) {
                //orgDt = SelectLoadXlsx(file,FileValues);
                //把資料轉成DataTable
                try {
                    #region 從第幾行開始?
                    int _FromRow = 0; //從第幾行開始?
                    switch (FileValues) {
                        case "1"://信託部 - 各項手收合計數
                            _FromRow = 2;
                            break;
                        case "2"://信託部 - 聯行貼補手續費收入
                            _FromRow = 3;
                            break;
                        case "3"://風管部 - 資本成本
                            _FromRow = 1;
                            break;
                        case "4"://財管部 - 手續費收入營運概況表
                            _FromRow = 3;
                            break;
                        case "5"://國外部 - 代辦OBU盈餘調整數
                            _FromRow = 4;
                            break;
                        case "6"://國外部 - 營運概況表
                            break;
                        case "7"://債管部 - 呆帳息
                            _FromRow = 2;
                            break;
                        case "8"://債管部 - 追債收回統計表
                            _FromRow = 4;
                            break;
                        case "9"://債管部 - 應提備抵呆帳數
                            _FromRow = 3;
                            break;
                        case "10"://債管部 - 營運概況表
                            break;
                        case "11"://會計處 - 存放利率(累計)
                            _FromRow = 2;
                            break;
                        case "12"://會計處 - 存放利率(當月)
                            _FromRow = 2;
                            break;
                        case "13"://會計處 - 存放利率
                            break;
                        case "14"://業務部 - 目標分配
                            _FromRow = 4;
                            break;
                        case "15"://業務部 - 存款調整數
                            _FromRow = 5;
                            break;
                    }
                    #endregion
                    orgDt = LoadXlsxForNum(file, _FromRow);
                }
                catch (Exception ex) {
                    throw new Exception("匯入檔案錯誤");
                }

                #region 預設
                List<ManualUploadModel> listModel = new List<ManualUploadModel>();

                string _Category = FileValues;
                double numberA01;
                double numberA02;
                double numberA03;
                double numberA04;
                double numberA05;
                double numberA06;
                double numberA07;
                double numberA08;
                double numberA09;
                double numberA10;
                double numberA11;
                double numberA12;
                double numberA13;
                double numberA14;
                double numberA15;
                double numberA22;
                double numberA29;
                double numberA30;
                double numberA31;
                double numberA57;
                double numberA58;
                double numberA59;
                double numberA61_1;
                double numberA61_2;
                #endregion

                for (int i = 0; i < orgDt.Rows.Count; i++) {
                    #region 資料-單Row
                    try {
                        //N'分行代碼' , N'Branch_No'
                        //N'年月' , N'YM'
                        bool _RunOK = false;
                        manualUploadModel = new ManualUploadModel();
                        manualUploadModel.ID = _ID; //申請單號
                        manualUploadModel.YM = _YM; //民國年月(10803)
                        manualUploadModel.Category = _Category;
                        manualUploadModel.Branch_No = orgDt.Rows[i][0].ToString(); //分行代碼


                        switch (FileValues) {
                            case "1"://信託部 - 各項手收合計數
                                     //N'信託部-各項手收合計數-手續費收入合計數(含證券經紀及承銷收入' , N'A59'
                                    manualUploadModel.Branch_No = orgDt.Rows[i][1].ToString();
                                    numberA59 = 0;
                                    if (Double.TryParse(orgDt.Rows[i][11].ToString(), out numberA59))
                                        manualUploadModel.A59 = Math.Round(numberA59, 0).ToString();
                                    _RunOK = true;
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
                                    manualUploadModel.A32 = orgDt.Rows[i][2].ToString();
                                    manualUploadModel.A33 = orgDt.Rows[i][3].ToString();
                                    manualUploadModel.A34 = orgDt.Rows[i][4].ToString();
                                    manualUploadModel.A35 = orgDt.Rows[i][5].ToString();
                                    manualUploadModel.A36 = orgDt.Rows[i][6].ToString();
                                    manualUploadModel.A37 = orgDt.Rows[i][7].ToString();
                                    manualUploadModel.A38 = orgDt.Rows[i][8].ToString();
                                    manualUploadModel.A39 = orgDt.Rows[i][9].ToString();
                                    manualUploadModel.A40 = orgDt.Rows[i][10].ToString();
                                    manualUploadModel.A41 = orgDt.Rows[i][11].ToString();
                                    manualUploadModel.A42 = orgDt.Rows[i][12].ToString();
                                    manualUploadModel.A43 = orgDt.Rows[i][13].ToString();
                                    manualUploadModel.A44 = orgDt.Rows[i][14].ToString();
                                    manualUploadModel.A45 = orgDt.Rows[i][15].ToString();
                                    manualUploadModel.A46 = orgDt.Rows[i][16].ToString();
                                    manualUploadModel.A47 = orgDt.Rows[i][17].ToString();
                                    manualUploadModel.A48 = orgDt.Rows[i][18].ToString();
                                    manualUploadModel.A49 = orgDt.Rows[i][19].ToString();
                                    _RunOK = true;
                                break;
                            case "3"://風管部 - 資本成本
                                     //N'風管部-資本成本-資本成本' , N'A60'
                                    manualUploadModel.Branch_No = orgDt.Rows[i][1].ToString();
                                    manualUploadModel.A60 = orgDt.Rows[i][4].ToString();
                                    _RunOK = true;
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
                                    manualUploadModel.A16 = orgDt.Rows[i][3].ToString();
                                    manualUploadModel.A17 = orgDt.Rows[i][4].ToString();
                                    manualUploadModel.A18 = orgDt.Rows[i][5].ToString();
                                    manualUploadModel.A19 = orgDt.Rows[i][6].ToString();
                                    manualUploadModel.A20 = orgDt.Rows[i][7].ToString();
                                    manualUploadModel.A21 = orgDt.Rows[i][8].ToString();
                                    numberA22 = 0;
                                    if (Double.TryParse(orgDt.Rows[i][9].ToString(), out numberA22))
                                        manualUploadModel.A22 = (numberA22 * 100).ToString();
                                    //manualUploadModel.A22 = orgDt.Rows[i][9].ToString();
                                    manualUploadModel.A23 = orgDt.Rows[i][10].ToString();
                                    manualUploadModel.A24 = orgDt.Rows[i][11].ToString();
                                    manualUploadModel.A25 = orgDt.Rows[i][12].ToString();
                                    manualUploadModel.A26 = orgDt.Rows[i][13].ToString();
                                    manualUploadModel.A27 = orgDt.Rows[i][14].ToString();
                                    manualUploadModel.A28 = orgDt.Rows[i][15].ToString();
                                    numberA29 = 0;
                                    numberA30 = 0;
                                    numberA31 = 0;
                                    if (Double.TryParse(orgDt.Rows[i][16].ToString(), out numberA29))
                                        manualUploadModel.A29 = (numberA29 * 100).ToString();
                                    if (Double.TryParse(orgDt.Rows[i][17].ToString(), out numberA30))
                                        manualUploadModel.A30 = (numberA30 * 100).ToString();
                                    if (Double.TryParse(orgDt.Rows[i][18].ToString(), out numberA31))
                                        manualUploadModel.A31 = (numberA31 * 100).ToString();
                                    //manualUploadModel.A29 = orgDt.Rows[i][16].ToString();
                                    //manualUploadModel.A30 = orgDt.Rows[i][17].ToString();
                                    //manualUploadModel.A31 = orgDt.Rows[i][18].ToString();
                                    manualUploadModel.A63 = orgDt.Rows[i][2].ToString(); //本年度目標 ,漏掉後來補上的
                                    _RunOK = true;
                                    break;
                            case "5"://國外部 - 代辦OBU盈餘調整數
                                     //N'國外部-代辦OBU盈餘調整數-合計' , N'A61'
                                     //manualUploadModel.A61 = orgDt.Rows[i][6].ToString();
                                    numberA61_1 = 0;
                                    numberA61_2 = 0;
                                    if (Double.TryParse(orgDt.Rows[i][4].ToString(), out numberA61_1) && Double.TryParse(orgDt.Rows[i][5].ToString(), out numberA61_2))
                                        manualUploadModel.A61 = (Math.Round(numberA61_1, 0) + numberA61_2).ToString();
                                       _RunOK = true;
                                    break;
                            case "6"://國外部 - 營運概況表
                                break;
                            case "7"://債管部 - 呆帳息
                                     //N'債管部-呆帳息-F欄' , N'A62'
                                    manualUploadModel.Branch_No = orgDt.Rows[i][1].ToString();
                                    manualUploadModel.A62 = orgDt.Rows[i][5].ToString();
                                    _RunOK = true;
                                    break;
                            case "8"://債管部 - 追債收回統計表
                                     //N'債管部-追債收回統計表-追索債權本金新增累計(B)' , N'A57'
                                     //N'債管部-追債收回統計表-本月份截止累計達成額(J)' , N'A58'
                                numberA57 = 0;
                                if (Double.TryParse(orgDt.Rows[i][7].ToString(), out numberA57))
                                    manualUploadModel.A57 = Math.Round(numberA57,0).ToString();
                                    numberA58 = 0;
                                    if (Double.TryParse(orgDt.Rows[i][23].ToString(), out numberA58))
                                        manualUploadModel.A58 = Math.Round(numberA58,0).ToString();
                                        _RunOK = true;
                                    break;
                            case "9"://債管部 - 應提備抵呆帳數
                                     //N'債管部-應提備抵呆帳數-本月' , N'A54'
                                     //N'債管部-應提備抵呆帳數-截至X月底累計' , N'A55'
                                     //N'債管部-應提備抵呆帳數-代辦OBU業務備呆' , N'A56'
                                    manualUploadModel.A54 = orgDt.Rows[i][3].ToString();
                                    manualUploadModel.A55 = orgDt.Rows[i][4].ToString();
                                    manualUploadModel.A56 = orgDt.Rows[i][5].ToString();
                                    _RunOK = true;
                                break;
                            case "10"://債管部 - 營運概況表
                                break;
                            case "11"://會計處 - 存放利率(累計)
                                      //N'會計處-存放利率(累計)-利差' , N'A13'
                                      //N'會計處-存放利率(累計)-(存款)利率' , N'A14'
                                      //N'會計處-存放利率(累計)-(放款)利率' , N'A15'
                                    manualUploadModel.Branch_No = orgDt.Rows[i][2].ToString();
                                    //manualUploadModel.A13 = orgDt.Rows[i][10].ToString();
                                    //manualUploadModel.A14 = orgDt.Rows[i][6].ToString();
                                    //manualUploadModel.A15 = orgDt.Rows[i][9].ToString();
                                    numberA13 = 0;
                                    numberA14 = 0;
                                    numberA15 = 0;
                                    if (Double.TryParse(orgDt.Rows[i][10].ToString(), out numberA13))
                                        manualUploadModel.A13 = (numberA13 * 100).ToString();
                                    if (Double.TryParse(orgDt.Rows[i][6].ToString(), out numberA14))
                                        manualUploadModel.A14 = (numberA14 * 100).ToString();
                                    if (Double.TryParse(orgDt.Rows[i][9].ToString(), out numberA15))
                                        manualUploadModel.A15 = (numberA15 * 100).ToString();
                                    _RunOK = true;
                                break;
                            case "12"://會計處 - 存放利率(當月)
                                      //N'會計處-存放利率(當月)-(存款)利率' , N'A10'
                                      //N'會計處-存放利率(當月)-(放款)利率' , N'A11'
                                      //N'會計處-存放利率(當月)-利差' , N'A12'
                                    manualUploadModel.Branch_No = orgDt.Rows[i][2].ToString();
                                    numberA10 = 0;
                                    numberA11 = 0;
                                    numberA12 = 0;
                                    if (Double.TryParse(orgDt.Rows[i][6].ToString(), out numberA10))
                                        manualUploadModel.A10 = (numberA10 * 100).ToString();
                                    if (Double.TryParse(orgDt.Rows[i][9].ToString(), out numberA11))
                                        manualUploadModel.A11 = (numberA11 * 100).ToString();
                                    if (Double.TryParse(orgDt.Rows[i][10].ToString(), out numberA12))
                                        manualUploadModel.A12 = (numberA12 * 100).ToString();
                                    _RunOK = true;
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
                                numberA01 = 0;
                                numberA02 = 0;
                                numberA03 = 0;
                                numberA04 = 0;
                                numberA05 = 0;
                                numberA06 = 0;
                                numberA07 = 0;
                                numberA08 = 0;
                                numberA09 = 0;

                                if (Double.TryParse(orgDt.Rows[i][3].ToString(), out numberA01))
                                    manualUploadModel.A01 = Math.Round(numberA01, 0).ToString();
                                if (Double.TryParse(orgDt.Rows[i][4].ToString(), out numberA02))
                                    manualUploadModel.A02 = Math.Round(numberA02, 0).ToString();
                                if (Double.TryParse(orgDt.Rows[i][5].ToString(), out numberA03))
                                    manualUploadModel.A03 = Math.Round(numberA03, 0).ToString();

                                if (Double.TryParse(orgDt.Rows[i][14].ToString(), out numberA04))
                                    manualUploadModel.A04 = Math.Round(numberA04, 0).ToString();
                                if (Double.TryParse(orgDt.Rows[i][16].ToString(), out numberA05))
                                    manualUploadModel.A05 = Math.Round(numberA05, 0).ToString();

                                if (Double.TryParse(orgDt.Rows[i][10].ToString(), out numberA06))
                                    manualUploadModel.A06 = Math.Round(numberA06, 0).ToString();
                                if (Double.TryParse(orgDt.Rows[i][11].ToString(), out numberA07))
                                    manualUploadModel.A07 = Math.Round(numberA07, 0).ToString();
                                if (Double.TryParse(orgDt.Rows[i][12].ToString(), out numberA08))
                                    manualUploadModel.A08 = Math.Round(numberA08, 0).ToString();
                                if (Double.TryParse(orgDt.Rows[i][13].ToString(), out numberA09))
                                    manualUploadModel.A09 = Math.Round(numberA09, 0).ToString();

                                    _RunOK = true;
                                break;
                            case "15"://業務部 - 存款調整數
                                      //N'業務部-存款調整數- 活期存款' , N'A50'
                                      //N'業務部-存款調整數- 定期存款' , N'A51'
                                      //N'業務部-存款調整數- 公庫存款' , N'A52'
                                      //N'業務部-存款調整數- 總存款' , N'A53'
                                    manualUploadModel.A50 = orgDt.Rows[i][2].ToString();
                                    manualUploadModel.A51 = orgDt.Rows[i][3].ToString();
                                    manualUploadModel.A52 = orgDt.Rows[i][4].ToString();
                                    manualUploadModel.A53 = orgDt.Rows[i][5].ToString();
                                    _RunOK = true;
                                break;
                        }

                        //manualUploadModel.A64 = orgDt.Rows[i][65].ToString();
                        //manualUploadModel.A65 = orgDt.Rows[i][66].ToString();
                        //manualUploadModel.A66 = orgDt.Rows[i][67].ToString();
                        //manualUploadModel.A67 = orgDt.Rows[i][68].ToString();
                        //manualUploadModel.A68 = orgDt.Rows[i][69].ToString();
                        //manualUploadModel.A69 = orgDt.Rows[i][70].ToString();
                        //manualUploadModel.A70=  orgDt.Rows[i][71].ToString();
                        //manualUploadModel.A71 = orgDt.Rows[i][72].ToString(); //債管部-營運概況表 "逾期放款 金額 (甲+乙)類（列報 + 列管）" 本月
                        //manualUploadModel.A72 = orgDt.Rows[i][73].ToString(); //債管部-營運概況表 "逾期放款 比率 (甲+乙)類（列報 + 列管）" 本月

                        //manualUploadModel.A73 = orgDt.Rows[i][74].ToString();
                        if (_RunOK) {
                            manualUploadModel.Branch_No = manualUploadModel.Branch_No.Replace(" ", ""); //去除空格
                            manualUploadModel.Branch_No = Strings.StrConv(manualUploadModel.Branch_No, VbStrConv.Narrow, 1028); //去除空格
                            if(manualUploadModel.Branch_No.Length == 3)
                                listModel.Add(manualUploadModel);
                        }
                    }                                         
                    catch (Exception) {
                        throw new Exception("第" + i + "筆資料有誤，請確認(" + manualUploadModel.Branch_No + ")");
                    }
                    #endregion
                }
                SqlConnection sqlConn;
                SqlTransaction sqlTrans;

                //select YM & Bank , no -- insert , yes -- update
                //將申請單暫存到Temp裡面
                successQty = manualUploadDao.AddTempManualUpload(listModel, FileValues,out sqlConn, out sqlTrans);
            }
            return successQty;
            #endregion
        }

        /// <summary>
        /// 讀取Xlsx轉成DataTable
        /// </summary>
        /// <returns></returns>
        public static DataTable LoadXlsxForNum(HttpPostedFileBase file, int _FromRow) {
            //儲存Xls的DataTable
            DataTable dt = new DataTable();

            XSSFWorkbook workbook;
            //讀取專案內中的sample.xls 的excel 檔案
            using (file.InputStream) {
                workbook = new XSSFWorkbook(file.InputStream);
            }

            //讀取Sheet1 工作表
            var sheet = workbook.GetSheetAt(0);


            for (int row = 0; row <= sheet.LastRowNum; row++) {
                if (sheet.GetRow(row) != null) //null is when the row only contains empty cells 
                {
                    if (row == _FromRow)//標題列
                    {

                        foreach (var c in sheet.GetRow(row).Cells) {
                            dt.Columns.Add(c.ColumnIndex.ToString()); //改以數字寫入
                            //dt.Columns.Add(HttpUtility.UrlEncode(c.StringCellValue));
                        }
                    }
                    else if(row > _FromRow) {
                        DataRow r = dt.NewRow();
                        foreach (var c in sheet.GetRow(row).Cells) {
                            //如果是數字型 就要取 NumericCellValue  這屬性的值
                            if (c.ColumnIndex < dt.Columns.Count) { //限制取到索引內的值就好,避免索引溢位錯誤
                                if (c.CellType == CellType.Numeric)
                                    if (HSSFDateUtil.IsCellDateFormatted(c))//日期類型
                                    {
                                        r[c.ColumnIndex] = c.DateCellValue.ToString("yyyy/MM/dd");
                                    }
                                    else {
                                        r[c.ColumnIndex] = (Decimal)c.NumericCellValue;
                                    }
                                //如果是字串型 就要取 StringCellValue  這屬性的值
                                else if (c.CellType == CellType.String)
                                    r[c.ColumnIndex] = c.StringCellValue;
                            }
                        }
                        dt.Rows.Add(r);
                    }
                }
            }
            //回傳DataTable
            return dt;
        }

        /// <summary>
        /// 建立申請單 - InsertSigningRecord()
        /// </summary>
        /// <param name="_base">Contoller底層,引用Log  </param>
        /// <param name="file">檔案  </param>
        /// <param name="FileValues">申請項目ID  </param>
        /// <param name="FileText">申請項目Name  </param>
        /// <param name="_ID">申請單編號(yyyyMMddHHmmss + 員編id)  </param>
        /// <param name="funcName">使用function,log參數用  </param>
        /// <param name="UserID">使用者ID  </param>
        /// <param name="UserName">使用者Name  </param>
        /// <returns></returns>
        internal int InsertSigningRecord(HttpPostedFileBase file, string FileValues, string FileText, string _ID, string funcName,string UserID,string UserName) {
            #region 參數宣告				
            SigningInsertItemsViewModel model = new SigningInsertItemsViewModel();
            SigningRecordDao SigningRecordDao = new SigningRecordDao();
            int successQty = 0;
            string fileName = file.FileName; //申請上傳檔案名稱
            if(fileName.LastIndexOf("\\") > 0)
                fileName = fileName.Substring(fileName.LastIndexOf("\\") + 1);
            

            string FilePath = _ID + System.IO.Path.GetExtension(file.FileName); ; //儲存路徑名稱
                #region 取上個月民國年
                DateTime dt = DateTime.Now.AddMonths(-1);
                CultureInfo culture = new CultureInfo("zh-TW");
                culture.DateTimeFormat.Calendar = new TaiwanCalendar();
                string _YM = dt.ToString("yyyMM", culture); //民國年月 YYYMM
                #endregion
            #endregion
            try {
                model.ProID       = _ID;                         //--申請單編號(yyyyMMddHHmmss + 員編id)       
                model.UserID      = UserID;                      //--申請人ID
                model.UserName    = UserName;                    //--申請人Name         
                model.PID         = FileValues;                  //--申請項目ID            
                model.PName       = FileText;                    //--申請項目Name              
                model.YM          = _YM;                         //--申請匯入年月YYYMM
                model.UploadName  = fileName;                    //--申請上傳檔案名稱
                model.UploadPath  = FilePath;                    //--申請上傳檔案路徑
                if(UpLoadFile(file, FilePath, funcName))         //--上傳檔案
                    SigningRecordDao.InsertSigningRecord(model); //--建立申請單
            }
            catch (Exception ex) {
                string _Message = ex.Message;
                throw new Exception("申請單建立失敗!!");
            }
            return successQty;
        }

        /// <summary>
        /// 建立申請單(批次專用) - InsertSigningRecordByBatchManage()
        /// </summary>
        /// <param name="FileValues">申請項目ID  </param>
        /// <param name="FileText">申請項目Name  </param>
        /// <param name="_ID">申請單編號(yyyyMMddHHmmss + 員編id)  </param>
        /// <param name="UserID">使用者ID  </param>
        /// <param name="UserName">使用者Name  </param>
        /// <returns></returns>
        internal int InsertSigningRecordByBatchManage(string FileValues, string FileText, string _ID, string UserID, string UserName) {
            #region 參數宣告				
            SigningInsertItemsViewModel model = new SigningInsertItemsViewModel();
            SigningRecordDao SigningRecordDao = new SigningRecordDao();
            int successQty = 0;
            //string fileName = file.FileName; //申請上傳檔案名稱
            //if (fileName.LastIndexOf("\\") > 0)
            //    fileName = fileName.Substring(fileName.LastIndexOf("\\") + 1);


            //string FilePath = _ID + System.IO.Path.GetExtension(file.FileName); ; //儲存路徑名稱
            #region 取上個月民國年
            DateTime dt = DateTime.Now.AddMonths(-1);
            CultureInfo culture = new CultureInfo("zh-TW");
            culture.DateTimeFormat.Calendar = new TaiwanCalendar();
            string _YM = dt.ToString("yyyMM", culture); //民國年月 YYYMM
            #endregion
            #endregion
            try {
                model.ProID = _ID;                         //--申請單編號(yyyyMMddHHmmss + 員編id)       
                model.UserID = UserID;                //--申請人ID
                model.UserName = UserName;              //--申請人Name         
                model.PID = FileValues;                  //--申請項目ID            
                model.PName = FileText;                    //--申請項目Name              
                model.YM = _YM;                         //--申請匯入年月YYYMM
                model.UploadName = "";                    //--申請上傳檔案名稱(批次不需要)
                model.UploadPath = "";                    //--申請上傳檔案路徑(批次不需要)
                //if (UpLoadFile(file, FilePath, funcName))  //--上傳檔案
                SigningRecordDao.InsertSigningRecord(model); //--建立申請單
            }
            catch (Exception ex) {
                string _Message = ex.Message;
                throw new Exception("申請單建立失敗!!");
            }
            return successQty;
        }

        #region 檔案上傳功能
        /// <summary>
        /// 檔案上傳功能
        /// </summary>
        /// <param name="httpPostedFile">指定檔案(集合)</param>
        /// <param name="FilePath">指定路徑儲存檔名</param>
        /// <returns></returns>
        public bool UpLoadFile(HttpPostedFileBase httpPostedFile, string FilePath,string funcName) {
            #region 參數宣告
            string strMsg = "";
            bool workSuccess = false;
            #endregion
            //## 讀取指定的上傳檔案ID
            string strBasicPath = HttpContext.Current.Server.MapPath("~/DataFile/SigningProcessFile/");//限定SERVER檔管路徑

            //_base.logging(funcName, "上傳附件 : " + Path.GetFileName(FilePath).ToString());
            if (httpPostedFile != null && httpPostedFile.ContentLength > 0) {
                var fileName = Path.GetFileName(FilePath);
                var path = Path.Combine(strBasicPath, fileName);
                FileInfo file_check = new FileInfo(strBasicPath);
                DirectoryInfo dirPath = new DirectoryInfo(strBasicPath);
                ////指定目錄是否存在
                //if (!dirPath.Exists) {
                //    _base.errLogging(funcName, "上傳附件 : 無指定到資料夾!");
                //}
                //else {
                //    _base.logging(funcName, "上傳附件 : 指定到資料夾(" + strBasicPath.ToString() + ")");
                //}

                file_check = new FileInfo(path);
                //檔案是否存在
                if (file_check.Exists) {
                    //_base.errLogging(funcName, "上傳附件 : 檔案已存在(路徑:" + strBasicPath.ToString() + fileName.ToString() + ")");
                    strMsg += fileName + "檔案已存在，請先刪除後，再新增!";
                }
                else {
                    try {
                        //_base.logging(funcName, "上傳附件 : 上傳" + strBasicPath.ToString() + fileName.ToString() + "完成");
                        httpPostedFile.SaveAs(path);
                        strMsg += "上傳" + fileName + "完成!";
                        workSuccess = true;
                    }
                    catch (Exception ex) {
                        strMsg += fileName + ex.Message;
                        //_base.errLogging(funcName, strMsg);
                    }
                }
            }
            else {
                //_base.errLogging(funcName, "上傳附件失敗 : 檔案不存在或是空檔案");
                strMsg += httpPostedFile.FileName + "上傳附件失敗：" + "檔案不存在或是空檔案!";
            }
            return workSuccess;
        }
        #endregion
    }
}