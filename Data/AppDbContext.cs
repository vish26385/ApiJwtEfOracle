using ApiJwtEfOracle.Configuration;
using ApiJwtEfOracle.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiJwtEfOracle.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        // Define DbSets for your entities here
        // public DbSet<YourEntity> YourEntities { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<Product>(e =>
            //{
            //    e.ToTable("PRODUCTS");
            //    e.HasKey(x => x.Id);
            //    e.Property(x => x.Id).HasColumnName("ID");
            //    e.Property(x => x.Name).HasColumnName("NAME").HasMaxLength(200);
            //    e.Property(x => x.Price).HasColumnName("PRICE");
            //    e.Property(x => x.CreatedAt).HasColumnName("CREATED_AT");
            //});
            modelBuilder.ApplyConfiguration(new ProductConfiguration());
            modelBuilder.ApplyConfiguration(new UserConfiguration());
        }
    }
}
