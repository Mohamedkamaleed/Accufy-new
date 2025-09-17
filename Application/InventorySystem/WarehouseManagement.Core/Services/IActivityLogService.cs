using WarehouseManagement.Core.Common;
using WarehouseManagement.Core.Entities;
using WarehouseManagement.Core.Repositories;
using WarehouseManagement.Core.ViewModels;

namespace WarehouseManagement.Core.Services
{
    public interface IActivityLogService
    {
        Task<List<TimelineEventViewModel>> GetTimelineEventsAsync(int productId);
        Task<List<ActivityLogViewModel>> GetActivityLogsAsync(int productId);
        Task LogActivityAsync(string actionType, int productId, string userId, string beforeValue = "", string afterValue = "", string details = "");
    }
}