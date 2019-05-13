using WaterCaseTracking;
using WaterCaseTracking.Dao;
using WaterCaseTracking.Models;
using WaterCaseTracking.Models.ViewModels.BusinessUT;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace WaterCaseTracking.Service
{
    public class BusinessUTService
    {
        #region 查詢 QuerySearchList()
        public SearchListViewModel QuerySearchList(SearchInfoViewModel searchInfo)
        {
            #region 參數宣告				
            SearchListViewModel searchList = new SearchListViewModel();
            UntilDao untilDao = new UntilDao();
            #endregion

            #region 流程																
            searchList = untilDao.QuerySearchList(searchInfo); //將參數送入Dao層,組立SQL字串並連接資料庫

            return searchList;
            #endregion
        }
        /// <summary>
        /// 修改初始值
        /// </summary>
        /// <param name="Branch_No">Branch_No</param>
        /// <returns></returns>
        #endregion
        internal UntilModel QueryUpdateData(string Branch_No)
        {
            #region 參數宣告				
            UntilModel untilModel = new UntilModel();
            UntilDao untilDao = new UntilDao();
            #endregion

            #region 流程																
            untilModel = untilDao.QueryUpdateData(Branch_No); //將參數送入Dao層,組立SQL字串並連接資料庫

            return untilModel;
            #endregion
        }

        internal void UpdateUntil(UntilModel untilModel)
        {
            #region 參數宣告				
            UntilDao untilDao = new UntilDao();
            #endregion

            #region 流程																
            untilDao.UpdateUntil(untilModel); //將參數送入Dao層,組立SQL字串並連接資料庫
            #endregion
        }

        internal void AddUntil(UntilModel untilModel)
        {
            #region 參數宣告				
            UntilDao untilDao = new UntilDao();
            #endregion

            #region 流程			
            List<UntilModel> listModel = new List<UntilModel>();
            listModel.Add(untilModel);
            untilDao.AddUntil(listModel); //將參數送入Dao層,組立SQL字串並連接資料庫
            #endregion
        }

        internal void DeleteUntil(string Branch_No)
        {
            #region 參數宣告				
            UntilDao untilDao = new UntilDao();
            #endregion

            #region 流程																
            untilDao.DeleteUntil(Branch_No); //將參數送入Dao層,組立SQL字串並連接資料庫
            #endregion
        }

        internal int doUpLoad(HttpPostedFileBase file)
        {
            #region 參數宣告				
            UntilModel untilModel = new UntilModel();
            UntilDao untilDao = new UntilDao();
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
                List<UntilModel> listModel = new List<UntilModel>();
                for (int i = 0; i < orgDt.Rows.Count; i++)
                {
                    try
                    {
                        untilModel = new UntilModel();
                        untilModel.Branch_No = orgDt.Rows[i][0].ToString();
                        untilModel.Name = orgDt.Rows[i][1].ToString();
                        untilModel.Create_Date = orgDt.Rows[i][2].ToString();
                        untilModel.AREA = orgDt.Rows[i][3].ToString();
                        untilModel.Zip_Code = orgDt.Rows[i][4].ToString();
                        untilModel.Address = orgDt.Rows[i][5].ToString();
                        untilModel.Telephone = orgDt.Rows[i][6].ToString();
                        untilModel.Fax = orgDt.Rows[i][7].ToString();
                        untilModel.DormName = orgDt.Rows[i][8].ToString();
                        untilModel.Manager = orgDt.Rows[i][9].ToString();
                        untilModel.Join_Date = orgDt.Rows[i][10].ToString();
                        untilModel.Vice_Manager_1 = orgDt.Rows[i][11].ToString();
                        untilModel.Vice_Manager_2 = orgDt.Rows[i][12].ToString();
                        untilModel.Ass_GroupName = orgDt.Rows[i][13].ToString();
                        untilModel.AreacenterName = orgDt.Rows[i][14].ToString();
                        untilModel.Gen_JurisName = orgDt.Rows[i][15].ToString();
                        untilModel.Remark = orgDt.Rows[i][16].ToString();
                        listModel.Add(untilModel);
                    }
                    catch (Exception)
                    {
                        throw new Exception("第" + i + "筆資料有誤，請確認");
                    }
                }
                SqlConnection sqlConn;
                SqlTransaction sqlTrans;
                //刪除資料
                untilDao.DeleteUpdateUntil(listModel, out sqlConn, out sqlTrans);
                //新增資料
                successQty =  untilDao.AddUpdateUntil(listModel, ref sqlConn, ref sqlTrans);
            }
            return successQty;
            #endregion
        }

        internal DataTable getExportData(ExportViewModel exportViewModel)
        {
            #region 參數宣告				
            UntilDao untilDao = new UntilDao();
            DataTable dt = new DataTable();
            #endregion

            #region 流程																
            dt = untilDao.getExportData(exportViewModel); //將參數送入Dao層,組立SQL字串並連接資料庫

            return dt;
            #endregion
        }
    }
}