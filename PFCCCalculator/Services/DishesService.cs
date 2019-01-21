

using Newtonsoft.Json;
using PFCCCalculatorService.Models;
using SharedModels;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace PFCCCalculatorService.Services
{
    public class DishesService : IDishesService
    {
        private readonly HttpClient httpClient;
        private readonly string remoteServiceBaseUrl = "http://localhost:58029";

        public DishesService(/*HttpClient httpClient*/ string url)
        {

            this.httpClient = new HttpClient();

            // this.remoteServiceBaseUrl = remoteServiceBaseUrl;
        }

        public async Task<List<DishModel>> GetDishes()
        {
            var uri = $"{remoteServiceBaseUrl}/api/dishes";
            HttpResponseMessage response = await httpClient.GetAsync(uri);

            try
            {    
                string responseBody = await response.Content.ReadAsStringAsync();

                var dishes = JsonConvert.DeserializeObject<List<DishModel>>(responseBody);

                return dishes;
            }
            catch (HttpRequestException e)
            {
                if (response.StatusCode != System.Net.HttpStatusCode.NotFound)
                    throw e;
            }
            catch (Exception e)
            {
                throw e;
            }

            return null;
        }

        public async Task<PaginatedModel<DishModel>> Items(int pageSize = 0, int pageIndex = 0)
        {
            var uri = $"{remoteServiceBaseUrl}/api/dishes/items?pageSize={pageSize}&pageIndex={pageIndex}";

            string responseString = await httpClient.GetStringAsync(uri);
            //  var brands = JArray.Parse(responseString);


            var dish = JsonConvert.DeserializeObject<PaginatedModel<DishModel>>
                (responseString);
            //   var products = 

            return dish;
        }

        public async Task<List<DishModel>> GetDishesWithProduct(int productId)
        {
            var uri = $"{remoteServiceBaseUrl}/api/dishes/withproduct/{productId}";
            HttpResponseMessage response = await httpClient.GetAsync(uri);

            try
            {
                string responseBody = await response.Content.ReadAsStringAsync();

                var dishes = JsonConvert.DeserializeObject<List<DishModel>>(responseBody);

                return dishes;
            }
            catch (HttpRequestException e)
            {
                if (response.StatusCode != System.Net.HttpStatusCode.NotFound)
                    throw e;
            }
            catch (Exception e)
            {
                throw e;
            }

            return null;

        }

        public async Task<DishModel> GetDishById(int dishId)
        {
            var uri = $"{remoteServiceBaseUrl}/api/dishes/{dishId}";

            HttpResponseMessage response = await httpClient.GetAsync(uri);
            try
            {
                response.EnsureSuccessStatusCode();           
                string responseBody = await response.Content.ReadAsStringAsync();               
                var dish = JsonConvert.DeserializeObject<DishModel>(responseBody);

                return dish; 
            }
            catch (HttpRequestException e)
            {
                if (response.StatusCode != System.Net.HttpStatusCode.NotFound)
                    throw e;                  
            }
            catch (Exception e)
            {
                throw e;
            }

            return null;
        }

        public async Task <DishModel> CreateDish(string userId, DishModel dish)
        {
            var uri = $"{remoteServiceBaseUrl}/api/dishes/user/{userId}";

            try
            {
                var dishContent = new StringContent(JsonConvert.SerializeObject(dish), System.Text.Encoding.UTF8, "application/json");
                var response = await httpClient.PostAsync(uri, dishContent);
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                return  JsonConvert.DeserializeObject<DishModel>(responseBody);
            }
            catch (HttpRequestException e)
            {
                throw e;
            }
        }

        public async Task<DishModel> UpdateDish(string userId, DishModel dish)
        {
            var uri = $"{remoteServiceBaseUrl}/api/dishes/user/{userId}";

            try
            {
                var dishContent = new StringContent(JsonConvert.SerializeObject(dish), System.Text.Encoding.UTF8, "application/json");
                var response = await httpClient.PutAsync(uri, dishContent);
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<DishModel>(responseBody);
            }
            catch (HttpRequestException e)
            {
                throw e;
            }
        }

        public async Task<bool> DeleteDish(string userId,int DishId)
        {
            var uri = $"{remoteServiceBaseUrl}/api/dishes/user/{userId}/{DishId}";
          
            try
            {
                var response = await httpClient.DeleteAsync(uri);

                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                    return false;
                    
                response.EnsureSuccessStatusCode();
            }
            catch (HttpRequestException e)
            {
                throw e;
            }

            return true;
            // Task<IActionResult> actionResult =  new Task<IActionResult> ();
        }


    }
}
