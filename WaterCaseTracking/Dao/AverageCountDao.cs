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
    public class AverageCountDao : _BaseDao
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
        internal  DataTable getAverage(string area,string YM,string Branch_No,string Ass_GroupName,string AreacenterName,string Gen_JurisName)
        {
            #region 參數告宣
            DataTable dt = new DataTable();
            #endregion

            _sqlParams = new Dapper.DynamicParameters();
            _sqlParams.Add("return_value", direction: ParameterDirection.ReturnValue);
            _sqlParams.Add("area", area);
            _sqlParams.Add("YM", YM);
            _sqlParams.Add("Branch_No", Branch_No);
            _sqlParams.Add("Ass_GroupName", Ass_GroupName);
            _sqlParams.Add("AreacenterName", AreacenterName);
            _sqlParams.Add("Gen_JurisName", Gen_JurisName);
          
            

            _sqlParams.Add("type", "admin_card");

            try
            {
                using (var cn = new SqlConnection(_dbConnPPP))//連接資料庫
                {
                    cn.Open();
                    //cn.Execute("getSimpleKPI_item", _sqlParams, commandType: CommandType.StoredProcedure);
                    //dt = _sqlParams.Get<DataTable>("@return_value");
                    dt.Load(cn.ExecuteReader("getmonth_adv_Until", _sqlParams, commandType: CommandType.StoredProcedure));
                    return dt;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        internal DataTable getAveragecount(string area, string YM, string Branch_No, string Ass_GroupName, string AreacenterName, string Gen_JurisName)
        {
            #region 參數告宣
            DataTable dt = new DataTable();
            #endregion

            _sqlParams = new Dapper.DynamicParameters();
            _sqlParams.Add("return_value", direction: ParameterDirection.ReturnValue);
            _sqlParams.Add("area", area);
            _sqlParams.Add("YM", YM);
            _sqlParams.Add("Branch_No", Branch_No);
            _sqlParams.Add("Ass_GroupName", Ass_GroupName);
            _sqlParams.Add("AreacenterName", AreacenterName);
            _sqlParams.Add("Gen_JurisName", Gen_JurisName);
            _sqlParams.Add("type", "admin_card_count");

            try
            {
                using (var cn = new SqlConnection(_dbConnPPP))//連接資料庫
                {
                    cn.Open();
                    //cn.Execute("getSimpleKPI_item", _sqlParams, commandType: CommandType.StoredProcedure);
                    //dt = _sqlParams.Get<DataTable>("@return_value");
                    dt.Load(cn.ExecuteReader("getmonth_adv_Until", _sqlParams, commandType: CommandType.StoredProcedure));
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