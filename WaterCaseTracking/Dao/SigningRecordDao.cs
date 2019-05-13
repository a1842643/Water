using Dapper;
using WaterCaseTracking.Models.ViewModels.SigningProcess;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;

namespace WaterCaseTracking.Dao {
    public class SigningRecordDao : _BaseDao {
        #region 申請單-申請 ApplicationInsert()
        internal int ApplicationInsert(SigningInsertItemsViewModel model) {
            #region 初始參考
            StringBuilder _sqlStr = new StringBuilder();
            _sqlParamsList = new List<DynamicParameters>();
            _sqlParams = new Dapper.DynamicParameters();
            #endregion

            #region 組立SQL字串並連接資料庫
            _sqlStr.Append(@"Insert Into SigningRecord ( 
	                                            ProID						--申請單編號(yyyyMMddHHmmss + 員編id)
	                                            ,PID						--申請項目ID
	                                            ,PName						--申請項目Name
	                                            ,PrincipalID				--申請單目前負責人
	                                            ,Levels						--此次紀錄的階層
	                                            ,UserID						--申請者ID
	                                            ,UserName					--申請者名稱
	                                            ,SupervisorID				--簽核主管編號
	                                            ,SupervisorName				--簽核主管姓名
	                                            --,MsgUser					--申請者註記
	                                            ,Status						--簽核狀態
	                                            ,ModifyDate					--最後修改日
	                                            ,ModifyUser					--最後修改者
	                                            ,CreateDate					--建立日期
	                                            ,CreateUser					--建立者
	                                            ,NoDelete					--有沒刪除申請單 1:開單,0:關單
                                            )
                                            Values(
	                                            @ProID
	                                            ,@PID
	                                            ,@PName
	                                            ,@PrincipalID
	                                            ,2
	                                            ,@UserID
	                                            ,@UserName
	                                            ,@SupervisorID
	                                            ,@SupervisorName
	                                            --,@MsgUser
	                                            ,2
	                                            ,GETDATE()
	                                            ,@ModifyUser
	                                            ,GETDATE()
	                                            ,@CreateUser
	                                            ,@NoDelete
                                            )"
            );
            string _ProID = model.UserID.Trim() + DateTime.Now.ToString("yyyyMMddHHmmssfff");
            _sqlParams.Add("ProID", _ProID);
            _sqlParams.Add("PID", model.PID);
            _sqlParams.Add("PName", model.PName);
            _sqlParams.Add("PrincipalID", model.SupervisorID);
            //_sqlParams.Add("Levels", model.Levels);
            _sqlParams.Add("UserID", model.UserID);
            _sqlParams.Add("UserName", model.UserName);
            _sqlParams.Add("SupervisorID", model.SupervisorID);
            _sqlParams.Add("SupervisorName", model.SupervisorName);
            //_sqlParams.Add("ReviewSupervisorID", model.ReviewSupervisorID);
            //_sqlParams.Add("ReviewSupervisorName", model.ReviewSupervisorName);
            //_sqlParams.Add("MsgUser", model.MsgUser);
            //_sqlParams.Add("MsgSupervisor", model.MsgSupervisor);
            //_sqlParams.Add("MsgReviewSupervisor", model.MsgReviewSupervisor);
            //_sqlParams.Add("Status", model.Status);
            //_sqlParams.Add("WorkTimeStart", model.WorkTimeStart);
            //_sqlParams.Add("WorkTimeEnd", model.WorkTimeEnd);
            _sqlParams.Add("ModifyUser", model.UserID);
            _sqlParams.Add("CreateUser", model.UserID);
            _sqlParams.Add("NoDelete",true);
            try {
                using (var cn = new SqlConnection(_dbConnPPP))//連接資料庫
                {
                    cn.Open();
                    var ExecResult = cn.Execute(_sqlStr.ToString(), _sqlParams);
                    //...這裡要插操作紀錄,某User新增某xxxx申請單
                    return ExecResult; //回報新增筆數
                }
            }
            catch (Exception ex) {
                throw ex;
            }
            #endregion
        }
        #endregion

        #region 由各[申請上傳]按鈕建立申請單 InsertSigningRecord()
        internal int InsertSigningRecord(SigningInsertItemsViewModel model) {
            #region 初始參考
            StringBuilder _sqlStr = new StringBuilder();
            _sqlParamsList = new List<DynamicParameters>();
            _sqlParams = new Dapper.DynamicParameters();
            #endregion

            #region 組立SQL字串並連接資料庫
            _sqlStr.Append(@"Insert Into SigningRecord ( 
                                                ProID                                 --申請單編號(yyyyMMddHHmmss + 員編id)            
                                                ,PID                                   --申請項目ID            
                                                ,PName                                 --申請項目Name                         
                                                ,YM                                    --申請匯入年月YYYMM
                                                ,UploadName                            --申請上傳檔案名稱
                                                ,UploadPath                            --申請上傳檔案路徑
                                                ,Status                                --簽核狀態;申請=1;簽核=2;覆核=3;通過=4;駁回=5;                                            
                                                ,UserID                                --申請者ID
                                                ,UserName                              --申請者名稱
                                                ,ModifyDate                            --最後修改日
                                                ,ModifyUser                            --最後修改者
                                                ,CreateDate                            --建立日期
                                                ,CreateUser                            --建立者
                                                ,NoDelete                              --有沒刪除申請單 1:開單,0:關單
                                            )
                                            Values(
	                                            @ProID               
	                                            ,@PID                 
	                                            ,@PName                    
	                                            ,@YM                  
	                                            ,@UploadName          
	                                            ,@UploadPath          
	                                            ,1                        
	                                            ,@UserID              
	                                            ,@UserName         
                                                ,GETDATE()           
                                                ,@ModifyUser          
                                                ,GETDATE()          
                                                ,@CreateUser          
                                                ,@NoDelete            
                                            )"
            );
            _sqlParams.Add("ProID", model.ProID);
            _sqlParams.Add("UserID", model.UserID);
            _sqlParams.Add("UserName", model.UserName);
            _sqlParams.Add("PID", model.PID);
            _sqlParams.Add("PName", model.PName);
            _sqlParams.Add("YM", model.YM);                   //--申請匯入年月YYYMM
            _sqlParams.Add("UploadName", model.UploadName);   //--申請上傳檔案名稱
            _sqlParams.Add("UploadPath", model.UploadPath);   //--申請上傳檔案路徑
            _sqlParams.Add("ModifyUser", model.UserID);
            _sqlParams.Add("CreateUser", model.UserID);
            _sqlParams.Add("NoDelete", true);

            try {
                using (var cn = new SqlConnection(_dbConnPPP))//連接資料庫
                {
                    cn.Open();
                    var ExecResult = cn.Execute(_sqlStr.ToString(), _sqlParams);
                    //...這裡要插操作紀錄,某User新增某xxxx申請單
                    return ExecResult; //回報新增筆數
                }
            }
            catch (Exception ex) {
                throw ex;
            }
            #endregion
        }
        #endregion

        #region 申請單-覆核通過 & 簽核通過 ApplicationUpdate()
        internal int ApplicationUpdate(SigningUpdateItemsViewModel model, out SqlConnection conn, out SqlTransaction trans) {
            #region 初始參考
            StringBuilder _sqlStr = new StringBuilder();
            _sqlParamsList = new List<DynamicParameters>();
            _sqlParams = new Dapper.DynamicParameters();
            #endregion

            #region  組立SQL字串並連接資料庫
            _sqlStr.Append(@"	UPDATE  SigningRecord 
	                            SET  SupervisorID = @SupervisorID                   --簽核主管ID
                                    ,SupervisorName = @SupervisorName               --簽核主管Name
                                    ,ReviewSupervisorID = @ReviewSupervisorID       --覆核主管ID
                                    ,ReviewSupervisorName = @ReviewSupervisorName   --覆核主管Name
                                    ,MsgSupervisor = @MsgSupervisor 	        	--簽核主管註記
	                                ,MsgReviewSupervisor = @MsgReviewSupervisor		--覆核主管註記
                                    ,ModifyDate	= GETDATE() 		                --最後修改日
	                                ,ModifyUser = @ModifyUser		                --最後修改者
                                    ,Status = 3                                     --狀態:通過
	                            WHERE	ProID = @ProID"
            );

            _sqlParams.Add("ProID", model.ProID);
            _sqlParams.Add("MsgSupervisor", model.MsgSupervisor);
            _sqlParams.Add("MsgReviewSupervisor", model.MsgReviewSupervisor);
            _sqlParams.Add("ModifyUser", model.ID);
            if (model.Status == 1) { //簽核主管流程
                _sqlParams.Add("SupervisorID", model.ID);
                _sqlParams.Add("SupervisorName", model.Name);
                _sqlParams.Add("ReviewSupervisorID", "");
                _sqlParams.Add("ReviewSupervisorName", "");
            }
            else if (model.Status == 2) { //覆核主管流程
                _sqlParams.Add("SupervisorID", model.SupervisorID);    //寫入畫面原本資料
                _sqlParams.Add("SupervisorName", model.SupervisorName);//寫入畫面原本資料
                _sqlParams.Add("ReviewSupervisorID", model.ID);
                _sqlParams.Add("ReviewSupervisorName", model.Name);
            }
            conn = GetOpenConnection();
            trans = GetTransaction(conn);

            try {
                var ExecResult = conn.Execute(_sqlStr.ToString(), _sqlParams, trans);
                //...這裡要插操作紀錄,某User新增某xxxx申請單
                return ExecResult; //回報新增筆數
            }
            catch (Exception ex) {
                TransactionRollback(trans);
                GetCloseConnection(conn);
                throw ex;
            }

            #endregion
        }
        #endregion

        #region 申請單-駁回 ApplicationDelete()
        internal void ApplicationDelete(SigningDeleteItemsViewModel model, out SqlConnection conn, out SqlTransaction trans) {
            #region 初始參考
            StringBuilder _sqlStr = new StringBuilder();
            _sqlParamsList = new List<DynamicParameters>();
            _sqlParams = new Dapper.DynamicParameters();
            #endregion

            #region 組立SQL字串並連接資料庫
            _sqlStr.Append(@"	UPDATE  SigningRecord 
	                            SET  SupervisorID = @SupervisorID                   --簽核主管ID
                                    ,SupervisorName = @SupervisorName               --簽核主管Name
                                    ,ReviewSupervisorID = @ReviewSupervisorID       --覆核主管ID
                                    ,ReviewSupervisorName = @ReviewSupervisorName   --覆核主管Name
                                    ,MsgSupervisor = @MsgSupervisor 	        	--簽核主管註記
	                                ,MsgReviewSupervisor = @MsgReviewSupervisor		--覆核主管註記
                                    ,ModifyDate	= GETDATE() 		                --最後修改日
	                                ,ModifyUser = @ModifyUser		                --最後修改者
                                    ,Status = 4                                     --狀態:駁回
	                            WHERE	ProID = @ProID"
            );

            _sqlParams.Add("ProID", model.ProID);
            _sqlParams.Add("MsgSupervisor", model.MsgSupervisor);
            _sqlParams.Add("MsgReviewSupervisor", model.MsgReviewSupervisor);
            _sqlParams.Add("ModifyUser", model.ID);
            if (model.Status == 1) { //簽核主管流程
                _sqlParams.Add("SupervisorID", model.ID);
                _sqlParams.Add("SupervisorName", model.Name);
                _sqlParams.Add("ReviewSupervisorID", "");
                _sqlParams.Add("ReviewSupervisorName", "");
            }
            else if(model.Status == 2) { //覆核主管流程
                _sqlParams.Add("SupervisorID", model.SupervisorID);    //寫入畫面原本資料
                _sqlParams.Add("SupervisorName", model.SupervisorName);//寫入畫面原本資料
                _sqlParams.Add("ReviewSupervisorID", model.ID);
                _sqlParams.Add("ReviewSupervisorName", model.Name);
            }
            conn = GetOpenConnection();
            trans = GetTransaction(conn);

            try {
                var ExecResult = conn.Execute(_sqlStr.ToString(), _sqlParams, trans);
            }
            catch (Exception ex) {
                TransactionRollback(trans);
                GetCloseConnection(conn);
                throw ex;
            }
            #endregion
        }
        #endregion

        #region 申請單-查詢 QuerySearchList()
        public SigningQueryItemViewModel QuerySearchList(SigningInfoItemsViewModel searchInfo) {
            //組立SQL字串並連接資料庫
            #region 參數告宣
            SigningQueryItemViewModel result = new SigningQueryItemViewModel();
            #endregion

            #region 流程
            //查詢SQL
            StringBuilder _sqlStr = new StringBuilder();
            //查詢條件SQL
            StringBuilder _sqlParamStr = new StringBuilder();
            //總筆數
            StringBuilder _sqlCountStr = new StringBuilder();
            _sqlCountStr.Append("select count(1) from SigningRecord WHERE 1 = 1 AND UserID = @ID or SupervisorID = @ID or ReviewSupervisorID = @ID");
            _sqlStr.Append(@"  SELECT 
	                                  ProID					    	--申請單編號(yyyyMMddHHmmss + 員編id)
	                                  ,PName						--申請項目Name
	                                  ,UserName				    	--申請者名稱
	                                  ,SupervisorName				--簽核主管姓名
                                      ,ReviewSupervisorName			--覆核主管姓名
	                                  ,CASE Status		            
                                       WHEN 1 THEN '待簽核'
                                       WHEN 2 THEN '已簽核，待覆核'
                                       WHEN 3 THEN '通過' 
                                       WHEN 4 THEN '駁回' 
									   END as StatusText            --簽核狀態
                                      ,Status 
                                  FROM SigningRecord WHERE 1 = 1 AND UserID = @ID or SupervisorID = @ID or ReviewSupervisorID = @ID");
            //,CONVERT(varchar(100), logged, 121)--紀錄時間
            _sqlParams = new Dapper.DynamicParameters();
            //簽核狀態
            //WHEN 1 THEN '待簽核'
            //WHEN 2 THEN '已簽核，待覆核'
            //WHEN 3 THEN '通過'
            //WHEN 4 THEN '駁回'
            if (!string.IsNullOrEmpty(searchInfo._ID)) {
                _sqlParams.Add("ID", searchInfo._ID);
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
        #endregion

        #region 簽核流程處理單-查詢 getQueryItem()
        public SigningQueryItemViewModel getQueryItem(SigningInfoItemsViewModel model) {
            //組立SQL字串並連接資料庫
            #region 參數告宣
            SigningQueryItemViewModel result = new SigningQueryItemViewModel();
            #endregion

            #region 流程
            //查詢SQL
            StringBuilder _sqlStr = new StringBuilder();
            //查詢條件SQL
            StringBuilder _sqlParamStr = new StringBuilder();
            //總筆數
            StringBuilder _sqlCountStr = new StringBuilder();
            _sqlCountStr.Append("select count(1) from SigningRecord WHERE 1 = 1 and NoDelete = 1");
            _sqlStr.Append(@"  SELECT 
	                                  ProID					    	--申請單編號(yyyyMMddHHmmss + 員編id)
	                                  ,PName						--申請項目Name
	                                  ,UserID				    	--申請者ID
	                                  ,UserName				    	--申請者名稱
	                                  ,SupervisorName				--簽核主管姓名
                                      ,ReviewSupervisorName			--覆核主管姓名
	                                  ,CASE Status		            
                                       WHEN 1 THEN '待簽核'
                                       WHEN 2 THEN '已簽核，待覆核'
                                       WHEN 3 THEN '通過' 
                                       WHEN 4 THEN '駁回' 
									   END as StatusText            --簽核狀態
                                      ,Status 
                                  FROM SigningRecord WHERE 1 = 1 and NoDelete = 1 ");
            _sqlParams = new Dapper.DynamicParameters();
            _sqlParamStr.Append(" and Status = @Status ");
            _sqlParams.Add("Status", model.ddlStatus);

            #region 條件、排序(起)
            _sqlStr.Append(_sqlParamStr);
            _sqlCountStr.Append(_sqlParamStr);
            //_sqlStr.Append(" ORDER BY " + (searchInfo.iSortCol_0 + 1).ToString() + " " + searchInfo.sSortDir_0 + " OFFSET @iDisplayStart ROWS  FETCH NEXT @iDisplayLength ROWS ONLY ");
            //_sqlParams.Add("iDisplayStart", searchInfo.iDisplayStart);
            //_sqlParams.Add("iDisplayLength", searchInfo.iDisplayLength);
            #endregion 條件、排序(迄)

            result.data = new List<SearchItemViewModel>();
            //result.draw = (searchInfo.sEcho + 1);
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
        #endregion

        #region 簽核流程處理單-查詢 QueryItemsbyProID()
        public SigningQueryItemViewModel QueryItemsbyProID(string ProID) {
            //組立SQL字串並連接資料庫
            #region 參數告宣
            SigningQueryItemViewModel result = new SigningQueryItemViewModel();
            #endregion

            #region 流程
            //查詢SQL
            StringBuilder _sqlStr = new StringBuilder();
            //查詢條件SQL
            StringBuilder _sqlParamStr = new StringBuilder();
            //總筆數
            StringBuilder _sqlCountStr = new StringBuilder();
            _sqlCountStr.Append("select count(1) from SigningRecord WHERE 1 = 1 and ProID = @ProID ");
            _sqlStr.Append(@"  SELECT 
	                                  ProID					    	--申請單編號(yyyyMMddHHmmss + 員編id)
                                      ,PID                          --申請項目ID
	                                  ,PName						--申請項目Name
	                                  ,UserID						--申請者ID
	                                  ,UserName					    --申請者名稱
	                                  ,SupervisorID			    	--簽核主管編號
	                                  ,SupervisorName				--簽核主管姓名
	                                  ,ReviewSupervisorID           --覆核主管編號
                                      ,ReviewSupervisorName			--覆核主管姓名
                                      ,YM                           --申請匯入年月YYYMM
                                      ,UploadName                   --申請上傳檔案名稱
                                      ,UploadPath                   --申請上傳檔案路徑
	                                  ,WorkTimeStart                --作業區間(起)
	                                  ,WorkTimeEnd                  --作業區間(尾)
                                      --,MsgUser                      --申請者註記
                                      ,MsgSupervisor                --簽核主管註記
                                      ,MsgReviewSupervisor          --覆核主管註記
	                                  ,CASE Status		            
                                       WHEN 1 THEN '待簽核'
                                       WHEN 2 THEN '已簽核，待覆核'
                                       WHEN 3 THEN '通過' 
                                       WHEN 4 THEN '駁回' 
									   END as StatusText            --簽核狀態
                                      ,Status 
                                  FROM SigningRecord WHERE 1 = 1  and ProID = @ProID");
            _sqlParams = new Dapper.DynamicParameters();
            _sqlParams.Add("ProID", ProID); //待SSO待入ID
            #region 條件、排序(起)
            _sqlStr.Append(_sqlParamStr);
            _sqlCountStr.Append(_sqlParamStr);
            //_sqlStr.Append(" ORDER BY " + (searchInfo.iSortCol_0 + 1).ToString() + " " + searchInfo.sSortDir_0 + " OFFSET @iDisplayStart ROWS  FETCH NEXT @iDisplayLength ROWS ONLY ");
            //_sqlParams.Add("iDisplayStart", searchInfo.iDisplayStart);
            //_sqlParams.Add("iDisplayLength", searchInfo.iDisplayLength);
            #endregion 條件、排序(迄)

            result.data = new List<SearchItemViewModel>();
            //result.draw = (searchInfo.sEcho + 1);
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
        #endregion

        #region 上呈 GiveIn()
        internal int GiveIn(SigningUpdateItemsViewModel model) {
            #region 初始參考
            StringBuilder _sqlStr = new StringBuilder();
            _sqlParamsList = new List<DynamicParameters>();
            _sqlParams = new Dapper.DynamicParameters();
            #endregion

            #region  組立SQL字串並連接資料庫
            _sqlStr.Append(@"	UPDATE  SigningRecord 
	                            SET  
	                                Status	= 2     					            --簽核狀態
                                    ,SupervisorID = @SupervisorID                   --簽核主管編號
                                    ,SupervisorName = @SupervisorName               --簽核主管姓名
                                    ,MsgSupervisor = @MsgSupervisor
	                                ,ModifyDate	= GETDATE() 			            --最後修改日
	                                ,ModifyUser = @ModifyUser			            --最後修改者
	                            WHERE	ProID = @ProID"
            );
            _sqlParams.Add("ProID", model.ProID);
            _sqlParams.Add("SupervisorID", model.UserID);
            _sqlParams.Add("SupervisorName", model.UserName); 
            _sqlParams.Add("MsgSupervisor", model.MsgSupervisor);
            _sqlParams.Add("ModifyUser", model.UserID);

            try {
                using (var cn = new SqlConnection(_dbConnPPP))//連接資料庫
                {
                    cn.Open();
                    var ExecResult = cn.Execute(_sqlStr.ToString(), _sqlParams);
                    //...這裡要插操作紀錄,某User新增某xxxx申請單
                    return ExecResult; //回報新增筆數
                }
            }
            catch (Exception ex) {
                throw ex;
            }
            #endregion
        }
        #endregion

        #region 匯入失敗刪除申請單號
        internal int DeleteSigningRecord(SigningInsertItemsViewModel model)
        {
            #region 初始參考
            StringBuilder _sqlStr = new StringBuilder();
            _sqlParamsList = new List<DynamicParameters>();
            _sqlParams = new Dapper.DynamicParameters();
            #endregion

            #region 組立SQL字串並連接資料庫
            _sqlStr.Append(@"Delete From SigningRecord Where ProID = @ProID ");
            _sqlParams.Add("ProID", model.ProID);

            try
            {
                using (var cn = new SqlConnection(_dbConnPPP))//連接資料庫
                {
                    cn.Open();
                    var ExecResult = cn.Execute(_sqlStr.ToString(), _sqlParams);
                    //...這裡要插操作紀錄,某User新增某xxxx申請單
                    return ExecResult; //回報新增筆數
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            #endregion
        }
        #endregion


    }
}