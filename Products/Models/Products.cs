using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Threading.Tasks;

namespace ProductsService.Models
{
    [Table(Name = "tblProducts")]
    public class Product
    {
        [Column(IsPrimaryKey = true, IsDbGenerated = true)]
        public int ProductId { get; set; }
        public int ProductsCategoryId { get; set; }
        public string ProductName { get; set; }
        public int Fat { get; set; }
        public int Protein { get; set; }
        public int Carbohydrates { get; set; }
        public int Calories { get; set; }
        public int UserId { get; set; }
    }



}
