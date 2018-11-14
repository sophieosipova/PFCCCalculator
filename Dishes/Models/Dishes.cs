using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.Linq.Mapping;

namespace Dishes.Models
{
    [Table(Name = "tblDishes")]
    public class Dish
    {
        public Dish()
        {
            Ingredients = new HashSet<Ingredient>();
        }
        [Column(IsPrimaryKey = true, IsDbGenerated = true)]
        public int DishId { get; set; }
        public string DishName { get; set; }
        public string DishImage { get; set; }
        public string Recipe { get; set; }
        public int? Fat { get; set; }
        public int? Protein { get; set; }
        public int? Carbohydrates { get; set; }
        public int? Calories { get; set; }

        public ICollection<Ingredient> Ingredients { get; set; }
    }
}
