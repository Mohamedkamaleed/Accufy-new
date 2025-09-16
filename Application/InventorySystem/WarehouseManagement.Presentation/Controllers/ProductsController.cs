using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WarehouseManagement.Core.Services;
using WarehouseManagement.Core.ViewModels;

namespace WarehouseManagement.Presentation.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IProductsService _productsService;
        private readonly ICategoryService _categoriesService;
        private readonly IBrandsService _brandsService;
        private readonly ISuppliersService _suppliersService;

        public ProductsController(
            IProductsService productsService,
            ICategoryService categoriesService,
            IBrandsService brandsService,
            ISuppliersService suppliersService)
        {
            _productsService = productsService;
            _categoriesService = categoriesService;
            _brandsService = brandsService;
            _suppliersService = suppliersService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string search, int? categoryId, int? brandId)
        {
            var allProducts = await _productsService.GetAllProductsAsync();

            var filteredProducts = allProducts.AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                filteredProducts = filteredProducts.Where(p =>
                    p.Name.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                    p.SKU.Contains(search, StringComparison.OrdinalIgnoreCase));
            }

            if (categoryId.HasValue)
                filteredProducts = filteredProducts.Where(p => p.CategoryID == categoryId);

            if (brandId.HasValue)
                filteredProducts = filteredProducts.Where(p => p.BrandID == brandId);

            var data = filteredProducts
                .Select(p => new ProductViewModel
                {
                    ProductID = p.ProductID,
                    Name = p.Name,
                    SKU = p.SKU,
                    Description = p.Description,
                    CategoryID = p.CategoryID,
                    CategoryName = p.Category.Name,
                    BrandID = p.BrandID,
                    BrandName = p.Brand.Name,
                    SupplierID = p.SupplierID,
                    SupplierName = p.Supplier.Name,
                    Barcode = p.Barcode,
                    PurchasePrice = p.PurchasePrice,
                    SellingPrice = p.SellingPrice,
                    MinPrice = p.MinPrice,
                    Discount = p.Discount,
                    DiscountType = p.DiscountType,
                    ProfitMargin = p.ProfitMargin,
                    TrackStock = p.TrackStock,
                    InitialStock = p.InitialStock,
                    LowStockThreshold = p.LowStockThreshold,
                    Status = p.Status
                }).ToList();

            ViewBag.Categories = new SelectList(await _categoriesService.GetAllCategoriesAsync(), "CategoryID", "Name");
            ViewBag.Brands = new SelectList(await _brandsService.GetAllBrandsAsync(), "BrandID", "Name");

            return View(data);
        }

        public async Task<IActionResult> Create()
        {
            await LoadDropdowns();
            return View(new ProductCreateViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductCreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                await LoadDropdowns();
                return View(model);
            }

            var result = await _productsService.CreateProductAsync(model);
            if (result.IsSuccess)
            {
                TempData["Success"] = "Product created successfully.";
                return RedirectToAction(nameof(Index));
            }

            ModelState.AddModelError("", result.Error);
            await LoadDropdowns();
            return View(model);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var product = await _productsService.GetProductByIdAsync(id);
            if (product == null) return NotFound();

            var model = new ProductEditViewModel
            {
                ProductID = product.ProductID,
                Name = product.Name,
                SKU = product.SKU,
                Description = product.Description,
                CategoryID = product.CategoryID,
                BrandID = product.BrandID,
                SupplierID = product.SupplierID,
                Barcode = product.Barcode,
                PurchasePrice = product.PurchasePrice,
                SellingPrice = product.SellingPrice,
                MinPrice = product.MinPrice,
                Discount = product.Discount,
                DiscountType = product.DiscountType,
                ProfitMargin = product.ProfitMargin,
                TrackStock = product.TrackStock,
                InitialStock = product.InitialStock,
                LowStockThreshold = product.LowStockThreshold,
                Status = product.Status
            };

            await LoadDropdowns();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ProductEditViewModel model)
        {
            if (!ModelState.IsValid)
            {
                await LoadDropdowns();
                return View(model);
            }

            var result = await _productsService.UpdateProductAsync(id, model);
            if (result.IsSuccess)
            {
                TempData["Success"] = "Product updated successfully.";
                return RedirectToAction(nameof(Index));
            }

            ModelState.AddModelError("", result.Error);
            await LoadDropdowns();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _productsService.DeleteProductAsync(id);
            if (result.IsSuccess)
                TempData["Success"] = result.Message ?? "Product deleted successfully.";
            else
                TempData["Error"] = result.Error;

            return RedirectToAction(nameof(Index));
        }

        private async Task LoadDropdowns()
        {
            ViewBag.Categories = new SelectList(await _categoriesService.GetAllCategoriesAsync(), "CategoryID", "Name");
            ViewBag.Brands = new SelectList(await _brandsService.GetAllBrandsAsync(), "BrandID", "Name");
            ViewBag.Suppliers = new SelectList(await _suppliersService.GetAllSuppliersAsync(), "SupplierID", "Name");
        }
    }
}
