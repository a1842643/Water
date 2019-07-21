using Dapper;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using WaterCaseTracking.Models.Common;
using WaterCaseTracking.Models.ViewModels;
using System.Linq;
using WaterCaseTracking.Models.ViewModels.MinuteLog;

namespace WaterCaseTracking.Dao
{
    public class LoggDao : _BaseDao
    {
        #region 查詢 QuerySearchList()
        public MinuteLogItemViewModel QuerySearchList(QueryGetoTableViewModel searchInfo) {
            //組立SQL字串並連接資料庫
            #region 參數告宣
            MinuteLogItemViewModel result = new MinuteLogItemViewModel();
            #endregion

            #region 流程
            //查詢SQL
            StringBuilder _sqlStr = new StringBuilder();
            //查詢條件SQL
            StringBuilder _sqlParamStr = new StringBuilder();
            //總筆數
            StringBuilder _sqlCountStr = new StringBuilder();
            _sqlCountStr.Append("select count(1) from Logg WHERE 1 = 1 ");
            _sqlStr.Append(@"  SELECT 
                                      UserID								   --操作員ID
                                      ,UserName						           --操作員Name
                                      ,FuncName						           --動作名稱
                                      ,Status								   --狀態
                                      ,Message						           --內容
                                      ,ID									   --流水號
                                      ,CONVERT(varchar(100), logged, 121) as   logged    --紀錄時間
                                  FROM Logg WHERE 1 = 1 ");
            //,CONVERT(varchar(100), logged, 121)--紀錄時間
            _sqlParams = new Dapper.DynamicParameters();
            //操作員Name
            if (!string.IsNullOrEmpty(searchInfo.ddlUserName)) {
                _sqlParamStr.Append(" and UserName = @UserName ");
                _sqlParams.Add("UserName", searchInfo.ddlUserName);
            }
            //動作名稱
            if (!string.IsNullOrEmpty(searchInfo.ddlFuncName)) {
                _sqlParamStr.Append(" and FuncName = @FuncName ");
                _sqlParams.Add("FuncName", searchInfo.ddlFuncName);
            }
            //狀態
            if (!string.IsNullOrEmpty(searchInfo.ddlStatus)) {
                _sqlParamStr.Append(" and Status = @Status ");
                _sqlParams.Add("Status", searchInfo.ddlStatus);
            }

            //紀錄時間(區間)
            if (!string.IsNullOrEmpty(searchInfo.ddlloggedStart))
            {
                _sqlParams.Add("loggedStart", searchInfo.ddlloggedStart);
                _sqlParamStr.Append(" and logged >= @loggedStart");
            }
            if (!string.IsNullOrEmpty(searchInfo.ddlloggedEnd))
            {
                _sqlParams.Add("loggedEnd", searchInfo.ddlloggedEnd);
                _sqlParamStr.Append(" and logged < DateAdd(Day,1,@loggedEnd)");
            }

            

            //if (!(string.IsNullOrEmpty(searchInfo.ddlloggedStart) || string.IsNullOrEmpty(searchInfo.ddlloggedEnd))) {
            //    searchInfo.ddlloggedStart = DateTime.Parse(searchInfo.ddlloggedStart).AddYears(1911).ToString("yyyy-MM-dd");
            //    searchInfo.ddlloggedEnd = DateTime.Parse(searchInfo.ddlloggedEnd).AddYears(1911).ToString("yyyy-MM-dd");
            //    _sqlParamStr.Append(" and logged BETWEEN CONVERT(varchar(100),@loggedStart, 121) AND CONVERT(varchar(100),@loggedEnd, 121) ");
            //    _sqlParams.Add("loggedStart", searchInfo.ddlloggedStart + " 00:00:00.000");
            //    _sqlParams.Add("loggedEnd", searchInfo.ddlloggedEnd + " 23:59:59.999");
            //}

            #region 條件、排序(起)
            _sqlStr.Append(_sqlParamStr);
            _sqlCountStr.Append(_sqlParamStr);
            _sqlStr.Append(" ORDER BY " + (searchInfo.iSortCol_0 + 1).ToString() + " " + searchInfo.sSortDir_0 + " OFFSET @iDisplayStart ROWS  FETCH NEXT @iDisplayLength ROWS ONLY ");
            _sqlParams.Add("iDisplayStart", searchInfo.iDisplayStart);
            _sqlParams.Add("iDisplayLength", searchInfo.iDisplayLength);
            #endregion 條件、排序(迄)

            result.data = new List<SearchItemViewModel>();
            result.draw = (searchInfo.sEcho + 1);
            using (var cn = new SqlConnection(_dbConnPPP)) //連接資料庫
            {
                cn.Open();
                result.data = cn.Query<SearchItemViewModel>(_sqlStr.ToString(), _sqlParams).ToList();
                result.recordsTotal = cn.Query<int>(_sqlCountStr.ToString(), _sqlParams).First();
                result.recordsFiltered = result.recordsTotal;
            }
            return result;
            #endregion
        }
        #endregion

        internal int AddLogg(LoggModel model)
        {
            //組立SQL字串並連接資料庫
            StringBuilder _sqlStr = new StringBuilder();
            _sqlStr.Append(@"Insert Into Logg ( 
                                	UserID	 --使用者編號
                                ,	UserName	 --使用者名稱
                                ,	FuncName	 --功能名稱
                                ,	Status	 --狀態
                                ,	Message	 --紀錄內容
                                ,	logged	 --紀錄時間

                            )
                            Values(
                                	@UserID	 --使用者編號
                                ,	@UserName	 --使用者名稱
                                ,	@FuncName	 --功能名稱
                                ,	@Status	 --狀態
                                ,	@Message	 --紀錄內容
                                ,	GETDATE()
                            )
                ");
            _sqlParamsList = new List<DynamicParameters>();
            _sqlParams = new Dapper.DynamicParameters();
            _sqlParams.Add("UserID", model.UserID);
            _sqlParams.Add("UserName", model.UserName);
            _sqlParams.Add("FuncName", model.FuncName);
            _sqlParams.Add("Status", model.Status);
            _sqlParams.Add("Message", model.Message);
            try
            {
                using (var cn = new SqlConnection(_dbConnPPP))//連接資料庫
                {
                    cn.Open();
                    var ExecResult = cn.Execute(_sqlStr.ToString(), _sqlParams);
                    return ExecResult;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #region 抓操作人員下拉選單
        /// <summary>
        /// 抓操作人員下拉選單
        /// </summary>
        /// <returns></returns>
        internal DropDownListViewModel getddlUserName() {
            //組立SQL字串並連接資料庫
            #region 參數告宣
            DropDownListViewModel result = new DropDownListViewModel();
            #endregion

            #region 流程

            StringBuilder _sqlStr = new StringBuilder();
            _sqlStr.Append(@"SELECT DISTINCT UserName as 'Values', UserName as 'Text' 
                             FROM Logg
                             WHERE UserName IS NOT NULL
                             Order by UserName ");
            _sqlParams = new Dapper.DynamicParameters();

            result.DropDownListLT = new List<DropDownListItem>();
            using (var cn = new SqlConnection(_dbConnPPP)) //連接資料庫
            {
                cn.Open();
                result.DropDownListLT = cn.Query<DropDownListItem>(_sqlStr.ToString(), _sqlParams).ToList();
            }
            return result;
            #endregion
        }
        #endregion

        #region 抓功能下拉選單
        /// <summary>
        /// 抓功能下拉選單
        /// </summary>
        /// <returns></returns>
        internal DropDownListViewModel getddlFuncName() {
            //組立SQL字串並連接資料庫
            #region 參數告宣
            DropDownListViewModel result = new DropDownListViewModel();
            #endregion

            #region 流程

            StringBuilder _sqlStr = new StringBuilder();
            _sqlStr.Append(@"SELECT DISTINCT FuncName as 'Values', FuncName as 'Text' 
                             FROM Logg
                             WHERE FuncName IS NOT NULL
                             Order by FuncName ");
            _sqlParams = new Dapper.DynamicParameters();

            result.DropDownListLT = new List<DropDownListItem>();
            using (var cn = new SqlConnection(_dbConnPPP)) //連接資料庫
            {
                cn.Open();
                result.DropDownListLT = cn.Query<DropDownListItem>(_sqlStr.ToString(), _sqlParams).ToList();
            }
            return result;
            #endregion
        }
        #endregion

        #region 抓狀態下拉選單
        /// <summary>
        /// 抓狀態下拉選單
        /// </summary>
        /// <returns></returns>
        internal DropDownListViewModel getddlStatus() {
            //組立SQL字串並連接資料庫
            #region 參數告宣
            DropDownListViewModel result = new DropDownListViewModel();
            #endregion

            #region 流程

            StringBuilder _sqlStr = new StringBuilder();
            _sqlStr.Append(@"SELECT DISTINCT Status as 'Values', Status as 'Text' 
                             FROM Logg
                             WHERE Status IS NOT NULL
                             Order by Status ");
            _sqlParams = new Dapper.DynamicParameters();

            result.DropDownListLT = new List<DropDownListItem>();
            using (var cn = new SqlConnection(_dbConnPPP)) //連接資料庫
            {
                cn.Open();
                result.DropDownListLT = cn.Query<DropDownListItem>(_sqlStr.ToString(), _sqlParams).ToList();
            }
            return result;
            #endregion
        }
        #endregion
    }
}