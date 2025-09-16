using WarehouseManagement.Core.Common;
using WarehouseManagement.Core.Entities;
using WarehouseManagement.Core.Repositories;
using WarehouseManagement.Core.ViewModels;

namespace WarehouseManagement.Core.Services
{
    public interface IServicesService
    {
        Task<Result<Service>> CreateServiceAsync(ServiceCreateViewModel model);
        Task<Result<Service>> UpdateServiceAsync(int id, ServiceEditViewModel model);
        Task<Result> DeleteServiceAsync(int id);
        Task<Service?> GetServiceByIdAsync(int id);
        Task<Service?> GetServiceByCodeAsync(string code);
        Task<IEnumerable<Service>> GetAllServicesAsync();
        Task<IEnumerable<Service>> GetServicesByCategoryAsync(int categoryId);
        Task<IEnumerable<Service>> GetServicesBySupplierAsync(int supplierId);
    }

    public class ServicesService : IServicesService
    {
        private readonly IServicesRepository _repository;
        private readonly ICategoryRepository _categoriesRepository;
        private readonly ISuppliersRepository _suppliersRepository;

        public ServicesService(
            IServicesRepository repository,
            ICategoryRepository categoriesRepository,
            ISuppliersRepository suppliersRepository)
        {
            _repository = repository;
            _categoriesRepository = categoriesRepository;
            _suppliersRepository = suppliersRepository;
        }

        public async Task<Result<Service>> CreateServiceAsync(ServiceCreateViewModel model)
        {
            // Validate category exists if provided
            if (model.CategoryID.HasValue)
            {
                var category = await _categoriesRepository.GetByIdAsync(model.CategoryID.Value);
                if (category == null)
                    return Result<Service>.Failure("Category not found");
            }

            // Validate supplier exists if provided
            if (model.SupplierID.HasValue)
            {
                var supplier = await _suppliersRepository.GetByIdAsync(model.SupplierID.Value);
                if (supplier == null)
                    return Result<Service>.Failure("Supplier not found");
            }

            // Check for duplicate code
            if (!string.IsNullOrEmpty(model.Code))
            {
                var existingByCode = await _repository.GetByCodeAsync(model.Code);
                if (existingByCode != null)
                    return Result<Service>.Failure("Service code already exists");
            }

            var service = new Service
            {
                Name = model.Name,
                Code = model.Code,
                Description = model.Description,
                CategoryID = model.CategoryID,
                SupplierID = model.SupplierID,
                PurchasePrice = model.PurchasePrice,
                UnitPrice = model.UnitPrice,
                MinPrice = model.MinPrice,
                Discount = model.Discount,
                DiscountType = model.DiscountType,
                ProfitMargin = model.ProfitMargin,
                Status = model.Status
            };

            await _repository.AddAsync(service);

            return Result<Service>.Success(service);
        }

        public async Task<Result<Service>> UpdateServiceAsync(int id, ServiceEditViewModel model)
        {
            var service = await _repository.GetByIdAsync(id);
            if (service == null)
                return Result<Service>.Failure("Service not found");

            // Validate category exists if provided
            if (model.CategoryID.HasValue)
            {
                var category = await _categoriesRepository.GetByIdAsync(model.CategoryID.Value);
                if (category == null)
                    return Result<Service>.Failure("Category not found");
            }

            // Validate supplier exists if provided
            if (model.SupplierID.HasValue)
            {
                var supplier = await _suppliersRepository.GetByIdAsync(model.SupplierID.Value);
                if (supplier == null)
                    return Result<Service>.Failure("Supplier not found");
            }

            // Check for duplicate code (excluding current service)
            if (!string.IsNullOrEmpty(model.Code) && model.Code != service.Code)
            {
                var existingByCode = await _repository.GetByCodeAsync(model.Code);
                if (existingByCode != null && existingByCode.ServiceID != id)
                    return Result<Service>.Failure("Service code already exists");
            }

            service.Name = model.Name;
            service.Code = model.Code;
            service.Description = model.Description;
            service.CategoryID = model.CategoryID;
            service.SupplierID = model.SupplierID;
            service.PurchasePrice = model.PurchasePrice;
            service.UnitPrice = model.UnitPrice;
            service.MinPrice = model.MinPrice;
            service.Discount = model.Discount;
            service.DiscountType = model.DiscountType;
            service.ProfitMargin = model.ProfitMargin;
            service.Status = model.Status;
            service.UpdatedAt = DateTime.UtcNow;

            await _repository.UpdateAsync(service);

            return Result<Service>.Success(service);
        }

        public async Task<Result> DeleteServiceAsync(int id)
        {
            var service = await _repository.GetByIdAsync(id);
            if (service == null)
                return Result.Failure("Service not found");

            await _repository.DeleteAsync(service);

            return Result.Success();
        }

        public async Task<Service?> GetServiceByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<Service?> GetServiceByCodeAsync(string code)
        {
            return await _repository.GetByCodeAsync(code);
        }

        public async Task<IEnumerable<Service>> GetAllServicesAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<IEnumerable<Service>> GetServicesByCategoryAsync(int categoryId)
        {
            return await _repository.GetByCategoryAsync(categoryId);
        }

        public async Task<IEnumerable<Service>> GetServicesBySupplierAsync(int supplierId)
        {
            return await _repository.GetBySupplierAsync(supplierId);
        }
    }
}