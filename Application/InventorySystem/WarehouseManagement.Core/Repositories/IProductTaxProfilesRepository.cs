using Microsoft.EntityFrameworkCore;
using WarehouseManagement.Core.Data;
using WarehouseManagement.Core.Entities;

namespace WarehouseManagement.Core.Repositories
{
    public interface IProductTaxProfilesRepository
    {
        Task<IEnumerable<ProductTaxProfile>> GetAllAsync();
        Task<ProductTaxProfile?> GetByProductAndTaxProfileAsync(int productId, int taxProfileId);
        Task<IEnumerable<ProductTaxProfile>> GetByProductIdAsync(int productId);
        Task<IEnumerable<ProductTaxProfile>> GetByTaxProfileIdAsync(int taxProfileId);
        Task<ProductTaxProfile?> GetPrimaryTaxProfileForProductAsync(int productId);
        Task AddAsync(ProductTaxProfile productTaxProfile);
        Task UpdateAsync(ProductTaxProfile productTaxProfile);
        Task DeleteAsync(int productId, int taxProfileId);
        Task SetPrimaryTaxProfileAsync(int productId, int taxProfileId);
        Task<ProductTaxProfile> GetProductTaxProfileByIdAsync(int productId, int taxProfileId);
    }

    public class ProductTaxProfilesRepository : IProductTaxProfilesRepository
    {
        private readonly ApplicationDbContext _context;

        public ProductTaxProfilesRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ProductTaxProfile>> GetAllAsync()
        {
            return await _context.ProductTaxProfiles
                .Include(pt => pt.Product)
                .Include(pt => pt.TaxProfile)
                .AsNoTracking()
                .ToListAsync();
        }
        public async Task<ProductTaxProfile> GetProductTaxProfileByIdAsync(int productId, int taxProfileId)
        {
                return await _context.ProductTaxProfiles
                    .Include(ptp => ptp.Product)
                    .Include(ptp => ptp.TaxProfile)
                    .FirstOrDefaultAsync(ptp => ptp.ProductID == productId && ptp.TaxProfileID == taxProfileId);
        }
        public async Task<ProductTaxProfile?> GetByProductAndTaxProfileAsync(int productId, int taxProfileId)
        {
            return await _context.ProductTaxProfiles
                .Include(pt => pt.Product)
                .Include(pt => pt.TaxProfile)
                .FirstOrDefaultAsync(pt => pt.ProductID == productId && pt.TaxProfileID == taxProfileId);
        }

        public async Task<IEnumerable<ProductTaxProfile>> GetByProductIdAsync(int productId)
        {
            return await _context.ProductTaxProfiles
                .Include(pt => pt.Product)
                .Include(pt => pt.TaxProfile)
                .Where(pt => pt.ProductID == productId)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IEnumerable<ProductTaxProfile>> GetByTaxProfileIdAsync(int taxProfileId)
        {
            return await _context.ProductTaxProfiles
                .Include(pt => pt.Product)
                .Include(pt => pt.TaxProfile)
                .Where(pt => pt.TaxProfileID == taxProfileId)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<ProductTaxProfile?> GetPrimaryTaxProfileForProductAsync(int productId)
        {
            return await _context.ProductTaxProfiles
                .Include(pt => pt.Product)
                .Include(pt => pt.TaxProfile)
                .FirstOrDefaultAsync(pt => pt.ProductID == productId && pt.IsPrimary);
        }

        public async Task AddAsync(ProductTaxProfile productTaxProfile)
        {
            await _context.ProductTaxProfiles.AddAsync(productTaxProfile);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(ProductTaxProfile productTaxProfile)
        {
            _context.ProductTaxProfiles.Update(productTaxProfile);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int productId, int taxProfileId)
        {
            var productTaxProfile = await GetByProductAndTaxProfileAsync(productId, taxProfileId);
            if (productTaxProfile != null)
            {
                _context.ProductTaxProfiles.Remove(productTaxProfile);
                await _context.SaveChangesAsync();
            }
        }

        public async Task SetPrimaryTaxProfileAsync(int productId, int taxProfileId)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                // First, set all tax profiles for this product as non-primary
                var existingProfiles = await _context.ProductTaxProfiles
                    .Where(pt => pt.ProductID == productId)
                    .ToListAsync();

                foreach (var profile in existingProfiles)
                {
                    profile.IsPrimary = false;
                }

                // Then set the specified tax profile as primary
                var targetProfile = await _context.ProductTaxProfiles
                    .FirstOrDefaultAsync(pt => pt.ProductID == productId && pt.TaxProfileID == taxProfileId);

                if (targetProfile != null)
                {
                    targetProfile.IsPrimary = true;
                }
                else
                {
                    // If the relationship doesn't exist, create it
                    var newProfile = new ProductTaxProfile
                    {
                        ProductID = productId,
                        TaxProfileID = taxProfileId,
                        IsPrimary = true
                    };

                    await _context.ProductTaxProfiles.AddAsync(newProfile);
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