using Api_Integration_Product.Models.CartItemmodels;
using Api_Integration_Product.Models.ProductModels;
using Api_Integration_Product.Models.UserModels;
using Microsoft.EntityFrameworkCore;

namespace Api_Integration_Product.Data
{
    public class MyAppContext : DbContext
    {
        public MyAppContext(DbContextOptions<MyAppContext> options) : base(options)
        {
            
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<CartItem> CartItem { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(MyAppContext).Assembly);
        }
    }
}
