

using Newtonsoft.Json;
using PFCCCalculatorService.Calculator;
using PFCCCalculatorService.Models;

using SharedModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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

                if (dish == null)
                    return null;

                Product[] products = new Product[dish.Ingredients.Count];
                List<PFCCIngredient> ingredientsList = new List<PFCCIngredient>();
                foreach (Ingredient ingredient in dish.Ingredients)
                {
                    Product product = await productsService.GetProductById(ingredient.ProductId);

                    if (product == null)
                        return null;
                    ingredientsList.Add(PFCCCalculations.CalculateIngredient(ingredient, product));
                }

                return PFCCCalculations.PFCCRecipeCalculator(dish, ingredientsList);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<bool> DeleteDish(int userId, int dishId)
        {
            await dishesService.DeleteDish(userId, dishId);

            try
            {
                PaginatedModel<Comment> model = await commentsService.GetCommentsByDishId(dishId);

                if (model == null)
                    return false;
                int n = Convert.ToInt32(model.Count / model.PageSize + 1);

                for (int i = 0; i < n; i++)
                {
                    foreach (Comment comment in model.Data)
                    {
                        if (!await commentsService.DeleteComment(userId, comment.CommentId))
                            return false;
                    }
                }
            }
            catch (HttpRequestException e)
            {
                throw e;
            }

            return true;
        }
        public async Task<bool> CreateProduct(int userId, Product product)
        {
            try
            {
                return await productsService.CreateProduct(userId, product);
            }
            catch (HttpRequestException e)
            {
                throw e;
            }
        }
        public async Task<bool> DeleteProduct(int userId, int productId)
        {
            try
            {
                if (await dishesService.GetDishesWithProduct(productId) == null)
                    return await productsService.DeleteProduct(userId, productId);
            }
            catch(HttpRequestException e)
            {
                throw e;
            }

            return false;
        }
        public async Task<bool> UpdateProduct(int userId, Product productToUpdate)
        {
            try
            {
               return await productsService.UpdateProduct(userId, productToUpdate);
            }
            catch (HttpRequestException e)
            {
                throw e;
            }
        }

    }
}
