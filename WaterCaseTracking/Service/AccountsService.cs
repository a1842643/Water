﻿using WaterCaseTracking;
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
        public SearchListViewModel QuerySearchList(SearchInfoViewModel searchInfo, string roleName, string Organizer)
        {
            #region 參數宣告				
            SearchListViewModel searchList = new SearchListViewModel();
            AccountsDao accountsDao = new AccountsDao();
            #endregion

            #region 流程																
            searchList = accountsDao.QuerySearchList(searchInfo, roleName, Organizer); //將參數送入Dao層,組立SQL字串並連接資料庫

            return searchList;
            #endregion
        }
        #endregion

        #region 取得登入者資訊
        internal AccountsModel QueryAccountInfo(string AccountID, string AlienSecurity)
        {
            #region 參數宣告				
            AccountsModel accountsModel = new AccountsModel();
            AccountsDao accountsDao = new AccountsDao();
            #endregion

            #region 流程																
            accountsModel = accountsDao.QueryAccountInfo(AccountID, AlienSecurity); //將參數送入Dao層,組立SQL字串並連接資料庫

            return accountsModel;
            #endregion
        }



        #endregion
        internal AccountsModel QueryAccountInfo(string AccountID)
        {
            #region 參數宣告				
            AccountsModel accountsModel = new AccountsModel();
            AccountsDao accountsDao = new AccountsDao();
            #endregion

            #region 流程																
            accountsModel = accountsDao.QueryAccountInfo(AccountID); //將參數送入Dao層,組立SQL字串並連接資料庫

            return accountsModel;
            #endregion
        }

        #region 修改密碼-起
        internal int ToChangePW(ChangePwViewModel changePwViewModel)
        {
            #region 參數宣告				
            AccountsDao accountsDao = new AccountsDao();
            AccountsModel accountsModel = new AccountsModel();
            #endregion

            #region 流程
            //修改密碼
            return accountsDao.ToChangePW(changePwViewModel); //將參數送入Dao層,組立SQL字串並連接資料庫
            #endregion
        }

        #endregion 修改密碼-迄

        #region 確認密碼有無與前三次相同
        internal AccountsModel CheckAlienSecurity(ChangePwViewModel changePwViewModel)
        {
            #region 參數宣告				
            AccountsModel accountsModel = new AccountsModel();
            AccountsDao accountsDao = new AccountsDao();
            #endregion

            #region 流程																
            accountsModel = accountsDao.CheckAlienSecurity(changePwViewModel);

            return accountsModel;
            #endregion
        }




        #endregion
        #region 啟用/停用
        internal void EnableAccount(string idx, string userName)
        {
            #region 參數宣告				
            AccountsDao accountsDao = new AccountsDao();
            AccountsModel accountsModel = new AccountsModel();
            #endregion

            #region 流程
            //修改密碼
            accountsDao.EnableAccount(idx, userName); //將參數送入Dao層,組立SQL字串並連接資料庫
            #endregion
        }

        #endregion
        #region 重設回預設密碼
        internal void pwToDefault(string idx, string userName)
        {
            #region 參數宣告				
            AccountsDao accountsDao = new AccountsDao();
            AccountsModel accountsModel = new AccountsModel();
            #endregion

            #region 流程
            //修改密碼
            accountsDao.pwToDefault(idx, userName); //將參數送入Dao層,組立SQL字串並連接資料庫
            #endregion
        }

        #endregion


        #region 單筆修改
        internal void UpdateAccountsTable(AccountsModel accountsModel, string userName)
        {
            #region 參數宣告				
            AccountsDao accountsDao = new AccountsDao();
            #endregion

            #region 流程																
            accountsDao.UpdateAccountsTable(accountsModel, userName);

            #endregion
        }
        #endregion

        #region 單筆新增
        internal void AddAccountsTable(AccountsModel accountsModel, string userName)
        {
            #region 參數宣告				
            AccountsDao accountsDao = new AccountsDao();
            #endregion

            #region 流程																
            accountsDao.AddAccountsTable(accountsModel, userName);

            #endregion
        }

        
        #endregion
    }
}