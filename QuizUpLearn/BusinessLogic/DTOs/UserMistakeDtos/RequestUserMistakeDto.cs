namespace BusinessLogic.DTOs.UserMistakeDtos
{
    public class RequestUserMistakeDto
    {
        public Guid? UserId { get; set; }
        public Guid? QuizId { get; set; }
        public int TimesAttempted { get; set; } = 0;
        public int TimesWrong { get; set; } = 0;
        public DateTime LastAttemptedAt { get; set; } = DateTime.UtcNow;
        public bool IsAnalyzed { get; set; } = false;
    }
}
