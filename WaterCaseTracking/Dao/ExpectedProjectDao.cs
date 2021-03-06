﻿using System;
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
            _sqlCountStr.Append("select count(1) from ExpectedProject WHERE 1 = 1 ");
            _sqlStr.Append(@"SELECT
                                '' as 'nothing'                                        --checkbox排序用
                                ,ROW_NUMBER() OVER(ORDER BY ID) as 'ID'            --編碼
                                ,NGuid + CONVERT(varchar,ID) as 'HID'                  --項次
                                ,ProjectName                                                 --工程名稱
                                ,ChiefExecutiveDate                                                 --長官交辦日期
                                ,SentAllowExpDate                                                 --發包簽准預計日期
                                ,SentAllowRelDate                                                 --發包簽准實際日期
                                ,ContractDate                                                 --訂約日期
                                ,PreSituation                                                 --前次辦理情形
                                ,HandlingSituation                                                 --目前辦理情形
                                ,CrProExpDate       --成案預計完成日期 
                                ,CrProReaDate           --成案實際完成日期
                                ,PlanExpDate          --規劃預計完成日期
                                ,PlanReaDate           --規劃實際完成日期
                                ,BasDesExpDate           --基本設計預計完成日期
                                ,BasDesReaDate         --基本設計實際完成日期
                                ,DetailDesExpDate           --細部設計預計完成日期
                                ,DetailDesReaDate          --細部設計實際完成日期
                                ,OnlineExpDate          --上網發包預計完成日期
                                ,OnlineReaDate           --上網發包實際完成日期
                                ,SelectionDate           --評選預計完成日期
                                ,AwardExpDate          --決標時間預計完成日期
                                ,AwardReaDate           --決標時間實際完成日期
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
                _sqlParams.Add("ProjectName", "%" + searchInfo.txtProjectName + "%");
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
            _sqlStr.Append(@"SELECT ");
            if (roleName != "user")
            {
                _sqlStr.Append(@" NGuid + CONVERT(varchar,ID)                           as '項次(不可修改，若要新增資料則留空白)' ");
            }
            else
            {
                _sqlStr.Append(@" NGuid + CONVERT(varchar,ID)                           as '項次(不可修改)' ");
            }
            _sqlStr.Append(@" 
                                ,ProjectName                                                 as '工程名稱'
                                ,ChiefExecutiveDate                                                 as '長官交辦日期'
                                ,CrProExpDate           as '成案預計完成日期 '
                                ,CrProReaDate            as '成案實際完成日期'
                                ,PlanExpDate            as '設計發包預計完成評選日期'
                                ,PlanReaDate            as '設計發包實際完成評選日期'
                                ,BasDesExpDate            as '基本設計預計完成日期'
                                ,BasDesReaDate            as '基本設計實際完成日期'
                                ,DetailDesExpDate            as '細部設計預計完成日期'
                                ,DetailDesReaDate            as '細部設計實際完成日期'
                                ,SentAllowExpDate                                                 as '發包簽准預計日期'
                                ,SentAllowRelDate                                                 as '發包簽准實際日期'
                                ,OnlineExpDate             as '上網發包預計完成日期'
                                ,OnlineReaDate            as '上網發包實際完成日期'
                                ,SelectionDate            as '評選日期'
                                ,AwardExpDate            as '決標時間預計完成日期'
                                ,AwardReaDate            as '決標時間實際完成日期'
                                ,ContractDate                                                 as '訂約日期'
                                ,PreSituation                                                 as '前次辦理情形'
                                ,HandlingSituation                                                 as '目前辦理情形'
                                ,Organizer                                            as '承辦單位(若角色是一般使用者或資料維護者，科室預設自己的科室)'
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

        #region 單筆新增ExpectedProjectTable-起
        internal void AddExpectedProjectTable(ExpectedProjectModel model, string UserName)
        {
            //組立SQL字串並連接資料庫
            StringBuilder _sqlStr = new StringBuilder();
            _sqlStr.Append(@"Insert Into ExpectedProject ( 
                                 NGuid
                                , ProjectName
                                , ChiefExecutiveDate
                                , SentAllowExpDate
                                , SentAllowRelDate
                                , ContractDate
                                , PreSituation
                                , HandlingSituation
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
                                , SelectionDate
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
                                , @ChiefExecutiveDate
                                , @SentAllowExpDate
                                , @SentAllowRelDate
                                , @ContractDate
                                , @PreSituation
                                , @HandlingSituation
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
                                , @SelectionDate
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
            _sqlParams.Add("ChiefExecutiveDate", model.ChiefExecutiveDate);
            _sqlParams.Add("SentAllowExpDate", model.SentAllowExpDate);
            _sqlParams.Add("SentAllowRelDate", model.SentAllowRelDate);
            _sqlParams.Add("ContractDate", model.ContractDate);
            _sqlParams.Add("PreSituation", model.PreSituation);
            _sqlParams.Add("HandlingSituation", model.HandlingSituation);
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
            _sqlParams.Add("SelectionDate", model.SelectionDate);
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
                           , ChiefExecutiveDate      = @ChiefExecutiveDate                --長官交辦日期 
                           , SentAllowExpDate      = @SentAllowExpDate                --發包簽准預計日期 
                           , SentAllowRelDate      = @SentAllowRelDate                --發包簽准實際日期 
                           , ContractDate      = @ContractDate                --訂約日期 
                           , PreSituation      = @PreSituation                --前次辦理情形 
                           , HandlingSituation      = @HandlingSituation                --目前辦理情形 
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
                           , SelectionDate  = @SelectionDate            --評選預計完成日期
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
            _sqlParams.Add("ChiefExecutiveDate", model.ChiefExecutiveDate);
            _sqlParams.Add("SentAllowExpDate", model.SentAllowExpDate);
            _sqlParams.Add("SentAllowRelDate", model.SentAllowRelDate);
            _sqlParams.Add("ContractDate", model.ContractDate);
            _sqlParams.Add("PreSituation", model.PreSituation);
            _sqlParams.Add("HandlingSituation", model.HandlingSituation);
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
            _sqlParams.Add("SelectionDate", model.SelectionDate);
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
        internal void DeleteExpectedProjectTable(List<string> ID)
        {
            //組立SQL字串並連接資料庫
            StringBuilder _sqlStr = new StringBuilder();
            _sqlStr.Append(@"Delete from ExpectedProject WHERE NGuid + CONVERT(varchar,ID) = @ID  ");

             _sqlParamsList = new List<DynamicParameters>();
            foreach (var idx in ID)
            {
                _sqlParams = new Dapper.DynamicParameters();
                _sqlParams.Add("ID", idx);
                _sqlParamsList.Add(_sqlParams);
            }


            try
            {
                using (var cn = new SqlConnection(_dbConnPPP))//連接資料庫
                {
                    cn.Open();
                    var ExecResult = cn.Execute(_sqlStr.ToString(), _sqlParamsList);
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
                                , ChiefExecutiveDate                                                   --長官交辦日期
                                , SentAllowExpDate                                                   --發包簽准預計日期
                                , SentAllowRelDate                                                   --發包簽准實際日期
                                , ContractDate                                                   --訂約日期
                                , PreSituation                                                   --前次辦理情形
                                , HandlingSituation                                                   --目前辦理情形
                                ,CrProExpDate         --成案預計完成日期 
                                ,CrProReaDate         --成案實際完成日期
                                ,PlanExpDate          --規劃預計完成日期
                                ,PlanReaDate          --規劃實際完成日期
                                ,BasDesExpDate          --基本設計預計完成日期
                                ,BasDesReaDate          --基本設計實際完成日期
                                ,DetailDesExpDate         --細部設計預計完成日期
                                ,DetailDesReaDate         --細部設計實際完成日期
                                ,OnlineExpDate           --上網發包預計完成日期
                                ,OnlineReaDate           --上網發包實際完成日期
                                ,SelectionDate         --評選預計完成日期
                                ,AwardExpDate         --決標時間預計完成日期
                                ,AwardReaDate         --決標時間實際完成日期
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

        #region 修改用查詢
        internal ExpectedProjectModel QueryUpdateData(string ID, ref SqlConnection conn, ref SqlTransaction trans)
        {
            #region 參數告宣
            ExpectedProjectModel result = new ExpectedProjectModel();
            #endregion

            #region 流程

            StringBuilder _sqlStr = new StringBuilder();
            _sqlStr.Append(@"select 
                                ID                                   --項次
                                , ProjectName                                                   --工程名稱
                                , ChiefExecutiveDate                                                   --長官交辦日期
                                , SentAllowExpDate                                                   --發包簽准預計日期
                                , SentAllowRelDate                                                   --發包簽准實際日期
                                , ContractDate                                                   --訂約日期
                                , PreSituation                                                   --前次辦理情形
                                , HandlingSituation                                                   --目前辦理情形
                                ,CrProExpDate           --成案預計完成日期 
                                ,CrProReaDate         --成案實際完成日期
                                ,PlanExpDate         --規劃預計完成日期
                                ,PlanReaDate           --規劃實際完成日期
                                ,BasDesExpDate          --基本設計預計完成日期
                                ,BasDesReaDate          --基本設計實際完成日期
                                ,DetailDesExpDate           --細部設計預計完成日期
                                ,DetailDesReaDate           --細部設計實際完成日期
                                ,OnlineExpDate          --上網發包預計完成日期
                                ,OnlineReaDate          --上網發包實際完成日期
                                ,SelectionDate           --評選日期
                                ,AwardExpDate           --決標時間預計完成日期
                                ,AwardReaDate           --決標時間實際完成日期
                                ,Organizer                                            --承辦單位
                                ,OrganizerMan                                         --承辦人員
                            from ExpectedProject WHERE NGuid + CONVERT(varchar,ID) = @ID 
                                             ");
            _sqlParams = new Dapper.DynamicParameters();
            _sqlParams.Add("ID", ID);

            try
            {
                result = conn.Query<ExpectedProjectModel>(_sqlStr.ToString(), _sqlParams, trans).FirstOrDefault();
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
                           , ChiefExecutiveDate      = @ChiefExecutiveDate                --長官交辦日期 
                           , SentAllowExpDate      = @SentAllowExpDate                --發包簽准預計日期 
                           , SentAllowRelDate      = @SentAllowRelDate                --發包簽准實際日期 
                           , ContractDate      = @ContractDate                --訂約日期 
                           , PreSituation      = @PreSituation                --前次辦理情形 
                           , HandlingSituation      = @HandlingSituation                --目前辦理情形 
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
                           , SelectionDate  = @SelectionDate            --評選預計完成日期
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
            _sqlParams.Add("ChiefExecutiveDate", model.ChiefExecutiveDate);
            _sqlParams.Add("SentAllowExpDate", model.SentAllowExpDate);
            _sqlParams.Add("SentAllowRelDate", model.SentAllowRelDate);
            _sqlParams.Add("ContractDate", model.ContractDate);
            _sqlParams.Add("PreSituation", model.PreSituation);
            _sqlParams.Add("HandlingSituation", model.HandlingSituation);
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
            _sqlParams.Add("SelectionDate", model.SelectionDate);
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
                                , ChiefExecutiveDate
                                , SentAllowExpDate
                                , SentAllowRelDate
                                , ContractDate
                                , PreSituation
                                , HandlingSituation
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
                                , SelectionDate
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
                                , @ChiefExecutiveDate
                                , @SentAllowExpDate
                                , @SentAllowRelDate
                                , @ContractDate
                                , @PreSituation
                                , @HandlingSituation
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
                                , @SelectionDate
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
            _sqlParams.Add("ChiefExecutiveDate", model.ChiefExecutiveDate);
            _sqlParams.Add("SentAllowExpDate", model.SentAllowExpDate);
            _sqlParams.Add("SentAllowRelDate", model.SentAllowRelDate);
            _sqlParams.Add("ContractDate", model.ContractDate);
            _sqlParams.Add("PreSituation", model.PreSituation);
            _sqlParams.Add("HandlingSituation", model.HandlingSituation);
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
            _sqlParams.Add("SelectionDate", model.SelectionDate);
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