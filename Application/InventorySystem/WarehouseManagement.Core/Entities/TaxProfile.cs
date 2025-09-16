using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WarehouseManagement.Core.Entities
{
    public class TaxProfile
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TaxProfileID { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = null!;

        [Column(TypeName = "decimal(5,2)")]
        public decimal TaxRate { get; set; }

        [MaxLength(500)]
        public string? Description { get; set; }


        //    // Navigation properties
        public ICollection<TaxProfileTax> TaxProfileTaxes { get; set; } = new List<TaxProfileTax>();
        public ICollection<ProductTaxProfile> ProductTaxProfiles { get; set; } = new List<ProductTaxProfile>();
    }

    //public class TaxProfile
    //{
    //    [Key]
    //    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    //    public int TaxProfileID { get; set; }

    //    [Required]
    //    [MaxLength(100)]
    //    public string Name { get; set; } = null!;

    //    // Navigation properties
    //    public ICollection<TaxProfileTax> TaxProfileTaxes { get; set; } = new List<TaxProfileTax>();
    //    public ICollection<ProductTaxProfile> ProductTaxProfiles { get; set; } = new List<ProductTaxProfile>();
    //}
}
