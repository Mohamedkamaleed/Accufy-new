using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WarehouseManagement.Core.Services;
using WarehouseManagement.Core.ViewModels;

namespace WarehouseManagement.Presentation.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        // GET: /Categories
        //public async Task<IActionResult> Index(string search, int? parentCategoryId, int page = 1, int pageSize = 10)
        //{
        //    var categories = await _categoryService.GetAllCategoriesAsync();

        //    if (!string.IsNullOrWhiteSpace(search))
        //        categories = categories.Where(c => c.Name.Contains(search, StringComparison.OrdinalIgnoreCase)).ToList();

        //    if (parentCategoryId.HasValue)
        //        categories = categories.Where(c => c.ParentCategoryID == parentCategoryId).ToList();

        //    var totalCount = categories.Count();
        //    var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
        //    var pagedItems = categories.Skip((page - 1) * pageSize).Take(pageSize).ToList();

        //    var viewModel = new PagedResult<CategoryListViewModel>
        //    {
        //        PageNumber = page,
        //        TotalPages = totalPages,
        //        TotalCount = totalCount,  // <-- Add this
        //        Items = pagedItems.Select(c => new CategoryListViewModel
        //        {
        //            CategoryID = c.CategoryID,
        //            Name = c.Name,
        //            Description = c.Description,
        //            ParentCategoryName = categories.FirstOrDefault(pc => pc.CategoryID == c.ParentCategoryID)?.Name
        //        })
        //    };
        //    ViewData["PageSize"] = pageSize;

        //    return View(viewModel);
        //}

        [HttpGet]
        public async Task<IActionResult> Index(string search, int? parentCategoryId)
        {
            var allCategories = await _categoryService.GetAllCategoriesAsync();

            var filteredCategories = allCategories.AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                filteredCategories = filteredCategories
                    .Where(c => c.Name.Contains(search, StringComparison.OrdinalIgnoreCase));
            }

            if (parentCategoryId.HasValue)
            {
                filteredCategories = filteredCategories
                    .Where(c => c.ParentCategoryID == parentCategoryId);
            }

            var data = filteredCategories
         .AsEnumerable() // ✅ Force in-memory LINQ
         .Select(c => new CategoryListViewModel
         {
             CategoryID = c.CategoryID,
             Name = c.Name,
             Description = c.Description,
             ParentCategoryName = allCategories
                 .FirstOrDefault(pc => pc.CategoryID == c.ParentCategoryID)?.Name ?? "—"
         })
         .ToList();

            var allCats = await _categoryService.GetAllCategoriesAsync();
            ViewBag.ParentCategories = new SelectList(allCats, "CategoryID", "Name", parentCategoryId);

            return View(data); // ✅ Return the view with model
        }



        // GET: /Categories/Create
        public async Task<IActionResult> Create()
        {
            // Load parent categories for dropdown if needed
            var allCategories = await _categoryService.GetAllCategoriesAsync();
            ViewBag.ParentCategories = new SelectList(allCategories, "CategoryID", "Name");
            return View(new CategoryCreateViewModel());
        }

        // POST: /Categories/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CategoryCreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var allCategories = await _categoryService.GetAllCategoriesAsync();
                ViewBag.ParentCategories = new SelectList(allCategories, "CategoryID", "Name", model.ParentCategoryID);
                return View(model);
            }

            var result = await _categoryService.CreateCategoryAsync(model);

            if (result.IsSuccess)
            {
                TempData["Success"] = "Category created successfully.";
                return RedirectToAction(nameof(Index));
            }

            ModelState.AddModelError("", result.Error);
            var allCats = await _categoryService.GetAllCategoriesAsync();
            ViewBag.ParentCategories = new SelectList(allCats, "CategoryID", "Name", model.ParentCategoryID);
            return View(model);
        }

        // GET: /Categories/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var category = await _categoryService.GetCategoryByIdAsync(id);
            if (category == null)
                return NotFound();

            var model = new CategoryEditViewModel
            {
                Name = category.Name,
                Description = category.Description,
                ParentCategoryID = category.ParentCategoryID
            };

            var allCategories = await _categoryService.GetAllCategoriesAsync();
            ViewBag.ParentCategories = new SelectList(allCategories.Where(c => c.CategoryID != id), "CategoryID", "Name", category.ParentCategoryID);

            return View(model);
        }

        // POST: /Categories/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CategoryEditViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var allCategories = await _categoryService.GetAllCategoriesAsync();
                ViewBag.ParentCategories = new SelectList(allCategories.Where(c => c.CategoryID != id), "CategoryID", "Name", model.ParentCategoryID);
                return View(model);
            }

            var result = await _categoryService.UpdateCategoryAsync(id, model);

            if (result.IsSuccess)
            {
                TempData["Success"] = "Category updated successfully.";
                return RedirectToAction(nameof(Index));
            }

            ModelState.AddModelError("", result.Error);
            var allCats = await _categoryService.GetAllCategoriesAsync();
            ViewBag.ParentCategories = new SelectList(allCats.Where(c => c.CategoryID != id), "CategoryID", "Name", model.ParentCategoryID);
            return View(model);
        }

        // POST: /Categories/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _categoryService.DeleteCategoryAsync(id);

            if (result.IsSuccess)
                TempData["Success"] = result.Message ?? "Category deleted successfully.";
            else
                TempData["Error"] = result.Error;

            return RedirectToAction(nameof(Index));
        }
    }

}
