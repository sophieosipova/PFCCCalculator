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
        private readonly string remoteServiceBaseUrl = "https://localhost:44358/api/";

        public AutorizationService(/*HttpClient httpClient*/ string url)
        {
            //    httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token.RefreshToken}");
            HttpClientHandler handler = new HttpClientHandler();
            handler.AllowAutoRedirect = false;
            this.httpClient = new HttpClient(handler);
            httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
        }
        public async Task<UsersToken> Login(User user)
        {
            var uri = $"{remoteServiceBaseUrl}account";
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
            var uri = $"{remoteServiceBaseUrl}account/refreshtokens";

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
            var uri = $"{remoteServiceBaseUrl}account/validate";

            if (accessToken == "")
                return false;
            try
            {
                var requestMessage = new HttpRequestMessage(HttpMethod.Get, uri);
                requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", $"{accessToken.Remove(0, 7)}");
                HttpResponseMessage response = await httpClient.SendAsync(requestMessage);


                response.EnsureSuccessStatusCode();
                return true;
            }
            catch (HttpRequestException e)
            {
                return false;
            }
            catch (Exception e)
            {
                throw e;
            }

         //   return false;

        }

        public async Task<UsersToken> GetToken(string code, string client_secret = "secret", string client_id = "app", string redirect_uri = "https://localhost:44358/api/account")
        {

            var uri = $"{remoteServiceBaseUrl}oauth/token?code={code}&client_secret={client_secret}&client_id={client_id}&redirect_uri={redirect_uri}";
            
            HttpResponseMessage response = await httpClient.GetAsync(uri);

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

        
        public async Task<ActionResult<string>> AuthorizeRequest(string client_id = "app", string redirect_uri = "https://localhost:44358/api/account", string response_type = "code")
        {

            var uri = $"{remoteServiceBaseUrl}oauth?client_id={client_id}&redirect_uri={redirect_uri}&response_type={response_type}";

             HttpResponseMessage response = await httpClient.GetAsync(uri);

          /*  using (HttpClientHandler handler = new HttpClientHandler())
            {
                handler.AllowAutoRedirect = false;
                using (HttpClient httpClient = new HttpClient(handler))
                using (HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Head, uri))
                using (var response = await httpClient.SendAsync(request))
                {
                    var location = response.Headers.Location;
                }
            } */

            try
            {
             //if (response.StatusCode == System.Net.HttpStatusCode.Redirect)
              //   return await response.Content.ReadAsStringAsync();
               // response.EnsureSuccessStatusCode();
                //string responseBody = await response.Content.ReadAsStringAsync();
              var authCode = response.Headers.Location.AbsoluteUri; // JsonConvert.DeserializeObject<string>(responseBody);

                return authCode;
            }
            catch (HttpRequestException e)
            {
                
                    throw e;
            }
            catch (Exception e)
            {
                throw e;
            }

            return null;
        }




    }

}

