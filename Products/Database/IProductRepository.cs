using ProductsService.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using SharedModels;

namespace ProductsService.Database
{
    public interface IProductRepository
    {
        Task<List<Product>> GetProducts();
        Task<List<Product>> GetUsersProducts(string userId);
        Task<List<ProductsCategory>> GetProductsCategories();
        Task<List<Product>> GetProductsByCategoryId(int productCategoryId);
        Task<Product> GetProductById(int productId);  
        Task<Product> CreateProduct(string userId, Product product);
        Task<bool> DeleteProduct(string userId, int productId);
        Task<Product> UpdateProduct(string userId, Product productToUpdate);
        Task <PaginatedModel<Product>> Items(int pageSize = 10, int pageIndex = 0);
    }

}

