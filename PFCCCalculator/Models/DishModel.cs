using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PFCCCalculatorService.Models
{
    public class DishModel
    {
        public int DishId { get; set; }
        public int UserId { get; set; }
        public string DishName { get; set; }
        public string DishImage { get; set; }
        public string Recipe { get; set; }
        public double TotalWeight { get; set; }

        public ICollection<IngredientModel> Ingredients { get; set; }
    }

    public class IngredientModel
    {
        public int IngredientId { get; set; }
        public int DishId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public double Count { get; set; }
    }
}
