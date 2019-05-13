using WaterCaseTracking.Dao;
using WaterCaseTracking.Models.ViewModels.JobReminder;
using WaterCaseTracking.Models.ViewModels.SigningProcess;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Web;

namespace WaterCaseTracking.Service {
    public class SigningProcessService {
        #region 申請單-申請 ApplicationInsert()
        public int ApplicationInsert(SigningInsertItemsViewModel searchInfo) {
            #region 參數宣告				
            //SigningProcessItemViewModel searchList = new SigningProcessItemViewModel();
            SigningRecordDao SigningRecordDao = new SigningRecordDao();
            #endregion

            #region 流程																
            int ReturnNum = SigningRecordDao.ApplicationInsert(searchInfo); //將參數送入Dao層,組立SQL字串並連接資料庫

            return ReturnNum;
            #endregion
        }
        #endregion
        #region 覆核通過 & 簽核通過 ApplicationUpdate()
        public string ApplicationUpdate(SigningUpdateItemsViewModel model,string Groups) {
            #region 參數宣告				
            //SigningProcessItemViewModel searchList = new SigningProcessItemViewModel();
            SigningRecordDao SigningRecordDao = new SigningRecordDao();
            ManualUploadDao ManualUploadDao = new ManualUploadDao();
            SigningDeleteItemsViewModel D_model = new SigningDeleteItemsViewModel();
            JobReminderDao jobReminderDao = new JobReminderDao();
            ModifyViewModel modifyViewModel = new ModifyViewModel();
            KPI_ScoreDao kpi_ScoreDao = new KPI_ScoreDao();
            string returnStr = string.Empty;
            SqlConnection sqlConn;
            SqlTransaction sqlTrans;
            #endregion
            D_model.ProID = model.ProID;
            #region 流程	
            try {
                //步驟1
                int ReturnNum = SigningRecordDao.ApplicationUpdate(model, out sqlConn, out sqlTrans); //申請單狀態修改-->[通過](SigningRecord)
                if (ReturnNum <= 0)
                    return "系統錯誤!!";

                //步驟2:
                switch (model.PID) {
                    //  1:(人工上傳)信託部 - 各項手收合計數
                    //  2:(人工上傳)信託部 - 聯行貼補手續費收入
                    //  3:(人工上傳)風管部 - 資本成本
                    //  4:(人工上傳)財管部 - 手續費收入營運概況表
                    //  5:(人工上傳)國外部 - 代辦OBU盈餘調整數
                    //  6:(人工上傳)國外部 - 營運概況表
                    //  7:(人工上傳)債管部 - 呆帳息
                    //  8:(人工上傳)債管部 - 追債收回統計表
                    //  9:(人工上傳)債管部 - 應提備抵呆帳數
                    // 10:(人工上傳)債管部 - 營運概況表
                    // 11:(人工上傳)會計處 - 存放利率(累計)
                    // 12:(人工上傳)會計處 - 存放利率(當月)
                    // 13:(人工上傳)會計處 - 存放利率
                    // 14:(人工上傳)業務部 - 目標分配
                    // 15:(人工上傳)業務部 - 存款調整數
                    case "1":
                    case "2":
                    case "3":
                    case "4":
                    case "5":
                    case "6":
                    case "7":
                    case "8":
                    case "9":
                    case "10":
                    case "11":
                    case "12":
                    case "13":
                    case "14":
                    case "15":
                        ManualUploadDao.TemptoManualUpload(model, ref sqlConn, ref sqlTrans);//將Temp資料寫入正式Table
                        ManualUploadDao.DeleteTempManualUpload1(D_model, ref sqlConn, ref sqlTrans); //清除Temp資料
                        break;
                    // 16:(管理考核)得分上傳
                    // 17:(業績得分)上傳
                    case "16":
                    case "17":
                        //刪除資料
                        kpi_ScoreDao.ApplyDeleteKPI_Score(D_model.ProID, ref sqlConn, ref sqlTrans);
                        //新增資料
                        kpi_ScoreDao.ApplyAddKPI_Score(D_model.ProID, ref sqlConn, ref sqlTrans);
                        break;
                    // 18:批次管理執行申請
                    case "18":
                        BatchManageService BatchManageService = new BatchManageService();
                        BatchManageService.applyToExecuteBatch(model.YM, ref sqlConn, ref sqlTrans);                    //修改批次狀態
                        break;
                }
                #region 取上個月民國年
                DateTime dt = DateTime.Now.AddMonths(0);
                CultureInfo culture = new CultureInfo("zh-TW");
                culture.DateTimeFormat.Calendar = new TaiwanCalendar();
                string _YM = dt.ToString("yyyMM", culture); //民國年月 YYYMM
                #endregion
                //步驟3
                modifyViewModel.GroupsCode = Groups;
                modifyViewModel.ActionCode = model.PID;
                modifyViewModel.YM = _YM;
                jobReminderDao.isRunModify(modifyViewModel, ref sqlConn, ref sqlTrans); //修改狀態為已執行
                if (model.Status == 2) {
                    returnStr = "覆核成功!!";
                }
                else {
                    returnStr = "簽核成功!!";
                }
            }
            catch (Exception ex) {
                return ex.ToString();
            }


            return returnStr;
            #endregion
        }
        #endregion

        #region 駁回 ApplicationDelete()
        public void ApplicationDelete(SigningDeleteItemsViewModel model) {
            #region 參數宣告				
            //SigningProcessItemViewModel searchList = new SigningProcessItemViewModel();
            SigningRecordDao SigningRecordDao = new SigningRecordDao();
            ManualUploadDao ManualUploadDao = new ManualUploadDao();
            SqlConnection sqlConn;
            SqlTransaction sqlTrans;
            #endregion

            #region 流程																
            SigningRecordDao.ApplicationDelete(model, out sqlConn, out sqlTrans); //將SigningRecord單狀態改成[駁回]
            ManualUploadDao.DeleteTempManualUpload2(model, ref sqlConn, ref sqlTrans); //駁回單已結單,故清除上傳暫存資料
            #endregion
        }
        #endregion

        #region 查詢 QuerySearchList()
        public SigningQueryItemViewModel QuerySearchList(SigningInfoItemsViewModel searchInfo) {
            #region 參數宣告				
            SigningQueryItemViewModel searchList = new SigningQueryItemViewModel();
            SigningRecordDao SigningRecordDao = new SigningRecordDao();
            #endregion

            #region 流程																
            searchList = SigningRecordDao.QuerySearchList(searchInfo); //將參數送入Dao層,組立SQL字串並連接資料庫

            return searchList;
            #endregion
        }
        #endregion

        #region 簽核流程處理單-查詢 getQueryItem()
        public SigningQueryItemViewModel getQueryItem(SigningInfoItemsViewModel model) {
            #region 參數宣告				
            SigningQueryItemViewModel searchList = new SigningQueryItemViewModel();
            SigningRecordDao SigningRecordDao = new SigningRecordDao();
            #endregion

            #region 流程																
            searchList = SigningRecordDao.getQueryItem(model); //將參數送入Dao層,組立SQL字串並連接資料庫

            return searchList;
            #endregion
        }
        #endregion

        #region 簽核流程處理單-查詢 QueryItemsbyProID()
        public SigningQueryItemViewModel QueryItemsbyProID(string ProID) {
            #region 參數宣告				
            SigningQueryItemViewModel searchList = new SigningQueryItemViewModel();
            SigningRecordDao SigningRecordDao = new SigningRecordDao();
            #endregion

            #region 流程																
            searchList = SigningRecordDao.QueryItemsbyProID(ProID); //將參數送入Dao層,組立SQL字串並連接資料庫

            return searchList;
            #endregion
        }
        #endregion
        #region 上呈 QueryItemsbyProID()
        public int GiveIn(SigningUpdateItemsViewModel model) {
            #region 參數宣告				
            SigningQueryItemViewModel searchList = new SigningQueryItemViewModel();
            SigningRecordDao SigningRecordDao = new SigningRecordDao();
            #endregion

            #region 流程																
            int ReturnNum = SigningRecordDao.GiveIn(model); //將參數送入Dao層,組立SQL字串並連接資料庫

            return ReturnNum;
            #endregion
        }
        #endregion
    }
}