﻿
using PFCCCalculatorService.Models;
using SharedModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PFCCCalculatorService.Services
{
     public interface IDishesService
    {
        Task<List<DishModel>> GetDishes();
        Task<List<DishModel>> GetDishesWithProduct(int productId);
        Task<DishModel> GetDishById(int dishId);

        Task<DishModel> CreateDish(string userId, DishModel dish);
        Task <bool> DeleteDish(string userId, int dishId);
        Task<DishModel> UpdateDish(string userId, DishModel dishToUpdate);

        Task<PaginatedModel<DishModel>> Items(int pageSize = 0, int pageIndex = 0);
    }
}
