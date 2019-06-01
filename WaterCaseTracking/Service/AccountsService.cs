using WaterCaseTracking;
using WaterCaseTracking.Dao;
using WaterCaseTracking.Models;
using WaterCaseTracking.Models.ViewModels.Accounts;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace WaterCaseTracking.Service
{
    public class AccountsService
    {
        #region 查詢 QuerySearchList()
        public SearchListViewModel QuerySearchList(SearchInfoViewModel searchInfo)
        {
            #region 參數宣告				
            SearchListViewModel searchList = new SearchListViewModel();
            AccountsDao accountsDao = new AccountsDao();
            #endregion

            #region 流程																
            searchList = accountsDao.QuerySearchList(searchInfo); //將參數送入Dao層,組立SQL字串並連接資料庫

            return searchList;
            #endregion
        }
        /// <summary>
        /// 修改初始值
        /// </summary>
        /// <param name="Branch_No">Branch_No</param>
        /// <returns></returns>
        #endregion
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

       

    }
}