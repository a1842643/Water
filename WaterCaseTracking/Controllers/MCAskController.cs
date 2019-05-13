using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WaterCaseTracking.Models.ViewModels.MCAsk;
using WaterCaseTracking.Service;

namespace WaterCaseTracking.Controllers
{
    public class MCAskController : BaseController
    {
        string FuncName0 = "市政總質詢事項";
        string FuncName1 = "議會案件";
        // GET: MCAsk
        public ActionResult Maintain0()
        {
            logging(FuncName0, "進入功能-" + FuncName0);

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
                //抓科室下拉選單
                searchInitViewModel.ddlOrganizer = dropDownListService.getddlOrganizer();
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

        #region 取得表單oTable資料-起
        public ActionResult GetoTable(SearchInfoViewModel searchInfo)
        {
            logging(FuncName0, "取得表單資料");
            #region 參數宣告

            SearchListViewModel searchList = new SearchListViewModel();
            MCAskService mcaskService = new MCAskService();
            #endregion

            #region 流程	

            try
            {
                //送參數進入Service層做商業邏輯
                searchList = mcaskService.QuerySearchList(searchInfo);
            }
            catch (Exception ex)
            {
                searchList.db_Result = "Fail , " + ex.Message;
                errLogging(FuncName0, ex.ToString());
            }
            //組Json格式回傳Models資料
            return Json(searchList, JsonRequestBehavior.AllowGet);
            #endregion

        }
        #endregion 取得表單oTable資料-迄

        #region 匯出範例檔-起
        [HttpPost]
        public ActionResult Export(ExportViewModel exportViewModel)
        {
            logging(FuncName0, "匯出範例檔");
            #region 參數宣告
            MCAskService mcaskService = new MCAskService();
            DataTable dt = new DataTable();
            string fileNamePath = "";
            #endregion

            #region 流程	
            try
            {
                //查詢修改資料
                dt = mcaskService.getExportData(exportViewModel);
                string Name = "";
                if (exportViewModel.Types == "0")
                {
                    Name = "市政總質詢事項";
                }
                else
                {
                    Name = "議會案件";
                }
                fileNamePath = ExportFunction.ExportDataTableTo(dt, exportViewModel.fileExtension, Name + "範例檔");
            }
            catch (Exception ex)
            {
                errLogging(FuncName0, ex.ToString());
            }
            return Json(fileNamePath, JsonRequestBehavior.AllowGet);

            #endregion

        }
        #endregion 匯出範例檔-迄

        public ActionResult Create0()
        {
            return View();
        }

        public ActionResult Edit()
        {
            return View();
        }
    }
}