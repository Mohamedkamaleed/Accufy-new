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

        public DbSet<ProductTaxProfile> ProductTaxProfiles { get; set; }

        public DbSet<DefaultTax> DefaultTaxes { get; set; }
        public DbSet<TaxProfile> TaxProfiles { get; set; }
        public DbSet<TaxProfileTax> TaxProfileTaxes { get; set; }
        public DbSet<StockTransaction> StockTransactions { get; set; }

        public DbSet<PurchaseOrder> PurchaseOrders { get; set; }
        public DbSet<PurchaseOrderLine> PurchaseOrderLines { get; set; }
        public DbSet<PurchaseOrderAttachment> PurchaseOrderAttachments { get; set; }
        public DbSet<PurchaseOrderStatusHistory> PurchaseOrderStatusHistory { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);



            // Configure all entities with CreatedAt to use CURRENT_TIMESTAMP
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                var createdAtProperty = entityType.FindProperty("CreatedAt");
                if (createdAtProperty != null && createdAtProperty.ClrType == typeof(DateTime))
                {
                    createdAtProperty.SetDefaultValueSql("CURRENT_TIMESTAMP");
                }

                var createdDateProperty = entityType.FindProperty("CreatedDate");
                if (createdDateProperty != null && createdDateProperty.ClrType == typeof(DateTime))
                {
                    createdDateProperty.SetDefaultValueSql("CURRENT_TIMESTAMP");
                }
            }

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

            // ItemGroupItem configuration
            modelBuilder.Entity<ItemGroupItem>(entity =>
            {
                entity.ToTable("ItemGroupItems");
                entity.HasKey(i => i.GroupItemID);

                entity.Property(i => i.SKU)
                    .HasMaxLength(100);

                entity.Property(i => i.Barcode)
                    .HasMaxLength(100);

                entity.Property(i => i.PurchasePrice)
                    .HasColumnType("decimal(18,2)");

                entity.Property(i => i.SellingPrice)
                    .HasColumnType("decimal(18,2)");

                // Relationships
                entity.HasOne(i => i.ItemGroup)
                    .WithMany()
                    .HasForeignKey(i => i.GroupID)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(i => i.Product)
                    .WithMany()
                    .HasForeignKey(i => i.ProductID)
                    .OnDelete(DeleteBehavior.Cascade);

                // Indexes
                entity.HasIndex(i => new { i.GroupID, i.ProductID })
                    .IsUnique();

                entity.HasIndex(i => i.SKU)
                    .IsUnique()
                    .HasFilter("[SKU] IS NOT NULL");

                entity.HasIndex(i => i.Barcode)
                    .IsUnique()
                    .HasFilter("[Barcode] IS NOT NULL");
            });


            // ProductTaxProfile configuration
            modelBuilder.Entity<ProductTaxProfile>(entity =>
            {
                entity.ToTable("ProductTaxProfiles");
                entity.HasKey(pt => new { pt.ProductID, pt.TaxProfileID });

                entity.Property(pt => pt.IsPrimary)
                    .HasDefaultValue(false);

                // Relationships
                entity.HasOne(pt => pt.Product)
                    .WithMany(p => p.ProductTaxProfiles)
                    .HasForeignKey(pt => pt.ProductID)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(pt => pt.TaxProfile)
                    .WithMany(tp => tp.ProductTaxProfiles)
                    .HasForeignKey(pt => pt.TaxProfileID)
                    .OnDelete(DeleteBehavior.Cascade);
            });


            // TaxProfile configuration (if not already defined)
            modelBuilder.Entity<TaxProfile>(entity =>
            {
                entity.ToTable("TaxProfiles");
                entity.HasKey(tp => tp.TaxProfileID);

                entity.Property(tp => tp.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(tp => tp.TaxRate)
                    .HasColumnType("decimal(5,2)");

                entity.Property(tp => tp.Description)
                    .HasMaxLength(500);

                entity.HasIndex(tp => tp.Name)
                    .IsUnique();
            });




            // DefaultTax configuration
            modelBuilder.Entity<DefaultTax>(entity =>
            {
                entity.ToTable("DefaultTaxes");
                entity.HasKey(t => t.TaxID);

                entity.Property(t => t.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(t => t.TaxValue)
                    .HasColumnType("decimal(10,4)");

                entity.Property(t => t.Type)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(t => t.Mode)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.HasIndex(t => t.Name)
                    .IsUnique();
            });

            // TaxProfile configuration
            modelBuilder.Entity<TaxProfile>(entity =>
            {
                entity.ToTable("TaxProfiles");
                entity.HasKey(tp => tp.TaxProfileID);

                entity.Property(tp => tp.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.HasIndex(tp => tp.Name)
                    .IsUnique();
            });

            // TaxProfileTax configuration (join table)
            modelBuilder.Entity<TaxProfileTax>(entity =>
            {
                entity.ToTable("TaxProfileTaxes");
                entity.HasKey(tpt => new { tpt.TaxProfileID, tpt.TaxID });

                // Relationships
                entity.HasOne(tpt => tpt.TaxProfile)
                    .WithMany(tp => tp.TaxProfileTaxes)
                    .HasForeignKey(tpt => tpt.TaxProfileID)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(tpt => tpt.DefaultTax)
                    .WithMany(t => t.TaxProfileTaxes)
                    .HasForeignKey(tpt => tpt.TaxID)
                    .OnDelete(DeleteBehavior.Cascade);
            });



            // StockTransaction configuration
            modelBuilder.Entity<StockTransaction>(entity =>
            {
                entity.ToTable("StockTransactions");
                entity.HasKey(st => st.TransactionID);

                entity.Property(st => st.TransactionDate)
                    .IsRequired()
                    .HasDefaultValueSql("GETUTCDATE()");

                entity.Property(st => st.TransactionType)
                    .IsRequired()
                    .HasConversion<string>()
                    .HasMaxLength(20);

                entity.Property(st => st.Quantity)
                    .IsRequired()
                    .HasColumnType("decimal(18,2)");

                entity.Property(st => st.UnitPrice)
                    .IsRequired()
                    .HasColumnType("decimal(18,2)");

                entity.Property(st => st.StockLevelAfter)
                    .IsRequired()
                    .HasColumnType("decimal(18,2)");

                entity.Property(st => st.Reference)
                    .HasMaxLength(100);

                entity.Property(st => st.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(450);

                // Relationships
                entity.HasOne(st => st.Product)
                    .WithMany(p => p.StockTransactions)
                    .HasForeignKey(st => st.ProductID)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(st => st.Warehouse)
                    .WithMany()
                    .HasForeignKey(st => st.WarehouseID)
                    .OnDelete(DeleteBehavior.Restrict);

                // Indexes for better query performance
                entity.HasIndex(st => st.TransactionDate);
                entity.HasIndex(st => st.ProductID);
                entity.HasIndex(st => st.WarehouseID);
                entity.HasIndex(st => st.TransactionType);
            });

            // PurchaseOrder configuration
            modelBuilder.Entity<PurchaseOrder>(entity =>
            {
                entity.ToTable("PurchaseOrders");
                entity.HasKey(po => po.PurchaseOrderID);

                entity.Property(po => po.OrderNumber)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(po => po.ReferenceNumber)
                    .HasMaxLength(50);

                entity.Property(po => po.ShippingAddress)
                    .HasMaxLength(500);

                entity.Property(po => po.BillingAddress)
                    .HasMaxLength(500);

                entity.Property(po => po.SubTotal)
                    .HasColumnType("decimal(18,2)")
                    .HasDefaultValue(0);

                entity.Property(po => po.TaxAmount)
                    .HasColumnType("decimal(18,2)")
                    .HasDefaultValue(0);

                entity.Property(po => po.DiscountAmount)
                    .HasColumnType("decimal(18,2)")
                    .HasDefaultValue(0);

                entity.Property(po => po.ShippingCost)
                    .HasColumnType("decimal(18,2)")
                    .HasDefaultValue(0);

                entity.Property(po => po.TotalAmount)
                    .HasColumnType("decimal(18,2)")
                    .HasDefaultValue(0);

                entity.Property(po => po.Notes)
                    .HasMaxLength(1000);

                entity.Property(po => po.TermsAndConditions)
                    .HasMaxLength(1000);

                entity.Property(po => po.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(450);

                entity.Property(po => po.UpdatedBy)
                    .HasMaxLength(450);

                entity.HasIndex(po => po.OrderNumber)
                    .IsUnique();

                entity.HasOne(po => po.Supplier)
                    .WithMany(s => s.PurchaseOrders)
                    .HasForeignKey(po => po.SupplierID)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // PurchaseOrderLine configuration
            modelBuilder.Entity<PurchaseOrderLine>(entity =>
            {
                entity.ToTable("PurchaseOrderLines");
                entity.HasKey(pol => pol.PurchaseOrderLineID);

                entity.Property(pol => pol.ProductName)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(pol => pol.ProductSKU)
                    .HasMaxLength(100);

                entity.Property(pol => pol.Quantity)
                    .HasColumnType("decimal(18,2)")
                    .HasDefaultValue(0);

                entity.Property(pol => pol.UnitPrice)
                    .HasColumnType("decimal(18,2)")
                    .HasDefaultValue(0);

                entity.Property(pol => pol.DiscountPercentage)
                    .HasColumnType("decimal(18,2)")
                    .HasDefaultValue(0);

                entity.Property(pol => pol.TaxPercentage)
                    .HasColumnType("decimal(18,2)")
                    .HasDefaultValue(0);

                entity.Property(pol => pol.LineTotal)
                    .HasColumnType("decimal(18,2)")
                    .HasDefaultValue(0);

                entity.Property(pol => pol.ReceivedQuantity)
                    .HasColumnType("decimal(18,2)")
                    .HasDefaultValue(0);

                entity.Property(pol => pol.Notes)
                    .HasMaxLength(500);

                entity.HasOne(pol => pol.PurchaseOrder)
                    .WithMany(po => po.OrderLines)
                    .HasForeignKey(pol => pol.PurchaseOrderID)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(pol => pol.Product)
                    .WithMany()
                    .HasForeignKey(pol => pol.ProductID)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // PurchaseOrderAttachment configuration
            modelBuilder.Entity<PurchaseOrderAttachment>(entity =>
            {
                entity.ToTable("PurchaseOrderAttachments");
                entity.HasKey(poa => poa.AttachmentID);

                entity.Property(poa => poa.FileName)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(poa => poa.FileType)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(poa => poa.Description)
                    .HasMaxLength(500);

                entity.Property(poa => poa.UploadedBy)
                    .IsRequired()
                    .HasMaxLength(450);

                entity.HasOne(poa => poa.PurchaseOrder)
                    .WithMany(po => po.Attachments)
                    .HasForeignKey(poa => poa.PurchaseOrderID)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // PurchaseOrderStatusHistory configuration
            modelBuilder.Entity<PurchaseOrderStatusHistory>(entity =>
            {
                entity.ToTable("PurchaseOrderStatusHistory");
                entity.HasKey(posh => posh.StatusHistoryID);

                entity.Property(posh => posh.Notes)
                    .HasMaxLength(500);

                entity.Property(posh => posh.ChangedBy)
                    .IsRequired()
                    .HasMaxLength(450);

                entity.HasOne(posh => posh.PurchaseOrder)
                    .WithMany(po => po.StatusHistory)
                    .HasForeignKey(posh => posh.PurchaseOrderID)
                    .OnDelete(DeleteBehavior.Cascade);
            });
            // Apply all configurations from the current assembly
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
