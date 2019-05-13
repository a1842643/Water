using WaterCaseTracking.Models.ViewModels.SystemPermission;
using WaterCaseTracking.Dao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WaterCaseTracking.Models;

namespace WaterCaseTracking.Service
{
    public class SystemPermissionService
    {
        #region 設定角色權限 SetPermission()
        public string SetPermission(SearchInfoViewModel searchInfo)
        {
            #region 參數宣告				
            string searchData = string.Empty;
            SystemPermissionDao SystemPermissionDao = new SystemPermissionDao();
            #endregion

            #region 流程

            searchData = SystemPermissionDao.SetPermission(searchInfo); //將參數送入Dao層,組立SQL字串並連接資料庫

            return searchData;
            #endregion
        }
        #endregion

        #region 查詢角色權限 GetPermission()
        public SearchListViewModel GetPermission(SearchInfoViewModel searchInfo)
        {
            #region 參數宣告				
            SearchListViewModel searchData = new SearchListViewModel();
            SystemPermissionDao SystemPermissionDao = new SystemPermissionDao();
            #endregion

            #region 流程

            searchData = SystemPermissionDao.GetPermission(searchInfo); //將參數送入Dao層,組立SQL字串並連接資料庫

            return searchData;
            #endregion
        }
        #endregion

        #region 查詢角色 GetPermissionList()
        public SearchListViewModel GetPermissionList()
        {
            #region 參數宣告				
            SearchListViewModel searchData = new SearchListViewModel();
            SystemPermissionDao SystemPermissionDao = new SystemPermissionDao();
            #endregion

            #region 流程

            searchData = SystemPermissionDao.GetPermissionList(); //將參數送入Dao層,組立SQL字串並連接資料庫

            return searchData;
            #endregion
        }
        #endregion

        #region 刪除角色 DelPermission()
        public void DelPermission(SearchInfoViewModel searchInfo)
        {
            #region 參數宣告				
            SystemPermissionDao SystemPermissionDao = new SystemPermissionDao();
            #endregion

            #region 流程
            SystemPermissionDao.DelPermission(searchInfo); //將參數送入Dao層,組立SQL字串並連接資料庫
            SystemPermissionDao.DelPermissionProg(searchInfo); //將參數送入Dao層,組立SQL字串並連接資料庫

            #endregion
        }
        #endregion

        #region 設定角色 EditPermission()
        public SearchListViewModel EditPermission(SearchInfoViewModel searchInfo)
        {
            #region 參數宣告				
            SearchListViewModel searchData = new SearchListViewModel();
            SystemPermissionDao SystemPermissionDao = new SystemPermissionDao();
            #endregion

            #region 流程
            if (searchInfo.ID == "" || searchInfo.ID == null)
            {
                SystemPermissionDao.AddPermission(searchInfo); //將參數送入Dao層,組立SQL字串並連接資料庫
                SystemPermissionDao.AddPermissionProg(searchInfo); //將參數送入Dao層,組立SQL字串並連接資料庫
            }
            else
            {
                SystemPermissionDao.UpdataPermissionProg(searchInfo); //將參數送入Dao層,組立SQL字串並連接資料庫
                SystemPermissionDao.UpdataPermission(searchInfo); //將參數送入Dao層,組立SQL字串並連接資料庫            
            }

            return searchData;
            #endregion
        }
        #endregion

    }
}