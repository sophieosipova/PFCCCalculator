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
                    Calories = 290,
                    Carbohydrates = 50,
                    Fat = 30,
                    Protein = 20
                });

                db.Ingredients.Add(new Ingredient {DishId = 1, ProductId = 2, Count = 2, Measure = "кусок"});
                db.Ingredients.Add(new Ingredient { DishId = 1, ProductId = 3, Count = 2, Measure = "ломтика" });

                db.SaveChanges();
            }
        }

        // GET api/products
        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(List<Dish>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetDishes()
        {
            /*var d = await db.Dishes.Select
                  (a => new {a.DishId,a.DishName,a.DishImage, a.Calories, a.Protein,a.Fat,a.Carbohydrates})
                  .ToListAsync();*/
            //var i = db.Ingredients.Where(i => i.DishId )

            List<Dish> dishes = await db.Dishes.ToListAsync();
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



        [Route("")]
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
                Protein = dish.Protein,
                Fat = dish.Fat,
                Carbohydrates = dish.Carbohydrates,
                Calories = dish.Calories,
               // Ingredients = dish.Ingredients
            };
            db.Dishes.Add(item);

            await db.SaveChangesAsync();

            foreach (Ingredient ingredient in dish.Ingredients)
            {
                var iItem = new Ingredient
                {
                    IngredientId = ingredient.IngredientId,
                    DishId = ingredient.DishId,
                    ProductId = ingredient.ProductId,
                    Measure = ingredient.Measure,
                    Count = ingredient.Count
                };
                db.Ingredients.Add(iItem);
            }
            await db.SaveChangesAsync();

            
            return CreatedAtAction(nameof(GetDishById), new { dishId = item.DishId }, null);
        }

      /*  [Route("{dishId:int}/ingredients")]
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        public async Task<IActionResult> CreateIngredient(int dishId,[FromBody] Ingredient ingredient)
        {
            var item = new Ingredient
            {
                IngredientId = ingredient.IngredientId,
                DishId = ingredient.DishId,
                ProductId = ingredient.ProductId,
                Measure = ingredient.Measure,
                Count = ingredient.Count
            };
            db.Ingredients.Add(item);

            await db.SaveChangesAsync();

            return CreatedAtAction(nameof(GetIngredientById), new { ingredientId = item.IngredientId }, null);
        }*/

        //DELETE api/v1/[controller]/id
        [Route("{dishId:int}")]
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

        [Route("")]
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

    /*    [HttpGet]
        [Route("{dishId:int}/ingredients/")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(List<Ingredient>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetIngredientsByDishId(int dishId)
        {
            if (dishId <= 0)
                return BadRequest();

            var dish = await db.Dishes.SingleOrDefaultAsync(d => d.DishId == dishId);

            if (dish != null)
                return Ok(await db.Ingredients.Where(i => i.DishId == dishId)
                    .OrderBy(c=> c.IngredientId).ToListAsync());

            return NotFound();
        }*/

 /*       [HttpGet]
        [Route("{dishId:int}/ingredients/{ingredientId:int}")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(Ingredient), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetIngredientById(int dishId, int ingredientId)
        {
            if (dishId <= 0)
                return BadRequest();

            var dish = await db.Dishes.SingleOrDefaultAsync(d => d.DishId == dishId);

            if (dish != null)
            {
                var ingredient = dish.Ingredients.Single(i => i.IngredientId == ingredientId);

                if (ingredient != null)
                    return Ok(ingredient);          
            }

            return NotFound();
        }

    */
    }
}