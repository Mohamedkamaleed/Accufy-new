using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WarehouseManagement.Core.Services;
using WarehouseManagement.Core.ViewModels;

namespace WarehouseManagement.Presentation.Controllers
{
    public class ProductTaxProfilesController : Controller
    {
        private readonly IProductTaxProfilesService _productTaxProfilesService;
        private readonly IProductsService _productsService;
        private readonly ITaxProfilesService _taxProfilesService;

        public ProductTaxProfilesController(
            IProductTaxProfilesService productTaxProfilesService,
            IProductsService productsService,
            ITaxProfilesService taxProfilesService)
        {
            _productTaxProfilesService = productTaxProfilesService;
            _productsService = productsService;
            _taxProfilesService = taxProfilesService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string search, int? productId, int? taxProfileId)
        {
            var allProductTaxProfiles = await _productTaxProfilesService.GetAllProductTaxProfilesAsync();

            var filteredProductTaxProfiles = allProductTaxProfiles.AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                filteredProductTaxProfiles = filteredProductTaxProfiles
                    .Where(ptp => ptp.Product.Name.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                               ptp.TaxProfile.Name.Contains(search, StringComparison.OrdinalIgnoreCase));
            }

            if (productId.HasValue)
                filteredProductTaxProfiles = filteredProductTaxProfiles.Where(ptp => ptp.ProductID == productId);

            if (taxProfileId.HasValue)
                filteredProductTaxProfiles = filteredProductTaxProfiles.Where(ptp => ptp.TaxProfileID == taxProfileId);

            var data = filteredProductTaxProfiles
                .Select(ptp => new ProductTaxProfileViewModel
                {
                    ProductID = ptp.ProductID,
                    ProductName = ptp.Product.Name,
                    TaxProfileID = ptp.TaxProfileID,
                    TaxProfileName = ptp.TaxProfile.Name,
                    TaxRate = ptp.TaxProfile.TaxRate,
                    IsPrimary = ptp.IsPrimary
                })
                .ToList();

            ViewBag.Products = new SelectList(await _productsService.GetAllProductsAsync(), "ProductID", "Name");
            ViewBag.TaxProfiles = new SelectList(await _taxProfilesService.GetAllTaxProfilesAsync(), "TaxProfileID", "Name");

            return View(data);
        }

        // GET: /ProductTaxProfiles/Create
        public async Task<IActionResult> Create()
        {
            await LoadDropdowns();
            return View(new ProductTaxProfileCreateViewModel());
        }

        // POST: /ProductTaxProfiles/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductTaxProfileCreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                await LoadDropdowns();
                return View(model);
            }

            var result = await _productTaxProfilesService.CreateProductTaxProfileAsync(model);

            if (result.IsSuccess)
            {
                TempData["Success"] = "Product Tax Profile created successfully.";
                return RedirectToAction(nameof(Index));
            }

            ModelState.AddModelError("", result.Error);
            await LoadDropdowns();
            return View(model);
        }

        // GET: /ProductTaxProfiles/Edit
        public async Task<IActionResult> Edit(int productId, int taxProfileId)
        {
            var productTaxProfile = await _productTaxProfilesService.GetProductTaxProfileByIdAsync(productId, taxProfileId);
            if (productTaxProfile == null)
                return NotFound();

            var model = new ProductTaxProfileEditViewModel
            {
                ProductID = productTaxProfile.ProductID,
                TaxProfileID = productTaxProfile.TaxProfileID,
                IsPrimary = productTaxProfile.IsPrimary
            };

            await LoadDropdowns();
            return View(model);
        }

        // POST: /ProductTaxProfiles/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ProductTaxProfileEditViewModel model)
        {
            if (!ModelState.IsValid)
            {
                await LoadDropdowns();
                return View(model);
            }

            var result = await _productTaxProfilesService.UpdateProductTaxProfileAsync(model);

            if (result.IsSuccess)
            {
                TempData["Success"] = "Product Tax Profile updated successfully.";
                return RedirectToAction(nameof(Index));
            }

            ModelState.AddModelError("", result.Error);
            await LoadDropdowns();
            return View(model);
        }

        // POST: /ProductTaxProfiles/Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int productId, int taxProfileId)
        {
            var result = await _productTaxProfilesService.DeleteProductTaxProfileAsync(productId, taxProfileId);

            if (result.IsSuccess)
                TempData["Success"] = result.Message ?? "Product Tax Profile deleted successfully.";
            else
                TempData["Error"] = result.Error;

            return RedirectToAction(nameof(Index));
        }

        private async Task LoadDropdowns()
        {
            ViewBag.Products = new SelectList(await _productsService.GetAllProductsAsync(), "ProductID", "Name");
            ViewBag.TaxProfiles = new SelectList(await _taxProfilesService.GetAllTaxProfilesAsync(), "TaxProfileID", "Name");
        }
    }
}
