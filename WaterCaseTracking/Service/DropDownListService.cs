using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WaterCaseTracking.Models.ViewModels;
using WaterCaseTracking.Dao;
using WaterCaseTracking.Models;

namespace WaterCaseTracking.Service
{
    public class DropDownListService
    {
        /// <summary>
        /// 抓地區別下拉選單
        /// </summary>
        /// <returns></returns>
        internal DropDownListViewModel getddlArea()
        {
            #region 參數宣告				
            SysCodeDao untilDao = new SysCodeDao();
            #endregion

            #region 流程																
            DropDownListViewModel ddlArea = untilDao.getddlItem("Area"); 

            return ddlArea;
            #endregion
        }

        /// <summary>
        /// 抓科室下拉選單
        /// </summary>
        /// <returns></returns>
        internal DropDownListViewModel getddlOrganizer()
        {
            #region 參數宣告				
            SysCodeDao untilDao = new SysCodeDao();
            #endregion

            #region 流程																
            DropDownListViewModel ddlOrganizer = untilDao.getddlItem("Organizer");

            return ddlOrganizer;
            #endregion
        }

        /// <summary>
        /// 抓狀態下拉選單
        /// </summary>
        /// <returns></returns>
        internal DropDownListViewModel getddlsStatus()
        {
            #region 參數宣告				
            SysCodeDao untilDao = new SysCodeDao();
            #endregion

            #region 流程																
            DropDownListViewModel ddlsStatus = untilDao.getddlItem("sStatus");

            return ddlsStatus;
            #endregion
        }

        /// <summary>
        /// 抓分行下拉選單
        /// </summary>
        /// <returns></returns>
        internal DropDownListViewModel getddlUntil(string Area = "")
        {
            #region 參數宣告				
            UntilDao untilDao = new UntilDao();
            #endregion

            #region 流程																
            DropDownListViewModel ddlArea = untilDao.GetddlUntil(Area); //將參數送入Dao層,組立SQL字串並連接資料庫

            return ddlArea;
            #endregion
        }
        /// <summary>
        /// 抓分行下拉選單
        /// </summary>
        /// <returns></returns>
        internal DropDownListViewModel getddlUntilforArea(string Area)
        {
            #region 參數宣告				
            UntilDao untilDao = new UntilDao();
            #endregion

            #region 流程																
            DropDownListViewModel ddlArea = untilDao.getddlUntilforArea(Area); //將參數送入Dao層,組立SQL字串並連接資料庫

            return ddlArea;
            #endregion
        }
        /// <summary>
        /// 抓分行下拉選單
        /// </summary>
        /// <returns></returns>
        internal DropDownListViewModel getddlUntilforAss_GroupName(string Ass_GroupName)
        {
            #region 參數宣告				
            UntilDao untilDao = new UntilDao();
            #endregion

            #region 流程																
            DropDownListViewModel ddlArea = untilDao.getddlUntilforAss_GroupName(Ass_GroupName); //將參數送入Dao層,組立SQL字串並連接資料庫

            return ddlArea;
            #endregion
        }
        /// <summary>
        /// 抓分行下拉選單
        /// </summary>
        /// <returns></returns>
        internal DropDownListViewModel getddlUntilforGen_JurisName(string Gen_JurisName)
        {
            #region 參數宣告				
            UntilDao untilDao = new UntilDao();
            #endregion

            #region 流程																
            DropDownListViewModel ddlArea = untilDao.getddlUntilforAss_GroupName(Gen_JurisName); //將參數送入Dao層,組立SQL字串並連接資料庫

            return ddlArea;
            #endregion
        }
       
        /// <summary>
        /// 抓分行下拉選單
        /// </summary>
        /// <returns></returns>
        internal DropDownListViewModel getddlUntilforAreacenterName(string AreacenterName)
        {
            #region 參數宣告				
            UntilDao untilDao = new UntilDao();
            #endregion

            #region 流程																
            DropDownListViewModel ddlArea = untilDao.getddlUntilforAreacenterName(AreacenterName); //將參數送入Dao層,組立SQL字串並連接資料庫

            return ddlArea;
            #endregion
        }
        /// <summary>
        /// 抓考核別下拉選單
        /// </summary>
        /// <returns></returns>
        internal DropDownListViewModel getddlAssessmentGroup()
        {
            #region 參數宣告				
            UntilDao untilDao = new UntilDao();
            #endregion

            #region 流程																
            DropDownListViewModel ddlArea = untilDao.getddlAss_GroupName(); //將參數送入Dao層,組立SQL字串並連接資料庫

            return ddlArea;
            #endregion
        }
        /// <summary>
        /// 抓區域中心
        /// </summary>
        /// <returns></returns>
        internal DropDownListViewModel getddlAreacenterName()
        {
            #region 參數宣告				
            UntilDao untilDao = new UntilDao();
            #endregion

            #region 流程																
            DropDownListViewModel ddlArea = untilDao.getddlAreacenterName(); //將參數送入Dao層,組立SQL字串並連接資料庫

            return ddlArea;
            #endregion
        }
        /// <summary>
        /// 抓副總轄管區
        /// </summary>
        /// <returns></returns>
        internal DropDownListViewModel getddlGen_JurisName()
        {
            #region 參數宣告				
            UntilDao untilDao = new UntilDao();
            #endregion

            #region 流程																
            DropDownListViewModel ddlArea = untilDao.getddlGen_JurisName(); //將參數送入Dao層,組立SQL字串並連接資料庫

            return ddlArea;
            #endregion
        }
        /// <summary>
        /// 抓行舍
        /// </summary>
        /// <returns></returns>
        internal DropDownListViewModel getddlDormName()
        {
            #region 參數宣告				
            UntilDao untilDao = new UntilDao();
            #endregion

            #region 流程																
            DropDownListViewModel ddlArea = untilDao.getddlDormName(); //將參數送入Dao層,組立SQL字串並連接資料庫

            return ddlArea;
            #endregion
        }

        /// <summary>
        /// 抓行舍
        /// </summary>
        /// <returns></returns>
        internal DropDownListViewModel getddlKPIPer()
        {
            #region 參數宣告				
            KPI_itemDao kpi_itemDao = new KPI_itemDao();
            #endregion

            #region 流程																
            DropDownListViewModel ddlKPIPer = kpi_itemDao.getddlKPIPer(); //將參數送入Dao層,組立SQL字串並連接資料庫

            return ddlKPIPer;
            #endregion
        }

        /// <summary>
        /// 抓角色別下拉選單
        /// </summary>
        /// <returns></returns>
        internal DropDownListViewModel ddlRoleList(string roleId)
        {
            #region 參數宣告				
            SystemPermissionDao SystemPermissionDao = new SystemPermissionDao();
            #endregion

            #region 流程																
            DropDownListViewModel ddlRole = SystemPermissionDao.GetddlPermissionList(roleId); //將參數送入Dao層,組立SQL字串並連接資料庫

            return ddlRole;
            #endregion
        }

        internal DropDownListViewModel getddlLog_FuncName(string jobID)
        {
            #region 參數宣告
            Dictionary<string, List<Tuple<string, string>>> JobContent = BatchManageJobContent.getJobContent();
            #endregion
            //抓批次產出報表下拉選單
            var ReportDDL = from item in JobContent.Where(x => x.Key == jobID)
                            .FirstOrDefault().Value
                            select new
                            {
                                val = item.Item1,
                                text = item.Item2
                            };
            List<DropDownListItem> list=new List<DropDownListItem>();
            foreach (var item in ReportDDL)
            {
                DropDownListItem ddlItem = new DropDownListItem();
                ddlItem.Text = item.text;
                ddlItem.Values = item.val;
                list.Add(ddlItem);
            }


            #region 流程					
            DropDownListViewModel ddlLog_FuncName = new DropDownListViewModel();
            ddlLog_FuncName.DropDownListLT= list;
            							

            return ddlLog_FuncName;
            #endregion
        }

        #region 操作系統查詢
        /// <summary>
        /// 抓操作人員下拉選單
        /// </summary>
        /// <returns></returns>
        internal DropDownListViewModel getddlUserName() {
            #region 參數宣告				
            LoggDao LandLogDao = new LoggDao();
            #endregion

            #region 流程																
            DropDownListViewModel ddlUserName = LandLogDao.getddlUserName(); //將參數送入Dao層,組立SQL字串並連接資料庫

            return ddlUserName;
            #endregion
        }
        /// <summary>
        /// 抓功能下拉選單
        /// </summary>
        /// <returns></returns>
        internal DropDownListViewModel getddlFuncName() {
            #region 參數宣告				
            LoggDao LandLogDao = new LoggDao();
            #endregion

            #region 流程																
            DropDownListViewModel ddlFuncName = LandLogDao.getddlFuncName(); //將參數送入Dao層,組立SQL字串並連接資料庫

            return ddlFuncName;
            #endregion
        }
        /// <summary>
        /// 抓狀態下拉選單
        /// </summary>
        /// <returns></returns>
        internal DropDownListViewModel getddlStatus() {
            #region 參數宣告				
            LoggDao LandLogDao = new LoggDao();
            #endregion

            #region 流程																
            DropDownListViewModel ddlStatus = LandLogDao.getddlStatus(); //將參數送入Dao層,組立SQL字串並連接資料庫

            return ddlStatus;
            #endregion
        }
        #endregion
    }
}