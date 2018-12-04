using Moq;
using PFCCCalculatorService.Controllers;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using SharedModels;
using Xunit;
using PFCCCalculatorService.Services;
using System.Net;

namespace Tests
{
    public class PFCCControllerTestDeleteDish
    {
        [Fact]
        public async void NotFoundDeleteRecipeTest()
        {
            // Arrange
            int dishId = 1;
            int userId = 1;
            var mockRepo = new Mock<IGatewayService>();
            mockRepo.Setup(c => c.DeleteDish(userId,dishId))
                .ReturnsAsync(ReturnCantDelete);
            var controller = new PFCCCalculatorController(mockRepo.Object);


            // Act
            var result = await controller.DeleteRecipe(userId, dishId);


            // Assert
            var requestResult = Assert.IsType<NotFoundResult>(result);

        }

        [Fact]
        public async void OkDeleteRecipeTest()
        {
            // Arrange
            int dishId = 1;
            int userId = 1;
            var mockRepo = new Mock<IGatewayService>();
            mockRepo.Setup(c => c.DeleteDish(userId, dishId))
                .ReturnsAsync(ReturnDeleted);
            var controller = new PFCCCalculatorController(mockRepo.Object);


            // Act
            var result = await controller.DeleteRecipe(userId, dishId);


            // Assert
            var requestResult = Assert.IsType<NoContentResult>(result);

        }

        [Fact]
        public async void ExceptionDeleteRecipeTest()
        {
            // Arrange
            int dishId = 1;
            int userId = 1;
            var mockRepo = new Mock<IGatewayService>();
            mockRepo.Setup(c => c.DeleteDish(userId, dishId))
                .ReturnsAsync(ServerError);
            var controller = new PFCCCalculatorController(mockRepo.Object);


            // Act
            var result = await controller.DeleteRecipe(userId, dishId);


            // Assert
            var requestResult = Assert.IsType<ConflictObjectResult>(result);
            Assert.Equal(requestResult.Value, message);

        }
        private bool ReturnCantDelete()
        {
            return false;
        }

        private bool ReturnDeleted()
        {
            return true;
        }

        private bool ServerError()
        {
            throw new System.Exception(message);
        }

        private string message = "Test exception";
    }
}