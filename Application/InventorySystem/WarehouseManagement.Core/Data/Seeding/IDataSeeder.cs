using Microsoft.EntityFrameworkCore;
using WarehouseManagement.Core.Entities;

namespace WarehouseManagement.Core.Data.Seeding
{
    public interface IDataSeeder
    {
        Task SeedAsync(ApplicationDbContext context, CancellationToken cancellationToken = default);
    }
    public class WarehouseDataSeeder : IDataSeeder
    {
        public async Task SeedAsync(ApplicationDbContext context, CancellationToken cancellationToken = default)
        {
            // Use a transaction to ensure all operations succeed or fail together
            using var transaction = await context.Database.BeginTransactionAsync(cancellationToken);

            try
            {
                // Check if a primary warehouse already exists
                var hasPrimaryWarehouse = await context.Warehouses
                    .AnyAsync(w => w.IsPrimary, cancellationToken);

                if (!hasPrimaryWarehouse)
                {
                    // Create primary warehouse if none exists
                    var primaryWarehouse = new Warehouse(
                        "Main Warehouse",
                        "123 Main St, City, State",
                        true,
                        true
                    );

                    await context.Warehouses.AddAsync(primaryWarehouse, cancellationToken);
                    await context.SaveChangesAsync(cancellationToken);
                }

                // Check if secondary warehouses exist
                var warehouseNames = await context.Warehouses
                    .Select(w => w.Name)
                    .ToListAsync(cancellationToken);

                if (!warehouseNames.Contains("West Coast Distribution"))
                {
                    var secondaryWarehouse = new Warehouse(
                        "West Coast Distribution",
                        "456 Ocean Ave, Los Angeles, CA",
                        true,
                        false
                    );

                    await context.Warehouses.AddAsync(secondaryWarehouse, cancellationToken);
                }

                if (!warehouseNames.Contains("Old Storage"))
                {
                    var inactiveWarehouse = new Warehouse(
                        "Old Storage",
                        "789 Legacy Rd, Old Town",
                        false,
                        false
                    );

                    await context.Warehouses.AddAsync(inactiveWarehouse, cancellationToken);
                }

                await context.SaveChangesAsync(cancellationToken);
                await transaction.CommitAsync(cancellationToken);
            }
            catch
            {
                await transaction.RollbackAsync(cancellationToken);
                throw;
            }
        }
    }

    public class CategoryDataSeeder : IDataSeeder
    {
        public async Task SeedAsync(ApplicationDbContext context, CancellationToken cancellationToken = default)
        {
            // Check if specific categories already exist by name to avoid duplicates
            var existingCategories = await context.Categories
                .Select(c => c.Name)
                .ToListAsync(cancellationToken);

            var categoriesToAdd = new List<Category>();

            if (!existingCategories.Contains("Electronics"))
            {
                categoriesToAdd.Add(new Category
                {
                    Name = "Electronics",
                    Description = "Electronic products"
                });
            }

            if (!existingCategories.Contains("Furniture"))
            {
                categoriesToAdd.Add(new Category
                {
                    Name = "Furniture",
                    Description = "Office and home furniture"
                });
            }

            if (categoriesToAdd.Any())
            {
                await context.Categories.AddRangeAsync(categoriesToAdd, cancellationToken);
                await context.SaveChangesAsync(cancellationToken);
            }

            // Now set up parent-child relationships
            var allCategories = await context.Categories.ToListAsync(cancellationToken);
            var electronics = allCategories.FirstOrDefault(c => c.Name == "Electronics");
            var furniture = allCategories.FirstOrDefault(c => c.Name == "Furniture");

            // Add child categories if they don't exist
            if (electronics != null && !allCategories.Any(c => c.Name == "Computers"))
            {
                context.Categories.Add(new Category
                {
                    Name = "Computers",
                    Description = "Computer systems and accessories",
                    ParentCategoryID = electronics.CategoryID
                });
            }

            if (electronics != null && !allCategories.Any(c => c.Name == "Smartphones"))
            {
                context.Categories.Add(new Category
                {
                    Name = "Smartphones",
                    Description = "Mobile phones",
                    ParentCategoryID = electronics.CategoryID
                });
            }

            if (furniture != null && !allCategories.Any(c => c.Name == "Office Chairs"))
            {
                context.Categories.Add(new Category
                {
                    Name = "Office Chairs",
                    Description = "Chairs for office use",
                    ParentCategoryID = furniture.CategoryID
                });
            }

            await context.SaveChangesAsync(cancellationToken);
        }
    }
    public class BrandDataSeeder : IDataSeeder
    {
        public async Task SeedAsync(ApplicationDbContext context, CancellationToken cancellationToken = default)
        {
            // Check if specific brands already exist by name to avoid duplicates
            var existingBrands = await context.Brands
                .Select(b => b.Name)
                .ToListAsync(cancellationToken);

            var brandsToAdd = new List<Brand>();

            // Add popular electronics brands
            if (!existingBrands.Contains("Apple"))
            {
                brandsToAdd.Add(new Brand { Name = "Apple" });
            }

            if (!existingBrands.Contains("Samsung"))
            {
                brandsToAdd.Add(new Brand { Name = "Samsung" });
            }

            if (!existingBrands.Contains("Dell"))
            {
                brandsToAdd.Add(new Brand { Name = "Dell" });
            }

            if (!existingBrands.Contains("HP"))
            {
                brandsToAdd.Add(new Brand { Name = "HP" });
            }

            if (!existingBrands.Contains("Lenovo"))
            {
                brandsToAdd.Add(new Brand { Name = "Lenovo" });
            }

            if (!existingBrands.Contains("Sony"))
            {
                brandsToAdd.Add(new Brand { Name = "Sony" });
            }

            // Add furniture brands
            if (!existingBrands.Contains("IKEA"))
            {
                brandsToAdd.Add(new Brand { Name = "IKEA" });
            }

            if (!existingBrands.Contains("Herman Miller"))
            {
                brandsToAdd.Add(new Brand { Name = "Herman Miller" });
            }

            if (!existingBrands.Contains("Steelcase"))
            {
                brandsToAdd.Add(new Brand { Name = "Steelcase" });
            }

            if (brandsToAdd.Any())
            {
                await context.Brands.AddRangeAsync(brandsToAdd, cancellationToken);
                await context.SaveChangesAsync(cancellationToken);
            }
        }
    }

    public class SupplierDataSeeder : IDataSeeder
    {
        public async Task SeedAsync(ApplicationDbContext context, CancellationToken cancellationToken = default)
        {
            // Check if specific suppliers already exist by name to avoid duplicates
            var existingSuppliers = await context.Suppliers
                .Select(s => s.Name)
                .ToListAsync(cancellationToken);

            var suppliersToAdd = new List<Supplier>();

            // Add electronics suppliers
            if (!existingSuppliers.Contains("Tech Distributors Inc."))
            {
                suppliersToAdd.Add(new Supplier { Name = "Tech Distributors Inc." });
            }

            if (!existingSuppliers.Contains("Global Electronics Supply"))
            {
                suppliersToAdd.Add(new Supplier { Name = "Global Electronics Supply" });
            }

            if (!existingSuppliers.Contains("Mobile Device Wholesalers"))
            {
                suppliersToAdd.Add(new Supplier { Name = "Mobile Device Wholesalers" });
            }

            if (!existingSuppliers.Contains("Computer Parts Unlimited"))
            {
                suppliersToAdd.Add(new Supplier { Name = "Computer Parts Unlimited" });
            }

            // Add furniture suppliers
            if (!existingSuppliers.Contains("Office Furniture Solutions"))
            {
                suppliersToAdd.Add(new Supplier { Name = "Office Furniture Solutions" });
            }

            if (!existingSuppliers.Contains("Home & Office Decor Ltd."))
            {
                suppliersToAdd.Add(new Supplier { Name = "Home & Office Decor Ltd." });
            }

            if (!existingSuppliers.Contains("Ergonomic Workspace Suppliers"))
            {
                suppliersToAdd.Add(new Supplier { Name = "Ergonomic Workspace Suppliers" });
            }

            // Add general suppliers
            if (!existingSuppliers.Contains("General Wholesale Distributors"))
            {
                suppliersToAdd.Add(new Supplier { Name = "General Wholesale Distributors" });
            }

            if (!existingSuppliers.Contains("Regional Supply Chain Partners"))
            {
                suppliersToAdd.Add(new Supplier { Name = "Regional Supply Chain Partners" });
            }

            if (suppliersToAdd.Any())
            {
                await context.Suppliers.AddRangeAsync(suppliersToAdd, cancellationToken);
                await context.SaveChangesAsync(cancellationToken);
            }
        }
    }

    public class ServiceDataSeeder : IDataSeeder
    {
        public async Task SeedAsync(ApplicationDbContext context, CancellationToken cancellationToken = default)
        {
            // Check if services already exist
            if (await context.Services.AnyAsync(cancellationToken))
            {
                return;
            }

            // Get categories and suppliers for relationships
            var categories = await context.Categories.ToListAsync(cancellationToken);
            var suppliers = await context.Suppliers.ToListAsync(cancellationToken);

            var electronicsCategory = categories.FirstOrDefault(c => c.Name == "Electronics");
            var furnitureCategory = categories.FirstOrDefault(c => c.Name == "Furniture");
            var techSupplier = suppliers.FirstOrDefault(s => s.Name.Contains("Tech"));
            var furnitureSupplier = suppliers.FirstOrDefault(s => s.Name.Contains("Furniture"));

            var services = new List<Service>
        {
            new Service
            {
                Name = "Device Repair Service",
                Code = "DEV-REP-001",
                Description = "Professional repair service for electronic devices",
                CategoryID = electronicsCategory?.CategoryID,
                SupplierID = techSupplier?.SupplierID,
                PurchasePrice = 25.00m,
                UnitPrice = 75.00m,
                MinPrice = 50.00m,
                Status = true
            },
            new Service
            {
                Name = "IT Support Consultation",
                Code = "IT-SUP-001",
                Description = "Professional IT support and consultation services",
                CategoryID = electronicsCategory?.CategoryID,
                SupplierID = techSupplier?.SupplierID,
                PurchasePrice = 40.00m,
                UnitPrice = 100.00m,
                MinPrice = 80.00m,
                Status = true
            },
            new Service
            {
                Name = "Furniture Assembly",
                Code = "FURN-ASS-001",
                Description = "Professional furniture assembly service",
                CategoryID = furnitureCategory?.CategoryID,
                SupplierID = furnitureSupplier?.SupplierID,
                PurchasePrice = 30.00m,
                UnitPrice = 80.00m,
                MinPrice = 60.00m,
                Status = true
            },
            new Service
            {
                Name = "Office Setup Service",
                Code = "OFF-SET-001",
                Description = "Complete office setup and organization service",
                CategoryID = furnitureCategory?.CategoryID,
                SupplierID = furnitureSupplier?.SupplierID,
                PurchasePrice = 50.00m,
                UnitPrice = 150.00m,
                MinPrice = 120.00m,
                Status = true
            }
        };

            await context.Services.AddRangeAsync(services, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);
        }
    }

    public class ItemGroupDataSeeder : IDataSeeder
    {
        public async Task SeedAsync(ApplicationDbContext context, CancellationToken cancellationToken = default)
        {
            // Check if item groups already exist
            if (await context.ItemGroups.AnyAsync(cancellationToken))
            {
                return;
            }

            // Get categories and brands for relationships
            var categories = await context.Categories.ToListAsync(cancellationToken);
            var brands = await context.Brands.ToListAsync(cancellationToken);

            var electronicsCategory = categories.FirstOrDefault(c => c.Name == "Electronics");
            var furnitureCategory = categories.FirstOrDefault(c => c.Name == "Furniture");
            var appleBrand = brands.FirstOrDefault(b => b.Name == "Apple");
            var samsungBrand = brands.FirstOrDefault(b => b.Name == "Samsung");
            var ikeaBrand = brands.FirstOrDefault(b => b.Name == "IKEA");

            var itemGroups = new List<ItemGroup>
        {
            new ItemGroup
            {
                Name = "Apple Devices",
                CategoryID = electronicsCategory?.CategoryID,
                BrandID = appleBrand?.BrandID,
                Description = "All Apple products including iPhones, MacBooks, and accessories"
            },
            new ItemGroup
            {
                Name = "Samsung Electronics",
                CategoryID = electronicsCategory?.CategoryID,
                BrandID = samsungBrand?.BrandID,
                Description = "Samsung smartphones, tablets, and electronics"
            },
            new ItemGroup
            {
                Name = "IKEA Furniture Sets",
                CategoryID = furnitureCategory?.CategoryID,
                BrandID = ikeaBrand?.BrandID,
                Description = "Complete furniture sets from IKEA"
            },
            new ItemGroup
            {
                Name = "Office Chairs Collection",
                CategoryID = furnitureCategory?.CategoryID,
                Description = "Various office chairs from different brands"
            }
        };

            await context.ItemGroups.AddRangeAsync(itemGroups, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);
        }
    }


    public class ItemGroupItemDataSeeder : IDataSeeder
    {
        public async Task SeedAsync(ApplicationDbContext context, CancellationToken cancellationToken = default)
        {
            // Check if item group items already exist
            if (await context.ItemGroupItems.AnyAsync(cancellationToken))
            {
                return;
            }

            // Get item groups and products for relationships
            var itemGroups = await context.ItemGroups.ToListAsync(cancellationToken);
            var products = await context.Products.ToListAsync(cancellationToken);

            var appleGroup = itemGroups.FirstOrDefault(g => g.Name == "Apple Devices");
            var samsungGroup = itemGroups.FirstOrDefault(g => g.Name == "Samsung Electronics");
            var ikeaGroup = itemGroups.FirstOrDefault(g => g.Name == "IKEA Furniture Sets");
            var officeChairsGroup = itemGroups.FirstOrDefault(g => g.Name == "Office Chairs Collection");

            var iphoneProduct = products.FirstOrDefault(p => p.Name.Contains("iPhone"));
            var macbookProduct = products.FirstOrDefault(p => p.Name.Contains("MacBook"));
            var samsungPhone = products.FirstOrDefault(p => p.Name.Contains("Samsung Galaxy"));
            var officeChair = products.FirstOrDefault(p => p.Name.Contains("Office Chair"));

            var itemGroupItems = new List<ItemGroupItem>();

            if (appleGroup != null && iphoneProduct != null)
            {
                itemGroupItems.Add(new ItemGroupItem
                {
                    GroupID = appleGroup.GroupID,
                    ProductID = iphoneProduct.ProductID,
                    SKU = "APP-IPH-001",
                    PurchasePrice = 600.00m,
                    SellingPrice = 999.99m,
                    Barcode = "1234567890123"
                });
            }

            if (appleGroup != null && macbookProduct != null)
            {
                itemGroupItems.Add(new ItemGroupItem
                {
                    GroupID = appleGroup.GroupID,
                    ProductID = macbookProduct.ProductID,
                    SKU = "APP-MAC-001",
                    PurchasePrice = 1200.00m,
                    SellingPrice = 1999.99m,
                    Barcode = "1234567890124"
                });
            }

            if (samsungGroup != null && samsungPhone != null)
            {
                itemGroupItems.Add(new ItemGroupItem
                {
                    GroupID = samsungGroup.GroupID,
                    ProductID = samsungPhone.ProductID,
                    SKU = "SAM-GAL-001",
                    PurchasePrice = 450.00m,
                    SellingPrice = 799.99m,
                    Barcode = "1234567890125"
                });
            }

            if (officeChairsGroup != null && officeChair != null)
            {
                itemGroupItems.Add(new ItemGroupItem
                {
                    GroupID = officeChairsGroup.GroupID,
                    ProductID = officeChair.ProductID,
                    SKU = "OFF-CHA-001",
                    PurchasePrice = 150.00m,
                    SellingPrice = 299.99m,
                    Barcode = "1234567890126"
                });
            }

            if (itemGroupItems.Any())
            {
                await context.ItemGroupItems.AddRangeAsync(itemGroupItems, cancellationToken);
                await context.SaveChangesAsync(cancellationToken);
            }
        }
    }


    public class ProductTaxProfileDataSeeder : IDataSeeder
    {
        public async Task SeedAsync(ApplicationDbContext context, CancellationToken cancellationToken = default)
        {
            // Check if product tax profiles already exist
            if (await context.ProductTaxProfiles.AnyAsync(cancellationToken))
            {
                return;
            }

            // Get products and tax profiles for relationships
            var products = await context.Products.Take(5).ToListAsync(cancellationToken);
            var taxProfiles = await context.TaxProfiles.Take(2).ToListAsync(cancellationToken);

            if (!products.Any() || !taxProfiles.Any())
            {
                return; // Need products and tax profiles first
            }

            var productTaxProfiles = new List<ProductTaxProfile>();

            // Assign first tax profile as primary for first product
            if (products.Count > 0 && taxProfiles.Count > 0)
            {
                productTaxProfiles.Add(new ProductTaxProfile
                {
                    ProductID = products[0].ProductID,
                    TaxProfileID = taxProfiles[0].TaxProfileID,
                    IsPrimary = true
                });
            }

            // Assign second tax profile to first product (non-primary)
            if (products.Count > 0 && taxProfiles.Count > 1)
            {
                productTaxProfiles.Add(new ProductTaxProfile
                {
                    ProductID = products[0].ProductID,
                    TaxProfileID = taxProfiles[1].TaxProfileID,
                    IsPrimary = false
                });
            }

            // Assign tax profiles to other products
            for (int i = 1; i < Math.Min(products.Count, 4); i++)
            {
                var taxProfile = taxProfiles[i % taxProfiles.Count];
                productTaxProfiles.Add(new ProductTaxProfile
                {
                    ProductID = products[i].ProductID,
                    TaxProfileID = taxProfile.TaxProfileID,
                    IsPrimary = true
                });
            }

            if (productTaxProfiles.Any())
            {
                await context.ProductTaxProfiles.AddRangeAsync(productTaxProfiles, cancellationToken);
                await context.SaveChangesAsync(cancellationToken);
            }
        }
    }


    public class DefaultTaxDataSeeder : IDataSeeder
    {
        public async Task SeedAsync(ApplicationDbContext context, CancellationToken cancellationToken = default)
        {
            // Check if default taxes already exist
            if (await context.DefaultTaxes.AnyAsync(cancellationToken))
            {
                return;
            }

            var defaultTaxes = new List<DefaultTax>
        {
            new DefaultTax
            {
                Name = "VAT Standard",
                TaxValue = 20.0m,
                Type = "Percentage",
                Mode = "Exclusive"
            },
            new DefaultTax
            {
                Name = "VAT Reduced",
                TaxValue = 5.0m,
                Type = "Percentage",
                Mode = "Exclusive"
            },
            new DefaultTax
            {
                Name = "Sales Tax",
                TaxValue = 8.5m,
                Type = "Percentage",
                Mode = "Exclusive"
            },
            new DefaultTax
            {
                Name = "Import Duty",
                TaxValue = 10.0m,
                Type = "Percentage",
                Mode = "Included"
            },
            new DefaultTax
            {
                Name = "Environmental Fee",
                TaxValue = 5.0m,
                Type = "Fixed",
                Mode = "Exclusive"
            }
        };

            await context.DefaultTaxes.AddRangeAsync(defaultTaxes, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);
        }
    }
    public class ProductDataSeeder : IDataSeeder
    {
        public async Task SeedAsync(ApplicationDbContext context, CancellationToken cancellationToken = default)
        {
            // Check if products already exist to avoid duplicates
            if (await context.Products.AnyAsync(cancellationToken))
            {
                return; // Database has been seeded
            }

            // Get reference data
            var categories = await context.Categories.ToListAsync(cancellationToken);
            var brands = await context.Brands.ToListAsync(cancellationToken);
            var suppliers = await context.Suppliers.ToListAsync(cancellationToken);

            // Ensure we have the necessary reference data
            if (!categories.Any() || !brands.Any() || !suppliers.Any())
            {
                Console.WriteLine("Skipping product seeding - missing reference data (categories, brands, or suppliers)");
                return;
            }

            var productsToAdd = new List<Product>();

            // Electronics products
            var electronicsCategory = categories.FirstOrDefault(c => c.Name == "Electronics");
            var computersCategory = categories.FirstOrDefault(c => c.Name == "Computers");
            var smartphonesCategory = categories.FirstOrDefault(c => c.Name == "Smartphones");

            var appleBrand = brands.FirstOrDefault(b => b.Name == "Apple");
            var samsungBrand = brands.FirstOrDefault(b => b.Name == "Samsung");
            var dellBrand = brands.FirstOrDefault(b => b.Name == "Dell");
            var hpBrand = brands.FirstOrDefault(b => b.Name == "HP");
            var sonyBrand = brands.FirstOrDefault(b => b.Name == "Sony");

            var techSupplier = suppliers.FirstOrDefault(s => s.Name == "Tech Distributors Inc.");
            var globalElectronics = suppliers.FirstOrDefault(s => s.Name == "Global Electronics Supply");
            var mobileWholesalers = suppliers.FirstOrDefault(s => s.Name == "Mobile Device Wholesalers");

            // Smartphones
            if (smartphonesCategory != null && appleBrand != null && mobileWholesalers != null)
            {
                productsToAdd.Add(new Product
                {
                    Name = "iPhone 15 Pro",
                    SKU = "IPH15PRO-256",
                    Description = "Latest iPhone with advanced camera system",
                    CategoryID = smartphonesCategory.CategoryID,
                    BrandID = appleBrand.BrandID,
                    SupplierID = mobileWholesalers.SupplierID,
                    Barcode = "194253954305",
                    PurchasePrice = 899.99m,
                    SellingPrice = 1199.99m,
                    MinPrice = 1099.99m,
                    ProfitMargin = 25.0m,
                    TrackStock = true,
                    InitialStock = 50,
                    LowStockThreshold = 5,
                    Status = true
                });

                productsToAdd.Add(new Product
                {
                    Name = "iPhone 14",
                    SKU = "IPH14-128",
                    Description = "Previous generation iPhone",
                    CategoryID = smartphonesCategory.CategoryID,
                    BrandID = appleBrand.BrandID,
                    SupplierID = mobileWholesalers.SupplierID,
                    Barcode = "194253954306",
                    PurchasePrice = 699.99m,
                    SellingPrice = 899.99m,
                    MinPrice = 799.99m,
                    ProfitMargin = 22.0m,
                    TrackStock = true,
                    InitialStock = 30,
                    LowStockThreshold = 3,
                    Status = true
                });
            }

            if (smartphonesCategory != null && samsungBrand != null && globalElectronics != null)
            {
                productsToAdd.Add(new Product
                {
                    Name = "Samsung Galaxy S23 Ultra",
                    SKU = "SGS23U-512",
                    Description = "Flagship Android smartphone with S Pen",
                    CategoryID = smartphonesCategory.CategoryID,
                    BrandID = samsungBrand.BrandID,
                    SupplierID = globalElectronics.SupplierID,
                    Barcode = "8806092712345",
                    PurchasePrice = 949.99m,
                    SellingPrice = 1299.99m,
                    MinPrice = 1199.99m,
                    ProfitMargin = 27.0m,
                    TrackStock = true,
                    InitialStock = 40,
                    LowStockThreshold = 4,
                    Status = true
                });
            }

            // Computers and Laptops
            if (computersCategory != null && dellBrand != null && techSupplier != null)
            {
                productsToAdd.Add(new Product
                {
                    Name = "Dell XPS 15 Laptop",
                    SKU = "DLLXPS15-I7",
                    Description = "Premium 15-inch business laptop",
                    CategoryID = computersCategory.CategoryID,
                    BrandID = dellBrand.BrandID,
                    SupplierID = techSupplier.SupplierID,
                    Barcode = "884116265734",
                    PurchasePrice = 1299.99m,
                    SellingPrice = 1799.99m,
                    MinPrice = 1599.99m,
                    ProfitMargin = 28.0m,
                    TrackStock = true,
                    InitialStock = 25,
                    LowStockThreshold = 2,
                    Status = true
                });
            }

            if (computersCategory != null && hpBrand != null && techSupplier != null)
            {
                productsToAdd.Add(new Product
                {
                    Name = "HP EliteBook 840",
                    SKU = "HPELB840-I5",
                    Description = "Business-class 14-inch laptop",
                    CategoryID = computersCategory.CategoryID,
                    BrandID = hpBrand.BrandID,
                    SupplierID = techSupplier.SupplierID,
                    Barcode = "190780319876",
                    PurchasePrice = 899.99m,
                    SellingPrice = 1299.99m,
                    MinPrice = 1149.99m,
                    ProfitMargin = 30.0m,
                    TrackStock = true,
                    InitialStock = 20,
                    LowStockThreshold = 3,
                    Status = true
                });
            }

            // Electronics Accessories
            if (electronicsCategory != null && sonyBrand != null && globalElectronics != null)
            {
                productsToAdd.Add(new Product
                {
                    Name = "Sony WH-1000XM5 Headphones",
                    SKU = "SONYWHXM5-BLK",
                    Description = "Noise-cancelling wireless headphones",
                    CategoryID = electronicsCategory.CategoryID,
                    BrandID = sonyBrand.BrandID,
                    SupplierID = globalElectronics.SupplierID,
                    Barcode = "027242924167",
                    PurchasePrice = 299.99m,
                    SellingPrice = 399.99m,
                    MinPrice = 349.99m,
                    ProfitMargin = 25.0m,
                    TrackStock = true,
                    InitialStock = 35,
                    LowStockThreshold = 5,
                    Status = true
                });
            }

            // Furniture products
            var furnitureCategory = categories.FirstOrDefault(c => c.Name == "Furniture");
            var officeChairsCategory = categories.FirstOrDefault(c => c.Name == "Office Chairs");

            var ikeaBrand = brands.FirstOrDefault(b => b.Name == "IKEA");
            var hermanMillerBrand = brands.FirstOrDefault(b => b.Name == "Herman Miller");
            var steelcaseBrand = brands.FirstOrDefault(b => b.Name == "Steelcase");

            var officeFurnitureSupplier = suppliers.FirstOrDefault(s => s.Name == "Office Furniture Solutions");
            var homeOfficeSupplier = suppliers.FirstOrDefault(s => s.Name == "Home & Office Decor Ltd.");

            // Office Chairs
            if (officeChairsCategory != null && hermanMillerBrand != null && officeFurnitureSupplier != null)
            {
                productsToAdd.Add(new Product
                {
                    Name = "Herman Miller Aeron Chair",
                    SKU = "HMAERON-M",
                    Description = "Ergonomic office chair with lumbar support",
                    CategoryID = officeChairsCategory.CategoryID,
                    BrandID = hermanMillerBrand.BrandID,
                    SupplierID = officeFurnitureSupplier.SupplierID,
                    Barcode = "718756000123",
                    PurchasePrice = 899.99m,
                    SellingPrice = 1299.99m,
                    MinPrice = 1149.99m,
                    ProfitMargin = 31.0m,
                    TrackStock = true,
                    InitialStock = 15,
                    LowStockThreshold = 2,
                    Status = true
                });
            }

            if (officeChairsCategory != null && steelcaseBrand != null && officeFurnitureSupplier != null)
            {
                productsToAdd.Add(new Product
                {
                    Name = "Steelcase Gesture Chair",
                    SKU = "SCGESTURE-BLK",
                    Description = "Modern ergonomic office chair",
                    CategoryID = officeChairsCategory.CategoryID,
                    BrandID = steelcaseBrand.BrandID,
                    SupplierID = officeFurnitureSupplier.SupplierID,
                    Barcode = "840119112345",
                    PurchasePrice = 799.99m,
                    SellingPrice = 1199.99m,
                    MinPrice = 1049.99m,
                    ProfitMargin = 33.0m,
                    TrackStock = true,
                    InitialStock = 12,
                    LowStockThreshold = 2,
                    Status = true
                });
            }

            if (officeChairsCategory != null && ikeaBrand != null && homeOfficeSupplier != null)
            {
                productsToAdd.Add(new Product
                {
                    Name = "IKEA Markus Office Chair",
                    SKU = "IKMARKUS-GRY",
                    Description = "Affordable office chair with good support",
                    CategoryID = officeChairsCategory.CategoryID,
                    BrandID = ikeaBrand.BrandID,
                    SupplierID = homeOfficeSupplier.SupplierID,
                    Barcode = "903884123456",
                    PurchasePrice = 149.99m,
                    SellingPrice = 199.99m,
                    MinPrice = 179.99m,
                    ProfitMargin = 25.0m,
                    TrackStock = true,
                    InitialStock = 40,
                    LowStockThreshold = 8,
                    Status = true
                });
            }

            // Add products to context and save
            if (productsToAdd.Any())
            {
                await context.Products.AddRangeAsync(productsToAdd, cancellationToken);
                await context.SaveChangesAsync(cancellationToken);

                Console.WriteLine($"Successfully seeded {productsToAdd.Count} products");
            }
        }
    }

    //public class DefaultTaxDataSeeder : IDataSeeder
    //{
    //    public async Task SeedAsync(ApplicationDbContext context, CancellationToken cancellationToken = default)
    //    {
    //        // Check if default taxes already exist
    //        if (await context.DefaultTaxes.AnyAsync(cancellationToken))
    //        {
    //            return;
    //        }

    //        var defaultTaxes = new List<DefaultTax>
    //    {
    //        new DefaultTax
    //        {
    //            Name = "VAT Standard",
    //            TaxValue = 20.0m,
    //            Type = "Percentage",
    //            Mode = "Exclusive"
    //        },
    //        new DefaultTax
    //        {
    //            Name = "VAT Reduced",
    //            TaxValue = 5.0m,
    //            Type = "Percentage",
    //            Mode = "Exclusive"
    //        },
    //        new DefaultTax
    //        {
    //            Name = "Sales Tax",
    //            TaxValue = 8.5m,
    //            Type = "Percentage",
    //            Mode = "Exclusive"
    //        },
    //        new DefaultTax
    //        {
    //            Name = "Import Duty",
    //            TaxValue = 10.0m,
    //            Type = "Percentage",
    //            Mode = "Included"
    //        },
    //        new DefaultTax
    //        {
    //            Name = "Environmental Fee",
    //            TaxValue = 5.0m,
    //            Type = "Fixed",
    //            Mode = "Exclusive"
    //        }
    //    };

    //        await context.DefaultTaxes.AddRangeAsync(defaultTaxes, cancellationToken);
    //        await context.SaveChangesAsync(cancellationToken);
    //    }
    //}

    public class TaxProfileDataSeeder : IDataSeeder
    {
        public async Task SeedAsync(ApplicationDbContext context, CancellationToken cancellationToken = default)
        {
            // Check if tax profiles already exist
            if (await context.TaxProfiles.AnyAsync(cancellationToken))
            {
                return;
            }

            // Get default taxes
            var defaultTaxes = await context.DefaultTaxes.ToListAsync(cancellationToken);
            if (!defaultTaxes.Any())
            {
                return; // Need default taxes first
            }

            var vatStandard = defaultTaxes.FirstOrDefault(t => t.Name == "VAT Standard");
            var vatReduced = defaultTaxes.FirstOrDefault(t => t.Name == "VAT Reduced");
            var environmentalFee = defaultTaxes.FirstOrDefault(t => t.Name == "Environmental Fee");

            var taxProfiles = new List<TaxProfile>
        {
            new TaxProfile { Name = "Standard VAT Profile" },
            new TaxProfile { Name = "Reduced VAT Profile" },
            new TaxProfile { Name = "Electronic Goods Profile" }
        };

            await context.TaxProfiles.AddRangeAsync(taxProfiles, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);

            // Add taxes to profiles
            if (vatStandard != null)
            {
                context.TaxProfileTaxes.Add(new TaxProfileTax
                {
                    TaxProfileID = taxProfiles[0].TaxProfileID,
                    TaxID = vatStandard.TaxID
                });
            }

            if (vatReduced != null)
            {
                context.TaxProfileTaxes.Add(new TaxProfileTax
                {
                    TaxProfileID = taxProfiles[1].TaxProfileID,
                    TaxID = vatReduced.TaxID
                });
            }

            if (vatStandard != null && environmentalFee != null)
            {
                context.TaxProfileTaxes.Add(new TaxProfileTax
                {
                    TaxProfileID = taxProfiles[2].TaxProfileID,
                    TaxID = vatStandard.TaxID
                });

                context.TaxProfileTaxes.Add(new TaxProfileTax
                {
                    TaxProfileID = taxProfiles[2].TaxProfileID,
                    TaxID = environmentalFee.TaxID
                });
            }

            await context.SaveChangesAsync(cancellationToken);
        }
    }
    public class CompositeDataSeeder : IDataSeeder
    {
        private readonly IEnumerable<IDataSeeder> _seeders;

        public CompositeDataSeeder(IEnumerable<IDataSeeder> seeders)
        {
            _seeders = seeders;
        }

        public async Task SeedAsync(ApplicationDbContext context, CancellationToken cancellationToken = default)
        {
            foreach (var seeder in _seeders)
            {
                await seeder.SeedAsync(context, cancellationToken);
            }
        }
    }
}
