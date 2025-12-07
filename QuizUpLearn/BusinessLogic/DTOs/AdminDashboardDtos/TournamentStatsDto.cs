namespace BusinessLogic.DTOs.AdminDashboardDtos
{
    public class TournamentStatsDto
    {
        public Guid TournamentId { get; set; }
        public string TournamentName { get; set; } = string.Empty;
        public int ParticipantCount { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Status { get; set; } = string.Empty;
    }
}

