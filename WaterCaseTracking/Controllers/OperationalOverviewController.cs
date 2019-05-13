using WaterCaseTracking.Models.ViewModels;
using WaterCaseTracking.Models.ViewModels.OperationalOverview;
using WaterCaseTracking.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WaterCaseTracking.Controllers
{
    public class OperationalOverviewController : Controller
    {
        // GET: ReportQuery
        public ActionResult OperationalOverview()
        {
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
            List<DropDownListItem> ddlListYeart = new List<DropDownListItem>();
            //List<DropDownListItem> ddlListMonth = new List<DropDownListItem>();
            string isHis = "";
            #endregion

            #region 流程	

            try
            {
                //抓年分
                for (int i = 0; i < 8; i++)
                {
                    if (i > 2)
                        isHis = "-歷史資料表";

                    ddlListYeart.Add(new DropDownListItem
                    {
                        Values = (DateTime.Now.Year - 1911 - i).ToString(),
                        Text = (DateTime.Now.Year - 1911 - i).ToString() + isHis
                    });
                }
                dropDownListViewModel.DropDownListLT = ddlListYeart;
                searchInitViewModel.ddlYeart = dropDownListViewModel;

                //抓月分
                //for (int i = 0; i < 12; i++)
                //{
                //    ddlListMonth.Add(new DropDownListItem
                //    {
                //        Values = (i + 1).ToString(),
                //        Text = (i + 1).ToString()
                //    });
                //}
                //dropDownListViewModel.DropDownListLT = ddlListMonth;
                //searchInitViewModel.ddlMonth = dropDownListViewModel; //設定月份

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
        public ActionResult AreaClick(SearchListViewModel Search)
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
               
               if (Search.Area==null)
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
        public ActionResult AssessmentGroupClick(SearchListViewModel Search)
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
                if (Search.Ass_GroupName==null)
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

        #endregion 初始化-迄
        
             [HttpPost]
        public ActionResult AreacenterNameClick(SearchListViewModel Search)
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
                if (Search.AreacenterName==null)
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
        public ActionResult lGen_JurisNameClick(SearchListViewModel Search)
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

                if (Search.Gen_JurisName==null)

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
        
        [HttpPost]
        public ActionResult OperatedCardBResult(SearchListViewModel Search)
        {
            string msg = "";
            OperationalOverviewService OperationalOverviewService = new OperationalOverviewService();
            msg = OperationalOverviewService.OperatedCardBResult(Search);
            return Json(msg, JsonRequestBehavior.AllowGet);
        }
    }
}