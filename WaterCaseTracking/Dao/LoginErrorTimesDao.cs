using Dapper;
using WaterCaseTracking.Models.ViewModels.Accounts;
using System;
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
    public class LoginErrorTimesDao : _BaseDao
    {
        #region 抓錯誤次數(起)
        internal LoginErrorTimesModel getAccountErrorTimes(string txtAccount)
        {
            //組立SQL字串並連接資料庫
            #region 參數告宣
            LoginErrorTimesModel result = new LoginErrorTimesModel();
            #endregion

            #region 流程

            StringBuilder _sqlStr = new StringBuilder();
            _sqlStr.Append(@"SELECT AccountID
                                  ,ErrorTimes
                                  ,LoginTime
                              FROM LoginErrorTimes 
                              Where AccountID = @AccountID ");
            _sqlParams = new Dapper.DynamicParameters();
            _sqlParams.Add("AccountID", txtAccount);

            using (var cn = new SqlConnection(_dbConnPPP)) //連接資料庫
            {
                cn.Open();
                result = cn.Query<LoginErrorTimesModel>(_sqlStr.ToString(), _sqlParams).FirstOrDefault();
            }
            return result;
            #endregion
        }

        #endregion 抓錯誤次數(迄)
        #region 新增錯誤次數(起)
        internal void AddAccountErrorTimes(string txtAccount, bool isSuccess)
        {
            //組立SQL字串並連接資料庫
            StringBuilder _sqlStr = new StringBuilder();
            _sqlStr.Append(@"Insert Into LoginErrorTimes ( 
                                AccountID
                                  ,ErrorTimes
                                  ,LoginTime
                            )
                            Values(
                                @AccountID
                                  ,@ErrorTimes
                                  ,GetDate() 
                            )
                ");
            _sqlParamsList = new List<DynamicParameters>();
            _sqlParams = new Dapper.DynamicParameters();
            _sqlParams.Add("AccountID", txtAccount);
            if(isSuccess)
            {
                _sqlParams.Add("ErrorTimes", 0);
            }
            else
            {
                _sqlParams.Add("ErrorTimes", 1);
            }

            try
            {
                using (var cn = new SqlConnection(_dbConnPPP))//連接資料庫
                {
                    cn.Open();
                    var ExecResult = cn.Execute(_sqlStr.ToString(), _sqlParams);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
        #region 修改錯誤次數(起)
        internal void UpdateAccountErrorTimes(LoginErrorTimesModel loginErrorTimesModel, bool isSuccess)
        {
            //組立SQL字串並連接資料庫
            StringBuilder _sqlStr = new StringBuilder();
            _sqlStr.Append(@" Update LoginErrorTimes set 
                                ErrorTimes = @ErrorTimes
                                , LoginTime = GetDate()
                                Where AccountID = @AccountID
                ");
            _sqlParamsList = new List<DynamicParameters>();
            _sqlParams = new Dapper.DynamicParameters();
            _sqlParams.Add("AccountID", loginErrorTimesModel.AccountID);
            if (isSuccess)
            {
                _sqlParams.Add("ErrorTimes", 0);
            }
            else
            {
                _sqlParams.Add("ErrorTimes", loginErrorTimesModel.ErrorTimes + 1);
            }

            try
            {
                using (var cn = new SqlConnection(_dbConnPPP))//連接資料庫
                {
                    cn.Open();
                    var ExecResult = cn.Execute(_sqlStr.ToString(), _sqlParams);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
    }
}