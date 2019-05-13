using WaterCaseTracking.Service;
using WaterCaseTracking.Models.ViewModels.ManagementRecordCardReport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WaterCaseTracking.Models.ViewModels;

namespace WaterCaseTracking.Controllers
{
    public class ManagementRecordCardReportController : BaseController
    {
        string funcName = "匯出經營管理記錄卡";
        // GET: ManagementRecordCardReport
        public ActionResult ManagementRecordCardReport()
        {
            logging(funcName, "進入功能-" + funcName);
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
            List<DropDownListItem> ddlListYeartMonth = new List<DropDownListItem>();
            DateTime date = DateTime.Now;
            #endregion

            #region 流程	

            try
            {
                //抓年月
                for (int i = 0; i < 4; i++)
                {
                    string mm = "";
                    bool status = true;
                    for (int j = 12; j > 0; j--)
                    {
                        if (i == 0 && j > date.Month)
                            status = false;
                        else
                            status = true;

                        if (j.ToString().Length < 2)
                            mm = "0" + j.ToString();
                        else
                            mm = j.ToString();

                        if (status)
                        {
                            ddlListYeartMonth.Add(new DropDownListItem
                            {
                                Values = (DateTime.Now.Year - 1911 - i).ToString() + mm,
                                Text = (DateTime.Now.Year - 1911 - i).ToString() + "年" + mm + "月"
                            });
                        }
                    }
                }
                dropDownListViewModel.DropDownListLT = ddlListYeartMonth;
                searchInitViewModel.ddlYeartMonth = dropDownListViewModel;

                //抓地區下拉選單
                searchInitViewModel.ddlArea = dropDownListService.getddlArea();

                ////抓分行下拉選單
                //searchInitViewModel.ddlUntil = dropDownListService.getddlUntil();

                //抓考核組別下拉選單
                searchInitViewModel.ddlAssessmentGroup = dropDownListService.getddlAssessmentGroup();

                //抓區域中心
                searchInitViewModel.ddlAreacenterName = dropDownListService.getddlAreacenterName();

                //抓副總轄管區
                searchInitViewModel.ddlGen_JurisName = dropDownListService.getddlGen_JurisName();
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

        #region 取得分行下拉選單
        public ActionResult searchInitUnitl(SearchInfoViewModel SearchInfo)
        {
            #region 參數宣告
            ManagementRecordCardReportService ManagementRecordCardReportService = new ManagementRecordCardReportService();
            SearchInitViewModel searchInitViewModel = new SearchInitViewModel();
            #endregion

            #region 流程	

            try
            {
                logging(funcName, "查詢-" + funcName);
                //抓分行下拉選單
                searchInitViewModel.ddlUntil = ManagementRecordCardReportService.getddlUntil(SearchInfo);
            }
            catch (Exception ex)
            {
                searchInitViewModel.db_Result = "Fail , " + ex.Message;
                errLogging(funcName, ex.ToString());
            }
            //組Json格式回傳Models資料
            return Json(searchInitViewModel, JsonRequestBehavior.AllowGet);
            #endregion
        }
        #endregion
    }
}