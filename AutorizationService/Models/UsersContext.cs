using AutorizationService.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AutorizationService.Database
{
    /*   public class UsersContext : DbContext
       {
           public DbSet<User> Users { get; set; }
           public CommentsContext(DbContextOptions<CommentsContext> options)
               : base(options)
           {
               Database.EnsureCreated();
           }*/


    public class BaseContext : IdentityDbContext
    {
        new public DbSet<UserAccount> Users { get; set; }
        public BaseContext(DbContextOptions options) : base(options)
        {
            Database.EnsureCreated();
        }
       
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }



    }
}
