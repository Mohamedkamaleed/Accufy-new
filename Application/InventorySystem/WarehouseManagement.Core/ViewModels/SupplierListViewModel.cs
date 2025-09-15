namespace WarehouseManagement.Core.ViewModels
{
    public class SupplierListViewModel
    {
        public SupplierListViewModel()
        {
                
        }

        public int SupplierID { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
    }

}
