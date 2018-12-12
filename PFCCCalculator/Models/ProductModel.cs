

using System;

namespace PFCCCalculatorService.Models
{
    public class ProductModel : IEquatable<ProductModel>
    {
        public int ProductId { get; set; }
        public int ProductsCategoryId { get; set; }
        public string ProductName { get; set; }
        public int Fat { get; set; }
        public int Protein { get; set; }
        public int Carbohydrates { get; set; }
        public int Calories { get; set; }
        public int UserId { get; set; }

        public bool Equals(ProductModel other)
        {
            return other != null &&
                ProductId == other.ProductId &&
                ProductsCategoryId == other.ProductsCategoryId &&
                ProductId == other.ProductId &&
                ProductName == other.ProductName &&
                Fat == other.Fat &&
                Protein == other.Protein &&
                Carbohydrates == other.Carbohydrates &&
                Calories == other.Calories &&
                UserId == other.UserId;
        }
        public override int GetHashCode()
        {
            return this.ProductId.GetHashCode();
        }
    }

    public  class ProductsCategoryModel : IEquatable<ProductsCategoryModel>
    {
        public int ProductsCategoryId { get; set; }
        public string ProductsCategoryName { get; set; }

        public bool Equals(ProductsCategoryModel other)
        {
            return other != null &&
                ProductsCategoryId == other.ProductsCategoryId &&
                ProductsCategoryName == other.ProductsCategoryName;
        }
        public override int GetHashCode()
        {
            return this.ProductsCategoryId.GetHashCode();
        }
    }
}
