using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WaterCaseTracking.Models;
using WaterCaseTracking.Models.ViewModels.ProjectControll;
using WaterCaseTracking.Service;

namespace WaterCaseTracking.Controllers
{
    [Authorize]
    public class ProjectControllController : BaseController
    {
        string FuncName0 = "工程列管";
        string FuncName0C = "工程列管-新增";
        string FuncName0M = "工程列管-修改";
        string FuncName0D = "工程列管-刪除";
        // GET: ProjectControll
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
            ProjectControllService projectControllService = new ProjectControllService();
            #endregion

            #region 流程	

            try
            {
                //送參數進入Service層做商業邏輯
                searchList = projectControllService.QuerySearchList(searchInfo, UserName, roleName, Organizer);
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
            ProjectControllService projectControllService = new ProjectControllService();
            DataTable dt = new DataTable();
            string fileNamePath = "";
            #endregion

            #region 流程	
            try
            {
                //查詢修改資料
                dt = projectControllService.getExportData(exportViewModel, UserName, roleName, Organizer);
                string Name = "";
                Name = "預計發包案件";
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
            ProjectControllModel projectControllModel = new ProjectControllModel();
            string fail = "";
            #endregion
            logging(FuncName0C, "進入功能-" + FuncName0C);
            return View(projectControllModel);
        }

        [HttpPost]
        public ActionResult Create0(ProjectControllModel projectControllModel)
        {
            logging(FuncName0C, "新增");
            #region 驗證
            if (!ModelState.IsValid)
            { return View(projectControllModel); }
            projectControllModel.ContractAmount = Convert.ToDecimal(projectControllModel.SContractAmount);
            #endregion
            #region 參數宣告
            ProjectControllService projectControllService = new ProjectControllService();
            string fail = "";
            #endregion

            #region 流程	
            try
            {
                //送參數進入Service層做商業邏輯
                projectControllService.AddProjectControllTable(projectControllModel, UserName);
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

            return View(projectControllModel);
            #endregion
        }
        #endregion 新增-迄

        #region 修改-起
        public ActionResult Edit0(string ID)
        {
            logging(FuncName0M, "進入功能-" + FuncName0M);
            #region 參數宣告
            ProjectControllService projectControllService = new ProjectControllService();
            ProjectControllModel projectControllModel = new ProjectControllModel();
            string fail = "";
            #endregion

            #region 流程	
            try
            {
                //查詢有無資料
                projectControllModel = projectControllService.QueryUpdateData(ID);
            }
            catch (Exception ex)
            {
                fail = "Fail," + ex.Message;
                errLogging(FuncName0M, ex.ToString());
            }
            #endregion

            return View("Edit0", projectControllModel);
        }

        [HttpPost]
        public ActionResult Edit0(ProjectControllModel projectControllModel)
        {
            logging(FuncName0M, "修改");
            #region 驗證
            if (!ModelState.IsValid)
            { return View(projectControllModel); }
            projectControllModel.ContractAmount = Convert.ToDecimal(projectControllModel.SContractAmount);
            #endregion
            #region 參數宣告
            ProjectControllService projectControllService = new ProjectControllService();
            string fail = "";
            #endregion

            #region 流程	
            try
            {
                //送參數進入Service層做商業邏輯
                projectControllService.UpdateProjectControllTable(projectControllModel, UserName);
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

            return View(projectControllModel);
            #endregion
        }
        #endregion 修改-迄

        #region 刪除-起
        public ActionResult Delete0(string ID)
        {
            logging(FuncName0D, "刪除");
            #region 參數宣告
            ProjectControllService projectControllService = new ProjectControllService();
            string fail = "";
            #endregion

            #region 流程	
            try
            {
                //送參數進入Service層做商業邏輯
                projectControllService.DeleteProjectControllTable(ID);
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

        #region 匯入
        [HttpPost]
        public JsonResult Upload0()
        {
            try
            {
                #region 參數宣告
                ProjectControllService projectControllService = new ProjectControllService();
                int successQty = 0;
                #endregion
                //## 如果有任何檔案類型才做
                if (Request.Files.AllKeys.Any())
                {
                    logging(FuncName0, "上傳");
                    //## 讀取指定的上傳檔案ID
                    HttpPostedFileBase httpPostedFile = Request.Files["UploadedFile"];
                    string fileName = httpPostedFile.FileName;
                    //取得副檔名
                    string FileExtension = System.IO.Path.GetExtension(fileName);
                    if (FileExtension != ".xlsx")
                        throw new Exception("匯入檔案錯誤");
                    successQty = projectControllService.doUpLoad(httpPostedFile, UserName);


                }

                return Json("匯入成功");
            }
            catch (Exception ex)
            {
                errLogging(FuncName0, ex.ToString());
                return Json(ex.Message);
            }
        }

        #endregion
    }
}