using System.ComponentModel.DataAnnotations;

namespace WarehouseManagement.Core.ViewModels
{
    public class WarehouseCreateViewModel
    {
        public WarehouseCreateViewModel()
        {

        }
        public int Id { get; set; }
        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [StringLength(500)]
        public string ShippingAddress { get; set; }

        public bool IsActive { get; set; } = true;
        public bool IsPrimary { get; set; }
    }



    public class WarehouseEditViewModel
    {
        public WarehouseEditViewModel()
        {

        }
        public int Id { get; set; }
        public string Name { get; set; }
        public string ShippingAddress { get; set; }
        public bool Status { get; set; }
        public bool IsPrimary { get; set; }
    }



    public class WarehouseListViewModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [StringLength(500)]
        public string ShippingAddress { get; set; }

        public bool IsActive { get; set; } = true;
        public bool IsPrimary { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string Status { get; set; }
    }
}
