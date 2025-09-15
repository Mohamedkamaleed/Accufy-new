namespace WarehouseManagement.Core.ViewModels
{
    public class BrandCreateViewModel
    {
        public BrandCreateViewModel()
        {
            
        }

        public int BrandID { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
    }

}
