using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SharedModels;

using Dishes.Models;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace DishesService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DishesController : ControllerBase
    {
        private readonly DishesContext db;

        public DishesController(DishesContext context)
        {
            this.db = context;
            if (!db.Ingredients.Any() && !db.Dishes.Any())
            {
                db.Dishes.Add(new Dish
                {
                    DishName = "Бутерброд с сыром",
                    DishImage = "buter.jpg",
                    Recipe = "Обжарить каждый ломтики хлеба с одной стороны," +
                    " перевернуть, положить сыр, накрыть сверху обжаренной " +
                     "стороной ломтика хлеба. Обжарить бутерброд с двух сторон",
                    TotalWeight = 120
                });

                db.Ingredients.Add(new Ingredient { DishId = 1, ProductName = "Батон", ProductId = 2, Count = 80 }
               );
                db.Ingredients.Add(new Ingredient {
                    DishId = 1,
                    ProductName = "Сыр",
                    ProductId = 3,
                  Count = 40
                });

                db.SaveChanges();
            }
        }

        // GET api/products
        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(List<Dish>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetDishes()
        {
            List<Dish> dishes = await db.Dishes.ToListAsync();
            foreach (Dish dish in dishes)
            {
                List<Ingredient> ings = await db.Ingredients.Where(i => i.DishId == dish.DishId).ToListAsync();
                dish.Ingredients = ings;

            }

            return Ok(dishes);
        }

        [HttpGet]
        [Route("withproduct/{productId:int}")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(List<Dish>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetDishesByProduct(int productId)
        {
            List<Dish> dishes  = await db.Ingredients.Where(i => i.ProductId == productId).Join(db.Dishes,
                i => i.DishId, d => d.DishId, (i, d) => d).ToListAsync();
            //.Select(x => x.DishId).ToListAsync();

            if (dishes.Count == 0)
                return NotFound();
            foreach (Dish dish in dishes)
            {
                List<Ingredient> ings = await db.Ingredients.Where(i => i.DishId == dish.DishId).ToListAsync();
                dish.Ingredients = ings;

            }

            return Ok(dishes);
        }

        [HttpGet]
        [Route("{dishId:int}")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(Dish), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetDishById(int dishId)
        {
            if (dishId <= 0)
                return BadRequest();

            var dish = await db.Dishes.SingleOrDefaultAsync(d => d.DishId == dishId);

            if (dish != null)
            {
                List<Ingredient> ings = await db.Ingredients.Where(i => i.DishId == dish.DishId).ToListAsync();
                dish.Ingredients = ings;
                return Ok(dish);
            }

            return NotFound();
        }



        [Route("user/{userId:int}")]
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        public async Task<IActionResult> CreateDish([FromBody]Dish dish)
        {
            var item = new Dish
            {
                DishId = dish.DishId,
                DishName = dish.DishName,
                DishImage = dish.DishImage,
                Recipe = dish.Recipe,
            };
            db.Dishes.Add(item);

            await db.SaveChangesAsync();

            foreach (Ingredient ingredient in dish.Ingredients)
            {
                var iItem = new Ingredient
                {
                    
                    IngredientId = ingredient.IngredientId,
                    DishId = dish.DishId,
                    ProductId = ingredient.ProductId,
                    ProductName = ingredient.ProductName,
                    Count = ingredient.Count,
                };
                db.Ingredients.Add(iItem);
            }
            await db.SaveChangesAsync();

            
            return CreatedAtAction(nameof(GetDishById), new { dishId = item.DishId }, item);
        }


        //DELETE api/v1/[controller]/id
        [Route("user/{userId:int}/{dishId:int}")]
        [HttpDelete]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        public async Task<IActionResult> DeleteDish(int dishId)
        {
            var dish = db.Dishes.SingleOrDefault(d => d.DishId == dishId);

            if (dish == null)
                return NotFound();

            List<Ingredient> ingredients = db.Ingredients.Where(i => i.DishId == dishId).ToList();

            foreach (Ingredient i in ingredients)
                db.Ingredients.Remove(i);

            db.Dishes.Remove(dish);
            await db.SaveChangesAsync();

            return NoContent();
        }

        [Route("user/{userId:int}")]
        [HttpPut]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        public async Task<IActionResult> UpdateDish([FromBody]Dish dishToUpdate)
        {
            var dish = await db.Dishes
                .SingleOrDefaultAsync(i => i.DishId == dishToUpdate.DishId);

            if (dish == null)
                return NotFound(new { Message = $"Dish with id {dishToUpdate.DishId} not found." });

            dish = dishToUpdate;

            List<Ingredient> ingredients = await db.Ingredients.Where(i => i.DishId == dish.DishId).ToListAsync();


            foreach (Ingredient ingredient in dishToUpdate.Ingredients)
            {
                int i = ingredients.IndexOf(ingredient);
                if (i != -1)
                    db.Ingredients.Update(ingredient);
                else
                    db.Ingredients.Add(ingredient);
            }

            db.Dishes.Update(dish);
            await db.SaveChangesAsync();

            return CreatedAtAction(nameof(GetDishById), new { productID = dishToUpdate.DishId }, null);
        }

    }
}