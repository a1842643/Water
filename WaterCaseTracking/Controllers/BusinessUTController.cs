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
    public class BusinessUTController : BaseController
    {
        string funcName = "營業單位資料維護";
        string funcNameCreate = "營業單位資料維護-新增";
        string funcNameModify = "營業單位資料維護-修改";
        // GET: BusinessUT
        public ActionResult Maintain()
        {
            logging(funcName, "進入功能-" + funcName);
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

        public ActionResult GetoTable(SearchInfoViewModel searchInfo)
        {
            logging(funcName, "查詢");
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
                errLogging(funcName, ex.ToString());
            }
            //組Json格式回傳Models資料
            return Json(searchList, JsonRequestBehavior.AllowGet);
            #endregion

        }
        public ActionResult Create()
        {
            logging(funcNameCreate, "進入功能-" + funcNameCreate);
            return View();
        }
        [HttpPost]
        public ActionResult Create(UntilModel untilModel)
        {
            #region 驗證
            if (!ModelState.IsValid)
            { return View(); }
            #endregion
            logging(funcNameCreate, "新增");
            #region 參數宣告
            BusinessUTService businessUTService = new BusinessUTService();
            string fail = "";
            #endregion

            #region 流程	
            try
            {
                //送參數進入Service層做商業邏輯
                businessUTService.AddUntil(untilModel);
            }
            catch (Exception ex)
            {
                fail = "Fail," + ex.Message;
                errLogging(funcNameCreate, ex.ToString());
            }
            //成功的話返回主頁
            if (string.IsNullOrEmpty(fail))
            {
                TempData["message"] = "新增成功";
                return RedirectToAction("Maintain");
            }
            else
            {
                TempData["message"] = fail;
            }

            return View(untilModel);
            #endregion
        }
        public ActionResult Edit(string Branch_No)
        {
            logging(funcNameModify, "進入功能-" + funcNameModify);
            #region 參數宣告
            BusinessUTService businessUTService = new BusinessUTService();
            UntilModel untilModel = new UntilModel();
            #endregion

            #region 流程	
            try
            {
                //查詢修改資料
                untilModel = businessUTService.QueryUpdateData(Branch_No);
            }
            catch (Exception ex)
            {
                errLogging(funcNameModify, ex.ToString());
            }

            return View(untilModel);
            #endregion
        }
        [HttpPost]
        public ActionResult Edit(UntilModel untilModel)
        {
            #region 驗證
            if (!ModelState.IsValid)
            { return View(); }
            #endregion
            logging(funcNameModify, "新增");
            #region 參數宣告
            BusinessUTService businessUTService = new BusinessUTService();
            string fail = "";
            #endregion

            #region 流程	
            try
            {
                //送參數進入Service層做商業邏輯
                businessUTService.UpdateUntil(untilModel);
            }
            catch (Exception ex)
            {
                fail = "Fail," + ex.Message;
                errLogging(funcNameModify, ex.ToString());
            }
            //成功的話返回主頁
            if (string.IsNullOrEmpty(fail))
            {
                TempData["message"] = "修改成功";
                return RedirectToAction("Maintain");
            }
            else
            {
                TempData["errprMessage"] = fail;
            }
            return View(untilModel);
            #endregion
        }
        public ActionResult Delete(string Branch_No)
        {
            logging(funcName, "刪除");
            #region 參數宣告
            BusinessUTService businessUTService = new BusinessUTService();
            UntilModel untilModel = new UntilModel();
            string fail = "";
            #endregion

            #region 流程	
            try
            {
                //查詢修改資料
                businessUTService.DeleteUntil(Branch_No);
            }
            catch (Exception ex)
            {
                fail = "Fail," + ex.Message;
                errLogging(funcName, ex.ToString());
            }
            //成功的話返回主頁
            if (string.IsNullOrEmpty(fail))
            {
                TempData["message"] = "刪除成功";
                return RedirectToAction("Maintain");
            }
            else
            {
                TempData["errprMessage"] = fail;
            }
            return View();
            #endregion
        }

        [HttpPost]
        public JsonResult Upload()
        {
            try
            {
                #region 參數宣告
                BusinessUTService businessUTService = new BusinessUTService();
                int successQty = 0;
                #endregion
                //## 如果有任何檔案類型才做
                if (Request.Files.AllKeys.Any())
                {
                    logging(funcName, "上傳");
                    //## 讀取指定的上傳檔案ID
                    HttpPostedFileBase httpPostedFile = Request.Files["UploadedFile"];
                    string fileName = httpPostedFile.FileName;
                    //取得副檔名
                    string FileExtension = System.IO.Path.GetExtension(fileName);
                    if (FileExtension != ".xlsx")
                        throw new Exception("匯入檔案錯誤");
                    successQty = businessUTService.doUpLoad(httpPostedFile);


                }

                return Json("成功匯入" + successQty + "筆");
            }
            catch (Exception ex)
            {
                errLogging(funcName, ex.ToString());
                return Json(ex.Message);
            }
        }
    }
}