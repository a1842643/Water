using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WaterCaseTracking.Models.ViewModels.SigningProcess;
using WaterCaseTracking.Service;

namespace WaterCaseTracking.Controllers
{
    public class SigningProcessController : BaseController {
        SearchItemViewModel SearchItemViewModel = new SearchItemViewModel();
        string FuncName = "簽核流程申請";
        #region 初始頁面
        // GET: SigningProcess
        public ActionResult Maintain() {
            return View();
        }
        //簽核處理
        public ActionResult SigningProcess() {
            return View();
        }
        //覆核處理
        public ActionResult ReviewProcess() {
            return View();
        }
        //申請單
        public ActionResult Application() {
            Default_ini();
            SigningQueryItemViewModel QueryViewModel = QueryItemsbyProID(SearchItemViewModel.ProID);
            if (QueryViewModel.data.Count == 0) { return View(SearchItemViewModel); }
            else {
                Url_ini(QueryViewModel);
                return View(QueryViewModel.data[0]);
            }
        }
        //申請單(查看專用)
        public ActionResult ReadSchedule() {
            Default_ini();
            SigningQueryItemViewModel QueryViewModel = QueryItemsbyProID(SearchItemViewModel.ProID);
            if (QueryViewModel.data.Count == 0) { return View(SearchItemViewModel); }
            else {
                Url_ini(QueryViewModel);
                return View(QueryViewModel.data[0]);
            }
        }
        //已結案申請單查詢
        public ActionResult QueryOver() {
            return View();
        }
        #endregion 初始頁面
        #region ini
        //初始化-參數寫入
        public void Default_ini() {
            SearchItemViewModel.ProID = Request.QueryString["ProID"]; //申請單編號
            if(!string.IsNullOrWhiteSpace(Request.QueryString["Status"]))
            SearchItemViewModel.Status = int.Parse(Request.QueryString["Status"]); //簽核種類

            //TEST,SSO回來要改掉的地方
            SearchItemViewModel.UserID = "090587"; //申請者ID
            SearchItemViewModel.UserName = "職員_王小明"; //申請者名稱
            SearchItemViewModel.SupervisorID = "094566"; //簽核主管ID
            SearchItemViewModel.SupervisorName = "長官_霸判官"; //簽核主管姓名
            SearchItemViewModel.ReviewSupervisorID = "086666"; //覆核主管ID
            SearchItemViewModel.ReviewSupervisorName = "最高級長官_啊哈"; //覆核主管姓名
        }
        #endregion ini
        #region Url_ini
        //Model補齊
        public void Url_ini(SigningQueryItemViewModel model) {
            //判斷階層
            if (model.data[0].Status == 1) {
                //這裡要補上Url_ID (自己
                model.data[0].SupervisorID = "094566"; //簽核主管ID
                model.data[0].ReviewSupervisorName = "長官_霸判官"; //簽核主管姓名
                //這裡要補上Url_upID (上司
                model.data[0].ReviewSupervisorID = "086666"; //覆核主管ID
                model.data[0].ReviewSupervisorName = "最高級長官_啊哈"; //覆核主管姓名
            }
            else if (model.data[0].Status == 2) {
                //這裡要補上Url_upID (自己
                model.data[0].ReviewSupervisorID = "086666"; //覆核主管ID
                model.data[0].ReviewSupervisorName = "最高級長官_啊哈"; //覆核主管姓名
            }
        }
        #endregion Url_ini

        #region 簽核流程處理單-起
        /// <summary>
        /// 簽核流程處理單
        /// </summary>
        /// <param name="searchInfo"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult searchInit(SigningInfoItemsViewModel model) {
            #region 參數宣告
            SigningProcessService SigningProcessService = new SigningProcessService();
            SigningQueryItemViewModel QueryItemsViewModel = new SigningQueryItemViewModel();
            #endregion

            #region 流程	

            try {
                //抓地區下拉選單
                QueryItemsViewModel = SigningProcessService.getQueryItem(model);
            }
            catch (Exception ex) {
                QueryItemsViewModel.db_Result = "Fail , " + ex.Message;
            }
            //組Json格式回傳Models資料
            return Json(QueryItemsViewModel, JsonRequestBehavior.AllowGet);
            #endregion

        }
        #endregion 簽核流程處理單-迄
        #region 申請單-申請
        /// <summary>
        /// 申請單-申請
        /// </summary>
        /// <param name="searchInfo"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult InsertItems(SigningInsertItemsViewModel searchInfo) {
            logging(FuncName, "申請");
            #region 參數宣告

            //SigningProcessItemViewModel searchList = new SigningProcessItemViewModel();
            SigningProcessService minuteLogService = new SigningProcessService();
            int ReturnNum = 0;//紀錄異動數量
            #endregion

            #region 流程	
            try {
                //送參數進入Service層做商業邏輯
                ReturnNum = minuteLogService.ApplicationInsert(searchInfo);
                //TempData["TempTable"] = searchList.data;
            }
            catch (Exception ex) {
                //searchList.db_Result = "Fail , " + ex.Message;
                errLogging(FuncName, ex.Message.ToString());
            }
            //組Json格式回傳Models資料
            return Json(new { data = ReturnNum });
            //return Json(searchList, JsonRequestBehavior.AllowGet);
            #endregion

        }
        #endregion 申請單-申請
        #region 申請單-簽核 & 覆核
        /// <summary>
        /// 申請單-覆核通過 & 簽核通過
        /// </summary>
        /// <param name="searchInfo"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ApplicationUpdate(SigningUpdateItemsViewModel model) {
            if(model.Status == 2)
                logging(FuncName, "覆核通過");
            else
                logging(FuncName, "簽核通過");
            model.ID = UserID;      //底層暫存身份ID
            model.Name = UserName;  //底層暫存身份Name1
            #region 參數宣告

            //SigningProcessItemViewModel searchList = new SigningProcessItemViewModel();
            SigningProcessService minuteLogService = new SigningProcessService();
            string returnStr = string.Empty;
            #endregion

            #region 流程	
            try {
                //送參數進入Service層做商業邏輯
                returnStr = minuteLogService.ApplicationUpdate(model,Groups);
                //TempData["TempTable"] = searchList.data;
            }
            catch (Exception ex) {
                //searchList.db_Result = "Fail , " + ex.Message;
                errLogging(FuncName, ex.Message.ToString());
            }
            //組Json格式回傳Models資料
            //if (model.Status == 2) {
            //    TempData["message"] = "覆核成功";
            //    return RedirectToAction("ReviewProcess");
            //}
            //else {
            //    TempData["message"] = "簽核成功";
            //    return RedirectToAction("SigningProcess");
            //}
            return Json(returnStr);
            //return Json(searchList, JsonRequestBehavior.AllowGet);
            #endregion

        }
        #endregion 申請單-簽核 & 覆核
        #region 申請單-駁回
        /// <summary>
        /// 申請單-駁回
        /// </summary>
        /// <param name="model">資料集合</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DeleteItems(SigningDeleteItemsViewModel model) {
                logging(FuncName, "駁回");

            #region 參數宣告

            //SigningProcessItemViewModel searchList = new SigningProcessItemViewModel();
            SigningProcessService minuteLogService = new SigningProcessService();
            int ReturnNum = 0;      //紀錄異動數量
            model.ID = UserID;      //帶入Base暫存ID
            model.Name = UserName;  //帶入Base暫存Name
            #endregion

            #region 流程	
            try {
                //送參數進入Service層做商業邏輯
                minuteLogService.ApplicationDelete(model);
                //刪除Temp_M...資料
            }
            catch (Exception ex) {
                //searchList.db_Result = "Fail , " + ex.Message;
                errLogging(FuncName, ex.Message.ToString());
            }
            //組Json格式回傳Models資料
            return Json(new { data = ReturnNum });
            //return Json(searchList, JsonRequestBehavior.AllowGet);
            #endregion

        }
        #endregion 申請單-駁回
        #region 工作進度追蹤-查詢
        /// <summary>
        /// 查詢
        /// </summary>
        /// <param name="searchInfo"></param>
        /// <returns></returns>
        public ActionResult GetoTable(SigningInfoItemsViewModel searchInfo) {
            logging(FuncName, "查詢");

            #region 參數宣告

            SigningQueryItemViewModel searchList = new SigningQueryItemViewModel();
            SigningProcessService SigningProcessService = new SigningProcessService();
            #endregion
            searchInfo._ID = UserID;
            #region 流程	
            try {
                //送參數進入Service層做商業邏輯
                searchList = SigningProcessService.QuerySearchList(searchInfo);
                TempData["TempTable"] = searchList.data;
            }
            catch (Exception ex) {
                searchList.db_Result = "Fail , " + ex.Message;
                errLogging(FuncName, ex.ToString());
            }
            //組Json格式回傳Models資料
            return Json(searchList, JsonRequestBehavior.AllowGet);
            #endregion
        }
        #endregion 查詢

        #region 查詢單筆
        /// <summary>
        /// 查詢單筆
        /// </summary>
        /// <param name="searchInfo"></param>
        /// <returns></returns>
        public SigningQueryItemViewModel QueryItemsbyProID(string ProID) {
            logging(FuncName, "查詢");
            #region 參數宣告

            SigningQueryItemViewModel searchList = new SigningQueryItemViewModel();
            SigningProcessService SigningProcessService = new SigningProcessService();
            #endregion

            #region 流程	
            try {
                //送參數進入Service層做商業邏輯
                searchList = SigningProcessService.QueryItemsbyProID(ProID);
                //TempData["TempTable"] = searchList.data;
            }
            catch (Exception ex) {
                //searchList.db_Result = "Fail , " + ex.Message;
                errLogging(FuncName, ex.ToString());
            }
            //組Json格式回傳Models資料
            return searchList;
            #endregion

        }
        #endregion 查詢單筆

        #region 上呈
        /// <summary>
        /// 上呈
        /// </summary>
        /// <param name="searchInfo"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GiveIn(SigningUpdateItemsViewModel model) {
            logging(FuncName, "上呈");
            #region 參數宣告

            //SigningProcessItemViewModel searchList = new SigningProcessItemViewModel();
            SigningProcessService minuteLogService = new SigningProcessService();
            int ReturnNum = 0;//紀錄異動數量
            //帶入使用者資訊
            model.UserID = UserID;
            model.UserName = UserName;
            #endregion

            #region 流程	
            try {
                //送參數進入Service層做商業邏輯
                ReturnNum = minuteLogService.GiveIn(model);
                //TempData["TempTable"] = searchList.data;
            }
            catch (Exception ex) {
                //searchList.db_Result = "Fail , " + ex.Message;
                errLogging(FuncName, ex.Message.ToString());
                //TempData["message"] = "上呈失敗";
                //return RedirectToAction("SigningProcess");
            }
            //TempData["message"] = "上呈成功";
            //return RedirectToAction("SigningProcess");
            //組Json格式回傳Models資料
            return Json(new { data = ReturnNum });
            //return Json(searchList, JsonRequestBehavior.AllowGet);
            #endregion

        }
        #endregion 上呈
        
    }
}