namespace BusinessLogic.DTOs.DashboardDtos
{
    public class ProgressDataDto
    {
        public string Day { get; set; } = string.Empty;
        public double ScorePercentage { get; set; }
        public DateTime Date { get; set; }
    }

    public class ProgressChartDto
    {
        public List<ProgressDataDto> WeeklyProgress { get; set; } = new List<ProgressDataDto>();
        public double OverallAccuracy { get; set; }
        public int TotalCorrectAnswers { get; set; }
        public int TotalWrongAnswers { get; set; }
        public int TotalQuestions { get; set; }
    }
}
