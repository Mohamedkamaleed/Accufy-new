using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WarehouseManagement.Core.Services;
using WarehouseManagement.Core.ViewModels;

namespace WarehouseManagement.Presentation.Controllers
{
    public class SuppliersController : Controller
    {
        private readonly ISuppliersService _suppliersService;

        public SuppliersController(ISuppliersService suppliersService)
        {
            _suppliersService = suppliersService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string search, int? parentSupplierId)
        {
            var allSuppliers = await _suppliersService.GetAllSuppliersAsync();

            var filteredSuppliers = allSuppliers.AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                filteredSuppliers = filteredSuppliers
                    .Where(c => c.Name.Contains(search, StringComparison.OrdinalIgnoreCase));
            }

            var data = filteredSuppliers
         .AsEnumerable() // ✅ Force in-memory LINQ
         .Select(c => new SupplierListViewModel
         {
             SupplierID = c.SupplierID,
             Name = c.Name,
             Description = c.Description
         })
         .ToList();

            var allCats = await _suppliersService.GetAllSuppliersAsync();
            ViewBag.ParentSuppliers = new SelectList(allCats, "SupplierID", "Name", parentSupplierId);

            return View(data); // ✅ Return the view with model
        }



        // GET: /Suppliers/Create
        public async Task<IActionResult> Create()
        {
            // Load parent suppliers for dropdown if needed
            var allSuppliers = await _suppliersService.GetAllSuppliersAsync();
            ViewBag.ParentSuppliers = new SelectList(allSuppliers, "SupplierID", "Name");
            return View(new SupplierCreateViewModel());
        }

        // POST: /Suppliers/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SupplierCreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var allSuppliers = await _suppliersService.GetAllSuppliersAsync();
                return View(model);
            }

            var result = await _suppliersService.CreateSupplierAsync(model);

            if (result.IsSuccess)
            {
                TempData["Success"] = "Supplier created successfully.";
                return RedirectToAction(nameof(Index));
            }

            ModelState.AddModelError("", result.Error);
            var allCats = await _suppliersService.GetAllSuppliersAsync();
            return View(model);
        }

        // GET: /Suppliers/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var supplier = await _suppliersService.GetSupplierByIdAsync(id);
            if (supplier == null)
                return NotFound();

            var model = new SupplierEditViewModel
            {
                Name = supplier.Name,
                Description = supplier.Description
            };

            var allSuppliers = await _suppliersService.GetAllSuppliersAsync();

            return View(model);
        }

        // POST: /Suppliers/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, SupplierEditViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var allSuppliers = await _suppliersService.GetAllSuppliersAsync();
                return View(model);
            }

            var result = await _suppliersService.UpdateSupplierAsync(id, model);

            if (result.IsSuccess)
            {
                TempData["Success"] = "Supplier updated successfully.";
                return RedirectToAction(nameof(Index));
            }

            ModelState.AddModelError("", result.Error);
            var allCats = await _suppliersService.GetAllSuppliersAsync();
            return View(model);
        }

        // POST: /Suppliers/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _suppliersService.DeleteSupplierAsync(id);

            if (result.IsSuccess)
                TempData["Success"] = result.Message ?? "Supplier deleted successfully.";
            else
                TempData["Error"] = result.Error;

            return RedirectToAction(nameof(Index));
        }
    }

}
