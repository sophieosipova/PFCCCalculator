using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using ProductsService.Models;
using System.Net;
using System.Threading.Tasks;
using SharedModels;



namespace ProductsService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly ProductsContext db;

        public ProductsController(ProductsContext context)
        {
            this.db = context;
            if (!db.ProductsCategories.Any())
            {
                db.ProductsCategories.Add(new ProductsCategory { ProductsCategoryName = "Овощи" });
                db.ProductsCategories.Add(new ProductsCategory { ProductsCategoryName = "Фрукты" });
                db.ProductsCategories.Add(new ProductsCategory { ProductsCategoryName = "Молочные продукты" });
                db.ProductsCategories.Add(new ProductsCategory { ProductsCategoryName = "Выпечка" });
                db.SaveChanges();
            } 
            if (!db.Products.Any())
            {
                db.Products.Add(new Product { ProductName = "Яблоко",ProductsCategoryId =2, Protein = 0, Fat = 0, Carbohydrates = 12, Calories = 60 });
                db.Products.Add(new Product { ProductName = "Сыр", ProductsCategoryId = 3, Protein = 15, Fat = 30, Carbohydrates = 1, Calories = 280 });
                db.Products.Add(new Product { ProductName = "Батон", ProductsCategoryId = 4, Protein = 1, Fat = 2, Carbohydrates = 50, Calories = 290 });
                db.SaveChanges();
            }
        }

        // GET api/products
        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(List<Product>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult>  GetProducts ()
        {
            return Ok( await db.Products.Where(p => p.UserId == 0).ToListAsync());
        }

        [HttpGet]
        [Route("users/{userId:int}")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(List<Product>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetUsersProducts(int userId)
        {
            return Ok(await db.Products.Where(p => p.UserId == userId).ToListAsync());
        }

        [HttpGet]
        [Route("categories")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(List<ProductsCategory>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetProductsCategories()
        {
            return Ok(await db.ProductsCategories.ToListAsync());
        }

        [HttpGet]
        [Route("categories/{productCategoryId:int}")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetProductsByCategoryId(int productCategoryId)
        {
            if (productCategoryId <= 0)
            {
                return BadRequest();
            }

            var product = await db.Products.Where(p => p.ProductsCategoryId == productCategoryId).ToListAsync();


            if (product != null)
            {
                return Ok(product);
            }

            return NotFound();
        }

        [HttpGet]
        [Route("{productId:int}")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetProductById(int productId)
        {
            if (productId <= 0)
            {
                return BadRequest();
            }

            var product = await db.Products.SingleOrDefaultAsync(ci => ci.ProductId == productId);


            if (product != null)
            {
                return Ok(product);
            }

            return NotFound();
        }

        
        [Route("user/{userId:int}")]
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        public async Task<IActionResult> CreateProduct(int userId,[FromBody]Product product)
        {
            var item = new Product
            {
                UserId = userId,
                ProductId = product.ProductId,
                ProductName = product.ProductName,
                Protein = product.Protein,
                Fat = product.Fat,
                Carbohydrates = product.Carbohydrates,
                Calories = product.Calories
            };
            db.Products.Add(item);

            await db.SaveChangesAsync();

            return CreatedAtAction(nameof(GetProductById), new { productId  = item.ProductId }, null);
        }

        //DELETE api/v1/[controller]/id
        [Route("user /{userId:int}/{productId:int}")]
        [HttpDelete]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        public async Task<IActionResult> DeleteProduct(int userId,int productId)
        {
            var product = db.Products.SingleOrDefault(x => x.ProductId == productId);

            if (product == null)
            {
                return NotFound();
            }

            db.Products.Remove(product);

            await db.SaveChangesAsync();

            return NoContent();
        }

        [HttpPut]
        [Route("user /{userId:int}")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        public async Task<IActionResult> UpdateProduct([FromBody]Product productToUpdate)
        {
            var product = await db.Products
                .SingleOrDefaultAsync(i => i.ProductId == productToUpdate.ProductId);

            if (product == null)
            {
                return NotFound(new { Message = $"Product with id {productToUpdate.ProductId} not found." });
            }

       
            product = productToUpdate;
            db.Products.Update(product);

             await db.SaveChangesAsync();


            return CreatedAtAction(nameof(GetProductById), new { productID = productToUpdate.ProductId}, null);
        }

        [HttpGet]
        [Route("items")]
        [ProducesResponseType(typeof(PaginatedModel<Product>), (int)HttpStatusCode.OK)]
       // [ProducesResponseType(typeof(IEnumerable<Product>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Items([FromQuery]int pageSize = 10, [FromQuery]int pageIndex = 0)
        {

            var totalItems = await db.Products
                .LongCountAsync();

            var itemsOnPage = await db.Products
                .OrderBy(c => c.ProductName)
                .Skip(pageSize * pageIndex)
                .Take(pageSize)
                .ToListAsync();

           

            var model = new PaginatedModel<Product>(pageIndex, pageSize, totalItems, itemsOnPage);

            return Ok(model);
        }

      



    }

    
}
        


    
