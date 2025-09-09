namespace WarehouseManagement.Core.ViewModels
{
    public class CategoryEditViewModel
    {
        public CategoryEditViewModel()
        {
            
        }
        public int CategoryID { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public int? ParentCategoryID { get; set; }
    }

}
