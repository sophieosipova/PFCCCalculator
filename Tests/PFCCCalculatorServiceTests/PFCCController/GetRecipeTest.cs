using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using PFCCCalculatorService.Services;
using Smart.Mock;
using Moq;
using PFCCCalculatorService.Controllers;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using SharedModels;

namespace Tests
{
    public  class PFCCControllerTestGetDish
    {
        [Fact]
        public async void GetPFCCRecipeTest()
        {
            // Arrange
            int testId = 1;
            var mockRepo = new Mock<IGatewayService>();
            mockRepo.Setup(c => c.GetRecipeWithPFCC(testId))
                .ReturnsAsync(GetTestRecipe);
            var controller = new PFCCCalculatorController(mockRepo.Object);


            // Act
            var result = await controller.GetRecipeWithPFCC(testId);
           

            // Assert
            var requestResult = Assert.IsType<OkObjectResult>(result);
            var model = Assert.IsType<PFCCRecipe>(requestResult.Value);
            var testmodel = GetTestRecipe();
             Assert.Equal<PFCCRecipe>(model, testmodel);

        }

        [Fact]
        public async void GetNullRecipeTest()
        {
            // Arrange
            int testId = 1;
            var mockRepo = new Mock<IGatewayService>();
            mockRepo.Setup(c => c.GetRecipeWithPFCC(testId))
                .ReturnsAsync(GetNullTestRecipe);
            var controller = new PFCCCalculatorController(mockRepo.Object);


            // Act
            var result = await controller.GetRecipeWithPFCC(testId);


            // Assert
            var requestResult = Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async void GetExeptionTest()
        {
            // Arrange
            int testId = 1;
            var mockRepo = new Mock<IGatewayService>();
            mockRepo.Setup(c => c.GetRecipeWithPFCC(testId))
                .ReturnsAsync(GetTestException);
            var controller = new PFCCCalculatorController(mockRepo.Object);


            // Act
            var result = await controller.GetRecipeWithPFCC(testId);


            // Assert
            var requestResult = Assert.IsType<ConflictObjectResult>(result);
            Assert.Equal(requestResult.Value, message);
        }

        private PFCCRecipe GetTestRecipe()
        {
            List<PFCCIngredient> pFCCIngredients = new List<PFCCIngredient>();
            PFCCIngredient pFCCIngredient = new PFCCIngredient
            {
                IngredientId = 1,
                ProductId = 1,
                ProductName = "Запечёное яблоко",
                Count = 120,
                DishId = 1,
                Protein = 1.2,
                Fat = 1.2,
                Carbohydrates = 30,
                Calories = 60
            };

            pFCCIngredients.Add(pFCCIngredient);
            PFCCRecipe pFCCRecipe = new PFCCRecipe
            {
                PFCCIngredients = pFCCIngredients,
                DishId = 1,
                DishImage = "",
                DishName = "Яблоко",
                Recipe = "Яблоко порезать на дольки",
                TotalWeight = 120
            };


            return pFCCRecipe;

        }

        private PFCCRecipe GetNullTestRecipe()
        {
            return null;
        }

        private PFCCRecipe GetTestException()
        {
            throw new Exception (message);
        }
        private string message = "Test exception";

    }
}