using ADO;
using Common;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using WaterCaseTracking.Models.ViewModels.Customreport;
using WaterCaseTracking.Models.ViewModels;
using WaterCaseTracking.Dao;
using ExportViewModel = WaterCaseTracking.Models.ViewModels.Customreport.ExportViewModel;

namespace WaterCaseTracking.Service
{
    public class CustomreportService 
    {
        internal DropDownListViewModel getddlUntil()
        {
            #region 參數宣告				
            UntilDao untilDao = new UntilDao();
            #endregion

            #region 流程																
            DropDownListViewModel ddlUntil = untilDao.getddlUntil(); //將參數送入Dao層,組立SQL字串並連接資料庫

            return ddlUntil;
            #endregion
        }
        internal DropDownListViewModel getddlUntil(string Area)
        {
            #region 參數宣告				
            UntilDao untilDao = new UntilDao();
            #endregion

            #region 流程																
            DropDownListViewModel ddlUntil = untilDao.GetddlUntil(Area); //將參數送入Dao層,組立SQL字串並連接資料庫

            return ddlUntil;
            #endregion
        }
        internal DropDownListViewModel getddlArea()
        {
            #region 參數宣告				
            UntilDao untilDao = new UntilDao();
            #endregion

            #region 流程																
            DropDownListViewModel ddlArea = untilDao.getddlARE(); //將參數送入Dao層,組立SQL字串並連接資料庫

            return ddlArea;
            #endregion
        }
        internal DropDownListViewModel getdllYM()
        {
            #region 參數宣告				
            CustomreportDao CustomreportDao = new CustomreportDao();
            #endregion

            #region 流程																
            DropDownListViewModel ddlYM = CustomreportDao.getddlYM(); //將參數送入Dao層,組立SQL字串並連接資料庫

            return ddlYM;
            #endregion
        }
        internal DropDownListViewModel getdllColums()
        {
            #region 參數宣告				
            CustomreportDao CustomreportDao = new CustomreportDao();
            #endregion

            #region 流程																
            DropDownListViewModel ddlColumns = CustomreportDao.getddFileColumns(); //將參數送入Dao層,組立SQL字串並連接資料庫

            return ddlColumns;
            #endregion
        }
        internal DropDownListViewModel getdllColums(string Group,string Class)
        {
            #region 參數宣告				
            CustomreportDao CustomreportDao = new CustomreportDao();
            #endregion

            #region 流程																
            DropDownListViewModel ddlColumns = CustomreportDao.getddFileColumns( Group,Class); //將參數送入Dao層,組立SQL字串並連接資料庫

            return ddlColumns;
            #endregion
        }
        internal DropDownListViewModel getdllUPColums( string Class)
        {
            #region 參數宣告				
            CustomreportDao CustomreportDao = new CustomreportDao();
            #endregion

            #region 流程																
            DropDownListViewModel ddlColumns = CustomreportDao.getddUPFileColumns(Class); //將參數送入Dao層,組立SQL字串並連接資料庫

            return ddlColumns;
            #endregion
        }
        internal DataTable getExportData(ExportViewModel Search)
        {
            #region 參數宣告				
          
            DataTable dt = new DataTable();
            CustomreportDao CustomreportDao = new CustomreportDao();
            #endregion

            #region 流程		
            dt = CustomreportDao.getCustomreport(Search.ddlUntil, Search.ddlSYM, Search.ddlEYM, Search.ddlFileColumns);
            return dt;

            #endregion
        }
    }
}