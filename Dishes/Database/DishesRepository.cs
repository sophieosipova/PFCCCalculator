using Dishes.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DishesService.Database
{
    public class DishesRepository : IDishesRepository
    {
        private readonly DishesContext db;

        public DishesRepository(DishesContext context)
        {
            this.db = context;
            if (!db.Ingredients.Any() && !db.Dishes.Any())
            {
                db.Dishes.Add(new Dish
                {
                    DishName = "Бутерброд с сыром",
                    Recipe = "Обжарить каждый ломтики хлеба с одной стороны," +
                    " перевернуть, положить сыр, накрыть сверху обжаренной " +
                     "стороной ломтика хлеба. Обжарить бутерброд с двух сторон",
                    TotalWeight = 120
                });

                db.Ingredients.Add(new Ingredient { DishId = 1, ProductName = "Батон", ProductId = 2, Count = 80 }
               );
                db.Ingredients.Add(new Ingredient
                {
                    DishId = 1,
                    ProductName = "Сыр",
                    ProductId = 3,
                    Count = 40
                });

                db.SaveChanges();
            }
        }

        public async Task<List<Dish>> GetDishes()
        {
            try
            {
                List<Dish> dishes = await db.Dishes.ToListAsync();

                if (dishes.Count == 0)
                    return null;

                foreach (Dish dish in dishes)
                {
                    List<Ingredient> ings = await db.Ingredients.Where(i => i.DishId == dish.DishId).ToListAsync();
                    dish.Ingredients = ings;
                }

                return dishes;

            }
            catch
            {
                return null;
            }
 
        }

        public async Task<List<Dish>> GetDishesByProduct(int productId)
        {
            try
            {
                List<Dish> dishes = await db.Ingredients.Where(i => i.ProductId == productId).Join(db.Dishes,
                i => i.DishId, d => d.DishId, (i, d) => d).ToListAsync();
                //.Select(x => x.DishId).ToListAsync();

                if (dishes.Count == 0)
                    return null;

                foreach (Dish dish in dishes)
                {
                    List<Ingredient> ings = await db.Ingredients.Where(i => i.DishId == dish.DishId).ToListAsync();
                    dish.Ingredients = ings;

                }
                return dishes;
            }
            catch
            {
                return null;
            }

        }


        public async Task<Dish> GetDishById(int dishId)
        {
            try
            { 
                var dish = await db.Dishes.SingleOrDefaultAsync(d => d.DishId == dishId);

                if (dish != null)
                {
                    List<Ingredient> ings = await db.Ingredients.Where(i => i.DishId == dish.DishId).ToListAsync();
                    dish.Ingredients = ings;
                    return dish;
                }
            }
            catch
            {
                return null;
            }

            return null;
        }



        public async Task<Dish> CreateDish(int userId, Dish dish)
        {
            var item = new Dish
            {
                DishName = dish.DishName,
                Recipe = dish.Recipe,
                TotalWeight = dish.TotalWeight,
                UserId = dish.UserId
            };

            try
            {
                db.Dishes.Add(item);
                db.SaveChanges();

                Dish createdDish = db.Dishes.Last();
                int dishId = createdDish.DishId;
                List<Ingredient> ingredients = new List<Ingredient>();
                foreach (Ingredient ingredient in dish.Ingredients)
                {
                    var iItem = new Ingredient
                    {
                        IngredientId = ingredient.IngredientId,
                        DishId = dishId,
                        ProductId = ingredient.ProductId,
                        ProductName = ingredient.ProductName,
                        Count = ingredient.Count,
                    };
                    ingredients.Add(iItem);
                }

                await db.Ingredients.AddRangeAsync(ingredients);
                await db.SaveChangesAsync();
                createdDish.Ingredients = ingredients;

                return createdDish;
            }
            catch
            {
                return null;
            }
        }


        public async Task<bool> DeleteDish(int userId, int dishId)
        {
            try
            {
                var dish = db.Dishes.SingleOrDefault(d => d.DishId == dishId && userId == d.UserId);

                if (dish == null)
                    return false;

                List<Ingredient> ingredients = db.Ingredients.Where(i => i.DishId == dishId).ToList();

                foreach (Ingredient i in ingredients)
                    db.Ingredients.Remove(i);


                db.Dishes.Remove(dish);
                await db.SaveChangesAsync();
            }
            catch
            {
                return false;
            }

            return true; 
        }


        public async Task<Dish> UpdateDish(int userId, Dish dishToUpdate)
        {
            try
            {
                var dish = await db.Dishes
                    .SingleOrDefaultAsync(i => i.DishId == dishToUpdate.DishId);
                if (dish == null)
                    return null;

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

                return dish;
            }
            catch
            {
                return null;
            }
        }
    }
}
