using ADO;
using Common;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using WaterCaseTracking.Models.ViewModels;
using WaterCaseTracking.Models.ViewModels.AverageCount;
using System.Text;
using WaterCaseTracking.Dao;
using ExportViewModel = WaterCaseTracking.Models.ViewModels.AverageCount.ExportViewModel;

namespace WaterCaseTracking.Service
{
    public class AverageCountBService
    {
        #region 回傳HTML語法
        public string AverageResult(ExportViewModel Search)
        {
            string msg = "";
            AverageCountDao AverageCountDao = new AverageCountDao();
           
            msg = Average(Search);

            return msg;
        }
        public string AveragecountResult(ExportViewModel Search)
        {
            string msg = "";
          
            msg = Averagecount(Search);

            return msg;
        }
        #endregion
        public string Average(ExportViewModel Search)//
        {
            string msg = "<table border='1' width='100%'><tr>";
            AverageCountDao AverageCountDao = new AverageCountDao();
            DataTable dt = AverageCountDao.getAverage(Search.ddlArea, Search.ddlYM,Search.ddlUntil,Search.Ass_GroupName,Search.AreacenterName,Search.Gen_JurisName);
            for (int i=0;i<dt.Columns.Count;i++)
            {
                msg = msg + "<td>" + dt.Columns[i].ColumnName + "</td>";
            }
            msg = msg + "</tr>";
            for (int i=0;i<dt.Rows.Count;i++)
            {
                msg = msg + "<tr>";
                for(int j=0;j<dt.Columns.Count;j++)
                {
                    msg = msg + "<td>" + dt.Rows[i][j].ToString() + "</td>";
                }
                msg = msg + "</tr>";
            }
            msg = msg + "</table>";

            return msg;
        }
        public string Averagecount(ExportViewModel Search)//
        {
            string msg = "<table border='1' width='100%'><tr>";
            AverageCountDao AverageCountDao = new AverageCountDao();
            DataTable dt = AverageCountDao.getAveragecount(Search.ddlArea, Search.ddlYM, Search.ddlUntil, Search.Ass_GroupName, Search.AreacenterName, Search.Gen_JurisName);
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                msg = msg + "<td>" + dt.Columns[i].ColumnName + "</td>";
            }
            msg = msg + "</tr>";
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                msg = msg + "<tr>";
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    msg = msg + "<td>" + dt.Rows[i][j].ToString() + "</td>";
                }
                msg = msg + "</tr>";
            }
            msg = msg + "</table>";

            return msg;
        }
        internal DropDownListViewModel getddlYM()
        {
            #region 參數宣告				
            AverageCountDao AverageCountDao = new AverageCountDao();
            #endregion

            #region 流程																
            DropDownListViewModel ddlYM = AverageCountDao.getddlYM(); //將參數送入Dao層,組立SQL字串並連接資料庫

            return ddlYM;
            #endregion
        }
        internal DataTable getExportData(ExportViewModel Search,string type)
        {
            #region 參數宣告				
          
            DataTable dt = new DataTable();
            AverageCountDao AverageCountDao = new AverageCountDao();
            #endregion

            #region 流程		
            if (type == "Admin_card_count")
                dt = AverageCountDao.getAveragecount(Search.ddlArea, Search.ddlYM, Search.ddlUntil, Search.Ass_GroupName, Search.AreacenterName, Search.Gen_JurisName);  //將參數送入Dao層,組立SQL字串並連接資料庫
            else
                dt = AverageCountDao.getAverage(Search.ddlArea, Search.ddlYM, Search.ddlUntil, Search.Ass_GroupName, Search.AreacenterName, Search.Gen_JurisName);  //將參數送入Dao層,組立SQL字串並連接資料庫

            return dt;
            #endregion
        }

    }
}