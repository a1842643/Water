using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using WaterCaseTracking.Dao;
using WaterCaseTracking.Models;
using WaterCaseTracking.Models.ViewModels.MCAsk;

namespace WaterCaseTracking.Service
{
    public class ExpectedProjectService
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
        #region 單筆新增MCAskTable-起
        internal void AddMCAskTable(MCAskModel mcaskModel, string UserName)
        {
            #region 參數宣告				
            MCAskDao mcaskDao = new MCAskDao();
            #endregion

            #region 流程			
            mcaskDao.AddMCAskTable(mcaskModel, UserName); //將參數送入Dao層,組立SQL字串並連接資料庫
            #endregion
        }

        #endregion 單筆新增MCAskTable-迄

        #region 單筆修改MCAskTable-起
        internal void UpdateMCAskTable(MCAskModel mcaskModel, string UserName)
        {
            #region 參數宣告				
            MCAskDao mcaskDao = new MCAskDao();
            #endregion

            #region 流程			
            mcaskDao.UpdateMCAskTable(mcaskModel, UserName); //將參數送入Dao層,組立SQL字串並連接資料庫
            #endregion
        }

        #endregion 單筆修改MCAskTable-迄

        #region 單筆刪除MCAskTable-起
        internal void DeleteMCAskTable(string ID, string Types, string UserName)
        {
            #region 參數宣告				
            MCAskDao mcaskDao = new MCAskDao();
            #endregion

            #region 流程			
            mcaskDao.DeleteMCAskTable(ID, Types, UserName); //將參數送入Dao層,組立SQL字串並連接資料庫
            #endregion
        }

        #endregion 單筆刪除MCAskTable-迄

        #region 修改用查詢
        internal MCAskModel QueryUpdateData(string ID, string Types)
        {
            #region 參數宣告				
            MCAskModel mcaskModel = new MCAskModel();
            MCAskDao mcaskDao = new MCAskDao();
            #endregion

            #region 流程																
            mcaskModel = mcaskDao.QueryUpdateData(ID, Types); //將參數送入Dao層,組立SQL字串並連接資料庫

            return mcaskModel;
            #endregion
        }

        #endregion
    }
}