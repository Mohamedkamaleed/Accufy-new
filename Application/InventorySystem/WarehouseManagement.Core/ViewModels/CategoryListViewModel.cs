namespace WarehouseManagement.Core.ViewModels
{
    public class CategoryListViewModel
    {
        public CategoryListViewModel()
        {
                
        }
        public int CategoryID { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public string? ParentCategoryName { get; set; }
    }

}
