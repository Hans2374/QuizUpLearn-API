namespace BusinessLogic.DTOs.QuizDtos
{
    public class QuizResponseDto
    {
        public Guid Id { get; set; }
        public Guid QuizSetId { get; set; }
        public string QuestionText { get; set; }
        public string CorrectAnswer { get; set; }
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
        //public List<AnswerOptionResponseDto> AnswerOptions { get; set; } = new List<AnswerOptionResponseDto>();
    }
}
