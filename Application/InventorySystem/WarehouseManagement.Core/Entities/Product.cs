using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WarehouseManagement.Core.Entities
{
    public class Product
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
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

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; }

        // Navigation properties
        public Category Category { get; set; } = null!;
        public Brand? Brand { get; set; }
        public Supplier? Supplier { get; set; }

        public int? GroupID { get; set; }

        // Navigation property
        public ItemGroup? ItemGroup { get; set; }



        public ICollection<ProductTaxProfile> ProductTaxProfiles { get; set; } = new List<ProductTaxProfile>();
        public ICollection<StockTransaction> StockTransactions { get; set; } = new List<StockTransaction>();
    }
}
