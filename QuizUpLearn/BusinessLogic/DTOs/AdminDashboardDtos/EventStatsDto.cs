namespace BusinessLogic.DTOs.AdminDashboardDtos
{
    public class EventStatsDto
    {
        public Guid EventId { get; set; }
        public string EventName { get; set; } = string.Empty;
        public int ParticipantCount { get; set; }
        public DateTime StartDate { get; set; }
        public string Status { get; set; } = string.Empty;
    }
}

