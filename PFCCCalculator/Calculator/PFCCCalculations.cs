using PFCCCalculatorService.Models;
using SharedModels;
using System.Collections.Generic;
using System.Linq;


namespace PFCCCalculatorService.Calculator
{
    public static  class PFCCCalculations
    {

        public static PFCCIngredient CalculateIngredient (IngredientModel ingredient, ProductModel product)
        {
            double sc = ingredient.Count / 100.0;
            PFCCIngredient pFCCIngredient = new PFCCIngredient

            { IngredientId = ingredient.IngredientId,
                ProductId = ingredient.ProductId,
                ProductName = product.ProductName,
                Count = ingredient.Count,
                DishId = ingredient.DishId,
                Protein = product.Protein * sc,
                Fat = product.Fat * sc,
                Carbohydrates = product.Carbohydrates * sc,
                Calories = product.Calories * sc
            }
                ;

            return pFCCIngredient;

        }

        private static PFCCIngredient CalculateIngredient(IngredientModel ingredient)
        {
            double sc = ingredient.Count / 100.0;
            PFCCIngredient pFCCIngredient = new PFCCIngredient

            {
                IngredientId = ingredient.IngredientId,
                ProductId = ingredient.ProductId,
                ProductName = ingredient.ProductName,
                Count = ingredient.Count,
                DishId = ingredient.DishId,
            }
            ;

            return pFCCIngredient;

        }

        public static PFCCRecipe PFCCRecipeCalculator (DishModel dish, List<PFCCIngredient> pFCCIngredients)
        {
            PFCCRecipe pFCCRecipe = new PFCCRecipe
            {
                PFCCIngredients = pFCCIngredients,
                UserId = dish.UserId,
                DishId =dish.DishId,
                DishName = dish.DishName,
                Recipe = dish.Recipe,
                TotalWeight = dish.TotalWeight
            };

           
           // double totalWeight = pFCCIngredients.Sum(i => i.Count);
            double sc = 100 / pFCCRecipe.TotalWeight;
            double  totalFat = pFCCIngredients.Sum(i => i.Fat) * sc;
            double totalProtein = pFCCIngredients.Sum(i => i.Protein) * sc;
            double totalCarbohydrates = pFCCIngredients.Sum(i => i.Carbohydrates) * sc;
            double  totalCalories = pFCCIngredients.Sum(i => i.Calories) * sc;

            pFCCRecipe.Protein = totalProtein;
            pFCCRecipe.Fat = totalFat;
            pFCCRecipe.Carbohydrates = totalCarbohydrates;
            pFCCRecipe.Calories = totalCalories;
            return pFCCRecipe;
        }


        public static PFCCRecipe PFCCRecipeCalculator(DishModel dish)
        {
            List<PFCCIngredient> IList = new List<PFCCIngredient>();
            if (dish.Ingredients.Any())
            {
                foreach (IngredientModel ingredient in dish.Ingredients)
                {
                    IList.Add(CalculateIngredient(ingredient));
                }
            }
            PFCCRecipe pFCCRecipe = new PFCCRecipe
            {
                PFCCIngredients = IList,
                UserId = dish.UserId,
                DishId = dish.DishId,
                DishName = dish.DishName,
                Recipe = dish.Recipe,
                TotalWeight = dish.TotalWeight
            };



            return pFCCRecipe;
        }
    }
}
