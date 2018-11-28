using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedModels
{
    public class PFCCRecipe
    {
        public int DishId { get; set; }
        public string DishName { get; set; }
        public string DishImage { get; set; }
        public string Recipe { get; set; }
        public double Fat { get; set; }
        public double Protein { get; set; }
        public double Carbohydrates { get; set; }
        public double Calories { get; set; }

        public List<PFCCIngredient> PFCCIngredients;

        public double TotalWeight;
       // Dish dish;
    }

    public class PFCCIngredient
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
    }
}
