using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;


using Newtonsoft.Json;
using PFCCCalculatorService.Models;
using SharedModels;

namespace PFCCCalculatorService.Services
{
    public class CommentsService : ICommentsService
    {

        private readonly HttpClient httpClient;
        private readonly string remoteServiceBaseUrl = "http://localhost:49449";

        public CommentsService(/*HttpClient httpClient*/ string url)
        {
            this.httpClient = new HttpClient();
        }

        public async Task<List<CommentModel>> GetCommentsByUserId(int userId)
        {
            var uri = $"{remoteServiceBaseUrl}/api/comments/user/{userId}";

            HttpResponseMessage response = await httpClient.GetAsync(uri);

            try
            {
               response.EnsureSuccessStatusCode();
               string responseBody = await response.Content.ReadAsStringAsync();
               var comments = JsonConvert.DeserializeObject<List<CommentModel>>(responseBody);

               return comments;
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


        public async  Task<PaginatedModel<CommentModel>> GetCommentsByDishId(int dishId, int pageSize = 10, int pageIndex = 0)
        {
            var uri = $"{remoteServiceBaseUrl}/api/comments/dish/{dishId}?pageSize={pageSize}&pageIndex={pageIndex}";

            HttpResponseMessage response = await httpClient.GetAsync(uri);

            try
            {
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                var comments = JsonConvert.DeserializeObject<PaginatedModel<CommentModel>>(responseBody);

                return comments;
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

        public async Task <bool> DeleteComment(int userId, int commentId)
        {
            var uri = $"{remoteServiceBaseUrl}/api/comments/user/{userId}/{commentId}";

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

        public async  Task<CommentModel> CreateComment(int userId, CommentModel comment)
         {
            var uri = $"{remoteServiceBaseUrl}/api/comments/user/{userId}";

            try
            {
                var commentContent = new StringContent(JsonConvert.SerializeObject(comment), System.Text.Encoding.UTF8, "application/json");
                var response = await httpClient.PostAsync(uri, commentContent);
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<CommentModel>(responseBody);
            }
            catch (HttpRequestException e)
            {
                //return null;
                throw e;
            }
        }
    }
}
