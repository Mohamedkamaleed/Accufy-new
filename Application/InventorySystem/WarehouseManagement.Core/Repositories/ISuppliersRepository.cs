using Microsoft.EntityFrameworkCore;
using WarehouseManagement.Core.Data;
using WarehouseManagement.Core.Entities;

namespace WarehouseManagement.Core.Repositories
{
    public interface ISuppliersRepository
    {
        Task<IEnumerable<Supplier>> GetAllAsync();
        Task<Supplier?> GetByIdAsync(int id);
        Task<Supplier?> GetByIdWithDetailsAsync(int id);
        Task<Supplier?> GetByBusinessNameAsync(string businessName);
        Task<Supplier?> GetBySupplierNumberAsync(string supplierNumber);
        Task<Supplier?> GetLastSupplierAsync();
        Task<IEnumerable<Supplier>> SearchAsync(string searchTerm);
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
                .OrderBy(s => s.BusinessName)
                .ToListAsync();
        }

        public async Task<Supplier?> GetByIdAsync(int id)
        {
            return await _context.Suppliers
                .FirstOrDefaultAsync(s => s.SupplierID == id);
        }

        public async Task<Supplier?> GetByIdWithDetailsAsync(int id)
        {
            return await _context.Suppliers
                .Include(s => s.Contacts)
                .Include(s => s.Attachments)
                .Include(s => s.PurchaseOrders)
                .FirstOrDefaultAsync(s => s.SupplierID == id);
        }

        public async Task<Supplier?> GetByBusinessNameAsync(string businessName)
        {
            return await _context.Suppliers
                .FirstOrDefaultAsync(s => s.BusinessName == businessName);
        }

        public async Task<Supplier?> GetBySupplierNumberAsync(string supplierNumber)
        {
            return await _context.Suppliers
                .FirstOrDefaultAsync(s => s.SupplierNumber == supplierNumber);
        }

        public async Task<Supplier?> GetLastSupplierAsync()
        {
            return await _context.Suppliers
                .OrderByDescending(s => s.SupplierID)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Supplier>> SearchAsync(string searchTerm)
        {
            return await _context.Suppliers
                .Where(s => s.BusinessName.Contains(searchTerm) ||
                           s.SupplierNumber.Contains(searchTerm) ||
                           (s.FirstName + " " + s.LastName).Contains(searchTerm) ||
                           s.Email.Contains(searchTerm) ||
                           s.Telephone.Contains(searchTerm) ||
                           s.City.Contains(searchTerm) ||
                           s.Country.Contains(searchTerm))
                .AsNoTracking()
                .OrderBy(s => s.BusinessName)
                .ToListAsync();
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
            _context.Suppliers.Remove(supplier);
            await _context.SaveChangesAsync();
        }
    }
}