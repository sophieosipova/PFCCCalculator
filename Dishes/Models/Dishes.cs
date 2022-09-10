using System;
using System.Collections.Generic;
//using System.Data.Linq.Mapping;

namespace Dishes.Models
{
    //[Table(Name = "tblDishes")]
    public class Dish : IEquatable<Dish>
    {
        public Dish()
        {
            Ingredients = new HashSet<Ingredient>();
        }
      //  [Column(IsPrimaryKey = true, IsDbGenerated = true)]
        public int DishId { get; set; }
        public string UserId { get; set; }
        public string DishName { get; set; }
        public string Recipe { get; set; }
        public double TotalWeight { get; set; }

        public ICollection<Ingredient> Ingredients { get; set; }

        public bool Equals(Dish other)
        {

            bool ingredientsEquals = new HashSet<Ingredient>(Ingredients).SetEquals(other.Ingredients);

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
}
