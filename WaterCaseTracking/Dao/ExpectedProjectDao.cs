using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using WaterCaseTracking.Models.ViewModels.ExpectedProject;
using System.Data.SqlClient;
using Dapper;
using System.Data;
using WaterCaseTracking.Models;

namespace WaterCaseTracking.Dao
{
    public class ExpectedProjectDao : _BaseDao
    {
        #region 取得表單oTable資料-起

        public SearchListViewModel QuerySearchList(SearchInfoViewModel searchInfo, string UserName, string roleName)
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
            _sqlCountStr.Append("select count(1) from ExpectedProject WHERE 1 = 1 ");
            _sqlStr.Append(@"SELECT
                                ROW_NUMBER() OVER(ORDER BY ID ASC) as 'ID'            --編碼
                                ,NGuid + CONVERT(varchar,ID) as 'HID'                  --項次
                                ,ProjectName                                                 --工程名稱
                                ,CONVERT(VARCHAR,CrProExpDate, 111) as 'CrProExpDate'           --成案預計完成日期 
                                ,CONVERT(VARCHAR,CrProReaDate, 111) as 'CrProReaDate'           --成案實際完成日期
                                ,CONVERT(VARCHAR,PlanExpDate, 111) as 'PlanExpDate'           --規劃預計完成日期
                                ,CONVERT(VARCHAR,PlanReaDate, 111) as 'PlanReaDate'           --規劃實際完成日期
                                ,CONVERT(VARCHAR,BasDesExpDate, 111) as 'BasDesExpDate'           --基本設計預計完成日期
                                ,CONVERT(VARCHAR,BasDesReaDate, 111) as 'BasDesReaDate'           --基本設計實際完成日期
                                ,CONVERT(VARCHAR,DetailDesExpDate, 111) as 'DetailDesExpDate'           --細部設計預計完成日期
                                ,CONVERT(VARCHAR,DetailDesReaDate, 111) as 'DetailDesReaDate'           --細部設計實際完成日期
                                ,CONVERT(VARCHAR,OnlineExpDate, 111) as 'OnlineExpDate'           --上網發包預計完成日期
                                ,CONVERT(VARCHAR,OnlineReaDate, 111) as 'OnlineReaDate'           --上網發包實際完成日期
                                ,CONVERT(VARCHAR,SelectionExpDate, 111) as 'SelectionExpDate'           --評選預計完成日期
                                ,CONVERT(VARCHAR,SelectionReaDate, 111) as 'SelectionReaDate'           --評選實際完成日期
                                ,CONVERT(VARCHAR,AwardExpDate, 111) as 'AwardExpDate'           --決標時間預計完成日期
                                ,CONVERT(VARCHAR,AwardReaDate, 111) as 'AwardReaDate'           --決標時間實際完成日期
                                ,Organizer                                            --承辦單位
                                ,OrganizerMan                                         --承辦人員
                                ,CreateUserName                                       --新增人員 
                                ,CreateDate                                           --新增時間
                                ,UpdateUserName                                       --修改人員
                                ,UpdateDate                                           --修改時間  
                                  FROM ExpectedProject                                          
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
                _sqlParamStr.Append(" and CreateUserName = @UserName ");
                _sqlParams.Add("UserName", UserName);
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
        internal DataTable getExportData(ExportViewModel model, string UserName, string roleName)
        {
            //組立SQL字串並連接資料庫
            #region 參數告宣
            DataTable dt = new DataTable();
            #endregion

            #region 流程
            //查詢SQL
            StringBuilder _sqlStr = new StringBuilder();
            _sqlStr.Append(@"SELECT
                               NGuid + CONVERT(varchar,ID)                   as '項次'
                                ,ProjectName                                                 as '工程名稱'
                                ,CONVERT(VARCHAR,CrProExpDate, 111)           as '成案預計完成日期 '
                                ,CONVERT(VARCHAR,CrProReaDate, 111)            as '成案實際完成日期'
                                ,CONVERT(VARCHAR,PlanExpDate, 111)            as '規劃預計完成日期'
                                ,CONVERT(VARCHAR,PlanReaDate, 111)            as '規劃實際完成日期'
                                ,CONVERT(VARCHAR,BasDesExpDate, 111)            as '基本設計預計完成日期'
                                ,CONVERT(VARCHAR,BasDesReaDate, 111)            as '基本設計實際完成日期'
                                ,CONVERT(VARCHAR,DetailDesExpDate, 111)            as '細部設計預計完成日期'
                                ,CONVERT(VARCHAR,DetailDesReaDate, 111)            as '細部設計實際完成日期'
                                ,CONVERT(VARCHAR,OnlineExpDate, 111)             as '上網發包預計完成日期'
                                ,CONVERT(VARCHAR,OnlineReaDate, 111)            as '上網發包實際完成日期'
                                ,CONVERT(VARCHAR,SelectionExpDate, 111)            as '評選預計完成日期'
                                ,CONVERT(VARCHAR,SelectionReaDate, 111)            as '評選實際完成日期'
                                ,CONVERT(VARCHAR,AwardExpDate, 111)            as '決標時間預計完成日期'
                                ,CONVERT(VARCHAR,AwardReaDate, 111)            as '決標時間實際完成日期'
                                ,Organizer                                            as '承辦單位'
                                ,OrganizerMan                                         as '承辦人員'
                                  FROM ExpectedProject                                          
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
                _sqlStr.Append(" and CreateUserName = @UserName ");
                _sqlParams.Add("UserName", UserName);
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

        #region 單筆新增ExpectedProjectTable-起
        internal void AddExpectedProjectTable(ExpectedProjectModel model, string UserName)
        {
            //組立SQL字串並連接資料庫
            StringBuilder _sqlStr = new StringBuilder();
            _sqlStr.Append(@"Insert Into ExpectedProject ( 
                                 NGuid
                                , ProjectName
                                , CrProExpDate
                                , CrProReaDate
                                , PlanExpDate
                                , PlanReaDate
                                , BasDesExpDate
                                , BasDesReaDate
                                , DetailDesExpDate
                                , DetailDesReaDate
                                , OnlineExpDate
                                , OnlineReaDate
                                , SelectionExpDate
                                , SelectionReaDate
                                , AwardExpDate
                                , AwardReaDate
                                , Organizer
                                , OrganizerMan
                                , CreateUserName
                                , CreateDate
                                , UpdateUserName
                                , UpdateDate  
                            )
                            Values(
                                 NEWID()
                                , @ProjectName
                                , @CrProExpDate
                                , @CrProReaDate  
                                , @PlanExpDate
                                , @PlanReaDate
                                , @BasDesExpDate
                                , @BasDesReaDate
                                , @DetailDesExpDate
                                , @DetailDesReaDate
                                , @OnlineExpDate
                                , @OnlineReaDate
                                , @SelectionExpDate
                                , @SelectionReaDate
                                , @AwardExpDate
                                , @AwardReaDate
                                , @Organizer
                                , @OrganizerMan
                                , @CreateUserName
                                , getdate()
                                , @UpdateUserName
                                , getdate()       
                            )
                ");
            _sqlParamsList = new List<DynamicParameters>();
            _sqlParams = new Dapper.DynamicParameters();
            _sqlParams.Add("ProjectName", model.ProjectName);
            _sqlParams.Add("CrProExpDate", model.CrProExpDate);
            _sqlParams.Add("CrProReaDate", model.CrProReaDate);
            _sqlParams.Add("PlanExpDate", model.PlanExpDate);
            _sqlParams.Add("PlanReaDate", model.PlanReaDate);
            _sqlParams.Add("BasDesExpDate", model.BasDesExpDate);
            _sqlParams.Add("BasDesReaDate", model.BasDesReaDate);
            _sqlParams.Add("DetailDesExpDate", model.DetailDesExpDate);
            _sqlParams.Add("DetailDesReaDate", model.DetailDesReaDate);
            _sqlParams.Add("OnlineExpDate", model.OnlineExpDate);
            _sqlParams.Add("OnlineReaDate", model.OnlineReaDate);
            _sqlParams.Add("SelectionExpDate", model.SelectionExpDate);
            _sqlParams.Add("SelectionReaDate", model.SelectionReaDate);
            _sqlParams.Add("AwardExpDate", model.AwardExpDate);
            _sqlParams.Add("AwardReaDate", model.AwardReaDate);
            _sqlParams.Add("Organizer", model.Organizer);
            _sqlParams.Add("OrganizerMan", model.OrganizerMan);
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
        #endregion 單筆新增ExpectedProjectTable-迄
        #region 單筆修改ExpectedProjectTable-起
        internal void UpdateExpectedProjectTable(ExpectedProjectModel model, string UserName)
        {
            //組立SQL字串並連接資料庫
            StringBuilder _sqlStr = new StringBuilder();
            _sqlStr.Append(@"UPDATE ExpectedProject SET                            
                            ProjectName       = @ProjectName                 --工程名稱
                           , CrProExpDate      = @CrProExpDate                --成案預計完成日期 
                           , CrProReaDate      = @CrProReaDate                --成案實際完成日期
                           , PlanExpDate       = @PlanExpDate                 --規劃預計完成日期
                           , PlanReaDate       = @PlanReaDate                 --規劃實際完成日期
                           , BasDesExpDate     = @BasDesExpDate               --基本設計預計完成日期
                           , BasDesReaDate     = @BasDesReaDate               --基本設計實際完成日期
                           , DetailDesExpDate  = @DetailDesExpDate            --細部設計預計完成日期
                           , DetailDesReaDate  = @DetailDesReaDate            --細部設計實際完成日期
                           , OnlineExpDate     = @OnlineExpDate               --上網發包預計完成日期
                           , OnlineReaDate     = @OnlineReaDate               --上網發包實際完成日期
                           , SelectionExpDate  = @SelectionExpDate            --評選預計完成日期
                           , SelectionReaDate  = @SelectionReaDate            --評選實際完成日期
                           , AwardExpDate      = @AwardExpDate                --決標時間預計完成日期
                           , AwardReaDate      = @AwardReaDate                --決標時間實際完成日期
                           , Organizer         = @Organizer                   --科室
                           , OrganizerMan      = @OrganizerMan                --承辦人
                            , UpdateUserName =@UpdateUserName            --修改人員
                            , UpdateDate = GetDate()                    --修改時間
                ");
            _sqlStr.Append("WHERE NGuid + CONVERT(varchar,ID) = @ID ");

            _sqlParams = new Dapper.DynamicParameters();
            _sqlParams.Add("ID", model.ID);
            _sqlParams.Add("ProjectName", model.ProjectName);
            _sqlParams.Add("CrProExpDate", model.CrProExpDate);
            _sqlParams.Add("CrProReaDate", model.CrProReaDate);
            _sqlParams.Add("PlanExpDate", model.PlanExpDate);
            _sqlParams.Add("PlanReaDate", model.PlanReaDate);
            _sqlParams.Add("BasDesExpDate", model.BasDesExpDate);
            _sqlParams.Add("BasDesReaDate", model.BasDesReaDate);
            _sqlParams.Add("DetailDesExpDate", model.DetailDesExpDate);
            _sqlParams.Add("DetailDesReaDate", model.DetailDesReaDate);
            _sqlParams.Add("OnlineExpDate", model.OnlineExpDate);
            _sqlParams.Add("OnlineReaDate", model.OnlineReaDate);
            _sqlParams.Add("SelectionExpDate", model.SelectionExpDate);
            _sqlParams.Add("SelectionReaDate", model.SelectionReaDate);
            _sqlParams.Add("AwardExpDate", model.AwardExpDate);
            _sqlParams.Add("AwardReaDate", model.AwardReaDate);
            _sqlParams.Add("Organizer", model.Organizer);
            _sqlParams.Add("OrganizerMan", model.OrganizerMan);
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
        #endregion 單筆修改ExpectedProjectTable-迄
        #region 單筆刪除ExpectedProjectTable-起
        internal void DeleteExpectedProjectTable(string ID)
        {
            //組立SQL字串並連接資料庫
            StringBuilder _sqlStr = new StringBuilder();
            _sqlStr.Append(@"Delete from ExpectedProject WHERE NGuid + CONVERT(varchar,ID) = @ID  ");

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
        #endregion 單筆刪除ExpectedProjectTable-迄
        #region 修改用查詢
        internal ExpectedProjectModel QueryUpdateData(string ID)
        {
            #region 參數告宣
            ExpectedProjectModel result = new ExpectedProjectModel();
            #endregion

            #region 流程

            StringBuilder _sqlStr = new StringBuilder();
            _sqlStr.Append(@"select 
                                ID                                   --項次
                                , ProjectName                                                   --工程名稱
                                ,CONVERT(VARCHAR,CrProExpDate, 111) as 'CrProExpDate'           --成案預計完成日期 
                                ,CONVERT(VARCHAR,CrProReaDate, 111) as 'CrProReaDate'           --成案實際完成日期
                                ,CONVERT(VARCHAR,PlanExpDate, 111) as 'PlanExpDate'           --規劃預計完成日期
                                ,CONVERT(VARCHAR,PlanReaDate, 111) as 'PlanReaDate'           --規劃實際完成日期
                                ,CONVERT(VARCHAR,BasDesExpDate, 111) as 'BasDesExpDate'           --基本設計預計完成日期
                                ,CONVERT(VARCHAR,BasDesReaDate, 111) as 'BasDesReaDate'           --基本設計實際完成日期
                                ,CONVERT(VARCHAR,DetailDesExpDate, 111) as 'DetailDesExpDate'           --細部設計預計完成日期
                                ,CONVERT(VARCHAR,DetailDesReaDate, 111) as 'DetailDesReaDate'           --細部設計實際完成日期
                                ,CONVERT(VARCHAR,OnlineExpDate, 111) as 'OnlineExpDate'           --上網發包預計完成日期
                                ,CONVERT(VARCHAR,OnlineReaDate, 111) as 'OnlineReaDate'           --上網發包實際完成日期
                                ,CONVERT(VARCHAR,SelectionExpDate, 111) as 'SelectionExpDate'           --評選預計完成日期
                                ,CONVERT(VARCHAR,SelectionReaDate, 111) as 'SelectionReaDate'           --評選實際完成日期
                                ,CONVERT(VARCHAR,AwardExpDate, 111) as 'AwardExpDate'           --決標時間預計完成日期
                                ,CONVERT(VARCHAR,AwardReaDate, 111) as 'AwardReaDate'           --決標時間實際完成日期
                                ,Organizer                                            --承辦單位
                                ,OrganizerMan                                         --承辦人員
                            from ExpectedProject WHERE NGuid + CONVERT(varchar,ID) = @ID 
                                             ");
            _sqlParams = new Dapper.DynamicParameters();
            _sqlParams.Add("ID", ID);

            using (var cn = new SqlConnection(_dbConnPPP)) //連接資料庫
            {
                cn.Open();
                result = cn.Query<ExpectedProjectModel>(_sqlStr.ToString(), _sqlParams).FirstOrDefault();
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
        internal void UpdateMulExpectedProjectTable(ExpectedProjectModel model, string UserName, ref SqlConnection conn, ref SqlTransaction trans)
        {
            //組立SQL字串並連接資料庫
            StringBuilder _sqlStr = new StringBuilder();
            _sqlStr.Append(@"UPDATE ExpectedProject SET                            
                             ProjectName       = @ProjectName                 --工程名稱
                           , CrProExpDate      = @CrProExpDate                --成案預計完成日期 
                           , CrProReaDate      = @CrProReaDate                --成案實際完成日期
                           , PlanExpDate       = @PlanExpDate                 --規劃預計完成日期
                           , PlanReaDate       = @PlanReaDate                 --規劃實際完成日期
                           , BasDesExpDate     = @BasDesExpDate               --基本設計預計完成日期
                           , BasDesReaDate     = @BasDesReaDate               --基本設計實際完成日期
                           , DetailDesExpDate  = @DetailDesExpDate            --細部設計預計完成日期
                           , DetailDesReaDate  = @DetailDesReaDate            --細部設計實際完成日期
                           , OnlineExpDate     = @OnlineExpDate               --上網發包預計完成日期
                           , OnlineReaDate     = @OnlineReaDate               --上網發包實際完成日期
                           , SelectionExpDate  = @SelectionExpDate            --評選預計完成日期
                           , SelectionReaDate  = @SelectionReaDate            --評選實際完成日期
                           , AwardExpDate      = @AwardExpDate                --決標時間預計完成日期
                           , AwardReaDate      = @AwardReaDate                --決標時間實際完成日期
                           , Organizer         = @Organizer                   --科室
                           , OrganizerMan      = @OrganizerMan                --承辦人
                            , UpdateUserName =@UpdateUserName            --修改人員
                            , UpdateDate = GetDate()                    --修改時間
                ");
            _sqlStr.Append("WHERE NGuid + CONVERT(varchar,ID) = @ID ");

            _sqlParams = new Dapper.DynamicParameters();
            _sqlParams.Add("ID", model.ID);
            _sqlParams.Add("ProjectName", model.ProjectName);
            _sqlParams.Add("CrProExpDate", model.CrProExpDate);
            _sqlParams.Add("CrProReaDate", model.CrProReaDate);
            _sqlParams.Add("PlanExpDate", model.PlanExpDate);
            _sqlParams.Add("PlanReaDate", model.PlanReaDate);
            _sqlParams.Add("BasDesExpDate", model.BasDesExpDate);
            _sqlParams.Add("BasDesReaDate", model.BasDesReaDate);
            _sqlParams.Add("DetailDesExpDate", model.DetailDesExpDate);
            _sqlParams.Add("DetailDesReaDate", model.DetailDesReaDate);
            _sqlParams.Add("OnlineExpDate", model.OnlineExpDate);
            _sqlParams.Add("OnlineReaDate", model.OnlineReaDate);
            _sqlParams.Add("SelectionExpDate", model.SelectionExpDate);
            _sqlParams.Add("SelectionReaDate", model.SelectionReaDate);
            _sqlParams.Add("AwardExpDate", model.AwardExpDate);
            _sqlParams.Add("AwardReaDate", model.AwardReaDate);
            _sqlParams.Add("Organizer", model.Organizer);
            _sqlParams.Add("OrganizerMan", model.OrganizerMan);
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
        internal void AddMulExpectedProjectTable(ExpectedProjectModel model, string UserName, ref SqlConnection conn, ref SqlTransaction trans)
        {
            //組立SQL字串並連接資料庫
            StringBuilder _sqlStr = new StringBuilder();
            _sqlStr.Append(@"Insert Into ExpectedProject ( 
                                 NGuid
                                , ProjectName
                                , CrProExpDate
                                , CrProReaDate
                                , PlanExpDate
                                , PlanReaDate
                                , BasDesExpDate
                                , BasDesReaDate
                                , DetailDesExpDate
                                , DetailDesReaDate
                                , OnlineExpDate
                                , OnlineReaDate
                                , SelectionExpDate
                                , SelectionReaDate
                                , AwardExpDate
                                , AwardReaDate
                                , Organizer
                                , OrganizerMan
                                , CreateUserName
                                , CreateDate
                                , UpdateUserName
                                , UpdateDate 
                            )
                            Values(
                                 NEWID()
                                , @ProjectName
                                , @CrProExpDate
                                , @CrProReaDate  
                                , @PlanExpDate
                                , @PlanReaDate
                                , @BasDesExpDate
                                , @BasDesReaDate
                                , @DetailDesExpDate
                                , @DetailDesReaDate
                                , @OnlineExpDate
                                , @OnlineReaDate
                                , @SelectionExpDate
                                , @SelectionReaDate
                                , @AwardExpDate
                                , @AwardReaDate
                                , @Organizer
                                , @OrganizerMan
                                , @CreateUserName
                                , getdate()
                                , @UpdateUserName
                                , getdate()       
                            )
                ");
            _sqlParamsList = new List<DynamicParameters>();
            _sqlParams = new Dapper.DynamicParameters();
            _sqlParams.Add("ProjectName", model.ProjectName);
            _sqlParams.Add("CrProExpDate", model.CrProExpDate);
            _sqlParams.Add("CrProReaDate", model.CrProReaDate);
            _sqlParams.Add("PlanExpDate", model.PlanExpDate);
            _sqlParams.Add("PlanReaDate", model.PlanReaDate);
            _sqlParams.Add("BasDesExpDate", model.BasDesExpDate);
            _sqlParams.Add("BasDesReaDate", model.BasDesReaDate);
            _sqlParams.Add("DetailDesExpDate", model.DetailDesExpDate);
            _sqlParams.Add("DetailDesReaDate", model.DetailDesReaDate);
            _sqlParams.Add("OnlineExpDate", model.OnlineExpDate);
            _sqlParams.Add("OnlineReaDate", model.OnlineReaDate);
            _sqlParams.Add("SelectionExpDate", model.SelectionExpDate);
            _sqlParams.Add("SelectionReaDate", model.SelectionReaDate);
            _sqlParams.Add("AwardExpDate", model.AwardExpDate);
            _sqlParams.Add("AwardReaDate", model.AwardReaDate);
            _sqlParams.Add("Organizer", model.Organizer);
            _sqlParams.Add("OrganizerMan", model.OrganizerMan);
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