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
            // Check if data already exists
            if (await context.Warehouses.AnyAsync(cancellationToken))
            {
                return; // Database has been seeded
            }

            // Create primary warehouse
            var primaryWarehouse = new Warehouse("Main Warehouse", "123 Main St, City, State", true, true);

            // Create additional warehouses
            var secondaryWarehouse = new Warehouse("West Coast Distribution", "456 Ocean Ave, Los Angeles, CA", true, false);
            var inactiveWarehouse = new Warehouse("Old Storage", "789 Legacy Rd, Old Town", false, false);

            // Add warehouses to context
            context.Warehouses.AddRange(primaryWarehouse, secondaryWarehouse, inactiveWarehouse);

            // Save changes
            await context.SaveChangesAsync(cancellationToken);
        }
    }

    public class CategoryDataSeeder : IDataSeeder
    {
        public async Task SeedAsync(ApplicationDbContext context, CancellationToken cancellationToken = default)
        {
            if (await context.Categories.AnyAsync(cancellationToken))
            {
                return;
            }

            var categories = new List<Category>
            {
                new Category { Name = "Electronics", Description = "Electronic products" },
                new Category { Name = "Computers", Description = "Computer systems and accessories", ParentCategoryID = 1 },
                new Category { Name = "Smartphones", Description = "Mobile phones", ParentCategoryID = 1 },
                new Category { Name = "Furniture", Description = "Office and home furniture" },
                new Category { Name = "Office Chairs", Description = "Chairs for office use", ParentCategoryID = 4 }
            };

            // Add categories with explicit IDs for relationship mapping
            for (int i = 0; i < categories.Count; i++)
            {
                categories[i].CategoryID = i + 1;
            }

            context.Categories.AddRange(categories);
            await context.SaveChangesAsync(cancellationToken);
        }
    }
}
