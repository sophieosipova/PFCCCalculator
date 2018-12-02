using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PFCCCalculatorService.Services;

using SharedModels;
using PFCCCalculator;
using PFCCCalculatorService.Calculator;

namespace PFCCCalculatorService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PFCCCalculatorController : ControllerBase
    {
        private readonly IGatewayService gatewayService;

        public PFCCCalculatorController (IGatewayService gatewayService)
        {
            this.gatewayService = gatewayService;
        }

        [HttpGet]
        [Route("{RecipeId:int}")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(PFCCRecipe), (int)HttpStatusCode.OK)]
         public async Task<IActionResult> GetRecipeWithPFCC(int RecipeId)
        {
            /*  Dish dish =  JsonConvert.DeserializeObject<Dish>(dishesService.GetDishById(RecipeId).ToString());

              Product[] products = new Product[dish.Ingredients.Count];
              List < PFCCIngredient >  ingredientsList = new List<PFCCIngredient>();
              foreach (Ingredient ingredient in dish.Ingredients)
              {
                  Product product = JsonConvert.DeserializeObject<Product>
                      (productsService.GetProductById(ingredient.ProductId).ToString());
                  ingredientsList.Add(PFCCCalculations.CalculateIngredient(ingredient, product));
              } */
            try
            {
                PFCCRecipe recipe = await gatewayService.GetRecipeWithPFCC(RecipeId);
                if (recipe == null)
                    return NotFound();

                return Ok(recipe);
            }
            catch (Exception e)
            {
                return Conflict(e);
            }

        }


        [HttpDelete]
        [Route("user/{userId:int}/{recipeId:int}")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(PFCCRecipe), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> DeleteRecipe(int userId, int recipeId)
        {
            try
            {
                if (!await gatewayService.DeleteDish(userId, recipeId))
                    return NotFound();
               
            }
            catch (Exception e)
            {
                return Conflict(e);
            }

            return NoContent();
        }
    }
}