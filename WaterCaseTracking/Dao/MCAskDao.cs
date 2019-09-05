using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using WaterCaseTracking.Models.ViewModels.MCAsk;
using System.Data.SqlClient;
using Dapper;
using System.Data;
using WaterCaseTracking.Models;

namespace WaterCaseTracking.Dao
{
    public class MCAskDao : _BaseDao
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
            _sqlCountStr.Append("select count(1) from MCAsk WHERE 1 = 1 ");
            _sqlStr.Append(@"SELECT
                                '' as 'nothing'                                        --checkbox排序用
                                ,ROW_NUMBER() OVER(ORDER BY ID DESC) as 'ID'            --編碼
                                ,NGuid + CONVERT(varchar,ID) as 'HID'                  --項次
                                ,CONVERT(VARCHAR,AskDate, 111) as 'AskDate'           --詢問日期
                                ,Area                                                 --地區
                                ,MemberName                                           --議員姓名
                                ,Inquiry                                              --詢問事項
                                ,HandlingSituation                                    --辦理情形
                                ,Organizer                                            --承辦單位
                                ,OrganizerMan                                         --承辦人員
                                ,sStatus                                              --狀態
                                ,CreateUserName                                       --新增人員 
                                ,CreateDate                                           --新增時間
                                ,UpdateUserName                                       --修改人員
                                ,UpdateDate                                           --修改時間  
                                  FROM MCAsk                                          
                                WHERE 1 = 1 ");

            _sqlParams = new Dapper.DynamicParameters();
            //詢問事項
            if (!string.IsNullOrEmpty(searchInfo.txtInquiry))
            {
                _sqlParamStr.Append(" and Inquiry like @Inquiry ");
                _sqlParams.Add("Inquiry", "%" + searchInfo.txtInquiry + "%");
            }
            //辦理情形
            if (!string.IsNullOrEmpty(searchInfo.txtHandlingSituation))
            {
                _sqlParamStr.Append(" and HandlingSituation like @HandlingSituation ");
                _sqlParams.Add("HandlingSituation", "%" + searchInfo.txtHandlingSituation + "%");
            }
            //議員姓名
            if (!string.IsNullOrEmpty(searchInfo.txtMemberName))
            {
                _sqlParamStr.Append(" and MemberName like @MemberName ");
                _sqlParams.Add("MemberName", "%" + searchInfo.txtMemberName + "%");
            }
            //地區
            if (!string.IsNullOrEmpty(searchInfo.ddlArea) && searchInfo.ddlArea != "全市")
            {
                _sqlParamStr.Append(" and (AREA like @AREA ");
                if(searchInfo.ddlArea != "無")
                    _sqlParamStr.Append(" OR AREA = '全市' ");
                _sqlParamStr.Append(" ) ");
                _sqlParams.Add("AREA", '%' + searchInfo.ddlArea + '%');
            }
            //承辦單位
            if (!string.IsNullOrEmpty(searchInfo.ddlOrganizer))
            {
                _sqlParamStr.Append(" and Organizer = @Organizer ");
                _sqlParams.Add("Organizer", searchInfo.ddlOrganizer);
            }
            //承辦單位
            if (!string.IsNullOrEmpty(searchInfo.Types))
            {
                _sqlParamStr.Append(" and Types = @Types ");
                _sqlParams.Add("Types", searchInfo.Types);
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
            _sqlStr.Append(@" ,CONVERT(VARCHAR,AskDate, 111)                        as '詢問日期'
                                ,Area                                                 as '地區'
                                ,MemberName                                           as '議員姓名'
                                ,Inquiry                                              as '詢問事項'
                                ,HandlingSituation                                    as '辦理情形'
                                ,Organizer                                            as '承辦單位(若角色是一般使用者或資料維護者，科室預設自己的科室)'
                                ,OrganizerMan                                         as '承辦人員'
                                ,sStatus                                              as '狀態'
                                  FROM MCAsk                                          
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
            // 0:市政總質詢事項
            // 1:議會案件
            if (!string.IsNullOrEmpty(model.Types))
            {
                _sqlStr.Append(" and Types = @Types ");
                _sqlParams.Add("Types", model.Types);
            }
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

        #region 單筆新增MCAskTable-起
        internal void AddMCAskTable(MCAskModel model, string UserName)
        {
            //組立SQL字串並連接資料庫
            StringBuilder _sqlStr = new StringBuilder();
            _sqlStr.Append(@"Insert Into MCAsk ( 
                                AskDate
                                , NGuid
                                , MemberName
                                , Area
                                , Inquiry
                                , HandlingSituation
                                , Organizer
                                , OrganizerMan
                                , sStatus
                                , Types
                                , CreateUserName
                                , CreateDate
                                , UpdateUserName
                                , UpdateDate       
                            )
                            Values(
                                @AskDate      
                                , NEWID()
                                , @MemberName
                                , @Area
                                , @Inquiry            
                                , @HandlingSituation
                                , @Organizer
                                , @OrganizerMan
                                , @sStatus
                                , @Types
                                , @CreateUserName
                                , getdate()
                                , @UpdateUserName
                                , getdate()       
                            )
                ");
            _sqlParamsList = new List<DynamicParameters>();
            _sqlParams = new Dapper.DynamicParameters();
            _sqlParams.Add("AskDate", model.AskDate);
            _sqlParams.Add("MemberName", model.MemberName);
            _sqlParams.Add("Area", model.Area);
            _sqlParams.Add("Inquiry", model.Inquiry);
            _sqlParams.Add("HandlingSituation", model.HandlingSituation);
            _sqlParams.Add("Organizer", model.Organizer);
            _sqlParams.Add("OrganizerMan", model.OrganizerMan);
            _sqlParams.Add("sStatus", model.sStatus);
            _sqlParams.Add("Types", model.Types);
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
        #endregion 單筆新增MCAskTable-迄
        #region 單筆修改MCAskTable-起
        internal void UpdateMCAskTable(MCAskModel model, string UserName)
        {
            //組立SQL字串並連接資料庫
            StringBuilder _sqlStr = new StringBuilder();
            _sqlStr.Append(@"UPDATE MCAsk SET                            
                            AskDate =@AskDate                            --詢問日期
                            , MemberName =@MemberName                    --地區
                            , Area =@Area                                --議員姓名
                            , Inquiry =@Inquiry                          --詢問事項
                            , HandlingSituation =@HandlingSituation      --辦理情形
                            , Organizer =@Organizer                      --承辦單位
                            , OrganizerMan =@OrganizerMan                --承辦人員
                            , sStatus =@sStatus                          --狀態
                            , UpdateUserName =@UpdateUserName            --修改人員
                            , UpdateDate = GetDate()                    --修改時間
                ");
            _sqlStr.Append("WHERE NGuid + CONVERT(varchar,ID) = @ID AND Types = @Types ");

            _sqlParams = new Dapper.DynamicParameters();
            _sqlParams.Add("ID", model.ID);
            _sqlParams.Add("AskDate", model.AskDate);
            _sqlParams.Add("MemberName", model.MemberName);
            _sqlParams.Add("Area", model.Area);
            _sqlParams.Add("Inquiry", model.Inquiry);
            _sqlParams.Add("HandlingSituation", model.HandlingSituation);
            _sqlParams.Add("Organizer", model.Organizer);
            _sqlParams.Add("OrganizerMan", model.OrganizerMan);
            _sqlParams.Add("sStatus", model.sStatus);
            _sqlParams.Add("Types", model.Types);
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
        #endregion 單筆修改MCAskTable-迄
        #region 單筆刪除MCAskTable-起
        internal void DeleteMCAskTable(List<string> ID, string Types, string UserName)
        {
            //組立SQL字串並連接資料庫
            StringBuilder _sqlStr = new StringBuilder();
            _sqlStr.Append(@"Delete from MCAsk WHERE NGuid + CONVERT(varchar,ID) = @ID AND Types = @Types  ");
            _sqlParamsList = new List<DynamicParameters>();
            foreach (var idx in ID)
            {
                _sqlParams = new Dapper.DynamicParameters();
                _sqlParams.Add("ID", idx);
                _sqlParams.Add("Types", Types);
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
        #endregion 單筆刪除MCAskTable-迄
        #region 修改用查詢
        internal MCAskModel QueryUpdateData(string ID, string Types)
        {
            #region 參數告宣
            MCAskModel result = new MCAskModel();
            #endregion

            #region 流程

            StringBuilder _sqlStr = new StringBuilder();
            _sqlStr.Append(@"select 
                                ID                                   --項次
                                , Convert(varchar(10), AskDate, 111) as 'AskDate' --詢問日期
                                , MemberName                         --地區
                                , Area                               --議員姓名
                                , Inquiry                            --詢問事項
                                , HandlingSituation                  --辦理情形
                                , Organizer                          --承辦單位
                                , OrganizerMan                       --承辦人員
                                , sStatus                            --狀態
                                , Types                              --0:市政總質詢事項, 議會案件
                                , CreateUserName                     --新增人員
                                , CreateDate                         --新增時間
                                , UpdateUserName                     --修改人員
                                , UpdateDate                         --修改時間
                            from MCAsk WHERE NGuid + CONVERT(varchar,ID) = @ID 
                                            AND  Types = @Types ");
            _sqlParams = new Dapper.DynamicParameters();
            _sqlParams.Add("ID", ID);
            _sqlParams.Add("Types", Types);

            using (var cn = new SqlConnection(_dbConnPPP)) //連接資料庫
            {
                cn.Open();
                result = cn.Query<MCAskModel>(_sqlStr.ToString(), _sqlParams).FirstOrDefault();
            }
            return result;
            #endregion
        }
        #endregion
        #region 修改用查詢Conn版
        internal MCAskModel QueryUpdateDataConn(string ID, string Types, ref SqlConnection conn, ref SqlTransaction trans)
        {
            #region 參數告宣
            MCAskModel result = new MCAskModel();
            #endregion

            #region 流程

            StringBuilder _sqlStr = new StringBuilder();
            _sqlStr.Append(@"select 
                                ID                                   --項次
                                , Convert(varchar(10), AskDate, 111) as 'AskDate' --詢問日期
                                , MemberName                         --地區
                                , Area                               --議員姓名
                                , Inquiry                            --詢問事項
                                , HandlingSituation                  --辦理情形
                                , Organizer                          --承辦單位
                                , OrganizerMan                       --承辦人員
                                , sStatus                            --狀態
                                , Types                              --0:市政總質詢事項, 議會案件
                                , CreateUserName                     --新增人員
                                , CreateDate                         --新增時間
                                , UpdateUserName                     --修改人員
                                , UpdateDate                         --修改時間
                            from MCAsk WHERE NGuid + CONVERT(varchar,ID) = @ID 
                                            AND  Types = @Types ");
            _sqlParams = new Dapper.DynamicParameters();
            _sqlParams.Add("ID", ID);
            _sqlParams.Add("Types", Types);

            try
            {
                result = conn.Query<MCAskModel>(_sqlStr.ToString(), _sqlParams, trans).FirstOrDefault();
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

        #region 多筆新增MCAskTable-起
        internal int AddMCAskListTable(List<MCAskModel> listModel, string UserName)
        {
            //組立SQL字串並連接資料庫
            StringBuilder _sqlStr = new StringBuilder();
            _sqlStr.Append(@"Insert Into MCAsk ( 
                                AskDate
                                , MemberName
                                , Area
                                , Inquiry
                                , HandlingSituation
                                , Organizer
                                , OrganizerMan
                                , sStatus
                                , Types
                                , CreateUserName
                                , CreateDate
                                , UpdateUserName
                                , UpdateDate       
                            )
                            Values(
                                @AskDate      
                                , @MemberName
                                , @Area
                                , @Inquiry            
                                , @HandlingSituation
                                , @Organizer
                                , @OrganizerMan
                                , @sStatus
                                , @Types
                                , @CreateUserName
                                , getdate()
                                , @UpdateUserName
                                , getdate()       
                            )
                ");
            _sqlParamsList = new List<DynamicParameters>();
            foreach (var model in listModel)
            {
                _sqlParams = new Dapper.DynamicParameters();
                _sqlParams.Add("AskDate", model.AskDate);
                _sqlParams.Add("MemberName", model.MemberName);
                _sqlParams.Add("Area", model.Area);
                _sqlParams.Add("Inquiry", model.Inquiry);
                _sqlParams.Add("HandlingSituation", model.HandlingSituation);
                _sqlParams.Add("Organizer", model.Organizer);
                _sqlParams.Add("OrganizerMan", model.OrganizerMan);
                _sqlParams.Add("sStatus", model.sStatus);
                _sqlParams.Add("Types", model.Types);
                _sqlParams.Add("CreateUserName", UserName);
                _sqlParams.Add("UpdateUserName", UserName);
                _sqlParamsList.Add(_sqlParams);
            }

            try
            {
                using (var cn = new SqlConnection(_dbConnPPP))//連接資料庫
                {
                    cn.Open();
                    var ExecResult = cn.Execute(_sqlStr.ToString(), _sqlParamsList);
                    return ExecResult;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        internal int DeleteMCAskListTable(List<MCAskModel> listModel)
        {
            //組立SQL字串並連接資料庫
            StringBuilder _sqlStr = new StringBuilder();
            _sqlStr.Append(@" Delete From MCAsk Where ID = @ID
                ");
            _sqlParamsList = new List<DynamicParameters>();
            foreach (var model in listModel)
            {
                _sqlParams = new Dapper.DynamicParameters();
                _sqlParams.Add("ID", model.ID);
                _sqlParamsList.Add(_sqlParams);
            }

            try
            {
                using (var cn = new SqlConnection(_dbConnPPP))//連接資料庫
                {
                    cn.Open();
                    var ExecResult = cn.Execute(_sqlStr.ToString(), _sqlParamsList);
                    return ExecResult;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion 多筆新增MCAskTable-迄

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

        internal void RollbackSqlP(ref SqlConnection conn, ref SqlTransaction trans)
        {
            TransactionRollback(trans);
            GetCloseConnection(conn);
        }
        #endregion
        #region 單筆修改Trans版
        internal void UpdateMulMCAskTable(MCAskModel model, string UserName, ref SqlConnection conn, ref SqlTransaction trans)
        {
            //組立SQL字串並連接資料庫
            StringBuilder _sqlStr = new StringBuilder();
            _sqlStr.Append(@"UPDATE MCAsk SET                            
                            AskDate =@AskDate                            --詢問日期
                            , MemberName =@MemberName                    --地區
                            , Area =@Area                                --議員姓名
                            , Inquiry =@Inquiry                          --詢問事項
                            , HandlingSituation =@HandlingSituation      --辦理情形
                            , Organizer =@Organizer                      --承辦單位
                            , OrganizerMan =@OrganizerMan                --承辦人員
                            , sStatus =@sStatus                          --狀態
                            , UpdateUserName =@UpdateUserName            --修改人員
                            , UpdateDate = GetDate()                    --修改時間
                ");
            _sqlStr.Append("WHERE NGuid + CONVERT(varchar,ID) = @ID AND Types = @Types ");

            _sqlParams = new Dapper.DynamicParameters();
            _sqlParams.Add("ID", model.ID);
            _sqlParams.Add("AskDate", model.AskDate);
            _sqlParams.Add("MemberName", model.MemberName);
            _sqlParams.Add("Area", model.Area);
            _sqlParams.Add("Inquiry", model.Inquiry);
            _sqlParams.Add("HandlingSituation", model.HandlingSituation);
            _sqlParams.Add("Organizer", model.Organizer);
            _sqlParams.Add("OrganizerMan", model.OrganizerMan);
            _sqlParams.Add("sStatus", model.sStatus);
            _sqlParams.Add("Types", model.Types);
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
        internal void AddMulMCAskTable(MCAskModel model, string UserName, ref SqlConnection conn, ref SqlTransaction trans)
        {
            //組立SQL字串並連接資料庫
            StringBuilder _sqlStr = new StringBuilder();
            _sqlStr.Append(@"Insert Into MCAsk ( 
                                AskDate
                                , NGuid
                                , MemberName
                                , Area
                                , Inquiry
                                , HandlingSituation
                                , Organizer
                                , OrganizerMan
                                , sStatus
                                , Types
                                , CreateUserName
                                , CreateDate
                                , UpdateUserName
                                , UpdateDate       
                            )
                            Values(
                                @AskDate      
                                , NEWID()
                                , @MemberName
                                , @Area
                                , @Inquiry            
                                , @HandlingSituation
                                , @Organizer
                                , @OrganizerMan
                                , @sStatus
                                , @Types
                                , @CreateUserName
                                , getdate()
                                , @UpdateUserName
                                , getdate()       
                            )
                ");
            _sqlParamsList = new List<DynamicParameters>();
            _sqlParams = new Dapper.DynamicParameters();
            _sqlParams.Add("AskDate", model.AskDate);
            _sqlParams.Add("MemberName", model.MemberName);
            _sqlParams.Add("Area", model.Area);
            _sqlParams.Add("Inquiry", model.Inquiry);
            _sqlParams.Add("HandlingSituation", model.HandlingSituation);
            _sqlParams.Add("Organizer", model.Organizer);
            _sqlParams.Add("OrganizerMan", model.OrganizerMan);
            _sqlParams.Add("sStatus", model.sStatus);
            _sqlParams.Add("Types", model.Types);
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