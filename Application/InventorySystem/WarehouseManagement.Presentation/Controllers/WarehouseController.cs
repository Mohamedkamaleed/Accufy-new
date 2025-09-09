using Microsoft.AspNetCore.Mvc;
using WarehouseManagement.Core.Services;
using WarehouseManagement.Core.ViewModels;
using WarehouseManagement.Core.ViewModels.Common;

namespace WarehouseManagement.Presentation.Controllers
{
    //[Authorize]
    public class WarehouseController : Controller
    {
        private readonly IWarehouseService _warehouseService;

        public WarehouseController(IWarehouseService warehouseService)
        {
            _warehouseService = warehouseService;
        }

        // GET: /Warehouse
        public async Task<IActionResult> Index(string search, string status, string primaryFilter, int page = 1, int pageSize = 10)
        {
            var warehouses = await _warehouseService.GetAllWarehousesAsync();

            if (!string.IsNullOrWhiteSpace(search))
                warehouses = warehouses.Where(w => w.Name.Contains(search, StringComparison.OrdinalIgnoreCase)).ToList();

            if (!string.IsNullOrWhiteSpace(status))
            {
                bool isActive = status.Equals("Active", StringComparison.OrdinalIgnoreCase);
                warehouses = warehouses.Where(w => w.Status == isActive).ToList();
            }

            if (!string.IsNullOrWhiteSpace(primaryFilter))
            {
                bool isPrimary = primaryFilter.Equals("Primary", StringComparison.OrdinalIgnoreCase);
                warehouses = warehouses.Where(w => w.IsPrimary == isPrimary).ToList();
            }

            var totalCount = warehouses.Count();
            var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
            var pagedItems = warehouses.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            var viewModel = new PagedResult<WarehouseListViewModel>
            {
                PageNumber = page,
                TotalPages = totalPages,
                Items = pagedItems.Select(w => new WarehouseListViewModel
                {
                    Id = w.Id,
                    Name = w.Name,
                    ShippingAddress = w.ShippingAddress,
                    Status = w.Status ? "Active" : "Inactive",
                    IsPrimary = w.IsPrimary
                })
            };

            return View(viewModel);
        }



        // GET: /Warehouse/Create
        public IActionResult Create()
        {
            return View(new WarehouseCreateViewModel());
        }

        // POST: /Warehouse/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(WarehouseCreateViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var result = await _warehouseService.CreateWarehouseAsync(model);

            if (result.IsSuccess)
            {
                TempData["Success"] = "Warehouse created successfully.";
                return RedirectToAction(nameof(Index));
            }

            ModelState.AddModelError("", result.Error);
            return View(model);
        }

        // GET: /Warehouse/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var warehouse = await _warehouseService.GetWarehouseByIdAsync(id);
            if (warehouse == null)
                return NotFound();

            var model = new WarehouseEditViewModel
            {
                Id = warehouse.Id, // ✅ Make sure to include this
                Name = warehouse.Name,
                ShippingAddress = warehouse.ShippingAddress,
                Status = warehouse.Status,
                IsPrimary = warehouse.IsPrimary
            };

            return View(model);
        }


        // POST: /Warehouse/Edit/5
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(WarehouseEditViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var result = await _warehouseService.UpdateWarehouseAsync(model.Id, model);

            if (result.IsSuccess)
            {
                TempData["Success"] = "Warehouse updated successfully.";
                return RedirectToAction(nameof(Index));
            }

            ModelState.AddModelError("", result.Error);
            return View(model);
        }


        // POST: /Warehouse/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _warehouseService.DeleteWarehouseAsync(id);

            if (result.IsSuccess)
            {
                TempData["Success"] = result.Message; // Use actual message (deleted or deactivated)
            }
            else
            {
                TempData["Error"] = result.Error;
            }

            return RedirectToAction(nameof(Index));
        }


        // POST: /Warehouse/SetPrimary/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SetPrimary(int id)
        {
            var result = await _warehouseService.SetPrimaryWarehouseAsync(id);

            if (result.Succeeded)
                TempData["Success"] = "Warehouse set as primary successfully.";
            else
                TempData["Error"] = result.Error;

            return RedirectToAction(nameof(Index));
        }

    }

}
