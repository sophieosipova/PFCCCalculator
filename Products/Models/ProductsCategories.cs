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
        [Column(IsPrimaryKey = true, IsDbGenerated = true)]
        public int ProductsCategoryId { get; set; }
        public string ProductsCategoryName { get; set; }

    }
}
