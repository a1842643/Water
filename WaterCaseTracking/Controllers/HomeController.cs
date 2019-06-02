using WaterCaseTracking.Models.ViewModels.Account;
using WaterCaseTracking.Models.ViewModels;
using System;
using System.Web.Security;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using System.IO;
using WaterCaseTracking.Models;
using WaterCaseTracking.Models.ViewModels.Home;
using WaterCaseTracking.Service;
using System.Linq;
using Microsoft.VisualBasic.FileIO;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using WaterCaseTracking.Dao;
using System.Globalization;
using System.Data;

namespace WaterCaseTracking.Controllers
{
    [Authorize]
    public class HomeController : BaseController
    {
        private string strBasicPath;//限定SERVER檔管路徑
        private string funcName = "首頁公告";
        private string funcNameDelete = "刪除";
        private string funcNameModify = "修改";
        private string funcNameCreate = "新增";

        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            base.Initialize(requestContext);
            strBasicPath = Server.MapPath("~/DataFile/bulletin/");//限定SERVER檔管路徑
        }

        public ActionResult Index()
        {
            logging(funcName, "進入功能-" + funcName);
            return View();
        }

        public ActionResult RoleMenu()
        {
            logging("角色選單", "進入功能-角色選單");
            return View();
        }

        public ActionResult GetUserID()
        {

            return Json(Groups, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [AllowAnonymous]
        //增加這個方法 add this method
        public bool AcceptAllCertificatePolicy(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }


        [AllowAnonymous]
        public ActionResult login()
        {
            var ALL_HTTP = Request.ServerVariables["ALL_HTTP"];
            ViewData["ALL_HTTP"] = ALL_HTTP;
            var HTTP_LOGUID = Request.ServerVariables["HTTP_LOGUID"];
            ViewData["HTTP_LOGUID"] = HTTP_LOGUID;


            FormsAuthentication.SignOut();

            HttpCookie cookie1 = new HttpCookie(FormsAuthentication.FormsCookieName, "");
            cookie1.Expires = DateTime.Now.AddYears(-1);
            Response.Cookies.Add(cookie1);
            HttpCookie cookie2 = new HttpCookie("ASP.NET_SessionId", "");
            cookie2.Expires = DateTime.Now.AddYears(-1);
            Response.Cookies.Add(cookie2);
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult login(string txtAccount, string txtPassword)
        {
            if (string.IsNullOrEmpty(txtAccount) || string.IsNullOrEmpty(txtPassword))
            {
                TempData["loginMsg"] = "請填帳號及密碼";
                return View();
            }
            AccountsService accountsService = new AccountsService();
            AccountsModel accountsModel = new AccountsModel();

            accountsModel = accountsService.QueryAccountInfo(txtAccount, txtPassword);
            if(accountsModel == null)
            {
                TempData["loginMsg"] = "帳號或密碼錯誤，請重新輸入";
                return View();
            }

            Session["Menu"] = GetOperating(accountsModel.Role);

            //單位名稱
            Session["UnitName"] = "營業部";
                //單位代碼
                Session["Unit"] = "00M";
                //登入者ID
                Session["UserID"] = accountsModel.AccountID;
                //登入者姓名
                Session["UserName"] = accountsModel.AccountName;
                //登入者角色
                Session["roleName"] = accountsModel.Role;
            //角色代碼清單
            Session["roleId"] = "admin,user,application,signing,review";//請傳入字串 格式如 : admin,user,application,signing,review
            UserInfoModel UserInfo = new UserInfoModel()
            {
                Account = txtAccount,
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
            //Session.RemoveAll();

            //string account = new AccountService().GetValue();
            
            logging(funcName, UserID + "登入成功");
            //判斷工作事項是否有需要提醒的


            return RedirectToAction("Maintain0", "MCAsk");

        }

        #region 選取角色登入 rolemenu()
        [HttpPost]
        [AllowAnonymous]
        public ActionResult rolelogin(DropDownListItem RoleList)
        {
            logging(funcName, UserID + "正式登入");

            //角色群組
            //Session["Groups"] = "user";   
            Session["Groups"] = RoleList.Values.ToString();
            //角色清單
            Session["roleName"] = RoleList.Text.ToString();

            Session["Menu"] = GetOperating("admin");

            if (RoleList.Values.ToString() == "admin")
            {
                Session["operating"] = "true";
            }
            else
            {
                Session["operating"] = "false";
            }

            Session["Menu"] = GetOperating(Groups);

            return Json(new { url = Url.Action("Index", "Home") }); ;
        }
        #endregion

        #region 取得角色下拉選單清單
        public ActionResult roleList()
        {
            #region 參數宣告
            DropDownListService dropDownListService = new DropDownListService();
            SearchInitViewModel searchInitViewModel = new SearchInitViewModel();
            #endregion

            #region 流程	

            try
            {
                //抓角色
                searchInitViewModel.ddlRole = dropDownListService.ddlRoleList(Session["roleId"] as String);
            }
            catch (Exception ex)
            {
                searchInitViewModel.db_Result = "Fail , " + ex.Message;
            }
            //組Json格式回傳Models資料
            return Json(searchInitViewModel, JsonRequestBehavior.AllowGet);
            #endregion
        }
        #endregion


        #region 取得功能列表 GetOperating()
        public string GetOperating(string searchInfo)
        {
            #region 參數宣告		
            SearchListViewModel searchList = new SearchListViewModel();
            HomeService HomeService = new HomeService();
            #endregion

            #region 流程					
            logging("功能設定", "取得功能列表 : " + searchInfo);
            searchList = HomeService.GetOperating(searchInfo); //將參數送入Dao層,組立SQL字串並連接資料庫

            return searchList.UserModel[0].PROID.ToString();
            #endregion
        }
        #endregion
    }
}