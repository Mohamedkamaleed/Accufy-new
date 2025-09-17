using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using WarehouseManagement.Core.Common;
using WarehouseManagement.Core.Entities;
using WarehouseManagement.Core.Repositories;
using WarehouseManagement.Core.ViewModels;

namespace WarehouseManagement.Core.Services
{
    public interface ISuppliersService
    {
        Task<Result<Supplier>> CreateSupplierAsync(SupplierCreateViewModel model, string userId);
        Task<Result<Supplier>> UpdateSupplierAsync(int id, SupplierEditViewModel model, string userId);
        Task<Result> DeleteSupplierAsync(int id);
        Task<Supplier?> GetSupplierByIdAsync(int id);
        Task<SupplierDetailViewModel?> GetSupplierDetailAsync(int id);
        Task<IEnumerable<SupplierListViewModel>> GetAllSuppliersAsync();
        Task<IEnumerable<SupplierListViewModel>> SearchSuppliersAsync(string searchTerm);
        Task<string> GenerateSupplierNumberAsync();
        Task<Supplier?> GetSupplierByNumberAsync(string supplierNumber);
        Task<Supplier?> GetSupplierByBusinessNameAsync(string businessName);

    }

    public class SuppliersService : ISuppliersService
    {
        private readonly ISuppliersRepository _repository;
        private readonly UserManager<IdentityUser> _userManager;

        public SuppliersService(ISuppliersRepository repository, UserManager<IdentityUser> userManager)
        {
            _repository = repository;
            _userManager = userManager;
        }
        public async Task<Supplier?> GetSupplierByBusinessNameAsync(string businessName)
        {
            if (string.IsNullOrWhiteSpace(businessName))
            {
                throw new ArgumentException("Business name cannot be null or empty.", nameof(businessName));
            }

            return await _repository.GetByBusinessNameAsync(businessName);
        }

        public async Task<Supplier?> GetSupplierByNumberAsync(string supplierNumber)
        {
            if (string.IsNullOrWhiteSpace(supplierNumber))
            {
                throw new ArgumentException("Supplier number cannot be null or empty.", nameof(supplierNumber));
            }

            return await _repository.GetBySupplierNumberAsync(supplierNumber);
        }
        public async Task<Result<Supplier>> CreateSupplierAsync(SupplierCreateViewModel model, string userId)
        {
            // Check if business name already exists
            var existingByName = await _repository.GetByBusinessNameAsync(model.BusinessName);
            if (existingByName != null)
                return Result<Supplier>.Failure("Business name already exists");

            // Generate supplier number
            var supplierNumber = await GenerateSupplierNumberAsync();

            // Get user info for audit
            var user = await _userManager.FindByIdAsync(userId);
            var userName = user?.UserName ?? "System";

            var supplier = new Supplier
            {
                BusinessName = model.BusinessName,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Telephone = model.Telephone,
                Mobile = model.Mobile,
                StreetAddress1 = model.StreetAddress1,
                StreetAddress2 = model.StreetAddress2,
                City = model.City,
                State = model.State,
                PostalCode = model.PostalCode,
                Country = model.Country,
                CommercialRegistration = model.CommercialRegistration,
                TaxID = model.TaxID,
                SupplierNumber = supplierNumber,
                Currency = model.Currency,
                OpeningBalance = model.OpeningBalance,
                OpeningBalanceDate = model.OpeningBalanceDate,
                Email = model.Email,
                Notes = model.Notes,
                IsActive = true,
                CreatedBy = userName,
                CreatedAt = DateTime.UtcNow
            };

            // Add contacts
            foreach (var contactModel in model.Contacts)
            {
                supplier.Contacts.Add(new SupplierContact
                {
                    Name = contactModel.Name,
                    Phone = contactModel.Phone,
                    Email = contactModel.Email,
                    Position = contactModel.Position,
                    IsPrimary = contactModel.IsPrimary
                });
            }

            await _repository.AddAsync(supplier);

            return Result<Supplier>.Success(supplier);
        }

        public async Task<Result<Supplier>> UpdateSupplierAsync(int id, SupplierEditViewModel model, string userId)
        {
            var supplier = await _repository.GetByIdAsync(id);
            if (supplier == null)
                return Result<Supplier>.Failure("Supplier not found");

            // Check if business name already exists (excluding current supplier)
            var existingByName = await _repository.GetByBusinessNameAsync(model.BusinessName);
            if (existingByName != null && existingByName.SupplierID != id)
                return Result<Supplier>.Failure("Business name already exists");

            // Get user info for audit
            var user = await _userManager.FindByIdAsync(userId);
            var userName = user?.UserName ?? "System";

            supplier.BusinessName = model.BusinessName;
            supplier.FirstName = model.FirstName;
            supplier.LastName = model.LastName;
            supplier.Telephone = model.Telephone;
            supplier.Mobile = model.Mobile;
            supplier.StreetAddress1 = model.StreetAddress1;
            supplier.StreetAddress2 = model.StreetAddress2;
            supplier.City = model.City;
            supplier.State = model.State;
            supplier.PostalCode = model.PostalCode;
            supplier.Country = model.Country;
            supplier.CommercialRegistration = model.CommercialRegistration;
            supplier.TaxID = model.TaxID;
            supplier.Currency = model.Currency;
            supplier.OpeningBalance = model.OpeningBalance;
            supplier.OpeningBalanceDate = model.OpeningBalanceDate;
            supplier.Email = model.Email;
            supplier.Notes = model.Notes;
            supplier.IsActive = model.IsActive;
            supplier.UpdatedBy = userName;
            supplier.UpdatedAt = DateTime.UtcNow;

            // Update contacts (simplified implementation)
            // In a real application, you'd want to handle contact updates more carefully
            supplier.Contacts.Clear();
            foreach (var contactModel in model.Contacts)
            {
                supplier.Contacts.Add(new SupplierContact
                {
                    Name = contactModel.Name,
                    Phone = contactModel.Phone,
                    Email = contactModel.Email,
                    Position = contactModel.Position,
                    IsPrimary = contactModel.IsPrimary
                });
            }

            await _repository.UpdateAsync(supplier);

            return Result<Supplier>.Success(supplier);
        }

        public async Task<Result> DeleteSupplierAsync(int id)
        {
            var supplier = await _repository.GetByIdAsync(id);
            if (supplier == null)
                return Result.Failure("Supplier not found");

            // Check if supplier has purchase orders
            if (supplier.PurchaseOrders.Any())
                return Result.Failure("Cannot delete supplier with associated purchase orders");

            await _repository.DeleteAsync(supplier);

            return Result.Success();
        }

        public async Task<Supplier?> GetSupplierByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<SupplierDetailViewModel?> GetSupplierDetailAsync(int id)
        {
            var supplier = await _repository.GetByIdWithDetailsAsync(id);
            if (supplier == null)
                return null;

            return new SupplierDetailViewModel
            {
                SupplierID = supplier.SupplierID,
                BusinessName = supplier.BusinessName,
                SupplierNumber = supplier.SupplierNumber,
                FirstName = supplier.FirstName,
                LastName = supplier.LastName,
                Telephone = supplier.Telephone,
                Mobile = supplier.Mobile,
                StreetAddress1 = supplier.StreetAddress1,
                StreetAddress2 = supplier.StreetAddress2,
                City = supplier.City,
                State = supplier.State,
                PostalCode = supplier.PostalCode,
                Country = supplier.Country,
                CommercialRegistration = supplier.CommercialRegistration,
                TaxID = supplier.TaxID,
                Currency = supplier.Currency,
                OpeningBalance = supplier.OpeningBalance,
                OpeningBalanceDate = supplier.OpeningBalanceDate,
                Email = supplier.Email,
                Notes = supplier.Notes,
                IsActive = supplier.IsActive,
                CreatedAt = supplier.CreatedAt,
                UpdatedAt = supplier.UpdatedAt,
                CreatedBy = supplier.CreatedBy,
                UpdatedBy = supplier.UpdatedBy,
                Contacts = supplier.Contacts.Select(c => new SupplierContactViewModel
                {
                    ContactID = c.ContactID,
                    SupplierID = c.SupplierID,
                    Name = c.Name,
                    Phone = c.Phone,
                    Email = c.Email,
                    Position = c.Position,
                    IsPrimary = c.IsPrimary
                }).ToList(),
                Attachments = supplier.Attachments.Select(a => new SupplierAttachmentViewModel
                {
                    AttachmentID = a.AttachmentID,
                    SupplierID = a.SupplierID,
                    FileName = a.FileName,
                    FileType = a.FileType,
                    FileSize = a.FileSize,
                    Description = a.Description,
                    UploadedAt = a.UploadedAt,
                    UploadedBy = a.UploadedBy
                }).ToList()
            };
        }

        public async Task<IEnumerable<SupplierListViewModel>> GetAllSuppliersAsync()
        {
            var suppliers = await _repository.GetAllAsync();
            return suppliers.Select(s => new SupplierListViewModel
            {
                SupplierID = s.SupplierID,
                BusinessName = s.BusinessName,
                SupplierNumber = s.SupplierNumber,
                ContactName = $"{s.FirstName} {s.LastName}".Trim(),
                Email = s.Email,
                Telephone = s.Telephone,
                City = s.City,
                Country = s.Country,
                IsActive = s.IsActive,
                CreatedAt = s.CreatedAt
            });
        }

        public async Task<IEnumerable<SupplierListViewModel>> SearchSuppliersAsync(string searchTerm)
        {
            var suppliers = await _repository.SearchAsync(searchTerm);
            return suppliers.Select(s => new SupplierListViewModel
            {
                SupplierID = s.SupplierID,
                BusinessName = s.BusinessName,
                SupplierNumber = s.SupplierNumber,
                ContactName = $"{s.FirstName} {s.LastName}".Trim(),
                Email = s.Email,
                Telephone = s.Telephone,
                City = s.City,
                Country = s.Country,
                IsActive = s.IsActive,
                CreatedAt = s.CreatedAt
            });
        }

        public async Task<string> GenerateSupplierNumberAsync()
        {
            var lastSupplier = await _repository.GetLastSupplierAsync();
            var lastNumber = lastSupplier?.SupplierNumber;

            if (string.IsNullOrEmpty(lastNumber) || !lastNumber.StartsWith("SUP"))
            {
                return "SUP00001";
            }

            if (int.TryParse(lastNumber.Substring(3), out int number))
            {
                return $"SUP{(number + 1).ToString("D5")}";
            }

            return "SUP00001";
        }
    }
}