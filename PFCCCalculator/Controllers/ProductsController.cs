using System;
using System.Collections.Generic;
using System.Linq;
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
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(List<ProductModel>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetProducts()
        {
            //p  = await productsService.GetProducts()
            logger.LogWarning(this.Request.ToString()+"HELLLOOOOOO");
            return Ok(await productsService.GetProducts());
        }

        [HttpGet]
        [Route("categories")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(List<ProductsCategoryModel>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetProductsCategories()
        {
            return Ok(await productsService.GetProductsCategories());
        }

        [HttpGet]
        [Route("categories/{productCategoryId:int}")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ProductModel), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetProductsByCategoryId(int productCategoryId)
        {
            if (productCategoryId <= 0)
                return BadRequest();

            var product = await productsService.GetProductsByCategoryId(productCategoryId);

            if (product != null)
                return Ok(product);

            return NotFound();
        }

        [HttpGet]
        [Route("{productId:int}")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ProductModel), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetProductById(int productId)
        {
            if (productId <= 0)
                return BadRequest();

            var product = await productsService.GetProductById(productId);

            if (product != null)
                return Ok(product);

            return NotFound();
        }

        [HttpGet]
        [Route("items")]
        [ProducesResponseType(typeof(PaginatedModel<ProductModel>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(IEnumerable<ProductModel>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Items([FromQuery]int pageSize = 10, [FromQuery]int pageIndex = 0)
        {

            var model = await productsService.Items(pageSize, pageIndex);

            return Ok(model);
        }
    }
}
