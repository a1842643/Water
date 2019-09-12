using System;
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
        SqlConnection sqlConn;
        SqlTransaction sqlTrans;
        #region 查詢 QuerySearchList()
        public SearchListViewModel QuerySearchList(SearchInfoViewModel searchInfo, string UserName, string roleName, string Organizer)
        {
            #region 參數宣告				
            SearchListViewModel searchList = new SearchListViewModel();
            MCAskDao mcaskDao = new MCAskDao();
            #endregion

            #region 流程																
            searchList = mcaskDao.QuerySearchList(searchInfo, UserName, roleName, Organizer); //將參數送入Dao層,組立SQL字串並連接資料庫

            return searchList;
            #endregion
        }
        #endregion

        #region 匯出範例檔-起
        internal DataTable getExportData(ExportViewModel exportViewModel, string UserName, string roleName, string Organizer)
        {
            #region 參數宣告				
            MCAskDao mcaskDao = new MCAskDao();
            DataTable dt = new DataTable();
            #endregion

            #region 流程																
            dt = mcaskDao.getExportData(exportViewModel, UserName, roleName, Organizer); //將參數送入Dao層,組立SQL字串並連接資料庫
            if (dt.Rows.Count == 0)
            {
                foreach (DataColumn col in dt.Columns)
                {
                    col.AllowDBNull = true;
                }
                DataRow row = dt.NewRow();
                dt.Rows.Add(row);
                //dt = new DataTable();
                //dt.Columns.Add("項次(不可修改，若要新增資料則留空白)");
                //dt.Columns.Add("詢問日期");
                //dt.Columns.Add("地區");
                //dt.Columns.Add("議員姓名");
                //dt.Columns.Add("詢問事項");
                //dt.Columns.Add("辦理情形");
                //dt.Columns.Add("承辦單位(若角色是一般使用者或資料維護者，科室預設自己的科室)");
                //dt.Columns.Add("承辦人員");
                //dt.Columns.Add("狀態");
            }

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
        internal void DeleteMCAskTable(List<string> ID, string Types, string UserName)
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
        internal int doUpLoad(HttpPostedFileBase file, string Types, string UserName, string roleName, string Organizer)
        {
            #region 參數宣告				
            MCAskModel mcaskModel = new MCAskModel();
            MCAskDao mcaskDao = new MCAskDao();
            SysCodeDao sysCodeDao = new SysCodeDao();
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
                //先初始化值
                mcaskDao.defaultSqlP(out sqlConn, out sqlTrans);
                List<MCAskModel> listModel = new List<MCAskModel>();
                if (roleName == "user")
                {
                    for (int i = 0; i < orgDt.Rows.Count; i++)
                    {
                        if (string.IsNullOrEmpty(orgDt.Rows[i][0].ToString().Replace("\n", "").Trim()))
                        {
                            throw new Exception("一般使用者只能修改");
                        }
                    }
                }
                for (int i = 0; i < orgDt.Rows.Count; i++)
                {
                    try
                    {
                        mcaskModel = new MCAskModel();
                        mcaskModel.ID = orgDt.Rows[i][0].ToString().Replace("\n","").Trim();
                        mcaskModel.AskDate = orgDt.Rows[i][1].ToString().Replace("\n", "").Trim() == ""? null: orgDt.Rows[i][1].ToString().Replace("\n", "").Trim();
                        mcaskModel.Area = orgDt.Rows[i][2].ToString().Replace("\n", "").Trim();
                        mcaskModel.MemberName = orgDt.Rows[i][3].ToString().Replace("\n", "").Trim();
                        mcaskModel.Inquiry = orgDt.Rows[i][4].ToString().Replace("\n", "").Trim();
                        mcaskModel.HandlingSituation = orgDt.Rows[i][5].ToString();
                        mcaskModel.DetailHandlingSituation = orgDt.Rows[i][6].ToString();
                        //如果是資料維護者或是一般使用者只能是自己的科室
                        if (roleName == "maintain" || roleName == "user")
                        {
                            mcaskModel.Organizer = Organizer;
                        }
                        else
                        {
                            mcaskModel.Organizer = orgDt.Rows[i][7].ToString().Replace("\n", "").Trim();
                            //判斷科室有無正確
                            if (!sysCodeDao.CheckSysCode(mcaskModel.Organizer))
                            {
                                throw new Exception("查無此科室");
                            }
                        }
                        mcaskModel.OrganizerMan = orgDt.Rows[i][8].ToString().Replace("\n", "").Trim();
                        mcaskModel.sStatus = orgDt.Rows[i][9].ToString().Replace("\n", "").Trim();
                        mcaskModel.Types = Types;
                        //listModel.Add(mcaskModel);
                        //判斷無ID則新增，有ID正確就修改
                        if (!string.IsNullOrEmpty(mcaskModel.ID))
                        {
                            //若資料正確則修改
                            if (mcaskDao.QueryUpdateDataConn(mcaskModel.ID, Types, ref sqlConn, ref sqlTrans) != null)
                            {
                                mcaskDao.UpdateMulMCAskTable(mcaskModel, UserName, ref sqlConn, ref sqlTrans);
                            }
                            //若沒有資料則錯誤
                            else
                            {
                                throw new Exception("查無此筆資料");
                            }
                        }
                        else
                        {
                            mcaskDao.AddMulMCAskTable(mcaskModel, UserName, ref sqlConn, ref sqlTrans);
                        }
                    }
                    catch (Exception ex)
                    {
                        //錯誤的話就Rollback
                        mcaskDao.RollbackSqlP(ref sqlConn, ref sqlTrans);
                        throw new Exception("第" + (i + 2) + "筆資料有誤" + ex.Message);
                    }
                }
                //沒錯誤則Commit
                mcaskDao.CommitSqlP(ref sqlConn, ref sqlTrans);
                ////刪除資料
                //untilDao.DeleteUpdateUntil(listModel, out sqlConn, out sqlTrans);
                ////新增資料
                //successQty = untilDao.AddUpdateUntil(listModel, ref sqlConn, ref sqlTrans);
                //新增資料
                //successQty = mcaskDao.AddMCAskListTable(listModel, UserName);
                //mcaskDao.DeleteMCAskListTable(listModel);
            }
            return successQty;
            #endregion
        }
        #endregion
    }
}