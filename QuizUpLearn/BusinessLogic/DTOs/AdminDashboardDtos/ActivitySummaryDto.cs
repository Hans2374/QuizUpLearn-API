namespace BusinessLogic.DTOs.AdminDashboardDtos
{
    public class ActivitySummaryDto
    {
        public string Period { get; set; } = string.Empty;
        public int NewUsers { get; set; }
        public int TotalQuizAttempts { get; set; }
        public int CompletedQuizzes { get; set; }
        public int NewEvents { get; set; }
        public int NewTournaments { get; set; }
        public TransactionSummaryDto Transactions { get; set; } = new();
        public int AIGenerations { get; set; }
    }

    public class TransactionSummaryDto
    {
        public int Total { get; set; }
        public int Completed { get; set; }
        public long Revenue { get; set; }
    }
}

