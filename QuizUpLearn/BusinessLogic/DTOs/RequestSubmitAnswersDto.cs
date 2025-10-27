namespace BusinessLogic.DTOs
{
    public class RequestSubmitAnswersDto
    {
        public required Guid AttemptId { get; set; }
        public required List<AnswerDto> Answers { get; set; }
    }

    public class AnswerDto
    {
        public required Guid QuestionId { get; set; }
        public required string UserAnswer { get; set; } // Sẽ lưu AnswerOptionId dưới dạng string
        public int? TimeSpent { get; set; }
    }
}

