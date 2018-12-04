using System;


namespace ProductsService.Models
{
    //[Table(Name = "tblProducts")]
    public class Product : IEquatable<Product>
    {
       // [Column(IsPrimaryKey = true, IsDbGenerated = true)]
        public int ProductId { get; set; }
        public int ProductsCategoryId { get; set; }
        public string ProductName { get; set; }
        public int Fat { get; set; }
        public int Protein { get; set; }
        public int Carbohydrates { get; set; }
        public int Calories { get; set; }
        public int UserId { get; set; }

        public bool Equals(Product other)
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



}
