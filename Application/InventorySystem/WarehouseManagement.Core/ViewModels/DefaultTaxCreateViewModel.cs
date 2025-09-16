using System.ComponentModel.DataAnnotations;

namespace WarehouseManagement.Core.ViewModels
{
    public class DefaultTaxCreateViewModel
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = null!;

        [Range(0, double.MaxValue)]
        public decimal TaxValue { get; set; }

        [Required]
        [RegularExpression("^(Percentage|Fixed)$", ErrorMessage = "Type must be either 'Percentage' or 'Fixed'")]
        public string Type { get; set; } = "Percentage";

        [Required]
        [RegularExpression("^(Included|Exclusive)$", ErrorMessage = "Mode must be either 'Included' or 'Exclusive'")]
        public string Mode { get; set; } = "Exclusive";
    }

    public class DefaultTaxEditViewModel
    {
        [Required]
        public int TaxID { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = null!;

        [Range(0, double.MaxValue)]
        public decimal TaxValue { get; set; }

        [Required]
        [RegularExpression("^(Percentage|Fixed)$", ErrorMessage = "Type must be either 'Percentage' or 'Fixed'")]
        public string Type { get; set; } = "Percentage";

        [Required]
        [RegularExpression("^(Included|Exclusive)$", ErrorMessage = "Mode must be either 'Included' or 'Exclusive'")]
        public string Mode { get; set; } = "Exclusive";
    }

    public class DefaultTaxViewModel
    {
        public int TaxID { get; set; }
        public string Name { get; set; } = null!;
        public decimal TaxValue { get; set; }
        public string Type { get; set; } = null!;
        public string Mode { get; set; } = null!;
    }
}