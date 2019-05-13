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
        #region 取得公告資料列表 GetbulletinList()
        public SearchListViewModel GetbulletinList(SearchInfoViewModel searchInfo)
        {
            #region 參數宣告				
            SearchListViewModel searchData = new SearchListViewModel();
            SYS_announcementDao SYS_announcementDao = new SYS_announcementDao();
            #endregion

            #region 流程

            searchData = SYS_announcementDao.GetbulletinList(searchInfo); //將參數送入Dao層,組立SQL字串並連接資料庫

            return searchData;
            #endregion
        }
        #endregion

        #region 傳入公告資料 PostBulletin()
        public SearchListViewModel PostBulletin(SYS_announcementModel searchInfo)
        {
            #region 參數宣告				
            SearchListViewModel searchData = new SearchListViewModel();
            SYS_announcementDao SYS_announcementDao = new SYS_announcementDao();
            #endregion

            #region 流程

            if (searchInfo.ID != null)
            {
                searchData = SYS_announcementDao.UpDateBulletin(searchInfo); //將參數送入Dao層,組立SQL字串並連接資料庫
            }
            else
            {
                searchData = SYS_announcementDao.AddBulletin(searchInfo); //將參數送入Dao層,組立SQL字串並連接資料庫
            }

            return searchData;
            #endregion
        }
        #endregion

        #region 刪除公告資料 DeleteBulletin()
        public SearchListViewModel DeleteBulletin(SYS_announcementModel searchInfo)
        {
            #region 參數宣告				
            SearchListViewModel searchData = new SearchListViewModel();
            SYS_announcementDao SYS_announcementDao = new SYS_announcementDao();
            #endregion

            #region 流程

            searchData = SYS_announcementDao.DeleteBulletin(searchInfo); //將參數送入Dao層,組立SQL字串並連接資料庫

            return searchData;
            #endregion
        }
        #endregion

        #region 取得公告資料列表
        public SearchListViewModel Getbulletin(SearchInfoViewModel searchInfo)
        {
            #region 參數宣告				
            SearchListViewModel searchData = new SearchListViewModel();
            SYS_announcementDao SYS_announcementDao = new SYS_announcementDao();
            #endregion

            #region 流程

            searchData = SYS_announcementDao.Getbulletin(searchInfo); //將參數送入Dao層,組立SQL字串並連接資料庫

            return searchData;
            #endregion
        }
        #endregion

        #region 取得功能列表 GetOperating()
        public SearchListViewModel GetOperating(string searchInfo)
        {
            #region 參數宣告				
            SearchListViewModel searchData = new SearchListViewModel();
            SYS_announcementDao SYS_announcementDao = new SYS_announcementDao();
            #endregion

            #region 流程

            searchData = SYS_announcementDao.GetOperating(searchInfo); //將參數送入Dao層,組立SQL字串並連接資料庫

            return searchData;
            #endregion
        }
        #endregion
    }
}