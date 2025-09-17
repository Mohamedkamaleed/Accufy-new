using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WarehouseManagement.Core.ViewModels
{
    public class SupplierCreateViewModel
    {
        [Required]
        [MaxLength(200)]
        [Display(Name = "Business Name")]
        public string BusinessName { get; set; } = null!;

        [MaxLength(100)]
        [Display(Name = "First Name")]
        public string? FirstName { get; set; }

        [MaxLength(100)]
        [Display(Name = "Last Name")]
        public string? LastName { get; set; }

        [MaxLength(20)]
        [Phone]
        public string? Telephone { get; set; }

        [MaxLength(20)]
        [Phone]
        public string? Mobile { get; set; }

        [MaxLength(200)]
        [Display(Name = "Street Address 1")]
        public string? StreetAddress1 { get; set; }

        [MaxLength(200)]
        [Display(Name = "Street Address 2")]
        public string? StreetAddress2 { get; set; }

        [MaxLength(100)]
        public string? City { get; set; }

        [MaxLength(100)]
        public string? State { get; set; }

        [MaxLength(20)]
        [Display(Name = "Postal Code")]
        public string? PostalCode { get; set; }

        [MaxLength(100)]
        public string? Country { get; set; }

        [MaxLength(100)]
        [Display(Name = "Commercial Registration")]
        public string? CommercialRegistration { get; set; }

        [MaxLength(100)]
        [Display(Name = "Tax ID")]
        public string? TaxID { get; set; }

        [Required]
        [MaxLength(10)]
        public string Currency { get; set; } = "USD";

        [Display(Name = "Opening Balance")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal OpeningBalance { get; set; }

        [Display(Name = "Opening Balance Date")]
        public DateTime? OpeningBalanceDate { get; set; }

        [EmailAddress]
        [MaxLength(100)]
        public string? Email { get; set; }

        public string? Notes { get; set; }

        public List<SupplierContactViewModel> Contacts { get; set; } = new List<SupplierContactViewModel>();
    }

    public class SupplierEditViewModel
    {
        public int SupplierID { get; set; }

        [Required]
        [MaxLength(200)]
        [Display(Name = "Business Name")]
        public string BusinessName { get; set; } = null!;

        [MaxLength(100)]
        [Display(Name = "First Name")]
        public string? FirstName { get; set; }

        [MaxLength(100)]
        [Display(Name = "Last Name")]
        public string? LastName { get; set; }

        [MaxLength(20)]
        [Phone]
        public string? Telephone { get; set; }

        [MaxLength(20)]
        [Phone]
        public string? Mobile { get; set; }

        [MaxLength(200)]
        [Display(Name = "Street Address 1")]
        public string? StreetAddress1 { get; set; }

        [MaxLength(200)]
        [Display(Name = "Street Address 2")]
        public string? StreetAddress2 { get; set; }

        [MaxLength(100)]
        public string? City { get; set; }

        [MaxLength(100)]
        public string? State { get; set; }

        [MaxLength(20)]
        [Display(Name = "Postal Code")]
        public string? PostalCode { get; set; }

        [MaxLength(100)]
        public string? Country { get; set; }

        [MaxLength(100)]
        [Display(Name = "Commercial Registration")]
        public string? CommercialRegistration { get; set; }

        [MaxLength(100)]
        [Display(Name = "Tax ID")]
        public string? TaxID { get; set; }

        [Required]
        [MaxLength(20)]
        [Display(Name = "Supplier Number")]
        public string SupplierNumber { get; set; } = null!;

        [Required]
        [MaxLength(10)]
        public string Currency { get; set; } = "USD";

        [Display(Name = "Opening Balance")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal OpeningBalance { get; set; }

        [Display(Name = "Opening Balance Date")]
        public DateTime? OpeningBalanceDate { get; set; }

        [EmailAddress]
        [MaxLength(100)]
        public string? Email { get; set; }

        public string? Notes { get; set; }

        public bool IsActive { get; set; }

        public List<SupplierContactViewModel> Contacts { get; set; } = new List<SupplierContactViewModel>();
    }

    public class SupplierListViewModel
    {
        public int SupplierID { get; set; }

        [Display(Name = "Business Name")]
        public string BusinessName { get; set; } = null!;

        [Display(Name = "Supplier Number")]
        public string SupplierNumber { get; set; } = null!;

        [Display(Name = "Contact Name")]
        public string? ContactName { get; set; }

        public string? Email { get; set; }

        [Display(Name = "Phone")]
        public string? Telephone { get; set; }

        public string? City { get; set; }

        public string? Country { get; set; }

        [Display(Name = "Active")]
        public bool IsActive { get; set; }

        [Display(Name = "Created Date")]
        public DateTime CreatedAt { get; set; }
    }

    public class SupplierContactViewModel
    {
        public int ContactID { get; set; }

        public int SupplierID { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = null!;

        [MaxLength(20)]
        [Phone]
        public string? Phone { get; set; }

        [EmailAddress]
        [MaxLength(100)]
        public string? Email { get; set; }

        [MaxLength(200)]
        public string? Position { get; set; }

        [Display(Name = "Primary Contact")]
        public bool IsPrimary { get; set; }
    }

    public class SupplierAttachmentViewModel
    {
        public int AttachmentID { get; set; }

        public int SupplierID { get; set; }

        [Display(Name = "File Name")]
        public string FileName { get; set; } = null!;

        [Display(Name = "File Type")]
        public string FileType { get; set; } = null!;

        [Display(Name = "File Size")]
        public long FileSize { get; set; }

        public string? Description { get; set; }

        [Display(Name = "Uploaded At")]
        public DateTime UploadedAt { get; set; }

        [Display(Name = "Uploaded By")]
        public string UploadedBy { get; set; } = null!;
    }

    public class SupplierDetailViewModel
    {
        public int SupplierID { get; set; }

        [Display(Name = "Business Name")]
        public string BusinessName { get; set; } = null!;

        [Display(Name = "Supplier Number")]
        public string SupplierNumber { get; set; } = null!;

        [Display(Name = "First Name")]
        public string? FirstName { get; set; }

        [Display(Name = "Last Name")]
        public string? LastName { get; set; }

        public string? Telephone { get; set; }

        public string? Mobile { get; set; }

        [Display(Name = "Street Address 1")]
        public string? StreetAddress1 { get; set; }

        [Display(Name = "Street Address 2")]
        public string? StreetAddress2 { get; set; }

        public string? City { get; set; }

        public string? State { get; set; }

        [Display(Name = "Postal Code")]
        public string? PostalCode { get; set; }

        public string? Country { get; set; }

        [Display(Name = "Commercial Registration")]
        public string? CommercialRegistration { get; set; }

        [Display(Name = "Tax ID")]
        public string? TaxID { get; set; }

        public string Currency { get; set; } = "USD";

        [Display(Name = "Opening Balance")]
        public decimal OpeningBalance { get; set; }

        [Display(Name = "Opening Balance Date")]
        public DateTime? OpeningBalanceDate { get; set; }

        public string? Email { get; set; }

        public string? Notes { get; set; }

        [Display(Name = "Active")]
        public bool IsActive { get; set; }

        [Display(Name = "Created At")]
        public DateTime CreatedAt { get; set; }

        [Display(Name = "Updated At")]
        public DateTime? UpdatedAt { get; set; }

        [Display(Name = "Created By")]
        public string CreatedBy { get; set; } = null!;

        [Display(Name = "Updated By")]
        public string? UpdatedBy { get; set; }

        public List<SupplierContactViewModel> Contacts { get; set; } = new List<SupplierContactViewModel>();
        public List<SupplierAttachmentViewModel> Attachments { get; set; } = new List<SupplierAttachmentViewModel>();
    }
}