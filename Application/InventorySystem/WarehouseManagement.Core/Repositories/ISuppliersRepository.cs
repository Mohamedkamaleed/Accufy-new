using Microsoft.EntityFrameworkCore;
using WarehouseManagement.Core.Data;
using WarehouseManagement.Core.Entities;

namespace WarehouseManagement.Core.Repositories
{
    public interface ISuppliersRepository
    {
        Task<IEnumerable<Supplier>> GetAllAsync();
        Task<Supplier?> GetByIdAsync(int id);
        Task<Supplier?> GetByNameAsync(string name);
        Task AddAsync(Supplier supplier);
        Task UpdateAsync(Supplier supplier);
        Task DeleteAsync(Supplier supplier);
    }
    public class SuppliersRepository : ISuppliersRepository
    {
        private readonly ApplicationDbContext _context;

        public SuppliersRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Supplier>> GetAllAsync()
        {
            return await _context.Suppliers
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Supplier?> GetByIdAsync(int id)
        {
            return await _context.Suppliers
                .FirstOrDefaultAsync(c => c.SupplierID == id);
        }

        public async Task<Supplier?> GetByNameAsync(string name)
        {
            return await _context.Suppliers
                .FirstOrDefaultAsync(c => c.Name == name);
        }

        public async Task AddAsync(Supplier supplier)
        {
            await _context.Suppliers.AddAsync(supplier);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Supplier supplier)
        {
            _context.Suppliers.Update(supplier);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Supplier supplier)
        {
            // Check if supplier has child categories
            var hasChildren = await _context.Suppliers
                .AnyAsync(c => c.SupplierID == supplier.SupplierID);

            if (hasChildren)
            {
                throw new InvalidOperationException("Cannot delete a supplier that has child categories.");
            }

            _context.Suppliers.Remove(supplier);
            await _context.SaveChangesAsync();
        }
    }

}
