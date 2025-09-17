using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WarehouseManagement.Core.Entities
{
    public class Supplier
    {
        public int SupplierID { get; set; }

        [Required]
        [MaxLength(200)]
        public string BusinessName { get; set; } = null!;

        [MaxLength(100)]
        public string? FirstName { get; set; }

        [MaxLength(100)]
        public string? LastName { get; set; }

        [MaxLength(20)]
        public string? Telephone { get; set; }

        [MaxLength(20)]
        public string? Mobile { get; set; }

        [MaxLength(200)]
        public string? StreetAddress1 { get; set; }

        [MaxLength(200)]
        public string? StreetAddress2 { get; set; }

        [MaxLength(100)]
        public string? City { get; set; }

        [MaxLength(100)]
        public string? State { get; set; }

        [MaxLength(20)]
        public string? PostalCode { get; set; }

        [MaxLength(100)]
        public string? Country { get; set; }

        [MaxLength(100)]
        public string? CommercialRegistration { get; set; }

        [MaxLength(100)]
        public string? TaxID { get; set; }

        [Required]
        [MaxLength(20)]
        public string SupplierNumber { get; set; } = null!;

        [Required]
        [MaxLength(10)]
        public string Currency { get; set; } = "USD"; // Default currency

        [Column(TypeName = "decimal(18,2)")]
        public decimal OpeningBalance { get; set; }

        public DateTime? OpeningBalanceDate { get; set; }

        [EmailAddress]
        [MaxLength(100)]
        public string? Email { get; set; }

        public string? Notes { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        // Navigation properties
        public ICollection<SupplierContact> Contacts { get; set; } = new List<SupplierContact>();
        public ICollection<SupplierAttachment> Attachments { get; set; } = new List<SupplierAttachment>();
        public ICollection<PurchaseOrder> PurchaseOrders { get; set; } = new List<PurchaseOrder>();

        // Audit fields
        [MaxLength(450)]
        public string CreatedBy { get; set; } = null!;

        [MaxLength(450)]
        public string? UpdatedBy { get; set; }
    }

    public class SupplierContact
    {
        [Key] // 👈 Add this

        public int ContactID { get; set; }

        [Required]
        public int SupplierID { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = null!;

        [MaxLength(20)]
        public string? Phone { get; set; }

        [EmailAddress]
        [MaxLength(100)]
        public string? Email { get; set; }

        [MaxLength(200)]
        public string? Position { get; set; }

        public bool IsPrimary { get; set; }

        // Navigation property
        public Supplier Supplier { get; set; } = null!;
    }

    public class SupplierAttachment
    {
        [Key] // 👈 Add this

        public int AttachmentID { get; set; }

        [Required]
        public int SupplierID { get; set; }

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
        public Supplier Supplier { get; set; } = null!;
    }
}