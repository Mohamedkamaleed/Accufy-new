namespace WarehouseManagement.Core.ViewModels
{
    public class BrandListViewModel
    {
        public BrandListViewModel()
        {
                
        }

        public int BrandID { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
    }

}
