using System.ComponentModel.DataAnnotations;

namespace WarehouseManagement.Core.ViewModels
{
    public class ProductTaxProfileCreateViewModel
    {
        [Required]
        public int ProductID { get; set; }

        [Required]
        public int TaxProfileID { get; set; }

        public bool IsPrimary { get; set; } = false;
    }

    public class ProductTaxProfileEditViewModel
    {
        [Required]
        public int ProductID { get; set; }

        [Required]
        public int TaxProfileID { get; set; }

        public bool IsPrimary { get; set; }
    }

    public class ProductTaxProfileViewModel
    {
        public int ProductID { get; set; }
        public string ProductName { get; set; } = null!;
        public int TaxProfileID { get; set; }
        public string TaxProfileName { get; set; } = null!;
        public decimal TaxRate { get; set; }
        public bool IsPrimary { get; set; }
    }
}
