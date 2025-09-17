using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WarehouseManagement.Core.Entities;

namespace WarehouseManagement.Core.ViewModels
{
    public class ProductCreateViewModel
    {
        [Required]
        [MaxLength(200)]
        public string Name { get; set; } = null!;

        [MaxLength(100)]
        public string? SKU { get; set; }

        public string? Description { get; set; }

        [Required]
        public int CategoryID { get; set; }

        public int? BrandID { get; set; }

        public int? SupplierID { get; set; }

        [MaxLength(100)]
        public string? Barcode { get; set; }

        [Range(0, double.MaxValue)]
        public decimal? PurchasePrice { get; set; }

        [Range(0, double.MaxValue)]
        public decimal? SellingPrice { get; set; }

        [Range(0, double.MaxValue)]
        public decimal? MinPrice { get; set; }

        [Range(0, 100)]
        public decimal? Discount { get; set; }

        [MaxLength(20)]
        public string? DiscountType { get; set; }

        [Range(0, 100)]
        public decimal? ProfitMargin { get; set; }

        public bool TrackStock { get; set; } = true;

        [Range(0, double.MaxValue)]
        public decimal? InitialStock { get; set; }

        [Range(0, double.MaxValue)]
        public decimal? LowStockThreshold { get; set; }

        public bool Status { get; set; } = true;
    }

    public class ProductEditViewModel
    {
        [Required]
        public int ProductID { get; set; }

        [Required]
        [MaxLength(200)]
        public string Name { get; set; } = null!;

        [MaxLength(100)]
        public string? SKU { get; set; }

        public string? Description { get; set; }

        [Required]
        public int CategoryID { get; set; }

        public int? BrandID { get; set; }

        public int? SupplierID { get; set; }

        [MaxLength(100)]
        public string? Barcode { get; set; }

        [Range(0, double.MaxValue)]
        public decimal? PurchasePrice { get; set; }

        [Range(0, double.MaxValue)]
        public decimal? SellingPrice { get; set; }

        [Range(0, double.MaxValue)]
        public decimal? MinPrice { get; set; }

        [Range(0, 100)]
        public decimal? Discount { get; set; }

        [MaxLength(20)]
        public string? DiscountType { get; set; }

        [Range(0, 100)]
        public decimal? ProfitMargin { get; set; }

        public bool TrackStock { get; set; } = true;

        [Range(0, double.MaxValue)]
        public decimal? InitialStock { get; set; }

        [Range(0, double.MaxValue)]
        public decimal? LowStockThreshold { get; set; }

        public bool Status { get; set; } = true;
    }

    public class ProductViewModel
    {
        public int ProductID { get; set; }
        public string Name { get; set; } = null!;
        public string? SKU { get; set; }
        public string? Description { get; set; }
        public int CategoryID { get; set; }
        public string CategoryName { get; set; } = null!;
        public int? BrandID { get; set; }
        public string? BrandName { get; set; }
        public int? SupplierID { get; set; }
        public string? SupplierName { get; set; }
        public string? Barcode { get; set; }
        public decimal? PurchasePrice { get; set; }
        public decimal? SellingPrice { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? Discount { get; set; }
        public string? DiscountType { get; set; }
        public decimal? ProfitMargin { get; set; }
        public bool TrackStock { get; set; }
        public decimal? InitialStock { get; set; }
        public decimal? LowStockThreshold { get; set; }
        public bool Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }


    public class ProductDetailViewModel
    {
        public int ProductID { get; set; }

        [Required]
        [MaxLength(200)]
        public string Name { get; set; } = null!;

        [MaxLength(100)]
        public string? SKU { get; set; }

        public string? Description { get; set; }

        [Required]
        public int CategoryID { get; set; }
        public string CategoryName { get; set; } = null!;

        public int? BrandID { get; set; }
        public string? BrandName { get; set; }

        public int? SupplierID { get; set; }
        public string? SupplierName { get; set; }

        [MaxLength(100)]
        public string? Barcode { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? PurchasePrice { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? SellingPrice { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? MinPrice { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal? Discount { get; set; }

        [MaxLength(20)]
        public string? DiscountType { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal? ProfitMargin { get; set; }

        public bool TrackStock { get; set; } = true;

        [Column(TypeName = "decimal(18,2)")]
        public decimal? InitialStock { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? LowStockThreshold { get; set; }

        public bool Status { get; set; } = true;

        public int? GroupID { get; set; }
        public string? GroupName { get; set; }

        // Summary metrics
        public decimal OnHandStock { get; set; }
        public decimal TotalSold { get; set; }
        public decimal SalesLast28Days { get; set; }
        public decimal SalesLast7Days { get; set; }
        public decimal AverageUnitCost { get; set; }
        public string StockStatus { get; set; } = "In Stock";

        // Navigation properties for tabs
        public List<StockTransactionViewModel> StockTransactions { get; set; } = new List<StockTransactionViewModel>();
        public List<TimelineEventViewModel> TimelineEvents { get; set; } = new List<TimelineEventViewModel>();
        public List<ActivityLogViewModel> ActivityLogs { get; set; } = new List<ActivityLogViewModel>();
        public ItemGroupViewModel? ItemGroup { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }

    public class StockTransactionViewModel
    {
        public int TransactionID { get; set; }
        public DateTime TransactionDate { get; set; }
        public decimal Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal StockLevelAfter { get; set; }
        public string WarehouseName { get; set; } = null!;
        public string Reference { get; set; } = null!;

        public int ProductID { get; set; }
        public string ProductName { get; set; } = null!;
        public string ProductSKU { get; set; } = null!;
        public int WarehouseID { get; set; }
        public StockTransactionType TransactionType { get; set; }
        public string TransactionTypeDisplay { get; set; } = null!;

        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; } = null!;
        public decimal LineTotal { get; set; }
    }


    public class TimelineEventViewModel
    {
        public int EventID { get; set; }
        public string ActionType { get; set; } = null!;
        public string ItemReference { get; set; } = null!;
        public string UserName { get; set; } = null!;
        public DateTime Timestamp { get; set; }
        public decimal StockBalance { get; set; }
        public decimal AveragePrice { get; set; }
        public string Description { get; set; } = null!;
    }

    public class ActivityLogViewModel
    {
        public int LogID { get; set; }
        public string ActionType { get; set; } = null!;
        public string UserName { get; set; } = null!;
        public DateTime Timestamp { get; set; }
        public string BeforeValue { get; set; } = null!;
        public string AfterValue { get; set; } = null!;
        public string Details { get; set; } = null!;
    }

    public class ProductItemGroupViewModel
    {
        public int GroupID { get; set; }
        public string GroupName { get; set; } = null!;
        public List<ItemGroupProductViewModel> Products { get; set; } = new List<ItemGroupProductViewModel>();
    }

    public class ItemGroupProductViewModel
    {
        public int ProductID { get; set; }
        public string ProductName { get; set; } = null!;
        public string SKU { get; set; } = null!;
        public decimal Quantity { get; set; }
        public decimal PurchasePrice { get; set; }
        public decimal SellingPrice { get; set; }
        public string Barcode { get; set; } = null!;
    }
}
