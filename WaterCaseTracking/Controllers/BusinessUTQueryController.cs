using WaterCaseTracking.Models;
using WaterCaseTracking.Models.ViewModels.BusinessUT;
using WaterCaseTracking.Service;
using System;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WaterCaseTracking.Controllers
{
    [Authorize]
    public class BusinessUTQueryController : BaseController
    {
        string FuncName = "營業單位資料查詢";
        // GET: BusinessUT
        public ActionResult Maintain()
        {
            logging(FuncName, "進入功能-" + FuncName);
            return View();
        }
        #region 初始化-起
        [HttpPost]
        public ActionResult searchInit()
        {
            #region 參數宣告
            DropDownListService dropDownListService = new DropDownListService();
            SearchInitViewModel searchInitViewModel = new SearchInitViewModel();
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
                //抓行舍
                searchInitViewModel.ddlDormName = dropDownListService.getddlDormName();
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
        /// <summary>
        /// 查詢
        /// </summary>
        /// <param name="searchInfo"></param>
        /// <returns></returns>
        public ActionResult GetoTable(SearchInfoViewModel searchInfo)
        {
            logging(FuncName, "查詢");
            #region 參數宣告

            SearchListViewModel searchList = new SearchListViewModel();
            BusinessUTService businessUTService = new BusinessUTService();
            #endregion

            #region 流程	
            try
            {
                //送參數進入Service層做商業邏輯
                searchList = businessUTService.QuerySearchList(searchInfo);
            }
            catch (Exception ex)
            {
                searchList.db_Result = "Fail , " + ex.Message;
                errLogging(FuncName, ex.ToString());
            }
            //組Json格式回傳Models資料
            return Json(searchList, JsonRequestBehavior.AllowGet);
            #endregion

        }

        #region 連動分行-起
        [HttpPost]
        public ActionResult GetddlUntil(string Area)
        {

            #region 參數宣告
            DropDownListService dropDownListService = new DropDownListService();
            SearchInitViewModel searchInitViewModel = new SearchInitViewModel();
            #endregion

            #region 流程	

            try
            {
                //抓分行下拉選單
                searchInitViewModel.ddlUntil = dropDownListService.getddlUntil(Area);
            }
            catch (Exception ex)
            {
                searchInitViewModel.db_Result = "Fail , " + ex.Message;
            }
            //組Json格式回傳Models資料
            return Json(searchInitViewModel, JsonRequestBehavior.AllowGet);
            #endregion

        }
        #endregion 連動分行-迄

        #region 產表-起
        [HttpPost]
        public ActionResult Export(ExportViewModel exportViewModel)
        {
            logging(FuncName, "匯出");
            #region 參數宣告
            BusinessUTService businessUTService = new BusinessUTService();
            DataTable dt = new DataTable();
            string fileNamePath = "";
            #endregion

            #region 流程	
            try
            {
                //查詢修改資料
                dt = businessUTService.getExportData(exportViewModel);

                fileNamePath = ExportFunction.ExportDataTableTo(dt, exportViewModel.fileExtension, "BussinessUTQuery");
            }
            catch (Exception ex)
            {
                errLogging(FuncName, ex.ToString());
            }
            return Json(fileNamePath, JsonRequestBehavior.AllowGet);

            #endregion

        }
        #endregion 產表-迄


    }
}