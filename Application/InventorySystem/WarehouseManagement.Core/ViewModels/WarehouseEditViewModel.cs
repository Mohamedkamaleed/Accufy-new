namespace WarehouseManagement.Core.ViewModels
{
    public class WarehouseEditViewModel
    {
        public WarehouseEditViewModel()
        {
                
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public string ShippingAddress { get; set; }
        public bool Status { get; set; }
        public bool IsPrimary { get; set; }
    }
}
