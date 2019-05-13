using Dapper;
using WaterCaseTracking.Models.ViewModels;
using WaterCaseTracking.Models.ViewModels.ManagementRecordCardReport;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;

namespace WaterCaseTracking.Dao
{
    public class ManagementRecordCardReportDao : _BaseDao
    {
        /// <summary>
        /// 抓分行下拉選單
        /// </summary>
        /// <returns></returns>
        # region 抓分行下拉選單
        internal DropDownListViewModel getddlUntil(SearchInfoViewModel SearchInfo)
        {
            //組立SQL字串並連接資料庫
            #region 參數告宣
            DropDownListViewModel result = new DropDownListViewModel();
            #endregion

            #region 流程

            StringBuilder _sqlStr = new StringBuilder();
            //_sqlStr.Append(@"SELECT DISTINCT Branch_No as 'Values', Branch_No + Name as 'Text' 
            //                 FROM Until 
            //                 WHERE AREA = '' AND Ass_GroupName = '' AND AreacenterName = '' AND Gen_JurisName = '' 
            //                 Order by Branch_No ");
            _sqlStr.Append(@"SELECT DISTINCT Branch_No as 'Values', Branch_No + Name as 'Text' 
                             FROM Until 
                             WHERE 1=1 ");

            _sqlParams = new Dapper.DynamicParameters();
            //地區
            if (SearchInfo.ddlArea != null)
            {
                _sqlStr.Append("AND AREA = @AREA ");
                _sqlParams.Add("AREA", SearchInfo.ddlArea);
            }
            //考核組別
            if (SearchInfo.ddlAssessmentGroup != null)
            {
                _sqlStr.Append("AND Ass_GroupName = @Ass_GroupName ");
                _sqlParams.Add("Ass_GroupName", SearchInfo.ddlAssessmentGroup);
            }
            //區域中心
            if (SearchInfo.ddlAreacenterName != null)
            {
                _sqlStr.Append("AND AreacenterName = @AreacenterName ");
                _sqlParams.Add("AreacenterName", SearchInfo.ddlAreacenterName);
            }
            //副總轄管區
            if (SearchInfo.ddlGen_JurisName != null)
            {
                _sqlStr.Append("AND Gen_JurisName = @AreacenterName ");
                _sqlParams.Add("AreacenterName", SearchInfo.ddlGen_JurisName);
            }

            _sqlStr.Append("Order by Branch_No ");

            result.DropDownListLT = new List<DropDownListItem>();
            using (var cn = new SqlConnection(_dbConnPPP)) //連接資料庫
            {
                cn.Open();
                result.DropDownListLT = cn.Query<DropDownListItem>(_sqlStr.ToString(), _sqlParams).ToList();
            }
            return result;
            #endregion
        }
        #endregion
    }
}