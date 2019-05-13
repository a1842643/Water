using WaterCaseTracking.Dao;
using WaterCaseTracking.Models.ViewModels;
using WaterCaseTracking.Models.ViewModels.ManagementRecordCardReport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WaterCaseTracking.Service
{
    public class ManagementRecordCardReportService
    {
        /// <summary>
        /// 抓分行別下拉選單
        /// </summary>
        /// <returns></returns>
        internal DropDownListViewModel getddlUntil(SearchInfoViewModel SearchInfo)
        {
            #region 參數宣告				
            ManagementRecordCardReportDao untilDao = new ManagementRecordCardReportDao();
            DropDownListViewModel ddlUntil = new DropDownListViewModel();
            #endregion

            #region 流程																
            ddlUntil = untilDao.getddlUntil(SearchInfo); //將參數送入Dao層,組立SQL字串並連接資料庫

            return ddlUntil;
            #endregion
        }
    }
}