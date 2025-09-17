using WarehouseManagement.Core.Common;
using WarehouseManagement.Core.Entities;
using WarehouseManagement.Core.Repositories;
using WarehouseManagement.Core.ViewModels;

namespace WarehouseManagement.Core.Services
{
    public interface IStockTransactionsService
    {
        Task<Result<StockTransaction>> CreateTransactionAsync(StockTransactionCreateViewModel model);
        Task<List<StockTransactionViewModel>> GetTransactionsByProductAsync(int productId);
        Task<decimal> GetCurrentStockAsync(int productId);
        Task<decimal> GetTotalSoldAsync(int productId);
        Task<decimal> GetSalesAmountAsync(int productId, DateTime startDate, DateTime endDate);
        Task<decimal> GetAverageUnitCostAsync(int productId);
    }
}