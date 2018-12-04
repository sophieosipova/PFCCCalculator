using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using ProductsService.Controllers;
using Moq;
using PFCCCalculatorService.Controllers;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using SharedModels;
using ProductsService.Database;
using ProductsService.Models;
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

        private List<Comment> GetTestComments()
        {
            List<Comment> comments = new List<Comment>();

            comments.Add( new Comment { UserId = 1, CommentText = "Тест 1", DishId = 1 });
            comments.Add(new Comment { UserId = 2, CommentText = "Тест 2", DishId = 2 });

            return comments;
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
    }
}
