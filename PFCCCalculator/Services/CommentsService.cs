using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

using CommentsService.Models;
using Newtonsoft.Json;
using SharedModels;

namespace PFCCCalculatorService.Services
{
    public class CommentsService : ICommentsService
    {

        private readonly HttpClient httpClient;
        private readonly string remoteServiceBaseUrl = "http://localhost:49449";

        public CommentsService(HttpClient httpClient)
        {

            this.httpClient = new HttpClient();

            // this.remoteServiceBaseUrl = remoteServiceBaseUrl;
        }

        public async Task<List<Comment>> GetCommentsByUserId(int userId)
        {
            var uri = $"{remoteServiceBaseUrl}/api/comments/user/{userId}";

            var comments = JsonConvert.DeserializeObject<List<Comment>>(await httpClient.GetStringAsync(uri));
            return comments;
        }


        public async  Task<PaginatedModel<Comment>> GetCommentsByDishId(int dishId, int pageSize = 10, int pageIndex = 0)
        {
            var uri = $"{remoteServiceBaseUrl}/api/comments/dish/{dishId}?pageSize={pageSize}&pageIndex={pageIndex}";

            string responseString = await httpClient.GetStringAsync(uri);

            var comments = JsonConvert.DeserializeObject<PaginatedModel<Comment>>
                (responseString);

            return comments;
        }

        public async Task DeleteComment(int userId, int commentId)
        {
            var uri = $"{remoteServiceBaseUrl}/api/comments/user/{userId}/{commentId}";


            var response = await httpClient.DeleteAsync(uri);

            // Task<IActionResult> actionResult =  new Task<IActionResult> ();
        }
    }
}
