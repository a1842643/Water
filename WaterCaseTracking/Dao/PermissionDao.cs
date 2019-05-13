using Dapper;
using WaterCaseTracking.Models.ViewModels.AverageCount;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;

namespace WaterCaseTracking.Dao {
    public class PermissionDao :_BaseDao{
        /// <summary>
        /// 撈群組mail
        /// </summary>
        /// <param name="model"></param>
        public DataTable QueryRoleMails(string PID) {

            #region 參數告宣
            DataTable dt = new DataTable();

            //組立SQL字串並連接資料庫
            SearchListViewModel result = new SearchListViewModel();
            //查詢SQL
            StringBuilder _sqlStr = new StringBuilder();
            //查詢條件SQL
            StringBuilder _sqlParamStr = new StringBuilder();
            #endregion

            #region 流程
            _sqlStr.Append(@" SELECT MailAddress
                              FROM Permission
                              WHERE PID = @PID ");

            _sqlParams = new Dapper.DynamicParameters();
            _sqlParams.Add("PID", PID);
            using (var cn = new SqlConnection(_dbConnPPP)) //連接資料庫
            {
                cn.Open();
                dt.Load(cn.ExecuteReader(_sqlStr.ToString(), _sqlParams));
            }
            return dt;
            #endregion
        }
    }
}