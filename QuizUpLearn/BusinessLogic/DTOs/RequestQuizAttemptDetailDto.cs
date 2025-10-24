namespace BusinessLogic.DTOs
{
    public class RequestQuizAttemptDetailDto
    {
        public required Guid AttemptId { get; set; }
        public required Guid QuestionId { get; set; }
        public required Guid SelectedAnswerOptionId { get; set; } // ID của AnswerOption được chọn
        public int? TimeSpent { get; set; }
    }
}
