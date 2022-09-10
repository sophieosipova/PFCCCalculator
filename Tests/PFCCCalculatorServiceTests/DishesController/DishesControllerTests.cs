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
using PFCCCalculatorService.Controllers;
using System;
using SharedModels;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc;

namespace Tests.PFCCCalculatorServiceTests.DishesController
{
    public class DishesControllerTests
    {
        [Fact]
        public async void OkCreateDishTest()
        {
            // Arrange
            int userTestId = 1;
            var dish= GetTestDishes();
            var mockRepo = new Mock<IDishesService>();
            mockRepo.Setup(c => c.CreateDish(userTestId, dish))
                .ReturnsAsync(GetTestDishes);
            var controller = new PFCCCalculatorService.Controllers.DishesController(mockRepo.Object);


            // Act
            var result = await controller.CreateDish(userTestId, dish);


            // Assert
            var requestResult = Assert.IsType<CreatedResult>(result);
            var model = Assert.IsType<DishModel>(requestResult.Value);
            var testmodel = GetTestDishes();
            Assert.Equal(model, testmodel);
        }

        [Fact]
        public async void CreateNullDishTest()
        {
            // Arrange
            int userTestId = 1;
            var dish = GetTestDishes();
            var mockRepo = new Mock<IDishesService>();
            mockRepo.Setup(c => c.CreateDish(userTestId, dish))
                .ReturnsAsync(GetNullTestDishes);
            var controller = new PFCCCalculatorService.Controllers.DishesController(mockRepo.Object);


            // Act
            var result = await controller.CreateDish(userTestId, dish);


            // Assert
            Assert.IsType<ConflictObjectResult>(result);
        }


        private DishModel GetNullTestDishes()
        {
            return null;
        }
            private DishModel GetTestDishes()
        {
           // List<Dish> dishes = new List<Dish>();

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

           
            return dish1;
        }
    }
}
