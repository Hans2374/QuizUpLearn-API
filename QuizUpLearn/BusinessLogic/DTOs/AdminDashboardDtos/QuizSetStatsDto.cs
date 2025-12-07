namespace BusinessLogic.DTOs.AdminDashboardDtos
{
    public class QuizSetStatsDto
    {
        public int Total { get; set; }
        public QuizSetTypeCountDto ByType { get; set; } = new();
        public int Published { get; set; }
        public int Draft { get; set; }
        public int AIGenerated { get; set; }
        public int ManuallyCreated { get; set; }
        public double AverageQuestionsPerSet { get; set; }
    }

    public class QuizSetTypeCountDto
    {
        public int Practice { get; set; }
        public int Placement { get; set; }
        public int Tournament { get; set; }
        public int Event { get; set; }
    }
}

