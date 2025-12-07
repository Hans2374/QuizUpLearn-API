namespace BusinessLogic.DTOs.AdminDashboardDtos
{
    public class AdminOverviewDto
    {
        public long TotalRevenue { get; set; }
        public int CompletedTransactions { get; set; }
        public int TotalUsers { get; set; }
        public int ActiveSubscriptions { get; set; }
        public int TotalEvents { get; set; }
        public int TotalTournaments { get; set; }
        public int TotalAIUsage { get; set; }
        public int TotalQuizSets { get; set; }
    }
}

