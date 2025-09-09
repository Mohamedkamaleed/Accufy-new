using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using WarehouseManagement.Core.Entities;

namespace WarehouseManagement.Core.Repositories
{
    public interface IWarehouseRepository
    {
        Task<IEnumerable<Warehouse>> GetAllAsync();
        Task<Warehouse> GetByIdAsync(int id);
        Task<Warehouse> GetByNameAsync(string name);
        Task<Warehouse> GetPrimaryWarehouseAsync();
        Task<IEnumerable<Warehouse>> GetActiveWarehousesAsync();
        Task<bool> HasTransactionsAsync(int warehouseId); // Placeholder for future logic
        Task AddAsync(Warehouse warehouse);
        Task UpdateAsync(Warehouse warehouse);
        Task DeleteAsync(Warehouse warehouse);
        Task SetPrimaryWarehouseAsync(int id);
    }
    public class WarehouseRepository : IWarehouseRepository
    {
        private readonly IDbConnection _db;

        public WarehouseRepository(IConfiguration config)
        {
            _db = new SqlConnection(config.GetConnectionString("DefaultConnection"));
        }

        public async Task<IEnumerable<Warehouse>> GetAllAsync() =>
            await _db.QueryAsync<Warehouse>("SELECT * FROM Warehouses");

        public async Task<Warehouse> GetByIdAsync(int id) =>
            await _db.QueryFirstOrDefaultAsync<Warehouse>("SELECT * FROM Warehouses WHERE Id = @id", new { id });

        public async Task<Warehouse> GetByNameAsync(string name) =>
            await _db.QueryFirstOrDefaultAsync<Warehouse>("SELECT * FROM Warehouses WHERE Name = @name", new { name });

        public async Task<Warehouse> GetPrimaryWarehouseAsync() =>
            await _db.QueryFirstOrDefaultAsync<Warehouse>("SELECT * FROM Warehouses WHERE IsPrimary = 1");

        public async Task<IEnumerable<Warehouse>> GetActiveWarehousesAsync() =>
            await _db.QueryAsync<Warehouse>("SELECT * FROM Warehouses WHERE Status = 1");

        public async Task<bool> HasTransactionsAsync(int warehouseId)
        {
            // Example stub: replace with actual transaction check
            return false;
        }

        public async Task AddAsync(Warehouse warehouse)
        {
            string sql = @"
            INSERT INTO Warehouses (Name, ShippingAddress, Status, IsPrimary)
            VALUES (@Name, @ShippingAddress, @Status, @IsPrimary)";
            await _db.ExecuteAsync(sql, warehouse);
        }

        public async Task UpdateAsync(Warehouse warehouse)
        {
            string sql = @"
            UPDATE Warehouses
            SET Name = @Name,
                ShippingAddress = @ShippingAddress,
                Status = @Status,
                IsPrimary = @IsPrimary
            WHERE Id = @Id";
            await _db.ExecuteAsync(sql, warehouse);
        }

        public async Task DeleteAsync(Warehouse warehouse)
        {
            string sql = "DELETE FROM Warehouses WHERE Id = @Id";
            await _db.ExecuteAsync(sql, new { warehouse.Id });
        }

        public async Task SetPrimaryWarehouseAsync(int id)
        {
            // Clear old primary
            await _db.ExecuteAsync("UPDATE Warehouses SET IsPrimary = 0 WHERE IsPrimary = 1");
            // Set new primary
            await _db.ExecuteAsync("UPDATE Warehouses SET IsPrimary = 1 WHERE Id = @id", new { id });
        }
    }
}
