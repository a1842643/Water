using WaterCaseTracking.Models.ViewModels.KPI_Manage;
using WaterCaseTracking.Models.ViewModels.KPI_Manage.Maintain;
using WaterCaseTracking.Service;
using WaterCaseTracking.Service.KPI_Manage.Maintain;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WaterCaseTracking.Controllers
{

    public class KPI_ManageController : BaseController
    {
        string FuncName = "管理績效";
        // GET: KPI_Manage
        public ActionResult Maintain()
        {
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
                //送參數進入Service層做商業邏輯
                searchItemList = maintainService.QueryTreeList(searchInfo);
            }
            catch (Exception ex)
            {
                searchItemList.db_Result = "Fail , " + ex.Message;
            }
            return Json(searchItemList.tree, JsonRequestBehavior.AllowGet);
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
                //送參數進入Service層做商業邏輯
                searchItemList = maintainService.GetItem(searchInfo);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Json(searchItemList.Item, JsonRequestBehavior.AllowGet);
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
                //送參數進入Service層做商業邏輯
                maintainService.PostItem(searchInfo);
                MsgStr = "成功";
            }
            catch (Exception ex)
            {
                MsgStr = "失敗 : " + ex.Message;
            }
            //組Json格式回傳Models資料
            return Json(MsgStr, JsonRequestBehavior.AllowGet);
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
                //送參數進入Service層做商業邏輯
                searchItemList = maintainService.DelItem(searchInfo);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Json(searchItemList.Item, JsonRequestBehavior.AllowGet);
            #endregion
        }
        #endregion

        #region 下載匯入管理績效格式
        public ActionResult Export()
        {
            #region 參數宣告
            DataTable dt = new DataTable();
            MaintainService maintainService = new MaintainService();
            string fileNamePath = "";
            #endregion

            #region 流程
            try
            {
                logging(FuncName, "下載匯入管理績效格式");
                //送參數進入Service層做商業邏輯
                dt = maintainService.getKPIM();

                fileNamePath = ExportFunction.ExportDataTableTo(dt, "xlsx", "KPI_Manage");
            }
            catch (Exception ex)
            {
                errLogging(FuncName, ex.ToString());
            }
            return Json(fileNamePath, JsonRequestBehavior.AllowGet);
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
                logging(FuncName, "上傳");
                if (Request.Files.AllKeys.Any())
                {
                    //## 讀取指定的上傳檔案ID
                    HttpPostedFileBase httpPostedFile = Request.Files["UploadedFile"];
                    string fileName = httpPostedFile.FileName;
                    //取得副檔名
                    string FileExtension = System.IO.Path.GetExtension(fileName);
                    if (FileExtension != ".xlsx")
                        throw new Exception("匯入檔案錯誤");
                    successQty = maintainService.doUpLoad(httpPostedFile, "");


                }

                return Json("成功匯入" + successQty + "筆");
            }
            catch (Exception ex)
            {
                errLogging(FuncName, ex.ToString());
                return Json(ex.Message);
            }
        }

        public JsonResult UpLoadApply()
        {
            try
            {
                #region 參數宣告
                MaintainService maintainService = new MaintainService();
                int successQty = 0;
                #endregion
                //## 如果有任何檔案類型才做
                logging(FuncName, "上傳");
                if (Request.Files.AllKeys.Any())
                {
                    //## 讀取指定的上傳檔案ID
                    HttpPostedFileBase httpPostedFile = Request.Files["UploadedFile"];
                    string fileName = httpPostedFile.FileName;
                    //取得副檔名
                    string FileExtension = System.IO.Path.GetExtension(fileName);
                    if (FileExtension != ".xlsx")
                        throw new Exception("匯入檔案錯誤");
                    successQty = maintainService.doUpLoadApply(httpPostedFile, NewID(), FuncName, UserID, UserName);


                }

                return Json("成功申請上傳");
            }
            catch (Exception ex)
            {
                errLogging(FuncName, ex.ToString());
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
                logging(FuncName, "匯入去年資料");
                //送參數進入Service層做商業邏輯
                maintainService.ImportLastYear();
            }
            catch (Exception ex)
            {
                errLogging(FuncName, ex.ToString());
            }
            return Json("", JsonRequestBehavior.AllowGet);
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
                logging(FuncName, "鎖定");
                //送參數進入Service層做商業邏輯
                maintainService.doLock(isLocked);
                
                return Json("", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                errLogging(FuncName, ex.ToString());
                throw;
            }
            #endregion
        }
        #endregion

    }
}