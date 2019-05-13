using Dapper;
using WaterCaseTracking.Models.ViewModels.KPI_Performance.Maintain;
using WaterCaseTracking.Models.ViewModels.ProductList;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;

namespace WaterCaseTracking.Dao.KPI_Performance
{
    public class MaintainDao : _BaseDao
    {
        #region 查詢 項目 QuerySearchList()
        public SearchListViewMode QuerySearchList(SearchInfoViewMode searchInfo)
        {
            //組立SQL字串並連接資料庫
            #region 參數告宣
            SearchListViewMode result = new SearchListViewMode();
            DateTime dt = DateTime.Now;
            #endregion

            #region 流程

            StringBuilder _sqlStr = new StringBuilder();
            StringBuilder _sqlCountStr = new StringBuilder();
            _sqlCountStr.Append("select count(*) from KPI_item where UPID = @UPID ");
            _sqlStr.Append("SELECT i.UUID as UUID ,i.SUBJECT as SUBJECT ,i.UPID as UPID, d.DISBUTION as Distribution, d.YEART, d.MODIFY_DATE, d.MODIFY_USER FROM KPI_item i " +
                "join ( select * from KPI_Distribution  where YEART = @Year) d on d.UUID = i.UUID " +
                "where i.UPID = @UPID " +
                "ORDER BY Sql ");

            Guid uuid = Guid.Parse(searchInfo.CmbItemId);

            _sqlParams = new Dapper.DynamicParameters();
            _sqlParams.Add("UPID", uuid);
            _sqlParams.Add("Year", (dt.Year - 1911));


            result.data = new List<SearchItemViewMode>();
            result.draw = (searchInfo.sEcho + 1);
            using (var cn = new SqlConnection(_dbConnPPP)) //連接資料庫
            {
                cn.Open();
                result.data = cn.Query<SearchItemViewMode>(_sqlStr.ToString(), _sqlParams).ToList();
                result.recordsTotal = cn.Query<int>(_sqlCountStr.ToString(), _sqlParams).First();
                result.recordsFiltered = result.recordsTotal;
            }
            return result;
            #endregion
        }
        #endregion
       
        #region 查詢 項目樹 QueryTreeList()
        public SearchListViewMode QueryTreeList(SearchInfoViewMode searchInfo)
        {
            //組立SQL字串並連接資料庫
            #region 參數告宣
            SearchListViewMode result = new SearchListViewMode();
            List<ItemTreeViewMode> tree = new List<ItemTreeViewMode>();
            #endregion

            #region 流程
            StringBuilder item_sqlStr = new StringBuilder();

            item_sqlStr.Append("SELECT " +
                "convert(varchar(40),a.UUID) as ID, " +
                "CASE LEVELS WHEN 2 THEN '#' " +
                "ELSE convert(varchar(40), UPID) END as parent, " +
                "CASE LEVELS WHEN 2 THEN  " +
                "(SUBJECT + ' ( ' + " +
                "convert(" +
                "varchar(40), (SELECT  ISNULL(SUM(b.DISBUTION) ,0) " +
                "from KPI_item a " +
                "inner join KPI_Distribution b on a.UUID = b.UUID " +
                "where GROUPS = '業績績效' and b.YEART = @Year and LEVELS = 3" +
                ")" +
                ") + ' 分 ) ') " +
                "ELSE (SUBJECT + ' ( ' + convert(varchar(40), b.DISBUTION) + ' 分 ) ') END as text " +
                "from KPI_item a " +
                "inner join KPI_Distribution b on a.UUID = b.UUID " +
                "where GROUPS = '業績績效' and b.YEART = @Year and LEVELS >= 2 " +
                "ORDER BY Sql ");
            //item_sqlStr.Append("SELECT convert(varchar(40),a.UUID) as ID, " +
            //    "CASE LEVELS WHEN 2 THEN '#' ELSE convert(varchar(40),UPID) END as parent, " +
            //    "SUBJECT  + ' ( ' + convert(varchar(40),b.DISBUTION) + ' 分 ) ' as text " +
            //    "from KPI_item a " +
            //    "inner join KPI_Distribution b on a.UUID = b.UUID " +
            //    "where GROUPS = @GROUPS and b.YEART = @Year and LEVELS >= 2 ");

            //item_sqlStr.Append("SELECT convert(varchar(40),a.UUID) as ID, " +
            //    "CASE LEVELS WHEN 1 THEN '#' ELSE convert(varchar(40),UPID) END as parent, " +
            //    "SUBJECT as text " +
            //    "from KPI_item a " +
            //    "inner join KPI_Distribution b on a.UUID = b.UUID " +
            //    "where GROUPS = @GROUPS and b.YEART = @Year ");

            StringBuilder _sqlStr = new StringBuilder();
            _sqlStr.Append(@"SELECT convert(varchar(40),a.UUID) as ID, 
                    CASE LEVELS WHEN 2 THEN '#' ELSE convert(varchar(40),UPID) END as parent, 
                    SUBJECT  + ' ( 0.00 分 ) ' as text 
                    from KPI_item a 
                    where GROUPS = @GROUPS and LEVELS = 2 
                    ORDER BY Sql ");

            //_sqlStr.Append("SELECT convert(varchar(40),a.UUID) as ID, " +
            //    "CASE LEVELS WHEN 2 THEN '#' ELSE convert(varchar(40),UPID) END as parent, " +
            //    "CASE LEVELS WHEN 2 THEN SUBJECT " +
            //    "ELSE ( SUBJECT  + ' ( ' + convert(varchar(40),b.DISBUTION) + ' 分 ) ' ) END as text " +
            //    "from KPI_item a " +
            //    "inner join KPI_Distribution b on a.UUID = b.UUID " +
            //    "where GROUPS = @GROUPS and LEVELS = 2 " +
            //    "ORDER BY Sql ");

            _sqlParams = new Dapper.DynamicParameters();
            _sqlParams.Add("Year", searchInfo.year);
            _sqlParams.Add("GROUPS", searchInfo.groups);

            result.tree = new List<ItemTreeViewMode>();
            using (var cn = new SqlConnection(_dbConnPPP)) //連接資料庫
            {
                cn.Open();
                tree = cn.Query<ItemTreeViewMode>(item_sqlStr.ToString(), _sqlParams).ToList();
                if (tree != null && tree.Count > 0)
                {
                    result.tree = cn.Query<ItemTreeViewMode>(item_sqlStr.ToString(), _sqlParams).ToList();
                }
                else
                {
                    result.tree = cn.Query<ItemTreeViewMode>(_sqlStr.ToString(), _sqlParams).ToList();
                }
            }
            return result;
            #endregion
        }
        #endregion

        #region 設定 項目 PostItemList()
        public void PostItemList(SearchItemViewMode searchInfo)
        {
            //組立SQL字串並連接資料庫
            #region 參數告宣
            SearchItemViewMode list = new SearchItemViewMode();
            List<SearchItemViewMode> result = new List<SearchItemViewMode>();
            List<SearchItemViewMode> itemresult = new List<SearchItemViewMode>();
            DateTime dt = DateTime.Now;
            #endregion

            #region 流程
            StringBuilder _sqlStr = new StringBuilder();
            _sqlStr.Append("SELECT i.UUID, i.SUBJECT " +
                "FROM KPI_item i " +
                "JOIN KPI_Distribution d on i.UUID = d.UUID " +
                "WHERE i.UUID = @UUID and i.UPID = @Upid and d.YEART = @YEART ");
            _sqlParams = new Dapper.DynamicParameters();
            _sqlParams.Add("UUID", searchInfo.UUID);
            _sqlParams.Add("Upid", searchInfo.UpID);
            _sqlParams.Add("YEART", (dt.Year - 1911));

            using (var cn = new SqlConnection(_dbConnPPP)) //連接資料庫
            {
                cn.Open();                
                list.Item = cn.Query<SearchItemViewMode>(_sqlStr.ToString(), _sqlParams).ToList();
                result = cn.Query<SearchItemViewMode>(_sqlStr.ToString(), _sqlParams).ToList();
                if (result != null && result.Count > 0)
                {
                    Update_KPU_item(searchInfo, cn);
                    Guid uuid = result[0].UUID;
                    float disbution = float.Parse(searchInfo.Distribution.ToString());
                    StringBuilder _sqlStrDistribution = new StringBuilder();

                    _sqlStrDistribution.Append("SELECT UUID ,YEART " +
                        "FROM KPI_Distribution " +
                        "WHERE UUID = @UUid and YEART = @YEART ");
                    _sqlParams.Add("UUid", uuid);
                    _sqlParams.Add("YEART", (dt.Year - 1911));
                    itemresult = cn.Query<SearchItemViewMode>(_sqlStrDistribution.ToString(), _sqlParams).ToList();

                    if (itemresult != null && itemresult.Count > 0)
                    {
                        #region 更新配分項目
                        StringBuilder _sqlStrUPDATE = new StringBuilder();
                        _sqlStrUPDATE.Append("UPDATE KPI_Distribution " +
                            "set DISBUTION = @DISBUTION, MODIFY_USER = @MODIFY_USER, MODIFY_DATE = @MODIFY_DATE " +
                            "where UUID = @UUid and YEART = @YEART ");
                        _sqlParams.Add("DISBUTION", searchInfo.Distribution);
                        _sqlParams.Add("MODIFY_USER", "admin");
                        _sqlParams.Add("MODIFY_DATE", dt);
                        _sqlParams.Add("UUid", uuid);
                        _sqlParams.Add("YEART", (dt.Year - 1911));

                        var ExecResult = cn.Execute(_sqlStrUPDATE.ToString(), _sqlParams);
                        #endregion
                    }
                    else
                    {
                        #region 新增配分項目
                        insert_KPI_Distribution(uuid, disbution, searchInfo.user, cn);
                        #endregion
                    }
                }
                else
                {
                    #region 查詢上層項目
                    StringBuilder _sqlStrItem = new StringBuilder();
                    _sqlStrItem.Append("SELECT UUID, GROUPS, SUBJECT, UPID, LEVELS " +
                        "FROM KPI_item " +
                        "WHERE UUID = @UpID ");
                    _sqlParams.Add("UpID", searchInfo.UpID);
                    list.Item = cn.Query<SearchItemViewMode>(_sqlStrItem.ToString(), _sqlParams).ToList();
                    #endregion

                    string GROUPS = list.Item[0].GROUPS;
                    int LEVELS = list.Item[0].LEVELS + 1;
                    Guid guid = Guid.NewGuid();

                    #region 新增主項目
                    StringBuilder _sqlStrEx = new StringBuilder();
                    _sqlStrEx.Append("INSERT INTO KPI_item ( UUID, GROUPS, SUBJECT, UPID, LEVELS) ");
                    _sqlStrEx.Append(" VALUES ");
                    _sqlStrEx.Append("( @UUID, @GROUPS ,@SUBJECT ,@UPID ,@LEVELS )");

                    _sqlParams.Add("UUID", guid);
                    _sqlParams.Add("GROUPS", GROUPS);
                    _sqlParams.Add("SUBJECT", searchInfo.Subject);
                    _sqlParams.Add("UPID", searchInfo.UpID);
                    _sqlParams.Add("LEVELS", LEVELS);

                    var ExecResult = cn.Execute(_sqlStrEx.ToString(), _sqlParams);
                    #endregion

                    #region 新增配分項目
                    float disbution = float.Parse(searchInfo.Distribution.ToString());//項目配分
                    insert_KPI_Distribution(guid, disbution, searchInfo.user, cn);
                    #endregion
                }

            }
            #endregion
        }
        #endregion


        #region 插入配分項目
        public void insert_KPI_Distribution(Guid uuid, float disbution, string user, SqlConnection cn)
        {
            StringBuilder _sqlStrEx2 = new StringBuilder();
            _sqlStrEx2.Append("INSERT INTO KPI_Distribution ( UUID, YEART, DISBUTION, MODIFY_USER, MODIFY_DATE) ");
            _sqlStrEx2.Append(" VALUES ");
            _sqlStrEx2.Append("(@UUid ,@YEART ,@DISBUTION ,@MODIFY_USER ,@MODIFY_DATE )");

            DateTime time = DateTime.Now;
            int year = time.Year - 1911;
            _sqlParams.Add("UUid", uuid);
            _sqlParams.Add("YEART", year);
            _sqlParams.Add("DISBUTION", disbution);
            _sqlParams.Add("MODIFY_USER", user);
            _sqlParams.Add("MODIFY_DATE", time);

            var ExecResult = cn.Execute(_sqlStrEx2.ToString(), _sqlParams);
        }
        #endregion

        #region 更新主項目
        public void Update_KPU_item(SearchItemViewMode searchInfo, SqlConnection cn)
        {
            StringBuilder _sqlStr = new StringBuilder();

            _sqlStr.Append("UPDATE KPI_item " +
                "set SUBJECT = @SUBJECT " +
                "where UUID = @UUid ");
            _sqlParams.Add("SUBJECT", searchInfo.Subject);
            _sqlParams.Add("UUid", searchInfo.UUID);

            var ExecResult = cn.Execute(_sqlStr.ToString(), _sqlParams);
        }
        #endregion

        #region 查詢 項目 QueryItemList()
        public SearchItemViewMode QueryItemList(SearchItemViewMode searchInfo)
        {
            //組立SQL字串並連接資料庫
            #region 參數告宣
            SearchItemViewMode result = new SearchItemViewMode();
            DateTime dt = DateTime.Now;
            #endregion

            #region 流程

            StringBuilder _sqlStr = new StringBuilder();
            _sqlStr.Append("SELECT UUID, YEART, DISBUTION as Distribution FROM KPI_Distribution " +
                "where UUID = @UUid and YEART = @Year");

            _sqlParams = new Dapper.DynamicParameters();
            _sqlParams.Add("UUid", searchInfo.UUID);
            _sqlParams.Add("Year", (dt.Year - 1911));

            using (var cn = new SqlConnection(_dbConnPPP)) //連接資料庫
            {
                cn.Open();
                result.Item = cn.Query<SearchItemViewMode>(_sqlStr.ToString(), _sqlParams).ToList();
            }
            return result;
            #endregion
        }
        #endregion

        #region 刪除 項目 DelItem()
        public SearchItemViewMode DelItem(SearchItemViewMode searchInfo)
        {
            //組立SQL字串並連接資料庫
            #region 參數告宣
            SearchItemViewMode result = new SearchItemViewMode();
            DateTime dt = DateTime.Now;
            #endregion

            #region 流程

            StringBuilder _sqlItemStr = new StringBuilder();            
            _sqlItemStr.Append("SELECT i.UUID as UUID, SUBJECT as　Subject, d.YEART as YEART, i.UPID as Upid " +
                "FROM KPI_item i " +
                "left join KPI_Distribution d on d.UUID = i.UUID " +
                "WHERE i.UUID = @UUid and d.YEART < @Year; ");

            StringBuilder _sqlDelkItemStrDistribution = new StringBuilder();
            _sqlDelkItemStrDistribution.Append("DELETE FROM KPI_item " +
            "WHERE UUID in ( " +
            "SELECT c.UUID " +
            "FROM uChilden(@UUid) c " +
            "left join KPI_Distribution d on d.UUID = c.UUID); ");


            StringBuilder _sqlDelkItemStr = new StringBuilder();
            _sqlDelkItemStr.Append("DELETE FROM KPI_Distribution " +
            "WHERE UUID in ( " +
            "SELECT c.UUID " +
            "FROM uChilden(@UUid) c " +
            "left join KPI_Distribution d on d.UUID = c.UUID " +
            "where d.YEART = @Year); ");

            _sqlParams = new Dapper.DynamicParameters();
            _sqlParams.Add("UUid", searchInfo.UUID);
            _sqlParams.Add("Subject", searchInfo.Subject);
            _sqlParams.Add("Upid", searchInfo.UpID);
            _sqlParams.Add("Year", (dt.Year - 1911));

            using (var cn = new SqlConnection(_dbConnPPP)) //連接資料庫
            {
                cn.Open();
                result.Item = cn.Query<SearchItemViewMode>(_sqlItemStr.ToString(), _sqlParams).ToList();
                if (result.Item != null && result.Item.Count > 0)
                {
                    var ExecResult2 = cn.Execute(_sqlDelkItemStrDistribution.ToString(), _sqlParams);
                }
                else
                {
                    var ExecResult = cn.Execute(_sqlDelkItemStr.ToString(), _sqlParams);
                    var ExecResult2 = cn.Execute(_sqlDelkItemStrDistribution.ToString(), _sqlParams);
                }
            }
            return result;
            #endregion
        }
        #endregion

    }
}