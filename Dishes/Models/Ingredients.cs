using System.Data.Linq.Mapping;

namespace Dishes.Models
{
   // [Table(Name = "tblIngredients")]
    public class Ingredient
    {
        [Column(IsPrimaryKey = true, IsDbGenerated = true)]
        public int IngredientId { get; set; }
        public int DishId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public double Count { get; set; }
    }
}
