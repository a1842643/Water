using WaterCaseTracking;
using WaterCaseTracking.Dao;
using WaterCaseTracking.Models;
using WaterCaseTracking.Models.ViewModels;
using WaterCaseTracking.Models.ViewModels.KPI_PerformanceQuery;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace WaterCaseTracking.Service
{
    public class KPI_PerformanceQueryService
    {
        internal DataTable getExportData(ExportViewModel exportViewModel)
        {
            #region 參數宣告				
            StoredProcedureDao storedProcedureDao = new StoredProcedureDao();
            DataTable dt = new DataTable();
            #endregion

            #region 流程	
            //暫時寫死第2層
            exportViewModel.ddlLevels = "2";
            string SPName = Convert.ToInt32(exportViewModel.ddlYeart) > (DateTime.Now.Year - 4) ? "getPerformance_Score_old" : "getPerformance_Score";
            dt = storedProcedureDao.getKPIMReport(exportViewModel, SPName); //將參數送入Dao層,組立SQL字串並連接資料庫

            return dt;
            #endregion
        }
    }
}