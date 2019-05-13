using WaterCaseTracking.Models.ViewModels.MinuteLog;
using WaterCaseTracking.Service;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WaterCaseTracking.Controllers {
    public class MinuteLogController : BaseController {
        string FuncName = "操作紀錄查詢";
        // GET: MinuteLog
        public ActionResult Maintain() {
            logging(FuncName, "進入功能-" + FuncName);
            return View();
        }
        #region 初始化-起
        [HttpPost]
        public ActionResult searchInit() {

            #region 參數宣告
            DropDownListService dropDownListService = new DropDownListService();
            SearchConditionViewModel SearchConditionViewModel = new SearchConditionViewModel();
            #endregion

            #region 流程	

            try {
                //抓操作人員下拉選單
                SearchConditionViewModel.ddlUserName = dropDownListService.getddlUserName();
                //抓功能下拉選單
                SearchConditionViewModel.ddlFuncName = dropDownListService.getddlFuncName();
                //抓狀態下拉選單
                SearchConditionViewModel.ddlStatus = dropDownListService.getddlStatus();
            }
            catch (Exception ex) {
                SearchConditionViewModel.db_Result = "Fail , " + ex.Message;
            }
            //組Json格式回傳Models資料
            return Json(SearchConditionViewModel, JsonRequestBehavior.AllowGet);
            #endregion

        }
        #endregion 初始化-迄

        /// <summary>
        /// 查詢
        /// </summary>
        /// <param name="searchInfo"></param>
        /// <returns></returns>
        public ActionResult GetoTable(QueryGetoTableViewModel searchInfo) {
            logging(FuncName, "查詢");
            #region 參數宣告

            MinuteLogItemViewModel searchList = new MinuteLogItemViewModel();
            MinuteLogService minuteLogService = new MinuteLogService();
            #endregion

            #region 流程	
            try {
                //送參數進入Service層做商業邏輯
                searchList = minuteLogService.QuerySearchList(searchInfo);
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

        #region 產表-起
        [HttpPost]
        public ActionResult Export(string fileExtension) {
            logging(FuncName, "匯出");
            #region 參數宣告
            string fileNamePath = "";
            //建立一個回傳用的 DataTable
            DataTable dt = new DataTable();
            #endregion
            List<SearchItemViewModel> _ListModel = (List<SearchItemViewModel>)TempData["TempTable"];

            #region 流程	
            try {
                string[] formatColumns = { "使用者ID", "使用者名稱", "功能名稱", "狀態", "訊息", "紀錄時間" };
                //定義表頭
                foreach (var item1 in formatColumns) {
                    dt.Columns.Add(item1);
                }
                //在 DataTable 中建立一個新的列

                //將資料足筆新增到 DataTable 中
                foreach (var item2 in _ListModel) {
                    DataRow dr = dt.NewRow();
                    dr[0] = item2.UserID;
                    dr[1] = item2.UserName;
                    dr[2] = item2.FuncName;
                    dr[3] = item2.Status;
                    dr[4] = item2.Message;
                    dr[5] = item2.logged;
                    dt.Rows.Add(dr);
                }


                fileNamePath = ExportFunction.ExportDataTableTo(dt, fileExtension, "MinuteLogExport");
            }
            catch (Exception ex) {
                errLogging(FuncName, ex.ToString());
            }
            return Json(fileNamePath, JsonRequestBehavior.AllowGet);

            #endregion

        }
        #endregion 產表-迄

        //#region 產表-起
        //[HttpPost]
        //public ActionResult Export(ExportViewModel exportViewModel) {
        //    logging(FuncName, "匯出");
        //    #region 參數宣告
        //    BusinessUTService businessUTService = new BusinessUTService();
        //    DataTable dt = new DataTable();
        //    string fileNamePath = "";
        //    #endregion

        //    #region 流程	
        //    try {
        //        //查詢修改資料
        //        dt = businessUTService.getExportData(exportViewModel);

        //        fileNamePath = ExportFunction.ExportDataTableTo(dt, exportViewModel.fileExtension, "BussinessUTQuery");
        //    }
        //    catch (Exception ex) {
        //        errLogging(FuncName, ex.ToString());
        //    }
        //    return Json(fileNamePath, JsonRequestBehavior.AllowGet);

        //    #endregion

        //}
        //#endregion 產表-迄
    }
}