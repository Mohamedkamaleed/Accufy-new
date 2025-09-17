using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WarehouseManagement.Core.Entities
{
    public enum StockTransactionType
    {
        Purchase,
        Sale,
        Adjustment,
        Transfer,
        Return,
        WriteOff
    }

    public class StockTransaction
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TransactionID { get; set; }

        [Required]
        public int ProductID { get; set; }

        [Required]
        public int WarehouseID { get; set; }

        [Required]
        public DateTime TransactionDate { get; set; } = DateTime.UtcNow;

        [Required]
        [MaxLength(20)]
        public StockTransactionType TransactionType { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Quantity { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal UnitPrice { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal StockLevelAfter { get; set; }

        [MaxLength(100)]
        public string? Reference { get; set; }

        // Navigation properties
        public Product Product { get; set; } = null!;
        public Warehouse Warehouse { get; set; } = null!;

        // Audit fields
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [MaxLength(450)]
        public string CreatedBy { get; set; } = null!;
    }
}