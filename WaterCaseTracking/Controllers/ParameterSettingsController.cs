using WaterCaseTracking.Models.ViewModels.ParameterSettings;
using WaterCaseTracking.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WaterCaseTracking.Controllers
{
    public class ParameterSettingsController : BaseController
    {
        private string funcName = "系統參數設定";
        private string funcNameModify = "修改";

        // GET: ParameterSettings
        public ActionResult ParameterSettings()
        {
            logging(funcName, "進入功能-" + funcName);
            return View();
        }

        #region 取得設定參數
        public ActionResult GetFileJson()
        {
            #region 參數宣告
            ParameterSettingsService GetFileJson = new ParameterSettingsService();
            string filepath = Server.MapPath("~/DataFile/SystemSettings/SystemSettings.json");
            string json = string.Empty;
            #endregion

            #region 流程
            try
            {
                logging(funcName, "取得設定參數");
                json = GetFileJson.GetFileJson(filepath);
                var searchList = json;
                return Json(searchList, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                errLogging(funcName, "取得設定參數-失敗");
                return Json("檔案讀取失敗", JsonRequestBehavior.AllowGet);
            }

            #endregion
        }
        #endregion

        #region 設定參數
        public ActionResult SetFileJson(SearchInfoViewModel data)
        {
            #region 參數宣告
            ParameterSettingsService GetFileJson = new ParameterSettingsService();
            string filepath = Server.MapPath("~/DataFile/SystemSettings/SystemSettings.json");
            string json = string.Empty;
            #endregion

            #region 流程

            try
            {
                logging(funcName, "設定參數-成功 : " + funcNameModify);
                json = GetFileJson.SetFileJson(data,filepath);
                var searchList = json;
                return Json(searchList, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                errLogging(funcName, "設定參數-失敗 : " + funcNameModify);
                json = "檔案讀取失敗";
                return Json(json, JsonRequestBehavior.AllowGet);
            }

            #endregion
        }
        #endregion
    }
}