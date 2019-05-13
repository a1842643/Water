using Dapper;
using WaterCaseTracking.Models.ViewModels.BusinessUT;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using WaterCaseTracking.Models.ViewModels;
using WaterCaseTracking.Models;
using System.Data;

namespace WaterCaseTracking.Dao
{
    public class UntilDao : _BaseDao
    {
        #region 查詢 QuerySearchList()
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
            _sqlCountStr.Append("select count(1) from Until WHERE 1 = 1 ");
            _sqlStr.Append(@"select 
                                Branch_No                         --分行代號
                                , Name                      --分行名稱
                                , Create_Date --成立日期
                                , Zip_Code                  --郵遞區號
                                , Address                   --住址
                                , Telephone                 --電話
                                , AREA                      --地區
                                , Fax                       --傳真
                                , DormName                  --行舍
                                , Manager                   --經理
                                , Join_Date --到職日
                                , Vice_Manager_1            --副理1
                                , Vice_Manager_2            --副理2
                                , Ass_GroupName             --考核組別
                                , AreacenterName            --區域中心
                                , Gen_JurisName             --副總轄管區
                                , Remark                    --備註
                            from Until WHERE 1 = 1 ");

            _sqlParams = new Dapper.DynamicParameters();
            //地區
            if (!string.IsNullOrEmpty(searchInfo.ddlArea))
            {
                _sqlParamStr.Append(" and AREA = @AREA ");
                _sqlParams.Add("AREA", searchInfo.ddlArea);
            }
            //分行代號
            if (!string.IsNullOrEmpty(searchInfo.ddlUntil))
            {
                _sqlParamStr.Append(" and Branch_No = @Branch_No ");
                _sqlParams.Add("Branch_No", searchInfo.ddlUntil);
            }
            //考核組別代碼
            if (!string.IsNullOrEmpty(searchInfo.ddlAssessmentGroup))
            {
                _sqlParamStr.Append(" and Ass_GroupName = @Ass_GroupName ");
                _sqlParams.Add("Ass_GroupName", searchInfo.ddlAssessmentGroup);
            }
            //區域中心
            if (!string.IsNullOrEmpty(searchInfo.ddlAreacenterName))
            {
                _sqlParamStr.Append(" and AreacenterName = @AreacenterName ");
                _sqlParams.Add("AreacenterName", searchInfo.ddlAreacenterName);
            }
            //副總轄管區
            if (!string.IsNullOrEmpty(searchInfo.ddlGen_JurisName))
            {
                _sqlParamStr.Append(" and Gen_JurisName = @Gen_JurisName ");
                _sqlParams.Add("Gen_JurisName", searchInfo.ddlGen_JurisName);
            }
            //行舍
            if (!string.IsNullOrEmpty(searchInfo.ddlDormName))
            {
                _sqlParamStr.Append(" and DormName = @DormName ");
                _sqlParams.Add("DormName", searchInfo.ddlDormName);
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

        internal int AddUntil(List<UntilModel> listModel)
        {
            //組立SQL字串並連接資料庫
            StringBuilder _sqlStr = new StringBuilder();
            _sqlStr.Append(@"Insert Into Until ( 
                                Branch_No                         --分行代號
                                , Name                      --分行名稱
                                , Create_Date 			    --成立日期
                                , Zip_Code                  --郵遞區號
                                , Address                   --住址
                                , Telephone                 --電話
                                , AREA                      --地區
                                , Fax                       --傳真
                                , DormName                  --行舍
                                , Manager                   --經理
                                , Join_Date 			    --到職日
                                , Vice_Manager_1            --副理1
                                , Vice_Manager_2            --副理2
                                , Ass_GroupName             --考核組別
                                , AreacenterName            --區域中心
                                , Gen_JurisName             --副總轄管區
                                , Remark                    --備註
                            )
                            Values(
                                @Branch_No             
                                , @Name          
                                , @Create_Date 	
                                , @Zip_Code                  
                                , @Address       
                                , @Telephone     
                                , @AREA          
                                , @Fax           
                                , @DormName      
                                , @Manager       
                                , @Join_Date 	
                                , @Vice_Manager_1
                                , @Vice_Manager_2
                                , @Ass_GroupName 
                                , @AreacenterName
                                , @Gen_JurisName 
                                , @Remark        
                            )
                ");
            _sqlParamsList = new List<DynamicParameters>();
            foreach (var model in listModel)
            {
                _sqlParams = new Dapper.DynamicParameters();
                _sqlParams.Add("Branch_No", model.Branch_No);
                _sqlParams.Add("Name", model.Name);
                _sqlParams.Add("Create_Date", model.Create_Date);
                _sqlParams.Add("Zip_Code", model.Zip_Code);
                _sqlParams.Add("Address", model.Address);
                _sqlParams.Add("Telephone", model.Telephone);
                _sqlParams.Add("AREA", model.AREA);
                _sqlParams.Add("Fax", model.Fax);
                _sqlParams.Add("DormName", model.DormName);
                _sqlParams.Add("Manager", model.Manager);
                _sqlParams.Add("Join_Date", model.Join_Date);
                _sqlParams.Add("Vice_Manager_1", model.Vice_Manager_1);
                _sqlParams.Add("Vice_Manager_2", model.Vice_Manager_2);
                _sqlParams.Add("Ass_GroupName", model.Ass_GroupName);
                _sqlParams.Add("AreacenterName", model.AreacenterName);
                _sqlParams.Add("Gen_JurisName", model.Gen_JurisName);
                _sqlParams.Add("Remark", model.Remark);
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


        internal void UpdateUntil(UntilModel model)
        {
            //組立SQL字串並連接資料庫
            StringBuilder _sqlStr = new StringBuilder();
            _sqlStr.Append(@"UPDATE Until SET 
                            Name= @Name                     --分行名稱
                            , Create_Date= @Create_Date       --成立日期
                            , Zip_Code  = @Zip_Code                --郵遞區號
                            , Address= @Address               --住址
                            , Telephone= @Telephone           --電話
                            , AREA= @AREA                     --地區
                            , Fax= @Fax                       --傳真
                            , DormName= @DormName             --行舍
                            , Manager= @Manager               --經理
                            , Join_Date= @Join_Date           --到職日
                            , Vice_Manager_1= @Vice_Manager_1 --副理1
                            , Vice_Manager_2= @Vice_Manager_2 --副理2
                            , Ass_GroupName= @Ass_GroupName   --考核組別
                            , AreacenterName= @AreacenterName --區域中心
                            , Gen_JurisName= @Gen_JurisName   --副總轄管區
                            , Remark= @Remark                 --備註
                ");
            _sqlStr.Append("WHERE Branch_No = @Branch_No");

            _sqlParams = new Dapper.DynamicParameters();
            _sqlParams.Add("Branch_No", model.Branch_No);
            _sqlParams.Add("Name", model.Name);
            _sqlParams.Add("Create_Date", model.Create_Date);
            _sqlParams.Add("Zip_Code", model.Zip_Code);
            _sqlParams.Add("Address", model.Address);
            _sqlParams.Add("Telephone", model.Telephone);
            _sqlParams.Add("AREA", model.AREA);
            _sqlParams.Add("Fax", model.Fax);
            _sqlParams.Add("DormName", model.DormName);
            _sqlParams.Add("Manager", model.Manager);
            _sqlParams.Add("Join_Date", model.Join_Date);
            _sqlParams.Add("Vice_Manager_1", model.Vice_Manager_1);
            _sqlParams.Add("Vice_Manager_2", model.Vice_Manager_2);
            _sqlParams.Add("Ass_GroupName", model.Ass_GroupName);
            _sqlParams.Add("AreacenterName", model.AreacenterName);
            _sqlParams.Add("Gen_JurisName", model.Gen_JurisName);
            _sqlParams.Add("Remark", model.Remark);

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

        internal void DeleteUntil(string Branch_No)
        {
            //組立SQL字串並連接資料庫
            StringBuilder _sqlStr = new StringBuilder();
            _sqlStr.Append(@"Delete from Until WHERE Branch_No = @Branch_No");

            _sqlParams = new Dapper.DynamicParameters();
            _sqlParams.Add("Branch_No", Branch_No);


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

        internal UntilModel QueryUpdateData(string Branch_No)
        {
            //組立SQL字串並連接資料庫
            #region 參數告宣
            UntilModel result = new UntilModel();
            #endregion

            #region 流程

            StringBuilder _sqlStr = new StringBuilder();
            _sqlStr.Append(@"select 
                                Branch_No--分行代號
                                , Name--分行名稱
                                , Create_Date --成立日期
                                , Zip_Code                  --郵遞區號
                                , Address--住址
                                , Telephone--電話
                                , AREA--地區
                                , Fax--傳真
                                , DormName--行舍
                                , Manager--經理
                                , Join_Date --到職日
                                , Vice_Manager_1--副理1
                                , Vice_Manager_2--副理2
                                , Ass_GroupName--考核組別
                                , AreacenterName--區域中心
                                , Gen_JurisName--副總轄管區
                                , Remark--備註
                            from Until WHERE Branch_No = @Branch_No ");
            _sqlParams = new Dapper.DynamicParameters();
            _sqlParams.Add("Branch_No", Branch_No);

            using (var cn = new SqlConnection(_dbConnPPP)) //連接資料庫
            {
                cn.Open();
                result = cn.Query<UntilModel>(_sqlStr.ToString(), _sqlParams).First();
            }
            return result;
            #endregion
        }
        #endregion
        #region 抓下拉選單(起)
        /// <summary>
        /// 抓地區別下拉選單
        /// </summary>
        /// <returns></returns>
        internal DropDownListViewModel getddlARE()
        {
            //組立SQL字串並連接資料庫
            #region 參數告宣
            DropDownListViewModel result = new DropDownListViewModel();
            #endregion

            #region 流程

            StringBuilder _sqlStr = new StringBuilder();
            _sqlStr.Append(@"SELECT DISTINCT AREA as 'Values', AREA as 'Text' 
                             FROM Until 
                             WHERE AREA IS NOT NULL
                             Order by AREA ");
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
        /// <summary>
        /// 抓分行下拉選單
        /// </summary>
        /// <returns></returns>
        internal DropDownListViewModel getddlUntil()
        {
            //組立SQL字串並連接資料庫
            #region 參數告宣
            DropDownListViewModel result = new DropDownListViewModel();
            #endregion

            #region 流程

            StringBuilder _sqlStr = new StringBuilder();
            _sqlStr.Append(@"SELECT DISTINCT Branch_No as 'Values', Branch_No + Name as 'Text' 
                             FROM Until 
                             Order by Branch_No ");
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
        /// <summary>
        /// 依地區別抓分行下拉選單
        /// </summary>
        /// <returns></returns>
        internal DropDownListViewModel getddlUntilforArea(string Area)
        {
            //組立SQL字串並連接資料庫
            #region 參數告宣
            DropDownListViewModel result = new DropDownListViewModel();
            #endregion

            #region 流程

            StringBuilder _sqlStr = new StringBuilder();
            _sqlStr.Append(@"SELECT DISTINCT Branch_No as 'Values', Branch_No + Name as 'Text' 
                             FROM Until where Area=@Area
                             Order by Branch_No ");
            _sqlParams = new Dapper.DynamicParameters();
            _sqlParams.Add("Area", Area);
            result.DropDownListLT = new List<DropDownListItem>();
            using (var cn = new SqlConnection(_dbConnPPP)) //連接資料庫
            {
                cn.Open();
                result.DropDownListLT = cn.Query<DropDownListItem>(_sqlStr.ToString(), _sqlParams).ToList();
            }
            return result;
            #endregion
        }
        /// <summary>
        /// 依地區別抓分行下拉選單
        /// </summary>
        /// <returns></returns>
        internal DropDownListViewModel getddlUntilforAss_GroupName(string Ass_GroupName)
        {
            //組立SQL字串並連接資料庫
            #region 參數告宣
            DropDownListViewModel result = new DropDownListViewModel();
            #endregion

            #region 流程

            StringBuilder _sqlStr = new StringBuilder();
            _sqlStr.Append(@"SELECT DISTINCT Branch_No as 'Values', Branch_No + Name as 'Text' 
                             FROM Until where Ass_GroupName=@Ass_GroupName
                             Order by Branch_No ");
            _sqlParams = new Dapper.DynamicParameters();
            _sqlParams.Add("Ass_GroupName", Ass_GroupName);
            result.DropDownListLT = new List<DropDownListItem>();
            using (var cn = new SqlConnection(_dbConnPPP)) //連接資料庫
            {
                cn.Open();
                result.DropDownListLT = cn.Query<DropDownListItem>(_sqlStr.ToString(), _sqlParams).ToList();
            }
            return result;
            #endregion
        }
        /// <summary>
        /// 依地區別抓分行下拉選單
        /// </summary>
        /// <returns></returns>
        internal DropDownListViewModel getddlUntilforGen_JurisName(string Gen_JurisName)
        {
            //組立SQL字串並連接資料庫
            #region 參數告宣
            DropDownListViewModel result = new DropDownListViewModel();
            #endregion

            #region 流程

            StringBuilder _sqlStr = new StringBuilder();
            _sqlStr.Append(@"SELECT DISTINCT Branch_No as 'Values', Branch_No + Name as 'Text' 
                             FROM Until where Gen_JurisName=@Gen_JurisName
                             Order by Branch_No ");
            _sqlParams = new Dapper.DynamicParameters();
            _sqlParams.Add("Gen_JurisName", Gen_JurisName);
            result.DropDownListLT = new List<DropDownListItem>();
            using (var cn = new SqlConnection(_dbConnPPP)) //連接資料庫
            {
                cn.Open();
                result.DropDownListLT = cn.Query<DropDownListItem>(_sqlStr.ToString(), _sqlParams).ToList();
            }
            return result;
            #endregion
        }
        internal DropDownListViewModel getddlUntilforAreacenterName(string AreacenterName)
        {
            //組立SQL字串並連接資料庫
            #region 參數告宣
            DropDownListViewModel result = new DropDownListViewModel();
            #endregion

            #region 流程

            StringBuilder _sqlStr = new StringBuilder();
            _sqlStr.Append(@"SELECT DISTINCT Branch_No as 'Values', Branch_No + Name as 'Text' 
                             FROM Until where Ass_GroupName=@AreacenterName
                             Order by Branch_No ");
            _sqlParams = new Dapper.DynamicParameters();
            _sqlParams.Add("AreacenterName", AreacenterName);
            result.DropDownListLT = new List<DropDownListItem>();
            using (var cn = new SqlConnection(_dbConnPPP)) //連接資料庫
            {
                cn.Open();
                result.DropDownListLT = cn.Query<DropDownListItem>(_sqlStr.ToString(), _sqlParams).ToList();
            }
            return result;
            #endregion
        }
        internal DropDownListViewModel GetddlUntil( string Area)
        {
            //組立SQL字串並連接資料庫
            #region 參數告宣
            DropDownListViewModel result = new DropDownListViewModel();
            #endregion

            #region 流程

            StringBuilder _sqlStr = new StringBuilder();
            _sqlStr.Append(@"SELECT DISTINCT Branch_No as 'Values', Branch_No + Name as 'Text' 
                             FROM Until  where 1 = 1
                              ");
            _sqlParams = new Dapper.DynamicParameters();

            if (!string.IsNullOrEmpty(Area))
            {
                _sqlParams.Add("Area", Area);
                _sqlStr.Append(" AND Area = @Area ");
            }
            _sqlStr.Append(" Order by Branch_No ");
            result.DropDownListLT = new List<DropDownListItem>();
            using (var cn = new SqlConnection(_dbConnPPP)) //連接資料庫
            {
                cn.Open();
                result.DropDownListLT = cn.Query<DropDownListItem>(_sqlStr.ToString(), _sqlParams).ToList();
            }
            return result;
            #endregion
        }
        /// <summary>
        /// 抓考核組別下拉選單
        /// </summary>
        /// <returns></returns>
        internal DropDownListViewModel getddlAss_GroupName()
        {
            //組立SQL字串並連接資料庫
            #region 參數告宣
            DropDownListViewModel result = new DropDownListViewModel();
            #endregion

            #region 流程

            StringBuilder _sqlStr = new StringBuilder();
            _sqlStr.Append(@"SELECT DISTINCT Ass_GroupName as 'Values', Ass_GroupName as 'Text' 
                             FROM Until 
                             WHERE Ass_GroupName IS NOT NULL
                             Order by Ass_GroupName ");
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
        /// <summary>
        /// 抓區域中心
        /// </summary>
        /// <returns></returns>
        internal DropDownListViewModel getddlAreacenterName()
        {
            //組立SQL字串並連接資料庫
            #region 參數告宣
            DropDownListViewModel result = new DropDownListViewModel();
            #endregion

            #region 流程

            StringBuilder _sqlStr = new StringBuilder();
            _sqlStr.Append(@"SELECT DISTINCT AreacenterName as 'Values', AreacenterName as 'Text' 
                             FROM Until 
                             WHERE AreacenterName IS NOT NULL
                             Order by AreacenterName ");
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
        /// <summary>
        /// 抓副總轄管區
        /// </summary>
        /// <returns></returns>
        internal DropDownListViewModel getddlGen_JurisName()
        {
            //組立SQL字串並連接資料庫
            #region 參數告宣
            DropDownListViewModel result = new DropDownListViewModel();
            #endregion

            #region 流程

            StringBuilder _sqlStr = new StringBuilder();
            _sqlStr.Append(@"SELECT DISTINCT Gen_JurisName as 'Values', Gen_JurisName as 'Text' 
                             FROM Until 
                             WHERE Gen_JurisName IS NOT NULL
                             Order by Gen_JurisName ");
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
        /// <summary>
        /// 抓行舍
        /// </summary>
        /// <returns></returns>
        internal DropDownListViewModel getddlDormName()
        {
            //組立SQL字串並連接資料庫
            #region 參數告宣
            DropDownListViewModel result = new DropDownListViewModel();
            #endregion

            #region 流程

            StringBuilder _sqlStr = new StringBuilder();
            _sqlStr.Append(@"SELECT DISTINCT DormName as 'Values', DormName as 'Text' 
                             FROM Until 
                             WHERE DormName IS NOT NULL
                             Order by DormName ");
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
        #endregion 抓下拉選單(迄)

        internal DataTable getExportData(Models.ViewModels.BusinessUT.ExportViewModel model)
        {
            //組立SQL字串並連接資料庫
            #region 參數告宣
            DataTable dt = new DataTable();
            #endregion

            #region 流程
            //查詢SQL
            StringBuilder _sqlStr = new StringBuilder();
            _sqlStr.Append(@"select 
                                Branch_No                         as '分行代號'
                                , Name                      as '分行名稱'
                                , Create_Date as '成立日期'
                                , Zip_Code                  as '郵遞區號'
                                , AREA                      as '地區'
                                , Address                   as '住址'
                                , Telephone                 as '電話'
                                , Fax                       as '傳真'
                                , DormName                  as '行舍'
                                , Manager                   as '經理'
                                , Join_Date as '到職日'
                                , Vice_Manager_1            as '副理1'
                                , Vice_Manager_2            as '副理2'
                                , Ass_GroupName             as '考核組別'
                                , AreacenterName            as '區域中心'
                                , Gen_JurisName             as '副總轄管區'
                                , Remark                    as '備註'
                            from Until WHERE 1 = 1 ");

            _sqlParams = new Dapper.DynamicParameters();
            //地區
            if (!string.IsNullOrEmpty(model.ddlArea))
            {
                _sqlStr.Append(" and AREA = @AREA ");
                _sqlParams.Add("AREA", model.ddlArea);
            }
            //分行代號
            if (!string.IsNullOrEmpty(model.ddlUntil))
            {
                _sqlStr.Append(" and Branch_No = @Branch_No ");
                _sqlParams.Add("Branch_No", model.ddlUntil);
            }
            //考核組別代碼
            if (!string.IsNullOrEmpty(model.ddlAssessmentGroup))
            {
                _sqlStr.Append(" and Ass_GroupName = @Ass_GroupName ");
                _sqlParams.Add("Ass_GroupName", model.ddlAssessmentGroup);
            }
            //區域中心
            if (!string.IsNullOrEmpty(model.ddlAreacenterName))
            {
                _sqlStr.Append(" and AreacenterName = @AreacenterName ");
                _sqlParams.Add("AreacenterName", model.ddlAreacenterName);
            }
            //副總轄管區
            if (!string.IsNullOrEmpty(model.ddlGen_JurisName))
            {
                _sqlStr.Append(" and Gen_JurisName = @Gen_JurisName ");
                _sqlParams.Add("Gen_JurisName", model.ddlGen_JurisName);
            }
            //行舍
            if (!string.IsNullOrEmpty(model.ddlDormName))
            {
                _sqlStr.Append(" and DormName = @DormName ");
                _sqlParams.Add("DormName", model.ddlDormName);
            }

            //排序
            _sqlStr.Append(" order by Branch_No ");

            using (var cn = new SqlConnection(_dbConnPPP)) //連接資料庫
            {
                cn.Open();
                dt.Load(cn.ExecuteReader(_sqlStr.ToString(), _sqlParams));
            }
            return dt;
            #endregion
        }
        #region 上傳資料
        internal int AddUpdateUntil(List<UntilModel> listModel, ref SqlConnection conn, ref SqlTransaction trans)
        {
            //組立SQL字串並連接資料庫
            StringBuilder _sqlStr = new StringBuilder();
            _sqlStr.Append(@"Insert Into Until ( 
                                Branch_No                         --分行代號
                                , Name                      --分行名稱
                                , Create_Date 			    --成立日期
                                , Zip_Code                  --郵遞區號
                                , Address                   --住址
                                , Telephone                 --電話
                                , AREA                      --地區
                                , Fax                       --傳真
                                , DormName                  --行舍
                                , Manager                   --經理
                                , Join_Date 			    --到職日
                                , Vice_Manager_1            --副理1
                                , Vice_Manager_2            --副理2
                                , Ass_GroupName             --考核組別
                                , AreacenterName            --區域中心
                                , Gen_JurisName             --副總轄管區
                                , Remark                    --備註
                            )
                            Values(
                                @Branch_No             
                                , @Name          
                                , @Create_Date 	
                                , @Zip_Code       
                                , @Address       
                                , @Telephone     
                                , @AREA          
                                , @Fax           
                                , @DormName      
                                , @Manager       
                                , @Join_Date 	
                                , @Vice_Manager_1
                                , @Vice_Manager_2
                                , @Ass_GroupName 
                                , @AreacenterName
                                , @Gen_JurisName 
                                , @Remark        
                            )
                ");
            _sqlParamsList = new List<DynamicParameters>();
            foreach (var model in listModel)
            {
                _sqlParams = new Dapper.DynamicParameters();
                _sqlParams.Add("Branch_No", model.Branch_No);
                _sqlParams.Add("Name", model.Name);
                _sqlParams.Add("Create_Date", model.Create_Date);
                _sqlParams.Add("Zip_Code", model.Zip_Code);
                _sqlParams.Add("Address", model.Address);
                _sqlParams.Add("Telephone", model.Telephone);
                _sqlParams.Add("AREA", model.AREA);
                _sqlParams.Add("Fax", model.Fax);
                _sqlParams.Add("DormName", model.DormName);
                _sqlParams.Add("Manager", model.Manager);
                _sqlParams.Add("Join_Date", model.Join_Date);
                _sqlParams.Add("Vice_Manager_1", model.Vice_Manager_1);
                _sqlParams.Add("Vice_Manager_2", model.Vice_Manager_2);
                _sqlParams.Add("Ass_GroupName", model.Ass_GroupName);
                _sqlParams.Add("AreacenterName", model.AreacenterName);
                _sqlParams.Add("Gen_JurisName", model.Gen_JurisName);
                _sqlParams.Add("Remark", model.Remark);
                _sqlParamsList.Add(_sqlParams);
            }

            try
            {
                var ExecResult = conn.Execute(_sqlStr.ToString(), _sqlParamsList, trans);
                TransactionCommit(trans);
                return ExecResult;
            }
            catch (Exception ex)
            {
                TransactionRollback(trans);
                GetCloseConnection(conn);
                throw ex;
            }
            finally
            {
                GetCloseConnection(conn);
            }
        }

        internal void DeleteUpdateUntil(List<UntilModel> listModel, out SqlConnection conn, out SqlTransaction trans)
        {
            //組立SQL字串並連接資料庫
            StringBuilder _sqlStr = new StringBuilder();
            _sqlStr.Append(@"Delete from Until WHERE Branch_No = @Branch_No");

            _sqlParamsList = new List<DynamicParameters>();
            foreach (var model in listModel)
            {
                _sqlParams = new Dapper.DynamicParameters();
                _sqlParams.Add("Branch_No", model.Branch_No);
                _sqlParamsList.Add(_sqlParams);
            }

            conn = GetOpenConnection();
            trans = GetTransaction(conn);

            try
            {
                var ExecResult = conn.Execute(_sqlStr.ToString(), _sqlParamsList, trans);
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