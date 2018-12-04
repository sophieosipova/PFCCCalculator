using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using ProductsService.Database;
using ProductsService.Models;

namespace Tests.ProductsServiceTests
{
    public class CreateAndUpdateProductTest
    {
            [Fact]
            public async void OkCreateProductTest()
            {
                // Arrange
                int userTestId = 1;
                var product = GetTestProduct(false);
                var mockRepo = new Mock<IProductRepository>();
                mockRepo.Setup(c => c.CreateProduct(userTestId, product)) 
                    .ReturnsAsync(CreateProductTest(userTestId, product));
                var controller = new ProductsService.Controllers.ProductsController(mockRepo.Object);


                // Act
                var result = await controller.CreateProduct(userTestId, product);


                // Assert
                var requestResult = Assert.IsType<CreatedResult>(result);
                var model = Assert.IsType<Product>(requestResult.Value);
                var testmodel = GetTestProduct (true);
                Assert.Equal(model, testmodel);
        }

            [Fact]
            public async void CreateNullProductsTest()
            {
            // Arrange
             int userTestId = 1;
             var product = GetTestProduct(false);
             var mockRepo = new Mock<IProductRepository>();
             mockRepo.Setup(c => c.CreateProduct(userTestId, product))
                .ReturnsAsync(CreateNullProductTest(userTestId, product));
             var controller = new ProductsService.Controllers.ProductsController(mockRepo.Object);


            // Act
            var result = await controller.CreateProduct(userTestId, product);


                // Assert
             var requestResult = Assert.IsType<ConflictResult>(result);
            }


        [Fact]
        public async void OkUpdateProductTest()
        {
            // Arrange
            int userTestId = 1;
            var product = GetTestProduct(true);
            var mockRepo = new Mock<IProductRepository>();
            mockRepo.Setup(c => c.UpdateProduct(userTestId, product))
                .ReturnsAsync(CreateProductTest(userTestId, product));
            var controller = new ProductsService.Controllers.ProductsController(mockRepo.Object);


            // Act
            var result = await controller.UpdateProduct(userTestId, product);


            // Assert
            var requestResult = Assert.IsType<CreatedResult>(result);
            var model = Assert.IsType<Product>(requestResult.Value);
            var testmodel = GetTestProduct(true);
            Assert.Equal(model, testmodel);
        }

        [Fact]
        public async void UpdateNullProductTest()
        {
            // Arrange
            int userTestId = 1;
            var product = GetTestProduct(true);
            var mockRepo = new Mock<IProductRepository>();
            mockRepo.Setup(c => c.UpdateProduct(userTestId, product))
                .ReturnsAsync(CreateNullProductTest(userTestId, product));
            var controller = new ProductsService.Controllers.ProductsController(mockRepo.Object);


            // Act
            var result = await controller.UpdateProduct(userTestId, product);


            // Assert
            var requestResult = Assert.IsType<NotFoundResult>(result);
        }

        private Product CreateNullProductTest(int userTestId, Product product)
        {
            return null;
        }


        private Product CreateProductTest(int userTestId, Product product)
        {
            return new Product
            {
                ProductId = 1,
                ProductName = "Тест 1",
                ProductsCategoryId = 1,
                Protein = 1,
                Fat = 1,
                Carbohydrates = 1,
                Calories = 20,
                UserId = 1
            };
        }

        private Product GetTestProduct (bool created)
        {

            var product =  new Product
            {
                ProductName = "Тест 1",
                ProductsCategoryId = 1,
                Protein = 1,
                Fat = 1,
                Carbohydrates = 1,
                Calories = 20,
                UserId = 1
            };

            if (created)
                product.ProductId = 1;

            return product;
        }
    }
}
