using Dapper;
using WaterCaseTracking.Models;
using WaterCaseTracking.Models.ViewModels.Home;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;

namespace WaterCaseTracking.Dao
{
    public class SYS_announcementDao : _BaseDao
    {
        #region 取得公告資料列表 GetbulletinList()
        public SearchListViewModel GetbulletinList(SearchInfoViewModel searchInfo)
        {
            //組立SQL字串並連接資料庫
            #region 參數告宣
            SearchListViewModel result = new SearchListViewModel();
            DateTime Rstr = new DateTime();
            if (searchInfo.startDate != null && searchInfo.startDate != "") { Rstr = DateTime.Parse(searchInfo.startDate); }
            DateTime Sstr = new DateTime();
            if (searchInfo.endDate != null && searchInfo.endDate != "") { Sstr = DateTime.Parse(searchInfo.endDate); }
            #endregion


            #region 流程

            StringBuilder _sqlStr = new StringBuilder();
            StringBuilder _sqlCountStr = new StringBuilder();
            _sqlCountStr.Append("select count(*) from SYS_announcement WHERE 1=1 ");

            //_sqlStr.Append("SELECT row_number() OVER(PARTITION BY 1 ORDER BY ID DESC ) AS Seq, ID, Title, Contents, AttachedFile, " +
            //    "AttachedFile, FORMAT( ReleaseDate, 'yyyy-MM-dd') as ReleaseDateStr, " +
            //    "CASE WHEN StopRelease IS NULL THEN '無截止時間' ELSE FORMAT(StopRelease, 'yyyy-MM-dd') END as StopReleaseStr " +
            //    "FROM SYS_announcement ");

            _sqlStr.Append(@"SELECT row_number() OVER(PARTITION BY 1 ORDER BY ID DESC ) AS Seq, 
                ID, Title, Contents, AttachedFile, 
				CASE 
                    WHEN GETDATE() > [StopRelease] THEN '1'
				    ELSE '2'
				END as 'StautsCode', 
			    CASE 
                    WHEN GETDATE() > [StopRelease] THEN '下架'
				    ELSE '上架'
				END as 'Stauts',
                convert(varchar(10) , (YEAR (CreationDate) - 1911)) +  '/' + Format(CreationDate, 'MM/dd') AS CreationDateStr, 
                convert(varchar(10) , (YEAR (ReleaseDate) - 1911)) + '/' + Format(ReleaseDate, 'MM/dd') AS ReleaseDateStr, 
                CASE 
                    WHEN StopRelease IS NULL THEN '無截止時間' 
                    ELSE convert(varchar(10) , (YEAR (StopRelease) - 1911)) + '/' + Format(StopRelease, 'MM/dd') 
                END as StopReleaseStr 
                FROM SYS_announcement 
                WHERE 1 = 1");

            if (searchInfo.user != "admin")
            {
                _sqlCountStr.Append("AND GETDATE() < StopRelease and OnEnable = 1 ");          
                _sqlStr.Append("AND GETDATE() < StopRelease and OnEnable = 1 ");
            }
            else
            {
                _sqlCountStr.Append("AND OnEnable = 1 ");          
                _sqlStr.Append("AND OnEnable = 1 ");

                switch (searchInfo.SearchMode)
                {
                    case "start":
                        break;
                    case "SearchTime":
                        if (Rstr < Sstr)
                        {
                            _sqlCountStr.Append("AND CreationDate >= @Rstr AND CreationDate <= @Sstr ");
                            _sqlStr.Append("AND CreationDate >= @Rstr AND CreationDate <= @Sstr ");
                        }
                        break;
                    case "SearchStatus":
                        if (searchInfo.Cmbddlstatus == 1)
                        {
                            _sqlCountStr.Append("AND GETDATE() <= StopRelease");
                            _sqlStr.Append("AND GETDATE() <= StopRelease");
                        }
                        if (searchInfo.Cmbddlstatus == 2)
                        {
                            _sqlCountStr.Append("AND GETDATE() > StopRelease");
                            _sqlStr.Append("AND GETDATE() > StopRelease");
                        }
                        break;
                }
                //if (searchInfo.SearchMode = "")
            }

            _sqlParams = new Dapper.DynamicParameters();

            _sqlStr.Append(" ORDER BY CreationDate " + searchInfo.sSortDir_0 + " OFFSET @iDisplayStart ROWS  FETCH NEXT @iDisplayLength ROWS ONLY ");
            _sqlParams.Add("iDisplayStart", searchInfo.iDisplayStart);
            _sqlParams.Add("iDisplayLength", searchInfo.iDisplayLength);           
            _sqlParams.Add("Rstr", Rstr.AddYears(1911).ToString("yyyy-MM-dd HH:mm:ss"));
            _sqlParams.Add("Sstr", Sstr.AddYears(1911).ToString("yyyy-MM-dd HH:mm:ss"));

            result.data = new List<SearchItemViewModel>();
            result.draw = (searchInfo.sEcho + 1);

            using (var cn = new SqlConnection(_dbConnPPP)) //連接資料庫
            {
                cn.Open();
                result.data = cn.Query<SearchItemViewModel>(_sqlStr.ToString(), _sqlParams).ToList();
                //result.data = cn.Query<SearchItemViewModel>(_sqlStr.ToString()).ToList();
                result.recordsTotal = cn.Query<int>(_sqlCountStr.ToString(), _sqlParams).First();
                result.recordsFiltered = result.recordsTotal;
            }
            return result;
            #endregion
        }
        #endregion

        #region 更新公告資料 UpDateBulletin()
        public SearchListViewModel UpDateBulletin(SYS_announcementModel searchInfo)
        {
            //組立SQL字串並連接資料庫
            #region 參數告宣
            SearchListViewModel result = new SearchListViewModel();
            DateTime Time = DateTime.Now;
            string Rstr, Sstr;
            #endregion

            #region 流程

            StringBuilder _sqlStr = new StringBuilder();
            _sqlStr.Append("UPDATE SYS_announcement " +
                "SET Title = @Title, Contents = @Contents, " +
                "ReleaseDate = @ReleaseDate, StopRelease = @StopRelease, " +
                "MODIFY_DATE = @MODIFY_DATE, MODIFY_USER = @MODIFY_USER, " +
                "OnEnable = 1 " +
                "WHERE ID = @ID ;");

            _sqlParams = new Dapper.DynamicParameters();

            //if (testTry)
            //{
            //    searchInfo.MODIFY_USER = "TryUser";
            //}
            Rstr = searchInfo.ReleaseDate.AddYears(1911).ToString("yyyy-MM-dd HH:mm:ss");
            Sstr = searchInfo.StopRelease.AddYears(1911).ToString("yyyy-MM-dd HH:mm:ss");

            _sqlParams.Add("Title", searchInfo.Title);
            _sqlParams.Add("Contents", searchInfo.Contents);
            _sqlParams.Add("ReleaseDate", Rstr);
            _sqlParams.Add("StopRelease", Sstr);
            _sqlParams.Add("MODIFY_DATE", Time.ToString("yyyy-MM-dd HH:mm:ss"));
            _sqlParams.Add("MODIFY_USER", searchInfo.MODIFY_USER); //等待小鋒SSO完成
            _sqlParams.Add("ID", searchInfo.ID);

            result.data = new List<SearchItemViewModel>();           

            using (var cn = new SqlConnection(_dbConnPPP)) //連接資料庫
            {
                cn.Open();
                result.data = cn.Query<SearchItemViewModel>(_sqlStr.ToString(), _sqlParams).ToList();
            }
            return result;
            #endregion
        }
        #endregion

        #region 新增公告資料 AddBulletin()
        public SearchListViewModel AddBulletin(SYS_announcementModel searchInfo)
        {
            //組立SQL字串並連接資料庫
            #region 參數告宣
            SearchListViewModel result = new SearchListViewModel();
            DateTime Time = DateTime.Now;
            string Rstr, Sstr;
            #endregion

            #region 流程

            StringBuilder _sqlStr = new StringBuilder();
            _sqlStr.Append("INSERT INTO SYS_announcement " +
                "(Title, Contents, AttachedFile, ReleaseDate, StopRelease,  " +
                "CreationDate, CreationUser, MODIFY_DATE, MODIFY_USER, " +
                "OnEnable) " +
                "VALUES" +
                "(@Title, @Contents, @AttachedFile, @ReleaseDate, @StopRelease,  " +
                "@CreationDate, @CreationUser, @MODIFY_DATE, @MODIFY_USER, " +
                "@OnEnable); ");

            _sqlParams = new Dapper.DynamicParameters();

            Rstr = searchInfo.ReleaseDate.AddYears(1911).ToString("yyyy-MM-dd HH:mm:ss");
            Sstr = searchInfo.StopRelease.AddYears(1911).ToString("yyyy-MM-dd HH:mm:ss");
            searchInfo.CreationDate = Time;
            searchInfo.MODIFY_DATE = Time;

            //if (testTry)
            //{
            //    searchInfo.CreationUser = "TryUser";
            //    searchInfo.MODIFY_USER = "TryUser";
            //}

            _sqlParams.Add("Title", searchInfo.Title);
            _sqlParams.Add("Contents", searchInfo.Contents);
            _sqlParams.Add("AttachedFile", searchInfo.AttachedFile);
            _sqlParams.Add("ReleaseDate", Rstr);
            _sqlParams.Add("StopRelease", Sstr);
            _sqlParams.Add("CreationDate", searchInfo.CreationDate.ToString("yyyy-MM-dd HH:mm:ss"));
            _sqlParams.Add("CreationUser", searchInfo.CreationUser);
            _sqlParams.Add("MODIFY_DATE", searchInfo.MODIFY_DATE.ToString("yyyy-MM-dd HH:mm:ss"));
            _sqlParams.Add("MODIFY_USER", searchInfo.MODIFY_USER);
            _sqlParams.Add("OnEnable", 1);

            using (var cn = new SqlConnection(_dbConnPPP)) //連接資料庫
            {
                cn.Open();
                var ExecResult = cn.Execute(_sqlStr.ToString(), _sqlParams);
            }
            return result;
            #endregion
        }
        #endregion

        #region 刪除公告資料 DeleteBulletin()
        public SearchListViewModel DeleteBulletin(SYS_announcementModel searchInfo)
        {
            //組立SQL字串並連接資料庫
            #region 參數告宣
            SearchListViewModel result = new SearchListViewModel();
            DateTime Time = DateTime.Now;
            #endregion

            #region 流程

            //if (testTry)
            //{
            //    searchInfo.MODIFY_USER = "TryUser";
            //}

            StringBuilder _sqlStr = new StringBuilder();
            _sqlStr.Append("UPDATE SYS_announcement " +
                "SET OnEnable = 0 " +
                "WHERE ID = @ID ;");

            _sqlParams = new Dapper.DynamicParameters();

            _sqlParams.Add("MODIFY_DATE", Time.ToString("yyyy-MM-dd HH:mm:ss"));
            _sqlParams.Add("MODIFY_USER", searchInfo.MODIFY_USER); //等待小鋒SSO完成
            _sqlParams.Add("ID", searchInfo.ID);

            result.data = new List<SearchItemViewModel>();

            using (var cn = new SqlConnection(_dbConnPPP)) //連接資料庫
            {
                cn.Open();
                result.data = cn.Query<SearchItemViewModel>(_sqlStr.ToString(), _sqlParams).ToList();
            }
            return result;
            #endregion
        }
        #endregion

        #region 取得公告資料 Getbulletin()
        public SearchListViewModel Getbulletin(SearchInfoViewModel searchInfo)
        {
            //組立SQL字串並連接資料庫
            #region 參數告宣
            SearchListViewModel result = new SearchListViewModel();
            #endregion

            #region 流程

            StringBuilder _sqlStr = new StringBuilder();
            //_sqlStr.Append("SELECT ID, Title, Contents, AttachedFile, " +
            //    "FORMAT(ReleaseDate, 'yyyy/MM/dd') AS ReleaseDateStr, " +
            //    "FORMAT(StopRelease, 'yyyy/MM/dd') AS StopReleaseStr " +
            //    "FROM SYS_announcement " +
            //    "WHERE ID = @ID ");
            _sqlStr.Append("SELECT ID, Title, Contents, AttachedFile, " +
                "convert(varchar(10) , (YEAR (ReleaseDate) - 1911)) + '/' + Format(ReleaseDate, 'MM/dd') AS ReleaseDateStr, " +
                "convert(varchar(10) , (YEAR (StopRelease) - 1911)) + '/' + Format(StopRelease, 'MM/dd') AS StopReleaseStr " +
                "FROM SYS_announcement " +
                "WHERE ID = @ID ");

            _sqlParams = new Dapper.DynamicParameters();

            _sqlParams.Add("ID", searchInfo.ID);

            result.data = new List<SearchItemViewModel>();

            using (var cn = new SqlConnection(_dbConnPPP)) //連接資料庫
            {
                cn.Open();
                result.data = cn.Query<SearchItemViewModel>(_sqlStr.ToString(), _sqlParams).ToList();
            }
            return result;
            #endregion
        }
        #endregion

        #region 取得功能列表 GetOperating()
        public SearchListViewModel GetOperating(string searchInfo)
        {
            //組立SQL字串並連接資料庫
            #region 參數告宣
            SearchListViewModel result = new SearchListViewModel();
            #endregion

            #region 流程

            StringBuilder _sqlStr = new StringBuilder();
            _sqlStr.Append(" SELECT p.PID, PP.PROID " +
                "FROM Permission p " +
                "LEFT JOIN PermissionProg PP ON p.PID = PP.Programs " +
                "WHERE p.PID = @PID");

            _sqlParams = new Dapper.DynamicParameters();

            _sqlParams.Add("PID", searchInfo);

            result.UserModel = new List<SearchUserViewModel>();

            using (var cn = new SqlConnection(_dbConnPPP)) //連接資料庫
            {
                cn.Open();
                result.UserModel = cn.Query<SearchUserViewModel>(_sqlStr.ToString(), _sqlParams).ToList();
            }
            return result;
            #endregion
        }
        #endregion
    }
}