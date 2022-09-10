using System.Collections.Generic;
using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using DishesService.Database;
using Dishes.Models;
using System.Linq;

namespace Tests.DishesServiceTests
{
    public class GetDishesTests
    {
        [Fact]
        public async void GetDishesListTest()
        {
            // Arrange
            var mockRepo = new Mock<IDishesRepository>();
            mockRepo.Setup(c => c.GetDishes())
                .ReturnsAsync(GetTestDishes);
            var controller = new DishesService.Controllers.DishesController(mockRepo.Object);

            // Act
            var result = await controller.GetDishes();

            // Assert
            var requestResult = Assert.IsType<OkObjectResult>(result);
            var model = Assert.IsType<List<Dish>>(requestResult.Value);
            var testmodel = GetTestDishes();
            Assert.Equal(model, testmodel);

        }

        [Fact]
        public async void GetNullDishesListTest()
        {
            // Arrange
            var mockRepo = new Mock<IDishesRepository>();
            mockRepo.Setup(c => c.GetDishes())
                .ReturnsAsync(GetNullTestDishes);
            var controller = new DishesService.Controllers.DishesController(mockRepo.Object);

            // Act
            var result = await controller.GetDishes();

            // Assert
            Assert.IsType<NotFoundResult>(result);

        }


        [Fact]
        public async void GetDishesByProductTest()
        {
            // Arrange
            int testProductId = 1;
            var mockRepo = new Mock<IDishesRepository>();
            mockRepo.Setup(c => c.GetDishesByProduct(testProductId))
                .ReturnsAsync(GetTestDishes);
            var controller = new DishesService.Controllers.DishesController(mockRepo.Object);

            // Act
            var result = await controller.GetDishesByProduct(testProductId);

            // Assert
            var requestResult = Assert.IsType<OkObjectResult>(result);
            var model = Assert.IsType<List<Dish>>(requestResult.Value);
            var testmodel = GetTestDishes();
            Assert.Equal(model, testmodel);

        }

        [Fact]
        public async void GetNullDishesByProductListTest()
        {
            // Arrange
            int testProductId = 1;
            var mockRepo = new Mock<IDishesRepository>();
            mockRepo.Setup(c => c.GetDishesByProduct(testProductId))
                .ReturnsAsync(GetNullTestDishes);
            var controller = new DishesService.Controllers.DishesController(mockRepo.Object);

            // Act
            var result = await controller.GetDishesByProduct(testProductId);

            // Assert
            Assert.IsType<NotFoundResult>(result);

        }
        /*       [Fact]
               public async void GetDishByIdTest()
               {
                   // Arrange
                   int dishId = 1;
                   var mockRepo = new Mock<IDishesRepository>();
                   mockRepo.Setup(c => c.GetDishById(dishId))
                       .ReturnsAsync(GetTestDishes().First());
                   var controller = new DishesService.Controllers.DishesController(mockRepo.Object);

                   // Act
                   var result = await controller.GetDishes();

                   // Assert
                   var requestResult = Assert.IsType<OkObjectResult>(result);
                   var model = Assert.IsType<Dish>(requestResult.Value);
                   var testmodel = GetTestDishes().First();
                   Assert.Equal(model, testmodel);

               }
               */

        [Fact]
        public async void GetNullProductsTest()
        {
            // Arrange
            int dishId = 2;
            var mockRepo = new Mock<IDishesRepository>();
            mockRepo.Setup(c => c.GetDishById(dishId))
                .ReturnsAsync(GetTestDishes().FirstOrDefault(d => d.DishId == dishId));
            var controller = new DishesService.Controllers.DishesController(mockRepo.Object);

            // Act
            var result = await controller.GetDishes();

            // Assert
            Assert.IsType<NotFoundResult>(result);

        }


        private List<Dish> GetNullTestDishes()
        {
            return null;
        }
        private List<Dish> GetTestDishes()
        {
            List<Dish> dishes = new List<Dish>();

            var dish1 = new Dish
            {
                DishId = 1,
                DishName = "Тест 1",
                Recipe = "Рецепт 1",
                TotalWeight = 1,
                Ingredients = new List<Ingredient>()
            };

            dish1.Ingredients.Add(new Ingredient
            {
                DishId = 1,
                ProductName = "Ингредиент 1",
                ProductId = 1,
                Count = 1
            });

            dish1.Ingredients.Add(new Ingredient
            {
                DishId = 1,
                ProductName = "Ингредиент 2",
                ProductId = 2,
                Count = 2
            });

            dishes.Add(dish1);
            return dishes;
        }
    }
}
