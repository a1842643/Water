using WaterCaseTracking.Models.ViewModels.ProductList;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using WaterCaseTracking.Models;

namespace WaterCaseTracking.Dao
{
    public class ProductDao: _BaseDao
    {
        #region 查詢 QuerySearchList()
        public SearchListViewModel QuerySearchList(SearchInfoViewModel searchInfo)
        {
            //組立SQL字串並連接資料庫
            #region 參數告宣
            SearchListViewModel result = new SearchListViewModel();
            #endregion

            #region 流程

            StringBuilder _sqlStr = new StringBuilder();
            _sqlStr.Append("SELECT * FROM Product p Where p.Status = 1");
            _sqlParams = new Dapper.DynamicParameters();
            if (!string.IsNullOrEmpty(searchInfo.Name))
            {
                _sqlStr.Append(" AND p.Name LIKE @Name ");
                _sqlParams.Add("Name", "%" + searchInfo.Name + "%");
            }

            result.SearchItemList = new List<SearchItemViewModel>();
            using (var cn = new SqlConnection(_dbConnPPP)) //連接資料庫
            {
                cn.Open();
                result.SearchItemList = cn.Query<SearchItemViewModel>(_sqlStr.ToString(), _sqlParams).ToList();
            }
            return result;
            #endregion
        }
        #endregion

        #region 新增 AddProductList()
        internal void AddProductList(ProductModel model)
        {
            //組立SQL字串並連接資料庫
            StringBuilder _sqlStr = new StringBuilder();
            _sqlStr.Append("INSERT INTO Product (Name ,Qty ,Description ,CategoryId ,Price ,CreationDate ,ModifiedDate ,Status ) ");
            _sqlStr.Append(" VALUES ");
            _sqlStr.Append("(@Name ,@Qty ,@Description ,@CategoryId ,@Price ,@CreationDate ,@ModifiedDate ,@Status )");

            _sqlParams = new Dapper.DynamicParameters();
            _sqlParams.Add("Name", model.Name);
            _sqlParams.Add("Qty", model.Qty);
            _sqlParams.Add("Description", model.Description);
            _sqlParams.Add("CategoryId", model.CategoryId);
            _sqlParams.Add("Price", model.Price);
            _sqlParams.Add("CreationDate", model.CreationDate);
            _sqlParams.Add("ModifiedDate", model.ModifiedDate);
            _sqlParams.Add("Status", model.Status);

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
        #endregion

        #region 修改 UpdateProductList()
        internal void UpdateProductList(ProductModel model)
        {
            //組立SQL字串並連接資料庫
            StringBuilder _sqlStr = new StringBuilder();
            _sqlStr.Append("UPDATE Product SET Name = @Name ,Price = @Price, Qty = @Qty , Description = @Description , CategoryId = @CategoryId , ModifiedDate = @ModifiedDate ");
            _sqlStr.Append("WHERE ID = @ID");

            _sqlParams = new Dapper.DynamicParameters();
            _sqlParams.Add("ID", model.ID);
            _sqlParams.Add("Name", model.Name);
            _sqlParams.Add("Price", model.Price);
            _sqlParams.Add("Qty", model.Qty);
            _sqlParams.Add("Description", model.Description);
            _sqlParams.Add("CategoryId", model.CategoryId);
            _sqlParams.Add("ModifiedDate", model.ModifiedDate);

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
        #endregion

        #region 刪除 DeleteProductList()
        internal void DeleteProductList(ProductModel model)
        {
            //組立SQL字串並連接資料庫
            StringBuilder _sqlStr = new StringBuilder();
            _sqlStr.Append("UPDATE Product SET ModifiedDate = @ModifiedDate ,Status = @Status WHERE ID = @ID ");

            _sqlParams = new Dapper.DynamicParameters();
            _sqlParams.Add("ID", model.ID);
            _sqlParams.Add("ModifiedDate", model.ModifiedDate);
            _sqlParams.Add("Status", model.Status);

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
        #endregion
    }
}