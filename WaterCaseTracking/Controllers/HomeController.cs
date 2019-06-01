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

        #region 取得公告資料列表 announcementJson()
        public ActionResult announcementJson(SearchInfoViewModel searchInfo)
        {
            #region 參數宣告				
            SearchListViewModel searchList = new SearchListViewModel();
            HomeService HomeService = new HomeService();
            #endregion

            #region 流程				
            logging(funcName, "取得公告列表");
            searchList = HomeService.GetbulletinList(searchInfo); //將參數送入Dao層,組立SQL字串並連接資料庫

            return Json(searchList, JsonRequestBehavior.AllowGet);
            #endregion
        }
        #endregion

        #region 傳入公告資訊
        public ActionResult PostBulletin(SYS_announcementModel searchInfo)
        {
            #region 參數宣告				
            SearchListViewModel searchList = new SearchListViewModel();
            HomeService HomeService = new HomeService();
            searchInfo.MODIFY_USER = UserID;
            searchInfo.CreationUser = UserID;
            #endregion

            #region 流程
            if (searchInfo.ID != null)
                logging(funcName, funcNameModify);
            else
                logging(funcName, funcNameCreate);

            searchList = HomeService.PostBulletin(searchInfo); //將參數送入Dao層,組立SQL字串並連接資料庫

            return Json(searchList, JsonRequestBehavior.AllowGet);
            #endregion
        }
        #endregion

        #region 刪除公告資訊 DeleteBulletin()
        public ActionResult DeleteBulletin(SYS_announcementModel searchInfo)
        {
            #region 參數宣告				
            SearchListViewModel searchList = new SearchListViewModel();
            HomeService HomeService = new HomeService();
            searchInfo.MODIFY_USER = UserID;
            #endregion

            #region 流程							
            logging(funcName, funcNameDelete);
            searchList = HomeService.DeleteBulletin(searchInfo); //將參數送入Dao層,組立SQL字串並連接資料庫

            return Json(searchList, JsonRequestBehavior.AllowGet);
            #endregion
        }
        #endregion

        #region 取得公告資料 Getbulletin()
        public ActionResult Getbulletin(SearchInfoViewModel searchInfo)
        {
            #region 參數宣告				
            SearchListViewModel searchList = new SearchListViewModel();
            HomeService HomeService = new HomeService();
            #endregion

            #region 流程					
            logging(funcName, "取得公告資料-ID : " + searchInfo.ID.ToString());
            searchList = HomeService.Getbulletin(searchInfo); //將參數送入Dao層,組立SQL字串並連接資料庫

            return Json(searchList, JsonRequestBehavior.AllowGet);
            #endregion
        }
        #endregion

        #region 上傳公告附件 UpLoadFile()
        /// <summary>
        /// 上傳檔案
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult UpLoadFile()
        {
            #region 參數宣告
            string strMsg = "";
            bool workSuccess = false;
            string dt = DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss") + "-";
            #endregion


            var aa = Request.Files.AllKeys.Any();
            //## 如果有任何檔案類型才做
            if (Request.Files.AllKeys.Any())
            {
                //## 讀取指定的上傳檔案ID
                foreach (string file in Request.Files)
                {
                    HttpPostedFileBase uploadFile = Request.Files[file] as HttpPostedFileBase;
                    logging(funcName, "上傳公告附件 : " + Path.GetFileName(uploadFile.FileName).ToString());
                    if (uploadFile != null && uploadFile.ContentLength > 0)
                    {
                        var fileName = Path.GetFileName(uploadFile.FileName);
                        var path = Path.Combine(strBasicPath, dt + fileName);
                        FileInfo file_check = new FileInfo(strBasicPath);
                        DirectoryInfo dirPath = new DirectoryInfo(strBasicPath);
                        if (!dirPath.Exists)
                        {
                            errLogging(funcName, "上傳公告附件 : 無指定到資料夾!");
                            return Json(new { success = true, responseText = "請指定到資料夾!" }, JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            logging(funcName, "上傳公告附件 : 指定到資料夾" + strBasicPath.ToString());
                        }
                        file_check = new FileInfo(path);
                        if (file_check.Exists)
                        {
                            errLogging(funcName, "上傳公告附件 : 檔案已存在" + strBasicPath.ToString() + fileName.ToString());
                            strMsg += fileName + "檔案已存在，請先刪除後，再新增!";
                            continue;
                        }
                        else
                        {
                            try
                            {
                                logging(funcName, "上傳公告附件 : 上傳" + strBasicPath.ToString() + fileName.ToString() + "完成");
                                uploadFile.SaveAs(path);
                                strMsg += "上傳" + fileName + "完成!";
                                workSuccess = true;
                            }
                            catch (Exception ex)
                            {
                                strMsg += fileName + ex.Message;
                                errLogging(funcName, strMsg);
                            }
                        }
                    }
                    else
                    {
                        errLogging(funcName, "上傳公告附件 : 上傳失敗 檔案不存在或是空檔案");
                        strMsg += uploadFile.FileName + "上傳失敗：" + "檔案不存在或是空檔案!";
                        continue;
                    }
                }
            }
            return Json(new { success = workSuccess, responseText = strMsg, dtime = dt }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 刪除公告附件 Delete_File()
        /// <summary>
        /// 刪除空資料夾或檔案
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Delete_File()
        {
            #region 參數宣告
            string strSelected = System.Web.HttpContext.Current.Request.Params["deletePath"].Replace("/", "\\");//刪除的路徑
            #endregion

            #region 流程	

            string msg = "";
            bool workSuccess = false;
            try
            {
                FileInfo selectedFile = new FileInfo(strBasicPath + strSelected);
                logging(funcName, funcNameDelete + "-上傳公告附件 : " + selectedFile.ToString());
                if (selectedFile.Exists)
                {
                    FileSystem.DeleteFile(selectedFile.FullName, UIOption.AllDialogs, RecycleOption.SendToRecycleBin);
                    //selectedFile.Delete();
                    workSuccess = true;
                    msg = strSelected + "刪除完成!";
                    logging(funcName, funcNameDelete + "-上傳公告附件 : " + msg);
                }
                else msg = "欲刪除檔案不存在，請重新整理畫面!";
            }
            catch (Exception ex)
            {
                msg = "Fail," + ex.Message;
                errLogging(funcName, funcNameDelete + "-上傳公告附件 : " + msg);
            }
            #endregion

            return Json(new { success = workSuccess, responseText = msg }, JsonRequestBehavior.AllowGet);
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