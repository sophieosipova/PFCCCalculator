using System;

namespace Dishes.Models
{
   // [Table(Name = "tblIngredients")]
    public class Ingredient : IEquatable<Ingredient>
    {
       // [Column(IsPrimaryKey = true, IsDbGenerated = true)]
        public int IngredientId { get; set; }
        public int DishId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public double Count { get; set; }


        public bool Equals(Ingredient other)
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
