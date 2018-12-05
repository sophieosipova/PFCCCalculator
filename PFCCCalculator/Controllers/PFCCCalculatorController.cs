using System;
using System.Net;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using PFCCCalculatorService.Services;

using SharedModels;

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
        [Route("recipe/{RecipeId:int}")]
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
                return Conflict(e.Message);
            }

        }


        [HttpDelete]
        [Route("user/{userId:int}/recipe/{recipeId:int}")]
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
                return Conflict(e.Message);
            }

            return NoContent();
        }


        [HttpDelete]
        [Route("user/{userId:int}/product/{productId:int}")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(PFCCRecipe), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> DeleteProduct(int userId, int productId)
        {
            if (productId < 0)
                return BadRequest();
            try
            {
                var result = await gatewayService.DeleteProduct(userId, productId);
                if (result == HttpStatusCode.Conflict)
                    return Conflict("Не возможно удалить");
                if (result == HttpStatusCode.NotFound)
                    return NotFound();
            }
            catch (Exception e)
            {
                return Conflict(e.Message);
            }

            return NoContent();
        }
    }
}
