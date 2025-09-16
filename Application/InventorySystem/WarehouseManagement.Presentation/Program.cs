using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using WarehouseManagement.Core.Data;
using WarehouseManagement.Core.Data.Seeding;
using WarehouseManagement.Core.Repositories;
using WarehouseManagement.Core.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Add DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});
// Remove this line:
// builder.Services.AddScoped<IDataSeeder, CompositeDataSeeder>();

// Keep these:
builder.Services.AddScoped<IDataSeeder, CategoryDataSeeder>();
builder.Services.AddScoped<IDataSeeder, WarehouseDataSeeder>();
builder.Services.AddScoped<IDataSeeder, BrandDataSeeder>();
builder.Services.AddScoped<IDataSeeder, SupplierDataSeeder>();
builder.Services.AddScoped<IDataSeeder, ServiceDataSeeder>();
builder.Services.AddScoped<IDataSeeder, ItemGroupDataSeeder>();
builder.Services.AddScoped<IDataSeeder, ItemGroupItemDataSeeder>();
builder.Services.AddScoped<IDataSeeder, ProductTaxProfileDataSeeder>();
// Register seeders
builder.Services.AddScoped<IDataSeeder, DefaultTaxDataSeeder>();
builder.Services.AddScoped<IDataSeeder, TaxProfileDataSeeder>();
// Add CompositeDataSeeder as a separate service
builder.Services.AddScoped<CompositeDataSeeder>();
builder.Services.AddScoped<IDbConnectionFactory, SqlConnectionFactory>();

builder.Services.AddScoped<IWarehouseRepository, WarehouseRepository>();
builder.Services.AddScoped<IWarehouseService, WarehouseService>();


builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<ICategoryService, CategoryService>();


builder.Services.AddScoped<IBrandsRepository, BrandsRepository>();
builder.Services.AddScoped<IBrandsService, BrandsService>();


builder.Services.AddScoped<ISuppliersRepository, SuppliersRepository>();
builder.Services.AddScoped<ISuppliersService, SuppliersService>();

// Register repositories
builder.Services.AddScoped<IProductsRepository, ProductsRepository>();

// Register services
builder.Services.AddScoped<IProductsService, ProductsService>();
// Register repositories
builder.Services.AddScoped<IItemGroupsRepository, ItemGroupsRepository>();

// Register services
builder.Services.AddScoped<IItemGroupsService, ItemGroupsService>();
// Register repositories
builder.Services.AddScoped<IItemGroupItemsRepository, ItemGroupItemsRepository>();

// Register services
builder.Services.AddScoped<IItemGroupItemsService, ItemGroupItemsService>();

// Register repositories
builder.Services.AddScoped<IProductTaxProfilesRepository, ProductTaxProfilesRepository>();
builder.Services.AddScoped<ITaxProfilesRepository, TaxProfilesRepository>();

// Register services
builder.Services.AddScoped<IProductTaxProfilesService, ProductTaxProfilesService>();



// Register repositories
builder.Services.AddScoped<IDefaultTaxesRepository, DefaultTaxesRepository>();
builder.Services.AddScoped<ITaxProfilesRepository, TaxProfilesRepository>();
builder.Services.AddScoped<ITaxProfileTaxesRepository, TaxProfileTaxesRepository>();

// Register services
builder.Services.AddScoped<IDefaultTaxesService, DefaultTaxesService>();
builder.Services.AddScoped<ITaxProfilesService, TaxProfilesService>();



builder.Services.AddControllersWithViews()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    });

var app = builder.Build();

// Improved seeding execution
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    var seeders = scope.ServiceProvider.GetServices<IDataSeeder>();

    // Ensure database is created and migrated
    await context.Database.MigrateAsync();
    var compositeSeeder = scope.ServiceProvider.GetRequiredService<CompositeDataSeeder>();
    await compositeSeeder.SeedAsync(context);
    // Run all seeders with transaction and error handling
    //foreach (var seeder in seeders)
    //{
    //    try
    //    {
    //        await seeder.SeedAsync(context);
    //    }
    //    catch (Exception ex)
    //    {
    //        // Log the error but don't stop application startup
    //        Console.WriteLine($"Seeder error: {ex.Message}");
    //    }
    //}
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();