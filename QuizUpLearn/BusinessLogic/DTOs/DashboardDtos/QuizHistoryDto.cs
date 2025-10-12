namespace BusinessLogic.DTOs.DashboardDtos
{
    public class QuizHistoryDto
    {
        public Guid Id { get; set; }
        public string QuizName { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public DateTime CompletedAt { get; set; }
        public double ScorePercentage { get; set; }
        public string Status { get; set; } = string.Empty; // "Passed", "Failed"
        public int TimeSpent { get; set; } // in minutes
        public int TotalQuestions { get; set; }
        public int CorrectAnswers { get; set; }
    }
}
