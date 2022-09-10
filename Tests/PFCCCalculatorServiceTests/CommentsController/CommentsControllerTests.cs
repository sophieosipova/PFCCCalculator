using Moq;
using Xunit;
using PFCCCalculatorService.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using System.Net;
using System.Net.Http;
using PFCCCalculator;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using PFCCCalculatorService.Models;
using System;
using SharedModels;
using Newtonsoft.Json;

namespace Tests.PFCCCalculatorServiceTests.CommentsController
{
    public class CommentsControllerTests
    {
        public Mock<IProductsService> ProductsServiceMock = new Mock<IProductsService>();
        public Mock<IDishesService> DishesServiceMock = new Mock<IDishesService>();
        public Mock<ICommentsService> CommentsServiceMock = new Mock<ICommentsService>();
        //    public Mock<IGateway> CommentsServiceMock = new Mock<ICommentsService>();
        private TestServer _server;
        public HttpClient Client { get; set; }

        public CommentsControllerTests()
        {
            _server = new TestServer(new WebHostBuilder().UseStartup<Startup>().ConfigureTestServices(services =>

            {
                services.AddSingleton<IProductsService>(ProductsServiceMock.Object);
                services.AddSingleton<IDishesService>(DishesServiceMock.Object);
                services.AddSingleton<ICommentsService>(CommentsServiceMock.Object);


            }));

            Client = _server.CreateClient();
        }




        [Fact]
        public async void GetCommentsByDishIdTest()
        {
            // Arrange
            int dishTestID = 1;
            int pageSize = 1;
            int pageIndex = 0;

            CommentsServiceMock.Setup(c => c.GetCommentsByDishId(dishTestID, pageSize, pageIndex))
             .ReturnsAsync(GetTestPaginatedComments);

            // Act
            var result = await Client.GetAsync($"/api/comments/dish/{dishTestID}?pageSize={pageSize}&pageIndex={pageIndex}");

            // Assert        
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
            var testmodel = GetTestPaginatedComments();
            var model = Assert.IsType<PaginatedModel<CommentModel>>(JsonConvert.DeserializeObject<PaginatedModel<CommentModel>>(await result.Content.ReadAsStringAsync()));
            

            Assert.Equal(model.Count, testmodel.Count);
            Assert.Equal(model.Data, testmodel.Data);
            Assert.Equal(model.PageIndex, testmodel.PageIndex);
            Assert.Equal(model.PageSize, testmodel.PageSize);
            CommentsServiceMock.Reset();

        }


        private CommentModel GetTestComment(bool created)
        {
            var comment = new CommentModel { UserId = 1, CommentText = "Тест", DishId = 1 };
            if (created)
                comment.CommentId = 1;
            return comment;
        }


        private CommentModel GetTestComment()
        {
            var comment = new CommentModel { UserId = 1, CommentText = "Тест", DishId = 1 };
           
            comment.CommentId = 1;
            return comment;
        }

        [Fact]
        public async void GetBadRequestCommentsByDishIdTest()
        {
            // Arrange
            int dishTestID = -1;
            int pageSize = 1;
            int pageIndex = 0;

            CommentsServiceMock.Setup(c => c.GetCommentsByDishId(dishTestID, pageSize, pageIndex))
             .ReturnsAsync(GetTestPaginatedComments);



            // Act
          var result = await Client.GetAsync($"/api/comments/dish/{dishTestID}?pageSize={pageSize}&pageIndex={pageIndex}");


            // Assert

            Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
            CommentsServiceMock.Reset();

        }

        [Fact]
        public async void GetNullCommentByDishIdTest()
        {
                 // Arrange
                  int dishTestID = 1;
                  int pageSize = 1;
                  int pageIndex = 0;

                  CommentsServiceMock.Setup(c => c.GetCommentsByDishId(dishTestID, pageSize, pageIndex))
                   .ReturnsAsync(GetNullPaginatedCommentTest);


            // Act
            var result = await Client.GetAsync($"/api/comments/dish/{dishTestID}?pageSize={pageSize}&pageIndex={pageIndex}");


            // Assert
            Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);
            CommentsServiceMock.Reset();
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async void OkDeleteCommentTest(bool deleted)
        {

            // Arrange
            int commentTestId = 1;
            int userTestId = 1;


             CommentsServiceMock.Setup(c => c.DeleteComment(userTestId, commentTestId))
                   .ReturnsAsync(deleted);

            // Act

            var result = await Client.DeleteAsync($"/api/comments/user/{userTestId}/{commentTestId}");

            // Assert
            if (deleted)
                Assert.Equal(HttpStatusCode.NoContent, result.StatusCode);
            else
                Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);
            CommentsServiceMock.Reset();
        }

   
        private bool ReturnCantDelete()
        {
            return false;
        }

        private bool ReturnDeleted()
        {
            return true;
        }

        private PaginatedModel<CommentModel> GetTestPaginatedComments()
        {
            List<CommentModel> comments = new List<CommentModel>();

            comments.Add(new CommentModel { UserId = 1, CommentText = "Тест 1", DishId = 1 });
            comments.Add(new CommentModel { UserId = 2, CommentText = "Тест 2", DishId = 2 });


            PaginatedModel<CommentModel> paginatedModel = new PaginatedModel<CommentModel>
                (0, 1, 2, comments);
            return paginatedModel;
        }




        private CommentModel GetNullCommentTest(int commentID)
        {
            return null;
        }

        private PaginatedModel<CommentModel> GetNullPaginatedCommentTest()
        {
            return null;
        }


    }
}
