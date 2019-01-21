using System;
using System.Net;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PFCCCalculatorService.Services;

using SharedModels;

namespace PFCCCalculatorService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PFCCCalculatorController : ControllerBase
    {
        private readonly IGatewayService gatewayService;
        private readonly ILogger<PFCCCalculatorController> logger;
        private readonly IAutorizationService autorizationService;

        public PFCCCalculatorController (IGatewayService gatewayService,  IAutorizationService autorizationService, ILogger<PFCCCalculatorController> logger)
        {
            this.gatewayService = gatewayService;
            this.autorizationService = autorizationService;
            this.logger = logger;
        }

        [HttpGet]
        [Route("recipe/{RecipeId:int}")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(PFCCRecipe), (int)HttpStatusCode.OK)]
         public async Task<IActionResult> GetRecipeWithPFCC(int RecipeId)
        {
            try
            {
                PFCCRecipe recipe = await gatewayService.GetRecipeWithPFCC(RecipeId);
                
                if (recipe == null)
                    return NotFound();
                logger.LogInformation("GET ---", recipe.ToString());
                return Ok(recipe);
            }
            catch (Exception e)
            {
                logger.LogInformation("GET ---", e.Message);
                return Conflict(e.Message);
            }

        }

        [HttpGet]
        [Route("recipe")]
        [ProducesResponseType(typeof(PaginatedModel<PFCCRecipe>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetRecipesWithPFCC([FromQuery]int pageSize = 0, [FromQuery]int pageIndex = 0)
        {
            try
            {
                PaginatedModel<PFCCRecipe> recipe = await gatewayService.GetRecipesWithPFCC();

                if (recipe == null)
                    return NotFound();
                logger.LogInformation("GET ---", recipe.ToString());
                return Ok(recipe);
            }
            catch (Exception e)
            {
                logger.LogInformation("GET ---", e.Message);
                return Conflict(e.Message);
            }

        }




        [HttpDelete]
        [Route("user/{userId}/recipe/{recipeId:int}")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(PFCCRecipe), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> DeleteRecipe(string userId, int recipeId)
        {
            try
            {
                if (!await autorizationService.ValidateToken(Request.Headers["Authorization"].ToString()))
                    return Unauthorized();

                if (!await gatewayService.DeleteDish(userId, recipeId))
                {
                    logger.LogInformation("DELETE --- fail");
                    return NotFound();
                }
               
            }
            catch (Exception e)
            {
                logger.LogInformation("DELETE ---", e.Message);
                return Conflict(e.Message);             
            }

            logger.LogInformation("DELETE --- Success");
            return NoContent();
        }


        [HttpDelete]
        [Route("user/{userId}/product/{productId:int}")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(PFCCRecipe), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> DeleteProduct(string userId, int productId)
        {
            if (productId < 0)
            {
                logger.LogInformation("DELETE --- fail");
                return BadRequest();
            }
            try
            {
                if (!await autorizationService.ValidateToken(Request.Headers["Authorization"].ToString()))
                    return Unauthorized();

                var result = await gatewayService.DeleteProduct(userId, productId);
                if (result == HttpStatusCode.Conflict)
                {
                    logger.LogInformation("DELETE --- fail");
                    return Conflict("Невозможно удалить");                  
                }
                if (result == HttpStatusCode.NotFound)
                {
                    logger.LogInformation("DELETE --- fail");
                    return NotFound();
                }
            }
            catch (Exception e)
            {
                logger.LogInformation("DELETE ---", e.Message);
                return Conflict(e.Message);
            }

            logger.LogInformation("DELETE --- Success");
            return NoContent();
        }
    }
}
