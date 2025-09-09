namespace WarehouseManagement.Core.Interfaces
{
    public interface ITrackable
    {
        DateTime CreatedDate { get; set; }
        DateTime? UpdatedAt { get; set; }
    }
}
