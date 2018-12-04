using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.Linq.Mapping;
using System.ComponentModel.DataAnnotations;

namespace ProductsService.Models
{
    [Table(Name = "tblProductsCategories")]
    public partial class ProductsCategory : IEquatable<ProductsCategory>
    {
        [Column(IsPrimaryKey = true, IsDbGenerated = true)]
        public int ProductsCategoryId { get; set; }
        public string ProductsCategoryName { get; set; }

        public bool Equals(ProductsCategory other)
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
