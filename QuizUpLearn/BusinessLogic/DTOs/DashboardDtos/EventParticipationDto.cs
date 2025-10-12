namespace BusinessLogic.DTOs.DashboardDtos
{
    public class EventParticipationDto
    {
        public Guid Id { get; set; }
        public string EventName { get; set; } = string.Empty;
        public string EventType { get; set; } = string.Empty; // "Challenge", "Battle", "Tournament"
        public string Rank { get; set; } = string.Empty; // "#15", "2nd Place", etc.
        public DateTime EventDate { get; set; }
        public int PointsEarned { get; set; }
        public string? IconUrl { get; set; }
        public string Status { get; set; } = string.Empty; // "Completed", "Ongoing", "Upcoming"
    }
}
