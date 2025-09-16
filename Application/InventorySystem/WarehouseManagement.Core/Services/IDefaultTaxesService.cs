using WarehouseManagement.Core.Common;
using WarehouseManagement.Core.Entities;
using WarehouseManagement.Core.Repositories;
using WarehouseManagement.Core.ViewModels;

namespace WarehouseManagement.Core.Services
{
    public interface IDefaultTaxesService
    {
        Task<Result<DefaultTax>> CreateDefaultTaxAsync(DefaultTaxCreateViewModel model);
        Task<Result<DefaultTax>> UpdateDefaultTaxAsync(int id, DefaultTaxEditViewModel model);
        Task<Result> DeleteDefaultTaxAsync(int id);
        Task<DefaultTax?> GetDefaultTaxByIdAsync(int id);
        Task<IEnumerable<DefaultTax>> GetAllDefaultTaxesAsync();
    }
    public class DefaultTaxesService : IDefaultTaxesService
    {
        private readonly IDefaultTaxesRepository _repository;

        public DefaultTaxesService(IDefaultTaxesRepository repository)
        {
            _repository = repository;
        }

        public async Task<Result<DefaultTax>> CreateDefaultTaxAsync(DefaultTaxCreateViewModel model)
        {
            var existing = await _repository.GetByNameAsync(model.Name);
            if (existing != null)
                return Result<DefaultTax>.Failure("Tax name already exists");

            var defaultTax = new DefaultTax
            {
                Name = model.Name,
                TaxValue = model.TaxValue,
                Type = model.Type,
                Mode = model.Mode
            };

            await _repository.AddAsync(defaultTax);

            return Result<DefaultTax>.Success(defaultTax);
        }

        public async Task<Result<DefaultTax>> UpdateDefaultTaxAsync(int id, DefaultTaxEditViewModel model)
        {
            var defaultTax = await _repository.GetByIdAsync(id);
            if (defaultTax == null)
                return Result<DefaultTax>.Failure("Tax not found");

            // Check for duplicate name (excluding current tax)
            if (model.Name != defaultTax.Name)
            {
                var existing = await _repository.GetByNameAsync(model.Name);
                if (existing != null && existing.TaxID != id)
                    return Result<DefaultTax>.Failure("Tax name already exists");
            }

            defaultTax.Name = model.Name;
            defaultTax.TaxValue = model.TaxValue;
            defaultTax.Type = model.Type;
            defaultTax.Mode = model.Mode;

            await _repository.UpdateAsync(defaultTax);

            return Result<DefaultTax>.Success(defaultTax);
        }

        public async Task<Result> DeleteDefaultTaxAsync(int id)
        {
            var defaultTax = await _repository.GetByIdAsync(id);
            if (defaultTax == null)
                return Result.Failure("Tax not found");

            try
            {
                await _repository.DeleteAsync(defaultTax);
                return Result.Success();
            }
            catch (InvalidOperationException ex)
            {
                return Result.Failure(ex.Message);
            }
        }

        public async Task<DefaultTax?> GetDefaultTaxByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<DefaultTax>> GetAllDefaultTaxesAsync()
        {
            return await _repository.GetAllAsync();
        }
    }
}
