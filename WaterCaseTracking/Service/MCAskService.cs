﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using WaterCaseTracking.Dao;
using WaterCaseTracking.Models;
using WaterCaseTracking.Models.ViewModels.MCAsk;

namespace WaterCaseTracking.Service
{
    public class MCAskService
    {
        #region 查詢 QuerySearchList()
        public SearchListViewModel QuerySearchList(SearchInfoViewModel searchInfo)
        {
            #region 參數宣告				
            SearchListViewModel searchList = new SearchListViewModel();
            MCAskDao mcaskDao = new MCAskDao();
            #endregion

            #region 流程																
            searchList = mcaskDao.QuerySearchList(searchInfo); //將參數送入Dao層,組立SQL字串並連接資料庫

            return searchList;
            #endregion
        }
        #endregion

        #region 匯出範例檔-起
        internal DataTable getExportData(ExportViewModel exportViewModel)
        {
            #region 參數宣告				
            MCAskDao mcaskDao = new MCAskDao();
            DataTable dt = new DataTable();
            #endregion

            #region 流程																
            dt = mcaskDao.getExportData(exportViewModel); //將參數送入Dao層,組立SQL字串並連接資料庫

            return dt;
            #endregion
        }

        #endregion 匯出範例檔-迄
        #region 單筆新增MCAskTable-起
        internal void AddMCAskTable(MCAskModel mcaskModel, string UserName)
        {
            #region 參數宣告				
            MCAskDao mcaskDao = new MCAskDao();
            #endregion

            #region 流程			
            mcaskDao.AddMCAskTable(mcaskModel, UserName); //將參數送入Dao層,組立SQL字串並連接資料庫
            #endregion
        }

        #endregion 單筆新增MCAskTable-迄

        #region 單筆修改MCAskTable-起
        internal void UpdateMCAskTable(MCAskModel mcaskModel, string UserName)
        {
            #region 參數宣告				
            MCAskDao mcaskDao = new MCAskDao();
            #endregion

            #region 流程			
            mcaskDao.UpdateMCAskTable(mcaskModel, UserName); //將參數送入Dao層,組立SQL字串並連接資料庫
            #endregion
        }

        #endregion 單筆修改MCAskTable-迄

        #region 單筆刪除MCAskTable-起
        internal void DeleteMCAskTable(string ID, string Types, string UserName)
        {
            #region 參數宣告				
            MCAskDao mcaskDao = new MCAskDao();
            #endregion

            #region 流程			
            mcaskDao.DeleteMCAskTable(ID, Types, UserName); //將參數送入Dao層,組立SQL字串並連接資料庫
            #endregion
        }

        #endregion 單筆刪除MCAskTable-迄

        #region 修改用查詢
        internal MCAskModel QueryUpdateData(string ID, string Types)
        {
            #region 參數宣告				
            MCAskModel mcaskModel = new MCAskModel();
            MCAskDao mcaskDao = new MCAskDao();
            #endregion

            #region 流程																
            mcaskModel = mcaskDao.QueryUpdateData(ID, Types); //將參數送入Dao層,組立SQL字串並連接資料庫

            return mcaskModel;
            #endregion
        }

        #endregion
        #region 匯入
        internal int doUpLoad(HttpPostedFileBase file, string Types, string UserName)
        {
            #region 參數宣告				
            MCAskModel mcaskModel = new MCAskModel();
            MCAskDao mcaskDao = new MCAskDao();
            int successQty = 0;
            #endregion

            #region 流程	
            //存Xls轉的DataTable
            DataTable orgDt = new DataTable();
            //判斷檔案室否有trans_date資料
            if (file.ContentLength > 0 || file != null)
            {
                //把資料轉成DataTable
                try
                {
                    orgDt = LoadExcel.LoadXlsx(file);
                }
                catch (Exception)
                {
                    throw new Exception("匯入檔案錯誤");
                }
                List<MCAskModel> listModel = new List<MCAskModel>();
                for (int i = 0; i < orgDt.Rows.Count; i++)
                {
                    try
                    {
                        mcaskModel = new MCAskModel();
                        mcaskModel.ID = Convert.ToInt32(orgDt.Rows[i][0]);
                        mcaskModel.AskDate = orgDt.Rows[i][1].ToString();
                        mcaskModel.Area = orgDt.Rows[i][2].ToString();
                        mcaskModel.MemberName = orgDt.Rows[i][3].ToString();
                        mcaskModel.Inquiry = orgDt.Rows[i][4].ToString();
                        mcaskModel.HandlingSituation = orgDt.Rows[i][5].ToString();
                        mcaskModel.Organizer = orgDt.Rows[i][6].ToString();
                        mcaskModel.OrganizerMan = orgDt.Rows[i][7].ToString();
                        mcaskModel.sStatus = orgDt.Rows[i][8].ToString();
                        mcaskModel.Types = Types;
                        listModel.Add(mcaskModel);
                    }
                    catch (Exception)
                    {
                        throw new Exception("第" + i + "筆資料有誤，請確認");
                    }
                }
                //SqlConnection sqlConn;
                //SqlTransaction sqlTrans;
                ////刪除資料
                //untilDao.DeleteUpdateUntil(listModel, out sqlConn, out sqlTrans);
                ////新增資料
                //successQty = untilDao.AddUpdateUntil(listModel, ref sqlConn, ref sqlTrans);
                //新增資料
                successQty = mcaskDao.AddMCAskListTable(listModel, UserName);
                mcaskDao.DeleteMCAskListTable(listModel);
            }
            return successQty;
            #endregion
        }
        #endregion
    }
}