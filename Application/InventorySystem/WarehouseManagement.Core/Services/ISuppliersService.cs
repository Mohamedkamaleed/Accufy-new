using WarehouseManagement.Core.Common;
using WarehouseManagement.Core.Entities;
using WarehouseManagement.Core.Repositories;
using WarehouseManagement.Core.ViewModels;

namespace WarehouseManagement.Core.Services
{
    public interface ISuppliersService
    {
        Task<Result<Supplier>> CreateSupplierAsync(SupplierCreateViewModel model);
        Task<Result<Supplier>> UpdateSupplierAsync(int id, SupplierEditViewModel model);
        Task<Result> DeleteSupplierAsync(int id);
        Task<Supplier?> GetSupplierByIdAsync(int id);
        Task<IEnumerable<Supplier>> GetAllSuppliersAsync();
    }
    public class SuppliersService : ISuppliersService
    {
        private readonly ISuppliersRepository _repository;

        public SuppliersService(ISuppliersRepository repository)
        {
            _repository = repository;
        }

        public async Task<Result<Supplier>> CreateSupplierAsync(SupplierCreateViewModel model)
        {
            var existing = await _repository.GetByNameAsync(model.Name);
            if (existing != null)
                return Result<Supplier>.Failure("Supplier name already exists");

            var Supplier = new Supplier
            {
                Name = model.Name,
                Description = model.Description
            };

            await _repository.AddAsync(Supplier);

            return Result<Supplier>.Success(Supplier);
        }

        public async Task<Result<Supplier>> UpdateSupplierAsync(int id, SupplierEditViewModel model)
        {
            var Supplier = await _repository.GetByIdAsync(id);
            if (Supplier == null)
                return Result<Supplier>.Failure("Supplier not found");

            Supplier.Name = model.Name;
            Supplier.Description = model.Description;

            await _repository.UpdateAsync(Supplier);

            return Result<Supplier>.Success(Supplier);
        }

        public async Task<Result> DeleteSupplierAsync(int id)
        {
            var Supplier = await _repository.GetByIdAsync(id);
            if (Supplier == null)
                return Result.Failure("Supplier not found");

            await _repository.DeleteAsync(Supplier);

            return Result.Success();
        }

        public async Task<Supplier?> GetSupplierByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<Supplier>> GetAllSuppliersAsync()
        {
            return await _repository.GetAllAsync();
        }
    }

}
