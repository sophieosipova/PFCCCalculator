

using PFCCCalculatorService.Calculator;
using PFCCCalculatorService.Models;

using SharedModels;
using System;
using System.Collections.Generic;
using System.Net;
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
                DishModel dish = await dishesService.GetDishById(RecipeId);

                if (dish == null)
                    return null;

                ProductModel[] products = new ProductModel[dish.Ingredients.Count];
                List<PFCCIngredient> ingredientsList = new List<PFCCIngredient>();
                foreach (IngredientModel ingredient in dish.Ingredients)
                {
                    ProductModel product = await productsService.GetProductById(ingredient.ProductId);

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
            
            try
            {
                await dishesService.DeleteDish(userId, dishId);

                PaginatedModel<CommentModel> model = await commentsService.GetCommentsByDishId(dishId);

                if (model == null)
                    return true;
                int n = Convert.ToInt32(model.Count / model.PageSize + 1);

                for (int i = 0; i < n; i++)
                {
                    foreach (CommentModel comment in model.Data)
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
     /*   public async Task<Dish> CreateProduct(int userId, Product product)
        {
            try
            {
                return await productsService.CreateProduct(userId, product);
            }
            catch (HttpRequestException e)
            {
                throw e;
            }
        }*/
        public async Task<HttpStatusCode> DeleteProduct(int userId, int productId)
        {
            try
            {
                if (await dishesService.GetDishesWithProduct(productId) == null)
                    if (await productsService.DeleteProduct(userId, productId))
                    {
                        return HttpStatusCode.NoContent;
                    }
                    else
                            return HttpStatusCode.NotFound;
            }
            catch(HttpRequestException e)
            {
                throw e;
            }

            return HttpStatusCode.Conflict;
        }
     /*   public async Task<bool> UpdateProduct(int userId, ProductModel productToUpdate)
        {
            try
            {
               return await productsService.UpdateProduct(userId, productToUpdate);
            }
            catch (HttpRequestException e)
            {
                throw e;
            }
        }*/

    }
}
