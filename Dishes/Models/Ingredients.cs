using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.Linq.Mapping;

namespace Dishes.Models
{
    [Table(Name = "tblDishes")]
    public class Ingredient
    {
        [Column(IsPrimaryKey = true, IsDbGenerated = true)]
        public int IngredientId { get; set; }
        public int DishId { get; set; }
        public int ProductId { get; set; }
        public double? Count { get; set; }
        public string Measure { get; set; }
    }
}
