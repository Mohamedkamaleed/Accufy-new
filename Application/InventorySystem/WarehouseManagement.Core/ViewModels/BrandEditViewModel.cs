namespace WarehouseManagement.Core.ViewModels
{
    public class BrandEditViewModel
    {
        public BrandEditViewModel()
        {

        }
        public int BrandID { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
    }

}
