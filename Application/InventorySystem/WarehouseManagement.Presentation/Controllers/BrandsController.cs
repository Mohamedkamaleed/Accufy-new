using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WarehouseManagement.Core.Services;
using WarehouseManagement.Core.ViewModels;

namespace WarehouseManagement.Presentation.Controllers
{
    public class BrandsController : Controller
    {
        private readonly IBrandsService _brandsService;

        public BrandsController(IBrandsService brandsService)
        {
            _brandsService = brandsService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string search, int? parentBrandId)
        {
            var allBrands = await _brandsService.GetAllBrandsAsync();

            var filteredBrands = allBrands.AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                filteredBrands = filteredBrands
                    .Where(c => c.Name.Contains(search, StringComparison.OrdinalIgnoreCase));
            }

            var data = filteredBrands
         .AsEnumerable() // ✅ Force in-memory LINQ
         .Select(c => new BrandListViewModel
         {
             BrandID = c.BrandID,
             Name = c.Name,
             Description = c.Description
         })
         .ToList();

            var allCats = await _brandsService.GetAllBrandsAsync();
            ViewBag.ParentBrands = new SelectList(allCats, "BrandID", "Name", parentBrandId);

            return View(data); // ✅ Return the view with model
        }



        // GET: /Brands/Create
        public async Task<IActionResult> Create()
        {
            // Load parent brands for dropdown if needed
            var allBrands = await _brandsService.GetAllBrandsAsync();
            ViewBag.ParentBrands = new SelectList(allBrands, "BrandID", "Name");
            return View(new BrandCreateViewModel());
        }

        // POST: /Brands/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BrandCreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var allBrands = await _brandsService.GetAllBrandsAsync();
                return View(model);
            }

            var result = await _brandsService.CreateBrandAsync(model);

            if (result.IsSuccess)
            {
                TempData["Success"] = "Brand created successfully.";
                return RedirectToAction(nameof(Index));
            }

            ModelState.AddModelError("", result.Error);
            var allCats = await _brandsService.GetAllBrandsAsync();
            return View(model);
        }

        // GET: /Brands/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var brand = await _brandsService.GetBrandByIdAsync(id);
            if (brand == null)
                return NotFound();

            var model = new BrandEditViewModel
            {
                Name = brand.Name,
                Description = brand.Description
            };

            var allBrands = await _brandsService.GetAllBrandsAsync();

            return View(model);
        }

        // POST: /Brands/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, BrandEditViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var allBrands = await _brandsService.GetAllBrandsAsync();
                return View(model);
            }

            var result = await _brandsService.UpdateBrandAsync(id, model);

            if (result.IsSuccess)
            {
                TempData["Success"] = "Brand updated successfully.";
                return RedirectToAction(nameof(Index));
            }

            ModelState.AddModelError("", result.Error);
            var allCats = await _brandsService.GetAllBrandsAsync();
            return View(model);
        }

        // POST: /Brands/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _brandsService.DeleteBrandAsync(id);

            if (result.IsSuccess)
                TempData["Success"] = result.Message ?? "Brand deleted successfully.";
            else
                TempData["Error"] = result.Error;

            return RedirectToAction(nameof(Index));
        }
    }

}
