using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using WarehouseManagement.Core.Data;
using WarehouseManagement.Core.Entities;

namespace WarehouseManagement.Core.Repositories
{
    public interface IWarehouseRepository
    {
        Task<IEnumerable<Warehouse>> GetAllAsync();
        Task<Warehouse> GetByIdAsync(int id);
        Task<Warehouse> GetByNameAsync(string name);
        Task<Warehouse> GetPrimaryWarehouseAsync();
        Task<IEnumerable<Warehouse>> GetActiveWarehousesAsync();
        Task<bool> HasTransactionsAsync(int warehouseId); // Placeholder for future logic
        Task AddAsync(Warehouse warehouse);
        Task UpdateAsync(Warehouse warehouse);
        Task DeleteAsync(Warehouse warehouse);
        Task SetPrimaryWarehouseAsync(int id);
    }
    public class WarehouseRepository : IWarehouseRepository
    {
        private readonly ApplicationDbContext _context;

        public WarehouseRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Warehouse>> GetAllAsync() =>
            await _context.Warehouses.AsNoTracking().ToListAsync();

        public async Task<Warehouse> GetByIdAsync(int id) =>
            await _context.Warehouses.FirstOrDefaultAsync(w => w.Id == id);

        public async Task<Warehouse> GetByNameAsync(string name) =>
            await _context.Warehouses.FirstOrDefaultAsync(w => w.Name == name);

        public async Task<Warehouse> GetPrimaryWarehouseAsync() =>
            await _context.Warehouses.FirstOrDefaultAsync(w => w.IsPrimary);

        public async Task<IEnumerable<Warehouse>> GetActiveWarehousesAsync() =>
            await _context.Warehouses.Where(w => w.Status).AsNoTracking().ToListAsync();

        public async Task<bool> HasTransactionsAsync(int warehouseId)
        {
            // Implement actual logic to check for transactions
            // This is a placeholder - you'd need to check related entities
            return await Task.FromResult(false);
        }

        public async Task AddAsync(Warehouse warehouse)
        {
            await _context.Warehouses.AddAsync(warehouse);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Warehouse warehouse)
        {
            _context.Warehouses.Update(warehouse);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Warehouse warehouse)
        {
            _context.Warehouses.Remove(warehouse);
            await _context.SaveChangesAsync();
        }

        public async Task SetPrimaryWarehouseAsync(int id)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // Clear current primary
                var currentPrimary = await _context.Warehouses
                    .Where(w => w.IsPrimary)
                    .ToListAsync();

                foreach (var warehouse in currentPrimary)
                {
                    warehouse.IsPrimary = false;
                }

                // Set new primary
                var newPrimary = await _context.Warehouses.FindAsync(id);
                if (newPrimary != null)
                {
                    newPrimary.IsPrimary = true;
                    newPrimary.Status = true; // Ensure primary is active
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
}
