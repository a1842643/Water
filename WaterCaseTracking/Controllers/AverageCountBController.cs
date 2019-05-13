using WaterCaseTracking.Models.ViewModels;
using WaterCaseTracking.Models.ViewModels.AverageCount;
using WaterCaseTracking.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using ExportViewModel = WaterCaseTracking.Models.ViewModels.AverageCount.ExportViewModel;
using static WaterCaseTracking.Models.ViewModels.AverageCount.SearchListViewModel;

namespace WaterCaseTracking.Controllers
{
    public class AverageCountBController : BaseController
    {
        string FuncName = "經營管理紀錄卡(月)累計平均查詢";
        // GET: AverageCountB
        public ActionResult Admin_card()
        {
            return View();
        }
        #region 初始化-起
        [HttpPost]
        public ActionResult searchInit()
        {
            #region 參數宣告

            DropDownListService dropDownListService = new DropDownListService();
            AverageCountBService averageCountService = new AverageCountBService();
            SearchInitViewModel searchInitViewModel = new SearchInitViewModel();
            DropDownListViewModel dropDownListViewModel = new DropDownListViewModel();
            List<DropDownListItem> ddlListYeart = new List<DropDownListItem>();
           
            #endregion

            #region 流程	

            try
            {
              
             
                searchInitViewModel.ddlYM = averageCountService.getddlYM();

                //抓考核組別下拉選單
                searchInitViewModel.ddlAssessmentGroup = dropDownListService.getddlAssessmentGroup();
                //抓區域中心
                searchInitViewModel.ddlAreacenterName = dropDownListService.getddlAreacenterName();
                //抓副總轄管區
                searchInitViewModel.ddlGen_JurisName = dropDownListService.getddlGen_JurisName();

                //抓地區下拉選單
                searchInitViewModel.ddlArea = dropDownListService.getddlArea();
                //抓分行下拉選單
                searchInitViewModel.ddlUntil = dropDownListService.getddlUntil();
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
        public ActionResult AverageResult(ExportViewModel Search)
        {
            string msg = "";
            AverageCountBService AverageCountService = new AverageCountBService();
            msg = AverageCountService.AverageResult(Search);
            return Json(msg, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult AveragecountResult(ExportViewModel Search)
        {
            string msg = "";
            AverageCountBService AverageCountService = new AverageCountBService();
            msg = AverageCountService.AveragecountResult(Search);
            return Json(msg, JsonRequestBehavior.AllowGet);
        }
      

        [HttpPost]
        public ActionResult AveragecountResult_Export(ExportViewModel Search)
        {
            #region 參數宣告
            DataTable dt = new DataTable();
            AverageCountBService AverageCountBService = new AverageCountBService();
            string fileNamePath = "";
            #endregion

            #region 流程
            try
            {
                logging(FuncName, "下載匯入3.經營管理紀錄卡累計平均");
                //送參數進入Service層做商業邏輯
                dt = AverageCountBService.getExportData(Search,"Admin_card_count");

                fileNamePath = ExportFunction.ExportDataTableTo(dt, "xlsx", "經營管理紀錄卡累計平均");
            }
            catch (Exception ex)
            {
                errLogging(FuncName, ex.ToString());
            }
            return Json(fileNamePath, JsonRequestBehavior.AllowGet);
            #endregion
            //string msg = "";
            //AverageCountBService AverageCountService = new AverageCountBService();
            //msg = AverageCountService.AveragecountResult(Search);
            //return Json(msg, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult AveragecountResult_ODF(ExportViewModel Search)
        {
            #region 參數宣告
            DataTable dt = new DataTable();
            AverageCountBService AverageCountBService = new AverageCountBService();
            string fileNamePath = "";
            #endregion

            #region 流程
            try
            {
                logging(FuncName, "下載匯入3.經營管理紀錄卡累計平均");
                //送參數進入Service層做商業邏輯
                dt = AverageCountBService.getExportData(Search, "Admin_card_count");

                fileNamePath = ExportFunction.ExportDataTableTo(dt, "ODF", "經營管理紀錄卡累計平均");
            }
            catch (Exception ex)
            {
                errLogging(FuncName, ex.ToString());
            }
            return Json(fileNamePath, JsonRequestBehavior.AllowGet);
            #endregion
            //string msg = "";
            //AverageCountBService AverageCountService = new AverageCountBService();
            //msg = AverageCountService.AveragecountResult(Search);
            //return Json(msg, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult AreaClick(SearchItemViewModel Search)
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

                if (Search.ddlArea == null)
                    //抓分行下拉選單
                    searchInitViewModel.ddlUntil = dropDownListService.getddlUntil();
                else

                    //抓分行下拉選單
                    searchInitViewModel.ddlUntil = dropDownListService.getddlUntilforArea(Search.ddlArea);
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
        public ActionResult AverageResult_Export(ExportViewModel Search)
        {
            #region 參數宣告
            DataTable dt = new DataTable();
            AverageCountBService AverageCountBService = new AverageCountBService();
            string fileNamePath = "";
            #endregion

            #region 流程
            try
            {
                logging(FuncName, "下載匯入經營管理紀錄卡月平均");
                //送參數進入Service層做商業邏輯
                dt = AverageCountBService.getExportData(Search, "Admin_card");

                fileNamePath = ExportFunction.ExportDataTableTo(dt, "xlsx", "test");//經營管理紀錄卡月平均
            }
            catch (Exception ex)
            {
                errLogging(FuncName, ex.ToString());
            }
            return Json(fileNamePath, JsonRequestBehavior.AllowGet);
            #endregion
            //string msg = "";
            //AverageCountBService AverageCountService = new AverageCountBService();
            //msg = AverageCountService.AveragecountResult(Search);
            //return Json(msg, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult AverageResult_ODF(ExportViewModel Search)
        {
            #region 參數宣告
            DataTable dt = new DataTable();
            AverageCountBService AverageCountBService = new AverageCountBService();
            string fileNamePath = "";
            #endregion

            #region 流程
            try
            {
                logging(FuncName, "下載匯入經營管理紀錄卡月平均");
                //送參數進入Service層做商業邏輯
                dt = AverageCountBService.getExportData(Search, "Admin_card");

                fileNamePath = ExportFunction.ExportDataTableTo(dt, "ODF", "test");//經營管理紀錄卡月平均
            }
            catch (Exception ex)
            {
                errLogging(FuncName, ex.ToString());
            }
            return Json(fileNamePath, JsonRequestBehavior.AllowGet);
            #endregion
            //string msg = "";
            //AverageCountBService AverageCountService = new AverageCountBService();
            //msg = AverageCountService.AveragecountResult(Search);
            //return Json(msg, JsonRequestBehavior.AllowGet);
        }
    }
}