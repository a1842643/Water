using WaterCaseTracking.Models.ViewModels;
using WaterCaseTracking.Models.ViewModels.KPI_ManageQuery;
using WaterCaseTracking.Service;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WaterCaseTracking.Controllers
{
    public class KPI_ManageQueryController : BaseController
    {
        string FuncName = "管理績效資料查詢";
        // GET: KPI_ManageQuery
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
            DropDownListViewModel dropDownListViewModel = new DropDownListViewModel();
            List<DropDownListItem> ddlList = new List<DropDownListItem>();
            string isHis = "";
            #endregion

            #region 流程	

            try
            {
                for(int i = 0; i < 8; i++)
                {
                    if (i > 2)
                        isHis = "-歷史資料表";

                    ddlList.Add(new DropDownListItem { Values = (DateTime.Now.Year - 1911 - i).ToString(), Text = (DateTime.Now.Year - 1911 - i).ToString() + isHis });
                }
                dropDownListViewModel.DropDownListLT = ddlList;
                //抓年分
                searchInitViewModel.ddlYeart = dropDownListViewModel;
                //抓地區下拉選單
                searchInitViewModel.ddlArea = dropDownListService.getddlArea();
                //抓分行下拉選單
                searchInitViewModel.ddlUntil = dropDownListService.getddlUntil();
                //抓考核組別下拉選單
                searchInitViewModel.ddlAssessmentGroup = dropDownListService.getddlAssessmentGroup();
                //抓區域中心
                searchInitViewModel.ddlAreacenterName = dropDownListService.getddlAreacenterName();
                //抓副總轄管區
                searchInitViewModel.ddlGen_JurisName = dropDownListService.getddlGen_JurisName();
                //抓行舍
                searchInitViewModel.ddlDormName = dropDownListService.getddlDormName();
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
        #region 產表-起
        [HttpPost]
        public ActionResult Export(ExportViewModel exportViewModel)
        {
            logging(FuncName, "匯出");
            #region 參數宣告
            KPI_ManageQueryService kpi_ManageQueryService = new KPI_ManageQueryService();
            DataTable dt = new DataTable();
            string fileNamePath = "";
            #endregion

            #region 流程	
            try
            {
                //查詢修改資料
                dt = kpi_ManageQueryService.getExportData(exportViewModel);
                if (dt.Rows.Count > 1)
                {
                    fileNamePath = ExportFunction.ExportDataTableTo(dt, exportViewModel.fileExtension, "KPI_ManageQuery");
                }
            }
            catch (Exception ex)
            {
                errLogging(FuncName, ex.ToString());
            }
            return Json(fileNamePath, JsonRequestBehavior.AllowGet);

            #endregion

        }
        #endregion 產表-迄

        #region 連動分行-起
        [HttpPost]
        public ActionResult GetddlUntil(string Area)
        {

            #region 參數宣告
            DropDownListService dropDownListService = new DropDownListService();
            SearchInitViewModel searchInitViewModel = new SearchInitViewModel();
            #endregion

            #region 流程	

            try
            {
                //抓分行下拉選單
                searchInitViewModel.ddlUntil = dropDownListService.getddlUntil(Area);
            }
            catch (Exception ex)
            {
                searchInitViewModel.db_Result = "Fail , " + ex.Message;
            }
            //組Json格式回傳Models資料
            return Json(searchInitViewModel, JsonRequestBehavior.AllowGet);
            #endregion

        }
        #endregion 連動分行-迄

    }
}