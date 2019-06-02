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
    }
}