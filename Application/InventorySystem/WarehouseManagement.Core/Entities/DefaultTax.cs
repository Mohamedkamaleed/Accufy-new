using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WarehouseManagement.Core.Entities
{
    public class DefaultTax
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TaxID { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = null!;

        [Column(TypeName = "decimal(10,4)")]
        public decimal TaxValue { get; set; }

        [Required]
        [MaxLength(20)]
        public string Type { get; set; } = "Percentage"; // Percentage or Fixed

        [Required]
        [MaxLength(20)]
        public string Mode { get; set; } = "Exclusive"; // Included or Exclusive

        // Navigation property for many-to-many relationship
        public ICollection<TaxProfileTax> TaxProfileTaxes { get; set; } = new List<TaxProfileTax>();
    }
}