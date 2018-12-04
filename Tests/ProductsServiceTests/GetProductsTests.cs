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

namespace Tests.ProductsServiceTests
{
    public class GetProductsTests
    {
        [Fact]
        public async void GetProductsListTest()
        {
            // Arrange
            var mockRepo = new Mock<IProductRepository>();
            mockRepo.Setup(c => c.GetProducts())
                .ReturnsAsync(GetTestProducts);
            var controller = new ProductsService.Controllers.ProductsController(mockRepo.Object);


            // Act
            var result = await controller.GetProducts();


            // Assert
            var requestResult = Assert.IsType<OkObjectResult>(result);
            var model = Assert.IsType<List<Product>>(requestResult.Value);
            var testmodel = GetTestProducts();
            Assert.Equal(model, testmodel);

        }

        [Fact]
        public async void GetNullProductsListTest()
        {
            // Arrange
            var mockRepo = new Mock<IProductRepository>();
            mockRepo.Setup(c => c.GetProducts())
                .ReturnsAsync(GetNullTest);
            var controller = new ProductsService.Controllers.ProductsController(mockRepo.Object);


            // Act
            var result = await controller.GetProducts();


            // Assert
            Assert.IsType<NotFoundResult>(result);

        }


        [Fact]
        public async void GetNullUsersProductsTest()
        {
            // Arrange
            int userTestId = 1;
            var mockRepo = new Mock<IProductRepository>();
            mockRepo.Setup(c => c.GetUsersProducts(userTestId))
                .ReturnsAsync(GetNullTest);
            var controller = new ProductsService.Controllers.ProductsController(mockRepo.Object);


            // Act
            var result = await controller.GetUsersProducts(userTestId);


            // Assert
            Assert.IsType<NotFoundResult>(result);

        }

        [Fact]
        public async void GetRequestUsersProductsTest()
        {
            // Arrange
            int userTestId = -1;
            var mockRepo = new Mock<IProductRepository>();
            mockRepo.Setup(c => c.GetUsersProducts(userTestId))
                .ReturnsAsync(GetNullTest);
            var controller = new ProductsService.Controllers.ProductsController(mockRepo.Object);


            // Act
            var result = await controller.GetUsersProducts(userTestId);


            // Assert
            Assert.IsType<BadRequestResult>(result);

        }

        [Fact]
        public async void GetUsersProductsListTest()
        {
            // Arrange
            int userTestId = 1;
            var mockRepo = new Mock<IProductRepository>();
            mockRepo.Setup(c => c.GetUsersProducts(userTestId))
                .ReturnsAsync(GetTestUsersProducts(userTestId));
            var controller = new ProductsService.Controllers.ProductsController(mockRepo.Object);


            // Act
            var result = await controller.GetUsersProducts(userTestId);


            // Assert
            var requestResult = Assert.IsType<OkObjectResult>(result);
            var model = Assert.IsType<List<Product>>(requestResult.Value);
            var testmodel = GetTestUsersProducts(userTestId);
            Assert.True(productsEquals(model, testmodel));

        }

        [Fact]
        public async void GetNullProductsByCategoryIdTest()
        {
            // Arrange
            int categoryTestId = 1;
            var mockRepo = new Mock<IProductRepository>();
            mockRepo.Setup(c => c.GetProductsByCategoryId(categoryTestId))
                .ReturnsAsync(GetNullTest);
            var controller = new ProductsService.Controllers.ProductsController(mockRepo.Object);


            // Act
            var result = await controller.GetProductsByCategoryId(categoryTestId);


            // Assert
            Assert.IsType<NotFoundResult>(result);
        }


        [Fact]
        public async void GetBadRequestProductsByCategoryIdTest()
        {
            // Arrange
            int categoryTestId = -1;
            var mockRepo = new Mock<IProductRepository>();
            mockRepo.Setup(c => c.GetProductsByCategoryId(categoryTestId))
                .ReturnsAsync(GetNullTest);
            var controller = new ProductsService.Controllers.ProductsController(mockRepo.Object);


            // Act
            var result = await controller.GetProductsByCategoryId(categoryTestId);


            // Assert
            Assert.IsType<BadRequestResult>(result);
        }


        [Fact]
        public async void GetProductsByCategoryIdTest()
        {
            // Arrange
            int categoryTestId = 1;
            var mockRepo = new Mock<IProductRepository>();
            mockRepo.Setup(c => c.GetProductsByCategoryId(categoryTestId))
                .ReturnsAsync(GetNullTest);
            var controller = new ProductsService.Controllers.ProductsController(mockRepo.Object);


            // Act
            var result = await controller.GetProductsByCategoryId(categoryTestId);


            // Assert
            Assert.IsType<NotFoundResult>(result);

        }


        [Fact]
        public async void GetNullProductsCategories()
        {
            // Arrange
            var mockRepo = new Mock<IProductRepository>();
            mockRepo.Setup(c => c.GetProductsCategories())
                .ReturnsAsync(GetNullProductsCategoriesTest);
            var controller = new ProductsService.Controllers.ProductsController(mockRepo.Object);


            // Act
            var result = await controller.GetProductsCategories();


            // Assert
            Assert.IsType<NotFoundResult>(result);
        }


        [Fact]
        public async void GetProductsCategoriesTest()
        {
            // Arrange
            var mockRepo = new Mock<IProductRepository>();
            mockRepo.Setup(c => c.GetProductsCategories())
                .ReturnsAsync(GetTestProductsCategories);
            var controller = new ProductsService.Controllers.ProductsController(mockRepo.Object);


            // Act
            var result = await controller.GetProductsCategories();


            // Assert
            var requestResult = Assert.IsType<OkObjectResult>(result);
            var model = Assert.IsType<List<ProductsCategory>>(requestResult.Value);
            var testmodel = GetTestProductsCategories();
            Assert.Equal(model, testmodel);

        }



        [Fact]
        public async void GetNullProductByIdTest()
        {
            // Arrange
            int productTestId = 1;
            var mockRepo = new Mock<IProductRepository>();
            mockRepo.Setup(c => c.GetProductById(productTestId))
                .ReturnsAsync(GetNullProductTest(productTestId));
            var controller = new ProductsService.Controllers.ProductsController(mockRepo.Object);


            // Act
            var result = await controller.GetProductsByCategoryId(productTestId);


            // Assert
            Assert.IsType<NotFoundResult>(result);
        }


        [Fact]
        public async void GetBadRequestProductByIdTest()
        {
            // Arrange
            int productTestId = -1;
            var mockRepo = new Mock<IProductRepository>();
            mockRepo.Setup(c => c.GetProductById(productTestId))
                .ReturnsAsync(GetNullProductTest(productTestId));
            var controller = new ProductsService.Controllers.ProductsController(mockRepo.Object);


            // Act
            var result = await controller.GetProductById(productTestId);


            // Assert
            Assert.IsType<BadRequestResult>(result);
        }


        [Fact]
        public async void GetProductByIdTest()
        {
            // Arrange
            int productTestId = 1;
            var mockRepo = new Mock<IProductRepository>();
            mockRepo.Setup(c => c.GetProductById(productTestId))
                .ReturnsAsync(GetProductTest(productTestId));
            var controller = new ProductsService.Controllers.ProductsController(mockRepo.Object);


            // Act
            var result = await controller.GetProductById(productTestId);


            // Assert
            var requestResult = Assert.IsType<OkObjectResult>(result);
            var model = Assert.IsType<Product>(requestResult.Value);
            var testmodel = GetProductTest(productTestId);
            Assert.Equal(model, testmodel);

        }



        private List<Product> GetTestProducts()
        {
            List<Product> products = new List<Product>();

            products.Add(new Product { ProductName = "Тест 1", ProductsCategoryId = 1, Protein = 1,
                Fat = 1, Carbohydrates = 1, Calories = 20, UserId = 2 });
            products.Add(new Product { ProductName = "Тест 2", ProductsCategoryId = 2, Protein = 0,
                Fat = 0, Carbohydrates = 1, Calories = 4, UserId = 1 });

            return products;
        }

        private List<Product> GetTestUsersProducts(int userId)
        {
            List<Product> products = new List<Product>();

            products.Add(new Product
            {
                ProductName = "Тест 1",
                ProductsCategoryId = 1,
                Protein = 1,
                Fat = 1,
                Carbohydrates = 1,
                Calories = 20,
                UserId = 1
            });
            products.Add(new Product
            {
                ProductName = "Тест 2",
                ProductsCategoryId = 2,
                Protein = 0,
                Fat = 0,
                Carbohydrates = 1,
                Calories = 4,
                UserId = 1
            });

            return products;
        }

        private List<ProductsCategory> GetTestProductsCategories()
        {
            List<ProductsCategory> productsCategory = new List<ProductsCategory>();

            productsCategory.Add(new ProductsCategory
            {
                ProductsCategoryId = 1,
                ProductsCategoryName = "Тест 1"

            });
            productsCategory.Add(new ProductsCategory
            {
                ProductsCategoryId = 2,
                ProductsCategoryName = "Тест 2"

            });

            return productsCategory;
        }

        private List<Product> GetNullTest()
        {
            return null;
        }

        private List<Product> GetNullTest(int userId)
        {
            return null;
        }

        private List<ProductsCategory> GetNullProductsCategoriesTest()
        {
            return null;
        }
        private bool productsEquals(List<Product> list1, List<Product> list2)
        {
            bool b = new HashSet<Product>(list1).SetEquals(list2);
            return true;
        }

        private Product GetNullProductTest(int productId)
        {
            return null;
        }

        private Product GetProductTest(int productId)
        {
            return new Product
            {
                ProductName = "Тест 1",
                ProductsCategoryId = 1,
                Protein = 1,
                Fat = 1,
                Carbohydrates = 1,
                Calories = 20,
                UserId = 1
            };
        }
    }
}
