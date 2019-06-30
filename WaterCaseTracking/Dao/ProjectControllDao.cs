using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using WaterCaseTracking.Models.ViewModels.ProjectControll;
using System.Data.SqlClient;
using Dapper;
using System.Data;
using WaterCaseTracking.Models;

namespace WaterCaseTracking.Dao
{
    public class ProjectControllDao : _BaseDao
    {
        #region 取得表單oTable資料-起

        public SearchListViewModel QuerySearchList(SearchInfoViewModel searchInfo, string UserName, string roleName, string Organizer)
        {
            //組立SQL字串並連接資料庫
            #region 參數告宣
            SearchListViewModel result = new SearchListViewModel();
            #endregion

            #region 流程
            //查詢SQL
            StringBuilder _sqlStr = new StringBuilder();
            //查詢條件SQL
            StringBuilder _sqlParamStr = new StringBuilder();
            //總筆數
            StringBuilder _sqlCountStr = new StringBuilder();
            _sqlCountStr.Append("select count(1) from ProjectControll WHERE 1 = 1 ");
            _sqlStr.Append(@"SELECT
                                ROW_NUMBER() OVER(ORDER BY ID ASC) as 'ID'            --編碼
                                ,NGuid + CONVERT(varchar,ID) as 'HID'                  --項次
                                ,ProjectName                                                 --工程名稱
                                ,ContractAmount                                                 --契約金額
                                ,CONVERT(VARCHAR,BeginDate, 111) as 'BeginDate'           --開工日期 
                                ,CONVERT(VARCHAR,PlanFinishDate, 111) as 'PlanFinishDate'           --預訂完工日期
                                ,CONVERT(VARCHAR,PlanScheduleExpDate, 111) as 'PlanScheduleExpDate'           --預定進度
                                ,CONVERT(VARCHAR,PlanScheduleReaDate, 111) as 'PlanScheduleReaDate'           --實際進度
                                ,Organizer                                            --承辦單位
                                ,OrganizerMan                                         --承辦人員
                                ,Remark                                               --備註
                                ,CreateUserName                                       --新增人員 
                                ,CreateDate                                           --新增時間
                                ,UpdateUserName                                       --修改人員
                                ,UpdateDate                                           --修改時間  
                                  FROM ProjectControll                                          
                                WHERE 1 = 1 ");

            _sqlParams = new Dapper.DynamicParameters();
            //工程名稱
            if (!string.IsNullOrEmpty(searchInfo.txtProjectName))
            {
                _sqlParamStr.Append(" and ProjectName like @ProjectName ");
                _sqlParams.Add("Inquiry", "%" + searchInfo.txtProjectName + "%");
            }
            //承辦單位
            if (!string.IsNullOrEmpty(searchInfo.ddlOrganizer))
            {
                _sqlParamStr.Append(" and Organizer = @Organizer ");
                _sqlParams.Add("Organizer", searchInfo.ddlOrganizer);
            }
            //登入角色若是User則只能查詢到自己的
            if (roleName == "user")
            {
                _sqlParamStr.Append(" and OrganizerMan = @UserName ");
                _sqlParams.Add("UserName", UserName);
            }
            //登入角色若是maintain則只能查詢到自己的
            if (roleName == "maintain")
            {
                _sqlParamStr.Append(" and Organizer = @Organizer ");
                _sqlParams.Add("Organizer", Organizer);
            }


            #region 條件、排序(起)
            _sqlStr.Append(_sqlParamStr);
            _sqlCountStr.Append(_sqlParamStr);
            _sqlStr.Append(" ORDER BY " + (searchInfo.iSortCol_0 + 1).ToString() + " " + searchInfo.sSortDir_0 + " OFFSET @iDisplayStart ROWS  FETCH NEXT @iDisplayLength ROWS ONLY ");
            _sqlParams.Add("iDisplayStart", searchInfo.iDisplayStart);
            _sqlParams.Add("iDisplayLength", searchInfo.iDisplayLength);
            #endregion 條件、排序(迄)

            result.data = new List<SearchItemViewModel>();
            result.draw = (searchInfo.sEcho + 1);
            using (var cn = new SqlConnection(_dbConnPPP)) //連接資料庫
            {
                cn.Open();
                result.data = cn.Query<SearchItemViewModel>(_sqlStr.ToString(), _sqlParams).ToList();
                result.recordsTotal = cn.Query<int>(_sqlCountStr.ToString(), _sqlParams).First();
                result.recordsFiltered = result.recordsTotal;
            }
            return result;
            #endregion
        }



        #endregion 取得表單oTable資料-迄

        #region 取得範例檔資料-起
        internal DataTable getExportData(ExportViewModel model, string UserName, string roleName, string Organizer)
        {
            //組立SQL字串並連接資料庫
            #region 參數告宣
            DataTable dt = new DataTable();
            #endregion

            #region 流程
            //查詢SQL
            StringBuilder _sqlStr = new StringBuilder();
            _sqlStr.Append(@"SELECT
                                NGuid + CONVERT(varchar,ID) as '項次(不可修改，若要新增資料則留空白)'
                                ,ProjectName                                                 as '工程名稱'
                                ,ContractAmount                                                 as '契約金額'
                                ,CONVERT(VARCHAR,BeginDate, 111)          as '開工日期 '
                                ,CONVERT(VARCHAR,PlanFinishDate, 111)           as '預訂完工日期'
                                ,CONVERT(VARCHAR,PlanScheduleExpDate, 111)            as '預定進度'
                                ,CONVERT(VARCHAR,PlanScheduleReaDate, 111)           as '實際進度'
                                ,Organizer                                            as '承辦單位(若角色是一般使用者或資料維護者，科室預設自己的科室)'
                                ,OrganizerMan                                         as '承辦人員'
                                ,Remark                                               as '備註'
                                  FROM ProjectControll                                            
                            WHERE 1 = 1 ");

            //_sqlStr.Append(@"SELECT
            //                    ''                        as '項次'
            //                    ,''                        as '詢問日期'
            //                    ,''                        as '地區'
            //                    ,''                        as '議員姓名'
            //                    ,''                        as '詢問事項'
            //                    ,''                        as '辦理情形'
            //                    ,''                        as '承辦單位'
            //                    ,''                        as '承辦人員'
            //                    ,''                        as '狀態'
            //                      ");

            _sqlParams = new Dapper.DynamicParameters();
            ////詢問事項
            //if (!string.IsNullOrEmpty(model.txtInquiry))
            //{
            //    _sqlStr.Append(" and Inquiry like @Inquiry ");
            //    _sqlParams.Add("Inquiry", "%" + model.txtInquiry + "%");
            //}
            ////辦理情形
            //if (!string.IsNullOrEmpty(model.txtHandlingSituation))
            //{
            //    _sqlStr.Append(" and HandlingSituation like @HandlingSituation ");
            //    _sqlParams.Add("HandlingSituation", "%" + model.txtHandlingSituation + "%");
            //}
            ////議員姓名
            //if (!string.IsNullOrEmpty(model.txtMemberName))
            //{
            //    _sqlStr.Append(" and MemberName like @MemberName ");
            //    _sqlParams.Add("MemberName", "%" + model.txtMemberName + "%");
            //}
            ////地區
            //if (!string.IsNullOrEmpty(model.ddlArea))
            //{
            //    _sqlStr.Append(" and AREA = @AREA ");
            //    _sqlParams.Add("AREA", model.ddlArea);
            //}
            ////承辦單位
            //if (!string.IsNullOrEmpty(model.ddlOrganizer))
            //{
            //    _sqlStr.Append(" and Organizer = @Organizer ");
            //    _sqlParams.Add("Organizer", model.ddlOrganizer);
            //}
            //登入角色若是User則只能查詢到自己的
            if (roleName == "user")
            {
                _sqlStr.Append(" and OrganizerMan = @UserName ");
                _sqlParams.Add("UserName", UserName);
            }
            //登入角色若是maintain則只能查詢到自己的
            if (roleName == "maintain")
            {
                _sqlStr.Append(" and Organizer = @Organizer ");
                _sqlParams.Add("Organizer", Organizer);
            }

            //排序
            _sqlStr.Append(" order by ID ");

            using (var cn = new SqlConnection(_dbConnPPP)) //連接資料庫
            {
                cn.Open();
                dt.Load(cn.ExecuteReader(_sqlStr.ToString(), _sqlParams));
            }
            return dt;
            #endregion
        }






        #endregion 取得範例檔資料-迄

        #region 單筆新增ProjectControllTable-起
        internal void AddProjectControllTable(ProjectControllModel model, string UserName)
        {
            //組立SQL字串並連接資料庫
            StringBuilder _sqlStr = new StringBuilder();
            _sqlStr.Append(@"Insert Into ProjectControll ( 
                                 NGuid
                                , ProjectName
                                , ContractAmount
                                , BeginDate
                                , PlanFinishDate
                                , PlanScheduleExpDate
                                , PlanScheduleReaDate
                                , Organizer
                                , OrganizerMan
                                , Remark
                                , CreateUserName
                                , CreateDate
                                , UpdateUserName
                                , UpdateDate  
                            )
                            Values(
                                 NEWID()
                                , @ProjectName
                                , @ContractAmount
                                , @BeginDate
                                , @PlanFinishDate
                                , @PlanScheduleExpDate
                                , @PlanScheduleReaDate
                                , @Organizer
                                , @OrganizerMan
                                , @Remark
                                , @CreateUserName
                                , getdate()
                                , @UpdateUserName
                                , getdate()       
                            )
                ");
            _sqlParamsList = new List<DynamicParameters>();
            _sqlParams = new Dapper.DynamicParameters();
            _sqlParams.Add("ProjectName", model.ProjectName);
            _sqlParams.Add("ContractAmount", model.ContractAmount);
            _sqlParams.Add("BeginDate", model.BeginDate);
            _sqlParams.Add("PlanFinishDate", model.PlanFinishDate);
            _sqlParams.Add("PlanScheduleExpDate", model.PlanScheduleExpDate);
            _sqlParams.Add("PlanScheduleReaDate", model.PlanScheduleReaDate);
            _sqlParams.Add("Organizer", model.Organizer);
            _sqlParams.Add("OrganizerMan", model.OrganizerMan);
            _sqlParams.Add("Remark", model.Remark);
            _sqlParams.Add("CreateUserName", UserName);
            _sqlParams.Add("UpdateUserName", UserName);

            try
            {
                using (var cn = new SqlConnection(_dbConnPPP))//連接資料庫
                {
                    cn.Open();
                    var ExecResult = cn.Execute(_sqlStr.ToString(), _sqlParams);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion 單筆新增ProjectControllTable-迄
        #region 單筆修改ProjectControllTable-起
        internal void UpdateProjectControllTable(ProjectControllModel model, string UserName)
        {
            //組立SQL字串並連接資料庫
            StringBuilder _sqlStr = new StringBuilder();
            _sqlStr.Append(@"UPDATE ProjectControll SET                            
                            ProjectName       = @ProjectName                 --工程名稱
                           , ContractAmount         = @ContractAmount        --契約金額
                           , BeginDate              = @BeginDate             --開工日期
                           , PlanFinishDate         = @PlanFinishDate        --預訂完工日期
                           , PlanScheduleExpDate    = @PlanScheduleExpDate   --預定進度
                           , PlanScheduleReaDate    = @PlanScheduleReaDate   --實際進度
                           , Organizer              = @Organizer             --科室
                           , OrganizerMan           = @OrganizerMan          --承辦人
                           , Remark                 = @Remark                --備註
                            , UpdateUserName =@UpdateUserName            --修改人員
                            , UpdateDate = GetDate()                    --修改時間
                ");
            _sqlStr.Append("WHERE NGuid + CONVERT(varchar,ID) = @ID ");

            _sqlParams = new Dapper.DynamicParameters();
            _sqlParams.Add("ID", model.ID);
            _sqlParams.Add("ProjectName", model.ProjectName);
            _sqlParams.Add("ContractAmount", model.ContractAmount);
            _sqlParams.Add("BeginDate", model.BeginDate);
            _sqlParams.Add("PlanFinishDate", model.PlanFinishDate);
            _sqlParams.Add("PlanScheduleExpDate", model.PlanScheduleExpDate);
            _sqlParams.Add("PlanScheduleReaDate", model.PlanScheduleReaDate);
            _sqlParams.Add("Organizer", model.Organizer);
            _sqlParams.Add("OrganizerMan", model.OrganizerMan);
            _sqlParams.Add("Remark", model.Remark);
            _sqlParams.Add("UpdateUserName", UserName);

            try
            {
                using (var cn = new SqlConnection(_dbConnPPP))//連接資料庫
                {
                    cn.Open();
                    var ExecResult = cn.Execute(_sqlStr.ToString(), _sqlParams);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion 單筆修改ProjectControllTable-迄
        #region 單筆刪除ProjectControllTable-起
        internal void DeleteProjectControllTable(string ID)
        {
            //組立SQL字串並連接資料庫
            StringBuilder _sqlStr = new StringBuilder();
            _sqlStr.Append(@"Delete from ProjectControll WHERE NGuid + CONVERT(varchar,ID) = @ID  ");

            _sqlParams = new Dapper.DynamicParameters();
            _sqlParams.Add("ID", ID);


            try
            {
                using (var cn = new SqlConnection(_dbConnPPP))//連接資料庫
                {
                    cn.Open();
                    var ExecResult = cn.Execute(_sqlStr.ToString(), _sqlParams);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion 單筆刪除ProjectControllTable-迄
        #region 修改用查詢
        internal ProjectControllModel QueryUpdateData(string ID, ref SqlConnection conn, ref SqlTransaction trans)
        {
            #region 參數告宣
            ProjectControllModel result = new ProjectControllModel();
            #endregion

            #region 流程

            StringBuilder _sqlStr = new StringBuilder();
            _sqlStr.Append(@"select 
                                ID                                   --項次
                                ,ProjectName                                                 --工程名稱
                                ,ContractAmount                                                 --契約金額
                                ,ContractAmount as 'SContractAmount'                                                --契約金額
                                ,CONVERT(VARCHAR,BeginDate, 111) as 'BeginDate'           --開工日期 
                                ,CONVERT(VARCHAR,PlanFinishDate, 111) as 'PlanFinishDate'           --預訂完工日期
                                ,CONVERT(VARCHAR,PlanScheduleExpDate, 111) as 'PlanScheduleExpDate'           --預定進度
                                ,CONVERT(VARCHAR,PlanScheduleReaDate, 111) as 'PlanScheduleReaDate'           --實際進度
                                ,Organizer                                            --承辦單位
                                ,OrganizerMan                                         --承辦人員
                                ,Remark                                               --備註
                                ,CreateUserName                                       --新增人員 
                                ,CreateDate                                           --新增時間
                                ,UpdateUserName                                       --修改人員
                                ,UpdateDate                                           --修改時間  
                                  FROM ProjectControll  WHERE NGuid + CONVERT(varchar,ID) = @ID 
                                             ");
            _sqlParams = new Dapper.DynamicParameters();
            _sqlParams.Add("ID", ID);

            try
            {
                result = conn.Query<ProjectControllModel>(_sqlStr.ToString(), _sqlParams, trans).FirstOrDefault();
            }
            catch (Exception ex)
            {
                TransactionRollback(trans);
                GetCloseConnection(conn);
                throw ex;
            }
            return result;
            #endregion
        }
        #endregion

        #region 修改用查詢
        internal ProjectControllModel QueryUpdateData(string ID)
        {
            #region 參數告宣
            ProjectControllModel result = new ProjectControllModel();
            #endregion

            #region 流程

            StringBuilder _sqlStr = new StringBuilder();
            _sqlStr.Append(@"select 
                                ID                                   --項次
                                ,ProjectName                                                 --工程名稱
                                ,ContractAmount                                                 --契約金額
                                ,ContractAmount as 'SContractAmount'                                                --契約金額
                                ,CONVERT(VARCHAR,BeginDate, 111) as 'BeginDate'           --開工日期 
                                ,CONVERT(VARCHAR,PlanFinishDate, 111) as 'PlanFinishDate'           --預訂完工日期
                                ,CONVERT(VARCHAR,PlanScheduleExpDate, 111) as 'PlanScheduleExpDate'           --預定進度
                                ,CONVERT(VARCHAR,PlanScheduleReaDate, 111) as 'PlanScheduleReaDate'           --實際進度
                                ,Organizer                                            --承辦單位
                                ,OrganizerMan                                         --承辦人員
                                ,Remark                                               --備註
                                ,CreateUserName                                       --新增人員 
                                ,CreateDate                                           --新增時間
                                ,UpdateUserName                                       --修改人員
                                ,UpdateDate                                           --修改時間  
                                  FROM ProjectControll  WHERE NGuid + CONVERT(varchar,ID) = @ID 
                                             ");
            _sqlParams = new Dapper.DynamicParameters();
            _sqlParams.Add("ID", ID);

            using (var cn = new SqlConnection(_dbConnPPP)) //連接資料庫
            {
                cn.Open();
                result = cn.Query<ProjectControllModel>(_sqlStr.ToString(), _sqlParams).FirstOrDefault();
            }
            return result;
            #endregion
        }
        #endregion


        #region 初始化值

        internal void defaultSqlP(out SqlConnection conn, out SqlTransaction trans)
        {
            conn = GetOpenConnection();
            trans = GetTransaction(conn);
        }
        #endregion
        #region Commit修改的值
        internal void CommitSqlP(ref SqlConnection conn, ref SqlTransaction trans)
        {
            TransactionCommit(trans);
            GetCloseConnection(conn);
        }
        #endregion
        #region 單筆修改Trans版
        internal void UpdateMulProjectControllTable(ProjectControllModel model, string UserName, ref SqlConnection conn, ref SqlTransaction trans)
        {
            //組立SQL字串並連接資料庫
            StringBuilder _sqlStr = new StringBuilder();
            _sqlStr.Append(@"UPDATE ProjectControll SET                            
                            ProjectName       = @ProjectName                 --工程名稱
                           , ContractAmount         = @ContractAmount        --契約金額
                           , BeginDate              = @BeginDate             --開工日期
                           , PlanFinishDate         = @PlanFinishDate        --預訂完工日期
                           , PlanScheduleExpDate    = @PlanScheduleExpDate   --預定進度
                           , PlanScheduleReaDate    = @PlanScheduleReaDate   --實際進度
                           , Organizer              = @Organizer             --科室
                           , OrganizerMan           = @OrganizerMan          --承辦人
                           , Remark                 = @Remark                --備註
                            , UpdateUserName =@UpdateUserName            --修改人員
                            , UpdateDate = GetDate()                    --修改時間
                ");
            _sqlStr.Append("WHERE NGuid + CONVERT(varchar,ID) = @ID ");

            _sqlParams = new Dapper.DynamicParameters();
            _sqlParams.Add("ID", model.ID);
            _sqlParams.Add("ProjectName", model.ProjectName);
            _sqlParams.Add("ContractAmount", model.ContractAmount);
            _sqlParams.Add("BeginDate", model.BeginDate);
            _sqlParams.Add("PlanFinishDate", model.PlanFinishDate);
            _sqlParams.Add("PlanScheduleExpDate", model.PlanScheduleExpDate);
            _sqlParams.Add("PlanScheduleReaDate", model.PlanScheduleReaDate);
            _sqlParams.Add("Organizer", model.Organizer);
            _sqlParams.Add("OrganizerMan", model.OrganizerMan);
            _sqlParams.Add("Remark", model.Remark);
            _sqlParams.Add("UpdateUserName", UserName);

            try
            {
                var ExecResult = conn.Execute(_sqlStr.ToString(), _sqlParams, trans);
            }
            catch (Exception ex)
            {
                TransactionRollback(trans);
                GetCloseConnection(conn);
                throw ex;
            }
        }
        #endregion
        #region 單筆新增Trans版
        internal void AddMulProjectControllTable(ProjectControllModel model, string UserName, ref SqlConnection conn, ref SqlTransaction trans)
        {
            //組立SQL字串並連接資料庫
            StringBuilder _sqlStr = new StringBuilder();
            _sqlStr.Append(@"Insert Into ProjectControll ( 
                                 NGuid
                                , ProjectName
                                , ContractAmount
                                , BeginDate
                                , PlanFinishDate
                                , PlanScheduleExpDate
                                , PlanScheduleReaDate
                                , Organizer
                                , OrganizerMan
                                , Remark
                                , CreateUserName
                                , CreateDate
                                , UpdateUserName
                                , UpdateDate  
                            )
                            Values(
                                 NEWID()
                                , @ProjectName
                                , @ContractAmount
                                , @BeginDate
                                , @PlanFinishDate
                                , @PlanScheduleExpDate
                                , @PlanScheduleReaDate
                                , @Organizer
                                , @OrganizerMan
                                , @Remark
                                , @CreateUserName
                                , getdate()
                                , @UpdateUserName
                                , getdate()       
                            )
                ");
            _sqlParamsList = new List<DynamicParameters>();
            _sqlParams = new Dapper.DynamicParameters();
            _sqlParams.Add("ProjectName", model.ProjectName);
            _sqlParams.Add("ContractAmount", model.ContractAmount);
            _sqlParams.Add("BeginDate", model.BeginDate);
            _sqlParams.Add("PlanFinishDate", model.PlanFinishDate);
            _sqlParams.Add("PlanScheduleExpDate", model.PlanScheduleExpDate);
            _sqlParams.Add("PlanScheduleReaDate", model.PlanScheduleReaDate);
            _sqlParams.Add("Organizer", model.Organizer);
            _sqlParams.Add("OrganizerMan", model.OrganizerMan);
            _sqlParams.Add("Remark", model.Remark);
            _sqlParams.Add("CreateUserName", UserName);
            _sqlParams.Add("UpdateUserName", UserName);

            try
            {
                var ExecResult = conn.Execute(_sqlStr.ToString(), _sqlParams, trans);
            }
            catch (Exception ex)
            {
                TransactionRollback(trans);
                GetCloseConnection(conn);
                throw ex;
            }
        }
        #endregion
    }
}