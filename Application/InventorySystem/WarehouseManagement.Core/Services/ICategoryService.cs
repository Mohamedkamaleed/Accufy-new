using WarehouseManagement.Core.Common;
using WarehouseManagement.Core.Entities;
using WarehouseManagement.Core.Repositories;
using WarehouseManagement.Core.ViewModels;

namespace WarehouseManagement.Core.Services
{
    public interface ICategoryService
    {
        Task<Result<Category>> CreateCategoryAsync(CategoryCreateViewModel model);
        Task<Result<Category>> UpdateCategoryAsync(int id, CategoryEditViewModel model);
        Task<Result> DeleteCategoryAsync(int id);
        Task<Category?> GetCategoryByIdAsync(int id);
        Task<IEnumerable<Category>> GetAllCategoriesAsync();
    }
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _repository;

        public CategoryService(ICategoryRepository repository)
        {
            _repository = repository;
        }

        public async Task<Result<Category>> CreateCategoryAsync(CategoryCreateViewModel model)
        {
            var existing = await _repository.GetByNameAsync(model.Name);
            if (existing != null)
                return Result<Category>.Failure("Category name already exists");

            var category = new Category
            {
                Name = model.Name,
                Description = model.Description,
                ParentCategoryID = model.ParentCategoryID
            };

            await _repository.AddAsync(category);

            return Result<Category>.Success(category);
        }

        public async Task<Result<Category>> UpdateCategoryAsync(int id, CategoryEditViewModel model)
        {
            var category = await _repository.GetByIdAsync(id);
            if (category == null)
                return Result<Category>.Failure("Category not found");

            category.Name = model.Name;
            category.Description = model.Description;
            category.ParentCategoryID = model.ParentCategoryID;

            await _repository.UpdateAsync(category);

            return Result<Category>.Success(category);
        }

        public async Task<Result> DeleteCategoryAsync(int id)
        {
            var category = await _repository.GetByIdAsync(id);
            if (category == null)
                return Result.Failure("Category not found");

            await _repository.DeleteAsync(category);

            return Result.Success();
        }

        public async Task<Category?> GetCategoryByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<Category>> GetAllCategoriesAsync()
        {
            return await _repository.GetAllAsync();
        }
    }

}
