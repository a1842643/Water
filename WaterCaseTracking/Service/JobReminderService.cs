using WaterCaseTracking.Dao;
using WaterCaseTracking.Models;
using WaterCaseTracking.Models.ViewModels.JobReminder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WaterCaseTracking.Service {
    public class JobReminderService {
        #region 搜尋年月(下拉清單) GroupByYM()
        public JobReminderViewModel GroupByYM(YMQueryViewModel model) {
            #region 參數宣告				
            JobReminderViewModel searchList = new JobReminderViewModel();
            JobReminderDao jobReminderDao = new JobReminderDao();
            #endregion

            #region 流程
            //需帶入model.GroupsCode參數 & YM年月參數           
            searchList = jobReminderDao.GroupByYM(model); //將參數送入Dao層,組立SQL字串並連接資料庫

            return searchList;
            #endregion
        }
        #endregion

        #region 工作提醒查詢 QuerySearchList()
        public QueryTableViewModel QuerySearchList(YMQueryViewModel model,string YM) {
            #region 參數宣告				
            QueryTableViewModel searchList = new QueryTableViewModel();
            JobReminderDao jobReminderDao = new JobReminderDao();
            #endregion

            #region 流程
            //需帶入model.GroupsCode參數 & YM年月參數           
            searchList = jobReminderDao.QuerySearchList(model,YM); //將參數送入Dao層,組立SQL字串並連接資料庫

            return searchList;
            #endregion
        }
        #endregion

        #region 工作提醒設定-新增 Create()
        public string Create(SearchItemViewModel model) {
            #region 參數宣告				
            JobReminderDao jobReminderDao = new JobReminderDao();
            string ErrMsg = string.Empty;
            #endregion

            #region 流程																
            ErrMsg = jobReminderDao.Create(model); //將參數送入Dao層,組立SQL字串並連接資料庫
            #endregion
            return ErrMsg;
        }
        #endregion

        #region 查詢某群組設定清單 QueryJobReminder()
        public JobReminderViewModel QueryJobReminder(string GroupsCode) {
            #region 參數宣告				
            JobReminderViewModel searchList = new JobReminderViewModel();
            JobReminderDao jobReminderDao = new JobReminderDao();
            #endregion

            #region 流程

            searchList = jobReminderDao.QueryJobReminder(GroupsCode); //將參數送入Dao層,組立SQL字串並連接資料庫

            return searchList;
            #endregion
        }
        #endregion

        #region 將設定清單建立在當月工作項目 JudgeInsert()
        /// <summary>
        /// 將設定清單建立在當月工作項目
        /// </summary>
        /// <param name="items"></param>
        /// <param name="YM"></param>
        /// <param name="GroupsCode"></param>
        /// <returns></returns>
        public int JudgeInsert(JobReminderViewModel items,string YM , string GroupsCode) {
            #region 參數宣告				
            JobReminderDao jobReminderDao = new JobReminderDao();
            int ExecResult = 0;
            #endregion

            #region 流程																
            ExecResult = jobReminderDao.JudgeInsert(items, YM, GroupsCode); //將參數送入Dao層,組立SQL字串並連接資料庫
            return ExecResult;
            #endregion
        }
        #endregion

        #region 查詢設定項目清單 QuerySetting()
        /// <summary>
        /// 查詢設定項目清單
        /// </summary>
        /// <param name="GroupsCode"></param>
        /// <returns></returns>
        public QueryTableViewModel QuerySetting(string GroupsCode) {
            #region 參數宣告				
            QueryTableViewModel searchList = new QueryTableViewModel();
            JobReminderDao jobReminderDao = new JobReminderDao();
            #endregion

            #region 流程

            searchList = jobReminderDao.QuerySetting(GroupsCode); //將參數送入Dao層,組立SQL字串並連接資料庫

            return searchList;
            #endregion
        }
        #endregion

        #region 工作功能開關切換 isOnoff()
        /// <summary>
        /// 工作功能開關切換
        /// </summary>
        /// <param name="GroupsCode">角色</param>
        /// <param name="ActionCode">功能</param>
        /// <returns></returns>
        public int isOnoff(string GroupsCode, string ActionCode,string IsOn) {
            #region 參數宣告				
            int ExecResult = 0;
            JobReminderDao jobReminderDao = new JobReminderDao();
            #endregion

            #region 流程

            ExecResult = jobReminderDao.isOnoff(GroupsCode, ActionCode, IsOn); //將參數送入Dao層,組立SQL字串並連接資料庫

            return ExecResult;
            #endregion
        }
        #endregion     

        #region 撈取修改資料 QueryUpdateData()
        /// <summary>
        /// 撈取修改資料
        /// </summary>
        /// <param name="GroupsCode"></param>
        /// <returns></returns>
        public SearchItemViewModel QueryUpdateData(string GroupsCode, string ActionCode) {
            #region 參數宣告				
            SearchItemViewModel searchList = new SearchItemViewModel();
            JobReminderDao jobReminderDao = new JobReminderDao();
            #endregion

            #region 流程

            searchList = jobReminderDao.QueryUpdateData(GroupsCode, ActionCode); //將參數送入Dao層,組立SQL字串並連接資料庫

            return searchList;
            #endregion
        }
        #endregion

        #region 修改資料 Updatedata()
        /// <summary>
        /// 修改資料
        /// </summary>
        /// <param name="GroupsCode">角色</param>
        /// <param name="ActionCode">功能</param>
        /// <returns></returns>
        public int Updatedata(UpdateViewModel model) {
            #region 參數宣告				
            int ExecResult = 0;
            JobReminderDao jobReminderDao = new JobReminderDao();
            #endregion

            #region 流程

            ExecResult = jobReminderDao.Updatedata(model); //將參數送入Dao層,組立SQL字串並連接資料庫

            return ExecResult;
            #endregion
        }
        #endregion

        #region 修改執行狀態 UpdateIsRun()
        /// <summary>
        /// 修改執行狀態
        /// </summary>
        /// <param name="model">包含參數(群組代碼、年月、功能代碼)</param>
        /// <returns></returns>
        public int UpdateIsRun(UpdateIsRunViewModel model) {
            #region 參數宣告				
            int ExecResult = 0;
            JobReminderDao jobReminderDao = new JobReminderDao();
            #endregion

            #region 流程

            ExecResult = jobReminderDao.UpdateIsRun(model); //將參數送入Dao層,組立SQL字串並連接資料庫

            return ExecResult;
            #endregion
        }
        #endregion
        
    }
}