using WaterCaseTracking.Dao;
using WaterCaseTracking.Models.Common;
using WaterCaseTracking.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WaterCaseTracking.Controllers
{
    public class BaseController : Controller
    {
        //使用者編號
        public string UserID { get { return Session["UserID"] as String; } }
        //使用者名稱
        public string UserName { get { return Session["UserName"] as String; } }
        //單位
        public string Unit { get { return Session["Unit"] as String; } }
        //單位名稱
        public string UnitName { get { return Session["UnitName"] as String; } }
        //群組
        public string Groups { get { return Session["Groups"] as String; } }
        //角色
        public string roleId { get { return Session["roleId"] as String; } }

        //編輯權限判定
        public string operating { get { return Session["operating"] as String; } }

        public string NewID() {
            return DateTime.Now.ToString("yyyyMMddHHmmss") + UserID;
        }
        public void logging(string funcName, string message)
        {
            #region 參數宣告	
            #endregion

            #region 流程			
            LoggModel loggModel = new LoggModel();
            loggModel.UserID = UserID;
            loggModel.UserName = UserName;
            loggModel.FuncName = funcName;
            loggModel.Message = message;
            loggModel.Status = "Success";

            CommonService.logging(loggModel); //將參數送入Dao層,組立SQL字串並連接資料庫
            #endregion
        }
        public void errLogging(string funcName, string message)
        {
            #region 參數宣告	
            #endregion

            #region 流程			
            LoggModel loggModel = new LoggModel();
            loggModel.UserID = UserID;
            loggModel.UserName = UserName;
            loggModel.FuncName = funcName;
            loggModel.Message = message;
            loggModel.Status = "Error";

            CommonService.logging(loggModel); //將參數送入Dao層,組立SQL字串並連接資料庫
            #endregion
        }
    }
}