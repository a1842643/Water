using WaterCaseTracking.Models.ViewModels.KPI_Busines;
using WaterCaseTracking.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WaterCaseTracking.Controllers
{
    public class KPI_BusinesController : Controller
    {
        // GET: KPI_Busines
        public ActionResult Maintain()
        {
            return View();
        }

        #region 取得項目資料
        public ActionResult GetItemRatio(SearchInfoViewModel searchInfo)
        {
            #region 參數宣告				
            SearchListViewModel searchList = new SearchListViewModel();
            KPI_BusinesService KPI_BusinesService = new KPI_BusinesService();
            #endregion

            #region 流程																
            searchList = KPI_BusinesService.GetItemRatio(searchInfo); //將參數送入Dao層,組立SQL字串並連接資料庫           

            return Json(searchList, JsonRequestBehavior.AllowGet);
            #endregion
        }

        #endregion

        #region 設定項目配比
        public ActionResult SetRatio(SearchInfoViewModel searchInfo)
        {
            #region 參數宣告				
            SearchListViewModel searchList = new SearchListViewModel();
            KPI_BusinesService KPI_BusinesService = new KPI_BusinesService();
            #endregion

            #region 流程																
            searchList = KPI_BusinesService.SetItem(searchInfo); //將參數送入Dao層,組立SQL字串並連接資料庫

            return Json(searchList, JsonRequestBehavior.AllowGet);
            #endregion
        }
        #endregion
    }
}