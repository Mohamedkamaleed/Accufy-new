using Microsoft.EntityFrameworkCore;
using WarehouseManagement.Core.Data;
using WarehouseManagement.Core.Entities;

namespace WarehouseManagement.Core.Repositories
{
    public interface ITaxProfilesRepository
    {
        Task<IEnumerable<TaxProfile>> GetAllAsync();
        Task<TaxProfile?> GetByIdAsync(int id);
        Task<TaxProfile?> GetByNameAsync(string name);
        Task<TaxProfile?> GetByIdWithTaxesAsync(int id);
        Task AddAsync(TaxProfile taxProfile);
        Task UpdateAsync(TaxProfile taxProfile);
        Task DeleteAsync(TaxProfile taxProfile);
    }
    public class TaxProfilesRepository : ITaxProfilesRepository
    {
        private readonly ApplicationDbContext _context;

        public TaxProfilesRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TaxProfile>> GetAllAsync()
        {
            return await _context.TaxProfiles
                .Include(tp => tp.TaxProfileTaxes)
                .ThenInclude(tpt => tpt.DefaultTax)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<TaxProfile?> GetByIdAsync(int id)
        {
            return await _context.TaxProfiles
                .FirstOrDefaultAsync(tp => tp.TaxProfileID == id);
        }

        public async Task<TaxProfile?> GetByNameAsync(string name)
        {
            return await _context.TaxProfiles
                .FirstOrDefaultAsync(tp => tp.Name == name);
        }

        public async Task<TaxProfile?> GetByIdWithTaxesAsync(int id)
        {
            return await _context.TaxProfiles
                .Include(tp => tp.TaxProfileTaxes)
                .ThenInclude(tpt => tpt.DefaultTax)
                .FirstOrDefaultAsync(tp => tp.TaxProfileID == id);
        }

        public async Task AddAsync(TaxProfile taxProfile)
        {
            await _context.TaxProfiles.AddAsync(taxProfile);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(TaxProfile taxProfile)
        {
            _context.TaxProfiles.Update(taxProfile);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(TaxProfile taxProfile)
        {
            // Check if tax profile is used by any products
            var isUsed = await _context.ProductTaxProfiles
                .AnyAsync(ptp => ptp.TaxProfileID == taxProfile.TaxProfileID);

            if (isUsed)
            {
                throw new InvalidOperationException("Cannot delete a tax profile that is used by products.");
            }

            _context.TaxProfiles.Remove(taxProfile);
            await _context.SaveChangesAsync();
        }
    }
}
