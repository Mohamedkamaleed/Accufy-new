using System.ComponentModel.DataAnnotations;
using WarehouseManagement.Core.Entities;

namespace WarehouseManagement.Core.ViewModels
{
    public class ItemGroupCreateViewModel
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = null!;

        public int? CategoryID { get; set; }

        public int? BrandID { get; set; }

        public string? Description { get; set; }
    }

    public class ItemGroupEditViewModel
    {
        [Required]
        public int GroupID { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = null!;

        public int? CategoryID { get; set; }

        public int? BrandID { get; set; }

        public string? Description { get; set; }
    }

    public class ItemGroupViewModel
    {
        public int GroupID { get; set; }
        public string Name { get; set; } = null!;
        public int? CategoryID { get; set; }
        public string? CategoryName { get; set; }
        public int? BrandID { get; set; }
        public string? BrandName { get; set; }
        public string? Description { get; set; }
        public int ProductCount { get; set; }
        public List<Product> Products = new List<Product>();
    }
}
