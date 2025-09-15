using WarehouseManagement.Core.Common;
using WarehouseManagement.Core.Entities;
using WarehouseManagement.Core.Repositories;
using WarehouseManagement.Core.ViewModels;

namespace WarehouseManagement.Core.Services
{
    public interface IBrandsService
    {
        Task<Result<Brand>> CreateBrandAsync(BrandCreateViewModel model);
        Task<Result<Brand>> UpdateBrandAsync(int id, BrandEditViewModel model);
        Task<Result> DeleteBrandAsync(int id);
        Task<Brand?> GetBrandByIdAsync(int id);
        Task<IEnumerable<Brand>> GetAllBrandsAsync();
    }
    public class BrandsService : IBrandsService
    {
        private readonly IBrandsRepository _repository;

        public BrandsService(IBrandsRepository repository)
        {
            _repository = repository;
        }

        public async Task<Result<Brand>> CreateBrandAsync(BrandCreateViewModel model)
        {
            var existing = await _repository.GetByNameAsync(model.Name);
            if (existing != null)
                return Result<Brand>.Failure("Brand name already exists");

            var brand = new Brand
            {
                Name = model.Name,
                Description = model.Description
            };

            await _repository.AddAsync(brand);

            return Result<Brand>.Success(brand);
        }

        public async Task<Result<Brand>> UpdateBrandAsync(int id, BrandEditViewModel model)
        {
            var brand = await _repository.GetByIdAsync(id);
            if (brand == null)
                return Result<Brand>.Failure("Brand not found");

            brand.Name = model.Name;
            brand.Description = model.Description;

            await _repository.UpdateAsync(brand);

            return Result<Brand>.Success(brand);
        }

        public async Task<Result> DeleteBrandAsync(int id)
        {
            var brand = await _repository.GetByIdAsync(id);
            if (brand == null)
                return Result.Failure("Brand not found");

            await _repository.DeleteAsync(brand);

            return Result.Success();
        }

        public async Task<Brand?> GetBrandByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<Brand>> GetAllBrandsAsync()
        {
            return await _repository.GetAllAsync();
        }
    }

}
