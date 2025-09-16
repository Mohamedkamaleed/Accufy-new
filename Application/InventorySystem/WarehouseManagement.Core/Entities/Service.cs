using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WarehouseManagement.Core.Entities
{
    public class Service
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ServiceID { get; set; }

        [Required]
        [MaxLength(200)]
        public string Name { get; set; } = null!;

        [MaxLength(50)]
        public string? Code { get; set; }

        public string? Description { get; set; }

        public int? CategoryID { get; set; }

        public int? SupplierID { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? PurchasePrice { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? UnitPrice { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? MinPrice { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal? Discount { get; set; }

        [MaxLength(20)]
        public string? DiscountType { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal? ProfitMargin { get; set; }

        public bool Status { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; }

        // Navigation properties
        public Category? Category { get; set; }
        public Supplier? Supplier { get; set; }
    }
}