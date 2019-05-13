using Dapper;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using WaterCaseTracking.Models.Common;
using WaterCaseTracking.Models;
using System.Data;

namespace WaterCaseTracking.Dao
{
    public class KPI_ScoreDao : _BaseDao
    {
        internal void DeleteKPI_Score(DataTable from_dt, out SqlConnection conn, out SqlTransaction trans)
        {
            //組立SQL字串並連接資料庫
            StringBuilder _sqlStr = new StringBuilder();
            _sqlStr.Append(@"Delete from KPI_Score WHERE UUID = @UUID");
            _sqlParamsList = new List<DynamicParameters>();
            DataTable dt = from_dt.DefaultView.ToTable(true, new string[] { "UUID" });

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                _sqlParams = new DynamicParameters();
                _sqlParams.Add("UUID", dt.Rows[i][0]);
                _sqlParamsList.Add(_sqlParams);
            }


            conn = GetOpenConnection();
            trans = GetTransaction(conn);

            try
            {
                var ExecResult = conn.Execute(_sqlStr.ToString(), _sqlParamsList, trans);
            }
            catch (Exception ex)
            {
                TransactionRollback(trans);
                GetCloseConnection(conn);
                throw ex;
            }
        }
        internal void AddKPI_Score(DataTable dt, ref SqlConnection conn, ref SqlTransaction trans)
        {

            try
            {
                using (SqlBulkCopy sqlBC = new SqlBulkCopy(conn, SqlBulkCopyOptions.Default, trans))
                {
                    //設定一個批次量寫入多少筆資料
                    sqlBC.BatchSize = 1000;
                    //設定要寫入的資料庫
                    sqlBC.DestinationTableName = "dbo.KPI_Score";

                    //對應資料行
                    sqlBC.ColumnMappings.Add("UUID", "UUID");
                    sqlBC.ColumnMappings.Add("Branch_No", "Branch_No");
                    sqlBC.ColumnMappings.Add("YEART", "YEART");
                    sqlBC.ColumnMappings.Add("MONTHS", "MONTHS");
                    sqlBC.ColumnMappings.Add("SCORE", "SCORE");
                    sqlBC.ColumnMappings.Add("MODIFY_USER", "MODIFY_USER");

                    sqlBC.WriteToServer(dt);
                    TransactionCommit(trans);
                }
            }
            catch (Exception ex)
            {
                TransactionRollback(trans);
                GetCloseConnection(conn);
                throw ex;
            }
            finally
            {
                GetCloseConnection(conn);
            }
        }

        internal void DeleteKPI_ScoreFromKPIDis(string GROUPS, int MONTH = 0)
        {
            //組立SQL字串並連接資料庫
            StringBuilder _sqlStr = new StringBuilder();
            _sqlStr.Append(@"Delete from KPI_Score WHERE UUID in (select D.UUID from KPI_Distribution D INNER JOIN KPI_item I ON D.UUID = I.UUID WHERE I.GROUPS = @GROUPS AND D.YEART = @YEART  ");

            _sqlParams = new Dapper.DynamicParameters();
            _sqlParams.Add("YEART", DateTime.Now.Year - 1911);
            if (MONTH > 0)
            {
                _sqlStr.Append(" AND MONTH = @MONTH ");
                _sqlParams.Add("MONTH", MONTH);
            }
            _sqlParams.Add("GROUPS", GROUPS);

            _sqlStr.Append(")");
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
        #region 新增至Temp
        internal void DeleteTempKPI_Score(DataTable from_dt, string MONTHS, out SqlConnection conn, out SqlTransaction trans)
        {
            //組立SQL字串並連接資料庫
            StringBuilder _sqlStr = new StringBuilder();
            _sqlStr.Append(@"Delete from TempKPI_Score WHERE YEART = @YEART AND 1 = 2 ");

            _sqlParams = new DynamicParameters();
            _sqlParams.Add("YEART", DateTime.Now.Year - 1911);
            if (string.IsNullOrEmpty(MONTHS))
            {
                _sqlStr.Append(" AND MONTHS = @MONTHS ");
                _sqlParams.Add("MONTHS", MONTHS);
            }


            conn = GetOpenConnection();
            trans = GetTransaction(conn);

            try
            {
                var ExecResult = conn.Execute(_sqlStr.ToString(), _sqlParams, trans);
            }
            catch (Exception ex)
            {
                TransactionRollback(trans);
                GetCloseConnection(conn);
                throw ex;
            }
        }
        internal void AddTempKPI_Score(DataTable dt, ref SqlConnection conn, ref SqlTransaction trans)
        {

            try
            {
                using (SqlBulkCopy sqlBC = new SqlBulkCopy(conn, SqlBulkCopyOptions.Default, trans))
                {
                    //設定一個批次量寫入多少筆資料
                    sqlBC.BatchSize = 1000;
                    //設定要寫入的資料庫
                    sqlBC.DestinationTableName = "dbo.TempKPI_Score";

                    //對應資料行
                    sqlBC.ColumnMappings.Add("ApplyID", "ApplyID");
                    sqlBC.ColumnMappings.Add("UUID", "UUID");
                    sqlBC.ColumnMappings.Add("Branch_No", "Branch_No");
                    sqlBC.ColumnMappings.Add("YEART", "YEART");
                    sqlBC.ColumnMappings.Add("MONTHS", "MONTHS");
                    sqlBC.ColumnMappings.Add("SCORE", "SCORE");
                    sqlBC.ColumnMappings.Add("MODIFY_USER", "MODIFY_USER");

                    sqlBC.WriteToServer(dt);
                    TransactionCommit(trans);
                }
            }
            catch (Exception ex)
            {
                TransactionRollback(trans);
                GetCloseConnection(conn);
                throw ex;
            }
            finally
            {
                GetCloseConnection(conn);
            }
        }
        #endregion
        #region 簽核通過Temp轉正式
        internal void ApplyDeleteKPI_Score(string ApplyID, ref SqlConnection conn, ref SqlTransaction trans)
        {
            //組立SQL字串並連接資料庫
            StringBuilder _sqlStr = new StringBuilder();
            _sqlStr.Append(@"Delete KPI_Score from KPI_Score S INNER JOIN TempKPI_Score TS on S.YEART = TS.YEART AND TS.ApplyID = @ApplyID
            AND  1 = CASE WHEN TS.MONTHS IS NULL THEN 1 ELSE TS.MONTHS END ");

            _sqlParams = new DynamicParameters();
            _sqlParams.Add("ApplyID", ApplyID);

            try {
                var ExecResult = conn.Execute(_sqlStr.ToString(), _sqlParams, trans);
            }
            catch (Exception ex) {
                TransactionRollback(trans);
                GetCloseConnection(conn);
                throw ex;
            }
        }
        internal void ApplyAddKPI_Score(string ApplyID, ref SqlConnection conn, ref SqlTransaction trans)
        {
            //組立SQL字串並連接資料庫
            StringBuilder _sqlStr = new StringBuilder();
            _sqlStr.Append(@" insert into KPI_Score 
                                    SELECT UUID
                                    ,Branch_No
                                    ,YEART
                                    ,MONTHS
                                    ,SCORE
                                    ,MODIFY_USER
                                    ,MODIFY_DATE
                                    FROM TempKPI_Score
                                    WHERE  ApplyID = @ApplyID ");
            _sqlParams = new Dapper.DynamicParameters();
            _sqlParams.Add("ApplyID", ApplyID);
            try {
                var ExecResult = conn.Execute(_sqlStr.ToString(), _sqlParams, trans);
            }
            catch (Exception ex) {
                TransactionRollback(trans);
                GetCloseConnection(conn);
                throw ex;
            }
        }

        internal void ApplyDeleteTempKPI_Score(string ApplyID, ref SqlConnection conn, ref SqlTransaction trans)
        {
            //組立SQL字串並連接資料庫
            StringBuilder _sqlStr = new StringBuilder();
            _sqlStr.Append(@" Delete From TempKPI_Score 
                                    WHERE  ApplyID = @ApplyID ");
            _sqlParams = new Dapper.DynamicParameters();
            _sqlParams.Add("ApplyID", ApplyID);
            try
            {
                var ExecResult = conn.Execute(_sqlStr.ToString(), _sqlParams, trans);
            }
            catch (Exception ex)
            {
                TransactionRollback(trans);
                GetCloseConnection(conn);
                throw ex;
            }
        }
        #endregion
    }
}