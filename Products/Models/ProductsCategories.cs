using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.Linq.Mapping;
using System.ComponentModel.DataAnnotations;

namespace ProductsService.Models
{
    [Table(Name = "tblProductsCategories")]
    public partial class ProductsCategory
    {
        public ProductsCategory()
        {
     //       Products = new HashSet<Product>();
        }
        [Column(IsPrimaryKey = true, IsDbGenerated = true)]
       // [Key]
        public int ProductsCategoryId { get; set; }
        public string ProductsCategoryName { get; set; }

      //  public ICollection<Product> Products { get; set; }
    }
}
