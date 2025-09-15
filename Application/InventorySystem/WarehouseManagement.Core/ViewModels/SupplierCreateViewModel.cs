namespace WarehouseManagement.Core.ViewModels
{
    public class SupplierCreateViewModel
    {
        public SupplierCreateViewModel()
        {
            
        }

        public int SupplierID { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
    }

}
