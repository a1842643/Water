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
            SysCodeDao sysCodeDao = new SysCodeDao();
            #endregion

            #region 流程																
            DropDownListViewModel ddlArea = sysCodeDao.getddlItem("Area"); 

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
            SysCodeDao sysCodeDao = new SysCodeDao();
            #endregion

            #region 流程																
            DropDownListViewModel ddlOrganizer = sysCodeDao.getddlItem("Organizer");

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
            SysCodeDao sysCodeDao = new SysCodeDao();
            #endregion

            #region 流程																
            DropDownListViewModel ddlsStatus = sysCodeDao.getddlItem("sStatus");

            return ddlsStatus;
            #endregion
        }

        /// <summary>
        /// 抓角色下拉選單
        /// </summary>
        /// <returns></returns>
        internal DropDownListViewModel getddlRole()
        {
            #region 參數宣告				
            SysCodeDao sysCodeDao = new SysCodeDao();
            #endregion

            #region 流程																
            DropDownListViewModel ddlsStatus = sysCodeDao.getddlItemValue("Roles");

            return ddlsStatus;
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