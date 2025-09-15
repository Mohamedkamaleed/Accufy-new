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
