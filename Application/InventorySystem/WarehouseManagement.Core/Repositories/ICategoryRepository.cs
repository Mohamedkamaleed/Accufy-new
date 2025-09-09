using Dapper;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;
using WarehouseManagement.Core.Entities;

namespace WarehouseManagement.Core.Repositories
{
    public interface ICategoryRepository
    {
        Task<IEnumerable<Category>> GetAllAsync();
        Task<Category?> GetByIdAsync(int id);
        Task<Category?> GetByNameAsync(string name);
        Task AddAsync(Category category);
        Task UpdateAsync(Category category);
        Task DeleteAsync(Category category);
    }
    public class CategoryRepository : ICategoryRepository
    {
        private readonly IDbConnection _db;

        public CategoryRepository(IConfiguration config)
        {
            _db = new SqlConnection(config.GetConnectionString("DefaultConnection"));
        }

        public async Task<IEnumerable<Category>> GetAllAsync()
        {
            var sql = "SELECT * FROM Categories";
            return await _db.QueryAsync<Category>(sql);
        }

        public async Task<Category?> GetByIdAsync(int id)
        {
            var sql = "SELECT * FROM Categories WHERE CategoryID = @Id";
            return await _db.QueryFirstOrDefaultAsync<Category>(sql, new { Id = id });
        }

        public async Task<Category?> GetByNameAsync(string name)
        {
            var sql = "SELECT * FROM Categories WHERE Name = @Name";
            return await _db.QueryFirstOrDefaultAsync<Category>(sql, new { Name = name });
        }

        public async Task AddAsync(Category category)
        {
            var sql = @"
            INSERT INTO Categories (Name, Description, ParentCategoryID)
            VALUES (@Name, @Description, @ParentCategoryID);
            SELECT CAST(SCOPE_IDENTITY() as int)";
            var id = await _db.QuerySingleAsync<int>(sql, category);
            category.CategoryID = id;
        }

        public async Task UpdateAsync(Category category)
        {
            var sql = @"
            UPDATE Categories
            SET Name = @Name,
                Description = @Description,
                ParentCategoryID = @ParentCategoryID
            WHERE CategoryID = @CategoryID";
            await _db.ExecuteAsync(sql, category);
        }

        public async Task DeleteAsync(Category category)
        {
            var sql = "DELETE FROM Categories WHERE CategoryID = @CategoryID";
            await _db.ExecuteAsync(sql, new { category.CategoryID });
        }
    }

}
