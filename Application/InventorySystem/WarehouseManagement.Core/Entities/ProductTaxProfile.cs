using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WarehouseManagement.Core.Entities
{
    public class ProductTaxProfile
    {
        [Key]
        [Column(Order = 1)]
        public int ProductID { get; set; }

        [Key]
        [Column(Order = 2)]
        public int TaxProfileID { get; set; }

        public bool IsPrimary { get; set; } = false;

        // Navigation properties
        public Product Product { get; set; } = null!;
        public TaxProfile TaxProfile { get; set; } = null!;
    }
}
