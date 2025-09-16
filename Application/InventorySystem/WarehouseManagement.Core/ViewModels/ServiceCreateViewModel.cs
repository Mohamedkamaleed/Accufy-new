using System.ComponentModel.DataAnnotations;

namespace WarehouseManagement.Core.ViewModels
{
    public class ServiceCreateViewModel
    {
        [Required]
        [MaxLength(200)]
        public string Name { get; set; } = null!;

        [MaxLength(50)]
        public string? Code { get; set; }

        public string? Description { get; set; }

        public int? CategoryID { get; set; }

        public int? SupplierID { get; set; }

        [Range(0, double.MaxValue)]
        public decimal? PurchasePrice { get; set; }

        [Range(0, double.MaxValue)]
        public decimal? UnitPrice { get; set; }

        [Range(0, double.MaxValue)]
        public decimal? MinPrice { get; set; }

        [Range(0, 100)]
        public decimal? Discount { get; set; }

        [MaxLength(20)]
        public string? DiscountType { get; set; }

        [Range(0, 100)]
        public decimal? ProfitMargin { get; set; }

        public bool Status { get; set; } = true;
    }

    public class ServiceEditViewModel
    {
        [Required]
        public int ServiceID { get; set; }

        [Required]
        [MaxLength(200)]
        public string Name { get; set; } = null!;

        [MaxLength(50)]
        public string? Code { get; set; }

        public string? Description { get; set; }

        public int? CategoryID { get; set; }

        public int? SupplierID { get; set; }

        [Range(0, double.MaxValue)]
        public decimal? PurchasePrice { get; set; }

        [Range(0, double.MaxValue)]
        public decimal? UnitPrice { get; set; }

        [Range(0, double.MaxValue)]
        public decimal? MinPrice { get; set; }

        [Range(0, 100)]
        public decimal? Discount { get; set; }

        [MaxLength(20)]
        public string? DiscountType { get; set; }

        [Range(0, 100)]
        public decimal? ProfitMargin { get; set; }

        public bool Status { get; set; } = true;
    }

    public class ServiceViewModel
    {
        public int ServiceID { get; set; }
        public string Name { get; set; } = null!;
        public string? Code { get; set; }
        public string? Description { get; set; }
        public int? CategoryID { get; set; }
        public string? CategoryName { get; set; }
        public int? SupplierID { get; set; }
        public string? SupplierName { get; set; }
        public decimal? PurchasePrice { get; set; }
        public decimal? UnitPrice { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? Discount { get; set; }
        public string? DiscountType { get; set; }
        public decimal? ProfitMargin { get; set; }
        public bool Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}