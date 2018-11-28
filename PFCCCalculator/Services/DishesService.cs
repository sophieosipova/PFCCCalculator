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


            var t = await httpClient.GetAsync(uri);
         //   var i = await httpClient.GetStringAsync(uri);
         //   httpClient.
       //     var k = t.Content.;
            try
            {
                var i = await httpClient.GetStringAsync(uri);
                var dish = JsonConvert.DeserializeObject<Dish>
                 (await httpClient.GetStringAsync(uri));

                return dish; 

            }
            catch 
            {
                return null;
            }
        }

        public async Task  CreateDish(int userId, Dish dish)
        {
            var uri = $"{remoteServiceBaseUrl}/api/dishes/user/{userId}";

            var dishContent= new StringContent(JsonConvert.SerializeObject(dish), System.Text.Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync(uri,dishContent);
            var r = response.StatusCode;

            var t = response.Content;

            // Task<IActionResult> actionResult =  new Task<IActionResult> ();
        }

        public async Task DeleteDish(int userId,int DishId)
        {
            var uri = $"{remoteServiceBaseUrl}/api/dishes/user/{userId}/{DishId}";


            var response = await httpClient.DeleteAsync(uri);


            // Task<IActionResult> actionResult =  new Task<IActionResult> ();
        }


    }
}
