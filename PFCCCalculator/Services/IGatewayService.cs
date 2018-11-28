using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProductsService.Models;
using SharedModels;


namespace PFCCCalculatorService.Services
{
    public interface IGatewayService
    {
        Task<PFCCRecipe> GetRecipeWithPFCC(int RecipeId);
        Task DeleteDish(int userId,int dishId);
        Task CreateProduct(int userId, Product product);
        Task DeleteProduct(int userId, int productId);
        Task UpdateProduct(int userId,Product productToUpdate);

    }
}
