using Microsoft.EntityFrameworkCore;

namespace Comments.Models
{

    public class CommentsContext : DbContext
    {
        public DbSet<Comment> Comments { get; set; }
        public CommentsContext(DbContextOptions<CommentsContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }

    }
}
