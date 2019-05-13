using WaterCaseTracking;
using WaterCaseTracking.Dao;
using WaterCaseTracking.Models;
using WaterCaseTracking.Models.ViewModels.BatchManage;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace WaterCaseTracking.Service
{
    public class BatchManageService
    {

        #region 查詢 Query()
        public BatchManageListViewModel QuerySearchList(BatchManageSearchItemModel searchInfo)
        {
            #region 參數宣告				
            BatchManageListViewModel searchList = new BatchManageListViewModel();
            BatchManageDao BatchManageDao = new BatchManageDao();
            #endregion

            #region 流程																
            searchList = BatchManageDao.QuerySearchList(searchInfo); //將參數送入Dao層,組立SQL字串並連接資料庫

            return searchList;
            #endregion
        }
        #endregion

        public BatchLoggerModel QuerySearchLog(BatchLoggerSearchItemModel searchInfo)
        {
            #region 參數宣告				
            BatchLoggerModel searchList = new BatchLoggerModel();
            BatchManageDao BatchManageDao = new BatchManageDao();
            #endregion

            #region 流程																
            searchList = BatchManageDao.QuerySearchLog(searchInfo); //將參數送入Dao層,組立SQL字串並連接資料庫

            return searchList;
            #endregion
        }

        internal int ExecuteBatch(BatchManageItemModel BatchItem)
        {
            #region 參數宣告 
            BatchManageDao BatchManageDao = new BatchManageDao();
            #endregion

            #region 流程		
            List<BatchManageItemModel> listModel = new List<BatchManageItemModel>();
            listModel.Add(BatchItem);
            return BatchManageDao.ExecuteBatch(listModel); //將參數送入Dao層,組立SQL字串並連接資料庫
            #endregion
        }


        internal void applyToExecuteBatch(string doYYYMM, ref SqlConnection conn, ref SqlTransaction trans)
        {
            try
            {
                BatchManageDao BatchManageDao = new BatchManageDao();
                BatchManageItemModel BatchItem = new BatchManageItemModel();
                BatchItem.Log_YM = doYYYMM;
                BatchItem.Log_Status = "3";//Log_Status 失敗 = 0,成功 = 1,中斷 = 2,等待執行 = 3,執行中 = 4,已申請 = 5


                BatchManageDao.applyToExecuteBatch(BatchItem, ref conn, ref trans); //將參數送入Dao層,組立SQL字串並連接資料庫
            }
            catch (Exception ex) { throw ex; }
        }

        internal int ExecuteBatchAgain(BatchManageItemModel BatchItem)
        {
            #region 參數宣告 
            BatchManageDao BatchManageDao = new BatchManageDao();
            #endregion

            #region 流程		
            List<BatchManageItemModel> listModel = new List<BatchManageItemModel>();
            listModel.Add(BatchItem);
            return BatchManageDao.ExecuteBatchAgain(listModel); //將參數送入Dao層,組立SQL字串並連接資料庫
            #endregion
        }

    }
}