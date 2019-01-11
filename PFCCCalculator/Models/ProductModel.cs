

using System;
using System.ComponentModel.DataAnnotations;

namespace PFCCCalculatorService.Models
{
    public class ProductModel : IEquatable<ProductModel>
    {
        public int ProductId { get; set; }
        public int ProductsCategoryId { get; set; }

        [Required(ErrorMessage = "Укажите название продукта")]
        public string ProductName { get; set; }

        [Range(0, 100, ErrorMessage = "Содежание жиров должно быть в промежутке от 0 до 100")]
        [Required(ErrorMessage = "Укажите содержание жиров в продукте")]
        public int Fat { get; set; }

        [Range(0, 100, ErrorMessage = "Содежание белков должно быть в промежутке от 0 до 100")]
        [Required(ErrorMessage = "Укажите содержание белков в продукте")]
        public int Protein { get; set; }

        [Range(0, 100, ErrorMessage = "Содежание углкводов должно быть в промежутке от 0 до 100")]
        [Required(ErrorMessage = "Укажите содержание углеводов в продукте")]
        public int Carbohydrates { get; set; }

        [Range(0, 1000, ErrorMessage = "Неверно указана калорийность продукта")]
        [Required(ErrorMessage = "Укажите содержание калоийность продукта")]
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
