using Dapper;
using WaterCaseTracking.Models;
using WaterCaseTracking.Models.ViewModels.KPI_Busines;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;

namespace WaterCaseTracking.Dao
{
    public class KPI_BusinesDao : _BaseDao
    {
        #region 取得主項目ID GetItemID()
        public SearchListViewModel GetItemID(SearchInfoViewModel searchInfo)
        {
            //組立SQL字串並連接資料庫
            #region 參數告宣
            SearchListViewModel result = new SearchListViewModel();
            #endregion

            #region 流程

            StringBuilder _sqlStr = new StringBuilder();

            _sqlStr.Append("SELECT convert(varchar(40),UUID) as UUID, GROUPS, SUBJECT, convert(varchar(40),UPID) as UPID, LEVELS " +
                "FROM KPI_item " +
                "WHERE LEVELS = @levels and GROUPS = @group and SUBJECT = @subject");

            _sqlParams = new Dapper.DynamicParameters();
            _sqlParams.Add("levels", searchInfo.LEVELS);
            _sqlParams.Add("group", searchInfo.GROUPS);
            _sqlParams.Add("subject", searchInfo.SUBJECT);

            using (var cn = new SqlConnection(_dbConnPPP)) //連接資料庫
            {
                cn.Open();
                result.data = cn.Query<SearchItemViewModel>(_sqlStr.ToString(), _sqlParams).ToList();
            }
            return result;
            #endregion
        }
        #endregion

        #region 查詢配分項目 GetItemDisbution()
        public SearchListViewModel GetItemDisbution(SearchInfoViewModel searchInfo)
        {
            //組立SQL字串並連接資料庫
            #region 參數告宣
            SearchListViewModel result = new SearchListViewModel();
            DateTime dt = DateTime.Now;
            #endregion

            #region 流程
            StringBuilder _sqlStr = new StringBuilder();

            _sqlStr.Append("SELECT convert(varchar(40),UUID) as UUID, YEART, DISBUTION, MODIFY_USER, MODIFY_DATE " +
                "FROM KPI_Distribution " +
                "WHERE UUID = @UUID and YEART = @YEART ");

            _sqlParams = new Dapper.DynamicParameters();
            _sqlParams.Add("UUID", searchInfo.UUID);
            _sqlParams.Add("YEART", (dt.Year - 1911));

            using (var cn = new SqlConnection(_dbConnPPP)) //連接資料庫
            {
                cn.Open();
                result.data = cn.Query<SearchItemViewModel>(_sqlStr.ToString(), _sqlParams).ToList();
            }
            return result;
            #endregion
        }
        #endregion

        #region 插入項目配比 InsertItem()
        public SearchListViewModel InsertItem(SearchItem searchInfo)
        {
            //組立SQL字串並連接資料庫
            #region 參數告宣
            SearchListViewModel result = new SearchListViewModel();
            DateTime dt = DateTime.Now;
            #endregion

            #region 流程

            StringBuilder _sqlStr = new StringBuilder();

            _sqlStr.Append("INSERT INTO KPI_Distribution ( UUID, YEART, DISBUTION, MODIFY_USER, MODIFY_DATE) " +
                " VALUES " +
                "(@UUID ,@YEART ,@DISBUTION ,@MODIFY_USER ,@MODIFY_DATE )");

            _sqlParams = new Dapper.DynamicParameters();
            _sqlParams.Add("UUID", searchInfo.UUID);
            _sqlParams.Add("YEART", (dt.Year - 1911));
            _sqlParams.Add("DISBUTION", searchInfo.DISBUTION);
            _sqlParams.Add("MODIFY_USER", "admin");
            _sqlParams.Add("MODIFY_DATE", dt);

            using (var cn = new SqlConnection(_dbConnPPP)) //連接資料庫
            {
                cn.Open();
                var ExecResult = cn.Execute(_sqlStr.ToString(), _sqlParams);
            }
            return result;
            #endregion
        }
        #endregion

        #region 更新項目配比 UpDataItem()
        public SearchListViewModel UpDataItem(SearchItem searchInfo)
        {
            //組立SQL字串並連接資料庫
            #region 參數告宣
            SearchListViewModel result = new SearchListViewModel();
            DateTime dt = DateTime.Now;
            #endregion

            #region 流程
            StringBuilder _sqlStr = new StringBuilder();

            _sqlStr.Append("UPDATE KPI_Distribution " +
                "set DISBUTION = @DISBUTION, MODIFY_USER = @MODIFY_USER, MODIFY_DATE = @MODIFY_DATE " +
                "where UUID = @UUID and YEART = @YEART ");

            _sqlParams = new Dapper.DynamicParameters();
            _sqlParams.Add("UUID", searchInfo.UUID);
            _sqlParams.Add("YEART", (dt.Year - 1911));
            _sqlParams.Add("DISBUTION", searchInfo.DISBUTION);
            _sqlParams.Add("MODIFY_USER", "admin");
            _sqlParams.Add("MODIFY_DATE", dt);

            using (var cn = new SqlConnection(_dbConnPPP)) //連接資料庫
            {
                cn.Open();
                var ExecResult = cn.Execute(_sqlStr.ToString(), _sqlParams);
            }
            return result;
            #endregion
        }
        #endregion

        #region 取得主項目配分 GetItemRatio()
        public float GetItemRatio(SearchInfoViewModel searchInfo)
        {
            //組立SQL字串並連接資料庫
            #region 參數告宣
            SearchListViewModel result = new SearchListViewModel();
            #endregion

            #region 流程

            StringBuilder _sqlStr = new StringBuilder();

            _sqlStr.Append("SELECT convert(varchar(40),i.UUID) as UUID, i.GROUPS, i.SUBJECT, d.DISBUTION, convert(varchar(40),i.UPID) as UPID, i.LEVELS, d.YEART  " +
                "FROM KPI_item i " +
                "LEFT JOIN KPI_Distribution d on d.UUID = i.UUID " +
                "WHERE LEVELS = @levels and GROUPS = @groups and d.YEART = @year ");

            _sqlParams = new Dapper.DynamicParameters();
            _sqlParams.Add("levels", searchInfo.LEVELS);
            _sqlParams.Add("groups", searchInfo.GROUPS);
            _sqlParams.Add("year", searchInfo.Year);

            using (var cn = new SqlConnection(_dbConnPPP)) //連接資料庫
            {
                cn.Open();
                result.data = cn.Query<SearchItemViewModel>(_sqlStr.ToString(), _sqlParams).ToList();
                if (result.data != null && result.data.Count > 0)
                {
                    return result.data[0].DISBUTION;
                }
                else
                {
                    return 0;
                }
            }
            #endregion
        }
        #endregion

        #region 取得管理配分或扣分 GetItemManage()
        public float GetItemManage(SearchInfoViewModel searchInfo)
        {
            //組立SQL字串並連接資料庫
            #region 參數告宣
            SearchListViewModel result = new SearchListViewModel();
            #endregion

            #region 流程

            StringBuilder _sqlStr = new StringBuilder();

            _sqlStr.Append("SELECT convert(varchar(40),i.UUID) as UUID, i.GROUPS, i.SUBJECT, d.DISBUTION, convert(varchar(40),i.UPID) as UPID, i.LEVELS, d.YEART  " +
                "FROM KPI_item i " +
                "LEFT JOIN KPI_Distribution d on d.UUID = i.UUID " +
                "WHERE LEVELS = @levels and GROUPS = @groups and d.YEART = @year and i.SUBJECT = @subject ");

            _sqlParams = new Dapper.DynamicParameters();
            _sqlParams.Add("levels", searchInfo.LEVELS);
            _sqlParams.Add("groups", searchInfo.GROUPS);
            _sqlParams.Add("year", searchInfo.Year);
            _sqlParams.Add("subject", searchInfo.SUBJECT);

            using (var cn = new SqlConnection(_dbConnPPP)) //連接資料庫
            {
                cn.Open();
                result.data = cn.Query<SearchItemViewModel>(_sqlStr.ToString(), _sqlParams).ToList();
                if (result.data != null && result.data.Count > 0)
                {
                    return result.data[0].DISBUTION;
                }
                else
                {
                    return 0;
                }
            }
            #endregion
        }
        #endregion

        #region 取得主項目ID GetItemID()
        public SearchListViewModel GetItemManageID(SearchInfoViewModel searchInfo)
        {
            //組立SQL字串並連接資料庫
            #region 參數告宣
            SearchListViewModel result = new SearchListViewModel();
            #endregion

            #region 流程

            StringBuilder _sqlStr = new StringBuilder();

            _sqlStr.Append("SELECT convert(varchar(40),UUID) as UUID, GROUPS, SUBJECT, convert(varchar(40),UPID) as UPID, LEVELS " +
                "FROM KPI_item " +
                "WHERE LEVELS = @levels and GROUPS = @GROUPS and SUBJECT = @subject ");

            _sqlParams = new Dapper.DynamicParameters();
            _sqlParams.Add("levels", searchInfo.LEVELS);
            _sqlParams.Add("GROUPS", searchInfo.GROUPS);
            _sqlParams.Add("subject", searchInfo.SUBJECT);

            using (var cn = new SqlConnection(_dbConnPPP)) //連接資料庫
            {
                cn.Open();
                result.data = cn.Query<SearchItemViewModel>(_sqlStr.ToString(), _sqlParams).ToList();
            }
            return result;
            #endregion
        }
        #endregion
    }
}