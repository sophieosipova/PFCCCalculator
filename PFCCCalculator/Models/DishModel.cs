
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PFCCCalculatorService.Models
{
    public class DishModel : IEquatable<DishModel>
    {
        public int DishId { get; set; }
        public string UserId { get; set; }

        [Required(ErrorMessage = "Укажите название рецепта")]
        public string DishName { get; set; }

        [Required(ErrorMessage = "Укажите рецепт")]
        public string Recipe { get; set; }

        [Required(ErrorMessage = "Укажите вес блюда")]
        public double TotalWeight { get; set; }

        [Required(ErrorMessage = "Ингредиенты не указаны")]
        public ICollection<IngredientModel> Ingredients { get; set; }
        public bool Equals(DishModel other)
        {

            bool ingredientsEquals = new HashSet<IngredientModel>(Ingredients).SetEquals(other.Ingredients);

            return other != null &&
                DishId == other.DishId &&
                DishName == other.DishName &&
                Recipe == other.Recipe &&
                TotalWeight == other.TotalWeight &&
                ingredientsEquals;
        }

        public override int GetHashCode()
        {
            return this.DishId.GetHashCode();
        }
    }

    public class IngredientModel : IEquatable<IngredientModel>
    {
        public int IngredientId { get; set; }
        public int DishId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public double Count { get; set; }

        public bool Equals(IngredientModel other)
        {
            return other != null &&
                DishId == other.DishId &&
                IngredientId == other.IngredientId &&
                ProductId == other.ProductId &&
                ProductName == other.ProductName &&
                Count == other.Count;
        }
        public override int GetHashCode()
        {
            return this.IngredientId.GetHashCode();
        }
    }
}
