using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Dapper;
using System.Text;
using System.Data.SqlClient;
using WaterCaseTracking.Models;

namespace WaterCaseTracking.Dao
{
    public class StepLogDao : _BaseDao
    {
        public int getIsLocked(StepLogModel stepLogModel)
        {
            //組立SQL字串並連接資料庫
            #region 參數告宣
            int LOCKED = 0;
            #endregion

            #region 流程

            StringBuilder _sqlStr = new StringBuilder();
            _sqlStr.Append(@"
                            SELECT Count(1) FROM StepLog 
                            WHERE YEART = @YEART AND MONTH = @MONTH AND GROUPS = @GROUPS
 
                            ");

            _sqlParams = new Dapper.DynamicParameters();
            _sqlParams.Add("YEART", stepLogModel.YEART);
            _sqlParams.Add("MONTH", stepLogModel.MONTH);
            _sqlParams.Add("GROUPS", stepLogModel.GROUPS);

            using (var cn = new SqlConnection(_dbConnPPP)) //連接資料庫
            {
                cn.Open();
                LOCKED = cn.ExecuteScalar<int>(_sqlStr.ToString(), _sqlParams);
            }
            return LOCKED;
            #endregion      
        }

        internal void DeleteStepLog(StepLogModel stepLogModel)
        {
            //組立SQL字串並連接資料庫
            StringBuilder _sqlStr = new StringBuilder();
            _sqlStr.Append(@"Delete from StepLog WHERE 1 = 1 ");

            _sqlParams = new Dapper.DynamicParameters();
            if (stepLogModel.YEART > 0)
            {
                _sqlStr.Append(" AND YEART = @YEART ");
                _sqlParams.Add("YEART", stepLogModel.YEART);
            }
            if (stepLogModel.MONTH > 0)
            {
                _sqlStr.Append(" AND MONTH = @MONTH ");
                _sqlParams.Add("MONTH", stepLogModel.MONTH);
            }
            if (!string.IsNullOrEmpty(stepLogModel.GROUPS));
            {
                _sqlStr.Append(" AND GROUPS = @GROUPS ");
                _sqlParams.Add("GROUPS", stepLogModel.GROUPS);
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

        internal void AddStepLog(StepLogModel stepLogModel)
        {
            //組立SQL字串並連接資料庫
            StringBuilder _sqlStr = new StringBuilder();
            _sqlStr.Append(@" INSERT INTO StepLog (
                                YEART
                               ,MONTH
                               ,GROUPS
                               ,LOCKED
                               ,ISDOWN
)
                                Values(
                                @YEART
                               ,@MONTH
                               ,@GROUPS
                               ,@LOCKED
                               ,@ISDOWN
)
");

            _sqlParams = new Dapper.DynamicParameters();
            _sqlParams.Add("YEART", stepLogModel.YEART);
            _sqlParams.Add("MONTH", stepLogModel.MONTH);
            _sqlParams.Add("GROUPS", stepLogModel.GROUPS);
            _sqlParams.Add("LOCKED", stepLogModel.LOCKED);
            _sqlParams.Add("ISDOWN", stepLogModel.ISDOWN);


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
    }
}