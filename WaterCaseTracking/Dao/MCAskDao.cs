using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using WaterCaseTracking.Models.ViewModels.MCAsk;
using System.Data.SqlClient;
using Dapper;
using System.Data;

namespace WaterCaseTracking.Dao
{
    public class MCAskDao : _BaseDao
    {
        #region 取得表單oTable資料-起

        public SearchListViewModel QuerySearchList(SearchInfoViewModel searchInfo)
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
                                ID                                                    --項次
                                ,CONVERT(VARCHAR,AskDate, 111) as 'AskDate'           --詢問日期
                                ,Area                                                 --地區
                                ,MemberName                                           --議員姓名
                                ,Inquiry                                              --詢問事項
                                ,HandlingSituation                                    --辦理情形
                                ,Organizer                                            --承辦單位
                                ,OrganizerMan                                         --承辦人員
                                ,sStatus                                              --狀態
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
            if (!string.IsNullOrEmpty(searchInfo.ddlArea))
            {
                _sqlParamStr.Append(" and AREA = @AREA ");
                _sqlParams.Add("AREA", searchInfo.ddlArea);
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
        internal DataTable getExportData(ExportViewModel model)
        {
            //組立SQL字串並連接資料庫
            #region 參數告宣
            DataTable dt = new DataTable();
            #endregion

            #region 流程
            //查詢SQL
            StringBuilder _sqlStr = new StringBuilder();
            _sqlStr.Append(@"SELECT
                                ID                                                    as '項次'
                                ,CONVERT(VARCHAR,AskDate, 111)                        as '詢問日期'
                                ,Area                                                 as '地區'
                                ,MemberName                                           as '議員姓名'
                                ,Inquiry                                              as '詢問事項'
                                ,HandlingSituation                                    as '辦理情形'
                                ,Organizer                                            as '承辦單位'
                                ,OrganizerMan                                         as '承辦人員'
                                ,sStatus                                              as '狀態'
                                  FROM MCAsk                                          
                            WHERE 1 = 1 ");

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
    }
}