using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WaterCaseTracking.Models.ViewModels;
using WaterCaseTracking.Models.ViewModels.CustomreportUntil;
using WaterCaseTracking.Service;
using System.Data;
using ExportViewModel = WaterCaseTracking.Models.ViewModels.CustomreportUntil.ExportViewModel;
using static WaterCaseTracking.Models.ViewModels.CustomreportUntil.SearchListViewModel;

namespace WaterCaseTracking.Controllers
{
    public class CustomReportUntilController : BaseController
    {
        string FuncName = "自定義報表查詢";
        // GET: CustomReportUntil
        public ActionResult CustomReportUntil()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AreaClick(SearchItemViewModel Search)
        {
            #region 參數宣告
            DropDownListService dropDownListService = new DropDownListService();
            SearchInitViewModel searchInitViewModel = new SearchInitViewModel();
            DropDownListViewModel dropDownListViewModel = new DropDownListViewModel();

            //List<DropDownListItem> ddlListMonth = new List<DropDownListItem>();
            string isHis = "";
            #endregion

            #region 流程	

            try
            {

                if (Search.Area == null)
                    //抓分行下拉選單
                    searchInitViewModel.ddlUntil = dropDownListService.getddlUntil();
                else

                    //抓分行下拉選單
                    searchInitViewModel.ddlUntil = dropDownListService.getddlUntilforArea(Search.Area);
                //抓地區下拉選單
                searchInitViewModel.ddlArea = dropDownListService.getddlArea();

                //抓考核組別下拉選單
                searchInitViewModel.ddlAssessmentGroup = dropDownListService.getddlAssessmentGroup();
                //抓區域中心
                searchInitViewModel.ddlAreacenterName = dropDownListService.getddlAreacenterName();
                //抓副總轄管區
                searchInitViewModel.ddlGen_JurisName = dropDownListService.getddlGen_JurisName();

            }
            catch (Exception ex)
            {
                searchInitViewModel.db_Result = "Fail , " + ex.Message;
            }
            //組Json格式回傳Models資料
            return Json(searchInitViewModel, JsonRequestBehavior.AllowGet);
            #endregion

        }
        #region 初始化-起
        [HttpPost]
        public ActionResult searchInit()
        {

            #region 參數宣告
            CustomreportUntilService CustomreportUntilService = new CustomreportUntilService();
            SearchInitViewModel searchInitViewModel = new SearchInitViewModel();
            DropDownListService dropDownListService = new DropDownListService();
            #endregion

            #region 流程	

            try
            {

                //抓地區下拉選單
                searchInitViewModel.ddlArea = dropDownListService.getddlArea();
                //抓分行下拉選單
                searchInitViewModel.ddlUntil = dropDownListService.getddlUntil();


                //抓考核組別下拉選單
                searchInitViewModel.ddlAssessmentGroup = dropDownListService.getddlAssessmentGroup();
                //抓區域中心
                searchInitViewModel.ddlAreacenterName = dropDownListService.getddlAreacenterName();
                //抓副總轄管區
                searchInitViewModel.ddlGen_JurisName = dropDownListService.getddlGen_JurisName();
                //抓欄位下拉選單
                searchInitViewModel.ddlSYM = CustomreportUntilService.getdllYM();
                //抓起YM下拉選單
              
                searchInitViewModel.ddlFileColumns = CustomreportUntilService.getdllColums();
                searchInitViewModel.ddlArea = CustomreportUntilService.getddlArea();

            }
            catch (Exception ex)
            {
                searchInitViewModel.db_Result = "Fail , " + ex.Message;
            }
            //組Json格式回傳Models資料
            return Json(searchInitViewModel, JsonRequestBehavior.AllowGet);
            #endregion

        }
        #endregion 初始化-迄
        [HttpPost]
        public ActionResult Export(ExportViewModel exportViewModel)
        {
            logging(FuncName, "匯出");
            #region 參數宣告
            CustomreportUntilService CustomreportUntilService = new CustomreportUntilService();
            DataTable dt = new DataTable();
            string fileNamePath = "";
            #endregion

            #region 流程	
            try
            {
                //查詢修改資料
                dt = CustomreportUntilService.getExportData(exportViewModel);

                fileNamePath = ExportFunction.ExportDataTableTo(dt, "xlsx", "經營管理紀錄卡累計平均");

            }
            catch (Exception ex)
            {
                errLogging(FuncName, ex.ToString());
            }
            return Json(fileNamePath, JsonRequestBehavior.AllowGet);

            #endregion

        }
        [HttpPost]
        public ActionResult ODFExport(ExportViewModel exportViewModel)
        {
            logging(FuncName, "匯出");
            #region 參數宣告
            CustomreportUntilService CustomreportUntilService = new CustomreportUntilService();
            DataTable dt = new DataTable();
            string fileNamePath = "";
            #endregion

            #region 流程	
            try
            {
                //查詢修改資料
                dt = CustomreportUntilService.getExportData(exportViewModel);

                fileNamePath = ExportFunction.ExportDataTableTo(dt, "ODF", "經營管理紀錄卡累計平均");

            }
            catch (Exception ex)
            {
                errLogging(FuncName, ex.ToString());
            }
            return Json(fileNamePath, JsonRequestBehavior.AllowGet);

            #endregion

        }
        [HttpPost]
        public ActionResult ClassClick(ExportViewModel exportViewModel)
        {
            #region 參數宣告
            CustomreportService CustomreportService = new CustomreportService();
            SearchInitViewModel searchInitViewModel = new SearchInitViewModel();
            #endregion

            #region 流程	

            try
            {

                searchInitViewModel.ddlFileColumns = CustomreportService.getdllColums(exportViewModel.FileColumns, exportViewModel.tablename);
                searchInitViewModel.ddlGroup = CustomreportService.getdllUPColums(exportViewModel.tablename);
            }
            catch (Exception ex)
            {
                searchInitViewModel.db_Result = "Fail , " + ex.Message;
            }
            //組Json格式回傳Models資料
            return Json(searchInitViewModel, JsonRequestBehavior.AllowGet);
        }
        #endregion
        [HttpPost]
        public ActionResult GroupClick(ExportViewModel exportViewModel)
        {
            #region 參數宣告
            CustomreportService CustomreportService = new CustomreportService();
            SearchInitViewModel searchInitViewModel = new SearchInitViewModel();
            #endregion

            try
            {

                searchInitViewModel.ddlFileColumns = CustomreportService.getdllColums(exportViewModel.FileColumns, exportViewModel.tablename);

            }
            catch (Exception ex)
            {
                searchInitViewModel.db_Result = "Fail , " + ex.Message;
            }
            //組Json格式回傳Models資料
            return Json(searchInitViewModel, JsonRequestBehavior.AllowGet);
        }

    }
}