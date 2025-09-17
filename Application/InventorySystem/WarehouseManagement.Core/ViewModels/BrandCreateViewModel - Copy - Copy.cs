using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WarehouseManagement.Core.Entities;

namespace WarehouseManagement.Core.ViewModels
{
    public class StockTransactionCreateViewModel
    {
        [Required]
        public int ProductID { get; set; }

        [Required]
        public int WarehouseID { get; set; }

        [Required]
        public DateTime TransactionDate { get; set; } = DateTime.UtcNow;

        [Required]
        public StockTransactionType TransactionType { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Quantity must be greater than 0")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Quantity { get; set; }

        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Unit price cannot be negative")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal UnitPrice { get; set; }

        [MaxLength(100)]
        public string? Reference { get; set; }
        public string ProductName { get; set; }
    }

    public class StockTransactionEditViewModel
    {
        [Required]
        public int TransactionID { get; set; }

        [Required]
        public int ProductID { get; set; }

        [Required]
        public int WarehouseID { get; set; }

        [Required]
        public DateTime TransactionDate { get; set; }

        [Required]
        public StockTransactionType TransactionType { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Quantity must be greater than 0")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Quantity { get; set; }

        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Unit price cannot be negative")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal UnitPrice { get; set; }

        [MaxLength(100)]
        public string? Reference { get; set; }
    }

    

    public class StockTransactionListViewModel
    {
        public int TransactionID { get; set; }
        public DateTime TransactionDate { get; set; }
        public string ProductName { get; set; } = null!;
        public string WarehouseName { get; set; } = null!;
        public string TransactionType { get; set; } = null!;
        public decimal Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal StockLevelAfter { get; set; }
        public string? Reference { get; set; }
        public decimal LineTotal { get; set; }
    }

    public class StockLevelViewModel
    {
        public int ProductID { get; set; }
        public string ProductName { get; set; } = null!;
        public string ProductSKU { get; set; } = null!;
        public int WarehouseID { get; set; }
        public string WarehouseName { get; set; } = null!;
        public decimal CurrentStock { get; set; }
        public DateTime LastTransactionDate { get; set; }
        public decimal ValueOnHand { get; set; }
    }
}