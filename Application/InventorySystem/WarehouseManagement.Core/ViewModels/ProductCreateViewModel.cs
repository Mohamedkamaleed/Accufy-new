using System.ComponentModel.DataAnnotations;

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

}
