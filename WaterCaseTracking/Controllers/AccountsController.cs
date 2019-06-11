using WaterCaseTracking.Models;
using WaterCaseTracking.Models.ViewModels.Accounts;
using WaterCaseTracking.Service;
using System.Web.Security;
using System;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;

namespace WaterCaseTracking.Controllers
{
    //[Authorize]
    public class AccountsController : BaseController
    {
        string FuncName = "帳號管理";
        string FuncNameC = "帳號管理-新增";
        string FuncNameM = "帳號管理-修改";
        string FuncName1 = "修改密碼功能";
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

        #region 初始化-起
        [HttpPost]
        public ActionResult searchInitCM()
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
                //抓科室下拉選單
                searchInitViewModel.ddlOrganizer = dropDownListService.getddlOrganizer();
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
                searchList = accountsService.QuerySearchList(searchInfo, roleName, Organizer);
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
        #region 啟用停用-起
        [HttpPost]
        public ActionResult Delete(string idx)
        {
            logging(FuncName, "啟用停用");
            #region 參數宣告
            AccountsService accountsService = new AccountsService();
            string fail = "";
            #endregion

            #region 流程	
            try
            {
                //送參數進入Service層做商業邏輯
                accountsService.EnableAccount(idx, UserName);
            }
            catch (Exception ex)
            {
                fail = "Fail," + ex.Message;
                errLogging(FuncName, ex.ToString());
            }
            

            return Json("", JsonRequestBehavior.AllowGet);
            #endregion
        }
        #endregion 啟用停用-迄

        #region 進入新增-起
        public ActionResult Create()
        {
            logging(FuncNameC, "進入功能-新增");
            #region 參數宣告
            AccountsService accountsService = new AccountsService();
            AccountsModel accountsModel = new AccountsModel();
            string fail = "";
            #endregion

            #region 流程	
            
            #endregion

            return View("Create", accountsModel);
        }

        [HttpPost]
        public ActionResult Create(AccountsModel accountsModel)
        {
            logging(FuncNameC, "新增");
            #region 驗證
            if (!ModelState.IsValid)
            { return View("Create", accountsModel); }
            #endregion
            #region 參數宣告
            AccountsService accountsService = new AccountsService();
            AccountsModel checkAccountID = new AccountsModel();
            string fail = "";
            #endregion

            #region 流程	
            try
            {
                //判斷是否已有帳號
                checkAccountID = accountsService.QueryAccountInfo(accountsModel.AccountID);
                if(checkAccountID != null)
                {
                    TempData["message"] = "該帳號已註冊，請使用其他帳號";
                    return View("Create", accountsModel);
                }
                //送參數進入Service層做商業邏輯
                accountsService.AddAccountsTable(accountsModel, UserName);
            }
            catch (Exception ex)
            {
                fail = "Fail," + ex.Message;
                errLogging(FuncNameC, ex.ToString());
            }
            //成功的話返回主頁
            if (string.IsNullOrEmpty(fail))
            {
                TempData["message"] = "新增成功";
                return RedirectToAction("Maintain");
            }
            else
            {
                TempData["message"] = fail;
            }

            return View("Create", accountsModel);
            #endregion
        }
        #endregion

        #region 進入修改-起
        [HttpPost]
        public ActionResult Edit(string idx)
        {
            logging(FuncNameM, "進入功能-" + FuncNameM);
            #region 參數宣告
            AccountsService accountsService = new AccountsService();
            AccountsModel accountsModel = new AccountsModel();
            string fail = "";
            #endregion

            #region 流程	
            try
            {
                //查詢有無資料
                accountsModel = accountsService.QueryAccountInfo(idx);
            }
            catch (Exception ex)
            {
                fail = "Fail," + ex.Message;
                errLogging(FuncNameM, ex.ToString());
            }
            #endregion

            return View("Edit", accountsModel);
        }

        [HttpPost]
        public ActionResult DoEdit(AccountsModel accountsModel)
        {
            logging(FuncNameM, "修改");
            #region 驗證
            if (!ModelState.IsValid)
            { return View("Edit", accountsModel); }
            #endregion
            #region 參數宣告
            AccountsService accountsService = new AccountsService();
            string fail = "";
            #endregion

            #region 流程	
            try
            {
                //送參數進入Service層做商業邏輯
                accountsService.UpdateAccountsTable(accountsModel, UserName);
            }
            catch (Exception ex)
            {
                fail = "Fail," + ex.Message;
                errLogging(FuncNameM, ex.ToString());
            }
            //成功的話返回主頁
            if (string.IsNullOrEmpty(fail))
            {
                TempData["message"] = "修改成功";
                return RedirectToAction("Maintain");
            }
            else
            {
                TempData["message"] = fail;
            }

            return View("Edit", accountsModel);
            #endregion
        }
        #endregion

        #region 重設回預設密碼-起
        [HttpPost]
        public ActionResult pwToDefault(string idx)
        {
            logging(FuncName, "啟用停用");
            #region 參數宣告
            AccountsService accountsService = new AccountsService();
            string fail = "";
            #endregion

            #region 流程	
            try
            {
                //送參數進入Service層做商業邏輯
                accountsService.pwToDefault(idx, UserName);
            }
            catch (Exception ex)
            {
                fail = "Fail," + ex.Message;
                errLogging(FuncName, ex.ToString());
            }


            return Json("", JsonRequestBehavior.AllowGet);
            #endregion
        }
        #endregion





        #region 修改密碼-起

        public ActionResult ChangePW(ChangePwViewModel changePwViewModel)
        {
            //判斷是否正確進入該頁面
            if (string.IsNullOrEmpty(changePwViewModel.AccountID ))
            {
                TempData["loginMsg"] = "資訊錯誤，請重新登入";
                return RedirectToAction("login", "Home");
            }
            logging(FuncName1, "進入修改密碼頁面");
            return View(changePwViewModel);

        }
        #endregion 修改密碼-訖
        #region 確認舊密碼正確性-起
        [HttpPost]
        public ActionResult CheckOldpassword(string AccountID, string oldpassword)
        {
            #region 參數宣告
            AccountsService accountsService = new AccountsService();
            AccountsModel accountsModel = new AccountsModel();
            //是否正確
            int IsCorrect = 0;
            #endregion

            #region 流程	

            try
            {
                //確認帳號正確性
                accountsModel = accountsService.QueryAccountInfo(AccountID, oldpassword);
                if (accountsModel != null)
                {
                    IsCorrect = 1;
                }
            }
            catch (Exception ex)
            {
                return Json(IsCorrect, JsonRequestBehavior.AllowGet);
            }
            //組Json格式回傳Models資料
            return Json(IsCorrect, JsonRequestBehavior.AllowGet);
            #endregion

        }
        #endregion 確認舊密碼正確性-迄

        #region 修改密碼-起
        [HttpPost]
        public ActionResult ToChangePW(ChangePwViewModel changePwViewModel)
        {
            logging(FuncName1, "修改密碼");
            #region 驗證
            if (!ModelState.IsValid)
            {
                return View("ChangePW", changePwViewModel);
            }
            #endregion
            #region 參數宣告
            AccountsService accountsService = new AccountsService();
            AccountsModel accountsModel = new AccountsModel();
            //是否修改成功
            int IsSuccess = 0;
            #endregion

            #region 流程	
            //確認密碼有無與前三次相同
            accountsModel = accountsService.CheckPassword(changePwViewModel);
            if (accountsModel != null)
            {
                TempData["ChangePWMessage"] = "密碼修改失敗，新密碼不得與前設定三次密碼相同";
                return View("ChangePW", changePwViewModel);
            }

            try
            {
                //修改密碼
                IsSuccess = accountsService.ToChangePW(changePwViewModel);
            }
            catch (Exception ex)
            {
                errLogging(FuncName1, "密碼修改失敗");
                TempData["ChangePWMessage"] = "密碼修改失敗";
                return View("ChangePW", changePwViewModel);
            }
                if (IsSuccess == 0)
                {
                errLogging(FuncName1, "密碼修改失敗");
                TempData["ChangePWMessage"] = "密碼修改失敗";
                    return View("ChangePW", changePwViewModel);
                }
            //取得帳號資訊
            accountsModel = accountsService.QueryAccountInfo(changePwViewModel.AccountID, changePwViewModel.password1);
            Session["Menu"] = GetOperating(accountsModel.Role);

            //單位名稱
            Session["UnitName"] = "營業部";
            //單位代碼
            Session["Unit"] = "00M";
            //登入者ID
            Session["UserID"] = accountsModel.AccountID;
            //登入者姓名
            Session["UserName"] = accountsModel.AccountName;
            //登入者科室
            Session["Organizer"] = accountsModel.Organizer;
            //登入者角色
            Session["roleName"] = accountsModel.Role;
            //角色代碼清單
            Session["roleId"] = "admin,user,application,signing,review";//請傳入字串 格式如 : admin,user,application,signing,review
            Models.ViewModels.Account.UserInfoModel UserInfo = new Models.ViewModels.Account.UserInfoModel()
            {
                Account = accountsModel.AccountID,
                UserName = UserName,
                Power = roleId,
                SessinID = Session.SessionID,
                LoginTime = DateTime.Now
            };

            ViewBag.UserInfo = UserInfo;
            string user = Environment.UserName;
            var ticket = new FormsAuthenticationTicket(
                         version: 1,
                         name: UserInfo.Account,
                         issueDate: DateTime.Now,
                         expiration: DateTime.Now.AddHours(360),
                         isPersistent: true,
                         userData: JsonConvert.SerializeObject(UserInfo),
                         cookiePath: FormsAuthentication.FormsCookiePath);

            string encryptedTicket = FormsAuthentication.Encrypt(ticket);
            //Response.Cookies.Clear();
            Response.Cookies[FormsAuthentication.FormsCookieName].Value = encryptedTicket;

            TempData["message"] = "密碼修改成功";
            return RedirectToAction("Maintain0", "MCAsk");
            #endregion

        }
        #endregion 修改密碼-迄

        

        #region 取得功能列表 GetOperating()
        public string GetOperating(string searchInfo)
        {
            #region 參數宣告		
            Models.ViewModels.Home.SearchListViewModel searchList = new Models.ViewModels.Home.SearchListViewModel();
            HomeService homeService = new HomeService();
            #endregion

            #region 流程					
            logging("功能設定", "取得功能列表 : " + searchInfo);
            searchList = homeService.GetOperating(searchInfo); //將參數送入Dao層,組立SQL字串並連接資料庫

            return searchList.UserModel[0].PROID.ToString();
            #endregion
        }
        #endregion
    }
}