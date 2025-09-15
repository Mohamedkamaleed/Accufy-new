using Microsoft.EntityFrameworkCore;
using WarehouseManagement.Core.Entities;

namespace WarehouseManagement.Core.Data.Seeding
{
    public interface IDataSeeder
    {
        Task SeedAsync(ApplicationDbContext context, CancellationToken cancellationToken = default);
    }
    public class WarehouseDataSeeder : IDataSeeder
    {
        public async Task SeedAsync(ApplicationDbContext context, CancellationToken cancellationToken = default)
        {
            // Use a transaction to ensure all operations succeed or fail together
            using var transaction = await context.Database.BeginTransactionAsync(cancellationToken);

            try
            {
                // Check if a primary warehouse already exists
                var hasPrimaryWarehouse = await context.Warehouses
                    .AnyAsync(w => w.IsPrimary, cancellationToken);

                if (!hasPrimaryWarehouse)
                {
                    // Create primary warehouse if none exists
                    var primaryWarehouse = new Warehouse(
                        "Main Warehouse",
                        "123 Main St, City, State",
                        true,
                        true
                    );

                    await context.Warehouses.AddAsync(primaryWarehouse, cancellationToken);
                    await context.SaveChangesAsync(cancellationToken);
                }

                // Check if secondary warehouses exist
                var warehouseNames = await context.Warehouses
                    .Select(w => w.Name)
                    .ToListAsync(cancellationToken);

                if (!warehouseNames.Contains("West Coast Distribution"))
                {
                    var secondaryWarehouse = new Warehouse(
                        "West Coast Distribution",
                        "456 Ocean Ave, Los Angeles, CA",
                        true,
                        false
                    );

                    await context.Warehouses.AddAsync(secondaryWarehouse, cancellationToken);
                }

                if (!warehouseNames.Contains("Old Storage"))
                {
                    var inactiveWarehouse = new Warehouse(
                        "Old Storage",
                        "789 Legacy Rd, Old Town",
                        false,
                        false
                    );

                    await context.Warehouses.AddAsync(inactiveWarehouse, cancellationToken);
                }

                await context.SaveChangesAsync(cancellationToken);
                await transaction.CommitAsync(cancellationToken);
            }
            catch
            {
                await transaction.RollbackAsync(cancellationToken);
                throw;
            }
        }
    }

    public class CategoryDataSeeder : IDataSeeder
    {
        public async Task SeedAsync(ApplicationDbContext context, CancellationToken cancellationToken = default)
        {
            // Check if specific categories already exist by name to avoid duplicates
            var existingCategories = await context.Categories
                .Select(c => c.Name)
                .ToListAsync(cancellationToken);

            var categoriesToAdd = new List<Category>();

            if (!existingCategories.Contains("Electronics"))
            {
                categoriesToAdd.Add(new Category
                {
                    Name = "Electronics",
                    Description = "Electronic products"
                });
            }

            if (!existingCategories.Contains("Furniture"))
            {
                categoriesToAdd.Add(new Category
                {
                    Name = "Furniture",
                    Description = "Office and home furniture"
                });
            }

            if (categoriesToAdd.Any())
            {
                await context.Categories.AddRangeAsync(categoriesToAdd, cancellationToken);
                await context.SaveChangesAsync(cancellationToken);
            }

            // Now set up parent-child relationships
            var allCategories = await context.Categories.ToListAsync(cancellationToken);
            var electronics = allCategories.FirstOrDefault(c => c.Name == "Electronics");
            var furniture = allCategories.FirstOrDefault(c => c.Name == "Furniture");

            // Add child categories if they don't exist
            if (electronics != null && !allCategories.Any(c => c.Name == "Computers"))
            {
                context.Categories.Add(new Category
                {
                    Name = "Computers",
                    Description = "Computer systems and accessories",
                    ParentCategoryID = electronics.CategoryID
                });
            }

            if (electronics != null && !allCategories.Any(c => c.Name == "Smartphones"))
            {
                context.Categories.Add(new Category
                {
                    Name = "Smartphones",
                    Description = "Mobile phones",
                    ParentCategoryID = electronics.CategoryID
                });
            }

            if (furniture != null && !allCategories.Any(c => c.Name == "Office Chairs"))
            {
                context.Categories.Add(new Category
                {
                    Name = "Office Chairs",
                    Description = "Chairs for office use",
                    ParentCategoryID = furniture.CategoryID
                });
            }

            await context.SaveChangesAsync(cancellationToken);
        }
    }
    public class BrandDataSeeder : IDataSeeder
    {
        public async Task SeedAsync(ApplicationDbContext context, CancellationToken cancellationToken = default)
        {
            // Check if specific brands already exist by name to avoid duplicates
            var existingBrands = await context.Brands
                .Select(b => b.Name)
                .ToListAsync(cancellationToken);

            var brandsToAdd = new List<Brand>();

            // Add popular electronics brands
            if (!existingBrands.Contains("Apple"))
            {
                brandsToAdd.Add(new Brand { Name = "Apple" });
            }

            if (!existingBrands.Contains("Samsung"))
            {
                brandsToAdd.Add(new Brand { Name = "Samsung" });
            }

            if (!existingBrands.Contains("Dell"))
            {
                brandsToAdd.Add(new Brand { Name = "Dell" });
            }

            if (!existingBrands.Contains("HP"))
            {
                brandsToAdd.Add(new Brand { Name = "HP" });
            }

            if (!existingBrands.Contains("Lenovo"))
            {
                brandsToAdd.Add(new Brand { Name = "Lenovo" });
            }

            if (!existingBrands.Contains("Sony"))
            {
                brandsToAdd.Add(new Brand { Name = "Sony" });
            }

            // Add furniture brands
            if (!existingBrands.Contains("IKEA"))
            {
                brandsToAdd.Add(new Brand { Name = "IKEA" });
            }

            if (!existingBrands.Contains("Herman Miller"))
            {
                brandsToAdd.Add(new Brand { Name = "Herman Miller" });
            }

            if (!existingBrands.Contains("Steelcase"))
            {
                brandsToAdd.Add(new Brand { Name = "Steelcase" });
            }

            if (brandsToAdd.Any())
            {
                await context.Brands.AddRangeAsync(brandsToAdd, cancellationToken);
                await context.SaveChangesAsync(cancellationToken);
            }
        }
    }

    public class SupplierDataSeeder : IDataSeeder
    {
        public async Task SeedAsync(ApplicationDbContext context, CancellationToken cancellationToken = default)
        {
            // Check if specific suppliers already exist by name to avoid duplicates
            var existingSuppliers = await context.Suppliers
                .Select(s => s.Name)
                .ToListAsync(cancellationToken);

            var suppliersToAdd = new List<Supplier>();

            // Add electronics suppliers
            if (!existingSuppliers.Contains("Tech Distributors Inc."))
            {
                suppliersToAdd.Add(new Supplier { Name = "Tech Distributors Inc." });
            }

            if (!existingSuppliers.Contains("Global Electronics Supply"))
            {
                suppliersToAdd.Add(new Supplier { Name = "Global Electronics Supply" });
            }

            if (!existingSuppliers.Contains("Mobile Device Wholesalers"))
            {
                suppliersToAdd.Add(new Supplier { Name = "Mobile Device Wholesalers" });
            }

            if (!existingSuppliers.Contains("Computer Parts Unlimited"))
            {
                suppliersToAdd.Add(new Supplier { Name = "Computer Parts Unlimited" });
            }

            // Add furniture suppliers
            if (!existingSuppliers.Contains("Office Furniture Solutions"))
            {
                suppliersToAdd.Add(new Supplier { Name = "Office Furniture Solutions" });
            }

            if (!existingSuppliers.Contains("Home & Office Decor Ltd."))
            {
                suppliersToAdd.Add(new Supplier { Name = "Home & Office Decor Ltd." });
            }

            if (!existingSuppliers.Contains("Ergonomic Workspace Suppliers"))
            {
                suppliersToAdd.Add(new Supplier { Name = "Ergonomic Workspace Suppliers" });
            }

            // Add general suppliers
            if (!existingSuppliers.Contains("General Wholesale Distributors"))
            {
                suppliersToAdd.Add(new Supplier { Name = "General Wholesale Distributors" });
            }

            if (!existingSuppliers.Contains("Regional Supply Chain Partners"))
            {
                suppliersToAdd.Add(new Supplier { Name = "Regional Supply Chain Partners" });
            }

            if (suppliersToAdd.Any())
            {
                await context.Suppliers.AddRangeAsync(suppliersToAdd, cancellationToken);
                await context.SaveChangesAsync(cancellationToken);
            }
        }
    }
    public class CompositeDataSeeder : IDataSeeder
    {
        private readonly IEnumerable<IDataSeeder> _seeders;

        public CompositeDataSeeder(IEnumerable<IDataSeeder> seeders)
        {
            _seeders = seeders;
        }

        public async Task SeedAsync(ApplicationDbContext context, CancellationToken cancellationToken = default)
        {
            foreach (var seeder in _seeders)
            {
                await seeder.SeedAsync(context, cancellationToken);
            }
        }
    }
}
