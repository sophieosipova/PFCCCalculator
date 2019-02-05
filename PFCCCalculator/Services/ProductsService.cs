using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

using SharedModels;
using PFCCCalculatorService.Models;
using System.Net.Http.Headers;

namespace PFCCCalculatorService.Services
{
    public class ProductsService: IProductsService
    {
        private readonly HttpClient httpClient;
        private readonly string remoteServiceBaseUrl = "https://localhost:44357";
        private readonly App client = new App() { ClientId = "Gateway", ClientSecret = "Sercret" };
        private AppsToken token;

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

        public async Task<ProductModel> CreateProduct(string userId, ProductModel product)
        {
            var uri = $"{remoteServiceBaseUrl}/api/products/user/{userId}";
            
            try
            {
                var productContent = new StringContent(JsonConvert.SerializeObject(product), System.Text.Encoding.UTF8, "application/json");
                var response = await httpClient.PostAsync(uri, productContent);
                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    await setHeaders();
                    response = await httpClient.PostAsync(uri, productContent);
                }
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<ProductModel>(responseBody);
            }
            catch (Exception e)
            {
                throw e;
            }

            
        }

        public async Task<bool> DeleteProduct(string userId, int productId)
        {
            var uri = $"{remoteServiceBaseUrl}/api/products/user/{userId}/{productId}";

            try
            {
                var response = await httpClient.DeleteAsync(uri);

                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    await setHeaders();
                    response = await httpClient.DeleteAsync(uri);
                }

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
        public async Task<ProductModel> UpdateProduct(string userId, ProductModel productToUpdate)
        {
            var uri = $"{remoteServiceBaseUrl}/api/products/user/{userId}";
            try
            {
                var productContent = new StringContent(JsonConvert.SerializeObject(productToUpdate), System.Text.Encoding.UTF8, "application/json");
                var response = await httpClient.PutAsync(uri, productContent);
                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    await setHeaders();
                    response = await httpClient.PutAsync(uri, productContent);
                }
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

        public  async Task<AppsToken> Login()
        {
            var uri = $"{remoteServiceBaseUrl}/api/autorization";

            try
            {
                var content = new StringContent(JsonConvert.SerializeObject(client), System.Text.Encoding.UTF8, "application/json");
                var response = await httpClient.PostAsync(uri, content);
                response.EnsureSuccessStatusCode();
                string responseBody =  await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<AppsToken>(responseBody);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        private  async Task setHeaders ()
        {
            token = await this.Login();
            httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", $"{token.Token}");
       //     httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token.Token}");
        }

    }
}
