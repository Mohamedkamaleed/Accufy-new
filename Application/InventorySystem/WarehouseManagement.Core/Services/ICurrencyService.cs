using WarehouseManagement.Core.Common;
using WarehouseManagement.Core.Entities;
using WarehouseManagement.Core.Repositories;
using WarehouseManagement.Core.ViewModels;

namespace WarehouseManagement.Core.Services
{
    public interface ICurrencyService
    {
        Task<IEnumerable<CurrencyViewModel>> GetAllCurrenciesAsync();
        Task<CurrencyViewModel?> GetCurrencyByCodeAsync(string code);
    }

    public class CurrencyViewModel
    {
        public string Code { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Symbol { get; set; } = null!;
    }
}
