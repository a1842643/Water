using WaterCaseTracking.Dao;
using WaterCaseTracking.Models.ViewModels.KPI_Busines;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WaterCaseTracking.Service
{
    public class KPI_BusinesService
    {
        #region 取得項目資料
        public SearchListViewModel GetItemRatio(SearchInfoViewModel searchInfo)
        {
            #region 參數宣告				
            SearchListViewModel searchList = new SearchListViewModel();
            KPI_BusinesDao KPI_BusinesDao = new KPI_BusinesDao();
            #endregion

            #region 流程
            searchInfo.GROUPS = "管理績效";
            searchInfo.LEVELS = 1;
            searchInfo.SUBJECT = "管理權重";
            searchList.ManageRatio = KPI_BusinesDao.GetItemRatio(searchInfo); //將參數送入Dao層,組立SQL字串並連接資料庫

            searchInfo.LEVELS = 3;
            searchInfo.SUBJECT = "考核項目及配分";
            searchList.ManageDistribution = KPI_BusinesDao.GetItemManage(searchInfo); //將參數送入Dao層,組立SQL字串並連接資料庫
            searchInfo.SUBJECT = "專案扣分及配分";
            searchList.ManageDeduction = KPI_BusinesDao.GetItemManage(searchInfo); //將參數送入Dao層,組立SQL字串並連接資料庫

            searchInfo.GROUPS = "業績績效";
            searchInfo.LEVELS = 1;
            searchInfo.SUBJECT = "業績權重";
            searchList.PerformanceRatio = KPI_BusinesDao.GetItemRatio(searchInfo); //將參數送入Dao層,組立SQL字串並連接資料庫

            return searchList;
            #endregion
        }
        #endregion

        #region 設定項目配比
        public SearchListViewModel SetItem(SearchInfoViewModel searchInfo)
        {
            #region 參數宣告				
            SearchListViewModel searchData = new SearchListViewModel();
            SearchItem SearchItem = new SearchItem();
            KPI_BusinesDao KPI_BusinesDao = new KPI_BusinesDao();
            string ManageID;
            string PerformanceID;
            #endregion

            #region 流程

            #region 管理績效權重設定
            searchInfo.GROUPS = "管理績效";
            searchInfo.LEVELS = 1;
            searchInfo.SUBJECT = "管理權重";
            ManageID = KPI_BusinesDao.GetItemID(searchInfo).data[0].UUID.ToString(); //將參數送入Dao層,組立SQL字串並連接資料庫
            searchInfo.UUID = ManageID;
            searchData = KPI_BusinesDao.GetItemDisbution(searchInfo);

            SearchItem.UUID = ManageID;
            SearchItem.DISBUTION = searchInfo.ManageRatio;

            #region 項目新增或更新	
            if (searchData.data != null && searchData.data.Count > 0)
            {
                KPI_BusinesDao.UpDataItem(SearchItem);
            }
            else
            {
                KPI_BusinesDao.InsertItem(SearchItem);
            }
            #endregion
            #endregion

            #region 管理績效分數設定
            //searchInfo.GROUPS = "管理績效";
            //searchInfo.LEVELS = 3;
            //searchInfo.SUBJECT = "考核項目及配分";
            //ManageID = KPI_BusinesDao.GetItemID(searchInfo).data[0].UUID.ToString(); //將參數送入Dao層,組立SQL字串並連接資料庫

            //searchInfo.UUID = ManageID;
            //searchData = KPI_BusinesDao.GetItemDisbution(searchInfo);

            //SearchItem.UUID = ManageID;
            //SearchItem.DISBUTION = searchInfo.ManageDistribution;

            //#region 項目新增或更新	
            //if (searchData.data != null && searchData.data.Count > 0)
            //{
            //    KPI_BusinesDao.UpDataItem(SearchItem);
            //}
            //else
            //{
            //    KPI_BusinesDao.InsertItem(SearchItem);
            //}
            //#endregion
            #endregion

            #region 管理績效扣分設定
            //searchInfo.GROUPS = "管理績效";
            //searchInfo.LEVELS = 3;
            //searchInfo.SUBJECT = "專案扣分及配分";
            //ManageID = KPI_BusinesDao.GetItemID(searchInfo).data[0].UUID.ToString(); //將參數送入Dao層,組立SQL字串並連接資料庫

            //searchInfo.UUID = ManageID;
            //searchData = KPI_BusinesDao.GetItemDisbution(searchInfo);

            //SearchItem.UUID = ManageID;
            //SearchItem.DISBUTION = searchInfo.ManageDeduction;

            //#region 項目新增或更新	
            //if (searchData.data != null && searchData.data.Count > 0)
            //{
            //    KPI_BusinesDao.UpDataItem(SearchItem);
            //}
            //else
            //{
            //    KPI_BusinesDao.InsertItem(SearchItem);
            //}
            //#endregion
            #endregion

            #region 業績績效權重設定

            searchInfo.GROUPS = "業績績效";
            searchInfo.LEVELS = 1;
            searchInfo.SUBJECT = "業績權重";
            PerformanceID = KPI_BusinesDao.GetItemID(searchInfo).data[0].UUID.ToString(); //將參數送入Dao層,組立SQL字串並連接資料庫
            searchInfo.UUID = PerformanceID;
            searchData = KPI_BusinesDao.GetItemDisbution(searchInfo);   //將參數送入Dao層,組立SQL字串並連接資料庫,獲取項目資訊

            SearchItem.UUID = PerformanceID;
            SearchItem.DISBUTION = searchInfo.PerformanceRatio;

            #region 項目新增或更新
            if (searchData.data != null && searchData.data.Count > 0)
            {
                KPI_BusinesDao.UpDataItem(SearchItem);
            }
            else
            {
                KPI_BusinesDao.InsertItem(SearchItem);
            }
            #endregion

            #endregion

            searchData.ManageRatio = searchInfo.ManageRatio;
            searchData.PerformanceRatio = searchInfo.PerformanceRatio;
            //searchData.ManageDistribution = searchInfo.ManageDistribution;
            //searchData.ManageDeduction = searchInfo.ManageDeduction;

            return searchData;
            #endregion
        }
        #endregion
    }
}