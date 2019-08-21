using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WaterCaseTracking.Controllers
{
    public class CommonController : BaseController
    {
        // GET: Common
        public ActionResult _AreaList()
        {
            return View();
        }
        /// <summary>
        /// 檢查登入資訊
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult checkLoginInfo()
        {
            string error = "";

            if (string.IsNullOrEmpty(Session["UserID"] as String)
                || string.IsNullOrEmpty(Session["Menu"] as String)
                || string.IsNullOrEmpty(Session["UnitName"] as String)
                || string.IsNullOrEmpty(Session["Unit"] as String)
                || string.IsNullOrEmpty(Session["UserName"] as String)
                || string.IsNullOrEmpty(Session["Organizer"] as String)
                || string.IsNullOrEmpty(Session["roleName"] as String)
                || string.IsNullOrEmpty(Session["roleId"] as String)
                )
            {
                error = "登入逾時";
                //清除Sission
                Session.Clear();

                System.Web.Security.FormsAuthentication.SignOut();
            }

            return Json(error, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 檢查登入資訊
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Logout()
        {
            //記錄登出
            logging("", UserID + UserName + "登出");
            //清除Sission
            Session.Clear();

            System.Web.Security.FormsAuthentication.SignOut();

            return Json("", JsonRequestBehavior.AllowGet);
        }
    }
}