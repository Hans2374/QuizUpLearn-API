namespace BusinessLogic.DTOs
{
    public class RequestAnswerOptionDto
    {
        public required Guid QuizId { get; set; }
        public required string OptionLabel { get; set; }
        public required string OptionText { get; set; }
        public int OrderIndex { get; set; }
        public bool IsCorrect { get; set; } = false;
    }
}
