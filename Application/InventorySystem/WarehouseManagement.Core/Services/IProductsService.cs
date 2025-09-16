using WarehouseManagement.Core.Common;
using WarehouseManagement.Core.Entities;
using WarehouseManagement.Core.Repositories;
using WarehouseManagement.Core.ViewModels;

namespace WarehouseManagement.Core.Services
{
    public interface IProductsService
    {
        Task<Result<Product>> CreateProductAsync(ProductCreateViewModel model);
        Task<Result<Product>> UpdateProductAsync(int id, ProductEditViewModel model);
        Task<Result> DeleteProductAsync(int id);
        Task<Product?> GetProductByIdAsync(int id);
        Task<Product?> GetProductBySkuAsync(string sku);
        Task<Product?> GetProductByBarcodeAsync(string barcode);
        Task<IEnumerable<Product>> GetAllProductsAsync();
        Task<IEnumerable<Product>> GetProductsByCategoryAsync(int categoryId);
        Task<IEnumerable<Product>> GetLowStockProductsAsync();
    }

    public class ProductsService : IProductsService
    {
        private readonly IProductsRepository _repository;
        private readonly ICategoryRepository _categoriesRepository;
        private readonly IBrandsRepository _brandsRepository;
        private readonly ISuppliersRepository _suppliersRepository;

        public ProductsService(
            IProductsRepository repository,
            ICategoryRepository categoriesRepository,
            IBrandsRepository brandsRepository,
            ISuppliersRepository suppliersRepository)
        {
            _repository = repository;
            _categoriesRepository = categoriesRepository;
            _brandsRepository = brandsRepository;
            _suppliersRepository = suppliersRepository;
        }

        public async Task<Result<Product>> CreateProductAsync(ProductCreateViewModel model)
        {
            // Validate category exists
            var category = await _categoriesRepository.GetByIdAsync(model.CategoryID);
            if (category == null)
                return Result<Product>.Failure("Category not found");

            // Validate brand exists if provided
            if (model.BrandID.HasValue)
            {
                var brand = await _brandsRepository.GetByIdAsync(model.BrandID.Value);
                if (brand == null)
                    return Result<Product>.Failure("Brand not found");
            }

            // Validate supplier exists if provided
            if (model.SupplierID.HasValue)
            {
                var supplier = await _suppliersRepository.GetByIdAsync(model.SupplierID.Value);
                if (supplier == null)
                    return Result<Product>.Failure("Supplier not found");
            }

            // Check for duplicate SKU
            if (!string.IsNullOrEmpty(model.SKU))
            {
                var existingBySku = await _repository.GetBySkuAsync(model.SKU);
                if (existingBySku != null)
                    return Result<Product>.Failure("SKU already exists");
            }

            // Check for duplicate barcode
            if (!string.IsNullOrEmpty(model.Barcode))
            {
                var existingByBarcode = await _repository.GetByBarcodeAsync(model.Barcode);
                if (existingByBarcode != null)
                    return Result<Product>.Failure("Barcode already exists");
            }

            var product = new Product
            {
                Name = model.Name,
                SKU = model.SKU,
                Description = model.Description,
                CategoryID = model.CategoryID,
                BrandID = model.BrandID,
                SupplierID = model.SupplierID,
                Barcode = model.Barcode,
                PurchasePrice = model.PurchasePrice,
                SellingPrice = model.SellingPrice,
                MinPrice = model.MinPrice,
                Discount = model.Discount,
                DiscountType = model.DiscountType,
                ProfitMargin = model.ProfitMargin,
                TrackStock = model.TrackStock,
                InitialStock = model.InitialStock,
                LowStockThreshold = model.LowStockThreshold,
                Status = model.Status
            };

            await _repository.AddAsync(product);

            return Result<Product>.Success(product);
        }

        public async Task<Result<Product>> UpdateProductAsync(int id, ProductEditViewModel model)
        {
            var product = await _repository.GetByIdAsync(id);
            if (product == null)
                return Result<Product>.Failure("Product not found");

            // Validate category exists
            var category = await _categoriesRepository.GetByIdAsync(model.CategoryID);
            if (category == null)
                return Result<Product>.Failure("Category not found");

            // Validate brand exists if provided
            if (model.BrandID.HasValue)
            {
                var brand = await _brandsRepository.GetByIdAsync(model.BrandID.Value);
                if (brand == null)
                    return Result<Product>.Failure("Brand not found");
            }

            // Validate supplier exists if provided
            if (model.SupplierID.HasValue)
            {
                var supplier = await _suppliersRepository.GetByIdAsync(model.SupplierID.Value);
                if (supplier == null)
                    return Result<Product>.Failure("Supplier not found");
            }

            // Check for duplicate SKU (excluding current product)
            if (!string.IsNullOrEmpty(model.SKU) && model.SKU != product.SKU)
            {
                var existingBySku = await _repository.GetBySkuAsync(model.SKU);
                if (existingBySku != null && existingBySku.ProductID != id)
                    return Result<Product>.Failure("SKU already exists");
            }

            // Check for duplicate barcode (excluding current product)
            if (!string.IsNullOrEmpty(model.Barcode) && model.Barcode != product.Barcode)
            {
                var existingByBarcode = await _repository.GetByBarcodeAsync(model.Barcode);
                if (existingByBarcode != null && existingByBarcode.ProductID != id)
                    return Result<Product>.Failure("Barcode already exists");
            }

            product.Name = model.Name;
            product.SKU = model.SKU;
            product.Description = model.Description;
            product.CategoryID = model.CategoryID;
            product.BrandID = model.BrandID;
            product.SupplierID = model.SupplierID;
            product.Barcode = model.Barcode;
            product.PurchasePrice = model.PurchasePrice;
            product.SellingPrice = model.SellingPrice;
            product.MinPrice = model.MinPrice;
            product.Discount = model.Discount;
            product.DiscountType = model.DiscountType;
            product.ProfitMargin = model.ProfitMargin;
            product.TrackStock = model.TrackStock;
            product.InitialStock = model.InitialStock;
            product.LowStockThreshold = model.LowStockThreshold;
            product.Status = model.Status;
            product.UpdatedAt = DateTime.UtcNow;

            await _repository.UpdateAsync(product);

            return Result<Product>.Success(product);
        }

        public async Task<Result> DeleteProductAsync(int id)
        {
            var product = await _repository.GetByIdAsync(id);
            if (product == null)
                return Result.Failure("Product not found");

            await _repository.DeleteAsync(product);

            return Result.Success();
        }

        public async Task<Product?> GetProductByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<Product?> GetProductBySkuAsync(string sku)
        {
            return await _repository.GetBySkuAsync(sku);
        }

        public async Task<Product?> GetProductByBarcodeAsync(string barcode)
        {
            return await _repository.GetByBarcodeAsync(barcode);
        }

        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<IEnumerable<Product>> GetProductsByCategoryAsync(int categoryId)
        {
            return await _repository.GetByCategoryAsync(categoryId);
        }

        public async Task<IEnumerable<Product>> GetLowStockProductsAsync()
        {
            return await _repository.GetLowStockAsync();
        }
    }
}