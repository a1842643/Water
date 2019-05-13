using ADO;
using Common;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using WaterCaseTracking.Models.ViewModels.CustomreportUntil;
using WaterCaseTracking.Models.ViewModels;
using WaterCaseTracking.Dao;
using ExportViewModel = WaterCaseTracking.Models.ViewModels.CustomreportUntil.ExportViewModel;

namespace WaterCaseTracking.Service
{
    public class CustomreportUntilService 
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

            DropDownListViewModel ddlUntil = untilDao.getddlUntilforArea(Area); //將參數送入Dao層,組立SQL字串並連接資料庫

          



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
            CustomreportUntilDao CustomreportUntilDao = new CustomreportUntilDao();
            #endregion

            #region 流程																
            DropDownListViewModel ddlYM = CustomreportUntilDao.getddlYM(); //將參數送入Dao層,組立SQL字串並連接資料庫

            return ddlYM;
            #endregion
        }
        internal DropDownListViewModel getdllColums()
        {
            #region 參數宣告				
            CustomreportUntilDao CustomreportUntilDao = new CustomreportUntilDao();
            #endregion

            #region 流程																
            DropDownListViewModel ddlColumns = CustomreportUntilDao.getddFileColumns(); //將參數送入Dao層,組立SQL字串並連接資料庫

            return ddlColumns;
            #endregion
        }
        internal DropDownListViewModel getdllColums(string Group,string Class)
        {
            #region 參數宣告				
            CustomreportUntilDao CustomreportUntilDao = new CustomreportUntilDao();
            #endregion

            #region 流程																
            DropDownListViewModel ddlColumns = CustomreportUntilDao.getddFileColumns( Group,Class); //將參數送入Dao層,組立SQL字串並連接資料庫

            return ddlColumns;
            #endregion
        }
        internal DropDownListViewModel getdllUPColums( string Class)
        {
            #region 參數宣告				
            CustomreportUntilDao CustomreportUntilDao = new CustomreportUntilDao();
            #endregion

            #region 流程																
            DropDownListViewModel ddlColumns = CustomreportUntilDao.getddUPFileColumns(Class); //將參數送入Dao層,組立SQL字串並連接資料庫

            return ddlColumns;
            #endregion
        }
        internal DataTable getExportData(ExportViewModel Search)
        {
            #region 參數宣告				
          
            DataTable dt = new DataTable();
            CustomreportUntilDao CustomreportUntilDao = new CustomreportUntilDao();
            #endregion

            #region 流程		
            dt = CustomreportUntilDao.getCustomreport(Search.ddlUntil,Search.ddlFileColumns);
            return dt;

            #endregion
        }
    }
}