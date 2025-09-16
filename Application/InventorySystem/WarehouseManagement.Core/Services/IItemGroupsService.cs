using WarehouseManagement.Core.Common;
using WarehouseManagement.Core.Entities;
using WarehouseManagement.Core.Repositories;
using WarehouseManagement.Core.ViewModels;

namespace WarehouseManagement.Core.Services
{
    public interface IItemGroupsService
    {
        Task<Result<ItemGroup>> CreateItemGroupAsync(ItemGroupCreateViewModel model);
        Task<Result<ItemGroup>> UpdateItemGroupAsync(int id, ItemGroupEditViewModel model);
        Task<Result> DeleteItemGroupAsync(int id);
        Task<ItemGroup?> GetItemGroupByIdAsync(int id);
        Task<ItemGroup?> GetItemGroupByNameAsync(string name);
        Task<IEnumerable<ItemGroup>> GetAllItemGroupsAsync();
        Task<IEnumerable<ItemGroup>> GetItemGroupsByCategoryAsync(int categoryId);
        Task<IEnumerable<ItemGroup>> GetItemGroupsByBrandAsync(int brandId);
    }

    public class ItemGroupsService : IItemGroupsService
    {
        private readonly IItemGroupsRepository _repository;
        private readonly ICategoryRepository _categoriesRepository;
        private readonly IBrandsRepository _brandsRepository;

        public ItemGroupsService(
            IItemGroupsRepository repository,
            ICategoryRepository categoriesRepository,
            IBrandsRepository brandsRepository)
        {
            _repository = repository;
            _categoriesRepository = categoriesRepository;
            _brandsRepository = brandsRepository;
        }

        public async Task<Result<ItemGroup>> CreateItemGroupAsync(ItemGroupCreateViewModel model)
        {
            // Validate category exists if provided
            if (model.CategoryID.HasValue)
            {
                var category = await _categoriesRepository.GetByIdAsync(model.CategoryID.Value);
                if (category == null)
                    return Result<ItemGroup>.Failure("Category not found");
            }

            // Validate brand exists if provided
            if (model.BrandID.HasValue)
            {
                var brand = await _brandsRepository.GetByIdAsync(model.BrandID.Value);
                if (brand == null)
                    return Result<ItemGroup>.Failure("Brand not found");
            }

            // Check for duplicate name
            var existing = await _repository.GetByNameAsync(model.Name);
            if (existing != null)
                return Result<ItemGroup>.Failure("Item group name already exists");

            var itemGroup = new ItemGroup
            {
                Name = model.Name,
                CategoryID = model.CategoryID,
                BrandID = model.BrandID,
                Description = model.Description
            };

            await _repository.AddAsync(itemGroup);

            return Result<ItemGroup>.Success(itemGroup);
        }

        public async Task<Result<ItemGroup>> UpdateItemGroupAsync(int id, ItemGroupEditViewModel model)
        {
            var itemGroup = await _repository.GetByIdAsync(id);
            if (itemGroup == null)
                return Result<ItemGroup>.Failure("Item group not found");

            // Validate category exists if provided
            if (model.CategoryID.HasValue)
            {
                var category = await _categoriesRepository.GetByIdAsync(model.CategoryID.Value);
                if (category == null)
                    return Result<ItemGroup>.Failure("Category not found");
            }

            // Validate brand exists if provided
            if (model.BrandID.HasValue)
            {
                var brand = await _brandsRepository.GetByIdAsync(model.BrandID.Value);
                if (brand == null)
                    return Result<ItemGroup>.Failure("Brand not found");
            }

            // Check for duplicate name (excluding current item group)
            if (model.Name != itemGroup.Name)
            {
                var existing = await _repository.GetByNameAsync(model.Name);
                if (existing != null && existing.GroupID != id)
                    return Result<ItemGroup>.Failure("Item group name already exists");
            }

            itemGroup.Name = model.Name;
            itemGroup.CategoryID = model.CategoryID;
            itemGroup.BrandID = model.BrandID;
            itemGroup.Description = model.Description;

            await _repository.UpdateAsync(itemGroup);

            return Result<ItemGroup>.Success(itemGroup);
        }

        public async Task<Result> DeleteItemGroupAsync(int id)
        {
            var itemGroup = await _repository.GetByIdAsync(id);
            if (itemGroup == null)
                return Result.Failure("Item group not found");

            try
            {
                await _repository.DeleteAsync(itemGroup);
                return Result.Success();
            }
            catch (InvalidOperationException ex)
            {
                return Result.Failure(ex.Message);
            }
        }

        public async Task<ItemGroup?> GetItemGroupByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<ItemGroup?> GetItemGroupByNameAsync(string name)
        {
            return await _repository.GetByNameAsync(name);
        }

        public async Task<IEnumerable<ItemGroup>> GetAllItemGroupsAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<IEnumerable<ItemGroup>> GetItemGroupsByCategoryAsync(int categoryId)
        {
            return await _repository.GetByCategoryAsync(categoryId);
        }

        public async Task<IEnumerable<ItemGroup>> GetItemGroupsByBrandAsync(int brandId)
        {
            return await _repository.GetByBrandAsync(brandId);
        }
    }
}