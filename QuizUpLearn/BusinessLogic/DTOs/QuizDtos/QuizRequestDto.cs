namespace BusinessLogic.DTOs.QuizDtos
{
    public class QuizRequestDto
    {
        public Guid QuizSetId { get; set; }
        public string QuestionText { get; set; } = string.Empty;
        public string CorrectAnswer { get; set; } = string.Empty;
        public string AudioURL { get; set; } = string.Empty;
        public string ImageURL { get; set; } = string.Empty;
        public string TOEICPart { get; set; } = string.Empty;
        public int? OrderIndex { get; set; }
        public bool IsActive { get; set; } = true;
        //public List<AnswerOptionDto> AnswerOptions { get; set; } = new List<AnswerOptionDto>();
    }
    
}
