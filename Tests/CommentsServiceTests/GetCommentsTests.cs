using System.Collections.Generic;
using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using SharedModels;
using CommentsService.Database;
using CommentsService.Models;

namespace Tests.CommentsServiceTests
{
    public class GetCommentsTests
    {

    //    Task<Comment> GetCommentById(int commentId);
    //    Task<List<Comment>> GetCommentsByUserId(int userId);

        [Fact]
        public async void GetGetCommentByIdTest()
        {
            // Arrange
            int commentTestID = 1;
            var mockRepo = new Mock<ICommentsRepository>();
            mockRepo.Setup(c => c.GetCommentById(commentTestID))
                .ReturnsAsync(GetTestComment(commentTestID));
            var controller = new CommentsService.Controllers.CommentsController(mockRepo.Object);


            // Act
            var result = await controller.GetCommentById(commentTestID);


            // Assert
            var requestResult = Assert.IsType<OkObjectResult>(result);
            var model = Assert.IsType<Comment>(requestResult.Value);
            var testmodel = GetTestComment(commentTestID);
            Assert.Equal(model, testmodel);

        }

        [Fact]
        public async void GetNullCommentByIdTest()
        {
            // Arrange
            int commentTestId= 1;
            var mockRepo = new Mock<ICommentsRepository>();
            mockRepo.Setup(c => c.GetCommentById(commentTestId))
                .ReturnsAsync(GetNullCommentTest(commentTestId));
            var controller = new CommentsService.Controllers.CommentsController(mockRepo.Object);


            // Act
            var result = await controller.GetCommentById(commentTestId);


            // Assert
            Assert.IsType<NotFoundResult>(result);
        }


        [Fact]
        public async void GetBadRequestCommentByIdTest()
        {
            // Arrange
            int commentTestId = -1;
            var mockRepo = new Mock<ICommentsRepository>();
            mockRepo.Setup(c => c.GetCommentById(commentTestId))
                .ReturnsAsync(GetNullCommentTest(commentTestId));
            var controller = new CommentsService.Controllers.CommentsController(mockRepo.Object);


            // Act
            var result = await controller.GetCommentById(commentTestId);


            // Assert
            Assert.IsType<BadRequestResult>(result);
        }


        [Fact]
        public async void GetCommentsByDishIdTest()
        {
            // Arrange
            int dishTestID = 1;
            int pageSize = 1;
            int pageIndex = 0;
            var mockRepo = new Mock<ICommentsRepository>();
            mockRepo.Setup(c => c.GetCommentsByDishId(dishTestID,pageSize,pageIndex))
                .ReturnsAsync(GetTestPaginatedComments(dishTestID, pageSize, pageIndex));
            var controller = new CommentsService.Controllers.CommentsController(mockRepo.Object);


            // Act
            var result = await controller.GetCommentsByDishId(dishTestID, pageSize, pageIndex);


            // Assert
            var requestResult = Assert.IsType<OkObjectResult>(result);
            var model = Assert.IsType<PaginatedModel<Comment>>(requestResult.Value);
            var testmodel = GetTestPaginatedComments(dishTestID, pageSize, pageIndex);
            Assert.Equal(model.Count, testmodel.Count);
            Assert.Equal(model.Data, testmodel.Data);
            Assert.Equal(model.PageIndex, testmodel.PageIndex);
            Assert.Equal(model.PageSize, testmodel.PageSize);

        }

        [Fact]
        public async void GetNullCommentByDishIdTest()
        {
            // Arrange
            int dishTestID = 1;
            int pageSize = 1;
            int pageIndex = 0;
            var mockRepo = new Mock<ICommentsRepository>();
            mockRepo.Setup(c => c.GetCommentsByDishId(dishTestID, pageSize, pageIndex))
                .ReturnsAsync(GetNullPaginatedCommentTest(dishTestID, pageSize, pageIndex));
            var controller = new CommentsService.Controllers.CommentsController(mockRepo.Object);


            // Act
            var result = await controller.GetCommentsByDishId(dishTestID, pageSize, pageIndex);


            // Assert
            Assert.IsType<NotFoundResult>(result);
        }


        [Fact]
        public async void GetBadRequestCommentsByDishIdTest()
        {
            // Arrange
            int dishTestID = -1;
            int pageSize = 1;
            int pageIndex = 0;
            var mockRepo = new Mock<ICommentsRepository>();
            mockRepo.Setup(c => c.GetCommentsByDishId(dishTestID, pageSize, pageIndex))
                .ReturnsAsync(GetNullPaginatedCommentTest(dishTestID, pageSize, pageIndex));
            var controller = new CommentsService.Controllers.CommentsController(mockRepo.Object);


            // Act
            var result = await controller.GetCommentsByDishId(dishTestID, pageSize, pageIndex);


            // Assert
            Assert.IsType<BadRequestResult>(result);
        }

        private PaginatedModel<Comment>  GetTestPaginatedComments(int dishTestID, int pageSize, int pageIndex)
        {
            List<Comment> comments = new List<Comment>();

            comments.Add( new Comment { UserId = 1, CommentText = "Тест 1", DishId = 1 });
            comments.Add(new Comment { UserId = 2, CommentText = "Тест 2", DishId = 2 });


            PaginatedModel<Comment> paginatedModel = new PaginatedModel<Comment>
                (0, 1, 2, comments);
            return paginatedModel;
        }


        private Comment GetTestComment(int commentID)
        {
            var comment = new Comment { UserId = 1, CommentText = "Тест", DishId = 1 };

            return comment;
        }

        private Comment GetNullCommentTest(int commentID)
        {
            return null;
        }

        private PaginatedModel<Comment> GetNullPaginatedCommentTest(int dishTestID, int pageSize, int pageIndex)
        {
            return null;
        }
    }
}
