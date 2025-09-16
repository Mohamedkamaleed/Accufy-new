using Microsoft.EntityFrameworkCore;
using WarehouseManagement.Core.Data;
using WarehouseManagement.Core.Entities;

namespace WarehouseManagement.Core.Repositories
{
    public interface ITaxProfileTaxesRepository
    {
        Task<IEnumerable<TaxProfileTax>> GetByTaxProfileIdAsync(int taxProfileId);
        Task AddTaxToProfileAsync(int taxProfileId, int taxId);
        Task RemoveTaxFromProfileAsync(int taxProfileId, int taxId);
        Task<bool> TaxProfileContainsTaxAsync(int taxProfileId, int taxId);
    }
    public class TaxProfileTaxesRepository : ITaxProfileTaxesRepository
    {
        private readonly ApplicationDbContext _context;

        public TaxProfileTaxesRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TaxProfileTax>> GetByTaxProfileIdAsync(int taxProfileId)
        {
            return await _context.TaxProfileTaxes
                .Include(tpt => tpt.DefaultTax)
                .Where(tpt => tpt.TaxProfileID == taxProfileId)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task AddTaxToProfileAsync(int taxProfileId, int taxId)
        {
            var existing = await _context.TaxProfileTaxes
                .FirstOrDefaultAsync(tpt => tpt.TaxProfileID == taxProfileId && tpt.TaxID == taxId);

            if (existing == null)
            {
                var taxProfileTax = new TaxProfileTax
                {
                    TaxProfileID = taxProfileId,
                    TaxID = taxId
                };

                await _context.TaxProfileTaxes.AddAsync(taxProfileTax);
                await _context.SaveChangesAsync();
            }
        }

        public async Task RemoveTaxFromProfileAsync(int taxProfileId, int taxId)
        {
            var taxProfileTax = await _context.TaxProfileTaxes
                .FirstOrDefaultAsync(tpt => tpt.TaxProfileID == taxProfileId && tpt.TaxID == taxId);

            if (taxProfileTax != null)
            {
                _context.TaxProfileTaxes.Remove(taxProfileTax);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> TaxProfileContainsTaxAsync(int taxProfileId, int taxId)
        {
            return await _context.TaxProfileTaxes
                .AnyAsync(tpt => tpt.TaxProfileID == taxProfileId && tpt.TaxID == taxId);
        }
    }
}
