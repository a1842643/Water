using WaterCaseTracking.Models.ViewModels;
using WaterCaseTracking.Models.ViewModels.ManagementRecordCard;
using WaterCaseTracking.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WaterCaseTracking.Controllers
{
    public class ManagementRecordCardController : BaseController
    {
        string funcName = "經營管理記錄卡";
        // GET: ManagementRecordCard
        public ActionResult ManagementRecordCard()
        {
            logging(funcName, "進入功能-" + funcName);
            return View();
        }

        #region 查詢報表
        public ActionResult ReportList(SearchInfoViewModel SearchInfo)
        {

            #region 參數宣告
            string msg = "";
            ManagementRecordCardService ManagementRecordCardService = new ManagementRecordCardService();
            SearchInitViewModel searchInitViewModel = new SearchInitViewModel();
            #endregion

            #region 流程	

            //try
            //{
                logging(funcName, "查詢-" + funcName);
                msg = ManagementRecordCardService.ReportList(SearchInfo);
            //}
            //catch (Exception ex)
            //{
            //    searchInitViewModel.db_Result = "Fail , " + ex.Message;
            //    errLogging(funcName, ex.ToString());
            //}
            #endregion
            //組Json格式回傳Models資料
            return Json(msg, JsonRequestBehavior.AllowGet);
        }
        #endregion

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

                //抓考核組別下拉選單
                searchInitViewModel.ddlAssessmentGroup = dropDownListService.getddlAssessmentGroup();
                //抓區域中心
                searchInitViewModel.ddlAreacenterName = dropDownListService.getddlAreacenterName();
                //抓副總轄管區
                searchInitViewModel.ddlGen_JurisName = dropDownListService.getddlGen_JurisName();
                //抓地區下拉選單
                searchInitViewModel.ddlArea = dropDownListService.getddlArea();

                //抓分行下拉選單
                searchInitViewModel.ddlUntil = dropDownListService.getddlUntil();
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
        [HttpPost]
        public ActionResult AreaClick(SearchInfoViewModel Search)
        {
            #region 參數宣告
            DropDownListService dropDownListService = new DropDownListService();
            SearchInitViewModel searchInitViewModel = new SearchInitViewModel();
            DropDownListViewModel dropDownListViewModel = new DropDownListViewModel();
            List<DropDownListItem> ddlListYeart = new List<DropDownListItem>();
            //List<DropDownListItem> ddlListMonth = new List<DropDownListItem>();
            string isHis = "";
            #endregion

            #region 流程	

            try
            {

                if (Search.Area == null)
                    //抓分行下拉選單
                    searchInitViewModel.ddlUntil = dropDownListService.getddlUntil();
                else

                    //抓分行下拉選單
                    searchInitViewModel.ddlUntil = dropDownListService.getddlUntilforArea(Search.Area);
                //抓地區下拉選單
                searchInitViewModel.ddlArea = dropDownListService.getddlArea();

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
        [HttpPost]
        public ActionResult AssessmentGroupClick(SearchInfoViewModel Search)
        {
            #region 參數宣告
            DropDownListService dropDownListService = new DropDownListService();
            SearchInitViewModel searchInitViewModel = new SearchInitViewModel();
            DropDownListViewModel dropDownListViewModel = new DropDownListViewModel();
            List<DropDownListItem> ddlListYeart = new List<DropDownListItem>();
            //List<DropDownListItem> ddlListMonth = new List<DropDownListItem>();
            string isHis = "";
            #endregion

            #region 流程	

            try
            {
                if (Search.Ass_GroupName == null)
                    //抓分行下拉選單
                    searchInitViewModel.ddlUntil = dropDownListService.getddlUntil();
                else

                    //抓分行下拉選單
                    searchInitViewModel.ddlUntil = dropDownListService.getddlUntilforAss_GroupName(Search.Ass_GroupName);
                //抓地區下拉選單
                searchInitViewModel.ddlArea = dropDownListService.getddlArea();

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



        [HttpPost]
        public ActionResult AreacenterNameClick(SearchInfoViewModel Search)
        {
            #region 參數宣告
            DropDownListService dropDownListService = new DropDownListService();
            SearchInitViewModel searchInitViewModel = new SearchInitViewModel();
            DropDownListViewModel dropDownListViewModel = new DropDownListViewModel();
            List<DropDownListItem> ddlListYeart = new List<DropDownListItem>();
            //List<DropDownListItem> ddlListMonth = new List<DropDownListItem>();
            string isHis = "";
            #endregion

            #region 流程	

            try
            {
                if (Search.AreacenterName == null)
                    //抓分行下拉選單
                    searchInitViewModel.ddlUntil = dropDownListService.getddlUntil();
                else
                    //抓分行下拉選單
                    searchInitViewModel.ddlUntil = dropDownListService.getddlUntilforAss_GroupName(Search.AreacenterName);
                //抓地區下拉選單
                searchInitViewModel.ddlArea = dropDownListService.getddlArea();

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



        [HttpPost]
        public ActionResult lGen_JurisNameClick(SearchInfoViewModel Search)
        {
            #region 參數宣告
            DropDownListService dropDownListService = new DropDownListService();
            SearchInitViewModel searchInitViewModel = new SearchInitViewModel();
            DropDownListViewModel dropDownListViewModel = new DropDownListViewModel();
            List<DropDownListItem> ddlListYeart = new List<DropDownListItem>();
            //List<DropDownListItem> ddlListMonth = new List<DropDownListItem>();

            #endregion

            #region 流程	

            try
            {

                if (Search.Gen_JurisName == null)

                    //抓分行下拉選單
                    searchInitViewModel.ddlUntil = dropDownListService.getddlUntil();
                else
                    //抓分行下拉選單
                    searchInitViewModel.ddlUntil = dropDownListService.getddlUntilforGen_JurisName(Search.Gen_JurisName);
                //抓地區下拉選單
                searchInitViewModel.ddlArea = dropDownListService.getddlArea();

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
    }
}