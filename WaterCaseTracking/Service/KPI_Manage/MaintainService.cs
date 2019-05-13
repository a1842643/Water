using WaterCaseTracking.Dao;
using WaterCaseTracking.Dao.KPI_Manage;
using WaterCaseTracking.Models;
using WaterCaseTracking.Models.ViewModels.KPI_Manage.Maintain;
using WaterCaseTracking.Models.ViewModels.SigningProcess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace WaterCaseTracking.Service.KPI_Manage.Maintain
{
    public class MaintainService
    {
        #region 查詢 QuerySearchList()
        public SearchListViewMode QuerySearchList(SearchInfoViewMode searchInfo)
        {
            #region 參數宣告				
            SearchListViewMode searchList = new SearchListViewMode();
            MaintainDao maintainDao = new MaintainDao();
            #endregion

            #region 流程																
            searchList = maintainDao.QuerySearchList(searchInfo); //將參數送入Dao層,組立SQL字串並連接資料庫

            return searchList;
            #endregion
        }
        #endregion

        #region 查詢 QueryTreeList
        public SearchListViewMode QueryTreeList(SearchInfoViewMode searchInfo)
        {
            #region 參數宣告				
            SearchListViewMode TreeList = new SearchListViewMode();
            MaintainDao maintainDao = new MaintainDao();
            #endregion

            #region 流程																
            TreeList = maintainDao.QueryTreeList(searchInfo); //將參數送入Dao層,組立SQL字串並連接資料庫

            //{ "text": "業績管理項目", "id": "root", "parent": "#" }

            return TreeList;
            #endregion
        }

        #endregion

        #region 傳送已設定項目 
        public void PostItem(SearchItemViewMode searchInfo)
        {
            #region 參數宣告				
            MaintainDao maintainDao = new MaintainDao();
            #endregion

            #region 流程																
            maintainDao.PostItemList(searchInfo); //將參數送入Dao層,組立SQL字串並連接資料庫

            #endregion
        }
        #endregion

        #region 查詢配分項目
        public SearchItemViewMode GetItem(SearchItemViewMode searchInfo)
        {
            #region 參數宣告				
            SearchItemViewMode searchList = new SearchItemViewMode();
            MaintainDao maintainDao = new MaintainDao();
            #endregion

            #region 流程																
            searchList = maintainDao.QueryItemList(searchInfo); //將參數送入Dao層,組立SQL字串並連接資料庫
            #endregion
            return searchList;
        }
        #endregion

        #region 刪除配分項目
        public SearchItemViewMode DelItem(SearchItemViewMode searchInfo)
        {
            #region 參數宣告				
            SearchItemViewMode searchList = new SearchItemViewMode();
            MaintainDao maintainDao = new MaintainDao();
            #endregion

            #region 流程																
            searchList = maintainDao.DelItem(searchInfo); //將參數送入Dao層,組立SQL字串並連接資料庫

            return searchList;
            #endregion
        }
        #endregion

        #region 抓取是否已鎖定
        public int getIsLocked()
        {
            #region 參數宣告				
            int LOCKED = 0;
            StepLogDao stepLogDao = new StepLogDao();
            StepLogModel stepLogModel = new StepLogModel();
            #endregion

            #region 流程		
            stepLogModel.YEART = (DateTime.Now.Year - 1911);
            stepLogModel.MONTH = (DateTime.Now.Month);
            stepLogModel.GROUPS = "管理績效";
            LOCKED = stepLogDao.getIsLocked(stepLogModel); //將參數送入Dao層,組立SQL字串並連接資料庫

            return LOCKED;
            #endregion
        }
        #endregion

        #region 抓取分數檔
        public DataTable getKPIM()
        {
            #region 參數宣告				
            DataTable dt = new DataTable();
            StoredProcedureDao storedProcedureDao = new StoredProcedureDao();
            #endregion

            #region 流程																
            dt = storedProcedureDao.getKPISimple("99B23540-D355-4E8A-9B1C-1ED111B9561F"); //將參數送入Dao層,組立SQL字串並連接資料庫
            //拿掉第1、0行不顯示給USER看
            //dt.Rows.Remove(dt.Rows[1]);
            dt.Rows.Remove(dt.Rows[0]);

            return dt;
            #endregion
        }
        #endregion

        internal int doUpLoad(HttpPostedFileBase file, string UserName)
        {
            #region 參數宣告		
            DataTable dt_insert = new DataTable();
            DataTable dt = new DataTable();
            StoredProcedureDao storedProcedureDao = new StoredProcedureDao();
            KPI_ScoreModel kpi_ScoreModel = new KPI_ScoreModel();
            KPI_ScoreDao kpi_ScoreDao = new KPI_ScoreDao();
            int successQty = 0;
            #endregion

            #region 流程	
            //存Xls轉的DataTable
            DataTable orgDt = new DataTable();
            //抓取分數檔
            dt = storedProcedureDao.getKPISimple("99B23540-D355-4E8A-9B1C-1ED111B9561F");
            //判斷檔案室否有trans_date資料
            if (file.ContentLength > 0 || file != null)
            {
                //建立dt_insert欄位
                dt_insert.Columns.Add("UUID");
                dt_insert.Columns[0].DataType = System.Type.GetType("System.Guid");
                dt_insert.Columns.Add("Branch_No");
                dt_insert.Columns.Add("YEART");
                dt_insert.Columns.Add("MONTHS");
                dt_insert.Columns.Add("SCORE");
                dt_insert.Columns.Add("MODIFY_USER");

                //把資料轉成DataTable
                try
                {
                    orgDt = LoadExcel.LoadXlsx(file);
                }
                catch (Exception)
                {
                    throw new Exception("匯入檔案錯誤");
                }
                List<KPI_ScoreModel> listModel = new List<KPI_ScoreModel>();
                for (int i = 1; i < orgDt.Rows.Count; i++)
                {
                    for (int j = 2; j < orgDt.Columns.Count; j++)
                    {
                        try
                        {
                            if (Convert.ToInt32(orgDt.Rows[i][j]) > Convert.ToInt32(dt.Rows[1][j]))
                            {
                                throw new Exception("得分不可大於配分");
                            }
                            //kpi_ScoreModel = new KPI_ScoreModel();
                            //kpi_ScoreModel.UUID = Guid.Parse(dt.Rows[0][j].ToString());
                            //kpi_ScoreModel.Branch_No = orgDt.Rows[i][0].ToString();
                            //kpi_ScoreModel.YEART = DateTime.Now.Year - 1911;
                            //kpi_ScoreModel.MONTHS = DateTime.Now.Month;
                            //kpi_ScoreModel.SCORE = Convert.ToInt32(orgDt.Rows[i][j]);
                            //kpi_ScoreModel.MODIFY_USER = UserName;
                            DataRow row = dt_insert.NewRow();
                            row["UUID"] = Guid.Parse(dt.Rows[0][j].ToString());
                            row["Branch_No"] = orgDt.Rows[i][0].ToString();
                            row["YEART"] = DateTime.Now.Year - 1911;
                            row["MONTHS"] = DateTime.Now.Month;
                            row["SCORE"] = Convert.ToInt32(orgDt.Rows[i][j]);
                            row["MODIFY_USER"] = UserName;

                            dt_insert.Rows.Add(row);
                        }
                        catch (Exception ex)
                        {
                            throw new Exception("第" + (i + 1) + "列資料有誤，請確認" + ex.Message);
                        }
                    }
                }
                SqlConnection sqlConn;
                SqlTransaction sqlTrans;
                //刪除資料
                kpi_ScoreDao.DeleteKPI_Score(dt_insert, out sqlConn, out sqlTrans);
                //新增資料
                kpi_ScoreDao.AddKPI_Score(dt_insert, ref sqlConn, ref sqlTrans);
            }
            return dt_insert.Rows.Count;
            #endregion
        }

        internal void ImportLastYear()
        {
            #region 參數宣告		
            KPI_DistributionDao kpi_DistributionDao = new KPI_DistributionDao();
            int successQty = 0;
            #endregion

            #region 流程	
            SqlConnection sqlConn;
            SqlTransaction sqlTrans;
            //刪除今年資料
            kpi_DistributionDao.DeleteKPI_Distribution("管理績效", out sqlConn, out sqlTrans);
            //匯入去年資料
            kpi_DistributionDao.AddKPI_Distribution("管理績效", ref sqlConn, ref sqlTrans);
            #endregion
        }

        internal int doUpLoadApply(HttpPostedFileBase file, string NEWID, string funcName, string UserID, string UserName)
        {
            #region 參數宣告		
            DataTable dt_insert = new DataTable();
            DataTable dt = new DataTable();
            StoredProcedureDao storedProcedureDao = new StoredProcedureDao();
            KPI_ScoreModel kpi_ScoreModel = new KPI_ScoreModel();
            KPI_ScoreDao kpi_ScoreDao = new KPI_ScoreDao();
            ManualUploadService manualUploadService = new ManualUploadService();
            SqlConnection sqlConn;
            SqlTransaction sqlTrans;
            int successQty = 0;
            SigningInsertItemsViewModel signingInsertItemsViewModel = new SigningInsertItemsViewModel();
            SigningRecordDao SigningRecordDao = new SigningRecordDao();
            #endregion

            #region 流程	
            manualUploadService.InsertSigningRecord(file, "16", "(管理考核)得分上傳", NEWID, funcName, UserID, UserName);
            try
            {
                //存Xls轉的DataTable
                DataTable orgDt = new DataTable();
                //抓取分數檔
                dt = storedProcedureDao.getKPISimple("99B23540-D355-4E8A-9B1C-1ED111B9561F");
                //判斷檔案室否有trans_date資料
                if (file.ContentLength > 0 || file != null)
                {
                    //建立dt_insert欄位
                    dt_insert.Columns.Add("ApplyID");
                    dt_insert.Columns.Add("UUID");
                    dt_insert.Columns[1].DataType = System.Type.GetType("System.Guid");
                    dt_insert.Columns.Add("Branch_No");
                    dt_insert.Columns.Add("YEART");
                    dt_insert.Columns.Add("MONTHS");
                    dt_insert.Columns.Add("SCORE");
                    dt_insert.Columns.Add("MODIFY_USER");

                    //把資料轉成DataTable
                    try
                    {
                        orgDt = LoadExcel.LoadXlsx(file);
                    }
                    catch (Exception)
                    {
                        throw new Exception("匯入檔案錯誤");
                    }
                    List<KPI_ScoreModel> listModel = new List<KPI_ScoreModel>();
                    for (int i = 1; i < orgDt.Rows.Count; i++)
                    {
                        for (int j = 2; j < orgDt.Columns.Count; j++)
                        {
                            try
                            {
                                if (Convert.ToInt32(orgDt.Rows[i][j]) > Convert.ToInt32(dt.Rows[1][j]))
                                {
                                    throw new Exception("得分不可大於配分");
                                }
                                //kpi_ScoreModel = new KPI_ScoreModel();
                                //kpi_ScoreModel.UUID = Guid.Parse(dt.Rows[0][j].ToString());
                                //kpi_ScoreModel.Branch_No = orgDt.Rows[i][0].ToString();
                                //kpi_ScoreModel.YEART = DateTime.Now.Year - 1911;
                                //kpi_ScoreModel.MONTHS = DateTime.Now.Month;
                                //kpi_ScoreModel.SCORE = Convert.ToInt32(orgDt.Rows[i][j]);
                                //kpi_ScoreModel.MODIFY_USER = UserName;
                                DataRow row = dt_insert.NewRow();
                                row["ApplyID"] = NEWID;
                                row["UUID"] = Guid.Parse(dt.Rows[0][j].ToString());
                                row["Branch_No"] = orgDt.Rows[i][0].ToString();
                                row["YEART"] = DateTime.Now.Year - 1911;
                                row["MONTHS"] = DateTime.Now.Month;
                                row["SCORE"] = Convert.ToInt32(orgDt.Rows[i][j]);
                                row["MODIFY_USER"] = UserName;

                                dt_insert.Rows.Add(row);
                            }
                            catch (Exception ex)
                            {
                                throw new Exception("第" + (i + 1) + "列資料有誤，請確認" + ex.Message);
                            }
                        }
                    }


                    //刪除資料
                    kpi_ScoreDao.DeleteTempKPI_Score(dt_insert, null, out sqlConn, out sqlTrans);
                    //新增資料
                    kpi_ScoreDao.AddTempKPI_Score(dt_insert, ref sqlConn, ref sqlTrans);


                }
            }
            catch (Exception ex)
            {
                signingInsertItemsViewModel.ProID = NEWID;
                SigningRecordDao.DeleteSigningRecord(signingInsertItemsViewModel);
                throw;
            }
            return dt_insert.Rows.Count;
            #endregion
        }

        #region 鎖定反鎖定
        internal void doLock(string isLocked)
        {
            #region 參數宣告	
            StepLogDao stepLogDao = new StepLogDao();
            StepLogModel stepLogModel = new StepLogModel();
            KPI_ScoreDao kpi_ScoreDao = new KPI_ScoreDao();
            #endregion

            #region 流程					
            stepLogModel.YEART = (DateTime.Now.Year - 1911);
            stepLogModel.MONTH = (DateTime.Now.Month);
            stepLogModel.GROUPS = "管理績效";
            stepLogModel.LOCKED = 1;

            if (isLocked == "1")
            {
                stepLogDao.DeleteStepLog(stepLogModel);
                kpi_ScoreDao.DeleteKPI_ScoreFromKPIDis("管理績效");
            }
            if (isLocked == "0")
                stepLogDao.AddStepLog(stepLogModel);

            #endregion
        }
        #endregion
    }
}