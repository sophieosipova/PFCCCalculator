
using System.Net;
using System.Threading.Tasks;
using PFCCCalculatorService.Models;
using SharedModels;


namespace PFCCCalculatorService.Services
{
    public interface IGatewayService
    {
       Task<PaginatedModel<PFCCRecipe>> GetRecipesWithPFCC(int pageSize = 10, int pageIndex = 0);

        Task<PFCCRecipe> GetRecipeWithPFCC(int RecipeId);
        Task<bool> DeleteDish(string userId,int dishId);
      //  Task<Product> CreateProduct(int userId, Product product);
        Task<HttpStatusCode> DeleteProduct(string userId, int productId);

        Task<HttpStatusCode> UpdateProduct(string userId, ProductModel product);
       // Task<bool> UpdateProduct(int userId,ProductModel productToUpdate);

    }
}
