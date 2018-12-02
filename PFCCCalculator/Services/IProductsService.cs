using Microsoft.AspNetCore.Mvc;
//using ProductsService.Models;
using SharedModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using ProductsService.Models;

namespace PFCCCalculatorService.Services
{
    /* public class IProductsService
     {
     }*/
    public interface IProductsService : IDisposable

    {
         Task<List<Product>> GetProducts();

         Task<List<ProductsCategory>> GetProductsCategories();

         Task<List<Product>> GetProductsByCategoryId(int productCategoryId);
    
         Task<Product> GetProductById(int productId);

          
           Task<bool> CreateProduct(int userId, Product product);
           Task<bool> DeleteProduct(int userId, int productId);
           Task<bool> UpdateProduct(int userId, Product productToUpdate);

        /*      [ProducesResponseType((int)HttpStatusCode.NotFound)]
           [ProducesResponseType((int)HttpStatusCode.Created)]
           Task<IActionResult> UpdateProduct([FromBody]Product productToUpdate);*/


        Task<PaginatedModel<Product>> Items(int pageSize = 10, int pageIndex = 0);

    }

}
