using Microsoft.EntityFrameworkCore;
using WarehouseManagement.Core.Data;
using WarehouseManagement.Core.Entities;

namespace WarehouseManagement.Core.Repositories
{
    public interface IDefaultTaxesRepository
    {
        Task<IEnumerable<DefaultTax>> GetAllAsync();
        Task<DefaultTax?> GetByIdAsync(int id);
        Task<DefaultTax?> GetByNameAsync(string name);
        Task AddAsync(DefaultTax defaultTax);
        Task UpdateAsync(DefaultTax defaultTax);
        Task DeleteAsync(DefaultTax defaultTax);
    }
    public class DefaultTaxesRepository : IDefaultTaxesRepository
    {
        private readonly ApplicationDbContext _context;

        public DefaultTaxesRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<DefaultTax>> GetAllAsync()
        {
            return await _context.DefaultTaxes
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<DefaultTax?> GetByIdAsync(int id)
        {
            return await _context.DefaultTaxes
                .FirstOrDefaultAsync(t => t.TaxID == id);
        }

        public async Task<DefaultTax?> GetByNameAsync(string name)
        {
            return await _context.DefaultTaxes
                .FirstOrDefaultAsync(t => t.Name == name);
        }

        public async Task AddAsync(DefaultTax defaultTax)
        {
            await _context.DefaultTaxes.AddAsync(defaultTax);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(DefaultTax defaultTax)
        {
            _context.DefaultTaxes.Update(defaultTax);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(DefaultTax defaultTax)
        {
            // Check if tax is used in any tax profile
            var isUsed = await _context.TaxProfileTaxes
                .AnyAsync(tpt => tpt.TaxID == defaultTax.TaxID);

            if (isUsed)
            {
                throw new InvalidOperationException("Cannot delete a tax that is used in tax profiles.");
            }

            _context.DefaultTaxes.Remove(defaultTax);
            await _context.SaveChangesAsync();
        }
    }
}
