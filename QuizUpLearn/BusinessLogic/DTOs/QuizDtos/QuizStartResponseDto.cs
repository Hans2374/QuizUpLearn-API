namespace BusinessLogic.DTOs.QuizDtos
{
    public class QuizStartResponseDto
    {
        public Guid Id { get; set; }
        public Guid QuizSetId { get; set; }
        public Guid? QuizGroupItemId { get; set; }
        public string QuestionText { get; set; }
        public string AudioURL { get; set; }
        public string ImageURL { get; set; }
        public string TOEICPart { get; set; }
        public int TimesAnswered { get; set; }
        public int TimesCorrect { get; set; }
        public int? OrderIndex { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public List<AnswerOptionStartDto> AnswerOptions { get; set; } = new List<AnswerOptionStartDto>();
    }
}
