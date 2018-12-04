using ProductsService.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SharedModels;

namespace ProductsService.Database
{
    public interface IProductRepository
    {
        Task<List<Product>> GetProducts();
        Task<List<Product>> GetUsersProducts(int userId);
        Task<List<ProductsCategory>> GetProductsCategories();
        Task<List<Product>> GetProductsByCategoryId(int productCategoryId);
        Task<Product> GetProductById(int productId);  
        Task<Product> CreateProduct(int userId, Product product);
        Task<bool> DeleteProduct(int userId, int productId);
        Task<Product> UpdateProduct(int userId, Product productToUpdate);
        Task <PaginatedModel<Product>> Items(int pageSize = 10, int pageIndex = 0);
    }

}

