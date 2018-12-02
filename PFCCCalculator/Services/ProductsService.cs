using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ProductsService.Models;
using Microsoft.EntityFrameworkCore;
using SharedModels;
using Newtonsoft.Json.Linq;

namespace PFCCCalculatorService.Services
{
    public class ProductsService: IProductsService
    {
        private readonly HttpClient httpClient;
        private readonly string remoteServiceBaseUrl = "http://localhost:52624";

        public ProductsService (HttpClient httpClient)
        {

            this.httpClient = new HttpClient ();
            
           // this.remoteServiceBaseUrl = remoteServiceBaseUrl;
        }


        public async Task<List<Product> > GetProducts()
        {
            var uri = $"{remoteServiceBaseUrl}/api/products";

            HttpResponseMessage response = await httpClient.GetAsync(uri);

            try
            {
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                var products = JsonConvert.DeserializeObject<List<Product>>(responseBody);

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

        public async Task<List<ProductsCategory>> GetProductsCategories()
        {
            var uri = $"{remoteServiceBaseUrl}/api/products/categories";

            HttpResponseMessage response = await httpClient.GetAsync(uri);

            try
            {
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                var categories = JsonConvert.DeserializeObject<List<ProductsCategory>>(responseBody);

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


        public async Task<List<Product>> GetProductsByCategoryId(int productCategoryId)
        {
            var uri = $"{remoteServiceBaseUrl}/api/products/categories/{productCategoryId}";

            HttpResponseMessage response = await httpClient.GetAsync(uri);

            try
            {
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                var products = JsonConvert.DeserializeObject<List<Product>>(responseBody);

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
        public async Task<Product> GetProductById (int productId)
        {
            var uri = $"{remoteServiceBaseUrl}/api/products/{productId}";

            HttpResponseMessage response = await httpClient.GetAsync(uri);

            try
            {
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                var product = JsonConvert.DeserializeObject<Product>(responseBody);

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

        public async Task<bool> CreateProduct(int userId, Product product)
        {
            var uri = $"{remoteServiceBaseUrl}/api/products/user/{userId}";
            
            try
            {
                var productContent = new StringContent(JsonConvert.SerializeObject(product), System.Text.Encoding.UTF8, "application/json");
                var response = await httpClient.PostAsync(uri, productContent);
                response.EnsureSuccessStatusCode();
            }
            catch (Exception e)
            {
                throw e;
            }

            return true;
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
        public async Task<bool> UpdateProduct(int userId, Product productToUpdate)
        {
            var uri = $"{remoteServiceBaseUrl}/api/products/user/{userId}";
            try
            {
                var productContent = new StringContent(JsonConvert.SerializeObject(productToUpdate), System.Text.Encoding.UTF8, "application/json");
                var response = await httpClient.PutAsync(uri, productContent);
                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                    return false;

                response.EnsureSuccessStatusCode();
            }
            catch (Exception e)
            {
                throw e;
            }

            return true;
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

        public async Task<PaginatedModel<Product>> Items(int pageSize = 10, int pageIndex = 0)
        {
            var uri = $"{remoteServiceBaseUrl}/api/products/items?pageSize={pageSize}&pageIndex={pageIndex}";

            string responseString = await httpClient.GetStringAsync(uri);
          //  var brands = JArray.Parse(responseString);


            var product = JsonConvert.DeserializeObject<PaginatedModel<Product>>
                (responseString);
            //   var products = 

            return product;
        }
           
    }
}
