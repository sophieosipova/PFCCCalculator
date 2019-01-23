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
        private readonly IAutorizationService autorizationService;

        
        public ProductsController(IProductsService p,IAutorizationService a, ILogger<ProductsController> logger)
        {
            this.productsService = p;
            this.autorizationService = a;
            this.logger = logger;
           // this.token = this.productsService.Login();
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
                if (!await autorizationService.ValidateToken(Request.Headers["Authorization"].ToString()))
                    return Unauthorized();

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

        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        [Route("user/{userId}")]
        public async Task<IActionResult> CreateProduct(string userId, ProductModel product)
        {
            try
            {
                if (!await autorizationService.ValidateToken(Request.Headers["Authorization"].ToString()))
                    return Unauthorized();

                if (product == null)
                {
                    ModelState.AddModelError("", "Продукт не задан");
                    return BadRequest(ModelState);
                }
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var created = await productsService.CreateProduct(userId, product);
                if (created != null)
                    return Created("", created);
                return Conflict("Не удалось создать");

            }
            catch (Exception e)
            {
                return Conflict(e.Message);
            }
        }


        [HttpPut]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        [Route("user/{userId}")]
        public async Task<IActionResult> UpdateDish(string UserId, ProductModel product)
        {
            try
            {
                var created = await productsService.UpdateProduct(UserId, product);
                if (created != null)
                    return Created("", created);
                return Conflict("Не удалось обновить");

            }
            catch (Exception e)
            {
                return Conflict(e.Message);
            }
        }




        [HttpGet]
        [ProducesResponseType(typeof(PaginatedModel<ProductModel>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(List<ProductModel>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetProducts([FromQuery]int pageSize = 0, [FromQuery]int pageIndex = 0)
        {

           var t = this.Request.Headers;
            try
            {
                if (pageSize == 0)
                    return Ok(await productsService.GetProducts());

                PaginatedModel<ProductModel>   model = await productsService.Items(pageSize, pageIndex);
            

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

        // GET api/products
        [HttpGet]
        [Route("all")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(List<ProductModel>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetProducts()
        {
            try
            {
                var products = await productsService.GetProducts();

                if (products == null)
                    return NotFound();
                return Ok(products);
            }
            catch
            {
                return NotFound();
            }
            
        }
    }
}
