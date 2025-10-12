namespace BusinessLogic.DTOs.DashboardDtos
{
    public class WeakPointDto
    {
        public string Topic { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public int MistakesCount { get; set; }
        public double AccuracyRate { get; set; }
        public List<string> CommonMistakes { get; set; } = new List<string>();
        public string DifficultyLevel { get; set; } = string.Empty; // "Beginner", "Intermediate", "Advanced"
    }
}
