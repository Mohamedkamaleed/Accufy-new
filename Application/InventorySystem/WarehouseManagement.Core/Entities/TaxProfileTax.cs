using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WarehouseManagement.Core.Entities
{
    public class TaxProfileTax
    {
        [Key]
        [Column(Order = 1)]
        public int TaxProfileID { get; set; }

        [Key]
        [Column(Order = 2)]
        public int TaxID { get; set; }

        // Navigation properties
        public TaxProfile TaxProfile { get; set; } = null!;
        public DefaultTax DefaultTax { get; set; } = null!;
    }
}