using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using ADO;
using Dapper;
using WaterCaseTracking.Models.ViewModels.ManagementRecordCard;

namespace WaterCaseTracking.Dao
{
    public class ManagementRecordCardDao : _BaseDao
    {

        #region 月平均
        public SearchListViewModel MonthList(SearchInfoViewModel searchInfo)
        {
            //組立SQL字串並連接資料庫
            #region 參數告宣
            SearchListViewModel result = new SearchListViewModel();
            #endregion

            #region 流程
            StringBuilder _sqlStr = new StringBuilder();
            //資料庫語法
            //_sqlStr.Append("SELECT TOP 50 " +
            //    "Branch_No AS 'A1', Branch_No AS 'A2', Branch_No AS 'A3', Branch_No AS 'A4', Branch_No AS 'A5', Branch_No AS 'A6' " +
            //    "FROM Until ");
            _sqlStr.Append("getAdmin_card_count");

            _sqlParams = new Dapper.DynamicParameters();
            _sqlParams.Add("Branch_No", searchInfo.Branch_No);
            _sqlParams.Add("YM7", (int.Parse(searchInfo.YeartMonth) + 191100).ToString());
            _sqlParams.Add("type", "admin_card");


            result.data = new List<SearchItemViewModel>();
            using (var cn = new SqlConnection(_dbConnPPP)) //連接資料庫
            {
                cn.Open();
                result.data = cn.Query<SearchItemViewModel>(_sqlStr.ToString(), _sqlParams, commandType: CommandType.StoredProcedure).ToList(); //加入參數使用此行
                //result.data = cn.Query<SearchItemViewModel>(_sqlStr.ToString()).ToList();
            }
            return result;
            #endregion
        }
        #endregion

        #region 累計平均
        public SearchListViewModel AccumulativeList(SearchInfoViewModel searchInfo)
        {
            //組立SQL字串並連接資料庫
            #region 參數告宣
            SearchListViewModel result = new SearchListViewModel();
            #endregion

            #region 流程
            StringBuilder _sqlStr = new StringBuilder();
            //資料庫語法
            //_sqlStr.Append("SELECT TOP 50 " +
            //    "Branch_No AS 'A1', Branch_No AS 'A2', Branch_No AS 'A3', Branch_No AS 'A4', Branch_No AS 'A5', Branch_No AS 'A6' " +
            //    "FROM Until ");
            _sqlStr.Append("getAdmin_card_count");

            _sqlParams = new Dapper.DynamicParameters();
            _sqlParams.Add("Branch_No", searchInfo.Branch_No);
            _sqlParams.Add("YM7", (int.Parse(searchInfo.YeartMonth) + 191100).ToString());
            _sqlParams.Add("type", "admin_card_count");


            result.data = new List<SearchItemViewModel>();
            using (var cn = new SqlConnection(_dbConnPPP)) //連接資料庫
            {
                cn.Open();
                result.data = cn.Query<SearchItemViewModel>(_sqlStr.ToString(), _sqlParams, commandType: CommandType.StoredProcedure).ToList(); //加入參數使用此行
                //result.data = cn.Query<SearchItemViewModel>(_sqlStr.ToString()).ToList();
            }
            return result;
            #endregion
        }
        #endregion

        #region 取得分行名稱
        public string BranchName(SearchInfoViewModel searchInfo)
        {
            //組立SQL字串並連接資料庫
            #region 參數告宣
            List<Until> result = new List<Until>();
            string BranchName;
            #endregion

            #region 流程

            StringBuilder _sqlStr = new StringBuilder();
            _sqlStr.Append(@"SELECT Name 
                             FROM Until 
                             WHERE Branch_No = @Branch_No");

            _sqlParams = new Dapper.DynamicParameters();
            _sqlParams.Add("Branch_No", searchInfo.Branch_No);
            
            using (var cn = new SqlConnection(_dbConnPPP)) //連接資料庫
            {
                cn.Open();
                result = cn.Query<Until>(_sqlStr.ToString(), _sqlParams).ToList();
                BranchName = result[0].Name;
            }
            return BranchName;
            #endregion
        }
        #endregion
    }
}