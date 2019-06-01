using WaterCaseTracking.Models;
using WaterCaseTracking.Models.ViewModels.Accounts;
using WaterCaseTracking.Service;
using System;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WaterCaseTracking.Controllers
{
    [Authorize]
    public class AccountsController : BaseController
    {
        string FuncName = "帳號管理";
        // GET: BusinessUT
        public ActionResult Maintain()
        {
            logging(FuncName, "進入功能-" + FuncName);
            return View();
        }
        #region 初始化-起
        [HttpPost]
        public ActionResult searchInit()
        {
            #region 參數宣告
            DropDownListService dropDownListService = new DropDownListService();
            SearchInitViewModel searchInitViewModel = new SearchInitViewModel();
            #endregion

            #region 流程	

            try
            {
                //抓角色下拉選單
                searchInitViewModel.ddlRole = dropDownListService.getddlRole();
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
        #region 查詢-起
        /// <summary>
        /// 查詢
        /// </summary>
        /// <param name="searchInfo"></param>
        /// <returns></returns>
        public ActionResult GetoTable(SearchInfoViewModel searchInfo)
        {
            logging(FuncName, "查詢");
            #region 參數宣告

            SearchListViewModel searchList = new SearchListViewModel();
            AccountsService accountsService = new AccountsService();
            #endregion

            #region 流程	
            try
            {
                //送參數進入Service層做商業邏輯
                searchList = accountsService.QuerySearchList(searchInfo);
            }
            catch (Exception ex)
            {
                searchList.db_Result = "Fail , " + ex.Message;
                errLogging(FuncName, ex.ToString());
            }
            //組Json格式回傳Models資料
            return Json(searchList, JsonRequestBehavior.AllowGet);
            #endregion

        }
        #endregion 查詢-訖


    }
}