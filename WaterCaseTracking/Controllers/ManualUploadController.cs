using WaterCaseTracking.Service;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WaterCaseTracking.Controllers
{
    public class ManualUploadController : BaseController {
        string funcName = "人工上傳";
        // GET: ManualUpload
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult Upload() {
            try {

                //## 如果有任何檔案類型才做
                if (Request.Files.AllKeys.Any()) {

                    logging(funcName, "上傳");
                    #region 參數宣告
                    string _ID = NewID(); //申請單號 yyyyMMddHHmmss + 員編
                    ManualUploadService manualUploadService = new ManualUploadService();
                    BaseController _BaseController = new BaseController();
                    int successQty = 0;

                    #region 取上個月民國年
                    DateTime dt = DateTime.Now.AddMonths(-1);
                    CultureInfo culture = new CultureInfo("zh-TW");
                    culture.DateTimeFormat.Calendar = new TaiwanCalendar();
                    string _YM = dt.ToString("yyyMM", culture); //民國年月 YYYMM
                    #endregion
                    //## 讀取指定的上傳檔案ID
                    HttpPostedFileBase file = Request.Files["UploadedFile"];
                    string fileName = file.FileName;
                    string FileValues = Request.Form["FileValues"];
                    string FileText = Request.Form["FileText"];
                    //取得副檔名
                    string FileExtension = System.IO.Path.GetExtension(fileName);
                    if (FileExtension != ".xlsx")
                        throw new Exception("匯入檔案錯誤");
                    #endregion
                    manualUploadService.InsertSigningRecord(file, FileValues, FileText, _ID, funcName,UserID,UserName); //建立申請單 & 檔案上傳
                    successQty = manualUploadService.doUpLoad(file, FileValues, FileText, _YM, _ID); //建立上傳站存資料
                    return Json("申請成功!!(單號:" + _ID + ")");
                }
                return Json("");
            }
            catch (Exception ex) {
                errLogging(funcName, ex.ToString());
                return Json(ex.Message);
            }
        }
    }
}