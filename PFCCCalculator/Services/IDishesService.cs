
using Microsoft.AspNetCore.Mvc;
using PFCCCalculatorService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PFCCCalculatorService.Services
{
     public interface IDishesService
    {
        Task<List<Dish>> GetDishes();
        Task<List<Dish>> GetDishesWithProduct(int productId);
        Task<Dish> GetDishById(int dishId);

        Task<Dish> CreateDish(int userId, Dish dish);
        Task <bool> DeleteDish(int userId, int dishId);
        //  Task<IActionResult> UpdateDish(Dish dishToUpdate);
    }
}
