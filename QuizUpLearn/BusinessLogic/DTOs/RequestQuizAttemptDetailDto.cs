namespace BusinessLogic.DTOs
{
    public class RequestQuizAttemptDetailDto
    {
        public required Guid AttemptId { get; set; }
        public required Guid QuestionId { get; set; }
        public required string UserAnswer { get; set; } // Sẽ lưu AnswerOptionId dưới dạng string
        public int? TimeSpent { get; set; }
    }
}
