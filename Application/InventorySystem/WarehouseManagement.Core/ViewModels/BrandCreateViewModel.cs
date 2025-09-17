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

    public class BrandEditViewModel
    {
        public BrandEditViewModel()
        {

        }
        public int BrandID { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
    }

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
