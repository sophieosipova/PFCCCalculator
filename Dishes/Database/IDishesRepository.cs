using Dishes.Models;
using SharedModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DishesService.Database
{
    public interface IDishesRepository
    {
        Task<List<Dish>> GetDishes();
        Task<List<Dish>> GetDishesByProduct(int productId);
        Task<Dish> GetDishById(int dishId);
        Task<Dish> CreateDish(int userId, Dish dish);
        Task<bool> DeleteDish(int userId, int dishId);
        Task<Dish> UpdateDish(int userId, Dish dishToUpdate);

        Task<PaginatedModel<Dish>> Items(int pageSize = 0, int pageIndex = 0);
    }
}
