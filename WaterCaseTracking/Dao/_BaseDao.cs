using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;

namespace WaterCaseTracking.Dao
{
    /// <summary>
    /// 資料庫連線底層
    /// </summary>
    public class _BaseDao
    {
        #region Properties
        protected StringBuilder _sqlStr;
        protected string _sqlOrderByStr;
        protected DynamicParameters _sqlParams;
        protected List<DynamicParameters> _sqlParamsList;
        protected SqlConnection SQLConnection;
        protected SqlTransaction SQLTransaction;
        protected bool testTry = true; 
        #region DB連線
        protected string _dbConnPPP = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
        #endregion
        #endregion
        public SqlConnection GetOpenConnection()
        {

            SQLConnection = new SqlConnection(_dbConnPPP);
            if (SQLConnection.State != ConnectionState.Open)
                SQLConnection.Open();
            return SQLConnection;
        }

        public SqlTransaction GetTransaction(SqlConnection conn)
        {
            SQLTransaction = conn.BeginTransaction();
            return SQLTransaction;
        }
        public void TransactionRollback(SqlTransaction trans)
        {
            trans.Rollback();
        }

        public void GetCloseConnection(SqlConnection conn)
        {
            conn.Close();
            conn.Dispose();
        }
        public void TransactionCommit(SqlTransaction trans)
        {
            trans.Commit();
        }
    }
}