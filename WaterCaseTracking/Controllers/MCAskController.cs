using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WaterCaseTracking.Models;
using WaterCaseTracking.Models.ViewModels.MCAsk;
using WaterCaseTracking.Service;

namespace WaterCaseTracking.Controllers
{
    public class MCAskController : BaseController
    {
        string FuncName0 = "市政總質詢事項";
        string FuncName0C = "市政總質詢事項-新增";
        string FuncName0M = "市政總質詢事項-修改";
        string FuncName0D = "市政總質詢事項-刪除";
        string FuncName1 = "議會案件";
        string FuncName1C = "議會案件-新增";
        string FuncName1M = "議會案件-修改";
        string FuncName1D = "議會案件-刪除";
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

        #region 新增-起
        public ActionResult Create0()
        {
            #region 參數宣告
            MCAskModel mcaskModel = new MCAskModel();
            string fail = "";
            #endregion
            logging(FuncName0C, "進入功能-" + FuncName0C);
            return View(mcaskModel);
        }

        [HttpPost]
        public ActionResult Create0(MCAskModel mcaskModel)
        {
            logging(FuncName0C, "新增");
            #region 驗證
            if (!ModelState.IsValid)
            { return View(mcaskModel); }
            #endregion
            #region 參數宣告
            MCAskService MCAskService = new MCAskService();
            string fail = "";
            #endregion

            #region 流程	
            try
            {
                //送參數進入Service層做商業邏輯
                MCAskService.AddMCAskTable(mcaskModel, UserName);
            }
            catch (Exception ex)
            {
                fail = "Fail," + ex.Message;
                errLogging(FuncName0C, ex.ToString());
            }
            //成功的話返回主頁
            if (string.IsNullOrEmpty(fail))
            {
                TempData["message"] = "新增成功";
                return RedirectToAction("Maintain0");
            }
            else
            {
                TempData["message"] = fail;
            }

            return View(mcaskModel);
            #endregion
        }
        #endregion 新增-迄

        #region 修改-起
        public ActionResult Edit0(string ID)
        {
            logging(FuncName0M, "進入功能-" + FuncName0M);
            #region 參數宣告
            MCAskService mcaskService = new MCAskService();
            MCAskModel mcaskModel = new MCAskModel();
            string fail = "";
            #endregion

            #region 流程	
            try
            {
                //查詢有無資料
                mcaskModel = mcaskService.QueryUpdateData(ID, "0");
            }
            catch (Exception ex)
            {
                fail = "Fail," + ex.Message;
                errLogging(FuncName0M, ex.ToString());
            }
            #endregion

            return View("Edit0", mcaskModel);
        }

        [HttpPost]
        public ActionResult Edit0(MCAskModel mcaskModel)
        {
            logging(FuncName0M, "修改");
            #region 驗證
            if (!ModelState.IsValid)
            { return View(mcaskModel); }
            #endregion
            #region 參數宣告
            MCAskService MCAskService = new MCAskService();
            string fail = "";
            #endregion

            #region 流程	
            try
            {
                //送參數進入Service層做商業邏輯
                MCAskService.UpdateMCAskTable(mcaskModel, UserName);
            }
            catch (Exception ex)
            {
                fail = "Fail," + ex.Message;
                errLogging(FuncName0M, ex.ToString());
            }
            //成功的話返回主頁
            if (string.IsNullOrEmpty(fail))
            {
                TempData["message"] = "修改成功";
                return RedirectToAction("Maintain0");
            }
            else
            {
                TempData["message"] = fail;
            }

            return View(mcaskModel);
            #endregion
        }
        #endregion 修改-迄

        #region 刪除-起
        public ActionResult Delete(string ID)
        {
            logging(FuncName0D, "刪除");
            #region 參數宣告
            MCAskService MCAskService = new MCAskService();
            string fail = "";
            #endregion

            #region 流程	
            try
            {
                //送參數進入Service層做商業邏輯
                MCAskService.DeleteMCAskTable(ID, "0", UserName);
            }
            catch (Exception ex)
            {
                fail = "Fail," + ex.Message;
                errLogging(FuncName0D, ex.ToString());
            }
            //成功的話返回主頁
            if (string.IsNullOrEmpty(fail))
            {
                TempData["message"] = "刪除成功";
                return RedirectToAction("Maintain0");
            }
            else
            {
                TempData["message"] = fail;
            }

            return View();
            #endregion
        }
#endregion 刪除-迄



        #region 新增修改初始化-起
        [HttpPost]
        public ActionResult searchInitCM()
        {

            #region 參數宣告
            DropDownListService dropDownListService = new DropDownListService();
            SearchInitCMViewModel searchInitCMViewModel = new SearchInitCMViewModel();
            #endregion

            #region 流程	

            try
            {
                //抓地區下拉選單
                searchInitCMViewModel.ddlArea = dropDownListService.getddlArea();
                //抓科室下拉選單
                searchInitCMViewModel.ddlsStatus = dropDownListService.getddlsStatus();
                //抓科室下拉選單
                searchInitCMViewModel.ddlOrganizer = dropDownListService.getddlOrganizer();
            }
            catch (Exception ex)
            {
                searchInitCMViewModel.db_Result = "Fail , " + ex.Message;
            }
            //組Json格式回傳Models資料
            return Json(searchInitCMViewModel, JsonRequestBehavior.AllowGet);
            #endregion

        }
        #endregion 新增初始化-迄
    }
}