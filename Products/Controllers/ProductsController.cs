using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using ProductsService.Models;
using System.Net;
using System.Threading.Tasks;
using SharedModels;
using ProductsService.Database;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace ProductsService.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        //private readonly ProductsContext db;

        private readonly IProductRepository productRepository;
        public ProductsController(IProductRepository productRepository)
        {
            this.productRepository = productRepository;
        }

        // GET api/products
        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(List<Product>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult>  GetProducts ()
        {
            var products = await productRepository.GetProducts();

            if (products == null)
                return NotFound();

            return Ok(products);
        }

        [HttpGet, AllowAnonymous]
        [Route("users/{userId}")]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(List<Product>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetUsersProducts(string userId)
        {
            if (userId == "")
                return BadRequest();

            var products = await productRepository.GetUsersProducts(userId);

            if (products == null)
                return NotFound();

            return Ok(products);
        }

        [HttpGet, AllowAnonymous]
        [Route("categories")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(List<ProductsCategory>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetProductsCategories()
        {
            var productCategories = await productRepository.GetProductsCategories();

            if (productCategories == null)
                return NotFound();

            return Ok(productCategories);
        }

        [HttpGet, AllowAnonymous]
        [Route("categories/{productCategoryId:int}")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetProductsByCategoryId(int productCategoryId)
        {        
            if (productCategoryId <= 0)
                return BadRequest();

            var productCategory = await productRepository.GetProductsByCategoryId(productCategoryId);

            if (productCategory == null)
                return NotFound();

            return Ok(productCategory);

        }

        [HttpGet, AllowAnonymous]
        [Route("{productId:int}")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetProductById(int productId)
        {
            if (productId <= 0)
                return BadRequest();

            var product = await productRepository.GetProductById(productId);

            if (product != null)
                return Ok(product);

            return NotFound();
        }

        
        [Route("user/{userId}")]
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        public async Task<IActionResult> CreateProduct(string userId,Product product)
        {
            Product p = await productRepository.CreateProduct(userId, product);
            if (p == null)
                return Conflict();
            else
                return Created("",p);

        }

        //DELETE api/v1/[controller]/id
        [Route("user/{userId}/{productId:int}")]
        [HttpDelete]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        public async Task<IActionResult> DeleteProduct(string userId, int productId)
        {
            if (!await productRepository.DeleteProduct(userId, productId))
                return NotFound();
            else
                return NoContent();
        }

        [HttpPut]
        [Route("user/{userId}")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        public async Task<IActionResult> UpdateProduct(string userId, [FromBody]Product productToUpdate)
        { 
            var product = await productRepository.UpdateProduct(userId,productToUpdate);

            if (product == null)
                return NotFound();

            return Created("", product);
        }

        [HttpGet, AllowAnonymous]
        [Route("items")]
        [ProducesResponseType(typeof(PaginatedModel<Product>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Items([FromQuery]int pageSize = 10, [FromQuery]int pageIndex = 0)
        {
            var model = await productRepository.Items(pageSize, pageIndex);

            if (model == null)
                return NotFound();

            return Ok(model);
        }


    }

    
}
        


    
