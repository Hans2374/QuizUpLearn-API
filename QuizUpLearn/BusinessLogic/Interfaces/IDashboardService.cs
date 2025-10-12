using BusinessLogic.DTOs.DashboardDtos;

namespace BusinessLogic.Interfaces
{
    public interface IDashboardService
    {
        Task<DashboardResponseDto> GetDashboardDataAsync(Guid userId);
        Task<DashboardStatsDto> GetUserStatsAsync(Guid userId);
        Task<ProgressChartDto> GetUserProgressAsync(Guid userId, int days = 7);
        Task<List<RecentActivityDto>> GetRecentActivitiesAsync(Guid userId, int count = 10);
        Task<List<EventParticipationDto>> GetEventParticipationsAsync(Guid userId, int count = 5);
        Task<List<QuizHistoryDto>> GetRecentQuizHistoryAsync(Guid userId, int count = 10);
        Task<List<WeakPointDto>> GetUserWeakPointsAsync(Guid userId, int count = 5);
        Task<bool> RecordActivityAsync(Guid userId, string activityType, string description, Dictionary<string, object>? metadata = null);
    }
}
