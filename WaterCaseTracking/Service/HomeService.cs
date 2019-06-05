using WaterCaseTracking.Dao;
using WaterCaseTracking.Models;
using WaterCaseTracking.Models.ViewModels.Home;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace WaterCaseTracking.Service
{
    public class HomeService
    {
        #region 取得功能列表 GetOperating()
        public SearchListViewModel GetOperating(string searchInfo)
        {
            #region 參數宣告				
            SearchListViewModel searchData = new SearchListViewModel();
            PermissionProgDao permissionProgDao = new PermissionProgDao();
            #endregion

            #region 流程

            searchData = permissionProgDao.GetOperating(searchInfo); //將參數送入Dao層,組立SQL字串並連接資料庫

            return searchData;
            #endregion
        }
        #endregion
        #region 取得登入者資訊
        internal AccountsModel QueryAccountInfo(string AccountID, string Password)
        {
            #region 參數宣告				
            AccountsModel accountsModel = new AccountsModel();
            AccountsDao accountsDao = new AccountsDao();
            #endregion

            #region 流程																
            accountsModel = accountsDao.QueryAccountInfo(AccountID, Password); //將參數送入Dao層,組立SQL字串並連接資料庫

            return accountsModel;
            #endregion
        }

        #endregion
        #region 抓錯誤次數(起)
        internal LoginErrorTimesModel getAccountErrorTimes(string txtAccount)
        {
            #region 參數宣告				
            LoginErrorTimesModel loginErrorTimesModel = new LoginErrorTimesModel();
            LoginErrorTimesDao loginErrorTimesDao = new LoginErrorTimesDao();
            #endregion

            #region 流程																
            loginErrorTimesModel = loginErrorTimesDao.getAccountErrorTimes(txtAccount); //將參數送入Dao層,組立SQL字串並連接資料庫

            return loginErrorTimesModel;
            #endregion
        }
        #endregion 抓錯誤次數(迄)
        #region 新增錯誤次數(起)
        internal void AddAccountErrorTimes(string txtAccount, bool isSuccess)
        {
            #region 參數宣告				
            LoginErrorTimesDao loginErrorTimesDao = new LoginErrorTimesDao();
            #endregion

            #region 流程																
            loginErrorTimesDao.AddAccountErrorTimes(txtAccount, isSuccess); //將參數送入Dao層,組立SQL字串並連接資料庫

            #endregion
        }
        #endregion 
        #region 修改錯誤次數(起)
        internal void UpdateAccountErrorTimes(LoginErrorTimesModel loginErrorTimesModel, bool isSuccess)
        {
            #region 參數宣告				
            LoginErrorTimesDao loginErrorTimesDao = new LoginErrorTimesDao();
            #endregion

            #region 流程																
            loginErrorTimesDao.UpdateAccountErrorTimes(loginErrorTimesModel, isSuccess); //將參數送入Dao層,組立SQL字串並連接資料庫

            #endregion
        }
        #endregion
    }
}