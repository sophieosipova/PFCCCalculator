using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using CommentsService.Database;

namespace Tests.CommentsServiceTests
{
    public class DeleteCommentTests
    {
        public class DeleteProductTest
        {
            [Fact]
            public async void OkDeleteCommentTest()
            {

                // Arrange
                int commentTestId = 1;
                int userTestId = 1;

                var mockRepo = new Mock<ICommentsRepository>();
                mockRepo.Setup(c => c.DeleteComment(userTestId, commentTestId))
                    .ReturnsAsync(ReturnDeleted(userTestId, commentTestId));
                var controller = new CommentsService.Controllers.CommentsController(mockRepo.Object);


                // Act
                var result = await controller.DeleteComment(userTestId, commentTestId);


                // Assert
                var requestResult = Assert.IsType<NoContentResult>(result);

            }

            [Fact]
            public async void CantDeleteCommentTest()
            {
                // Arrange
                // Arrange
                int commentTestId = 1;
                int userTestId = 1;

                var mockRepo = new Mock<ICommentsRepository>();
                mockRepo.Setup(c => c.DeleteComment(userTestId, commentTestId))
                    .ReturnsAsync(ReturnCantDelete(userTestId, commentTestId));
                var controller = new CommentsService.Controllers.CommentsController(mockRepo.Object);


                // Act
                var result = await controller.DeleteComment(userTestId, commentTestId);


                // Assert
                var requestResult = Assert.IsType<NotFoundResult>(result);
            }

            [Fact]
            public async void BadRequestDeleteProductTest()
            {
                // Arrange
                // Arrange
                int commentTestId = -1;
                int userTestId = 1;

                var mockRepo = new Mock<ICommentsRepository>();
                mockRepo.Setup(c => c.DeleteComment(userTestId, commentTestId))
                    .ReturnsAsync(ReturnCantDelete(userTestId, commentTestId));
                var controller = new CommentsService.Controllers.CommentsController(mockRepo.Object);


                // Act
                var result = await controller.DeleteComment(userTestId, commentTestId);


                // Assert
                var requestResult = Assert.IsType<NotFoundResult>(result);
            }

            private bool ReturnCantDelete(int userTestId, int productTestId)
            {
                return false;
            }

            private bool ReturnDeleted(int userTestId, int commentTestId)
            {
                return true;
            }
        }
    }
}
