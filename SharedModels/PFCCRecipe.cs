using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedModels
{
    public class PFCCRecipe : IEquatable<PFCCRecipe>
    {
        public int DishId { get; set; }
        public string UserId { get; set; }
        public string DishName { get; set; }
        public string Recipe { get; set; }
        public double Fat { get; set; }
        public double Protein { get; set; }
        public double Carbohydrates { get; set; }
        public double Calories { get; set; }

        public List<PFCCIngredient> PFCCIngredients;

        public double TotalWeight;

        public bool Equals(PFCCRecipe other)
        {

           bool ingredientsEquals = new HashSet<PFCCIngredient>(PFCCIngredients).SetEquals(other.PFCCIngredients);

            return other != null &&
                DishId == other.DishId &&
                UserId == other.UserId &&
                DishName == other.DishName &&
                Recipe == other.Recipe &&
                Fat == other.Fat &&
                Protein == other.Protein &&
                Carbohydrates == other.Carbohydrates &&
                Calories == other.Calories &&
                TotalWeight == other.TotalWeight &&
                ingredientsEquals;
        }

    }

    public class PFCCIngredient : IEquatable<PFCCIngredient>
    {
        public int IngredientId { get; set; }
        public int DishId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public double Fat { get; set; }
        public double Protein { get; set; }
        public double Carbohydrates { get; set; }
        public double Calories { get; set; }
        public double Count { get; set; }

        public bool Equals(PFCCIngredient other)
        {
            return other != null &&
                DishId == other.DishId &&
                IngredientId == other.IngredientId &&
                ProductId == other.ProductId &&
                ProductName == other.ProductName &&
                Fat == other.Fat &&
                Protein == other.Protein &&
                Carbohydrates == other.Carbohydrates &&
                Calories == other.Calories &&
                Count == other.Count;
        }
        public override int GetHashCode()
        {
            return this.IngredientId.GetHashCode();
        }

    }
}
