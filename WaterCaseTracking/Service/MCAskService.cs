using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using WaterCaseTracking.Dao;
using WaterCaseTracking.Models.ViewModels.MCAsk;

namespace WaterCaseTracking.Service
{
    public class MCAskService
    {
        #region 查詢 QuerySearchList()
        public SearchListViewModel QuerySearchList(SearchInfoViewModel searchInfo)
        {
            #region 參數宣告				
            SearchListViewModel searchList = new SearchListViewModel();
            MCAskDao mcaskDao = new MCAskDao();
            #endregion

            #region 流程																
            searchList = mcaskDao.QuerySearchList(searchInfo); //將參數送入Dao層,組立SQL字串並連接資料庫

            return searchList;
            #endregion
        }
        #endregion

        #region 匯出範例檔-起
        internal DataTable getExportData(ExportViewModel exportViewModel)
        {
            #region 參數宣告				
            MCAskDao mcaskDao = new MCAskDao();
            DataTable dt = new DataTable();
            #endregion

            #region 流程																
            dt = mcaskDao.getExportData(exportViewModel); //將參數送入Dao層,組立SQL字串並連接資料庫

            return dt;
            #endregion
        }
        #endregion 匯出範例檔-迄
    }
}