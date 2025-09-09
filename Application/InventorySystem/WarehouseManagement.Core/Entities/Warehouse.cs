using System.ComponentModel.DataAnnotations;
using WarehouseManagement.Core.Interfaces;

namespace WarehouseManagement.Core.Entities
{
    public class Warehouse : ITrackable
    {// Parameterless constructor (needed for Dapper)
        public Warehouse() { }

        // Your custom constructor (used by your own code)

        public Warehouse(string name, string shippingAddress, bool status, bool isPrimary)
        {
            Name = name;
            ShippingAddress = shippingAddress;
            Status = status;
            IsPrimary = isPrimary;
            CreatedDate = DateTime.UtcNow;
        }

        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [StringLength(500)]
        public string ShippingAddress { get; set; }

        public bool Status { get; set; } = true;
        public bool IsPrimary { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedAt { get; set; }
        DateTime ITrackable.CreatedDate { get => CreatedDate; set => CreatedDate = value; }
        DateTime? ITrackable.UpdatedAt { get => UpdatedAt; set => UpdatedAt = value; }

        public void Update(string name, string shippingAddress, bool isActive)
        {
            Name = name;
            ShippingAddress = shippingAddress;
            Status = isActive;
            UpdatedAt = DateTime.UtcNow;
        }

        public void SetAsPrimary()
        {
            IsPrimary = true;
            UpdatedAt = DateTime.UtcNow;
        }

        public void RemoveAsPrimary()
        {
            IsPrimary = false;
            UpdatedAt = DateTime.UtcNow;
        }

        public bool CanBeDeactivated() => !IsPrimary;
    }
}
