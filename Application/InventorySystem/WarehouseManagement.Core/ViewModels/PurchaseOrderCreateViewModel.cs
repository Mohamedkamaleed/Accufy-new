using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WarehouseManagement.Core.Entities;

namespace WarehouseManagement.Core.ViewModels
{
    public class PurchaseOrderCreateViewModel
    {
        [Required]
        public int SupplierID { get; set; }

        public DateTime OrderDate { get; set; } = DateTime.UtcNow;

        public DateTime? ExpectedDeliveryDate { get; set; }

        [MaxLength(50)]
        public string? ReferenceNumber { get; set; }

        [MaxLength(500)]
        public string? ShippingAddress { get; set; }

        [MaxLength(500)]
        public string? BillingAddress { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal ShippingCost { get; set; }

        [MaxLength(1000)]
        public string? Notes { get; set; }

        [MaxLength(1000)]
        public string? TermsAndConditions { get; set; }

        public List<PurchaseOrderLineCreateViewModel> OrderLines { get; set; } = new List<PurchaseOrderLineCreateViewModel>();
    }

    public class PurchaseOrderEditViewModel
    {
        public int PurchaseOrderID { get; set; }

        [Required]
        public int SupplierID { get; set; }

        public DateTime OrderDate { get; set; }

        public DateTime? ExpectedDeliveryDate { get; set; }

        [MaxLength(50)]
        public string? ReferenceNumber { get; set; }

        [MaxLength(500)]
        public string? ShippingAddress { get; set; }

        [MaxLength(500)]
        public string? BillingAddress { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal ShippingCost { get; set; }

        [MaxLength(1000)]
        public string? Notes { get; set; }

        [MaxLength(1000)]
        public string? TermsAndConditions { get; set; }

        public List<PurchaseOrderLineEditViewModel> OrderLines { get; set; } = new List<PurchaseOrderLineEditViewModel>();
    }

    public class PurchaseOrderLineCreateViewModel
    {
        [Required]
        public int ProductID { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Quantity { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal UnitPrice { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal DiscountPercentage { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal TaxPercentage { get; set; }

        public DateTime? ExpectedDate { get; set; }

        [MaxLength(500)]
        public string? Notes { get; set; }
    }

    public class PurchaseOrderLineEditViewModel
    {
        public int PurchaseOrderLineID { get; set; }

        [Required]
        public int ProductID { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Quantity { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal UnitPrice { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal DiscountPercentage { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal TaxPercentage { get; set; }

        public DateTime? ExpectedDate { get; set; }

        [MaxLength(500)]
        public string? Notes { get; set; }
    }

    public class PurchaseOrderViewModel
    {
        public int PurchaseOrderID { get; set; }

        public string OrderNumber { get; set; } = null!;

        public int SupplierID { get; set; }
        public string SupplierName { get; set; } = null!;

        public DateTime OrderDate { get; set; }
        public DateTime? ExpectedDeliveryDate { get; set; }
        public DateTime? ActualDeliveryDate { get; set; }

        public PurchaseOrderStatus Status { get; set; }
        public string StatusDisplay { get; set; } = null!;

        public string? ReferenceNumber { get; set; }

        public decimal SubTotal { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal ShippingCost { get; set; }
        public decimal TotalAmount { get; set; }

        public string? Notes { get; set; }
        public string? TermsAndConditions { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string CreatedBy { get; set; } = null!;
        public string? UpdatedBy { get; set; }

        public List<PurchaseOrderLineViewModel> OrderLines { get; set; } = new List<PurchaseOrderLineViewModel>();
        public List<PurchaseOrderAttachmentViewModel> Attachments { get; set; } = new List<PurchaseOrderAttachmentViewModel>();
        public List<PurchaseOrderStatusHistoryViewModel> StatusHistory { get; set; } = new List<PurchaseOrderStatusHistoryViewModel>();
    }

    public class PurchaseOrderLineViewModel
    {
        public int PurchaseOrderLineID { get; set; }
        public int PurchaseOrderID { get; set; }
        public int ProductID { get; set; }
        public string ProductName { get; set; } = null!;
        public string? ProductSKU { get; set; }
        public decimal Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal DiscountPercentage { get; set; }
        public decimal TaxPercentage { get; set; }
        public decimal LineTotal { get; set; }
        public decimal ReceivedQuantity { get; set; }
        public DateTime? ExpectedDate { get; set; }
        public string? Notes { get; set; }
    }

    public class PurchaseOrderAttachmentViewModel
    {
        public int AttachmentID { get; set; }
        public int PurchaseOrderID { get; set; }
        public string FileName { get; set; } = null!;
        public string FileType { get; set; } = null!;
        public long FileSize { get; set; }
        public string? Description { get; set; }
        public DateTime UploadedAt { get; set; }
        public string UploadedBy { get; set; } = null!;
    }

    public class PurchaseOrderStatusHistoryViewModel
    {
        public int StatusHistoryID { get; set; }
        public int PurchaseOrderID { get; set; }
        public PurchaseOrderStatus Status { get; set; }
        public string StatusDisplay { get; set; } = null!;
        public string? Notes { get; set; }
        public DateTime StatusDate { get; set; }
        public string ChangedBy { get; set; } = null!;
    }

    public class PurchaseOrderListViewModel
    {
        public int PurchaseOrderID { get; set; }
        public string OrderNumber { get; set; } = null!;
        public string SupplierName { get; set; } = null!;
        public DateTime OrderDate { get; set; }
        public DateTime? ExpectedDeliveryDate { get; set; }
        public PurchaseOrderStatus Status { get; set; }
        public string StatusDisplay { get; set; } = null!;
        public decimal TotalAmount { get; set; }
        public int LineItemsCount { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}