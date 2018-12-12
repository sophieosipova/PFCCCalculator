using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PFCCCalculatorService.Models;
using PFCCCalculatorService.Services;

using SharedModels;

namespace PFCCCalculatorService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController: ControllerBase
    {
        private readonly IProductsService productsService;
        private readonly ILogger <ProductsController>  logger;

        public ProductsController(IProductsService p, ILogger<ProductsController> logger)
        {
            this.productsService = p;
            this.logger = logger;
        }

        [HttpGet]
        [Route("categories")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(List<ProductsCategoryModel>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetProductsCategories()
        {
            
            try
            {
                var product = await productsService.GetProductsCategories();
                if (product != null)
                {
                    logger.LogInformation("GET ---", product.ToString());
                    return Ok(product);
                }
                logger.LogInformation("GET --- Not found");
            }
            catch (Exception e)
            {
                logger.LogInformation("GET ---", e.Message);
                return Conflict(e.Message);
            }

            return NotFound();
        }

        [HttpGet]
        [Route("categories/{productCategoryId:int}")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ProductModel), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetProductsByCategoryId(int productCategoryId)
        {
            if (productCategoryId <= 0)
                return BadRequest();

            try
            {
                var product = await productsService.GetProductsByCategoryId(productCategoryId);
               
                if (product != null)
                {
                    logger.LogInformation("GET ---", product.ToString());
                    return Ok(product);
                }
                logger.LogInformation("GET --- Not Found");
            }
            catch (Exception e)
            {
                logger.LogInformation("GET ---", e.Message);
                return Conflict(e.Message);
            }

            return NotFound();
        }

  /*      [HttpGet]
        [Route("{productId:int}")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ProductModel), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetProductById(int productId)
        {
            if (productId <= 0)
                return BadRequest();
            try
            {
                var product =  await productsService.GetProductById(productId);

                if (product != null)
                    return Ok(product);
            }
            catch (Exception e)
            {
                return Conflict(e.Message);
            }

            return NotFound();
        } */

        [HttpGet]
        [ProducesResponseType(typeof(PaginatedModel<ProductModel>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(IEnumerable<ProductModel>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetProducts([FromQuery]int pageSize = 10, [FromQuery]int pageIndex = 0)
        {
            try
            {
                var model = await productsService.Items(pageSize, pageIndex);

                if (model != null)
                {
                    logger.LogInformation("GET ---", model.ToString());
                    return Ok(model);
                }
                logger.LogInformation("GET --- Not Found");
                return NotFound();
            }
            catch (Exception e)
            {
                logger.LogInformation("GET ---", e.Message);
                return Conflict(e.Message);
            }
            
        }
    }
}
