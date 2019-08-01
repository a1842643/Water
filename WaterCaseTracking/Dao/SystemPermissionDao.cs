using Dapper;
using WaterCaseTracking.Models;
using WaterCaseTracking.Models.ViewModels;
using WaterCaseTracking.Models.ViewModels.SystemPermission;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;

namespace WaterCaseTracking.Dao
{
    public class SystemPermissionDao : _BaseDao
    {

        #region 設定角色權限資料 SetPermission()
        public string SetPermission(SearchInfoViewModel searchInfo)
        {
            //組立SQL字串並連接資料庫
            #region 參數告宣
            DateTime Time = DateTime.Now;
            string result = string.Empty;
            #endregion

            #region 流程

            StringBuilder _sqlStr = new StringBuilder();
            _sqlStr.Append("UPDATE PermissionProg " +
                "SET PROID = @PROID, " +
                "MODIFY_DATE = @MODIFY_DATE, MODIFY_USER = @MODIFY_USER " +
                "WHERE Programs = @Programs");

            _sqlParams = new Dapper.DynamicParameters();

            searchInfo.MODIFY_DATE = Time;

            //if (testTry)
            //{
            //    searchInfo.MODIFY_USER = "TryUser";
            //}

            _sqlParams.Add("Programs", searchInfo.ID);
            _sqlParams.Add("PROID", searchInfo.PROID);
            _sqlParams.Add("MODIFY_DATE", searchInfo.MODIFY_DATE.ToString("yyyy-MM-dd HH:mm:ss"));
            _sqlParams.Add("MODIFY_USER", searchInfo.MODIFY_USER);

            using (var cn = new SqlConnection(_dbConnPPP)) //連接資料庫
            {
                cn.Open();
                var ExecResult = cn.Execute(_sqlStr.ToString(), _sqlParams);
                result = ExecResult.ToString();
            }
            return result;
            #endregion
        }
        #endregion

        #region 取得角色權限資料 GetPermission()
        public SearchListViewModel GetPermission(SearchInfoViewModel searchInfo)
        {
            //組立SQL字串並連接資料庫
            #region 參數告宣
            DateTime Time = DateTime.Now;
            SearchListViewModel result = new SearchListViewModel();
            #endregion

            #region 流程

            StringBuilder _sqlStr = new StringBuilder();
            _sqlStr.Append("SELECT ID, Programs, PROID, FORMAT(MODIFY_DATE, 'yyyy-MM-dd') AS MODIFY_DATE, MODIFY_USER " +
                "FROM PermissionProg " +
                "WHERE Programs = @Programs ");

            _sqlParams = new Dapper.DynamicParameters();
            _sqlParams.Add("Programs", searchInfo.PID);

            using (var cn = new SqlConnection(_dbConnPPP)) //連接資料庫
            {
                cn.Open();
                result.data = cn.Query<SearchItemViewModel>(_sqlStr.ToString(), _sqlParams).ToList();
            }
            return result;
            #endregion
        }
        #endregion

        #region 取得角色列表 GetPermission()
        public SearchListViewModel GetPermissionList()
        {
            //組立SQL字串並連接資料庫
            #region 參數告宣
            SearchListViewModel result = new SearchListViewModel();
            #endregion

            #region 流程

            StringBuilder _sqlStr = new StringBuilder();
            _sqlStr.Append("SELECT SID, PID, NAME FROM Permission WHERE PID != @PID ORDER BY SID ");
            //_sqlStr.Append("SELECT SID, PID, NAME FROM Permission ");

            _sqlParams = new Dapper.DynamicParameters();
            _sqlParams.Add("PID", "admin");

            using (var cn = new SqlConnection(_dbConnPPP)) //連接資料庫
            {
                cn.Open();
                result.list = cn.Query<PermissionModel>(_sqlStr.ToString(), _sqlParams).ToList();
            }
            return result;
            #endregion
        }
        #endregion

        #region 刪除角色 DelPermission()
        public string DelPermission(SearchInfoViewModel searchInfo)
        {
            //組立SQL字串並連接資料庫
            #region 參數告宣
            DateTime Time = DateTime.Now;
            string result = string.Empty;
            #endregion

            #region 流程

            StringBuilder _sqlStr = new StringBuilder();
            _sqlStr.Append("DELETE FROM Permission " +
                "WHERE PID = @PID; ");

            _sqlParams = new Dapper.DynamicParameters();
            _sqlParams.Add("PID", searchInfo.Programs);

            using (var cn = new SqlConnection(_dbConnPPP)) //連接資料庫
            {
                cn.Open();
                var ExecResult = cn.Execute(_sqlStr.ToString(), _sqlParams);
                result = ExecResult.ToString();
            }
            return result;
            #endregion
        }
        #endregion

        #region 刪除角色權限 DelPermissionProg()
        public string DelPermissionProg(SearchInfoViewModel searchInfo)
        {
            //組立SQL字串並連接資料庫
            #region 參數告宣
            DateTime Time = DateTime.Now;
            string result = string.Empty;
            #endregion

            #region 流程

            StringBuilder _sqlStr = new StringBuilder();
            _sqlStr.Append("DELETE FROM PermissionProg " +
                "WHERE Programs = @Programs; ");

            _sqlParams = new Dapper.DynamicParameters();
            _sqlParams.Add("Programs", searchInfo.Programs);

            using (var cn = new SqlConnection(_dbConnPPP)) //連接資料庫
            {
                cn.Open();
                var ExecResult = cn.Execute(_sqlStr.ToString(), _sqlParams);
                result = ExecResult.ToString();
            }
            return result;
            #endregion
        }
        #endregion

        #region 新增角色 AddPermission()
        public string AddPermission(SearchInfoViewModel searchInfo)
        {
            //組立SQL字串並連接資料庫
            #region 參數告宣
            DateTime Time = DateTime.Now;
            string result = string.Empty;
            #endregion

            #region 流程

            StringBuilder _sqlStr = new StringBuilder();
            _sqlStr.Append("INSERT INTO Permission (PID, NAME, MailAddress) " +
                "VALUES (@PID, @NAME, @MailAddress)");

            _sqlParams = new Dapper.DynamicParameters();
            _sqlParams.Add("PID", searchInfo.PID);
            _sqlParams.Add("NAME", searchInfo.NAME);
            _sqlParams.Add("MailAddress", searchInfo.MAIL);

            using (var cn = new SqlConnection(_dbConnPPP)) //連接資料庫
            {
                cn.Open();
                var ExecResult = cn.Execute(_sqlStr.ToString(), _sqlParams);
                result = ExecResult.ToString();
            }
            return result;
            #endregion
        }
        #endregion

        #region 新增角色權限 AddPermissionProg()
        public string AddPermissionProg(SearchInfoViewModel searchInfo)
        {
            //組立SQL字串並連接資料庫
            #region 參數告宣
            DateTime Time = DateTime.Now;
            string result = string.Empty;
            #endregion

            #region 流程

            StringBuilder _sqlStr = new StringBuilder();
            _sqlStr.Append("INSERT INTO PermissionProg (Programs, PROID, MODIFY_DATE, MODIFY_USER) " +
                "VALUES (@Programs, 'A,A02,D', @MODIFY_DATE, @MODIFY_USER) ");

            _sqlParams = new Dapper.DynamicParameters();

            searchInfo.MODIFY_DATE = Time;

            //if (testTry)
            //{
            //    searchInfo.MODIFY_USER = "TryUser";
            //}
            searchInfo.MODIFY_USER = "TryUser";

            _sqlParams.Add("Programs", searchInfo.PID);
            _sqlParams.Add("MODIFY_DATE", searchInfo.MODIFY_DATE.ToString("yyyy-MM-dd HH:mm:ss"));
            _sqlParams.Add("MODIFY_USER", searchInfo.MODIFY_USER);


            using (var cn = new SqlConnection(_dbConnPPP)) //連接資料庫
            {
                cn.Open();
                var ExecResult = cn.Execute(_sqlStr.ToString(), _sqlParams);
                result = ExecResult.ToString();
            }
            return result;
            #endregion
        }
        #endregion

        #region 更新角色資料 UpdataPermission()
        public string UpdataPermission(SearchInfoViewModel searchInfo)
        {
            //組立SQL字串並連接資料庫
            #region 參數告宣
            DateTime Time = DateTime.Now;
            string result = string.Empty;
            #endregion

            #region 流程

            StringBuilder _sqlStr = new StringBuilder();
            _sqlStr.Append("UPDATE Permission " +
                "SET PID = @PID, NAME = @NAME, MailAddress = @MailAddress " +
                "WHERE SID = @SID ");

            _sqlParams = new Dapper.DynamicParameters();

            _sqlParams.Add("PID", searchInfo.PID);
            _sqlParams.Add("NAME", searchInfo.NAME);
            _sqlParams.Add("SID", searchInfo.ID);
            _sqlParams.Add("MailAddress", searchInfo.MAIL);

            using (var cn = new SqlConnection(_dbConnPPP)) //連接資料庫
            {
                cn.Open();
                var ExecResult = cn.Execute(_sqlStr.ToString(), _sqlParams);
                result = ExecResult.ToString();
            }
            return result;
            #endregion
        }
        #endregion

        #region 更新角色權限資料 UpdataPermissionProg()
        public string UpdataPermissionProg(SearchInfoViewModel searchInfo)
        {
            //組立SQL字串並連接資料庫
            #region 參數告宣
            DateTime Time = DateTime.Now;
            string result = string.Empty;
            #endregion

            #region 流程

            StringBuilder _sqlStr = new StringBuilder();
            _sqlStr.Append("UPDATE PermissionProg " +
                "SET Programs = @Programs, " +
                "MODIFY_DATE = @MODIFY_DATE, MODIFY_USER = @MODIFY_USER " +
                "FROM PermissionProg, Permission " +
                "WHERE PermissionProg.Programs = Permission.PID " +
                "AND Permission.SID = @SID");

            _sqlParams = new Dapper.DynamicParameters();

            searchInfo.MODIFY_DATE = Time;

            //if (testTry)
            //{
            //    searchInfo.MODIFY_USER = "TryUser";
            //}

            _sqlParams.Add("Programs", searchInfo.PID);
            _sqlParams.Add("MODIFY_DATE", searchInfo.MODIFY_DATE.ToString("yyyy-MM-dd HH:mm:ss"));
            _sqlParams.Add("MODIFY_USER", searchInfo.MODIFY_USER);
            _sqlParams.Add("SID", searchInfo.ID);

            using (var cn = new SqlConnection(_dbConnPPP)) //連接資料庫
            {
                cn.Open();
                var ExecResult = cn.Execute(_sqlStr.ToString(), _sqlParams);
                result = ExecResult.ToString();
            }
            return result;
            #endregion
        }
        #endregion

        #region 取得角色列表 GetddlPermissionList()
        public DropDownListViewModel GetddlPermissionList(string roleId)
        {
            //組立SQL字串並連接資料庫
            #region 參數告宣
            DropDownListViewModel result = new DropDownListViewModel();
            string[] list = roleId.Split(',');
            string _sqlStrSearchInfo;
            #endregion

            #region 流程

            StringBuilder _sqlStr = new StringBuilder();
            _sqlStr.Append("SELECT PID AS 'Values', NAME AS 'Text' FROM Permission ");

            //WHERE PID IN('')
            _sqlStrSearchInfo = "WHERE PID IN (";
            string value = "";
            foreach (string str in list)
            {
                value += ",'" + str + "'";
            }
            if (value =="") { _sqlStrSearchInfo += "''"; }
            else { _sqlStrSearchInfo += value.Substring(1); }

            _sqlStrSearchInfo += ") ";

            _sqlStr.Append(_sqlStrSearchInfo);
            _sqlStr.Append("ORDER BY SID ");

            result.DropDownListLT = new List<DropDownListItem>();
            using (var cn = new SqlConnection(_dbConnPPP)) //連接資料庫
            {
                cn.Open();
                result.DropDownListLT = cn.Query<DropDownListItem>(_sqlStr.ToString()).ToList();
            }
            return result;
            #endregion
        }
        #endregion
    }
}