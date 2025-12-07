namespace BusinessLogic.DTOs.AdminDashboardDtos
{
    public class AIUsageDto
    {
        public string Period { get; set; } = string.Empty;
        public int QueryCount { get; set; }
        public int QuestionCount { get; set; }
        public double SuccessRate { get; set; }
    }
}

