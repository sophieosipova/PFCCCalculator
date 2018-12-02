using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PFCCCalculatorService.Models;
using SharedModels;


namespace PFCCCalculatorService.Services
{
    public interface IGatewayService
    {
        Task<PFCCRecipe> GetRecipeWithPFCC(int RecipeId);
        Task<bool> DeleteDish(int userId,int dishId);
        Task<bool> CreateProduct(int userId, Product product);
        Task<bool> DeleteProduct(int userId, int productId);
        Task<bool> UpdateProduct(int userId,Product productToUpdate);

    }
}
