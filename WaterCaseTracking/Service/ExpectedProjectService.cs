using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using WaterCaseTracking.Dao;
using WaterCaseTracking.Models;
using WaterCaseTracking.Models.ViewModels.ExpectedProject;

namespace WaterCaseTracking.Service
{
    public class ExpectedProjectService
    {
        SqlConnection sqlConn;
        SqlTransaction sqlTrans;
        #region 查詢 QuerySearchList()
        public SearchListViewModel QuerySearchList(SearchInfoViewModel searchInfo, string UserName, string roleName, string Organizer)
        {
            #region 參數宣告				
            SearchListViewModel searchList = new SearchListViewModel();
            ExpectedProjectDao expectedProjectDao = new ExpectedProjectDao();
            #endregion

            #region 流程																
            searchList = expectedProjectDao.QuerySearchList(searchInfo, UserName, roleName, Organizer); //將參數送入Dao層,組立SQL字串並連接資料庫

            return searchList;
            #endregion
        }
        #endregion

        #region 匯出範例檔-起
        internal DataTable getExportData(ExportViewModel exportViewModel, string UserName, string roleName, string Organizer)
        {
            #region 參數宣告				
            ExpectedProjectDao expectedProjectDao = new ExpectedProjectDao();
            DataTable dt = new DataTable();
            #endregion

            #region 流程																
            dt = expectedProjectDao.getExportData(exportViewModel, UserName, roleName, Organizer); //將參數送入Dao層,組立SQL字串並連接資料庫
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
                //dt.Columns.Add("工程名稱");
                //dt.Columns.Add("成案預計完成日期");
                //dt.Columns.Add("成案實際完成日期");
                //dt.Columns.Add("規劃預計完成日期");
                //dt.Columns.Add("規劃實際完成日期");
                //dt.Columns.Add("基本設計預計完成日期");
                //dt.Columns.Add("基本設計實際完成日期");
                //dt.Columns.Add("細部設計預計完成日期");
                //dt.Columns.Add("細部設計實際完成日期");
                //dt.Columns.Add("上網發包預計完成日期");
                //dt.Columns.Add("上網發包實際完成日期");
                //dt.Columns.Add("評選預計完成日期");
                //dt.Columns.Add("評選實際完成日期");
                //dt.Columns.Add("決標時間預計完成日期");
                //dt.Columns.Add("決標時間實際完成日期");
                //dt.Columns.Add("承辦單位(若角色是一般使用者或資料維護者，科室預設自己的科室)");
                //dt.Columns.Add("承辦人員");
            }

            return dt;
            #endregion
        }

        #endregion 匯出範例檔-迄
        #region 單筆新增ExpectedProjectTable-起
        internal void AddExpectedProjectTable(ExpectedProjectModel expectedProjectModel, string UserName)
        {
            #region 參數宣告				
            ExpectedProjectDao expectedProjectDao = new ExpectedProjectDao();
            #endregion

            #region 流程			
            expectedProjectDao.AddExpectedProjectTable(expectedProjectModel, UserName); //將參數送入Dao層,組立SQL字串並連接資料庫
            #endregion
        }

        #endregion 單筆新增ExpectedProjectTable-迄

        #region 單筆修改ExpectedProjectTable-起
        internal void UpdateExpectedProjectTable(ExpectedProjectModel expectedProjectModel, string UserName)
        {
            #region 參數宣告				
            ExpectedProjectDao expectedProjectDao = new ExpectedProjectDao();
            #endregion

            #region 流程			
            expectedProjectDao.UpdateExpectedProjectTable(expectedProjectModel, UserName); //將參數送入Dao層,組立SQL字串並連接資料庫
            #endregion
        }

        #endregion 單筆修改ExpectedProjectTable-迄

        #region 單筆刪除ExpectedProjectTable-起
        internal void DeleteExpectedProjectTable(List<string> ID)
        {
            #region 參數宣告				
            ExpectedProjectDao expectedProjectDao = new ExpectedProjectDao();
            #endregion

            #region 流程			
            expectedProjectDao.DeleteExpectedProjectTable(ID); //將參數送入Dao層,組立SQL字串並連接資料庫
            #endregion
        }

        #endregion 單筆刪除ExpectedProjectTable-迄

        #region 修改用查詢
        internal ExpectedProjectModel QueryUpdateData(string ID)
        {
            #region 參數宣告				
            ExpectedProjectModel expectedProjectModel = new ExpectedProjectModel();
            ExpectedProjectDao expectedProjectDao = new ExpectedProjectDao();
            #endregion

            #region 流程																
            expectedProjectModel = expectedProjectDao.QueryUpdateData(ID); //將參數送入Dao層,組立SQL字串並連接資料庫

            return expectedProjectModel;
            #endregion
        }

        #endregion
        #region 匯入
        internal int doUpLoad(HttpPostedFileBase file, string UserName, string roleName, string Organizer)
        {
            #region 參數宣告				
            ExpectedProjectModel expectedProjectModel = new ExpectedProjectModel();
            ExpectedProjectDao expectedProjectDao = new ExpectedProjectDao();
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
                expectedProjectDao.defaultSqlP(out sqlConn, out sqlTrans);
                List<ExpectedProjectModel> listModel = new List<ExpectedProjectModel>();
                for (int i = 0; i < orgDt.Rows.Count; i++)
                {
                    try
                    {
                        expectedProjectModel = new ExpectedProjectModel();
                        expectedProjectModel.ID = orgDt.Rows[i][0].ToString();                     //項次
                        expectedProjectModel.ProjectName = orgDt.Rows[i][1].ToString();            //工程名稱
                        expectedProjectModel.CrProExpDate = orgDt.Rows[i][2].ToString();           //成案預計完成日期 
                        expectedProjectModel.CrProReaDate = orgDt.Rows[i][3].ToString();           //成案實際完成日期
                        expectedProjectModel.PlanExpDate = orgDt.Rows[i][4].ToString();            //規劃預計完成日期
                        expectedProjectModel.PlanReaDate = orgDt.Rows[i][5].ToString();            //規劃實際完成日期
                        expectedProjectModel.BasDesExpDate = orgDt.Rows[i][6].ToString();          //基本設計預計完成日期
                        expectedProjectModel.BasDesReaDate = orgDt.Rows[i][7].ToString();          //基本設計實際完成日期
                        expectedProjectModel.DetailDesExpDate = orgDt.Rows[i][8].ToString();       //細部設計預計完成日期
                        expectedProjectModel.DetailDesReaDate = orgDt.Rows[i][9].ToString();       //細部設計實際完成日期
                        expectedProjectModel.OnlineExpDate = orgDt.Rows[i][10].ToString();         //上網發包預計完成日期
                        expectedProjectModel.OnlineReaDate = orgDt.Rows[i][11].ToString();         //上網發包實際完成日期
                        expectedProjectModel.SelectionExpDate = orgDt.Rows[i][12].ToString();      //評選預計完成日期
                        expectedProjectModel.SelectionReaDate = orgDt.Rows[i][13].ToString();      //評選實際完成日期
                        expectedProjectModel.AwardExpDate = orgDt.Rows[i][14].ToString();          //決標時間預計完成日期
                        expectedProjectModel.AwardReaDate = orgDt.Rows[i][15].ToString();          //決標時間實際完成日期
                        expectedProjectModel.Organizer = orgDt.Rows[i][16].ToString();             //科室
                        //如果是資料維護者或是一般使用者只能是自己的科室
                        if (roleName == "maintain" || roleName == "user")
                        {
                            expectedProjectModel.Organizer = Organizer;
                        }
                        else
                        {
                            expectedProjectModel.Organizer = orgDt.Rows[i][16].ToString();             //科室
                            //判斷科室有無正確
                            if (!sysCodeDao.CheckSysCode(expectedProjectModel.Organizer))
                            {
                                throw new Exception("查無此科室");
                            }
                        }
                        expectedProjectModel.OrganizerMan = orgDt.Rows[i][17].ToString();          //承辦人

                        //判斷無ID則新增，有ID正確就修改
                        if (!string.IsNullOrEmpty(expectedProjectModel.ID))
                        {
                            //若資料正確則修改
                            if (expectedProjectDao.QueryUpdateData(expectedProjectModel.ID, ref sqlConn, ref sqlTrans) != null)
                            {
                                expectedProjectDao.UpdateMulExpectedProjectTable(expectedProjectModel, UserName, ref sqlConn, ref sqlTrans);
                            }
                            //若沒有資料則錯誤
                            else
                            {
                                throw new Exception("");
                            }
                        }
                        else
                        {
                            expectedProjectDao.AddMulExpectedProjectTable(expectedProjectModel, UserName, ref sqlConn, ref sqlTrans);
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("第" + (i + 2) + "筆資料有誤，請確認");
                    }
                    //沒錯誤則Commit
                }
                expectedProjectDao.CommitSqlP(ref sqlConn, ref sqlTrans);
            }
            return successQty;
            #endregion
        }
        #endregion
    }
}