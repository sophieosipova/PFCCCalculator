using Dishes.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace PFCCCalculatorService.Services
{
    public class DishesService : IDishesService
    {
        private readonly HttpClient httpClient;
        private readonly string remoteServiceBaseUrl = "http://localhost:58029";

        public DishesService(HttpClient httpClient)
        {

            this.httpClient = new HttpClient();

            // this.remoteServiceBaseUrl = remoteServiceBaseUrl;
        }

        public async Task<List<Dish>> GetDishes()
        {
            var uri = $"{remoteServiceBaseUrl}/api/dishes";

            var dishes = JsonConvert.DeserializeObject<List<Dish>>(await httpClient.GetStringAsync(uri));
            return dishes;
        }

  
        public async Task<Dish> GetDishById(int dishId)
        {
            var uri = $"{remoteServiceBaseUrl}/api/dishes/{dishId}";


            var dish = JsonConvert.DeserializeObject<Dish>
                (await httpClient.GetStringAsync(uri));

            return dish;

        }
        



    }
}
