using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using CommentsService.Database;
using CommentsService.Models;

namespace Tests.CommentsServiceTests
{
    public class CreateOrUpdateComment
    {
        [Fact]
        public async void OkCreateCommentTest()
        {
            // Arrange
            // Arrange
            
            int userTestId = 1;
            var comment = GetTestComment(false);
            var mockRepo = new Mock<ICommentsRepository>();
            mockRepo.Setup(c => c.CreateComment(userTestId, comment))
                .ReturnsAsync(GetTestComment(userTestId, comment));
            var controller = new CommentsService.Controllers.CommentsController(mockRepo.Object);


            // Act
            var result = await controller.CreateComment(userTestId, comment);



            // Assert
            var requestResult = Assert.IsType<CreatedResult>(result);
            var model = Assert.IsType<Comment>(requestResult.Value);
            var testmodel = GetTestComment(true);
            Assert.Equal(model, testmodel);
        }

        [Fact]
        public async void CreateNullCommentTest()
        {
            // Arrange
            int userTestId = 1;
            var comment = GetTestComment(false);
            var mockRepo = new Mock<ICommentsRepository>();
            mockRepo.Setup(c => c.CreateComment(userTestId, comment))
                .ReturnsAsync(CreateNullTestComment(userTestId, comment));
            var controller = new CommentsService.Controllers.CommentsController(mockRepo.Object);


            // Act
            var result = await controller.CreateComment(userTestId, comment);

            // Assert
            var requestResult = Assert.IsType<ConflictResult>(result);
        }


        [Fact]
        public async void OkUpdateCommentTest()
        {
            // Arrange
            int userTestId = 1;
            var comment = GetTestComment(true);
            var mockRepo = new Mock<ICommentsRepository>();
            mockRepo.Setup(c => c.UpdateComment(userTestId, comment))
                .ReturnsAsync(GetTestComment(userTestId, comment));
            var controller = new CommentsService.Controllers.CommentsController(mockRepo.Object);


            // Act
            var result = await controller.UpdateComment(userTestId, comment);


            // Assert
            var requestResult = Assert.IsType<CreatedResult>(result);
            var model = Assert.IsType<Comment>(requestResult.Value);
            var testmodel = GetTestComment(true);
            Assert.Equal(model, testmodel);
        }

        [Fact]
        public async void UpdateNullCommentTest()
        {
            // Arrange
            int userTestId = 1;
            var comment = GetTestComment(true);
            var mockRepo = new Mock<ICommentsRepository>();
            mockRepo.Setup(c => c.UpdateComment(userTestId, comment))
                .ReturnsAsync(CreateNullTestComment(userTestId, comment));
            var controller = new CommentsService.Controllers.CommentsController(mockRepo.Object);


            // Act
            var result = await controller.UpdateComment(userTestId, comment);


            // Assert
            var requestResult = Assert.IsType<NotFoundResult>(result);
        }

        private Comment CreateNullTestComment (int userTestId, Comment comment)
        {
            return null;
        }

        private Comment GetTestComment(int userTestId, Comment comment)
        {
            var c = new Comment { UserId = 1, CommentText = "Тест", DishId = 1, CommentId = 1};
            return c;
        }

        private Comment GetTestComment(bool created)
        {
            var comment = new Comment { UserId = 1, CommentText = "Тест", DishId = 1 };
            if (created)
                comment.CommentId = 1;
            return comment;
        }
    }
}
