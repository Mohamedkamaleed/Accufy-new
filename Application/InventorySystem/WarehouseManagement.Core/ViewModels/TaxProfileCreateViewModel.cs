using System.ComponentModel.DataAnnotations;

namespace WarehouseManagement.Core.ViewModels
{
    public class TaxProfileCreateViewModel
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = null!;

        public List<int> TaxIds { get; set; } = new List<int>();
    }

    public class TaxProfileEditViewModel
    {
        [Required]
        public int TaxProfileID { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = null!;

        public List<int> TaxIds { get; set; } = new List<int>();
    }

    public class TaxProfileViewModel
    {
        public int TaxProfileID { get; set; }
        public string Name { get; set; } = null!;
        public List<DefaultTaxViewModel> Taxes { get; set; } = new List<DefaultTaxViewModel>();
    }
}