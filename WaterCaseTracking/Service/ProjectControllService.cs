using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using WaterCaseTracking.Dao;
using WaterCaseTracking.Models;
using WaterCaseTracking.Models.ViewModels.ProjectControll;

namespace WaterCaseTracking.Service
{
    public class ProjectControllService
    {
        SqlConnection sqlConn;
        SqlTransaction sqlTrans;
        #region 查詢 QuerySearchList()
        public SearchListViewModel QuerySearchList(SearchInfoViewModel searchInfo, string UserName, string roleName, string Organizer)
        {
            #region 參數宣告				
            SearchListViewModel searchList = new SearchListViewModel();
            ProjectControllDao projectControllDao = new ProjectControllDao();
            #endregion

            #region 流程																
            searchList = projectControllDao.QuerySearchList(searchInfo, UserName, roleName, Organizer); //將參數送入Dao層,組立SQL字串並連接資料庫

            return searchList;
            #endregion
        }
        #endregion

        #region 匯出範例檔-起
        internal DataTable getExportData(ExportViewModel exportViewModel, string UserName, string roleName, string Organizer)
        {
            #region 參數宣告				
            ProjectControllDao projectControllDao = new ProjectControllDao();
            DataTable dt = new DataTable();
            #endregion

            #region 流程																
            dt = projectControllDao.getExportData(exportViewModel, UserName, roleName, Organizer); //將參數送入Dao層,組立SQL字串並連接資料庫
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
                //dt.Columns.Add("契約金額");
                //dt.Columns.Add("開工日期");
                //dt.Columns.Add("預訂完工日期");
                //dt.Columns.Add("預定進度");
                //dt.Columns.Add("實際進度");
                //dt.Columns.Add("承辦單位(若角色是一般使用者或資料維護者，科室預設自己的科室)");
                //dt.Columns.Add("承辦人員");
                //dt.Columns.Add("備註");
            }


            return dt;
            #endregion
        }

        #endregion 匯出範例檔-迄
        #region 單筆新增ProjectControllTable-起
        internal void AddProjectControllTable(ProjectControllModel projectControllModel, string UserName)
        {
            #region 參數宣告				
            ProjectControllDao projectControllDao = new ProjectControllDao();
            #endregion

            #region 流程			
            projectControllDao.AddProjectControllTable(projectControllModel, UserName); //將參數送入Dao層,組立SQL字串並連接資料庫
            #endregion
        }

        #endregion 單筆新增ProjectControllTable-迄

        #region 單筆修改ProjectControllTable-起
        internal void UpdateProjectControllTable(ProjectControllModel projectControllModel, string UserName)
        {
            #region 參數宣告				
            ProjectControllDao projectControllDao = new ProjectControllDao();
            #endregion

            #region 流程			
            projectControllDao.UpdateProjectControllTable(projectControllModel, UserName); //將參數送入Dao層,組立SQL字串並連接資料庫
            #endregion
        }

        #endregion 單筆修改ProjectControllTable-迄

        #region 單筆刪除ProjectControllTable-起
        internal void DeleteProjectControllTable(List<string> ID)
        {
            #region 參數宣告				
            ProjectControllDao projectControllDao = new ProjectControllDao();
            #endregion

            #region 流程			
            projectControllDao.DeleteProjectControllTable(ID); //將參數送入Dao層,組立SQL字串並連接資料庫
            #endregion
        }

        #endregion 單筆刪除ProjectControllTable-迄

        #region 修改用查詢
        internal ProjectControllModel QueryUpdateData(string ID)
        {
            #region 參數宣告				
            ProjectControllModel projectControllModel = new ProjectControllModel();
            ProjectControllDao projectControllDao = new ProjectControllDao();
            #endregion

            #region 流程																
            projectControllModel = projectControllDao.QueryUpdateData(ID); //將參數送入Dao層,組立SQL字串並連接資料庫

            return projectControllModel;
            #endregion
        }

        #endregion
        #region 匯入
        internal int doUpLoad(HttpPostedFileBase file, string UserName, string roleName, string Organizer)
        {
            #region 參數宣告				
            ProjectControllModel projectControllModel = new ProjectControllModel();
            ProjectControllDao projectControllDao = new ProjectControllDao();
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
                projectControllDao.defaultSqlP(out sqlConn, out sqlTrans);
                List<ProjectControllModel> listModel = new List<ProjectControllModel>();
                for (int i = 0; i < orgDt.Rows.Count; i++)
                {
                    try
                    {
                        projectControllModel = new ProjectControllModel();
                        projectControllModel.ID = orgDt.Rows[i][0].ToString();                     //項次
                        projectControllModel.ProjectName = orgDt.Rows[i][1].ToString();            //工程名稱
                        projectControllModel.ContractAmount = Convert.ToDecimal(orgDt.Rows[i][2]); //契約金額
                        projectControllModel.BeginDate = orgDt.Rows[i][3].ToString();              //開工日期
                        projectControllModel.PlanFinishDate = orgDt.Rows[i][4].ToString();         //預訂完工日期   
                        projectControllModel.PlanScheduleExpDate = orgDt.Rows[i][5].ToString();    //預定進度        
                        projectControllModel.PlanScheduleReaDate = orgDt.Rows[i][6].ToString();    //實際進度        

                        //如果是資料維護者或是一般使用者只能是自己的科室
                        if (roleName == "maintain" || roleName == "user")
                        {
                            projectControllModel.Organizer = Organizer;
                        }
                        else
                        {
                            projectControllModel.Organizer = orgDt.Rows[i][7].ToString();              //科室
                            //判斷科室有無正確
                            if (!sysCodeDao.CheckSysCode(projectControllModel.Organizer))
                            {
                                throw new Exception("查無此科室");
                            }
                        }
                        projectControllModel.OrganizerMan = orgDt.Rows[i][8].ToString();           //承辦人 
                        projectControllModel.Remark = orgDt.Rows[i][9].ToString();                 //備註

                        //判斷無ID則新增，有ID正確就修改
                        if (!string.IsNullOrEmpty(projectControllModel.ID))
                        {
                            //若資料正確則修改
                            if (projectControllDao.QueryUpdateData(projectControllModel.ID, ref sqlConn, ref sqlTrans) != null)
                            {
                                projectControllDao.UpdateMulProjectControllTable(projectControllModel, UserName, ref sqlConn, ref sqlTrans);
                            }
                            //若沒有資料則錯誤
                            else
                            {
                                throw new Exception("");
                            }
                        }
                        else
                        {
                            projectControllDao.AddMulProjectControllTable(projectControllModel, UserName, ref sqlConn, ref sqlTrans);
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("第" + (i + 2) + "筆資料有誤，請確認");
                    }
                    //沒錯誤則Commit
                }
                projectControllDao.CommitSqlP(ref sqlConn, ref sqlTrans);
            }
            return successQty;
            #endregion
        }
        #endregion
    }
}