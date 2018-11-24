using Microsoft.EntityFrameworkCore;


namespace ProductsService.Models
{
    public class ProductsContext : DbContext
    {
            public DbSet<Product> Products { get; set; }
            public DbSet<ProductsCategory> ProductsCategories { get; set; }
            public ProductsContext(DbContextOptions<ProductsContext> options)
                : base(options)
            {
            Database.EnsureCreated();
        }
       
    }
}
