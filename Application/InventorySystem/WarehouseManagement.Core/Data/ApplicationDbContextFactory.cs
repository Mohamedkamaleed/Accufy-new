using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace WarehouseManagement.Core.Data
{
    public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            //var configuration = new ConfigurationBuilder()
            //    .SetBasePath(Directory.GetCurrentDirectory()) // Important: where appsettings.json is
            //    .AddJsonFile("appsettings.json", optional: true)
            //    .Build();

            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            //var connectionString = configuration.GetConnectionString("DefaultConnection");
            var connectionString = "Host=localhost;Port=5432;Database=WarehouseDb;Username=postgres;Password=Admin@123";

            optionsBuilder.UseNpgsql(connectionString); // or UseSqlServer, etc.

            return new ApplicationDbContext(optionsBuilder.Options);
        }
    }
}
