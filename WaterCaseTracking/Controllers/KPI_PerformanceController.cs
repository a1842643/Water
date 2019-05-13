using WaterCaseTracking.Models.ViewModels.KPI_Performance.Maintain;
using WaterCaseTracking.Models.ViewModels.KPI_Performance;
using WaterCaseTracking.Service.KPI_Performance.Maintain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using System.Data;
using WaterCaseTracking.Service;

namespace WaterCaseTracking.Controllers
{
    public class KPI_PerformanceController : BaseController
    {
        string funcName = "經營績效考核";
        string funcNameDelete = "業績考核-刪除";
        string funcNameModify = "業績考核-修改";

        // GET: KPI_Performance
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
            MaintainService maintainService = new MaintainService();
            #endregion

            #region 流程	

            try
            {
                //抓是否已經Locked
                searchInitViewModel.LOCKED = maintainService.getIsLocked();
                //抓地區下拉選單
                searchInitViewModel.ddlKPIPer = dropDownListService.getddlKPIPer();
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
        #region 下載匯入業績績效格式
        public ActionResult Export(string ddlKPIPerVal, string ddlKPIPerText)
        {
            #region 參數宣告
            DataTable dt = new DataTable();
            MaintainService maintainService = new MaintainService();
            string fileNamePath = "";
            #endregion

            #region 流程
            try
            {
                logging(funcName, "下載匯入" + ddlKPIPerText + "格式");
                //送參數進入Service層做商業邏輯
                dt = maintainService.getKPI_Per(ddlKPIPerVal);

                fileNamePath = ExportFunction.ExportDataTableTo(dt, "xlsx", ddlKPIPerText);
            }
            catch (Exception ex)
            {
                errLogging(funcName, ex.ToString());
            }
            return Json(fileNamePath, JsonRequestBehavior.AllowGet);
            #endregion
        }
        #endregion

        #region 鎖定
        public ActionResult ToLock(string isLocked)
        {
            #region 參數宣告
            MaintainService maintainService = new MaintainService();
            string fileNamePath = "";
            #endregion

            #region 流程
            try
            {
                logging(funcName, "鎖定");
                //送參數進入Service層做商業邏輯
                maintainService.doLock(isLocked);

                return Json("", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                errLogging(funcName, ex.ToString());
                throw;
            }
            #endregion
        }
        #endregion
        [HttpPost]
        public JsonResult Upload()
        {
            try
            {
                #region 參數宣告
                MaintainService maintainService = new MaintainService();
                int successQty = 0;
                #endregion
                //## 如果有任何檔案類型才做
                logging(funcName, "上傳");
                if (Request.Files.AllKeys.Any())
                {
                    //## 讀取指定的上傳檔案ID
                    HttpPostedFileBase httpPostedFile = Request.Files["UploadedFile"];
                    string ddlKPIPerVal  = Request["ddlKPIPerVal"];
                    string fileName = httpPostedFile.FileName;
                    //取得副檔名
                    string FileExtension = System.IO.Path.GetExtension(fileName);
                    if (FileExtension != ".xlsx")
                        throw new Exception("匯入檔案錯誤");
                    //successQty = maintainService.doUpLoad(httpPostedFile, "", ddlKPIPerVal);


                }

                return Json("成功匯入" + successQty + "筆");
            }
            catch (Exception ex)
            {
                errLogging(funcName, ex.ToString());
                return Json(ex.Message);
            }
        }

        #region 匯入去年資料
        public ActionResult ImportLastYear()
        {
            #region 參數宣告
            MaintainService maintainService = new MaintainService();
            #endregion

            #region 流程
            try
            {
                logging(funcName, "匯入去年資料");
                //送參數進入Service層做商業邏輯
                maintainService.ImportLastYear();
            }
            catch (Exception ex)
            {
                errLogging(funcName, ex.ToString());
            }
            return Json("", JsonRequestBehavior.AllowGet);
            #endregion
        }
        #endregion

        public JsonResult UpLoadApply()
        {
            try
            {
                #region 參數宣告
                MaintainService maintainService = new MaintainService();
                int successQty = 0;
                #endregion
                //## 如果有任何檔案類型才做
                logging(funcName, "上傳");
                if (Request.Files.AllKeys.Any())
                {
                    //## 讀取指定的上傳檔案ID
                    HttpPostedFileBase httpPostedFile = Request.Files["UploadedFile"];
                    string ddlKPIPerVal = Request["ddlKPIPerVal"];
                    string fileName = httpPostedFile.FileName;
                    //取得副檔名
                    string FileExtension = System.IO.Path.GetExtension(fileName);
                    if (FileExtension != ".xlsx")
                        throw new Exception("匯入檔案錯誤");
                    successQty = maintainService.doUpLoad(httpPostedFile, NewID(), funcName, UserID, UserName, ddlKPIPerVal);


                }

                return Json("成功申請上傳");
            }
            catch (Exception ex)
            {
                errLogging(funcName, ex.ToString());
                return Json(ex.Message);
            }
        }

        #region 次項列表
        public ActionResult SecondaryItemJson(SearchInfoViewMode searchInfo)
        {
            #region 參數宣告

            SearchListViewMode searchItemList = new SearchListViewMode();
            MaintainService maintainService = new MaintainService();
            #endregion

            #region 流程	

            try
            {
                //送參數進入Service層做商業邏輯
                searchItemList = maintainService.QuerySearchList(searchInfo);
            }
            catch (Exception ex)
            {
                searchItemList.db_Result = "Fail , " + ex.Message;
            }
            //組Json格式回傳Models資料
            return Json(searchItemList, JsonRequestBehavior.AllowGet);
            #endregion
        }
        #endregion

        #region 查詢項目樹
        public ActionResult QueryTreeList(SearchInfoViewMode searchInfo)
        {
            #region 參數宣告

            SearchListViewMode searchItemList = new SearchListViewMode();
            MaintainService maintainService = new MaintainService();
            #endregion

            #region 流程	

            try
            {
                logging(funcName, "查詢系統樹");
                //送參數進入Service層做商業邏輯
                searchItemList = maintainService.QueryTreeList(searchInfo);
            }
            catch (Exception ex)
            {
                searchItemList.db_Result = "Fail , " + ex.Message;
                errLogging(funcName, ex.ToString());
            }
            
            return Json(searchItemList.tree, JsonRequestBehavior.AllowGet);
            #endregion
        }
        #endregion

        #region 設定項目資料
        public ActionResult SetItem(SearchItemViewMode searchInfo)
        {
            #region 參數宣告
            string MsgStr;
            MaintainService maintainService = new MaintainService();
            searchInfo.user = UserID;
            #endregion

            #region 流程	

            try
            {
                logging(funcName, "設定-" + funcNameModify);
                //送參數進入Service層做商業邏輯
                maintainService.PostItem(searchInfo);
                MsgStr = "成功";
            }
            catch (Exception ex)
            {
                errLogging(funcName, ex.ToString());
                MsgStr = "失敗 : " + ex.Message;
            }
            //組Json格式回傳Models資料
            return Json(MsgStr, JsonRequestBehavior.AllowGet);
            #endregion
        }
        #endregion

        #region 取得項目資料
        public ActionResult getitem(SearchItemViewMode searchInfo)
        {
            #region 參數宣告

            SearchItemViewMode searchItemList = new SearchItemViewMode();
            MaintainService maintainService = new MaintainService();
            #endregion

            #region 流程
            try
            {
                logging(funcName, "查詢子項目");
                //送參數進入Service層做商業邏輯
                searchItemList = maintainService.GetItem(searchInfo);
            }
            catch (Exception ex)
            {
                errLogging(funcName, ex.ToString());
                throw ex;
            }
            return Json(searchItemList.Item, JsonRequestBehavior.AllowGet);
            #endregion
        }
        #endregion

        #region 刪除項目
        public ActionResult DelItem(SearchItemViewMode searchInfo)
        {
            #region 參數宣告

            SearchItemViewMode searchItemList = new SearchItemViewMode();
            MaintainService maintainService = new MaintainService();
            #endregion

            #region 流程
            try
            {
                logging(funcName, funcNameDelete);
                //送參數進入Service層做商業邏輯
                searchItemList = maintainService.DelItem(searchInfo);
            }
            catch (Exception ex)
            {
                errLogging(funcName, ex.ToString());
                throw ex;
            }
            return Json(searchItemList.Item, JsonRequestBehavior.AllowGet);
            #endregion
        }
        #endregion
    }
}