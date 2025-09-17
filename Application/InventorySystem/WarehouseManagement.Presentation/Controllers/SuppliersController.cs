using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WarehouseManagement.Core.Services;
using WarehouseManagement.Core.ViewModels;
using System.Linq;
using System.Threading.Tasks;

namespace WarehouseManagement.Presentation.Controllers
{
    public class SuppliersController : Controller
    {
        private readonly ISuppliersService _suppliersService;
        private readonly ICurrencyService _currencyService; // Assuming you have a currency service
        public IEnumerable<CurrencyViewModel> currenciesList = new List<CurrencyViewModel>();   
        public SuppliersController(ISuppliersService suppliersService, ICurrencyService currencyService)
        {
            _suppliersService = suppliersService;
            _currencyService = currencyService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string search, string city, string country, bool? activeStatus)
        {
            IEnumerable<SupplierListViewModel> suppliers;

            if (!string.IsNullOrWhiteSpace(search))
            {
                suppliers = await _suppliersService.SearchSuppliersAsync(search);
            }
            else
            {
                suppliers = await _suppliersService.GetAllSuppliersAsync();
            }

            // Apply additional filters
            if (!string.IsNullOrWhiteSpace(city))
            {
                suppliers = suppliers.Where(s => s.City?.Contains(city, StringComparison.OrdinalIgnoreCase) == true);
            }

            if (!string.IsNullOrWhiteSpace(country))
            {
                suppliers = suppliers.Where(s => s.Country?.Contains(country, StringComparison.OrdinalIgnoreCase) == true);
            }

            if (activeStatus.HasValue)
            {
                suppliers = suppliers.Where(s => s.IsActive == activeStatus.Value);
            }

            return View(suppliers.ToList());
        }

        // GET: /Suppliers/Create
        public async Task<IActionResult> Create()
        {
            var model = new SupplierCreateViewModel();

            // Load currencies for dropdown
            var currencies = await _currencyService.GetAllCurrenciesAsync();
            ViewBag.Currencies = new SelectList(currencies, "Code", "Name", model.Currency);

            return View(model);
        }

        // POST: /Suppliers/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SupplierCreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                // Reload currencies for dropdown
                currenciesList = await _currencyService.GetAllCurrenciesAsync();
                ViewBag.Currencies = new SelectList(currenciesList, "Code", "Name", model.Currency);
                return View(model);
            }

            // Set default opening balance date if not provided
            if (model.OpeningBalance != 0 && !model.OpeningBalanceDate.HasValue)
            {
                model.OpeningBalanceDate = DateTime.UtcNow;
            }

            var result = await _suppliersService.CreateSupplierAsync(model, User.Identity.Name);

            if (result.Succeeded)
            {
                TempData["Success"] = "Supplier created successfully.";
                return RedirectToAction(nameof(Index));
            }

            ModelState.AddModelError("", result.Error);

            // Reload currencies for dropdown
            var currencies = await _currencyService.GetAllCurrenciesAsync();
            ViewBag.Currencies = new SelectList(currencies, "Code", "Name", model.Currency);

            return View(model);
        }

        // GET: /Suppliers/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var supplier = await _suppliersService.GetSupplierDetailAsync(id);
            if (supplier == null)
                return NotFound();

            var model = new SupplierEditViewModel
            {
                SupplierID = supplier.SupplierID,
                BusinessName = supplier.BusinessName,
                FirstName = supplier.FirstName,
                LastName = supplier.LastName,
                Telephone = supplier.Telephone,
                Mobile = supplier.Mobile,
                StreetAddress1 = supplier.StreetAddress1,
                StreetAddress2 = supplier.StreetAddress2,
                City = supplier.City,
                State = supplier.State,
                PostalCode = supplier.PostalCode,
                Country = supplier.Country,
                CommercialRegistration = supplier.CommercialRegistration,
                TaxID = supplier.TaxID,
                SupplierNumber = supplier.SupplierNumber,
                Currency = supplier.Currency,
                OpeningBalance = supplier.OpeningBalance,
                OpeningBalanceDate = supplier.OpeningBalanceDate,
                Email = supplier.Email,
                Notes = supplier.Notes,
                IsActive = supplier.IsActive,
                Contacts = supplier.Contacts
            };

            // Load currencies for dropdown
            var currencies = await _currencyService.GetAllCurrenciesAsync();
            ViewBag.Currencies = new SelectList(currencies, "Code", "Name", model.Currency);

            return View(model);
        }

        // POST: /Suppliers/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, SupplierEditViewModel model)
        {
            if (id != model.SupplierID)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                // Reload currencies for dropdown
                var currenciesList = await _currencyService.GetAllCurrenciesAsync();
                ViewBag.Currencies = new SelectList(currenciesList, "Code", "Name", model.Currency);
                return View(model);
            }

            var result = await _suppliersService.UpdateSupplierAsync(id, model, User.Identity.Name);

            if (result.Succeeded)
            {
                TempData["Success"] = "Supplier updated successfully.";
                return RedirectToAction(nameof(Index));
            }

            ModelState.AddModelError("", result.Error);

            // Reload currencies for dropdown
            var currencies = await _currencyService.GetAllCurrenciesAsync();
            ViewBag.Currencies = new SelectList(currencies, "Code", "Name", model.Currency);

            return View(model);
        }

        // GET: /Suppliers/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var supplier = await _suppliersService.GetSupplierDetailAsync(id);
            if (supplier == null)
                return NotFound();

            return View(supplier);
        }

        // POST: /Suppliers/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _suppliersService.DeleteSupplierAsync(id);

            if (result.Succeeded)
            {
                TempData["Success"] = "Supplier deleted successfully.";
            }
            else
            {
                TempData["Error"] = result.Error;
            }

            return RedirectToAction(nameof(Index));
        }

        // AJAX action to add contact row in the form
        [HttpPost]
        public IActionResult AddContactRow(int index)
        {
            ViewData["ContactIndex"] = index;
            return PartialView("_ContactEditor", new SupplierContactViewModel());
        }

        // AJAX action to validate supplier number
        [AcceptVerbs("GET", "POST")]
        public async Task<IActionResult> VerifySupplierNumber(string supplierNumber, int supplierId = 0)
        {
            var existing = await _suppliersService.GetSupplierByNumberAsync(supplierNumber);

            if (existing != null && existing.SupplierID != supplierId)
            {
                return Json($"Supplier number {supplierNumber} is already in use.");
            }

            return Json(true);
        }

        // AJAX action to validate business name
        [AcceptVerbs("GET", "POST")]
        public async Task<IActionResult> VerifyBusinessName(string businessName, int supplierId = 0)
        {
            var existing = await _suppliersService.GetSupplierByBusinessNameAsync(businessName);

            if (existing != null && existing.SupplierID != supplierId)
            {
                return Json($"Business name {businessName} is already in use.");
            }

            return Json(true);
        }
    }
}