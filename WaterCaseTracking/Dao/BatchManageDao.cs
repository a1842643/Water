using System;
using Dapper;
using WaterCaseTracking.Models.ViewModels.BatchManage;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using WaterCaseTracking.Models.ViewModels;
using WaterCaseTracking.Models;
using System.Data;

namespace WaterCaseTracking.Dao
{
    public class BatchManageDao : _BaseDao
    {
        /// <summary>
        /// 1080402 1.拿掉所有條件欄位，留執行按鈕； 直接取得今日的上月資料(last YYMM)
        /// Log_Status 失敗 = 0,成功 = 1,中斷 = 2,等待執行 = 3,執行中 = 4,已申請 = 5
        /// </summary>
        /// <param name="searchInfo"></param>
        /// <returns></returns>
        public BatchManageListViewModel QuerySearchList(BatchManageSearchItemModel searchInfo)
        {
            //組立SQL字串並連接資料庫
            #region 參數告宣
            BatchManageListViewModel result = new BatchManageListViewModel();
            #endregion

            #region 流程
            //查詢SQL
            StringBuilder _sqlStr = new StringBuilder();
            //查詢條件SQL
            StringBuilder _sqlParamStr = new StringBuilder();
            //總筆數
            StringBuilder _sqlCountStr = new StringBuilder();
            _sqlCountStr.Append(@"select count(1) from BatchLogger 
                            WHERE Log_JobID in ('T','R') --只執行轉檔跟報表
                            and Log_YM=FORMAT(year(DATEADD(month, -1, getdate()))-1911 ,'000')+FORMAT(month(DATEADD(month, -1, getdate())),'00')  ");
            _sqlStr.Append(@"select 
                                Log_YM                      --年月
                                , Log_JobID                   --排程作業類別id
                                , Log_JobName                   --排程作業類別
                                , Log_FuncName                 --作業項目代碼
                                , Log_Description           --作業項目
                                , Format(Log_StartTime, 'yyyy/MM/dd HH:mm:ss') as Log_StartTime --作業開始時間
                                , Format(Log_EndTime, 'yyyy/MM/dd HH:mm:ss') as Log_EndTime --作業結束時間
                                , Log_UserID            --操作者ID
                                , Log_Status
                                , (CASE ISNULL(Log_Status,'') WHEN '1' THEN '成功' WHEN '0' THEN '失敗' WHEN '2' THEN '中斷' WHEN '3' THEN '等待執行' WHEN '4' THEN '執行中'  WHEN '5' THEN '已申請'   ELSE '' END)  AS Log_StatusName            --執行狀態
                            from BatchLogger 
                            WHERE Log_JobID in ('T','R') --只執行轉檔跟報表
                            and Log_YM = FORMAT(year(DATEADD(month, -1, getdate())) - 1911, '000') + FORMAT(month(DATEADD(month, -1, getdate())), '00')  ");

            _sqlParams = new Dapper.DynamicParameters();
            //年月
            if (!string.IsNullOrEmpty(searchInfo.txtLog_YM))
            {
                _sqlParamStr.Append(" and Log_YM = @Log_YM ");
                _sqlParams.Add("Log_YM", searchInfo.txtLog_YM);
            }
            //排程作業類別
            if (!string.IsNullOrEmpty(searchInfo.ddlLog_JobID))
            {
                _sqlParamStr.Append(" and Log_JobID = @Log_JobID ");
                _sqlParams.Add("Log_JobID", searchInfo.ddlLog_JobID);
            }
            //排程作業項目
            if (!string.IsNullOrEmpty(searchInfo.ddlLog_FuncName))
            {
                _sqlParamStr.Append(" and Log_FuncName = @Log_FuncName ");
                _sqlParams.Add("Log_FuncName", searchInfo.ddlLog_FuncName);
            }
            #region 條件、排序(起)
            _sqlStr.Append(_sqlParamStr);
            _sqlCountStr.Append(_sqlParamStr);
            _sqlStr.Append(" ORDER BY Log_YM DESC,Log_JobID desc, Log_FuncName");
            //_sqlParams.Add("iDisplayStart", searchInfo.iDisplayStart);
            //_sqlParams.Add("iDisplayLength", searchInfo.iDisplayLength);
            #endregion 條件、排序(迄)

            result.data = new List<BatchManageItemModel>();
            result.draw = (searchInfo.sEcho + 1);
            using (var cn = new SqlConnection(_dbConnPPP)) //連接資料庫
            {
                cn.Open();
                result.data = cn.Query<BatchManageItemModel>(_sqlStr.ToString(), _sqlParams).ToList();
                result.recordsTotal = cn.Query<int>(_sqlCountStr.ToString(), _sqlParams).First();
                result.recordsFiltered = result.recordsTotal;
            }
            return result;
            #endregion
        }
        
        public BatchLoggerModel QuerySearchLog(BatchLoggerSearchItemModel searchInfo)
        {
            //組立SQL字串並連接資料庫
            #region 參數宣告
            BatchLoggerModel result = new BatchLoggerModel();
            #endregion

            #region 流程
            //查詢SQL
            StringBuilder _sqlStr = new StringBuilder();
            //查詢條件SQL
            StringBuilder _sqlParamStr = new StringBuilder();
            //總筆數
            StringBuilder _sqlCountStr = new StringBuilder();
            _sqlCountStr.Append("select count(1) from BatchLogger WHERE 1 = 1 ");
            _sqlStr.Append(@"select 
                                Log_Text
                            from BatchLogger WHERE 1 = 1 ");

            _sqlParams = new Dapper.DynamicParameters();
            _sqlParamStr.Append(" and Log_YM = @Log_YM ");
            _sqlParams.Add("Log_YM", searchInfo.YM);
            _sqlParamStr.Append(" and Log_JobID = @Log_JobID ");
            _sqlParams.Add("Log_JobID", searchInfo.JobID);
            _sqlParamStr.Append(" and Log_FuncName = @Log_FuncName ");
            _sqlParams.Add("Log_FuncName", searchInfo.FuncN);

            #region 條件、排序(起)
            _sqlStr.Append(_sqlParamStr);
            _sqlCountStr.Append(_sqlParamStr);
            //_sqlStr.Append(" ORDER BY " + (searchInfo.iSortCol_0 + 1).ToString() + " " + searchInfo.sSortDir_0 + " OFFSET @iDisplayStart ROWS  FETCH NEXT @iDisplayLength ROWS ONLY ");
            //_sqlParams.Add("iDisplayStart", searchInfo.iDisplayStart);
            //_sqlParams.Add("iDisplayLength", searchInfo.iDisplayLength);
            #endregion 條件、排序(迄)
            using (var cn = new SqlConnection(_dbConnPPP)) //連接資料庫
            {
                cn.Open();
                var rv = cn.Query<BatchLoggerModel>(_sqlStr.ToString(), _sqlParams).FirstOrDefault();
                result.Log_Text = rv.Log_Text;
            }
            return result;
            #endregion
        }
        internal int ExecuteBatch(List<BatchManageItemModel> listModel)
        {
            //組立SQL字串並連接資料庫
            StringBuilder _sqlStr = new StringBuilder();
            _sqlStr.Append(@"Insert Into BatchLogger ( Log_YM
                            ,Log_JobID
                            ,Log_JobName
                            ,Log_FuncName
                            ,Log_Description
                            ,Log_ExRate
                            ,Log_StartTime
                            ,Log_UserID
                            ,Log_Status
                            ,Log_Text
                            )
                            Values(
                            @Log_YM
                            ,@Log_JobID
                            ,@Log_JobName
                            ,@Log_FuncName
                            ,@Log_Description
                            ,@Log_ExRate
                            ,@Log_StartTime
                            ,@Log_UserID
                            ,@Log_Status
                            ,@Log_Text      
                            )
                ");
            _sqlParamsList = new List<DynamicParameters>();
            foreach (var model in listModel)
            {
                _sqlParams = new Dapper.DynamicParameters();
                _sqlParams.Add("Log_YM", model.Log_YM);
                _sqlParams.Add("Log_JobID", model.Log_JobID);
                _sqlParams.Add("Log_JobName", model.Log_JobName);
                _sqlParams.Add("Log_FuncName", model.Log_FuncName);
                _sqlParams.Add("Log_Description", model.Log_Description);
                _sqlParams.Add("Log_ExRate", model.Log_ExRate);
                _sqlParams.Add("Log_StartTime", model.Log_StartTime);
                _sqlParams.Add("Log_UserID", model.Log_UserID);
                _sqlParams.Add("Log_Status", model.Log_Status);
                _sqlParams.Add("Log_Text", model.Log_Text);
                _sqlParamsList.Add(_sqlParams);
            }

            try
            {
                using (var cn = new SqlConnection(_dbConnPPP))//連接資料庫
                {
                    cn.Open();
                    var ExecResult = cn.Execute(_sqlStr.ToString(), _sqlParamsList);
                    return ExecResult;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        internal int ExecuteBatchAgain(List<BatchManageItemModel> listModel)
        {
            //組立SQL字串並連接資料庫
            StringBuilder _sqlStr = new StringBuilder();
            _sqlStr.Append(@"update BatchLogger set
                             Log_StartTime=@Log_StartTime
                            ,Log_UserID=@Log_UserID
                            ,Log_Status=@Log_Status
                            ,Log_Text=@Log_Text+ISNULL(Log_Text,'')
                            where
                            Log_YM=@Log_YM
                            and Log_JobID=@Log_JobID
                            and Log_FuncName=@Log_FuncName
                ");
            _sqlParamsList = new List<DynamicParameters>();
            foreach (var model in listModel)
            {
                _sqlParams = new Dapper.DynamicParameters();
                _sqlParams.Add("Log_YM", model.Log_YM);
                _sqlParams.Add("Log_JobID", model.Log_JobID);
                _sqlParams.Add("Log_FuncName", model.Log_FuncName);
                _sqlParams.Add("Log_StartTime", model.Log_StartTime);
                _sqlParams.Add("Log_UserID", model.Log_UserID);
                _sqlParams.Add("Log_Status", model.Log_Status);
                _sqlParams.Add("Log_Text", model.Log_Text);
                _sqlParamsList.Add(_sqlParams);
            }

            try
            {
                using (var cn = new SqlConnection(_dbConnPPP))//連接資料庫
                {
                    cn.Open();
                    var ExecResult = cn.Execute(_sqlStr.ToString(), _sqlParamsList);
                    return ExecResult;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        internal void applyToExecuteBatch(BatchManageItemModel BatchModel, ref SqlConnection conn, ref SqlTransaction trans)
        {
            //組立SQL字串並連接資料庫
            StringBuilder _sqlStr = new StringBuilder();
            _sqlStr.Append(@"update BatchLogger set
                            Log_Status=@Log_Status
                            where
                            Log_YM=@Log_YM
                ");
                _sqlParamsList = new List<DynamicParameters>();
                _sqlParams = new Dapper.DynamicParameters();
                _sqlParams.Add("Log_YM", BatchModel.Log_YM);
                _sqlParams.Add("Log_Status", BatchModel.Log_Status);
                _sqlParamsList.Add(_sqlParams);

            try {
                var ExecResult = conn.Execute(_sqlStr.ToString(), _sqlParams, trans);
            }
            catch (Exception ex) {
                TransactionRollback(trans);
                GetCloseConnection(conn);
                throw ex;
            }
        }
    }
}