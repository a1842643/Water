using Dapper;
using WaterCaseTracking.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;

namespace WaterCaseTracking.Dao
{
    public class SysCodeDao : _BaseDao
    {
        #region 抓下拉選單(起)
        /// <summary>
        /// 抓下拉選單
        /// </summary>
        /// <param name="GroupCode">群組代碼</param>
        /// <returns></returns>
        internal DropDownListViewModel getddlItem(string GroupCode)
        {
            //組立SQL字串並連接資料庫
            #region 參數告宣
            DropDownListViewModel result = new DropDownListViewModel();
            #endregion

            #region 流程

            StringBuilder _sqlStr = new StringBuilder();
            _sqlStr.Append(@"SELECT ITEM_NAME as 'Values', ITEM_NAME as 'Text'
                             FROM Sys_Code 
                             Where GROUP_CODE = @GROUP_CODE 
                             Order by SORT ");
            _sqlParams = new Dapper.DynamicParameters();

            _sqlParams.Add("GROUP_CODE", GroupCode);

            result.DropDownListLT = new List<DropDownListItem>();
            using (var cn = new SqlConnection(_dbConnPPP)) //連接資料庫
            {
                cn.Open();
                result.DropDownListLT= cn.Query<DropDownListItem>(_sqlStr.ToString(), _sqlParams).ToList();
            }
            return result;
            #endregion
        }
        #endregion 抓下拉選單(迄)
    }
}