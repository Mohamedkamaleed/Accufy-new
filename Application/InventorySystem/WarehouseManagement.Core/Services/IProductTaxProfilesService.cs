using WarehouseManagement.Core.Common;
using WarehouseManagement.Core.Entities;
using WarehouseManagement.Core.Repositories;
using WarehouseManagement.Core.ViewModels;

namespace WarehouseManagement.Core.Services
{
    public interface IProductTaxProfilesService
    {
        Task<Result<ProductTaxProfile>> CreateProductTaxProfileAsync(ProductTaxProfileCreateViewModel model);
        Task<Result<ProductTaxProfile>> UpdateProductTaxProfileAsync(ProductTaxProfileEditViewModel model);
        Task<Result> DeleteProductTaxProfileAsync(int productId, int taxProfileId);
        Task<ProductTaxProfile?> GetProductTaxProfileAsync(int productId, int taxProfileId);
        Task<IEnumerable<ProductTaxProfile>> GetTaxProfilesForProductAsync(int productId);
        Task<IEnumerable<ProductTaxProfile>> GetProductsForTaxProfileAsync(int taxProfileId);
        Task<ProductTaxProfile?> GetPrimaryTaxProfileForProductAsync(int productId);
        Task<Result> SetPrimaryTaxProfileAsync(int productId, int taxProfileId);
        Task<ProductTaxProfile> GetProductTaxProfileByIdAsync(int productId, int taxProfileId);
        Task<IEnumerable<ProductTaxProfile>> GetAllProductTaxProfilesAsync();
    }

    public class ProductTaxProfilesService : IProductTaxProfilesService
    {
        private readonly IProductTaxProfilesRepository _repository;
        private readonly IProductsRepository _productsRepository;
        private readonly ITaxProfilesRepository _taxProfilesRepository;

        public ProductTaxProfilesService(
            IProductTaxProfilesRepository repository,
            IProductsRepository productsRepository,
            ITaxProfilesRepository taxProfilesRepository)
        {
            _repository = repository;
            _productsRepository = productsRepository;
            _taxProfilesRepository = taxProfilesRepository;
        }

        public async Task<Result<ProductTaxProfile>> CreateProductTaxProfileAsync(ProductTaxProfileCreateViewModel model)
        {
            // Validate product exists
            var product = await _productsRepository.GetByIdAsync(model.ProductID);
            if (product == null)
                return Result<ProductTaxProfile>.Failure("Product not found");

            // Validate tax profile exists
            var taxProfile = await _taxProfilesRepository.GetByIdAsync(model.TaxProfileID);
            if (taxProfile == null)
                return Result<ProductTaxProfile>.Failure("Tax profile not found");

            // Check if relationship already exists
            var existing = await _repository.GetByProductAndTaxProfileAsync(model.ProductID, model.TaxProfileID);
            if (existing != null)
                return Result<ProductTaxProfile>.Failure("This tax profile is already assigned to the product");

            var productTaxProfile = new ProductTaxProfile
            {
                ProductID = model.ProductID,
                TaxProfileID = model.TaxProfileID,
                IsPrimary = model.IsPrimary
            };

            // If setting as primary, ensure only one primary exists
            if (model.IsPrimary)
            {
                await _repository.SetPrimaryTaxProfileAsync(model.ProductID, model.TaxProfileID);
            }
            else
            {
                await _repository.AddAsync(productTaxProfile);
            }

            return Result<ProductTaxProfile>.Success(productTaxProfile);
        }

        public async Task<Result<ProductTaxProfile>> UpdateProductTaxProfileAsync(ProductTaxProfileEditViewModel model)
        {
            var productTaxProfile = await _repository.GetByProductAndTaxProfileAsync(model.ProductID, model.TaxProfileID);
            if (productTaxProfile == null)
                return Result<ProductTaxProfile>.Failure("Product tax profile relationship not found");

            // If setting as primary, use the dedicated method to ensure only one primary exists
            if (model.IsPrimary && !productTaxProfile.IsPrimary)
            {
                await _repository.SetPrimaryTaxProfileAsync(model.ProductID, model.TaxProfileID);
            }
            else
            {
                productTaxProfile.IsPrimary = model.IsPrimary;
                await _repository.UpdateAsync(productTaxProfile);
            }

            return Result<ProductTaxProfile>.Success(productTaxProfile);
        }

        public async Task<Result> DeleteProductTaxProfileAsync(int productId, int taxProfileId)
        {
            var productTaxProfile = await _repository.GetByProductAndTaxProfileAsync(productId, taxProfileId);
            if (productTaxProfile == null)
                return Result.Failure("Product tax profile relationship not found");

            // Prevent deletion of primary tax profile if it's the only one
            if (productTaxProfile.IsPrimary)
            {
                var otherProfiles = await _repository.GetByProductIdAsync(productId);
                if (otherProfiles.Count() > 1)
                    return Result.Failure("Cannot delete the primary tax profile. Set another tax profile as primary first.");
            }

            await _repository.DeleteAsync(productId, taxProfileId);

            return Result.Success();
        }

        public async Task<ProductTaxProfile?> GetProductTaxProfileAsync(int productId, int taxProfileId)
        {
            return await _repository.GetByProductAndTaxProfileAsync(productId, taxProfileId);
        }

        public async Task<IEnumerable<ProductTaxProfile>> GetTaxProfilesForProductAsync(int productId)
        {
            return await _repository.GetByProductIdAsync(productId);
        }

        public async Task<IEnumerable<ProductTaxProfile>> GetProductsForTaxProfileAsync(int taxProfileId)
        {
            return await _repository.GetByTaxProfileIdAsync(taxProfileId);
        }

        public async Task<ProductTaxProfile?> GetPrimaryTaxProfileForProductAsync(int productId)
        {
            return await _repository.GetPrimaryTaxProfileForProductAsync(productId);
        }

        public async Task<Result> SetPrimaryTaxProfileAsync(int productId, int taxProfileId)
        {
            // Validate product exists
            var product = await _productsRepository.GetByIdAsync(productId);
            if (product == null)
                return Result.Failure("Product not found");

            // Validate tax profile exists
            var taxProfile = await _taxProfilesRepository.GetByIdAsync(taxProfileId);
            if (taxProfile == null)
                return Result.Failure("Tax profile not found");

            // Validate relationship exists
            var relationship = await _repository.GetByProductAndTaxProfileAsync(productId, taxProfileId);
            if (relationship == null)
                return Result.Failure("This tax profile is not assigned to the product");

            await _repository.SetPrimaryTaxProfileAsync(productId, taxProfileId);

            return Result.Success();
        }

        public async Task<ProductTaxProfile> GetProductTaxProfileByIdAsync(int productId, int taxProfileId)
        {
            return await _repository.GetProductTaxProfileByIdAsync(productId, taxProfileId);
        }

        public async Task<IEnumerable<ProductTaxProfile>> GetAllProductTaxProfilesAsync()
        {
           return await _repository.GetAllAsync();
        }
    }
}