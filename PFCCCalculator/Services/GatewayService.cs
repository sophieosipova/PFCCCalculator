

using DalSoft.Hosting.BackgroundQueue;
using PFCCCalculatorService.Calculator;
using PFCCCalculatorService.Models;

using SharedModels;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace PFCCCalculatorService.Services
{
    public class GatewayService : IGatewayService
    {
        private readonly IProductsService productsService;
        private readonly IDishesService dishesService;
        private readonly ICommentsService commentsService;
        private  BackgroundQueue backgroundQueue;
    
        private static Queue<Message> messages = new Queue<Message>();

        ConcurrentQueue<Task> queue = new ConcurrentQueue<Task> ();

        public GatewayService(IProductsService productsService, IDishesService dishesService, ICommentsService commentsService, BackgroundQueue backgroundQueue)
        {

            this.productsService = productsService;
            this.dishesService = dishesService;
            this.commentsService = commentsService;
            this.backgroundQueue = backgroundQueue;

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

                List <ProductModel> productsList = await productsService.GetProducts();

                if (productsList == null)
                {
                    return PFCCCalculations.PFCCRecipeCalculator(dish);
                }
                foreach (IngredientModel ingredient in dish.Ingredients)
                {
                    ProductModel product = productsList.SingleOrDefault(ci => ci.ProductId == ingredient.ProductId);//await productsService.GetProductById(ingredient.ProductId);

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


        public async Task<PaginatedModel<PFCCRecipe>> GetRecipesWithPFCC(int pageSize = 10, int pageIndex = 0)
        {
            try
            {
                PaginatedModel <DishModel> dishes = await dishesService.Items(pageSize,pageIndex);

                if (dishes == null)
                    return null;

                // PaginatedModel<PFCCRecipe> pFCCdishes = new PaginatedModel<PFCCRecipe>(pageIndex,pageSize,dishes.Count)
                List<PFCCRecipe> recipes = new List<PFCCRecipe>();

                List<ProductModel> productsList = await productsService.GetProducts();

                if (productsList == null)
                {
                    foreach (DishModel dish in dishes.Data)
                    { 
                    
                        recipes.Add(PFCCCalculations.PFCCRecipeCalculator(dish));
                    }
                    return  new PaginatedModel<PFCCRecipe>(pageIndex, pageSize, dishes.Count, recipes);
                }
                foreach (DishModel dish in dishes.Data)
                {
                    ProductModel[] products = new ProductModel[dish.Ingredients.Count];
                    List<PFCCIngredient> ingredientsList = new List<PFCCIngredient>();

                    foreach (IngredientModel ingredient in dish.Ingredients)
                    {
                        ProductModel product = await productsService.GetProductById(ingredient.ProductId);

                        if (product == null)
                            return null;
                        ingredientsList.Add(PFCCCalculations.CalculateIngredient(ingredient, product));
                    }

                    recipes.Add(PFCCCalculations.PFCCRecipeCalculator(dish));
                }

                return new PaginatedModel<PFCCRecipe>(pageIndex, pageSize, dishes.Count, recipes);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<bool> DeleteDish(string userId, int dishId)
        {
            
            try
            {
                PaginatedModel<CommentModel> model = await commentsService.GetCommentsByDishId(dishId);

                if (model == null)
                    return true;
               //int n = Convert.ToInt32(model.Count / model.PageSize + 1);

                //for (int i = 0; i < n; i++)
                //  {
                try
                {
                    foreach (CommentModel comment in model.Data)
                    {
                        if (!await commentsService.DeleteComment(userId, comment.CommentId))
                        {
                            return false;
                        }
                    }
                }
                catch
                {

                }
                //}

                await dishesService.DeleteDish(userId, dishId);
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
        public async Task<HttpStatusCode> DeleteProduct(string userId, int productId)
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

        public async Task<HttpStatusCode> UpdateProduct(string userId, ProductModel product)
        {
            try
            {
              //  await productsService.UpdateProduct(userId, product);
                //  List <DishModel>  dishes = await dishesService.GetDishesWithProduct(product.ProductId);

                backgroundQueue.Enqueue(async cancellationToken =>
                {
                    bool stop = false;
                    while (!stop)
                    {
                        try
                        {
                            await productsService.UpdateProduct(userId, product);
                            stop = true;
                        }
                        catch
                        {
                            Thread.Sleep(10000);
                        }
                    }

                });


                //Task<Object> task = new Task<Object>(( ) => { return dishesService.GetDishesWithProduct(product.ProductId); });

                backgroundQueue.Enqueue(async cancellationToken =>
                {
                    bool stop = false;
                    while (!stop)
                    {
                        try
                        {
                            var dishes = await dishesService.GetDishesWithProduct(product.ProductId);

                            if (dishes != null)
                            {
                                foreach (DishModel dish in dishes)
                                {
                                    dish.Ingredients.Where(ing => ing.ProductId == product.ProductId).First().ProductName = product.ProductName;
                                    bool flag = false;
                                    while (!flag)
                                    {
                                        try
                                        {
                                            await dishesService.UpdateDish(userId, dish);
                                            flag = true;
                                        }
                                        catch
                                        {
                                            Thread.Sleep(10000);
                                        }
                                    }
                                }

                            }
                            stop = true;
                        }
                        catch
                        {
                            Thread.Sleep(10000);
                        }
                    }

                });


                /*      if (dishes != null)
                      {
                          foreach (DishModel dish in dishes )
                          {
                              dish.Ingredients.Where(ing => ing.ProductId == product.ProductId).First().ProductName = product.ProductName;
                              await dishesService.UpdateDish(userId, dish);
                          }

                      }*/

                return HttpStatusCode.OK; 
            }
            catch (HttpRequestException e)
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
