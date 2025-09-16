using Microsoft.EntityFrameworkCore;
using WarehouseManagement.Core.Data;
using WarehouseManagement.Core.Entities;

namespace WarehouseManagement.Core.Repositories
{
    public interface IServicesRepository
    {
        Task<IEnumerable<Service>> GetAllAsync();
        Task<Service?> GetByIdAsync(int id);
        Task<Service?> GetByCodeAsync(string code);
        Task<IEnumerable<Service>> GetByCategoryAsync(int categoryId);
        Task<IEnumerable<Service>> GetBySupplierAsync(int supplierId);
        Task AddAsync(Service service);
        Task UpdateAsync(Service service);
        Task DeleteAsync(Service service);
    }

    public class ServicesRepository : IServicesRepository
    {
        private readonly ApplicationDbContext _context;

        public ServicesRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Service>> GetAllAsync()
        {
            return await _context.Services
                .Include(s => s.Category)
                .Include(s => s.Supplier)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Service?> GetByIdAsync(int id)
        {
            return await _context.Services
                .Include(s => s.Category)
                .Include(s => s.Supplier)
                .FirstOrDefaultAsync(s => s.ServiceID == id);
        }

        public async Task<Service?> GetByCodeAsync(string code)
        {
            return await _context.Services
                .Include(s => s.Category)
                .Include(s => s.Supplier)
                .FirstOrDefaultAsync(s => s.Code == code);
        }

        public async Task<IEnumerable<Service>> GetByCategoryAsync(int categoryId)
        {
            return await _context.Services
                .Include(s => s.Category)
                .Include(s => s.Supplier)
                .Where(s => s.CategoryID == categoryId)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IEnumerable<Service>> GetBySupplierAsync(int supplierId)
        {
            return await _context.Services
                .Include(s => s.Category)
                .Include(s => s.Supplier)
                .Where(s => s.SupplierID == supplierId)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task AddAsync(Service service)
        {
            await _context.Services.AddAsync(service);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Service service)
        {
            service.UpdatedAt = DateTime.UtcNow;
            _context.Services.Update(service);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Service service)
        {
            _context.Services.Remove(service);
            await _context.SaveChangesAsync();
        }
    }
}