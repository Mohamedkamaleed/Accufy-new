using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WarehouseManagement.Core.Entities
{
    public enum PurchaseOrderStatus
    {
        Draft,
        PendingApproval,
        Approved,
        Ordered,
        PartiallyReceived,
        Completed,
        Cancelled
    }

    public class PurchaseOrder
    {
        public int PurchaseOrderID { get; set; }

        [Required]
        [MaxLength(20)]
        public string OrderNumber { get; set; } = null!;

        [Required]
        public int SupplierID { get; set; }

        public DateTime OrderDate { get; set; } = DateTime.UtcNow;

        public DateTime? ExpectedDeliveryDate { get; set; }

        public DateTime? ActualDeliveryDate { get; set; }

        [Required]
        public PurchaseOrderStatus Status { get; set; } = PurchaseOrderStatus.Draft;

        [MaxLength(50)]
        public string? ReferenceNumber { get; set; }

        [MaxLength(500)]
        public string? ShippingAddress { get; set; }

        [MaxLength(500)]
        public string? BillingAddress { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal SubTotal { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal TaxAmount { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal DiscountAmount { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal ShippingCost { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalAmount { get; set; }

        [MaxLength(1000)]
        public string? Notes { get; set; }

        [MaxLength(1000)]
        public string? TermsAndConditions { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        // Audit fields
        [MaxLength(450)]
        public string CreatedBy { get; set; } = null!;

        [MaxLength(450)]
        public string? UpdatedBy { get; set; }

        // Navigation properties
        public Supplier Supplier { get; set; } = null!;
        public ICollection<PurchaseOrderLine> OrderLines { get; set; } = new List<PurchaseOrderLine>();
        public ICollection<PurchaseOrderAttachment> Attachments { get; set; } = new List<PurchaseOrderAttachment>();
        public ICollection<PurchaseOrderStatusHistory> StatusHistory { get; set; } = new List<PurchaseOrderStatusHistory>();
    }

    public class PurchaseOrderLine
    {
        public int PurchaseOrderLineID { get; set; }

        [Required]
        public int PurchaseOrderID { get; set; }

        [Required]
        public int ProductID { get; set; }

        [Required]
        [MaxLength(200)]
        public string ProductName { get; set; } = null!;

        [MaxLength(100)]
        public string? ProductSKU { get; set; }

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

        [Column(TypeName = "decimal(18,2)")]
        public decimal LineTotal { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal ReceivedQuantity { get; set; }

        public DateTime? ExpectedDate { get; set; }

        [MaxLength(500)]
        public string? Notes { get; set; }

        // Navigation properties
        public PurchaseOrder PurchaseOrder { get; set; } = null!;
        public Product Product { get; set; } = null!;
    }

    public class PurchaseOrderAttachment
    {
        public int AttachmentID { get; set; }

        [Required]
        public int PurchaseOrderID { get; set; }

        [Required]
        [MaxLength(200)]
        public string FileName { get; set; } = null!;

        [MaxLength(50)]
        public string FileType { get; set; } = null!;

        public long FileSize { get; set; }

        [MaxLength(500)]
        public string? Description { get; set; }

        public DateTime UploadedAt { get; set; } = DateTime.UtcNow;

        [MaxLength(450)]
        public string UploadedBy { get; set; } = null!;

        // Navigation property
        public PurchaseOrder PurchaseOrder { get; set; } = null!;
    }

    public class PurchaseOrderStatusHistory
    {
        public int StatusHistoryID { get; set; }

        [Required]
        public int PurchaseOrderID { get; set; }

        [Required]
        public PurchaseOrderStatus Status { get; set; }

        [MaxLength(500)]
        public string? Notes { get; set; }

        public DateTime StatusDate { get; set; } = DateTime.UtcNow;

        [MaxLength(450)]
        public string ChangedBy { get; set; } = null!;

        // Navigation property
        public PurchaseOrder PurchaseOrder { get; set; } = null!;
    }
}