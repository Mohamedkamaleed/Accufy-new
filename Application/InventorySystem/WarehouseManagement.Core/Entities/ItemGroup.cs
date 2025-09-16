using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WarehouseManagement.Core.Entities
{
    public class ItemGroup
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int GroupID { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = null!;

        public int? CategoryID { get; set; }

        public int? BrandID { get; set; }

        public string? Description { get; set; }

        // Navigation properties
        public Category? Category { get; set; }
        public Brand? Brand { get; set; }

        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}