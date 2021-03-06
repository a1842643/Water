﻿using System;
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
                                '' as 'nothing'                                        --checkbox排序用
                                ,ROW_NUMBER() OVER(ORDER BY ID) as 'ID'            --編碼
                                ,NGuid + CONVERT(varchar,ID) as 'HID'                  --項次
                                ,ProjectName                                                 --工程名稱
                                ,AwardDate                                                 --決標日
                                ,ContractDate                                                 --訂約日
                                ,ConstructionDate                                                 --進場施工時間
                                ,Duration                                                 --原工期
                                ,Company                                                 -- 承商 
                                ,ConstructionGap                                                 --施工落差％
                                ,BehindReason                                                 --施工落後原因
                                ,Countermeasures                                                 --因應對策及預訂期程
                                ,ExtensionTimes                                                 --展期工期次數(累計)
                                ,ExtensionDays                                                 --展期工期天數(累計)
                                ,Changes                                                 --變更設計
                                ,ChangeAmount                                                 --變更設計變更增減金額(千元）
                                ,CompletedExpDate                                                 --完工預定日期
                                ,CompletedRelDate                                                 --完工實際日期
                                ,CorrectionAmount                                                 --修正契約總價(千元)
                                ,CumulativeValuation                                                 --累計估驗計價(千元)
                                ,EstimateRate                                                 --估驗款執行率
                                ,EstimateBehind                                                 --估驗款落後%
                                ,EstimateBehindReason                                                 --估驗款進度延遲因素分析 
                                ,EstimateDate                                                 -- 估驗提報日期
                                ,HandlingSituation                                                 --目前辦理情形
                                ,ContractAmount                                                 --契約金額
                                ,BeginDate           --開工日期 
                                ,PlanFinishDate           --預訂完工日期
                                ,PlanScheduleExpDate            --預定進度
                                ,PlanScheduleReaDate            --實際進度
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
            _sqlStr.Append(@" ,ProjectName                                                 as '工程名稱'
                                ,AwardDate                                                 as '決標日'
                                ,ContractDate                                                 as '訂約日'
                                ,BeginDate          as '開工日期 '
                                ,ConstructionDate                                                 as '進場施工時間'
                                ,Duration                                                 as '原工期'
                                ,Company                                                 as ' 承商 '
                                ,PlanFinishDate           as '原預訂完工日期'
                                ,ContractAmount                                                 as '契約金額（千元）'
                                ,PlanScheduleExpDate            as '預定進度％'
                                ,PlanScheduleReaDate           as '實際進度％'
                                ,ConstructionGap                                                 as '施工落差％'
                                ,BehindReason                                                 as '施工落後原因'
                                ,Countermeasures                                                 as '因應對策及預訂期程'
                                ,ExtensionTimes                                                 as '展期工期次數(累計)'
                                ,ExtensionDays                                                 as '展期工期天數(累計)'
                                ,Changes                                                 as '變更設計'
                                ,ChangeAmount                                                 as '變更設計變更增減金額(千元）'
                                ,CompletedExpDate                                                 as '完工預定日期'
                                ,CompletedRelDate                                                 as '完工實際日期'
                                ,CorrectionAmount                                                 as '修正契約總價(千元)'
                                ,CumulativeValuation                                                 as '累計估驗計價(千元)'
                                ,EstimateRate                                                 as '估驗款執行率'
                                ,EstimateBehind                                                 as '估驗款落後%'
                                ,EstimateBehindReason                                                 as '估驗款進度延遲因素分析 '
                                ,EstimateDate                                                 as ' 估驗提報日期'
                                ,HandlingSituation                                                 as '目前辦理情形'
                                ,Remark                                               as '備註'
                                ,Organizer                                            as '承辦單位(若角色是一般使用者或資料維護者，科室預設自己的科室)'
                                ,OrganizerMan                                         as '承辦人員'
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
                                , AwardDate
                                , ContractDate
                                , ConstructionDate
                                , Duration
                                , Company
                                , ConstructionGap
                                , BehindReason
                                , Countermeasures
                                , ExtensionTimes
                                , ExtensionDays
                                , Changes
                                , ChangeAmount
                                , CompletedExpDate
                                , CompletedRelDate
                                , CorrectionAmount
                                , CumulativeValuation
                                , EstimateRate
                                , EstimateBehind
                                , EstimateBehindReason
                                , EstimateDate
                                , HandlingSituation
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
                                , @AwardDate
                                , @ContractDate
                                , @ConstructionDate
                                , @Duration
                                , @Company
                                , @ConstructionGap
                                , @BehindReason
                                , @Countermeasures
                                , @ExtensionTimes
                                , @ExtensionDays
                                , @Changes
                                , @ChangeAmount
                                , @CompletedExpDate
                                , @CompletedRelDate
                                , @CorrectionAmount
                                , @CumulativeValuation
                                , @EstimateRate
                                , @EstimateBehind
                                , @EstimateBehindReason
                                , @EstimateDate
                                , @HandlingSituation
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
            _sqlParams.Add("AwardDate", model.AwardDate);
            _sqlParams.Add("ContractDate", model.ContractDate);
            _sqlParams.Add("ConstructionDate", model.ConstructionDate);
            _sqlParams.Add("Duration", model.Duration);
            _sqlParams.Add("Company", model.Company);
            _sqlParams.Add("ConstructionGap", model.ConstructionGap);
            _sqlParams.Add("BehindReason", model.BehindReason);
            _sqlParams.Add("Countermeasures", model.Countermeasures);
            _sqlParams.Add("ExtensionTimes", model.ExtensionTimes);
            _sqlParams.Add("ExtensionDays", model.ExtensionDays);
            _sqlParams.Add("Changes", model.Changes);
            _sqlParams.Add("ChangeAmount", model.ChangeAmount);
            _sqlParams.Add("CompletedExpDate", model.CompletedExpDate);
            _sqlParams.Add("CompletedRelDate", model.CompletedRelDate);
            _sqlParams.Add("CorrectionAmount", model.CorrectionAmount);
            _sqlParams.Add("CumulativeValuation", model.CumulativeValuation);
            _sqlParams.Add("EstimateRate", model.EstimateRate);
            _sqlParams.Add("EstimateBehind", model.EstimateBehind);
            _sqlParams.Add("EstimateBehindReason", model.EstimateBehindReason);
            _sqlParams.Add("EstimateDate", model.EstimateDate);
            _sqlParams.Add("HandlingSituation", model.HandlingSituation);
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
                           , AwardDate         = @AwardDate        --決標日
                           , ContractDate         = @ContractDate        --訂約日
                           , ConstructionDate         = @ConstructionDate        --進場施工時間
                           , Duration         = @Duration        --原工期
                           , Company         = @Company        -- 承商 
                           , ConstructionGap         = @ConstructionGap        --施工落差％
                           , BehindReason         = @BehindReason        --施工落後原因
                           , Countermeasures         = @Countermeasures        --因應對策及預訂期程
                           , ExtensionTimes         = @ExtensionTimes        --展期工期次數(累計)
                           , ExtensionDays         = @ExtensionDays        --展期工期天數(累計)
                           , Changes         = @Changes        --變更設計
                           , ChangeAmount         = @ChangeAmount        --變更設計變更增減金額(千元）
                           , CompletedExpDate         = @CompletedExpDate        --完工預定日期
                           , CompletedRelDate         = @CompletedRelDate        --完工實際日期
                           , CorrectionAmount         = @CorrectionAmount        --修正契約總價(千元)
                           , CumulativeValuation         = @CumulativeValuation        --累計估驗計價(千元)
                           , EstimateRate         = @EstimateRate        --估驗款執行率
                           , EstimateBehind         = @EstimateBehind        --估驗款落後%
                           , EstimateBehindReason         = @EstimateBehindReason        --估驗款進度延遲因素分析 
                           , EstimateDate         = @EstimateDate        -- 估驗提報日期
                           , HandlingSituation         = @HandlingSituation        --目前辦理情形
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
            _sqlParams.Add("AwardDate", model.AwardDate);
            _sqlParams.Add("ContractDate", model.ContractDate);
            _sqlParams.Add("ConstructionDate", model.ConstructionDate);
            _sqlParams.Add("Duration", model.Duration);
            _sqlParams.Add("Company", model.Company);
            _sqlParams.Add("ConstructionGap", model.ConstructionGap);
            _sqlParams.Add("BehindReason", model.BehindReason);
            _sqlParams.Add("Countermeasures", model.Countermeasures);
            _sqlParams.Add("ExtensionTimes", model.ExtensionTimes);
            _sqlParams.Add("ExtensionDays", model.ExtensionDays);
            _sqlParams.Add("Changes", model.Changes);
            _sqlParams.Add("ChangeAmount", model.ChangeAmount);
            _sqlParams.Add("CompletedExpDate", model.CompletedExpDate);
            _sqlParams.Add("CompletedRelDate", model.CompletedRelDate);
            _sqlParams.Add("CorrectionAmount", model.CorrectionAmount);
            _sqlParams.Add("CumulativeValuation", model.CumulativeValuation);
            _sqlParams.Add("EstimateRate", model.EstimateRate);
            _sqlParams.Add("EstimateBehind", model.EstimateBehind);
            _sqlParams.Add("EstimateBehindReason", model.EstimateBehindReason);
            _sqlParams.Add("EstimateDate", model.EstimateDate);
            _sqlParams.Add("HandlingSituation", model.HandlingSituation);
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
        internal void DeleteProjectControllTable(List<string> ID)
        {
            //組立SQL字串並連接資料庫
            StringBuilder _sqlStr = new StringBuilder();
            _sqlStr.Append(@"Delete from ProjectControll WHERE NGuid + CONVERT(varchar,ID) = @ID  ");

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
                                ,AwardDate                                                 --決標日
                                ,ContractDate                                                 --訂約日
                                ,ConstructionDate                                                 --進場施工時間
                                ,Duration                                                 --原工期
                                ,Company                                                 -- 承商 
                                ,ConstructionGap                                                 --施工落差％
                                ,BehindReason                                                 --施工落後原因
                                ,Countermeasures                                                 --因應對策及預訂期程
                                ,ExtensionTimes                                                 --展期工期次數(累計)
                                ,ExtensionDays                                                 --展期工期天數(累計)
                                ,Changes                                                 --變更設計
                                ,ChangeAmount                                                 --變更設計變更增減金額(千元）
                                ,CompletedExpDate                                                 --完工預定日期
                                ,CompletedRelDate                                                 --完工實際日期
                                ,CorrectionAmount                                                 --修正契約總價(千元)
                                ,CumulativeValuation                                                 --累計估驗計價(千元)
                                ,EstimateRate                                                 --估驗款執行率
                                ,EstimateBehind                                                 --估驗款落後%
                                ,EstimateBehindReason                                                 --估驗款進度延遲因素分析 
                                ,EstimateDate                                                 -- 估驗提報日期
                                ,HandlingSituation                                                 --目前辦理情形
                                ,ContractAmount                                                 --契約金額
                                ,ContractAmount as 'SContractAmount'                                                --契約金額
                                ,BeginDate           --開工日期 
                                ,PlanFinishDate          --預訂完工日期
                                ,PlanScheduleExpDate           --預定進度
                                ,PlanScheduleReaDate           --實際進度
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
                                ,AwardDate                                                 --決標日
                                ,ContractDate                                                 --訂約日
                                ,ConstructionDate                                                 --進場施工時間
                                ,Duration                                                 --原工期
                                ,Company                                                 -- 承商 
                                ,ConstructionGap                                                 --施工落差％
                                ,BehindReason                                                 --施工落後原因
                                ,Countermeasures                                                 --因應對策及預訂期程
                                ,ExtensionTimes                                                 --展期工期次數(累計)
                                ,ExtensionDays                                                 --展期工期天數(累計)
                                ,Changes                                                 --變更設計
                                ,ChangeAmount                                                 --變更設計變更增減金額(千元）
                                ,CompletedExpDate                                                 --完工預定日期
                                ,CompletedRelDate                                                 --完工實際日期
                                ,CorrectionAmount                                                 --修正契約總價(千元)
                                ,CumulativeValuation                                                 --累計估驗計價(千元)
                                ,EstimateRate                                                 --估驗款執行率
                                ,EstimateBehind                                                 --估驗款落後%
                                ,EstimateBehindReason                                                 --估驗款進度延遲因素分析 
                                ,EstimateDate                                                 -- 估驗提報日期
                                ,HandlingSituation                                                 --目前辦理情形
                                ,ContractAmount                                                 --契約金額
                                ,ContractAmount as 'SContractAmount'                                                --契約金額
                                ,BeginDate           --開工日期 
                                ,PlanFinishDate           --預訂完工日期
                                ,PlanScheduleExpDate           --預定進度
                                ,PlanScheduleReaDate           --實際進度
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
                           , AwardDate         = @AwardDate        --決標日
                           , ContractDate         = @ContractDate        --訂約日
                           , ConstructionDate         = @ConstructionDate        --進場施工時間
                           , Duration         = @Duration        --原工期
                           , Company         = @Company        -- 承商 
                           , ConstructionGap         = @ConstructionGap        --施工落差％
                           , BehindReason         = @BehindReason        --施工落後原因
                           , Countermeasures         = @Countermeasures        --因應對策及預訂期程
                           , ExtensionTimes         = @ExtensionTimes        --展期工期次數(累計)
                           , ExtensionDays         = @ExtensionDays        --展期工期天數(累計)
                           , Changes         = @Changes        --變更設計
                           , ChangeAmount         = @ChangeAmount        --變更設計變更增減金額(千元）
                           , CompletedExpDate         = @CompletedExpDate        --完工預定日期
                           , CompletedRelDate         = @CompletedRelDate        --完工實際日期
                           , CorrectionAmount         = @CorrectionAmount        --修正契約總價(千元)
                           , CumulativeValuation         = @CumulativeValuation        --累計估驗計價(千元)
                           , EstimateRate         = @EstimateRate        --估驗款執行率
                           , EstimateBehind         = @EstimateBehind        --估驗款落後%
                           , EstimateBehindReason         = @EstimateBehindReason        --估驗款進度延遲因素分析 
                           , EstimateDate         = @EstimateDate        -- 估驗提報日期
                           , HandlingSituation         = @HandlingSituation        --目前辦理情形
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
            _sqlParams.Add("AwardDate", model.AwardDate);
            _sqlParams.Add("ContractDate", model.ContractDate);
            _sqlParams.Add("ConstructionDate", model.ConstructionDate);
            _sqlParams.Add("Duration", model.Duration);
            _sqlParams.Add("Company", model.Company);
            _sqlParams.Add("ConstructionGap", model.ConstructionGap);
            _sqlParams.Add("BehindReason", model.BehindReason);
            _sqlParams.Add("Countermeasures", model.Countermeasures);
            _sqlParams.Add("ExtensionTimes", model.ExtensionTimes);
            _sqlParams.Add("ExtensionDays", model.ExtensionDays);
            _sqlParams.Add("Changes", model.Changes);
            _sqlParams.Add("ChangeAmount", model.ChangeAmount);
            _sqlParams.Add("CompletedExpDate", model.CompletedExpDate);
            _sqlParams.Add("CompletedRelDate", model.CompletedRelDate);
            _sqlParams.Add("CorrectionAmount", model.CorrectionAmount);
            _sqlParams.Add("CumulativeValuation", model.CumulativeValuation);
            _sqlParams.Add("EstimateRate", model.EstimateRate);
            _sqlParams.Add("EstimateBehind", model.EstimateBehind);
            _sqlParams.Add("EstimateBehindReason", model.EstimateBehindReason);
            _sqlParams.Add("EstimateDate", model.EstimateDate);
            _sqlParams.Add("HandlingSituation", model.HandlingSituation);
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
                                , AwardDate
                                , ContractDate
                                , ConstructionDate
                                , Duration
                                , Company
                                , ConstructionGap
                                , BehindReason
                                , Countermeasures
                                , ExtensionTimes
                                , ExtensionDays
                                , Changes
                                , ChangeAmount
                                , CompletedExpDate
                                , CompletedRelDate
                                , CorrectionAmount
                                , CumulativeValuation
                                , EstimateRate
                                , EstimateBehind
                                , EstimateBehindReason
                                , EstimateDate
                                , HandlingSituation
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
                                , @AwardDate
                                , @ContractDate
                                , @ConstructionDate
                                , @Duration
                                , @Company
                                , @ConstructionGap
                                , @BehindReason
                                , @Countermeasures
                                , @ExtensionTimes
                                , @ExtensionDays
                                , @Changes
                                , @ChangeAmount
                                , @CompletedExpDate
                                , @CompletedRelDate
                                , @CorrectionAmount
                                , @CumulativeValuation
                                , @EstimateRate
                                , @EstimateBehind
                                , @EstimateBehindReason
                                , @EstimateDate
                                , @HandlingSituation
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
            _sqlParams.Add("AwardDate", model.AwardDate);
            _sqlParams.Add("ContractDate", model.ContractDate);
            _sqlParams.Add("ConstructionDate", model.ConstructionDate);
            _sqlParams.Add("Duration", model.Duration);
            _sqlParams.Add("Company", model.Company);
            _sqlParams.Add("ConstructionGap", model.ConstructionGap);
            _sqlParams.Add("BehindReason", model.BehindReason);
            _sqlParams.Add("Countermeasures", model.Countermeasures);
            _sqlParams.Add("ExtensionTimes", model.ExtensionTimes);
            _sqlParams.Add("ExtensionDays", model.ExtensionDays);
            _sqlParams.Add("Changes", model.Changes);
            _sqlParams.Add("ChangeAmount", model.ChangeAmount);
            _sqlParams.Add("CompletedExpDate", model.CompletedExpDate);
            _sqlParams.Add("CompletedRelDate", model.CompletedRelDate);
            _sqlParams.Add("CorrectionAmount", model.CorrectionAmount);
            _sqlParams.Add("CumulativeValuation", model.CumulativeValuation);
            _sqlParams.Add("EstimateRate", model.EstimateRate);
            _sqlParams.Add("EstimateBehind", model.EstimateBehind);
            _sqlParams.Add("EstimateBehindReason", model.EstimateBehindReason);
            _sqlParams.Add("EstimateDate", model.EstimateDate);
            _sqlParams.Add("HandlingSituation", model.HandlingSituation);
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