using WaterCaseTracking.Dao;
using WaterCaseTracking.Models;
using WaterCaseTracking.Models.ViewModels.ProductList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WaterCaseTracking.Service
{
    public class ProductListService
    {
        #region 查詢 QuerySearchList()
        public SearchListViewModel QuerySearchList(SearchInfoViewModel searchInfo)
        {
            #region 參數宣告				
            SearchListViewModel searchList = new SearchListViewModel();
            ProductDao productDao = new ProductDao();
            #endregion

            #region 流程																
            searchList = productDao.QuerySearchList(searchInfo); //將參數送入Dao層,組立SQL字串並連接資料庫

            return searchList;
            #endregion
        }
        #endregion

        #region 新增 AddProductList()
        internal void AddProductList(detailViewModel detail)
        {
            #region 參數宣告				
            ProductDao productDao = new ProductDao();
            ProductModel model = new ProductModel();
            #endregion

            #region 流程	
            model.Name = detail.Name;
            model.Price = detail.Price;
            model.Qty = detail.Qty;
            model.Description = detail.Description;
            model.CategoryId = detail.CategoryId;
            model.CreationDate = DateTime.Now;
            model.ModifiedDate = DateTime.Now;
            model.Status = true;
            productDao.AddProductList(model);//將參數送入Dao層,組立SQL字串並連接資料庫

            #endregion
        }
        #endregion

        #region 修改 UpdateProductList()
        internal void UpdateProductList(detailViewModel detail)
        {
            #region 參數宣告				
            ProductDao productDao = new ProductDao();
            ProductModel model = new ProductModel();
            #endregion

            #region 流程
            model.ID = detail.ID;
            model.Name = detail.Name;
            model.Price = detail.Price;
            model.Qty = detail.Qty;
            model.Description = detail.Description;
            model.CategoryId = detail.CategoryId;
            model.ModifiedDate = DateTime.Now;

            productDao.UpdateProductList(model);//將參數送入Dao層,組立SQL字串並連接資料庫



            #endregion
        }
        #endregion

        #region 刪除 DeleteProductList()
        internal void DeleteProductList(detailViewModel detail)
        {
            #region 參數宣告				
            ProductDao productDao = new ProductDao();
            ProductModel model = new ProductModel();
            #endregion

            #region 流程	
            model.ID = detail.ID;
            model.ModifiedDate = DateTime.Now;
            model.Status = false;
            productDao.DeleteProductList(model);//將參數送入Dao層,組立SQL字串並連接資料庫

            #endregion
        }
        #endregion
    }
}