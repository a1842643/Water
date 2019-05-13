using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WaterCaseTracking.Models;
using WaterCaseTracking.Models.ViewModels.BatchManage;
using WaterCaseTracking.Service;

namespace WaterCaseTracking.Controllers
{
    public class BatchLoggerController : BaseController
    {
        string funcName = "批次作業明細";
        public ActionResult BatchLogger()
        {
            logging(funcName, "查詢Log");
            #region 驗證
            if (!ModelState.IsValid)
            { return View(); }
            #endregion
            #region 參數宣告 
            BatchLoggerModel searchList = new BatchLoggerModel();
            BatchManageService batchManageService = new BatchManageService();

            BatchLoggerSearchItemModel SearchItem = new BatchLoggerSearchItemModel();
            SearchItem.YM = System.Web.HttpContext.Current.Request.Params["YM"];
            SearchItem.JobID = System.Web.HttpContext.Current.Request.Params["JobID"];
            SearchItem.FuncN = System.Web.HttpContext.Current.Request.Params["FuncN"];
            #endregion

            #region 流程	
            try
            {
                //送參數進入Service層做商業邏輯
                searchList = batchManageService.QuerySearchLog(SearchItem);
            }
            catch (Exception ex)
            {
                searchList.db_Result = "Fail , " + ex.Message;
                errLogging(funcName, ex.ToString());
            }
            #endregion
            return View(searchList);//Json(searchList, JsonRequestBehavior.AllowGet); //組Json格式回傳Models資料
        }

    }
}