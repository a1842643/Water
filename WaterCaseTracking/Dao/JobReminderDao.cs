using Dapper;
using WaterCaseTracking.Models;
using WaterCaseTracking.Models.ViewModels.JobReminder;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;

namespace WaterCaseTracking.Dao {
    public class JobReminderDao : _BaseDao {

        #region 工作提醒設定-新增 Create()
        internal string Create(SearchItemViewModel model) {
            //組立SQL字串並連接資料庫
            //每個月固定做的事情,不可調整
            //只有年月跟執行狀態、通知狀態可以不一樣
            StringBuilder _sqlStr = new StringBuilder();
            _sqlStr.Append(@"Insert Into JobReminder ( 
                               GroupsCode   --角色代碼
                              ,ActionCode   --功能代碼
                              ,ActionName   --功能
                              ,WorkStart    --工作區間(起)
                              ,WorkEnd      --工作區間(尾)
                              ,WorkName     --工作說明
                            )
                            Values(
                               @GroupsCode
                              ,@ActionCode   
                              ,@ActionName        
                              ,@WorkStart
                              ,@WorkEnd
                              ,@WorkName 
                            )
                ");

            _sqlParams = new Dapper.DynamicParameters();
            _sqlParams.Add("GroupsCode", model.GroupsCode);
            _sqlParams.Add("ActionCode", model.ActionCode);
            _sqlParams.Add("ActionName", model.ActionName);
            _sqlParams.Add("WorkStart", model.WorkStart);
            _sqlParams.Add("WorkEnd", model.WorkEnd);
            _sqlParams.Add("WorkName", model.WorkName);

            try {
                using (var cn = new SqlConnection(_dbConnPPP))//連接資料庫
                {
                    cn.Open();
                    var ExecResult = cn.Execute(_sqlStr.ToString(), _sqlParams);
                    return null;
                }
            }
            catch (Exception ex) {
                if (ex.Message.IndexOf("插入重複的索引鍵") != -1)    // 若插入重複的主鍵值(Primary Key) 
                {
                    string exmessage = "[角色]" + model.GroupsCode + "，[功能]" + model.ActionName.ToString() + "，已經新增過了!!";
                    return exmessage;
                }
                throw ex;
            }
        }
        #endregion

        #region 將設定清單建立在當月工作項目 JudgeInsert()
        /// <summary>
        /// 將設定清單建立在當月工作項目
        /// </summary>
        /// <param name="items"></param>
        /// <param name="YM"></param>
        /// <param name="GroupsCode"></param>
        /// <returns></returns>
        internal int JudgeInsert(JobReminderViewModel items, string YM, string GroupsCode) {
            //組立SQL字串並連接資料庫
            #region 宣告
            StringBuilder _sqlStr = new StringBuilder();
            var dynamicParametersList = new List<DynamicParameters>();
            #endregion

            #region 流程
            _sqlStr.Append(@"IF NOT EXISTS(select ActionCode  from JobReminderWork where GroupsCode=@GroupsCode and ActionCode = @ActionCode)  
                             Insert Into JobReminderWork ( 
                                GroupsCode   --角色代碼
                               ,ActionCode   --功能代碼
                               ,YM           --年月
                             )
                             Values(
                                @GroupsCode
                               ,@ActionCode   
                               ,@YM  
                             )
                 ");

            foreach (SearchItemViewModel model in items.data) {
                _sqlParams = new Dapper.DynamicParameters();
                _sqlParams.Add("GroupsCode", model.GroupsCode);
                _sqlParams.Add("ActionCode", model.ActionCode);
                _sqlParams.Add("YM", YM);
                dynamicParametersList.Add(_sqlParams);
            }
            #endregion

            #region 執行 & 結果
            try {
                using (var cn = new SqlConnection(_dbConnPPP))//連接資料庫
                {
                    cn.Open();
                    int ExecResult = cn.Execute(_sqlStr.ToString(), dynamicParametersList);
                    return ExecResult;
                }
            }
            catch (Exception ex) {
                throw ex;
            }
            #endregion
        }
        #endregion

        #region 工作提醒設定-修改各年月項目 Update()
        internal int Update(SearchItemViewModel model) {
            //組立SQL字串並連接資料庫
            //每個月固定做的事情,不可調整
            //只有年月跟執行狀態、通知狀態可以不一樣
            StringBuilder _sqlStr = new StringBuilder();
            _sqlStr.Append(@"  Update JobReminder
                               SET WorkName = @WorkName,
	                               WorkStart = @WorkStart,
	                               WorkEnd = @WorkEnd
                               Where 1=1 AND GroupsCode = @GroupsCode AND ActionCode = @ActionCode
                ");

            _sqlParams = new Dapper.DynamicParameters();
            _sqlParams.Add("GroupsCode", model.GroupsCode);
            _sqlParams.Add("ActionCode", model.ActionCode);
            _sqlParams.Add("WorkStart", model.WorkStart);
            _sqlParams.Add("WorkEnd", model.WorkEnd);
            _sqlParams.Add("WorkName", model.WorkName);

            try {
                using (var cn = new SqlConnection(_dbConnPPP))//連接資料庫
                {
                    cn.Open();
                    var ExecResult = cn.Execute(_sqlStr.ToString(), _sqlParams);
                    return ExecResult;
                }
            }
            catch (Exception ex) {
                throw ex;
            }
        }
        #endregion

        #region 查詢某群組設定清單 QueryJobReminder()
        public JobReminderViewModel QueryJobReminder(string GroupsCode) {
            //組立SQL字串並連接資料庫
            #region 參數告宣
            JobReminderViewModel result = new JobReminderViewModel();
            #endregion

            #region 流程
            //查詢SQL
            StringBuilder _sqlStr = new StringBuilder();
            //查詢條件SQL
            StringBuilder _sqlStrWhere = new StringBuilder();
            //總筆數
            StringBuilder _sqlCountStr = new StringBuilder();
            //_sqlCountStr.Append("select count(1) from SigningRecord WHERE 1 = 1 and NoDelete = 1");
            _sqlStr.Append(@"  SELECT GroupsCode
                                      ,ActionCode
                                      ,ActionName
                                      ,WorkName
                                      ,WorkStart
                                      ,WorkEnd
                                      ,IsOn
                                  FROM JobReminder WHERE 1 = 1 AND IsOn = 1 ");
            _sqlParams = new Dapper.DynamicParameters();
            _sqlStrWhere.Append(" AND GroupsCode = @GroupsCode ");
            _sqlParams.Add("GroupsCode", GroupsCode);

            _sqlStr.Append(_sqlStrWhere);

            result.data = new List<SearchItemViewModel>();
            //result.draw = (searchInfo.sEcho + 1);
            using (var cn = new SqlConnection(_dbConnPPP)) //連接資料庫
            {
                cn.Open();

                result.data = cn.Query<SearchItemViewModel>(_sqlStr.ToString(), _sqlParams).ToList();
                //result.recordsTotal = cn.Query<int>(_sqlCountStr.ToString(), _sqlParams).First();
                //result.recordsFiltered = result.recordsTotal;
            }
            return result;
            #endregion
        }
        #endregion

        #region 搜尋年月(下拉清單) GroupByYM()
        public JobReminderViewModel GroupByYM(YMQueryViewModel model) {
            //組立SQL字串並連接資料庫
            #region 參數告宣
            JobReminderViewModel result = new JobReminderViewModel();
            #endregion

            #region 流程
            //查詢SQL
            StringBuilder _sqlStr = new StringBuilder();
            //查詢條件SQL
            StringBuilder _sqlParamStr = new StringBuilder();
            //總筆數
            StringBuilder _sqlCountStr = new StringBuilder();
            //_sqlCountStr.Append("select count(1) from SigningRecord WHERE 1 = 1 and NoDelete = 1");
            _sqlStr.Append(@"    SELECT YM
                                 FROM JobReminderWork
                                 WHERE 1=1 ");
            _sqlParams = new Dapper.DynamicParameters();
            _sqlParamStr.Append(" AND GroupsCode = @GroupsCode ");
            _sqlParams.Add("GroupsCode", model.GroupsCode);

            #region 條件、排序(起)
            _sqlStr.Append(_sqlParamStr);
            _sqlStr.Append(@"    GROUP BY YM
                                 ORDER BY YM ");
            //_sqlCountStr.Append(_sqlParamStr);
            //_sqlStr.Append(" ORDER BY " + (searchInfo.iSortCol_0 + 1).ToString() + " " + searchInfo.sSortDir_0 + " OFFSET @iDisplayStart ROWS  FETCH NEXT @iDisplayLength ROWS ONLY ");
            //_sqlParams.Add("iDisplayStart", searchInfo.iDisplayStart);
            //_sqlParams.Add("iDisplayLength", searchInfo.iDisplayLength);
            #endregion 條件、排序(迄)

            result.data = new List<SearchItemViewModel>();
            //result.draw = (searchInfo.sEcho + 1);
            using (var cn = new SqlConnection(_dbConnPPP)) //連接資料庫
            {
                cn.Open();

                result.data = cn.Query<SearchItemViewModel>(_sqlStr.ToString(), _sqlParams).ToList();
                //result.recordsTotal = cn.Query<int>(_sqlCountStr.ToString(), _sqlParams).First();
                result.recordsFiltered = result.recordsTotal;
            }
            return result;
            #endregion
        }
        #endregion

        #region 查詢 QuerySearchList()
        public QueryTableViewModel QuerySearchList(YMQueryViewModel model, string YM) {
            //組立SQL字串並連接資料庫
            #region 參數告宣
            QueryTableViewModel result = new QueryTableViewModel();
            #endregion

            #region 流程
            //查詢SQL
            StringBuilder _sqlStr = new StringBuilder();
            //查詢條件SQL
            StringBuilder _sqlParamStr = new StringBuilder();
            //總筆數
            StringBuilder _sqlCountStr = new StringBuilder();
            string _YYY = model.YM.Substring(0, 3);
            string _MM = model.YM.Substring(3, 2);
            string _YYYMM = _YYY + '-' + _MM;
            //_sqlCountStr.Append("select count(1) from SigningRecord WHERE 1 = 1 and NoDelete = 1");
            _sqlStr.Append(@"  SELECT row_number() OVER(PARTITION BY a.GroupsCode ORDER BY a.GroupsCode) As Seq,
                                      a.GroupsCode,a.ActionCode,a.ActionName,a.WorkName,
									  @YYYMM + '-' + 
									  CASE WHEN LEN(a.WorkStart) <= 1 THEN '0' + CAST(a.WorkStart AS varchar(1))
										   ELSE CAST(a.WorkStart AS varchar(2)) END as WorkStart,
									  @YYYMM + '-' + 
									  CASE WHEN LEN(a.WorkEnd) <= 1 THEN '0' + CAST(a.WorkEnd AS varchar(1))
										   ELSE CAST(a.WorkEnd AS varchar(2)) END as WorkEnd,
                                      a.IsOn,b.IsRun,b.IsNotice
                               FROM JobReminder a
                               INNER JOIN JobReminderWork b on a.GroupsCode = b.GroupsCode and a.ActionCode = b.ActionCode
                               WHERE 1=1 ");
            _sqlParams = new Dapper.DynamicParameters();
            _sqlParams.Add("YYYMM", _YYYMM); //組民國年
            _sqlParamStr.Append(" AND a.GroupsCode = @GroupsCode ");
            _sqlParams.Add("GroupsCode", model.GroupsCode);
            _sqlParamStr.Append(" AND b.YM = @YM ");
            _sqlParams.Add("YM", model.YM);

            //開關功能: 1是開
            _sqlParamStr.Append(" AND a.IsOn = @IsOn ");
            _sqlParams.Add("IsOn", 1);

            if (model.YM == YM)//查詢年月是否等於當月? , 是,則遮蔽已刪除項目
                {
                _sqlParamStr.Append(" AND a.IsDel = @IsDel ");
                _sqlParams.Add("IsDel", 0);
            }


            #region 條件、排序(起)
            _sqlStr.Append(_sqlParamStr);
            _sqlStr.Append(" ORDER BY a.ActionCode ");
            //_sqlCountStr.Append(_sqlParamStr);
            //_sqlStr.Append(" ORDER BY " + (searchInfo.iSortCol_0 + 1).ToString() + " " + searchInfo.sSortDir_0 + " OFFSET @iDisplayStart ROWS  FETCH NEXT @iDisplayLength ROWS ONLY ");
            //_sqlParams.Add("iDisplayStart", searchInfo.iDisplayStart);
            //_sqlParams.Add("iDisplayLength", searchInfo.iDisplayLength);
            #endregion 條件、排序(迄)

            result.data = new List<QueryItemViewModel>();
            //result.draw = (searchInfo.sEcho + 1);
            using (var cn = new SqlConnection(_dbConnPPP)) //連接資料庫
            {
                cn.Open();

                result.data = cn.Query<QueryItemViewModel>(_sqlStr.ToString(), _sqlParams).ToList();
                //result.recordsTotal = cn.Query<int>(_sqlCountStr.ToString(), _sqlParams).First();
                result.recordsFiltered = result.recordsTotal;
            }
            return result;
            #endregion
        }
        #endregion

        #region 1.判斷是否發mail提醒 WhoIsSendMail()
        /// <summary>
        /// 判斷是否發mail提醒 WhoIsSendMail()
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public QueryTableViewModel WhoIsSendMail(YMQueryViewModel model) {
            //組立SQL字串並連接資料庫
            #region 參數告宣
            QueryTableViewModel result = new QueryTableViewModel();
            #endregion

            #region 流程
            //查詢SQL
            StringBuilder _sqlStr = new StringBuilder();
            //查詢條件SQL
            StringBuilder _sqlParamStr = new StringBuilder();
            //總筆數
            StringBuilder _sqlCountStr = new StringBuilder();

            _sqlStr.Append(@"  SELECT a.GroupsCode
                                     ,a.ActionCode
                                     ,a.ActionName
                                     ,a.WorkName
                                     ,a.WorkStart
                                     ,a.WorkEnd
                                 FROM JobReminder a
                                 LEFT JOIN JobReminderWork b on a.GroupsCode = b.GroupsCode and a.ActionCode = b.ActionCode
                                 WHERE day(getdate()) > a.WorkEnd 
		                                 AND b.IsRun = '0'
		                                 AND b.IsNotice = '0'
                                         AND a.IsOn = '1' 
                                ");
            //model.GroupsCode 群組代碼
            //modle.YM 現在時間YYYMM
            _sqlParams = new Dapper.DynamicParameters();
            //_sqlParamStr.Append(" AND b.GroupsCode = @GroupsCode ");
            //_sqlParams.Add("GroupsCode", model.GroupsCode);
            _sqlParamStr.Append(" AND b.YM = @YM ");
            _sqlParams.Add("YM", model.YM);

            #region 條件、排序(起)
            _sqlStr.Append(_sqlParamStr);
            _sqlStr.Append(" ORDER BY a.ActionCode ");
            #endregion 條件、排序(迄)

            result.data = new List<QueryItemViewModel>();
            using (var cn = new SqlConnection(_dbConnPPP)) //連接資料庫
            {
                cn.Open();
                result.data = cn.Query<QueryItemViewModel>(_sqlStr.ToString(), _sqlParams).ToList();
                result.recordsFiltered = result.recordsTotal;
            }
            return result;
            #endregion
        }
        #endregion

        #region 2.mail通知狀態異動(固定改已通知) IsNoticeModify()
        /// <summary>
        /// mail通知狀態異動(固定改已通知)
        /// </summary>
        /// <param name="GroupsCode">角色</param>
        /// <param name="ActionCode">功能</param>
        /// <returns></returns>
        public int IsNoticeModify(ModifyViewModel model) {
            //組立SQL字串並連接資料庫
            #region 參數告宣
            QueryTableViewModel result = new QueryTableViewModel();
            #endregion

            #region 流程
            //查詢SQL
            StringBuilder _sqlStr = new StringBuilder();
            //查詢條件SQL
            StringBuilder _sqlStrWhere = new StringBuilder();
            //總筆數
            StringBuilder _sqlCountStr = new StringBuilder();
            _sqlStr.Append(@"     Update JobReminderWork
								  set IsNotice = 1
								  where 1=1 ");
            _sqlParams = new Dapper.DynamicParameters();
            _sqlStrWhere.Append(" AND GroupsCode = @GroupsCode ");
            _sqlParams.Add("GroupsCode", model.GroupsCode);
            _sqlStrWhere.Append(" AND ActionCode = @ActionCode ");
            _sqlParams.Add("ActionCode", model.ActionCode);
            _sqlStrWhere.Append(" AND YM = @YM ");
            _sqlParams.Add("YM", model.YM);
            _sqlStr.Append(_sqlStrWhere);

            result.data = new List<QueryItemViewModel>();
            using (var cn = new SqlConnection(_dbConnPPP)) //連接資料庫
            {
                cn.Open();

                var ExecResult = cn.Execute(_sqlStr.ToString(), _sqlParams);
                return ExecResult;
            }
            #endregion
        }
        #endregion

        #region 查詢設定項目清單 QuerySetting()
        public QueryTableViewModel QuerySetting(string GroupsCode) {
            //組立SQL字串並連接資料庫
            #region 參數告宣
            QueryTableViewModel result = new QueryTableViewModel();
            #endregion

            #region 流程
            //查詢SQL
            StringBuilder _sqlStr = new StringBuilder();
            //查詢條件SQL
            StringBuilder _sqlStrWhere = new StringBuilder();
            //總筆數
            StringBuilder _sqlCountStr = new StringBuilder();
            _sqlStr.Append(@"   SELECT row_number() OVER(PARTITION BY GroupsCode ORDER BY GroupsCode) As Seq,
                                      GroupsCode,
                                      ActionCode,
                                      ActionName,
                                      WorkName,
									  convert(varchar(7), getdate(), 126) + '-' + 
									  CASE WHEN LEN(WorkStart) <= 1 THEN '0' + CAST(WorkStart AS varchar(1))
										   ELSE CAST(WorkStart AS varchar(2)) END as WorkStart,
									  convert(varchar(7), getdate(), 126) + '-' + 
									  CASE WHEN LEN(WorkEnd) <= 1 THEN '0' + CAST(WorkEnd AS varchar(1))
										   ELSE CAST(WorkEnd AS varchar(2)) END as WorkEnd,
                                      IsOn
                                FROM JobReminder WHERE 1 = 1 ");
            _sqlParams = new Dapper.DynamicParameters();
            _sqlStrWhere.Append(" AND GroupsCode = @GroupsCode ");
            _sqlParams.Add("GroupsCode", GroupsCode);

            _sqlStr.Append(_sqlStrWhere);

            result.data = new List<QueryItemViewModel>();
            using (var cn = new SqlConnection(_dbConnPPP)) //連接資料庫
            {
                cn.Open();

                result.data = cn.Query<QueryItemViewModel>(_sqlStr.ToString(), _sqlParams).ToList();
            }
            return result;
            #endregion
        }
        #endregion

        #region 工作功能開關切換 isOnoff()
        /// <summary>
        /// 工作功能開關切換
        /// </summary>
        /// <param name="GroupsCode">角色</param>
        /// <param name="ActionCode">功能</param>
        /// <returns></returns>
        public int isOnoff(string GroupsCode, string ActionCode, string IsOn) {
            //組立SQL字串並連接資料庫
            #region 參數告宣
            QueryTableViewModel result = new QueryTableViewModel();
            #endregion

            #region 流程
            //查詢SQL
            StringBuilder _sqlStr = new StringBuilder();
            //查詢條件SQL
            StringBuilder _sqlStrWhere = new StringBuilder();
            //總筆數
            StringBuilder _sqlCountStr = new StringBuilder();
            _sqlStr.Append(@"     Update JobReminder
								  set IsOn = @IsOn
								  where 1=1 ");
            _sqlParams = new Dapper.DynamicParameters();
            if (IsOn == "1")
                _sqlParams.Add("IsOn", 0);
            else
                _sqlParams.Add("IsOn", 1);
            _sqlStrWhere.Append(" AND GroupsCode = @GroupsCode ");
            _sqlParams.Add("GroupsCode", GroupsCode);
            _sqlStrWhere.Append(" AND ActionCode = @ActionCode ");
            _sqlParams.Add("ActionCode", ActionCode);
            _sqlStr.Append(_sqlStrWhere);

            result.data = new List<QueryItemViewModel>();
            using (var cn = new SqlConnection(_dbConnPPP)) //連接資料庫
            {
                cn.Open();

                var ExecResult = cn.Execute(_sqlStr.ToString(), _sqlParams);
                return ExecResult;
            }
            #endregion
        }
        #endregion

        #region 執行狀態異動(固定改已執行) isRunModify()
        /// <summary>
        /// 執行狀態異動(固定改已執行)
        /// </summary>
        /// <param name="GroupsCode">角色</param>
        /// <param name="ActionCode">功能</param>
        /// <param name="YM">年月</param>
        /// <returns></returns>
        public void isRunModify(ModifyViewModel model, ref SqlConnection conn, ref SqlTransaction trans) {
            //組立SQL字串並連接資料庫
            #region 參數告宣
            QueryTableViewModel result = new QueryTableViewModel();
            #endregion

            #region 流程
            //查詢SQL
            StringBuilder _sqlStr = new StringBuilder();
            //查詢條件SQL
            StringBuilder _sqlStrWhere = new StringBuilder();
            //總筆數
            StringBuilder _sqlCountStr = new StringBuilder();
            _sqlStr.Append(@"     Update JobReminderWork
								  set IsRun = 1
								  where 1=1 ");
            _sqlParams = new Dapper.DynamicParameters();
            _sqlStrWhere.Append(" AND GroupsCode = @GroupsCode ");
            _sqlParams.Add("GroupsCode", model.GroupsCode);
            _sqlStrWhere.Append(" AND ActionCode = @ActionCode ");
            _sqlParams.Add("ActionCode", model.ActionCode);
            _sqlStrWhere.Append(" AND YM = @YM ");
            _sqlParams.Add("YM", model.YM);
            _sqlStr.Append(_sqlStrWhere);

            result.data = new List<QueryItemViewModel>();
            try {
                var ExecResult = conn.Execute(_sqlStr.ToString(), _sqlParams, trans);
                TransactionCommit(trans);
            }
            catch (Exception ex) {
                TransactionRollback(trans);
                GetCloseConnection(conn);
                throw ex;
            }
            finally {
                GetCloseConnection(conn);
            }
            #endregion
        }
        #endregion

        #region 撈取要修改的資料 QueryUpdateData()
        public SearchItemViewModel QueryUpdateData(string GroupsCode, string ActionCode) {
            //組立SQL字串並連接資料庫
            #region 參數告宣
            JobReminderViewModel result = new JobReminderViewModel();
            SearchItemViewModel reslutOnedata = new SearchItemViewModel();
            #endregion

            #region 流程
            //查詢SQL
            StringBuilder _sqlStr = new StringBuilder();
            //查詢條件SQL
            StringBuilder _sqlStrWhere = new StringBuilder();
            //總筆數
            StringBuilder _sqlCountStr = new StringBuilder();
            _sqlStr.Append(@"   SELECT GroupsCode,
                                      ActionCode,
                                      ActionName,
                                      WorkName,
									  WorkStart,
									  WorkEnd,
                                      IsOn
                                FROM JobReminder WHERE 1 = 1 ");
            _sqlParams = new Dapper.DynamicParameters();
            _sqlStrWhere.Append(" AND GroupsCode = @GroupsCode ");
            _sqlParams.Add("GroupsCode", GroupsCode);
            _sqlStrWhere.Append(" AND ActionCode = @ActionCode ");
            _sqlParams.Add("ActionCode", ActionCode);
            _sqlStr.Append(_sqlStrWhere);

            result.data = new List<SearchItemViewModel>();
            using (var cn = new SqlConnection(_dbConnPPP)) //連接資料庫
            {
                cn.Open();

                result.data = cn.Query<SearchItemViewModel>(_sqlStr.ToString(), _sqlParams).ToList();
                reslutOnedata = result.data[0];
            }
            return reslutOnedata;
            #endregion
        }
        #endregion

        #region 修改資料 Updatedata()
        /// <summary>
        /// 修改資料
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        internal int Updatedata(UpdateViewModel model) {
            //組立SQL字串並連接資料庫
            //每個月固定做的事情,不可調整
            //只有年月跟執行狀態、通知狀態可以不一樣
            StringBuilder _sqlStr = new StringBuilder();
            _sqlStr.Append(@"  Update JobReminder
                               SET WorkName = @WorkName,
	                               WorkStart = @WorkStart,
	                               WorkEnd = @WorkEnd
                               Where 1=1 AND GroupsCode = @GroupsCode AND ActionCode = @ActionCode
                ");

            _sqlParams = new Dapper.DynamicParameters();
            _sqlParams.Add("GroupsCode", model.GroupsCode);
            _sqlParams.Add("ActionCode", model.ActionCode);
            _sqlParams.Add("WorkStart", model.WorkStart);
            _sqlParams.Add("WorkEnd", model.WorkEnd);
            _sqlParams.Add("WorkName", model.WorkName);

            try {
                using (var cn = new SqlConnection(_dbConnPPP))//連接資料庫
                {
                    cn.Open();
                    var ExecResult = cn.Execute(_sqlStr.ToString(), _sqlParams);
                    return ExecResult;
                }
            }
            catch (Exception ex) {
                throw ex;
            }
        }
        #endregion

        #region 修改執行狀態 UpdateIsRun()
        /// <summary>
        /// 修改資料
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        internal int UpdateIsRun(UpdateIsRunViewModel model) {
            //組立SQL字串並連接資料庫
            //每個月固定做的事情,不可調整
            //只有年月跟執行狀態、通知狀態可以不一樣
            StringBuilder _sqlStr = new StringBuilder();
            //查詢條件SQL
            StringBuilder _sqlStrWhere = new StringBuilder();
            _sqlStr.Append(@"    UPDATE JobReminderWork
                                 SET IsRun = 1
                                 WHERE 1=1 GroupsCode = @GroupsCode
                                      AND ActionCode = @ActionCode
                                      AND YM = @YM
                ");

            _sqlParams = new Dapper.DynamicParameters();
            _sqlStrWhere.Append(" AND GroupsCode = @GroupsCode ");
            _sqlParams.Add("GroupsCode", model.GroupsCode);
            _sqlStrWhere.Append(" AND ActionCode = @ActionCode ");
            _sqlParams.Add("ActionCode", model.ActionCode);
            _sqlStrWhere.Append(" AND YM = @YM ");
            _sqlParams.Add("YM", model.YM);

            _sqlStr.Append(_sqlStrWhere);
            try {
                using (var cn = new SqlConnection(_dbConnPPP))//連接資料庫
                {
                    cn.Open();
                    var ExecResult = cn.Execute(_sqlStr.ToString(), _sqlParams);
                    return ExecResult;
                }
            }
            catch (Exception ex) {
                throw ex;
            }
        }
        #endregion
    }
}