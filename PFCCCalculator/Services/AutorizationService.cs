using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SharedModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace PFCCCalculatorService.Services
{
    public class AutorizationService : IAutorizationService
    {
        private readonly HttpClient httpClient;
        private readonly string remoteServiceBaseUrl = "https://localhost:44358/api/account";

        public AutorizationService(/*HttpClient httpClient*/ string url)
        {
        //    httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token.RefreshToken}");
            
            this.httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
        }
        public async Task<UsersToken> Login(User user)
        {
            var uri = $"{remoteServiceBaseUrl}";
            var userContent = new StringContent(JsonConvert.SerializeObject(user), System.Text.Encoding.UTF8, "application/json");
            HttpResponseMessage response = await httpClient.PostAsync(uri, userContent);

            try
            {
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                var usersToken = JsonConvert.DeserializeObject<UsersToken>(responseBody);

                return usersToken;
            }
            catch (HttpRequestException e)
            {
                if (response.StatusCode != System.Net.HttpStatusCode.Unauthorized)
                    throw e;
            }
            catch (Exception e)
            {
                throw e;
            }

            return null;
        }
        public async Task<UsersToken> RefreshTokens(UsersToken token)
        {
            var uri = $"{remoteServiceBaseUrl}/refreshtokens";

            //   httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token.RefreshToken}");
            //   HttpResponseMessage response = await httpClient.GetAsync(uri,);
            var requestMessage = new HttpRequestMessage(HttpMethod.Get, uri);           
            requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token.RefreshToken);
             HttpResponseMessage response = await httpClient.SendAsync(requestMessage);
            
            try
            {
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                var usersToken = JsonConvert.DeserializeObject<UsersToken>(responseBody);

                return usersToken;
            }
            catch (HttpRequestException e)
            {
                if (response.StatusCode != System.Net.HttpStatusCode.Unauthorized)
                    throw e;
            }
            catch (Exception e)
            {
                throw e;
            }

            return null;

        }

        public async Task<bool> ValidateToken(string accessToken)
        {
            var uri = $"{remoteServiceBaseUrl}/validate";

            //   httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token.RefreshToken}");
            //   HttpResponseMessage response = await httpClient.GetAsync(uri,);
            var requestMessage = new HttpRequestMessage(HttpMethod.Get, uri);
            requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", $"{accessToken.Remove(0,7)}");
            HttpResponseMessage response = await httpClient.SendAsync(requestMessage);

            try
            {
                response.EnsureSuccessStatusCode();
                return true;
            }
            catch (HttpRequestException e)
            {
                if (response.StatusCode != System.Net.HttpStatusCode.Unauthorized)
                    throw e;
            }
            catch (Exception e)
            {
                throw e;
            }

            return false;

        }
    }


   
}

