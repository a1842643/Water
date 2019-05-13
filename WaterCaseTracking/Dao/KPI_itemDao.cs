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
    public class KPI_itemDao : _BaseDao
    {
        /// <summary>
        /// 抓分行下拉選單
        /// </summary>
        /// <returns></returns>
        internal DropDownListViewModel getddlKPIPer()
        {
            //組立SQL字串並連接資料庫
            #region 參數告宣
            DropDownListViewModel result = new DropDownListViewModel();
            #endregion

            #region 流程

            StringBuilder _sqlStr = new StringBuilder();
            _sqlStr.Append(@"SELECT DISTINCT CONVERT(nvarchar(100),item.UUID) as 'Values', SUBJECT as 'Text' 
                             FROM KPI_item item
							 inner join KPI_Distribution dis on item.UUID = dis.UUID AND dis.YEART = (year(getdate()) -1911)
							 where LEVELS = 3 AND UPID = (select UUID from KPI_item where LEVELS = 2 AND SUBJECT = '業績績效')  ");
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

    }
}