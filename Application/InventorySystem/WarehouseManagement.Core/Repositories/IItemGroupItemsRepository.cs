using Microsoft.EntityFrameworkCore;
using WarehouseManagement.Core.Data;
using WarehouseManagement.Core.Entities;

namespace WarehouseManagement.Core.Repositories
{
    public interface IItemGroupItemsRepository
    {
        Task<IEnumerable<ItemGroupItem>> GetAllAsync();
        Task<ItemGroupItem?> GetByIdAsync(int id);
        Task<IEnumerable<ItemGroupItem>> GetByGroupIdAsync(int groupId);
        Task<IEnumerable<ItemGroupItem>> GetByProductIdAsync(int productId);
        Task<ItemGroupItem?> GetByGroupAndProductAsync(int groupId, int productId);
        Task AddAsync(ItemGroupItem itemGroupItem);
        Task UpdateAsync(ItemGroupItem itemGroupItem);
        Task DeleteAsync(ItemGroupItem itemGroupItem);
    }

    public class ItemGroupItemsRepository : IItemGroupItemsRepository
    {
        private readonly ApplicationDbContext _context;

        public ItemGroupItemsRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ItemGroupItem>> GetAllAsync()
        {
            return await _context.ItemGroupItems
                .Include(i => i.ItemGroup)
                .Include(i => i.Product)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<ItemGroupItem?> GetByIdAsync(int id)
        {
            return await _context.ItemGroupItems
                .Include(i => i.ItemGroup)
                .Include(i => i.Product)
                .FirstOrDefaultAsync(i => i.GroupItemID == id);
        }

        public async Task<IEnumerable<ItemGroupItem>> GetByGroupIdAsync(int groupId)
        {
            return await _context.ItemGroupItems
                .Include(i => i.ItemGroup)
                .Include(i => i.Product)
                .Where(i => i.GroupID == groupId)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IEnumerable<ItemGroupItem>> GetByProductIdAsync(int productId)
        {
            return await _context.ItemGroupItems
                .Include(i => i.ItemGroup)
                .Include(i => i.Product)
                .Where(i => i.ProductID == productId)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<ItemGroupItem?> GetByGroupAndProductAsync(int groupId, int productId)
        {
            return await _context.ItemGroupItems
                .Include(i => i.ItemGroup)
                .Include(i => i.Product)
                .FirstOrDefaultAsync(i => i.GroupID == groupId && i.ProductID == productId);
        }

        public async Task AddAsync(ItemGroupItem itemGroupItem)
        {
            await _context.ItemGroupItems.AddAsync(itemGroupItem);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(ItemGroupItem itemGroupItem)
        {
            _context.ItemGroupItems.Update(itemGroupItem);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(ItemGroupItem itemGroupItem)
        {
            _context.ItemGroupItems.Remove(itemGroupItem);
            await _context.SaveChangesAsync();
        }
    }
}
