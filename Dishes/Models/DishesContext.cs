using Microsoft.EntityFrameworkCore;

namespace Dishes.Models
{
    public class DishesContext : DbContext
    {
        public DbSet<Dish> Dishes { get; set; }
        public DbSet<Ingredient> Ingredients { get; set; }
        public DishesContext(DbContextOptions<DishesContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }
    }
}
