using WaterCaseTracking.Models;
using WaterCaseTracking.Models.ViewModels.SystemPermission;
using WaterCaseTracking.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WaterCaseTracking.Controllers
{
    public class SystemPermissionController : BaseController
    {
        private string funcName = "角色權限設定";
        private string funcNameModify = "修改";

        // GET: SystemPermission
        public ActionResult SystemPermission()
        {
            logging(funcName, "進入功能-" + funcName);       
            return View();
        }

        #region 設定角色權限 SetPrograms()
        [ValidateAntiForgeryToken]
        public ActionResult SetPrograms(SearchInfoViewModel searchInfo)
        {
            #region 參數宣告
            string searchItemList = string.Empty;
            SystemPermissionService SystemPermissionService = new SystemPermissionService();
            searchInfo.MODIFY_USER = UserID;
            #endregion

            #region 流程
            try
            {
                logging(funcName, "角色權限設定-" + funcNameModify);
                //送參數進入Service層做商業邏輯
                SystemPermissionService.SetPermission(searchInfo);
                searchItemList = "角色權限設定完成";
            }
            catch (Exception ex)
            {
                errLogging(funcName, "角色權限設定-" + funcNameModify + "-" + ex.ToString());
                searchItemList = "角色權限設定失敗";
                throw ex;
            }
            return Json(searchItemList, JsonRequestBehavior.AllowGet);
            #endregion
        }
        #endregion

        #region 查詢角色權限 GetPrograms()
        public ActionResult GetPrograms(SearchInfoViewModel searchInfo)
        {
            #region 參數宣告
            SearchListViewModel searchItemList = new SearchListViewModel();
            SystemPermissionService SystemPermissionService = new SystemPermissionService();
            #endregion

            #region 流程

            logging(funcName, "角色權限設定-讀取資料");
            //送參數進入Service層做商業邏輯

            searchItemList = SystemPermissionService.GetPermission(searchInfo);

            return Json(searchItemList.data, JsonRequestBehavior.AllowGet);
            #endregion
        }
        #endregion

        #region 查詢角色 GetPermissionList()
        public ActionResult GetPermissionList()
        {
            #region 參數宣告
            //List<PermissionModel> searchItemList = new List<PermissionModel>;
            SearchListViewModel searchItemList = new SearchListViewModel();
            SystemPermissionService SystemPermissionService = new SystemPermissionService();
            #endregion

            #region 流程

            logging(funcName, "角色權限設定-讀取資料");
            //送參數進入Service層做商業邏輯

            searchItemList = SystemPermissionService.GetPermissionList();

            return Json(searchItemList.list, JsonRequestBehavior.AllowGet);
            #endregion
        }
        #endregion

        #region 刪除角色 DelPrograms()
        public ActionResult DelPrograms(SearchInfoViewModel searchInfo)
        {
            #region 參數宣告
            SearchListViewModel searchItemList = new SearchListViewModel();
            SystemPermissionService SystemPermissionService = new SystemPermissionService();
            #endregion

            #region 流程
            try
            {
                logging(funcName, "角色設定-刪除角色");
                //送參數進入Service層做商業邏輯
                SystemPermissionService.DelPermission(searchInfo);
                searchItemList = SystemPermissionService.GetPermissionList();
            }
            catch (Exception ex)
            {
                errLogging(funcName, "角色權限設定-角色刪除失敗" + ex.ToString());
                throw ex;
            }
            return Json(searchItemList.list, JsonRequestBehavior.AllowGet);
            #endregion
        }
        #endregion

        #region 設定角色 EditPrograms()
        public ActionResult EditPrograms(SearchInfoViewModel searchInfo)
        {
            #region 參數宣告
            SearchListViewModel searchItemList = new SearchListViewModel();
            string Modify;
            SystemPermissionService SystemPermissionService = new SystemPermissionService();
            searchInfo.MODIFY_USER = UserID;
            #endregion

            #region 流程
            if (searchInfo.ID == "" || searchInfo.ID == null) { Modify = "新增"; }
            else { Modify = funcNameModify; }
            try
            {
                logging(funcName, "角色設定-" + Modify);
                //送參數進入Service層做商業邏輯
                SystemPermissionService.EditPermission(searchInfo);
                searchItemList = SystemPermissionService.GetPermissionList();
            }
            catch (Exception ex)
            {
                errLogging(funcName, "角色權限設定-" + Modify + "-" + ex.ToString());
                throw ex;
            }
            return Json(searchItemList.list, JsonRequestBehavior.AllowGet);
            #endregion
        }
        #endregion
    }
}