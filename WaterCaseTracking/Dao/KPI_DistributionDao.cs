using Dapper;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using WaterCaseTracking.Models.Common;
using WaterCaseTracking.Models;
using System.Data;
using WaterCaseTracking.Models.ViewModels;
using System.Linq;

namespace WaterCaseTracking.Dao
{
    public class KPI_DistributionDao : _BaseDao
    {
        internal void DeleteKPI_Distribution(string GROUPS, out SqlConnection conn, out SqlTransaction trans)
        {
            //組立SQL字串並連接資料庫
            StringBuilder _sqlStr = new StringBuilder();
            _sqlStr.Append(@" Delete from KPI_Distribution 
    WHERE UUID in (select item.UUID from KPI_item item 
    inner join  KPI_Distribution Dis 
			on item.UUID = dis.UUID 
    where YEART = @YEART 
		AND GROUPS = @GROUPS) AND YEART = @YEART ");
            _sqlParamsList = new List<DynamicParameters>();
            _sqlParams = new DynamicParameters();
            _sqlParams.Add("YEART", DateTime.Now.Year - 1911);
            _sqlParams.Add("GROUPS", GROUPS);


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
        internal void AddKPI_Distribution(string GROUPS, ref SqlConnection conn, ref SqlTransaction trans)
        {

            //組立SQL字串並連接資料庫
            StringBuilder _sqlStr = new StringBuilder();
            _sqlStr.Append(@" insert into KPI_Distribution 
                select  
                Dis.UUID 
                ,@NowYEART 
                ,DISBUTION 
                ,'SYSTEM' 
                ,GETDATE() 
                  FROM KPI_item item 
                inner join  KPI_Distribution Dis  
                			on item.UUID = dis.UUID 
                where YEART = @LastYEART 
                		AND GROUPS = @GROUPS ");
            _sqlParams = new DynamicParameters();
            _sqlParams.Add("NowYEART", DateTime.Now.Year - 1911);
            _sqlParams.Add("LastYEART", DateTime.Now.Year - 1912);
            _sqlParams.Add("GROUPS", GROUPS);



            try
            {
                var ExecResult = conn.Execute(_sqlStr.ToString(), _sqlParams, trans);
                TransactionCommit(trans);
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

    }
}