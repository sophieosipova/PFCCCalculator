using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using ProductsService.Database;

namespace Tests.ProductsServiceTests
{
    public class DeleteProductTest
    {
        [Fact]
        public async void OkDeleteProductTest()
        {
            // Arrange
            int productTestId = 1;
            int userTestId = 1;
            var mockRepo = new Mock<IProductRepository>();
            mockRepo.Setup(c => c.DeleteProduct(userTestId, productTestId))
                .ReturnsAsync(ReturnDeleted(userTestId, productTestId));
            var controller = new ProductsService.Controllers.ProductsController(mockRepo.Object);


            // Act
            var result = await controller.DeleteProduct(userTestId, productTestId);


            // Assert
            var requestResult = Assert.IsType<NoContentResult>(result);

        }

        [Fact]
        public async void CantDeleteProductTest()
        {
            // Arrange
            int productTestId = 1;
            int userTestId = 1;
            var mockRepo = new Mock<IProductRepository>();
            mockRepo.Setup(c => c.DeleteProduct(userTestId, productTestId))
                .ReturnsAsync(ReturnCantDelete(userTestId, productTestId));
            var controller = new ProductsService.Controllers.ProductsController(mockRepo.Object);


            // Act
            var result = await controller.DeleteProduct(userTestId, productTestId);


            // Assert
            var requestResult = Assert.IsType<NotFoundResult>(result);
        }

        private bool ReturnCantDelete(int userTestId, int productTestId)
        {
            return false;
        }

        private bool ReturnDeleted(int userTestId, int productTestId)
        {
            return true;
        }
    }
}
