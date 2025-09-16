using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WarehouseManagement.Core.Services;
using WarehouseManagement.Core.ViewModels;

namespace WarehouseManagement.Presentation.Controllers
{
    public class ItemGroupsController : Controller
    {
        private readonly IItemGroupsService _itemGroupsService;
        private readonly ICategoryService _categoriesService;
        private readonly IBrandsService _brandsService;

        public ItemGroupsController(
            IItemGroupsService itemGroupsService,
            ICategoryService categoriesService,
            IBrandsService brandsService)
        {
            _itemGroupsService = itemGroupsService;
            _categoriesService = categoriesService;
            _brandsService = brandsService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string search, int? categoryId, int? brandId)
        {
            var allItemGroups = await _itemGroupsService.GetAllItemGroupsAsync();

            var filteredItemGroups = allItemGroups.AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                filteredItemGroups = filteredItemGroups
                    .Where(g => g.Name.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                               g.Description.Contains(search, StringComparison.OrdinalIgnoreCase));
            }

            if (categoryId.HasValue)
                filteredItemGroups = filteredItemGroups.Where(g => g.CategoryID == categoryId);

            if (brandId.HasValue)
                filteredItemGroups = filteredItemGroups.Where(g => g.BrandID == brandId);

            var data = filteredItemGroups
                .Select(g => new ItemGroupViewModel
                {
                    GroupID = g.GroupID,
                    Name = g.Name,
                    CategoryID = g.CategoryID,
                    CategoryName = g.Category.Name,
                    //BrandID = g.BrandID,
                    //BrandName = g.Brand.Name,
                    Description = g.Description,
                    ProductCount = 50
                })
                .ToList();

            ViewBag.Categories = new SelectList(await _categoriesService.GetAllCategoriesAsync(), "CategoryID", "Name");
            ViewBag.Brands = new SelectList(await _brandsService.GetAllBrandsAsync(), "BrandID", "Name");

            return View(data);
        }

        // GET: /ItemGroups/Create
        public async Task<IActionResult> Create()
        {
            await LoadDropdowns();
            return View(new ItemGroupCreateViewModel());
        }

        // POST: /ItemGroups/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ItemGroupCreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                await LoadDropdowns();
                return View(model);
            }

            var result = await _itemGroupsService.CreateItemGroupAsync(model);

            if (result.IsSuccess)
            {
                TempData["Success"] = "Item Group created successfully.";
                return RedirectToAction(nameof(Index));
            }

            ModelState.AddModelError("", result.Error);
            await LoadDropdowns();
            return View(model);
        }

        // GET: /ItemGroups/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var itemGroup = await _itemGroupsService.GetItemGroupByIdAsync(id);
            if (itemGroup == null)
                return NotFound();

            var model = new ItemGroupEditViewModel
            {
                GroupID = itemGroup.GroupID,
                Name = itemGroup.Name,
                CategoryID = itemGroup.CategoryID,
                BrandID = itemGroup.BrandID,
                Description = itemGroup.Description
            };

            await LoadDropdowns();
            return View(model);
        }

        // POST: /ItemGroups/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ItemGroupEditViewModel model)
        {
            if (!ModelState.IsValid)
            {
                await LoadDropdowns();
                return View(model);
            }

            var result = await _itemGroupsService.UpdateItemGroupAsync(id, model);

            if (result.IsSuccess)
            {
                TempData["Success"] = "Item Group updated successfully.";
                return RedirectToAction(nameof(Index));
            }

            ModelState.AddModelError("", result.Error);
            await LoadDropdowns();
            return View(model);
        }

        // POST: /ItemGroups/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _itemGroupsService.DeleteItemGroupAsync(id);

            if (result.IsSuccess)
                TempData["Success"] = result.Message ?? "Item Group deleted successfully.";
            else
                TempData["Error"] = result.Error;

            return RedirectToAction(nameof(Index));
        }

        private async Task LoadDropdowns()
        {
            ViewBag.Categories = new SelectList(await _categoriesService.GetAllCategoriesAsync(), "CategoryID", "Name");
            ViewBag.Brands = new SelectList(await _brandsService.GetAllBrandsAsync(), "BrandID", "Name");
        }
    }
}