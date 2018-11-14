using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Products.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Products.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
            ProductsContext db;
            public ProductsController(ProductsContext context)
            {
                this.db = context;
                if (!db.Products.Any())
                {
                    db.Products.Add(new Product { ProductName = "Яблоко", Protein = 0, Fat = 0, Carbohydrates = 12, Calories = 60 });
                    db.Products.Add(new Product { ProductName = "Сыр", Protein = 15, Fat = 30, Carbohydrates = 1, Calories = 280 });
                    db.Products.Add(new Product {  ProductName = "Батон", Protein = 1, Fat = 2, Carbohydrates = 50, Calories = 290 });
                    db.SaveChanges();
                }
            }

             // GET api/products
            [HttpGet]
            public ActionResult<List<Product>> Products()
            {
                return db.Products.ToList();
            }

            // GET api/products/id
            [HttpGet("{id}")]
            public IActionResult Products(int id)
            {
                Product product = db.Products.FirstOrDefault(x => x.ProductId == id);
                if (product == null)
                    return NotFound();
                return new ObjectResult(product);
            }

            // POST api/products
            [HttpPost]
            public IActionResult Post([FromBody]Product product)
            {
                if (product == null)
                {
                    return BadRequest();
                }

                db.Products.Add(product);
                db.SaveChanges();
                return Ok(product);
            }

            // PUT api/products/
            [HttpPut]
            public IActionResult Put([FromBody]Product product)
            {
                if (product == null)
                {
                    return BadRequest();
                }
                if (!db.Products.Any(x => x.ProductId == product.ProductId))
                {
                    return NotFound();
                }

                db.Update(product);
                db.SaveChanges();
                return Ok(product);
            }

            // DELETE api/products/id
            [HttpDelete("{id}")]
            public IActionResult Delete(int id)
            {
                Product product = db.Products.FirstOrDefault(x => x.ProductId == id);
                if (product == null)
                {
                    return NotFound();
                }
                db.Products.Remove(product);
                db.SaveChanges();
                return Ok(product);
            }
    }
}
