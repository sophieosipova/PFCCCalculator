
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
        Task<List<DishModel>> GetDishes();
        Task<List<DishModel>> GetDishesWithProduct(int productId);
        Task<DishModel> GetDishById(int dishId);

        Task<DishModel> CreateDish(int userId, DishModel dish);
        Task <bool> DeleteDish(int userId, int dishId);
        //  Task<IActionResult> UpdateDish(Dish dishToUpdate);
    }
}
