namespace BusinessLogic.DTOs
{
    public class RequestQuizAttemptDto
    {
        public required Guid UserId { get; set; }
        public required Guid QuizSetId { get; set; }
        public required string AttemptType { get; set; }
        public int TotalQuestions { get; set; }
        public int CorrectAnswers { get; set; } = 0;
        public int WrongAnswers { get; set; } = 0;
        public int Score { get; set; } = 0;
        public decimal Accuracy { get; set; } = 0;
        public int? TimeSpent { get; set; }
        public Guid? OpponentId { get; set; }
        public bool? IsWinner { get; set; }
        public required string Status { get; set; }
    }
}
