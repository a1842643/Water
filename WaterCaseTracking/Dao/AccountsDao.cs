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
    public class AccountsDao : _BaseDao
    {
        #region 查詢 QuerySearchList()
        public SearchListViewModel QuerySearchList(SearchInfoViewModel searchInfo)
        {
            //組立SQL字串並連接資料庫
            #region 參數告宣
            SearchListViewModel result = new SearchListViewModel();
            #endregion

            #region 流程
            //查詢SQL
            StringBuilder _sqlStr = new StringBuilder();
            //查詢條件SQL
            StringBuilder _sqlParamStr = new StringBuilder();
            //總筆數
            StringBuilder _sqlCountStr = new StringBuilder();
            _sqlCountStr.Append(@"select count(1) from Accounts A 
                            Left join Sys_Code SC ON A.Role = SC.ITEM_CODE
                            WHERE 1 = 1  ");
            _sqlStr.Append(@"select 
                                AccountID                                             --帳號
                                , AccountName                                         --姓名
                                , SC.ITEM_NAME as 'RoleName'                          --角色
                                ,CreateUserName                                       --新增人員 
                                ,CreateDate                                           --新增時間
                                ,UpdateUserName                                       --修改人員
                                ,UpdateDate                                           --修改時間  
                            from Accounts A 
                            Left join Sys_Code SC ON A.Role = SC.ITEM_CODE
                            WHERE 1 = 1 
");

            _sqlParams = new Dapper.DynamicParameters();
            //姓名
            if (!string.IsNullOrEmpty(searchInfo.txtAccountName))
            {
                _sqlParamStr.Append(" and AccountName like @AccountName ");
                _sqlParams.Add("AccountName", "%" + searchInfo.txtAccountName + "%");
            }
            //角色
            if (!string.IsNullOrEmpty(searchInfo.ddlRole))
            {
                _sqlParamStr.Append(" and Role = @Role ");
                _sqlParams.Add("Role", searchInfo.ddlRole);
            }

            #region 條件、排序(起)
            _sqlStr.Append(_sqlParamStr);
            _sqlCountStr.Append(_sqlParamStr);
            _sqlStr.Append(" ORDER BY " + (searchInfo.iSortCol_0 + 1).ToString() + " " + searchInfo.sSortDir_0 + " OFFSET @iDisplayStart ROWS  FETCH NEXT @iDisplayLength ROWS ONLY ");
            _sqlParams.Add("iDisplayStart", searchInfo.iDisplayStart);
            _sqlParams.Add("iDisplayLength", searchInfo.iDisplayLength);
            #endregion 條件、排序(迄)

            result.data = new List<SearchItemViewModel>();
            result.draw = (searchInfo.sEcho + 1);
            using (var cn = new SqlConnection(_dbConnPPP)) //連接資料庫
            {
                cn.Open();
                result.data = cn.Query<SearchItemViewModel>(_sqlStr.ToString(), _sqlParams).ToList();
                result.recordsTotal = cn.Query<int>(_sqlCountStr.ToString(), _sqlParams).First();
                result.recordsFiltered = result.recordsTotal;
            }
            return result;
            #endregion
        }



        internal AccountsModel QueryAccountInfo(string AccountID, string Password)
        {
            //組立SQL字串並連接資料庫
            #region 參數告宣
            AccountsModel result = new AccountsModel();
            #endregion

            #region 流程

            StringBuilder _sqlStr = new StringBuilder();
            _sqlStr.Append(@"select 
                                AccountID                                           --帳號
                                , Password                                          --密碼
                                , AccountName                                       --帳號名稱
                                , Role                                              --角色
                                , LoginTime                                         --登入時間
                                , CreateUserName                                    --新增人員
                                , CreateDate                                        --新增時間
                                , UpdateUserName                                    --修改人員
                                , UpdateDate                                        --修改時間
                            from Accounts WHERE AccountID = @AccountID AND Password = @Password ");
            _sqlParams = new Dapper.DynamicParameters();
            _sqlParams.Add("AccountID", AccountID);
            _sqlParams.Add("Password", Password);

            using (var cn = new SqlConnection(_dbConnPPP)) //連接資料庫
            {
                cn.Open();
                result = cn.Query<AccountsModel>(_sqlStr.ToString(), _sqlParams).FirstOrDefault();
            }
            return result;
            #endregion
        }
        #endregion
        #region 抓下拉選單(起)
        /// <summary>
        /// 抓地區別下拉選單
        /// </summary>
        /// <returns></returns>
        internal DropDownListViewModel getddlARE()
        {
            //組立SQL字串並連接資料庫
            #region 參數告宣
            DropDownListViewModel result = new DropDownListViewModel();
            #endregion

            #region 流程

            StringBuilder _sqlStr = new StringBuilder();
            _sqlStr.Append(@"SELECT DISTINCT AREA as 'Values', AREA as 'Text' 
                             FROM Until 
                             WHERE AREA IS NOT NULL
                             Order by AREA ");
            _sqlParams = new Dapper.DynamicParameters();

            result.DropDownListLT = new List<DropDownListItem>();
            using (var cn = new SqlConnection(_dbConnPPP)) //連接資料庫
            {
                cn.Open();
                result.DropDownListLT = cn.Query<DropDownListItem>(_sqlStr.ToString(), _sqlParams).ToList();
            }
            return result;
            #endregion
        }
        /// <summary>
        /// 抓分行下拉選單
        /// </summary>
        /// <returns></returns>
        internal DropDownListViewModel getddlUntil()
        {
            //組立SQL字串並連接資料庫
            #region 參數告宣
            DropDownListViewModel result = new DropDownListViewModel();
            #endregion

            #region 流程

            StringBuilder _sqlStr = new StringBuilder();
            _sqlStr.Append(@"SELECT DISTINCT Branch_No as 'Values', Branch_No + Name as 'Text' 
                             FROM Until 
                             Order by Branch_No ");
            _sqlParams = new Dapper.DynamicParameters();

            result.DropDownListLT = new List<DropDownListItem>();
            using (var cn = new SqlConnection(_dbConnPPP)) //連接資料庫
            {
                cn.Open();
                result.DropDownListLT = cn.Query<DropDownListItem>(_sqlStr.ToString(), _sqlParams).ToList();
            }
            return result;
            #endregion
        }
        /// <summary>
        /// 依地區別抓分行下拉選單
        /// </summary>
        /// <returns></returns>
        internal DropDownListViewModel getddlUntilforArea(string Area)
        {
            //組立SQL字串並連接資料庫
            #region 參數告宣
            DropDownListViewModel result = new DropDownListViewModel();
            #endregion

            #region 流程

            StringBuilder _sqlStr = new StringBuilder();
            _sqlStr.Append(@"SELECT DISTINCT Branch_No as 'Values', Branch_No + Name as 'Text' 
                             FROM Until where Area=@Area
                             Order by Branch_No ");
            _sqlParams = new Dapper.DynamicParameters();
            _sqlParams.Add("Area", Area);
            result.DropDownListLT = new List<DropDownListItem>();
            using (var cn = new SqlConnection(_dbConnPPP)) //連接資料庫
            {
                cn.Open();
                result.DropDownListLT = cn.Query<DropDownListItem>(_sqlStr.ToString(), _sqlParams).ToList();
            }
            return result;
            #endregion
        }
        /// <summary>
        /// 依地區別抓分行下拉選單
        /// </summary>
        /// <returns></returns>
        internal DropDownListViewModel getddlUntilforAss_GroupName(string Ass_GroupName)
        {
            //組立SQL字串並連接資料庫
            #region 參數告宣
            DropDownListViewModel result = new DropDownListViewModel();
            #endregion

            #region 流程

            StringBuilder _sqlStr = new StringBuilder();
            _sqlStr.Append(@"SELECT DISTINCT Branch_No as 'Values', Branch_No + Name as 'Text' 
                             FROM Until where Ass_GroupName=@Ass_GroupName
                             Order by Branch_No ");
            _sqlParams = new Dapper.DynamicParameters();
            _sqlParams.Add("Ass_GroupName", Ass_GroupName);
            result.DropDownListLT = new List<DropDownListItem>();
            using (var cn = new SqlConnection(_dbConnPPP)) //連接資料庫
            {
                cn.Open();
                result.DropDownListLT = cn.Query<DropDownListItem>(_sqlStr.ToString(), _sqlParams).ToList();
            }
            return result;
            #endregion
        }
        /// <summary>
        /// 依地區別抓分行下拉選單
        /// </summary>
        /// <returns></returns>
        internal DropDownListViewModel getddlUntilforGen_JurisName(string Gen_JurisName)
        {
            //組立SQL字串並連接資料庫
            #region 參數告宣
            DropDownListViewModel result = new DropDownListViewModel();
            #endregion

            #region 流程

            StringBuilder _sqlStr = new StringBuilder();
            _sqlStr.Append(@"SELECT DISTINCT Branch_No as 'Values', Branch_No + Name as 'Text' 
                             FROM Until where Gen_JurisName=@Gen_JurisName
                             Order by Branch_No ");
            _sqlParams = new Dapper.DynamicParameters();
            _sqlParams.Add("Gen_JurisName", Gen_JurisName);
            result.DropDownListLT = new List<DropDownListItem>();
            using (var cn = new SqlConnection(_dbConnPPP)) //連接資料庫
            {
                cn.Open();
                result.DropDownListLT = cn.Query<DropDownListItem>(_sqlStr.ToString(), _sqlParams).ToList();
            }
            return result;
            #endregion
        }
        internal DropDownListViewModel getddlUntilforAreacenterName(string AreacenterName)
        {
            //組立SQL字串並連接資料庫
            #region 參數告宣
            DropDownListViewModel result = new DropDownListViewModel();
            #endregion

            #region 流程

            StringBuilder _sqlStr = new StringBuilder();
            _sqlStr.Append(@"SELECT DISTINCT Branch_No as 'Values', Branch_No + Name as 'Text' 
                             FROM Until where Ass_GroupName=@AreacenterName
                             Order by Branch_No ");
            _sqlParams = new Dapper.DynamicParameters();
            _sqlParams.Add("AreacenterName", AreacenterName);
            result.DropDownListLT = new List<DropDownListItem>();
            using (var cn = new SqlConnection(_dbConnPPP)) //連接資料庫
            {
                cn.Open();
                result.DropDownListLT = cn.Query<DropDownListItem>(_sqlStr.ToString(), _sqlParams).ToList();
            }
            return result;
            #endregion
        }
        internal DropDownListViewModel GetddlUntil( string Area)
        {
            //組立SQL字串並連接資料庫
            #region 參數告宣
            DropDownListViewModel result = new DropDownListViewModel();
            #endregion

            #region 流程

            StringBuilder _sqlStr = new StringBuilder();
            _sqlStr.Append(@"SELECT DISTINCT Branch_No as 'Values', Branch_No + Name as 'Text' 
                             FROM Until  where 1 = 1
                              ");
            _sqlParams = new Dapper.DynamicParameters();

            if (!string.IsNullOrEmpty(Area))
            {
                _sqlParams.Add("Area", Area);
                _sqlStr.Append(" AND Area = @Area ");
            }
            _sqlStr.Append(" Order by Branch_No ");
            result.DropDownListLT = new List<DropDownListItem>();
            using (var cn = new SqlConnection(_dbConnPPP)) //連接資料庫
            {
                cn.Open();
                result.DropDownListLT = cn.Query<DropDownListItem>(_sqlStr.ToString(), _sqlParams).ToList();
            }
            return result;
            #endregion
        }
        /// <summary>
        /// 抓考核組別下拉選單
        /// </summary>
        /// <returns></returns>
        internal DropDownListViewModel getddlAss_GroupName()
        {
            //組立SQL字串並連接資料庫
            #region 參數告宣
            DropDownListViewModel result = new DropDownListViewModel();
            #endregion

            #region 流程

            StringBuilder _sqlStr = new StringBuilder();
            _sqlStr.Append(@"SELECT DISTINCT Ass_GroupName as 'Values', Ass_GroupName as 'Text' 
                             FROM Until 
                             WHERE Ass_GroupName IS NOT NULL
                             Order by Ass_GroupName ");
            _sqlParams = new Dapper.DynamicParameters();

            result.DropDownListLT = new List<DropDownListItem>();
            using (var cn = new SqlConnection(_dbConnPPP)) //連接資料庫
            {
                cn.Open();
                result.DropDownListLT = cn.Query<DropDownListItem>(_sqlStr.ToString(), _sqlParams).ToList();
            }
            return result;
            #endregion
        }
        /// <summary>
        /// 抓區域中心
        /// </summary>
        /// <returns></returns>
        internal DropDownListViewModel getddlAreacenterName()
        {
            //組立SQL字串並連接資料庫
            #region 參數告宣
            DropDownListViewModel result = new DropDownListViewModel();
            #endregion

            #region 流程

            StringBuilder _sqlStr = new StringBuilder();
            _sqlStr.Append(@"SELECT DISTINCT AreacenterName as 'Values', AreacenterName as 'Text' 
                             FROM Until 
                             WHERE AreacenterName IS NOT NULL
                             Order by AreacenterName ");
            _sqlParams = new Dapper.DynamicParameters();

            result.DropDownListLT = new List<DropDownListItem>();
            using (var cn = new SqlConnection(_dbConnPPP)) //連接資料庫
            {
                cn.Open();
                result.DropDownListLT = cn.Query<DropDownListItem>(_sqlStr.ToString(), _sqlParams).ToList();
            }
            return result;
            #endregion
        }
        /// <summary>
        /// 抓副總轄管區
        /// </summary>
        /// <returns></returns>
        internal DropDownListViewModel getddlGen_JurisName()
        {
            //組立SQL字串並連接資料庫
            #region 參數告宣
            DropDownListViewModel result = new DropDownListViewModel();
            #endregion

            #region 流程

            StringBuilder _sqlStr = new StringBuilder();
            _sqlStr.Append(@"SELECT DISTINCT Gen_JurisName as 'Values', Gen_JurisName as 'Text' 
                             FROM Until 
                             WHERE Gen_JurisName IS NOT NULL
                             Order by Gen_JurisName ");
            _sqlParams = new Dapper.DynamicParameters();

            result.DropDownListLT = new List<DropDownListItem>();
            using (var cn = new SqlConnection(_dbConnPPP)) //連接資料庫
            {
                cn.Open();
                result.DropDownListLT = cn.Query<DropDownListItem>(_sqlStr.ToString(), _sqlParams).ToList();
            }
            return result;
            #endregion
        }
        /// <summary>
        /// 抓行舍
        /// </summary>
        /// <returns></returns>
        internal DropDownListViewModel getddlDormName()
        {
            //組立SQL字串並連接資料庫
            #region 參數告宣
            DropDownListViewModel result = new DropDownListViewModel();
            #endregion

            #region 流程

            StringBuilder _sqlStr = new StringBuilder();
            _sqlStr.Append(@"SELECT DISTINCT DormName as 'Values', DormName as 'Text' 
                             FROM Until 
                             WHERE DormName IS NOT NULL
                             Order by DormName ");
            _sqlParams = new Dapper.DynamicParameters();

            result.DropDownListLT = new List<DropDownListItem>();
            using (var cn = new SqlConnection(_dbConnPPP)) //連接資料庫
            {
                cn.Open();
                result.DropDownListLT = cn.Query<DropDownListItem>(_sqlStr.ToString(), _sqlParams).ToList();
            }
            return result;
            #endregion
        }
        #endregion 抓下拉選單(迄)

    }
}