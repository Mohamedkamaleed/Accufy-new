using WarehouseManagement.Core.Common;
using WarehouseManagement.Core.Entities;
using WarehouseManagement.Core.Repositories;
using WarehouseManagement.Core.ViewModels;

namespace WarehouseManagement.Core.Services
{
    public interface IWarehouseService
    {
        Task<Result<Warehouse>> CreateWarehouseAsync(WarehouseCreateViewModel model);
        Task<Result<Warehouse>> UpdateWarehouseAsync(int id, WarehouseEditViewModel model);
        Task<Result> DeleteWarehouseAsync(int id);
        Task<Result> SetPrimaryWarehouseAsync(int id);
        Task<Warehouse> GetWarehouseByIdAsync(int id);
        Task<IEnumerable<Warehouse>> GetAllWarehousesAsync();
        Task<IEnumerable<Warehouse>> GetActiveWarehousesAsync();
        Task<Warehouse> GetPrimaryWarehouseAsync();
    }

    public class WarehouseService : IWarehouseService
    {
        private readonly IWarehouseRepository _repository;

        public WarehouseService(IWarehouseRepository repository)
        {
            _repository = repository;
        }

        public async Task<Result<Warehouse>> CreateWarehouseAsync(WarehouseCreateViewModel model)
        {
            var existing = await _repository.GetByNameAsync(model.Name);
            if (existing != null)
                return Result<Warehouse>.Failure("Warehouse name already exists");

            var warehouse = new Warehouse(model.Name, model.ShippingAddress, model.IsActive, model.IsPrimary);

            if (model.IsPrimary)
                await _repository.SetPrimaryWarehouseAsync(warehouse.Id);

            await _repository.AddAsync(warehouse);

            return Result<Warehouse>.Success(warehouse);
        }

        public async Task<Result<Warehouse>> UpdateWarehouseAsync(int id, WarehouseEditViewModel model)
        {
            var warehouse = await _repository.GetByIdAsync(id);
            if (warehouse == null)
                return Result<Warehouse>.Failure("Warehouse not found");

            // Check for duplicate name
            if (!string.Equals(warehouse.Name, model.Name, StringComparison.OrdinalIgnoreCase))
            {
                var existing = await _repository.GetByNameAsync(model.Name);
                if (existing != null)
                    return Result<Warehouse>.Failure("Warehouse name already exists");
            }
            // Rule 4: Warn if warehouse has transactions and is being set inactive
            if (!model.Status)
            {
                bool hasTransactions = await _repository.HasTransactionsAsync(id);
                if (hasTransactions)
                    return Result<Warehouse>.Failure("This warehouse has linked transactions. Cannot deactivate.");
            }

            // Rule 1: Cannot deactivate primary warehouse
            if (warehouse.IsPrimary && !model.Status)
                return Result<Warehouse>.Failure("Cannot deactivate the primary warehouse.");

            // Rule 2: Handle change to primary warehouse
            if (model.IsPrimary && !warehouse.IsPrimary)
            {
                // Automatically unset current primary warehouse
                var currentPrimary = await _repository.GetPrimaryWarehouseAsync();
                if (currentPrimary != null && currentPrimary.Id != warehouse.Id)
                {
                    currentPrimary.IsPrimary = false;
                    await _repository.UpdateAsync(currentPrimary);
                }

                warehouse.IsPrimary = true;
            }
            else if (!model.IsPrimary && warehouse.IsPrimary)
            {
                // Prevent unsetting primary flag manually
                return Result<Warehouse>.Failure("Cannot unset the primary warehouse via update.");
            }

            // Update warehouse fields
            warehouse.Update(model.Name, model.ShippingAddress, model.Status);

            await _repository.UpdateAsync(warehouse);

            return Result<Warehouse>.Success(warehouse);
        }


        public async Task<Result> DeleteWarehouseAsync(int id)
        {
            var warehouse = await _repository.GetByIdAsync(id);
            if (warehouse == null)
                return Result.Failure("Warehouse not found");

            // Rule 3: Cannot delete primary warehouse
            if (warehouse.IsPrimary)
                return Result.Failure("Cannot delete the primary warehouse");

            // Rule 1: Check for linked transactions
            bool hasTransactions = await _repository.HasTransactionsAsync(id);

            if (hasTransactions)
            {
                // Rule 2: Deactivate instead of delete
                if (!warehouse.Status)
                    return Result.Failure("Warehouse has transactions and is already inactive");

                warehouse.Update(warehouse.Name, warehouse.ShippingAddress, false);
                await _repository.UpdateAsync(warehouse);
                return Result.Success("Warehouse has transactions; it has been deactivated instead of deleted.");
            }

            await _repository.DeleteAsync(warehouse);
            return Result.Success("Warehouse deleted successfully.");
        }


        public async Task<Result> SetPrimaryWarehouseAsync(int id)
        {
            var warehouse = await _repository.GetByIdAsync(id);
            if (warehouse == null)
                return Result.Failure("Warehouse not found");

            if (!warehouse.Status)
                return Result.Failure("Cannot set an inactive warehouse as primary");

            var currentPrimary = await _repository.GetPrimaryWarehouseAsync();
            if (currentPrimary != null && currentPrimary.Id != id)
            {
                currentPrimary.IsPrimary = false;
                await _repository.UpdateAsync(currentPrimary);
            }

            warehouse.IsPrimary = true;
            await _repository.UpdateAsync(warehouse);

            return Result.Success();
        }


        public async Task<Warehouse> GetWarehouseByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<Warehouse>> GetAllWarehousesAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<IEnumerable<Warehouse>> GetActiveWarehousesAsync()
        {
            return await _repository.GetActiveWarehousesAsync();
        }

        public async Task<Warehouse> GetPrimaryWarehouseAsync()
        {
            return await _repository.GetPrimaryWarehouseAsync();
        }
    }

}
