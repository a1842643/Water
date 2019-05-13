using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WaterCaseTracking.Models.ViewModels;
using WaterCaseTracking.Models.ViewModels.CharTable;
using WaterCaseTracking.Service;
using System.Data;
using ExportViewModel = WaterCaseTracking.Models.ViewModels.CharTable.ExportViewModel;
using static WaterCaseTracking.Models.ViewModels.CharTable.SearchListViewModel;

namespace WaterCaseTracking.Controllers
{
    public class ChartTableController : BaseController
    {
        string FuncName = "圖表查詢";
        // GET: ChartTable
        public ActionResult ChartTable()
        {
            return View();
        }
        #region 初始化-起
        [HttpPost]
        public ActionResult searchInit()
        {

            #region 參數宣告
            CharTableService CharTableService = new CharTableService();
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
                searchInitViewModel.ddlSYM = CharTableService.getdllYM();
                //抓起YM下拉選單
                searchInitViewModel.ddlEYM = CharTableService.getdllYM();
                //抓迄YM下拉選單
                searchInitViewModel.ddlFileColumns = CharTableService.getdllColums();
                searchInitViewModel.ddlArea = CharTableService.getddlArea();

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
            CharTableService CharTableService = new CharTableService();
            DataTable dt = new DataTable();
            string fileNamePath = "";
            #endregion

            #region 流程	
            try
            {
                //查詢修改資料
                dt = CharTableService.getExportData(exportViewModel);
                

                fileNamePath = CharTableService.DataTableToExcel(dt, "OperatedCardBResult");
                fileNamePath = CharTableService.getExportChart(fileNamePath, dt.Rows.Count);
               


            }
            catch (Exception ex)
            {
                errLogging(FuncName, ex.ToString());
            }
            return Json(fileNamePath, JsonRequestBehavior.AllowGet);

            #endregion

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
        [HttpPost]
        public ActionResult AssessmentGroupClick(SearchItemViewModel Search)
        {
            #region 參數宣告
            DropDownListService dropDownListService = new DropDownListService();
            SearchInitViewModel searchInitViewModel = new SearchInitViewModel();
            DropDownListViewModel dropDownListViewModel = new DropDownListViewModel();
            List<DropDownListItem> ddlListYeart = new List<DropDownListItem>();
            //List<DropDownListItem> ddlListMonth = new List<DropDownListItem>();
            string isHis = "";
            #endregion

            #region 流程	

            try
            {
                if (Search.Ass_GroupName == null)
                    //抓分行下拉選單
                    searchInitViewModel.ddlUntil = dropDownListService.getddlUntil();
                else

                    //抓分行下拉選單
                    searchInitViewModel.ddlUntil = dropDownListService.getddlUntilforAss_GroupName(Search.Ass_GroupName);
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



        [HttpPost]
        public ActionResult AreacenterNameClick(SearchItemViewModel Search)
        {
            #region 參數宣告
            DropDownListService dropDownListService = new DropDownListService();
            SearchInitViewModel searchInitViewModel = new SearchInitViewModel();
            DropDownListViewModel dropDownListViewModel = new DropDownListViewModel();
            List<DropDownListItem> ddlListYeart = new List<DropDownListItem>();
            //List<DropDownListItem> ddlListMonth = new List<DropDownListItem>();
            string isHis = "";
            #endregion

            #region 流程	

            try
            {
                if (Search.AreacenterName == null)
                    //抓分行下拉選單
                    searchInitViewModel.ddlUntil = dropDownListService.getddlUntil();
                else
                    //抓分行下拉選單
                    searchInitViewModel.ddlUntil = dropDownListService.getddlUntilforAss_GroupName(Search.AreacenterName);
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



        [HttpPost]
        public ActionResult lGen_JurisNameClick(SearchItemViewModel Search)
        {
            #region 參數宣告
            DropDownListService dropDownListService = new DropDownListService();
            SearchInitViewModel searchInitViewModel = new SearchInitViewModel();
            DropDownListViewModel dropDownListViewModel = new DropDownListViewModel();
            List<DropDownListItem> ddlListYeart = new List<DropDownListItem>();
            //List<DropDownListItem> ddlListMonth = new List<DropDownListItem>();

            #endregion

            #region 流程	

            try
            {

                if (Search.Gen_JurisName == null)

                    //抓分行下拉選單
                    searchInitViewModel.ddlUntil = dropDownListService.getddlUntil();
                else
                    //抓分行下拉選單
                    searchInitViewModel.ddlUntil = dropDownListService.getddlUntilforGen_JurisName(Search.Gen_JurisName);
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