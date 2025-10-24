namespace BusinessLogic.DTOs
{
    public class AnswerOptionStartDto
    {
        public Guid Id { get; set; }
        public Guid QuizId { get; set; }
        public string OptionLabel { get; set; }
        public string OptionText { get; set; }
        public int OrderIndex { get; set; }
    }
}
