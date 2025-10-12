namespace BusinessLogic.DTOs.DashboardDtos
{
    public class DashboardResponseDto
    {
        public DashboardStatsDto Stats { get; set; } = new DashboardStatsDto();
        public ProgressChartDto Progress { get; set; } = new ProgressChartDto();
        public List<RecentActivityDto> RecentActivities { get; set; } = new List<RecentActivityDto>();
        public List<EventParticipationDto> EventParticipations { get; set; } = new List<EventParticipationDto>();
        public List<QuizHistoryDto> RecentQuizHistory { get; set; } = new List<QuizHistoryDto>();
        public List<WeakPointDto> WeakPoints { get; set; } = new List<WeakPointDto>();
        public DateTime LastUpdated { get; set; }
    }
}
