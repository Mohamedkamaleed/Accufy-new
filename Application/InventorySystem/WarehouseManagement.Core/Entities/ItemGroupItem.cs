using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WarehouseManagement.Core.Entities
{
    public class ItemGroupItem
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int GroupItemID { get; set; }

        [Required]
        public int GroupID { get; set; }

        [Required]
        public int ProductID { get; set; }

        [MaxLength(100)]
        public string? SKU { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? PurchasePrice { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? SellingPrice { get; set; }

        [MaxLength(100)]
        public string? Barcode { get; set; }

        // Navigation properties
        public ItemGroup ItemGroup { get; set; } = null!;
        public Product Product { get; set; } = null!;
    }
}