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

namespace Tests.PFCCCalculatorServiceTests.PFCCController
{
    public class DeleteProductTests
    {
        public Mock <IProductsService> ProductsServiceMock = new Mock<IProductsService>();
        public Mock  <IDishesService> DishesServiceMock = new Mock<IDishesService>();
        public Mock  <ICommentsService> CommentsServiceMock = new Mock<ICommentsService>();
    //    public Mock<IGateway> CommentsServiceMock = new Mock<ICommentsService>();
        private TestServer _server;
        public HttpClient Client { get; set; }

        public DeleteProductTests ()
        {
         _server = new TestServer(new WebHostBuilder().UseStartup<Startup>().ConfigureTestServices(services =>

            {
                services.AddSingleton<IProductsService>( ProductsServiceMock.Object);
                services.AddSingleton<IDishesService>(DishesServiceMock.Object);
                services.AddSingleton<ICommentsService>(CommentsServiceMock.Object);


            }));

            Client = _server.CreateClient();
        }

        [Fact]
        public async void ConflictDeleteProductTest()
        {
            // Arrange
            int productId = 1;
            int userId = 1;

            ProductsServiceMock.Setup(c => c.DeleteProduct(userId, productId))
                .ReturnsAsync(ReturnDeleted);
            DishesServiceMock.Setup(c => c.GetDishesWithProduct(productId))
                .ReturnsAsync(GetTestDishes);
            // Act 
            var result = await Client.DeleteAsync("api/pfcccalculator/user/0/product/1");

            // Assert

            Assert.Equal(HttpStatusCode.Conflict, result.StatusCode);
            Assert.Equal("Невозможно удалить", await result.Content.ReadAsStringAsync());
            ProductsServiceMock.Reset();
            DishesServiceMock.Reset();
        }

        [Fact]
        public async void ExceptionDeleteProductTest()
        {
            // Arrange
            int productId = 1;
            int userId = 1;

            ProductsServiceMock.Setup(c => c.DeleteProduct(userId, productId))
                .ReturnsAsync(ReturnDeleted);
            DishesServiceMock.Setup(c => c.GetDishesWithProduct(productId))
                .ReturnsAsync(GetExceptionTestDishes);
            // Act 
            var result = await Client.DeleteAsync("api/pfcccalculator/user/0/product/1");

            // Assert

            Assert.Equal(HttpStatusCode.Conflict, result.StatusCode);
            Assert.Equal("Test", await result.Content.ReadAsStringAsync());
            ProductsServiceMock.Reset();
            DishesServiceMock.Reset();
        }

   

        [Fact]
        public async void NotFoundDeleteProductTest()
        {
            // Arrange
            int productId = 1;
            int userId = 1;

            ProductsServiceMock.Setup(c => c.DeleteProduct(userId, productId))
                .ReturnsAsync(ReturnCantDelete);
            DishesServiceMock.Setup(c => c.GetDishesWithProduct(productId))
                .ReturnsAsync(GetNullTestDishes);
            // Act 
            var result = await Client.DeleteAsync("api/pfcccalculator/user/0/product/1");

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);
            ProductsServiceMock.Reset();
            DishesServiceMock.Reset();
        }

        private bool ReturnCantDelete()
        {
            return false;
        }

        private bool ReturnDeleted()
        {
            return true;
        }

        private static  List<ProductModel> GetTestProducts()
        {
            List<ProductModel> products = new List<ProductModel>();

            products.Add(new ProductModel
            {
                ProductName = "Тест 1",
                ProductsCategoryId = 1,
                Protein = 1,
                Fat = 1,
                Carbohydrates = 1,
                Calories = 20,
                UserId = 2
            });
            products.Add(new ProductModel
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
        private  List<DishModel> GetTestDishes()
        {
            List<DishModel> dishes = new List<DishModel>();

            var dish1 = new DishModel
            {
                DishId = 1,
                DishName = "Тест 1",
                Recipe = "Рецепт 1",
                TotalWeight = 1,
                Ingredients = new List<IngredientModel>()
            };

            dish1.Ingredients.Add(new IngredientModel
            {
                DishId = 1,
                ProductName = "Ингредиент 1",
                ProductId = 1,
                Count = 1
            });

            dish1.Ingredients.Add(new IngredientModel
            {
                DishId = 1,
                ProductName = "Ингредиент 2",
                ProductId = 2,
                Count = 2
            });

            dishes.Add(dish1);
            return dishes;
        }

        private  List<DishModel> GetExceptionTestDishes()
        {
            throw new Exception("Test");
        }

        private  List<DishModel> GetNullTestDishes()
        {
            return null;
        }
    }
}
