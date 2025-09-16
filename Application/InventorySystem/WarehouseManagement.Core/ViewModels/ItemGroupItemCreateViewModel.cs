using System.ComponentModel.DataAnnotations;

namespace WarehouseManagement.Core.ViewModels
{
    public class ItemGroupItemCreateViewModel
    {
        [Required]
        public int GroupID { get; set; }

        [Required]
        public int ProductID { get; set; }

        [MaxLength(100)]
        public string? SKU { get; set; }

        [Range(0, double.MaxValue)]
        public decimal? PurchasePrice { get; set; }

        [Range(0, double.MaxValue)]
        public decimal? SellingPrice { get; set; }

        [MaxLength(100)]
        public string? Barcode { get; set; }
    }

    public class ItemGroupItemEditViewModel
    {
        [Required]
        public int GroupItemID { get; set; }

        [MaxLength(100)]
        public string? SKU { get; set; }

        [Range(0, double.MaxValue)]
        public decimal? PurchasePrice { get; set; }

        [Range(0, double.MaxValue)]
        public decimal? SellingPrice { get; set; }

        [MaxLength(100)]
        public string? Barcode { get; set; }
    }

    public class ItemGroupItemViewModel
    {
        public int GroupItemID { get; set; }
        public int GroupID { get; set; }
        public string GroupName { get; set; } = null!;
        public int ProductID { get; set; }
        public string ProductName { get; set; } = null!;
        public string? SKU { get; set; }
        public decimal? PurchasePrice { get; set; }
        public decimal? SellingPrice { get; set; }
        public string? Barcode { get; set; }
    }
}