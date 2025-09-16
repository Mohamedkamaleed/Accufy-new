using Microsoft.EntityFrameworkCore;
using System.Reflection;
using WarehouseManagement.Core.Entities;

namespace WarehouseManagement.Core.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Warehouse> Warehouses { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<ItemGroup> ItemGroups { get; set; }
        public DbSet<ItemGroupItem> ItemGroupItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Product configuration
            modelBuilder.Entity<Product>(entity =>
            {
                entity.ToTable("Products");
                entity.HasKey(p => p.ProductID);

                entity.Property(p => p.Name)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(p => p.SKU)
                    .HasMaxLength(100);

                entity.Property(p => p.Barcode)
                    .HasMaxLength(100);

                entity.Property(p => p.PurchasePrice)
                    .HasColumnType("decimal(18,2)");

                entity.Property(p => p.SellingPrice)
                    .HasColumnType("decimal(18,2)");

                entity.Property(p => p.MinPrice)
                    .HasColumnType("decimal(18,2)");

                entity.Property(p => p.Discount)
                    .HasColumnType("decimal(10,2)");

                entity.Property(p => p.ProfitMargin)
                    .HasColumnType("decimal(10,2)");

                entity.Property(p => p.InitialStock)
                    .HasColumnType("decimal(18,2)");

                entity.Property(p => p.LowStockThreshold)
                    .HasColumnType("decimal(18,2)");

                entity.Property(p => p.Status)
                    .HasDefaultValue(true);

                entity.Property(p => p.CreatedAt)
                    .HasDefaultValueSql("GETUTCDATE()");

                // Relationships
                entity.HasOne(p => p.Category)
                    .WithMany()
                    .HasForeignKey(p => p.CategoryID)
                    .OnDelete(DeleteBehavior.Restrict);

                //entity.HasOne(p => p.Brand)
                //    .WithMany(b => b.Products)
                //    .HasForeignKey(p => p.BrandID)
                //    .OnDelete(DeleteBehavior.SetNull);

                //entity.HasOne(p => p.Supplier)
                //    .WithMany(s => s.Products)
                //    .HasForeignKey(p => p.SupplierID)
                //    .OnDelete(DeleteBehavior.SetNull);

                // Indexes
                entity.HasIndex(p => p.SKU)
                    .IsUnique()
                    .HasFilter("[SKU] IS NOT NULL");

                entity.HasIndex(p => p.Barcode)
                    .IsUnique()
                    .HasFilter("[Barcode] IS NOT NULL");

                entity.HasIndex(p => p.CategoryID);

                entity.HasIndex(p => p.BrandID);

                entity.HasIndex(p => p.SupplierID);


                // Add relationship to ItemGroup
                entity.HasOne(p => p.ItemGroup)
                    .WithMany(g => g.Products)
                    .HasForeignKey(p => p.GroupID)
                    .OnDelete(DeleteBehavior.SetNull);

                // Add index for GroupID
                entity.HasIndex(p => p.GroupID);

            });

            // Brand configuration
            modelBuilder.Entity<Brand>(entity =>
            {
                entity.ToTable("Brands");
                entity.HasKey(b => b.BrandID);

                entity.Property(b => b.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.HasIndex(b => b.Name)
                    .IsUnique();
            });


            // Service configuration
            modelBuilder.Entity<Service>(entity =>
            {
                entity.ToTable("Services");
                entity.HasKey(s => s.ServiceID);

                entity.Property(s => s.Name)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(s => s.Code)
                    .HasMaxLength(50);

                entity.Property(s => s.PurchasePrice)
                    .HasColumnType("decimal(18,2)");

                entity.Property(s => s.UnitPrice)
                    .HasColumnType("decimal(18,2)");

                entity.Property(s => s.MinPrice)
                    .HasColumnType("decimal(18,2)");

                entity.Property(s => s.Discount)
                    .HasColumnType("decimal(10,2)");

                entity.Property(s => s.ProfitMargin)
                    .HasColumnType("decimal(10,2)");

                entity.Property(s => s.Status)
                    .HasDefaultValue(true);

                entity.Property(s => s.CreatedAt)
                    .HasDefaultValueSql("GETUTCDATE()");

                // Relationships
                entity.HasOne(s => s.Category)
                    .WithMany()
                    .HasForeignKey(s => s.CategoryID)
                    .OnDelete(DeleteBehavior.SetNull);

                entity.HasOne(s => s.Supplier)
                    .WithMany()
                    .HasForeignKey(s => s.SupplierID)
                    .OnDelete(DeleteBehavior.SetNull);

                // Indexes
                entity.HasIndex(s => s.Code)
                    .IsUnique()
                    .HasFilter("[Code] IS NOT NULL");

                entity.HasIndex(s => s.CategoryID);

                entity.HasIndex(s => s.SupplierID);
            });



            // ItemGroup configuration
            modelBuilder.Entity<ItemGroup>(entity =>
            {
                entity.ToTable("ItemGroups");
                entity.HasKey(g => g.GroupID);

                entity.Property(g => g.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                // Relationships
                entity.HasOne(g => g.Category)
                    .WithMany()
                    .HasForeignKey(g => g.CategoryID)
                    .OnDelete(DeleteBehavior.SetNull);

                entity.HasOne(g => g.Brand)
                    .WithMany()
                    .HasForeignKey(g => g.BrandID)
                    .OnDelete(DeleteBehavior.SetNull);

                // Indexes
                entity.HasIndex(g => g.Name)
                    .IsUnique();

                entity.HasIndex(g => g.CategoryID);

                entity.HasIndex(g => g.BrandID);
            });


            // Apply all configurations from the current assembly
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
