using CommentsService.Models;
using Dishes.Models;
using Newtonsoft.Json;
using PFCCCalculatorService.Calculator;
using ProductsService.Models;
using SharedModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PFCCCalculatorService.Services
{
    public class GatewayService: IGatewayService
    {
        private readonly IProductsService productsService;
        private readonly IDishesService dishesService;
        private readonly ICommentsService commentsService;
        public GatewayService(IProductsService productsService, IDishesService dishesService, ICommentsService commentsService)
        {

            this.productsService = productsService;
            this.dishesService = dishesService;
            this.commentsService = commentsService;

        }
        public async Task<PFCCRecipe> GetRecipeWithPFCC(int RecipeId)
        {
            try
            {
                Dish dish = await dishesService.GetDishById(RecipeId);

                Product[] products = new Product[dish.Ingredients.Count];
                List<PFCCIngredient> ingredientsList = new List<PFCCIngredient>();
                foreach (Ingredient ingredient in dish.Ingredients)
                {
                    Product product = await productsService.GetProductById(ingredient.ProductId);
                    ingredientsList.Add(PFCCCalculations.CalculateIngredient(ingredient, product));
                }

                return PFCCCalculations.PFCCRecipeCalculator(dish, ingredientsList);
            }
            catch
            {
                return null;
            }
        }

        public async Task DeleteDish(int userId, int dishId)
        {
            await dishesService.DeleteDish(userId, dishId);

            try
            {
                PaginatedModel<Comment> model = await commentsService.GetCommentsByDishId(dishId);
                int n = Convert.ToInt32(model.Count / model.PageSize + 1);

                for (int i = 0; i < n; i++)
                {
                    foreach (Comment comment in model.Data)
                    {
                        await commentsService.DeleteComment(userId, comment.CommentId);
                    }
                }
            }
            catch
            {

            }
        }
        public async Task CreateProduct(int userId, Product product)
        {
             await productsService.CreateProduct(userId, product);
        }
        public async Task DeleteProduct(int userId, int productId)
        {
            await productsService.DeleteProduct(userId, productId);
            await dishesService.
        }
        public async Task UpdateProduct(int userId, Product productToUpdate)
        {
          //  await productsService.GetProductById(productToUpdate.ProductId);

            await productsService.UpdateProduct(userId, productToUpdate);
           // await dishesService.
        }

    }
}
