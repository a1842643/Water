using WaterCaseTracking.Models;
using WaterCaseTracking.Models.ViewModels.JobReminder;
using WaterCaseTracking.Service;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WaterCaseTracking.Controllers
{
    public class JobReminderController : BaseController
    {
        string FuncName = "工作提醒設定(月執行)";
        // GET: Maintain
        public ActionResult Maintain() {
            logging(FuncName, "進入功能-" + FuncName);
            return View();
        }
        public ActionResult Query() {
            logging(FuncName, "進入查詢介面-" + FuncName);
            return View();
        }
        // GET: Create
        public ActionResult Create() {
            logging(FuncName, "進入新增頁面" + FuncName);
            SearchItemViewModel model = new SearchItemViewModel();
            //Detail();
            return View(model);
        }

        public void Detail() {
            //SearchItemViewModel.ActionGroups = new List<SelectListItem>()
            //{
            //    new SelectListItem {Text="管理績效得分上傳", Value="1"},
            //    new SelectListItem {Text="業績績效得分上傳", Value="2" },
            //    new SelectListItem {Text="人工上傳", Value="3" },
            //};
            //ViewBag.CategoryId = new List<items>()
            //{
            //    new items {Text="管理績效得分上傳", Value="1"},
            //    new items {Text="業績績效得分上傳", Value="2" },
            //    new items {Text="人工上傳", Value="3" },
            //};
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Create(SearchItemViewModel model) {
            #region 驗證

            if (!ModelState.IsValid) { return View(model); }
            #endregion
            logging(FuncName, "新增-" + FuncName);
            #region 參數宣告
            JobReminderService jobReminderService = new JobReminderService();
            string fail = "";
            string ErrMsg = string.Empty;
            #endregion
            string Groups = Session["Groups"].ToString();
            #region 流程	
            try {
                if (string.IsNullOrWhiteSpace(Groups))
                    return View(model);

                model.GroupsCode = Groups;
                model.YM = YM();
                //送參數進入Service層做商業邏輯
                ErrMsg = jobReminderService.Create(model);
                if (!string.IsNullOrWhiteSpace(ErrMsg)) {
                    errLogging(FuncName, ErrMsg);
                    fail = ErrMsg;
                }
            }
            catch (Exception ex) {
                fail = "Fail," + ex.Message;
                errLogging(FuncName, ex.ToString());
            }
            //成功的話返回主頁
            if (string.IsNullOrEmpty(fail)) {
                TempData["message"] = "新增成功";
                return RedirectToAction("Maintain");
            }
            else {
                TempData["message"] = fail;
            }

            return View(model);
            #endregion
        }

        /// <summary>
        /// 預設查詢
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult QueryTable(YMQueryViewModel model) {
            logging(FuncName, "查詢");
            #region 參數宣告
            QueryTableViewModel data = new QueryTableViewModel();
            JobReminderService jobReminderService = new JobReminderService();
            string GroupsCode = Session["Groups"].ToString();
            model.GroupsCode = GroupsCode;
            #endregion

            #region 流程	
            try {
                //送參數進入Service層做商業邏輯
                data = jobReminderService.QuerySearchList(model,YM());
            }
            catch (Exception ex) {
                data.db_Result = "Fail , " + ex.Message;
                errLogging(FuncName, ex.ToString());
            }
            //組Json格式回傳Models資料
            return Json(data, JsonRequestBehavior.AllowGet);
            #endregion

        }

        /// <summary>
        /// 搜尋年月(下拉清單)GroupByYM
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult GroupByYM(YMQueryViewModel model) {
            logging(FuncName, "查詢");
            #region 參數宣告
            JobReminderViewModel data = new JobReminderViewModel();
            JobReminderService jobReminderService = new JobReminderService();
            string GroupsCode = Session["Groups"].ToString();
            model.GroupsCode = GroupsCode;
            #endregion

            #region 流程	
            try {
                //送參數進入Service層做商業邏輯
                data = jobReminderService.GroupByYM(model);
            }
            catch (Exception ex) {
                data.db_Result = "Fail , " + ex.Message;
                errLogging(FuncName, ex.ToString());
            }
            //組Json格式回傳Models資料
            return Json(data, JsonRequestBehavior.AllowGet);
            #endregion

        }

        /// <summary>
        /// 工作提醒設定-判斷Insert JudgeInsert()
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult JudgeInsert() {
            logging(FuncName, "判斷當月,當月Insert控件");
            #region 參數宣告
            int ExecResult = 0;
            JobReminderService jobReminderService = new JobReminderService();
            JobReminderViewModel model = new JobReminderViewModel();
            string GroupsCode = Session["Groups"].ToString();
            #endregion

            #region 流程	
            try {
                //送參數進入Service層做商業邏輯
                model = jobReminderService.QueryJobReminder(GroupsCode); //找出某群組的設定清單(只取啟用中的)
                ExecResult = jobReminderService.JudgeInsert(model, YM(), GroupsCode); //將設定清單建立在當月工作項目
                if(ExecResult >= 0)
                    logging(FuncName, "JudgeInsert() Insert成功:" + ExecResult + " 筆");
            }
            catch (Exception ex) {
                errLogging(FuncName, ex.ToString());
                return Json(ex.Message);
            }
            //組Json格式回傳Models資料
            return Json(string.Empty);
            #endregion

        }
        /// <summary>
        /// 取上個月民國年
        /// </summary>
        /// <returns></returns>
        public string YM() {
            DateTime dt = DateTime.Now.AddMonths(0);
            CultureInfo culture = new CultureInfo("zh-TW");
            culture.DateTimeFormat.Calendar = new TaiwanCalendar();
            string _YM = dt.ToString("yyyMM", culture); //民國年月 YYYMM
            return _YM;
        }
        /// <summary>
        /// 查詢設定項目清單
        /// </summary>
        /// <returns></returns>
        public ActionResult QuerySetting() {
            logging(FuncName, "查詢設定項目清單");
            #region 參數宣告
            QueryTableViewModel data = new QueryTableViewModel();
            JobReminderService jobReminderService = new JobReminderService();
            string GroupsCode = Session["Groups"].ToString();
            #endregion

            #region 流程	
            try {
                //送參數進入Service層做商業邏輯
                data = jobReminderService.QuerySetting(GroupsCode);
            }
            catch (Exception ex) {
                data.db_Result = "Fail , " + ex.Message;
                errLogging(FuncName, ex.ToString());
            }
            //組Json格式回傳Models資料
            return Json(data, JsonRequestBehavior.AllowGet);
            #endregion
        }

        /// <summary>
        /// 撈取修改資料
        /// </summary>
        /// <param name="ActionCode"></param>
        /// <returns></returns>
        public ActionResult Update(string ActionCode) {
            logging(FuncName, "進入修改頁面" + FuncName);
            #region 參數宣告
            SearchItemViewModel searchItemViewModel = new SearchItemViewModel();
            JobReminderService jobReminderService = new JobReminderService();
            string GroupsCode = Session["Groups"].ToString(); //角色權限
            #endregion

            #region 流程	
            try {
                //查詢修改資料
                searchItemViewModel = jobReminderService.QueryUpdateData(GroupsCode,ActionCode);
            }
            catch (Exception ex) {
                errLogging(FuncName, ex.ToString());
            }

            return View(searchItemViewModel);
            #endregion
        }

        [HttpPost]
        public ActionResult Update(UpdateViewModel model) {
            #region 驗證
            if (!ModelState.IsValid) {
                SearchItemViewModel searchItemViewModel = new SearchItemViewModel();
                searchItemViewModel.ActionCode = model.ActionCode;
                searchItemViewModel.ActionName = model.ActionName;
                //ViewData["ActionName"] = model.ActionName;
                //ViewData["ActionCode"] = model.ActionCode;
                return View(searchItemViewModel);
            }
            #endregion
            logging(FuncName, "修改資料" + FuncName);
            #region 參數宣告
            JobReminderService jobReminderService = new JobReminderService();
            string GroupsCode = Session["Groups"].ToString(); //角色權限
            model.GroupsCode = GroupsCode;
            string fail = "";
            int ExecResult = 0;
            #endregion

            #region 流程	
            try {
                //送參數進入Service層做商業邏輯
                ExecResult = jobReminderService.Updatedata(model);

            }
            catch (Exception ex) {
                fail = "Fail," + ex.Message;
                errLogging(FuncName, ex.ToString());
            }
            //成功的話返回主頁
            if (string.IsNullOrEmpty(fail)) {
                TempData["message"] = "修改成功";
                return RedirectToAction("Maintain");
            }
            else {
                TempData["errprMessage"] = fail;
            }
            return View(model);
            #endregion
        }

        /// <summary>
        /// 工作功能開關切換
        /// </summary>
        /// <param name="ActionCode">功能</param>
        /// <returns></returns>
        public ActionResult isOnoff(string ActionCode,string IsOn) {
            logging(FuncName, "工作功能開關-" + FuncName);
            #region 參數宣告
            JobReminderService jobReminderService = new JobReminderService();
            UntilModel untilModel = new UntilModel();
            string fail = "";
            string GroupsCode = Session["Groups"].ToString();
            int ExecResult = 0;
            #endregion

            #region 流程	
            try {
                //查詢修改資料
                ExecResult = jobReminderService.isOnoff(GroupsCode,ActionCode, IsOn);
            }
            catch (Exception ex) {
                fail = "Fail," + ex.Message;
                errLogging(FuncName, ex.ToString());
            }
            //成功的話返回主頁
            if (string.IsNullOrEmpty(fail)) {
                //TempData["message"] = "功能開關切換成功";
                return RedirectToAction("Maintain");
            }
            else {
                TempData["errprMessage"] = fail;
            }
            return View();
            #endregion
        }

    }
}