using System;
using System.Collections.Generic;
using WaterCaseTracking.Models;
using WaterCaseTracking.Dao;
using WaterCaseTracking.Models.ViewModels.BatchManage;
using WaterCaseTracking.Service;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using WaterCaseTracking;

namespace WaterCaseTracking.Controllers
{
    public class BatchManageController : BaseController
    {
        string funcName = "批次作業管理";
        // GET: BatchManage
        public ActionResult BatchManage()
        {
            logging(funcName, "進入功能-" + funcName);
            return View();
        }

        #region 初始化-起
        [HttpPost]
        public ActionResult InitBatchManage()
        {

            #region 參數宣告
            DropDownListService dropDownListService = new DropDownListService();
            BatchManageInitUIViewModel batchManageInitUIViewModel = new BatchManageInitUIViewModel();

            string strJobID = System.Web.HttpContext.Current.Request.Params["JobID"];
            #endregion

            #region 流程	
            try
            {
                batchManageInitUIViewModel.ddlLog_FuncName = dropDownListService.getddlLog_FuncName(strJobID);
            }
            catch (Exception ex)
            {
                batchManageInitUIViewModel.db_Result = "Fail , " + ex.Message;
            }
            //組Json格式回傳Models資料
            return Json(batchManageInitUIViewModel, JsonRequestBehavior.AllowGet);
            #endregion

        }
        #endregion 初始化-迄
        public ActionResult QueryTable(BatchManageSearchItemModel SearchItem)
        {
            logging(funcName, "查詢");
            #region 驗證
            if (!ModelState.IsValid)
            { return View(); }
            #endregion
            #region 參數宣告 
            BatchManageListViewModel searchList = new BatchManageListViewModel();
            BatchManageService batchManageService = new BatchManageService();

            #endregion

            #region 流程	
            try
            {
                //送參數進入Service層做商業邏輯
                searchList = batchManageService.QuerySearchList(SearchItem);
            }
            catch (Exception ex)
            {
                searchList.db_Result = "Fail , " + ex.Message;
                errLogging(funcName, ex.ToString());
            }
            #endregion
            
            //組Json格式回傳Models資料
            return Json(searchList, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 新增或更新，1080418 本動作已修改為 已申請 Log_Status= 5，再由簽核端 改為等待執行=3
        /// Log_Status 失敗 = 0,成功 = 1,中斷 = 2,等待執行 = 3,執行中 = 4,已申請 = 5
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Execute()
        {
            #region 驗證
            if (!ModelState.IsValid)
            { return View(); }
            #endregion
            logging(funcName, "執行");
            
            #region 參數宣告 
            BatchManageService batchManageService = new BatchManageService();
            BatchManageItemModel BatchItem = new BatchManageItemModel();
            /*1080402 直接執行上月轉檔==>產出報表
            BatchItem.Log_YM = System.Web.HttpContext.Current.Request.Params["YM"];
            BatchItem.Log_JobID = System.Web.HttpContext.Current.Request.Params["JobID"];
            BatchItem.Log_JobName = System.Web.HttpContext.Current.Request.Params["JobName"];
            BatchItem.Log_FuncName = System.Web.HttpContext.Current.Request.Params["FuncN"];
            
            */
            
            int successQty = 0;

            #endregion
            //提交申請
            ManualUploadService manualUploadService = new ManualUploadService();
            manualUploadService.InsertSigningRecordByBatchManage("18", "批次管理執行申請", NewID(), UserID, UserName);
            #region 流程	
            
            try
            { 
                foreach (var jobs in BatchManageJobContent.getJobContent())
                {
                    List<Tuple<string, string>> jobContent = jobs.Value;
                    string[] jobsKind = jobs.Key.Split('-');
                    foreach (Tuple<string, string> job in jobContent)
                    {

                        DateTime doYYYYMM = DateTime.Now.AddMonths(-1);
                        string doYYYMM = (doYYYYMM.Year - 1911).ToString("000") + doYYYYMM.Month.ToString("00");
                        BatchItem.Log_YM = doYYYMM;

                        #region 此次執行已在批次排程內，採取update
                        bool hasDone = false;
                        BatchManageListViewModel searchList = new BatchManageListViewModel();
                        BatchManageSearchItemModel SearchItem = new BatchManageSearchItemModel();
                        SearchItem.txtLog_YM = doYYYMM;
                        SearchItem.ddlLog_JobID = jobsKind[0];
                        SearchItem.ddlLog_FuncName = job.Item1;
                        searchList = batchManageService.QuerySearchList(SearchItem);
                        if (searchList.recordsTotal > 0) hasDone = true;
                        #endregion

                        BatchItem.Log_JobID = jobsKind[0];
                        BatchItem.Log_JobName = jobsKind[1];
                        BatchItem.Log_FuncName = job.Item1;
                        BatchItem.Log_Description = job.Item2;
                        BatchItem.Log_ExRate = 0;
                        BatchItem.Log_StartTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
                        BatchItem.Log_UserID = UserID;
                        BatchItem.Log_Status = "5";

                        string LogTXT = "作業年月：" + doYYYMM;// + "  匯率：" + BatchItem.Log_ExRate.ToString();
                        LogTXT += "     排程作業類別：" + jobsKind[1] + "    作業項目：" + job.Item2 + Environment.NewLine;
                        LogTXT += "Log記錄時間：" + BatchItem.Log_StartTime + Environment.NewLine;
                        LogTXT += "操作者：" + BatchItem.Log_UserID + Environment.NewLine;
                        LogTXT += "作業開始時間：" + BatchItem.Log_StartTime + Environment.NewLine;
                        LogTXT += "作業結束時間：" + Environment.NewLine;
                        LogTXT += "執行狀態：等待執行" + Environment.NewLine;
                        LogTXT += "訊息：等待執行" + Environment.NewLine;
                        LogTXT += "===========================================================================================" + Environment.NewLine;

                        BatchItem.Log_Text = LogTXT;
                        
                        //送參數進入Service層做商業邏輯
                        if (!hasDone)
                        {
                            successQty += batchManageService.ExecuteBatch(BatchItem); //將參數送入Dao層,組立SQL字串並連接資料庫
                        }else
                        {
                            successQty += batchManageService.ExecuteBatchAgain(BatchItem); //將參數送入Dao層,組立SQL字串並連接資料庫
                        }
                    }
                    //
                }
                return Json("成功執行" + successQty + "筆");
            }
            catch (Exception ex)
            {
                errLogging(funcName, ex.ToString());
                return Json(ex.Message);
            }
            #endregion
        }

        /// <summary>
        /// 20190418  改一次執行，故不使用單次再次執行
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ExecuteAgain()
        {
            #region 驗證
            if (!ModelState.IsValid)
            { return View(); }
            #endregion
            logging(funcName, "重新執行");

            #region 參數宣告 
            BatchManageService batchManageService = new BatchManageService();
            BatchManageItemModel BatchItem = new BatchManageItemModel();
            BatchItem.Log_YM = System.Web.HttpContext.Current.Request.Params["YM"];
            BatchItem.Log_JobID = System.Web.HttpContext.Current.Request.Params["JobID"];
            BatchItem.Log_FuncName = System.Web.HttpContext.Current.Request.Params["FuncN"];
            string doAction= System.Web.HttpContext.Current.Request.Params["Status"];//   中斷 = 2 ,等待執行 = 3
            BatchItem.Log_StartTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            BatchItem.Log_UserID = UserID;
            #region 重新執行需在批次排程內
            BatchManageListViewModel searchList = new BatchManageListViewModel();
            BatchManageSearchItemModel SearchItem = new BatchManageSearchItemModel();
            SearchItem.txtLog_YM = BatchItem.Log_YM;
            SearchItem.ddlLog_JobID = BatchItem.Log_JobID;
            SearchItem.ddlLog_FuncName = BatchItem.Log_FuncName;
            searchList = batchManageService.QuerySearchList(SearchItem);
            
            if (searchList.recordsTotal <1)
            {
                return Json("操作項目不在批次排程內!");
            }
            
            BatchManageItemModel doItem = new BatchManageItemModel();
            doItem = searchList.data[0];

            if((doItem.Log_Status=="3" && doAction=="3"))//已重新執行
                return Json("該筆操作已在排程等待執行!");
            if ((doItem.Log_Status == "2" && doAction == "2")) //已中斷，想中斷
                return Json("該筆操作已在排程中斷!");
            if ((doItem.Log_Status == "1" && doAction == "2"))//執行完成，但想中斷
                return Json("該筆操作已成功完成，無法中斷!");
            if ((doItem.Log_Status == "0" && doAction == "2"))//失敗，但想中斷
                return Json("該筆操作，排程失敗，請重新執行!");
            #endregion

            BatchItem.Log_Status = doAction;
            string doActionName = "";
            if (doAction.Equals("3")) doActionName = "等待執行";
            else if (doAction.Equals("2")) doActionName = "中斷";

            string LogTXT = "作業年月：" + BatchItem.Log_YM;// + "  匯率：" + doItem.Log_ExRate.ToString();
            LogTXT += "     排程作業類別：" + doItem.Log_JobName + "   作業項目：" + doItem.Log_Description + Environment.NewLine;
            LogTXT += "Log記錄時間：" + BatchItem.Log_StartTime + Environment.NewLine;
            LogTXT += "操作者："  + Environment.NewLine;
            LogTXT += "作業開始時間：" + BatchItem.Log_StartTime + Environment.NewLine;
            LogTXT += "作業結束時間：" + Environment.NewLine;
            LogTXT += "執行狀態："+ doActionName + Environment.NewLine;
            LogTXT += "訊息："+ doActionName + Environment.NewLine;
            LogTXT += "===========================================================================================" + Environment.NewLine;

            BatchItem.Log_Text = LogTXT;

            #endregion

            #region 流程	
            try
            {
                int successQty = 0;
                //送參數進入Service層做商業邏輯
                successQty = batchManageService.ExecuteBatchAgain(BatchItem); //將參數送入Dao層,組立SQL字串並連接資料庫

                return Json("操作完成："+ doActionName + successQty + "筆");
            }
            catch (Exception ex)
            {
                errLogging(funcName, ex.ToString());
                return Json(ex.Message);
            }
            #endregion
        }
    }
}