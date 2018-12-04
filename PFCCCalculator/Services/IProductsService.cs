using Microsoft.AspNetCore.Mvc;
//using ProductsService.Models;
using SharedModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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

          
           Task<ProductModel> CreateProduct(int userId, ProductModel product);
           Task<bool> DeleteProduct(int userId, int productId);
           Task<bool> UpdateProduct(int userId, ProductModel productToUpdate);

        /*      [ProducesResponseType((int)HttpStatusCode.NotFound)]
           [ProducesResponseType((int)HttpStatusCode.Created)]
           Task<IActionResult> UpdateProduct([FromBody]Product productToUpdate);*/


        Task<PaginatedModel<ProductModel>> Items(int pageSize = 10, int pageIndex = 0);

    }

}
