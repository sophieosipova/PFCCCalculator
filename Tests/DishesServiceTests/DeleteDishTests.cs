using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using DishesService.Database;

namespace Tests.DishesServiceTests
{
    public class DeleteDishTests
    {
        [Fact]
        public async void OkDeleteProductTest()
        {
            // Arrange
            int dishTestId = 1;
            int userTestId = 1;
            var mockRepo = new Mock<IDishesRepository>();
            mockRepo.Setup(c => c.DeleteDish(userTestId,dishTestId))
                .ReturnsAsync(ReturnDeleted);
            var controller = new DishesService.Controllers.DishesController(mockRepo.Object);

            // Act
            var result = await controller.DeleteDish(userTestId, dishTestId);


            // Assert
            var requestResult = Assert.IsType<NoContentResult>(result);

        }

        [Fact]
        public async void CantDeleteProductTest()
        {
            // Arrange
            int dishTestId = 1;
            int userTestId = 1;
            var mockRepo = new Mock<IDishesRepository>();
            mockRepo.Setup(c => c.DeleteDish(userTestId, dishTestId))
                .ReturnsAsync(ReturnCantDelete);
            var controller = new DishesService.Controllers.DishesController(mockRepo.Object);

            // Act
            var result = await controller.DeleteDish(userTestId, dishTestId);


            // Assert
            var requestResult = Assert.IsType<NotFoundResult>(result);
        }

        private bool ReturnCantDelete()
        {
            return false;
        }

        private bool ReturnDeleted()
        {
            return true;
        }
    }
}
