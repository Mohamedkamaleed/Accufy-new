using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WarehouseManagement.Core.Services;
using WarehouseManagement.Core.ViewModels;

namespace WarehouseManagement.Presentation.Controllers
{
    public class ServicesController : Controller
    {
        private readonly IServicesService _servicesService;
        private readonly ICategoryService _categoriesService;
        private readonly ISuppliersService _suppliersService;

        public ServicesController(
            IServicesService servicesService,
            ICategoryService categoriesService,
            ISuppliersService suppliersService)
        {
            _servicesService = servicesService;
            _categoriesService = categoriesService;
            _suppliersService = suppliersService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string search, int? categoryId, int? supplierId)
        {
            var allServices = await _servicesService.GetAllServicesAsync();

            var filteredServices = allServices.AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                filteredServices = filteredServices
                    .Where(s => s.Name.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                               s.Code.Contains(search, StringComparison.OrdinalIgnoreCase));
            }

            if (categoryId.HasValue)
                filteredServices = filteredServices.Where(s => s.CategoryID == categoryId);

            if (supplierId.HasValue)
                filteredServices = filteredServices.Where(s => s.SupplierID == supplierId);

            var data = filteredServices
                .Select(s => new ServiceViewModel
                {
                    ServiceID = s.ServiceID,
                    Name = s.Name,
                    Code = s.Code,
                    Description = s.Description,
                    CategoryID = s.CategoryID,
                    CategoryName = s.Category.Name,
                    SupplierID = s.SupplierID,
                    SupplierName = s.Supplier.Name,
                    PurchasePrice = s.PurchasePrice,
                    UnitPrice = s.UnitPrice,
                    MinPrice = s.MinPrice,
                    Discount = s.Discount,
                    DiscountType = s.DiscountType,
                    ProfitMargin = s.ProfitMargin,
                    Status = s.Status,
                    CreatedAt = s.CreatedAt,
                    UpdatedAt = s.UpdatedAt
                })
                .ToList();

            ViewBag.Categories = new SelectList(await _categoriesService.GetAllCategoriesAsync(), "CategoryID", "Name");
            ViewBag.Suppliers = new SelectList(await _suppliersService.GetAllSuppliersAsync(), "SupplierID", "Name");

            return View(data);
        }

        // GET: /Services/Create
        public async Task<IActionResult> Create()
        {
            await LoadDropdowns();
            return View(new ServiceCreateViewModel());
        }

        // POST: /Services/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ServiceCreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                await LoadDropdowns();
                return View(model);
            }

            var result = await _servicesService.CreateServiceAsync(model);

            if (result.IsSuccess)
            {
                TempData["Success"] = "Service created successfully.";
                return RedirectToAction(nameof(Index));
            }

            ModelState.AddModelError("", result.Error);
            await LoadDropdowns();
            return View(model);
        }

        // GET: /Services/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var service = await _servicesService.GetServiceByIdAsync(id);
            if (service == null)
                return NotFound();

            var model = new ServiceEditViewModel
            {
                ServiceID = service.ServiceID,
                Name = service.Name,
                Code = service.Code,
                Description = service.Description,
                CategoryID = service.CategoryID,
                SupplierID = service.SupplierID,
                PurchasePrice = service.PurchasePrice,
                UnitPrice = service.UnitPrice,
                MinPrice = service.MinPrice,
                Discount = service.Discount,
                DiscountType = service.DiscountType,
                ProfitMargin = service.ProfitMargin,
                Status = service.Status
            };

            await LoadDropdowns();
            return View(model);
        }

        // POST: /Services/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ServiceEditViewModel model)
        {
            if (!ModelState.IsValid)
            {
                await LoadDropdowns();
                return View(model);
            }

            var result = await _servicesService.UpdateServiceAsync(id, model);

            if (result.IsSuccess)
            {
                TempData["Success"] = "Service updated successfully.";
                return RedirectToAction(nameof(Index));
            }

            ModelState.AddModelError("", result.Error);
            await LoadDropdowns();
            return View(model);
        }

        // POST: /Services/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _servicesService.DeleteServiceAsync(id);

            if (result.IsSuccess)
                TempData["Success"] = result.Message ?? "Service deleted successfully.";
            else
                TempData["Error"] = result.Error;

            return RedirectToAction(nameof(Index));
        }

        private async Task LoadDropdowns()
        {
            ViewBag.Categories = new SelectList(await _categoriesService.GetAllCategoriesAsync(), "CategoryID", "Name");
            ViewBag.Suppliers = new SelectList(await _suppliersService.GetAllSuppliersAsync(), "SupplierID", "Name");
        }
    }
}