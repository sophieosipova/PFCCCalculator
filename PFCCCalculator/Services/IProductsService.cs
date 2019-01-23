

using SharedModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PFCCCalculatorService.Models;

namespace PFCCCalculatorService.Services
{
    /* public class IProductsService
     {
     }*/
    public interface IProductsService : IDisposable

    {
         Task<List<ProductModel>> GetProducts();

         Task<List<ProductsCategoryModel>> GetProductsCategories();

         Task<List<ProductModel>> GetProductsByCategoryId(int productCategoryId);
    
         Task<ProductModel> GetProductById(int productId);

          
        Task<ProductModel> CreateProduct(string userId, ProductModel product);
        Task<bool> DeleteProduct(string userId, int productId);
        Task<ProductModel> UpdateProduct(string userId, ProductModel productToUpdate);

        Task<AppsToken> Login();
 

        Task<PaginatedModel<ProductModel>> Items(int pageSize = 10, int pageIndex = 0);

    }

}
