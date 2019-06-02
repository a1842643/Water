using Dapper;
using WaterCaseTracking.Models;
using WaterCaseTracking.Models.ViewModels.Home;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;

namespace WaterCaseTracking.Dao
{
    public class PermissionProgDao : _BaseDao
    {

        #region 取得功能列表 GetOperating()
        public SearchListViewModel GetOperating(string searchInfo)
        {
            //組立SQL字串並連接資料庫
            #region 參數告宣
            SearchListViewModel result = new SearchListViewModel();
            #endregion

            #region 流程

            StringBuilder _sqlStr = new StringBuilder();
            _sqlStr.Append(" SELECT p.PID, PP.PROID " +
                "FROM Permission p " +
                "LEFT JOIN PermissionProg PP ON p.PID = PP.Programs " +
                "WHERE p.PID = @PID");

            _sqlParams = new Dapper.DynamicParameters();

            _sqlParams.Add("PID", searchInfo);

            result.UserModel = new List<SearchUserViewModel>();

            using (var cn = new SqlConnection(_dbConnPPP)) //連接資料庫
            {
                cn.Open();
                result.UserModel = cn.Query<SearchUserViewModel>(_sqlStr.ToString(), _sqlParams).ToList();
            }
            return result;
            #endregion
        }
        #endregion
    }
}