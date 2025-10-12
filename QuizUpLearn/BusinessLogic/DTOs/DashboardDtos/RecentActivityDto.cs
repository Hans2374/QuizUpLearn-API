namespace BusinessLogic.DTOs.DashboardDtos
{
    public class RecentActivityDto
    {
        public Guid Id { get; set; }
        public string ActivityType { get; set; } = string.Empty; // "QuizCompleted", "GameJoined", "AchievementUnlocked", "QuizCreated"
        public string Description { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; }
        public string? IconUrl { get; set; }
        public Dictionary<string, object>? Metadata { get; set; } // Additional data like quiz name, achievement name, etc.
    }
}
