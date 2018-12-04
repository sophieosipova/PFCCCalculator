using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PFCCCalculatorService.Models
{
    public class ProductModel
    {
        public int ProductId { get; set; }
        public int ProductsCategoryId { get; set; }
        public string ProductName { get; set; }
        public int Fat { get; set; }
        public int Protein { get; set; }
        public int Carbohydrates { get; set; }
        public int Calories { get; set; }
        public int UserId { get; set; }
    }

    public  class ProductsCategoryModel
    {
        public int ProductsCategoryId { get; set; }
        public string ProductsCategoryName { get; set; }
    }
}
