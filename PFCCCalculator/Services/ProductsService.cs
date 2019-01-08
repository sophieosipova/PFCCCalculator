using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

using SharedModels;
using PFCCCalculatorService.Models;

namespace PFCCCalculatorService.Services
{
    public class ProductsService: IProductsService
    {
        private readonly HttpClient httpClient;
        private readonly string remoteServiceBaseUrl = "http://localhost:52624";

        public ProductsService (/*HttpClient httpClient*/)
        {

            this.httpClient = new HttpClient ();
            
           // this.remoteServiceBaseUrl = remoteServiceBaseUrl;
        }


        public async Task<List<ProductModel> > GetProducts()
        {
            var uri = $"{remoteServiceBaseUrl}/api/products";

            HttpResponseMessage response = await httpClient.GetAsync(uri);

            try
            {
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                var products = JsonConvert.DeserializeObject<List<ProductModel>>(responseBody);

                return products;
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

        public async Task<List<ProductsCategoryModel>> GetProductsCategories()
        {
            var uri = $"{remoteServiceBaseUrl}/api/products/categories";

            HttpResponseMessage response = await httpClient.GetAsync(uri);

            try
            {
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                var categories = JsonConvert.DeserializeObject<List<ProductsCategoryModel>>(responseBody);

                return categories;
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


        public async Task<List<ProductModel>> GetProductsByCategoryId(int productCategoryId)
        {
            var uri = $"{remoteServiceBaseUrl}/api/products/categories/{productCategoryId}";

            HttpResponseMessage response = await httpClient.GetAsync(uri);

            try
            {
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                var products = JsonConvert.DeserializeObject<List<ProductModel>>(responseBody);

                return products;
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
        public async Task<ProductModel> GetProductById (int productId)
        {
            var uri = $"{remoteServiceBaseUrl}/api/products/{productId}";

            HttpResponseMessage response = await httpClient.GetAsync(uri);

            try
            {
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                var product = JsonConvert.DeserializeObject<ProductModel>(responseBody);

                return product;
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

        public async Task<ProductModel> CreateProduct(int userId, ProductModel product)
        {
            var uri = $"{remoteServiceBaseUrl}/api/products/user/{userId}";
            
            try
            {
                var productContent = new StringContent(JsonConvert.SerializeObject(product), System.Text.Encoding.UTF8, "application/json");
                var response = await httpClient.PostAsync(uri, productContent);
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<ProductModel>(responseBody);
            }
            catch (Exception e)
            {
                throw e;
            }

            
        }

        public async Task<bool> DeleteProduct(int userId, int productId)
        {
            var uri = $"{remoteServiceBaseUrl}/api/products/user/{userId}/{productId}";

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

        }
        public async Task<ProductModel> UpdateProduct(int userId, ProductModel productToUpdate)
        {
            var uri = $"{remoteServiceBaseUrl}/api/products/user/{userId}";
            try
            {
                var productContent = new StringContent(JsonConvert.SerializeObject(productToUpdate), System.Text.Encoding.UTF8, "application/json");
                var response = await httpClient.PutAsync(uri, productContent);
   
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<ProductModel>(responseBody);
            }
            catch (Exception e)
            {
                throw e;
            }

        }
        public  void Dispose()

        {
             httpClient.Dispose();
        }

        /*   Task<List<ProductsCategory>> GetProductsCategories();

           Task<Product> GetProductsByCategoryId(int productCategoryId);

           Task<Product> GetProductById(int productId);

           [ProducesResponseType((int)HttpStatusCode.Created)]
           Task<IActionResult> CreateProduct([FromBody]Product product);

           [ProducesResponseType((int)HttpStatusCode.NoContent)]
           Task<IActionResult> DeleteProduct(int productId);

           [ProducesResponseType((int)HttpStatusCode.NotFound)]
           [ProducesResponseType((int)HttpStatusCode.Created)]
           Task<IActionResult> UpdateProduct([FromBody]Product productToUpdate);
           */

        public async Task<PaginatedModel<ProductModel>> Items(int pageSize = 0, int pageIndex = 0)
        {
            var uri = $"{remoteServiceBaseUrl}/api/products/items?pageSize={pageSize}&pageIndex={pageIndex}";

            string responseString = await httpClient.GetStringAsync(uri);
          //  var brands = JArray.Parse(responseString);


            var product = JsonConvert.DeserializeObject<PaginatedModel<ProductModel>>
                (responseString);
            //   var products = 

            return product;
        }
           
    }
}
