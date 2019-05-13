using Dapper;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using WaterCaseTracking.Models.Common;
using System.Data;
using WaterCaseTracking.Models.ViewModels.KPI_ManageQuery;
using WaterCaseTracking.Models.ViewModels;

namespace WaterCaseTracking.Dao
{
    public class StoredProcedureDao : _BaseDao
    {
        internal DataTable getKPISimple(string ddlKPIPerVal)
        {
            #region 參數告宣
            DataTable dt = new DataTable();
            #endregion

            _sqlParams = new Dapper.DynamicParameters();
            _sqlParams.Add("return_value", direction: ParameterDirection.ReturnValue);
            _sqlParams.Add("UUID", Guid.Parse(ddlKPIPerVal));
            _sqlParams.Add("year", DateTime.Now.Year - 1911);

            
            try
            {
                using (var cn = new SqlConnection(_dbConnPPP))//連接資料庫
                {
                    cn.Open();
                    //cn.Execute("getSimpleKPI_item", _sqlParams, commandType: CommandType.StoredProcedure);
                    //dt = _sqlParams.Get<DataTable>("@return_value");
                    dt.Load(cn.ExecuteReader("getSimpleManage", _sqlParams, commandType: CommandType.StoredProcedure));
                    return dt;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        internal DataTable getKPIPerSimple(string ddlKPIPerVal)
        {
            #region 參數告宣
            DataTable dt = new DataTable();
            #endregion

            _sqlParams = new Dapper.DynamicParameters();
            _sqlParams.Add("return_value", direction: ParameterDirection.ReturnValue);
            _sqlParams.Add("UUID", Guid.Parse(ddlKPIPerVal));
            _sqlParams.Add("year", DateTime.Now.Year - 1911);


            try
            {
                using (var cn = new SqlConnection(_dbConnPPP))//連接資料庫
                {
                    cn.Open();
                    //cn.Execute("getSimpleKPI_item", _sqlParams, commandType: CommandType.StoredProcedure);
                    //dt = _sqlParams.Get<DataTable>("@return_value");
                    dt.Load(cn.ExecuteReader("getSimpletPerformance", _sqlParams, commandType: CommandType.StoredProcedure));
                    return dt;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        internal DataTable getKPIMReport(ExportViewModel exportViewModel, string SPName)
        {
            #region 參數告宣
            DataTable dt = new DataTable();
            
            #endregion


            _sqlParams = new Dapper.DynamicParameters();
            _sqlParams.Add("year", exportViewModel.ddlYeart);
            if (!string.IsNullOrEmpty(exportViewModel.ddlMonth))
            {
                _sqlParams.Add("months", exportViewModel.ddlMonth);
            }
            if (!string.IsNullOrEmpty(exportViewModel.ddlLevels))
            {
                _sqlParams.Add("LEVEL", exportViewModel.ddlLevels);
            }
            _sqlParams.Add("AREA", exportViewModel.ddlArea);
            _sqlParams.Add("Ass_GroupName", exportViewModel.ddlAssessmentGroup);
            _sqlParams.Add("AreacenterName", exportViewModel.ddlAreacenterName);
            _sqlParams.Add("Gen_JurisName", exportViewModel.ddlGen_JurisName);
            _sqlParams.Add("DormName", exportViewModel.ddlDormName);
            _sqlParams.Add("Branch_No", exportViewModel.ddlUntil);


            try
            {
                using (var cn = new SqlConnection(_dbConnPPP))//連接資料庫
                {
                    cn.Open();
                    dt.Load(cn.ExecuteReader(SPName, _sqlParams, commandType: CommandType.StoredProcedure));
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