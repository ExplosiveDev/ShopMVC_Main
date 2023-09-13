using DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using DataAccess.EntiitesConfiguration;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace DataAccess.Data
{
    public class ShopMVCDbContext:IdentityDbContext<User>
    {
        public ShopMVCDbContext(DbContextOptions<ShopMVCDbContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Category>().HasData(SeedData.GetCategory());
            modelBuilder.Entity<Product>().HasData(SeedData.GetProduct());

            #region Fluent API => Configurations
            ////Set Primary Key
            //modelBuilder.Entity<Product>().HasKey(x => x.Id);

            ////Set Property configurations
            //modelBuilder.Entity<Product>()
            //            .Property(x => x.Name)
            //            .HasMaxLength(150)
            //            .IsRequired();

            ////Set Relationship configurations
            //modelBuilder.Entity<Product>()
            //    .HasOne<Category>(x => x.Category)
            //    .WithMany(x => x.Products)
            //    .HasForeignKey(x => x.CategoryId);
            #endregion

            // ApplyConfigurations for Entities 
            // modelBuilder.ApplyConfigurationsFromAssembly(typeof(ShopMVCDbContext).Assembly);
            // or 
            //modelBuilder.ApplyConfiguration(new ProductConfiguration());


        }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; } 
        public DbSet<Order> Orders { get; set; }
        public DbSet<Storage> Storage { get; set; }
    }
}
