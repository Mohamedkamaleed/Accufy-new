using Microsoft.EntityFrameworkCore;
using WarehouseManagement.Core.Data;
using WarehouseManagement.Core.Entities;

namespace WarehouseManagement.Core.Repositories
{
    public interface IItemGroupsRepository
    {
        Task<IEnumerable<ItemGroup>> GetAllAsync();
        Task<ItemGroup?> GetByIdAsync(int id);
        Task<ItemGroup?> GetByNameAsync(string name);
        Task<IEnumerable<ItemGroup>> GetByCategoryAsync(int categoryId);
        Task<IEnumerable<ItemGroup>> GetByBrandAsync(int brandId);
        Task AddAsync(ItemGroup itemGroup);
        Task UpdateAsync(ItemGroup itemGroup);
        Task DeleteAsync(ItemGroup itemGroup);
    }

    public class ItemGroupsRepository : IItemGroupsRepository
    {
        private readonly ApplicationDbContext _context;

        public ItemGroupsRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ItemGroup>> GetAllAsync()
        {
            return await _context.ItemGroups
                .Include(g => g.Category)
                .Include(g => g.Brand)
                .Include(g => g.Products)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<ItemGroup?> GetByIdAsync(int id)
        {
            return await _context.ItemGroups
                .Include(g => g.Category)
                .Include(g => g.Brand)
                .Include(g => g.Products)
                .FirstOrDefaultAsync(g => g.GroupID == id);
        }

        public async Task<ItemGroup?> GetByNameAsync(string name)
        {
            return await _context.ItemGroups
                .Include(g => g.Category)
                .Include(g => g.Brand)
                .FirstOrDefaultAsync(g => g.Name == name);
        }

        public async Task<IEnumerable<ItemGroup>> GetByCategoryAsync(int categoryId)
        {
            return await _context.ItemGroups
                .Include(g => g.Category)
                .Include(g => g.Brand)
                .Where(g => g.CategoryID == categoryId)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IEnumerable<ItemGroup>> GetByBrandAsync(int brandId)
        {
            return await _context.ItemGroups
                .Include(g => g.Category)
                .Include(g => g.Brand)
                .Where(g => g.BrandID == brandId)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task AddAsync(ItemGroup itemGroup)
        {
            await _context.ItemGroups.AddAsync(itemGroup);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(ItemGroup itemGroup)
        {
            _context.ItemGroups.Update(itemGroup);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(ItemGroup itemGroup)
        {
            // Check if item group has products
            var hasProducts = await _context.Products
                .AnyAsync(p => EF.Property<int>(p, "GroupID") == itemGroup.GroupID);

            if (hasProducts)
            {
                throw new InvalidOperationException("Cannot delete an item group that has products.");
            }

            _context.ItemGroups.Remove(itemGroup);
            await _context.SaveChangesAsync();
        }
    }
}