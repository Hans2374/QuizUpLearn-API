namespace BusinessLogic.DTOs
{
    public class ResponseAnswerOptionDto
    {
        public Guid Id { get; set; }
        public Guid QuizId { get; set; }
        public string OptionLabel { get; set; }
        public string OptionText { get; set; }
        public int OrderIndex { get; set; }
        public bool IsCorrect { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
    }
}
