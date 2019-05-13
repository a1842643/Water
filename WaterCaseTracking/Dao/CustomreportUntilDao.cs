using Dapper;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using WaterCaseTracking.Models.Common;
using WaterCaseTracking.Models;
using System.Data;
using WaterCaseTracking.Models.ViewModels;
using System.Linq;
namespace WaterCaseTracking.Dao
{
    public class CustomreportUntilDao : _BaseDao
    {
       
            internal DropDownListViewModel getddlYM()
            {
                //組立SQL字串並連接資料庫
                #region 參數告宣
                DropDownListViewModel result = new DropDownListViewModel();
                #endregion

                #region 流程

                StringBuilder _sqlStr = new StringBuilder();
                _sqlStr.Append(@"SELECT DISTINCT case when Len(YM)=4 then '0'+YM else YM end  as 'Values', substring(YM, 1, LEN(YM) - 2) + '年' + RIGHT(YM, 2) + '月' as 'Text'
                             FROM Admin_card_count  order by  'Values'  desc   ");
                _sqlParams = new Dapper.DynamicParameters();

                result.DropDownListLT = new List<DropDownListItem>();
                using (var cn = new SqlConnection(_dbConnPPP)) //連接資料庫
                {
                    cn.Open();
                    result.DropDownListLT = cn.Query<DropDownListItem>(_sqlStr.ToString(), _sqlParams).ToList();
                }
                return result;
                #endregion
            }
            internal DropDownListViewModel getddFileColumns()
            {
                //組立SQL字串並連接資料庫
                #region 參數告宣
                DropDownListViewModel result = new DropDownListViewModel();
                #endregion

                #region 流程

                StringBuilder _sqlStr = new StringBuilder();
                _sqlStr.Append(@"SELECT DISTINCT ColumnID as 'Values', ChinColumns as 'Text'
                             FROM Table_Colums   where UPID is not null    ");
                _sqlParams = new Dapper.DynamicParameters();

                result.DropDownListLT = new List<DropDownListItem>();
                using (var cn = new SqlConnection(_dbConnPPP)) //連接資料庫
                {
                    cn.Open();
                    result.DropDownListLT = cn.Query<DropDownListItem>(_sqlStr.ToString(), _sqlParams).ToList();
                }
                return result;
                #endregion
            }
            internal DropDownListViewModel getddUPFileColumns(string Class)
            {
                //組立SQL字串並連接資料庫
                #region 參數告宣
                DropDownListViewModel result = new DropDownListViewModel();
                #endregion

                #region 流程

                StringBuilder _sqlStr = new StringBuilder();
                _sqlStr.Append(@"SELECT DISTINCT ColumnID as 'Values', ChinColumns as 'Text'
                             FROM Table_Colums   where UPID is null and   tablename =@tablename");
                _sqlParams = new Dapper.DynamicParameters();
                _sqlParams.Add("tablename", Class);
                result.DropDownListLT = new List<DropDownListItem>();
                using (var cn = new SqlConnection(_dbConnPPP)) //連接資料庫
                {
                    cn.Open();
                    result.DropDownListLT = cn.Query<DropDownListItem>(_sqlStr.ToString(), _sqlParams).ToList();
                }
                return result;
                #endregion
            }
            internal DropDownListViewModel getddFileColumns(string group, string Class)
            {
                //組立SQL字串並連接資料庫
                #region 參數告宣
                DropDownListViewModel result = new DropDownListViewModel();
                #endregion

                #region 流程

                StringBuilder _sqlStr = new StringBuilder();
                _sqlStr.Append(@"SELECT DISTINCT ColumnID as 'Values', ChinColumns as 'Text'
                             FROM Table_Colums   where UPID is not  null    ");
                if (!string.IsNullOrEmpty(group))
                {
                    _sqlStr.Append(@" and  UPID=@ColumnID");
                }
                if (!string.IsNullOrEmpty(Class))
                {
                    _sqlStr.Append(@" and  tablename=@tablename");
                }
                _sqlParams = new Dapper.DynamicParameters();
                _sqlParams.Add("ColumnID", group);
                _sqlParams.Add("tablename", Class);
                result.DropDownListLT = new List<DropDownListItem>();
                using (var cn = new SqlConnection(_dbConnPPP)) //連接資料庫
                {
                    cn.Open();
                    result.DropDownListLT = cn.Query<DropDownListItem>(_sqlStr.ToString(), _sqlParams).ToList();
                }
                return result;
                #endregion
            }
           
            internal DataTable getCustomreport(string Branch_No, string Filecolumn)
            {
                #region 參數告宣
                DataTable dt = new DataTable();
                #endregion

                _sqlParams = new Dapper.DynamicParameters();
                _sqlParams.Add("return_value", direction: ParameterDirection.ReturnValue);
               
                _sqlParams.Add("@FileNames", Branch_No);

                _sqlParams.Add("inputfilecolumn", Filecolumn);

                try
                 {
                    using (var cn = new SqlConnection(_dbConnPPP))//連接資料庫
                    {
                        cn.Open();
                        //cn.Execute("getSimpleKPI_item", _sqlParams, commandType: CommandType.StoredProcedure);
                        //dt = _sqlParams.Get<DataTable>("@return_value");
                        dt.Load(cn.ExecuteReader("getCustomReport", _sqlParams, commandType: CommandType.StoredProcedure));
                        return dt;
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }

            }
        }
    
}