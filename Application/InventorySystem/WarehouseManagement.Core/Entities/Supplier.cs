namespace WarehouseManagement.Core.Entities
{
    public class Supplier
    {
        public int SupplierID { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
    }
}
