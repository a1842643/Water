using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using Microsoft.VisualBasic.FileIO;
using System.Net;

namespace WaterCaseTracking.Controllers
{
    public class FileManagerController : Controller
    {
        private string selectedNodePath = "root";//目前選擇的NODE路徑，可配合排序，只限定選擇的資料夾來排序(目前先不做)
        private string SortKey = "Name";
        private bool SortIsDescending =false;
        private string UserType = "user";
        private string strUserFolder= WebConfigurationManager.AppSettings["ReportUserDefaultFolder"].ToString();
        private string strBasicFolder= "FileManagerUploads";
        private string strBasicPath;//限定SERVER檔管路徑
        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            base.Initialize(requestContext);
            strBasicPath = Server.MapPath("../FileManagerUploads/");//限定SERVER檔管路徑
        }
        
        // GET: FileManager
        public ActionResult FileManager()
        {
            return View();
        }

        #region "jstree Expand"
        public ActionResult GetFileManagerDirectoryInfo()
        {
            if (Request.QueryString["selectedPath"] !=null)
                selectedNodePath = Request.QueryString["selectedPath"].Replace("/", "\\").Trim();
            if (Request.QueryString["UserType"]!=null)
                UserType = Request.QueryString["UserType"].Trim();
            if (Request.QueryString["SortKey"] !=null)
                SortKey = Request.QueryString["SortKey"].Trim();
            if (Request.QueryString["IsDescending"] !=null)
                SortIsDescending = Request.QueryString["IsDescending"].ToUpper().Trim().Equals("TRUE") ? true : false;
            DirectoryInfo dirBasic = new DirectoryInfo(strBasicPath);
            if (!dirBasic.Exists) dirBasic.Create();
            string[] entries = System.IO.Directory.GetDirectories(strBasicPath);
            if (entries.Count() < 1)
            {
                //根目錄，必要
                Directory.CreateDirectory(strBasicPath + "root");
                //以下測試資料，可移除
                #region "for default test"
                //Directory.CreateDirectory(strBasicPath + "root\\node1");
                //Directory.CreateDirectory(strBasicPath + "root\\node1\\node10");
                //Directory.CreateDirectory(strBasicPath + "root\\node1\\node11");
                //Directory.CreateDirectory(strBasicPath + "root\\node2");
                #endregion
            }
            //報表預設路徑
            DirectoryInfo dirUserFolder = new DirectoryInfo(strBasicPath+  strUserFolder);
            if (!dirUserFolder.Exists) dirUserFolder.Create();

            List<TreeAttributeViewModel> DirNodes = new List<TreeAttributeViewModel>();
            if(UserType.Equals("admin")) DirNodes = getCuriseLocus(DirNodes, strBasicPath);
            else DirNodes = getCuriseLocus(DirNodes, strBasicPath+ strUserFolder);
            return Json(DirNodes, JsonRequestBehavior.AllowGet);

        }

        /// <summary>
        /// 向下剝層巡跡
        /// </summary>
        /// <returns></returns>
        private List<TreeAttributeViewModel> getCuriseLocus(List<TreeAttributeViewModel> DirNodes, string currentDir)
        {
            string[] entries = System.IO.Directory.GetDirectories(currentDir);
            try
            {
                var result = from dir in
                                 from e in entries select new DirectoryInfo(e)
                             orderby dir.Name
                             select new
                             {
                                 //isSelected = dir.Equals(new DirectoryInfo(strBasicPath+ selectedNodePath)),
                                 currectPath = dir.FullName,
                                 text = dir.Name,
                                 CreatTime = dir.CreationTime,
                                 LastWriteTime = dir.LastWriteTime
                             };
                //資料夾先不排序
                //if (result.FirstOrDefault().isSelected)
                //{
                //    switch (SortKey.ToUpper().Trim())
                //    {
                //        case "NAME":
                //            if (!SortIsDescending) result.OrderBy(x => x.currectPath);
                //            else result = result.OrderByDescending(x => x.currectPath);
                //            break;
                //        case "TIME":
                //            if (!SortIsDescending) result.OrderBy(x => x.LastWriteTime);
                //            else result = result.OrderByDescending(x => x.LastWriteTime);
                //            break;

                //    }
                //}
                foreach (var dir in result)
                {
                    DirectoryInfo d = new DirectoryInfo(dir.currectPath);
                    if (d.GetDirectories().Count() < 1)
                    {
                        if (d.GetFiles().Count() > 0)
                        {
                            DirNodes.Add(new TreeAttributeViewModel { text = dir.text.ToString(), children = getFiles(dir.currectPath) });

                        }
                        else DirNodes.Add(new TreeAttributeViewModel { text = dir.text.ToString() });
                    }
                    else
                    {
                        DirNodes.Add(new TreeAttributeViewModel { text = dir.text.ToString(), children = getCuriseLocus(new List<TreeAttributeViewModel>(), d.FullName) });
                    }
                }
                DirectoryInfo thisDir = new DirectoryInfo(currentDir);
                if (thisDir.GetFiles().Count() > 0)
                {
                    DirNodes.AddRange(getFiles(currentDir));
                }
                return DirNodes;
            }
            catch
            {
                return DirNodes;
            }
        }


        private List<TreeAttributeViewModel> getFiles(string currentDir)
        {
            string dirRootPath = "";
            if(currentDir.Split(new[] { strBasicFolder }, StringSplitOptions.None)[1].Count()>0)
                dirRootPath = currentDir.Split(new[] { strBasicFolder }, StringSplitOptions.None)[1];
            if (string.IsNullOrEmpty(dirRootPath))
                return null;
            dirRootPath = dirRootPath.Replace(Path.DirectorySeparatorChar, '/')+@"/";
            List <TreeAttributeViewModel> DirNodes = new List<TreeAttributeViewModel>();
            string[] entries = System.IO.Directory.GetFiles(currentDir);
            string serverDomain = Request.Url.GetLeftPart(UriPartial.Authority)+ @"/";
            Uri url = new Uri(currentDir);
            var result = from file in
                             from e in entries select new FileInfo(e)
                         select new
                         {
                             text = file.Name,
                             state = "{ 'file': true }",
                             icon = "jstree-file",
                             CreatTime = file.CreationTime,
                             a_attr = new { href = serverDomain + strBasicFolder + dirRootPath + file.Name },
                             LastWriteTime = file.LastWriteTime,
                             Size = file.Length
                         };
            if (result.Count() > 0)
            {
                switch (SortKey.ToUpper().Trim())
                {
                    case "NAME":
                        if (!SortIsDescending) result = result.OrderBy(x => x.text);
                        else result = result.OrderByDescending(x => x.text);
                        break;
                    case "TIME":
                        if (!SortIsDescending) result = result.OrderBy(x => x.LastWriteTime);
                        else result = result.OrderByDescending(x => x.LastWriteTime);
                        break;
                    case "SIZE":
                        if (!SortIsDescending) result = result.OrderBy(x => x.Size);
                        else result = result.OrderByDescending(x => x.Size);
                        break;

                }
            }
            foreach (var f in result)
            {
                DirNodes.Add(new TreeAttributeViewModel { text = f.text.ToString(), state = f.state, icon = f.icon, a_attr = f.a_attr });

            }
            return DirNodes;
        }
        
        #endregion


        /// <summary>
        /// 新增資料夾，ReName_FileDir可以一併處理，暫時用不到
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Create_Dir()
        {
            #region 驗證
            if (!ModelState.IsValid)
            { return View(); }

            string newFolder = System.Web.HttpContext.Current.Request.Params["newFolder"];//新資料夾名稱
            if (string.IsNullOrEmpty(newFolder))
            { return Json(new { success = false, responseText = "資料夾名稱不可為空!" }, JsonRequestBehavior.AllowGet); }
            if (!verPathOrFileChars("D", newFolder))
            { return Json(new { success = false, responseText = "資料夾名稱不符合規則!" }, JsonRequestBehavior.AllowGet); }
            #endregion
            #region 參數宣告
            string newPath = System.Web.HttpContext.Current.Request.Params["newPath"];//節點路徑，最後節點NEW NODE是jstree給定

            string[] Path_Arr = newPath.Split('/');//原來的路徑陣列
            Path_Arr[Path_Arr.Length - 1] = newFolder;//插入新folder name
            newPath = string.Join("\\", Path_Arr); //新folder路徑(STRING)

            #endregion

            #region 流程	

            string msg = "";
            bool workSuccess = false;
            try
            {
                DirectoryInfo newDir_Path = new DirectoryInfo(strBasicPath + newPath);//新資料夾路徑(DirInfo)

                if (!newDir_Path.Exists)
                {
                    newDir_Path.Create();
                    workSuccess = true;
                }
                else//SERVER原路徑存在
                {
                    msg = "Fail,原路徑已有名稱相同的資料夾!";
                }

            }
            catch (Exception ex)
            {
                msg = "Fail," + ex.Message;
                workSuccess = true;
            }
            finally
            {
                //成功的話返回主頁
                if (workSuccess == true)
                {
                    msg = "新增成功!";
                    //GetFileManagerDirectoryInfo();
                }
            }
            #endregion
            //return View(newPath);
            return Json(new { success = workSuccess, responseText = msg }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// ReName
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ReName_FileDir()
        {
            #region 驗證
            if (!ModelState.IsValid)
            { return View(); }
            string newText = "";
            try
            {
                newText = System.Web.HttpContext.Current.Request.Params["newText"];//新的名稱
            }
            catch (Exception ex)
            {
                return Json(new { success = false, responseText = ex.Message }, JsonRequestBehavior.AllowGet);
            }
            if (string.IsNullOrEmpty(newText))
            { return Json(new { success = false, responseText = "名稱不可為空!" }, JsonRequestBehavior.AllowGet); }

            string fileOrDir = System.Web.HttpContext.Current.Request.Params["fileOrDir"];//F:File ; D:Dir
            if (fileOrDir.ToUpper() == "D")
            {
                if (!verPathOrFileChars("D", newText))
                { return Json(new { success = false, responseText = "資料夾名稱不符合規則!" }, JsonRequestBehavior.AllowGet); }

            }
            else if (fileOrDir.ToUpper() == "F")
            {
                if (!verPathOrFileChars("F", newText))
                { return Json(new { success = false, responseText = "檔案名稱不符合規則!" }, JsonRequestBehavior.AllowGet); }

            }
            else {
                return Json(new { success = false, responseText = "路逕不存在!" }, JsonRequestBehavior.AllowGet);
            }
            #endregion
            #region 參數宣告

            
            string newPath = System.Web.HttpContext.Current.Request.Params["newPath"].Replace("/", "\\");//改成新的路徑
            string oldName = System.Web.HttpContext.Current.Request.Params["oldText"];//舊的名稱
            string[] Path_Arr = newPath.Split('\\');//新的路徑陣列
            Path_Arr[Path_Arr.Length - 1] = oldName;//還原舊路徑(陣列)
            string oldPath = string.Join("\\", Path_Arr); //還原舊路徑(STRING)
            DirectoryInfo old_Path = new DirectoryInfo(strBasicPath + oldPath);//還原舊路徑(DirInfo)
            #endregion

            #region 流程	


            string msg = "";
            bool workSuccess = false;
            try
            {
                if (old_Path.Exists)//資料夾SERVER原路徑存在，視為資料夾改名稱
                {
                    old_Path.MoveTo(strBasicPath + newPath);//資料夾改名稱
                    workSuccess = true;
                }
                else
                {
                    if (fileOrDir.ToUpper() == "D") //屬資料夾，資料夾SERVER原路徑不存在，視為新增資料夾
                    {
                        DirectoryInfo new_Path = new DirectoryInfo(strBasicPath + newPath);//新路徑(DirInfo)
                        new_Path.Create();
                        msg = "新增完成!";
                    }
                    else if(fileOrDir.ToUpper() == "F")//屬檔案，資料夾SERVER原路徑不存在，視為檔案改名稱
                    {
                        FileInfo f = new FileInfo(strBasicPath + oldPath);
                        if (f.Exists)
                        {
                            f.MoveTo(strBasicPath + newPath);
                            msg = newPath;
                            workSuccess = true;
                        }
                        else
                        {
                            msg = "Fail,路逕不存在!";
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                msg = "Fail," + ex.Message;
            }
            #endregion
            //return View(newPath); not msg of success 
            return Json(new { success = workSuccess, responseText = msg }, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// 刪除空資料夾或檔案
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Delete_FileDir()
        {

            #region 驗證
            if (!ModelState.IsValid)
            { return View(); }
            #endregion
            #region 參數宣告

            string fileOrDir = System.Web.HttpContext.Current.Request.Params["fileOrDir"];//F:File ; D:Dir
            string strSelected = System.Web.HttpContext.Current.Request.Params["deletePath"].Replace("/", "\\");//刪除的路徑

            
            #endregion

            #region 流程	

            //刪除所有內容，此部份先保留
            //foreach (FileInfo file in selectedPath.EnumerateFiles())
            //{
            //    file.Delete();
            //}
            //foreach (DirectoryInfo dir in selectedPath.EnumerateDirectories())
            //{
            //    dir.Delete(true);
            //}

            string msg = "";
            bool workSuccess = false;
            try
            {
                if (fileOrDir.ToUpper() == "D")
                {
                    DirectoryInfo selectedDir = new DirectoryInfo(strBasicPath + strSelected);//還原路徑(DirInfo)

                    //if (selectedDir.GetFiles().Count() > 0 || selectedDir.GetDirectories().Count()>0)
                    //{
                    //    msg = "刪除內容不包含檔案及資料夾，必須為空資料夾!";
                    //}
                        FileSystem.DeleteDirectory(selectedDir.FullName, UIOption.AllDialogs, RecycleOption.SendToRecycleBin);
                    
                        workSuccess = true;
                        msg = strSelected+"刪除完成!";
                }
                else if (fileOrDir.ToUpper() == "F")
                {
                    FileInfo selectedFile = new FileInfo(strBasicPath + strSelected);
                    if (selectedFile.Exists)
                    {
                        FileSystem.DeleteFile(selectedFile.FullName,UIOption.AllDialogs, RecycleOption.SendToRecycleBin);
                        //selectedFile.Delete();
                        workSuccess = true;
                        msg = strSelected + "刪除完成!";
                    }
                    else msg = "欲刪除檔案不存在，請重新整理畫面!";
                }



            }
            catch (Exception ex)
            {
                msg = "Fail," + ex.Message;
            }
            #endregion
            //return View(newPath); not msg of success 
            return Json(new { success = workSuccess, responseText = msg }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Move Dir or File
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Move_FileDir()
        {
            #region 驗證
            if (!ModelState.IsValid)
            { return View(); }

            #endregion
            #region 參數宣告

            string fileOrDir = System.Web.HttpContext.Current.Request.Params["fileOrDir"];//F:File ; D:Dir 
            string dragTarget = System.Web.HttpContext.Current.Request.Params["dragTarget"];//拖拉的對象File OR Dir 
            #endregion

            #region 流程	


            string msg = "";
            bool workSuccess = false;
            try
            {
                if(fileOrDir.ToUpper()=="D")
                {
                    string strNewPath = System.Web.HttpContext.Current.Request.Params["newPath"].Replace("/", "\\");//改成絕對路徑

                    DirectoryInfo newPath_chk = new DirectoryInfo(strBasicPath + strNewPath + @"\" + dragTarget);
                    if (newPath_chk.Exists)
                    {
                        workSuccess = false;
                        msg = "目標目錄已存在相同資料夾："+dragTarget  ;
                    }
                    else
                    {
                        DirectoryInfo newPath = new DirectoryInfo(strBasicPath + strNewPath + @"\" + dragTarget);

                        //原先父PATH 加上 拖拉目標名稱 ，始成原始路徑
                        string strOldPath = System.Web.HttpContext.Current.Request.Params["origParentPath"].Replace("/", "\\");//改成絕對路徑
                        DirectoryInfo oldPath = new DirectoryInfo(strBasicPath + strOldPath + @"\" + dragTarget);
                        if (oldPath.Exists)
                        {
                            if (!newPath.Exists) newPath.Create();
                            FileSystem.MoveDirectory(oldPath.FullName, newPath.FullName, true);
                            //oldPath.MoveTo(newPath.FullName);
                            workSuccess = true;
                            msg = dragTarget + "位置移動完成!";
                        }
                    }
                }
                else if (fileOrDir.ToUpper() == "F")
                {

                    string strNewPath = System.Web.HttpContext.Current.Request.Params["newPath"].Replace("/", "\\");//改成絕對路徑

                    FileInfo newPath = new FileInfo(strBasicPath + strNewPath + @"\" + dragTarget);
                    if (newPath.Exists)
                    {
                        workSuccess = false;
                        msg = "目標目錄已有同名檔案："+ dragTarget;
                    }
                    else
                    {
                        //原先父PATH 加上 拖拉目標名稱 ，始成原始路徑
                        string strOldPath = System.Web.HttpContext.Current.Request.Params["origParentPath"].Replace("/", "\\");//改成絕對路徑
                        FileInfo oldPath = new FileInfo(strBasicPath + strOldPath + @"\" + dragTarget);
                        if (oldPath.Exists)
                        {
                            FileSystem.MoveFile(oldPath.FullName, newPath.FullName, true);
                            //oldPath.MoveTo(strBasicPath + newPath.FullName);
                            workSuccess = true;
                            msg = dragTarget + "位置移動完成!";
                        }
                    }
                }
                else
                {
                    workSuccess = false;
                    msg ="請重新操作!";
                }
            }
            catch (Exception ex)
            {
                msg = "Fail," + ex.Message;
            }
            #endregion
            //return View(newPath); not msg of success 
            return Json(new { success = workSuccess, responseText = msg }, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// 上傳檔案
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult UpLoadFile()
        {
            #region 參數宣告
            string UploadPath = Request.Form["UploadPath"];// System.Web.HttpContext.Current.Request.Params["UploadPath"].Replace("/", "\\");//改成新的路徑
            string UploadPathReplace = UploadPath.Replace("/", "\\");
            string strMsg = "";
            bool workSuccess = false;
            #endregion
            //## 如果有任何檔案類型才做
            if (Request.Files.AllKeys.Any())
            {
                //## 讀取指定的上傳檔案ID
                foreach (string file in Request.Files)
                {
                    HttpPostedFileBase uploadFile = Request.Files[file] as HttpPostedFileBase;
                    if (uploadFile != null && uploadFile.ContentLength > 0)
                    {
                        var fileName = Path.GetFileName(uploadFile.FileName);
                        var path = Path.Combine(strBasicPath + UploadPathReplace, fileName);
                        FileInfo file_check = new FileInfo(strBasicPath + UploadPathReplace);
                        DirectoryInfo dirPath = new DirectoryInfo(strBasicPath + UploadPathReplace);
                        if (!dirPath.Exists) return Json(new { success = true, responseText = "請指定到資料夾!" }, JsonRequestBehavior.AllowGet);
                        file_check = new FileInfo(path);
                        if (file_check.Exists)
                        {
                            strMsg += fileName + "檔案已存在，請先刪除後，再新增!";
                            continue;
                        }
                        else
                        {
                            try
                            {
                                uploadFile.SaveAs(path);
                                strMsg += "上傳" + fileName + "完成!";
                                workSuccess = true;
                            }
                            catch (Exception ex)
                            {
                                strMsg += fileName + ex.Message;
                            }
                        }
                    }
                    else
                    {
                        strMsg += uploadFile.FileName + "上傳失敗：" + "檔案不存在或是空檔案!";
                        continue;
                    }
                }
            }
            return Json(new { success = workSuccess, responseText = strMsg }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 下載檔案
        /// </summary>
        /// <returns></returns>
        public ActionResult DownloadFile()
        {
            #region 驗證 

            #endregion
            #region 參數宣告

            string url = System.Web.HttpContext.Current.Request.Params["downLoadPath"];
            string fileName = System.Web.HttpContext.Current.Request.Params["fileName"];
            #endregion

            #region 流程	

            try
            {                
                string filepath = AppDomain.CurrentDomain.BaseDirectory+ strBasicFolder+ url;
                byte[] filedata = System.IO.File.ReadAllBytes(filepath);
                string contentType = MimeMapping.GetMimeMapping(filepath);

                var cd = new System.Net.Mime.ContentDisposition
                {
                    FileName = fileName,
                    Inline = true,
                };

                //Response.AppendHeader("Content-Disposition", cd.ToString());
                //Response.Headers.Add("Content-Disposition", cd.ToString());
                return File(filedata, contentType, fileName);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            #endregion
        }
        /// <summary>
        /// 檔案或資料夾名稱驗證
        /// </summary>
        /// <param name="dirOrFile">D(DIR);F(File)</param>
        /// <param name="myText">要驗證的字串</param>
        /// <returns>true :驗證通過，false:有無效字元</returns>
        private bool verPathOrFileChars(string dirOrFile, string myText)
        {
            try
            {
                char[] invalidPathChars = Path.GetInvalidFileNameChars();
                char[] myChr = myText.ToCharArray();
                char[] invalidResult = invalidPathChars.Intersect(myChr).ToArray();
                if (invalidResult.Count() > 0) return false;
                else return true;
            }
            catch { return false; }
        }


    }


    public class TreeAttributeViewModel
    {

        //public string currectPath { get; set; }
        /// <summary>
        /// 資料夾或檔案名稱
        /// </summary>
        public string text { get; set; }
        /// <summary>
        /// 屬性 資料夾或檔案
        /// </summary>
        public string state { get; set; }
        /// <summary>
        /// CSS
        /// </summary>
        public string icon { get; set; }
        /// <summary>
        /// 向下從屬
        /// </summary>
        public List<TreeAttributeViewModel> children { get; set; }
        /// <summary>
        /// 創建時間
        /// </summary>
        //public DateTime CreatTime { get; set; }
        ///// <summary>
        ///// 修改時間
        ///// </summary>
        //public DateTime EditTime { get; set; }
        public  dynamic  a_attr { get; set; }
    }

}