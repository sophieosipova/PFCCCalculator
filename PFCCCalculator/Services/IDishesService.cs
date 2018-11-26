using Dishes.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PFCCCalculatorService.Services
{
     public interface IDishesService
    {
        Task<List<Dish>> GetDishes();
        Task<Dish> GetDishById(int dishId);

        //Task<IActionResult> CreateDish(Dish dish);
        // Task<IActionResult> DeleteDish(int dishId)
        //  Task<IActionResult> UpdateDish(Dish dishToUpdate);
    }
}
