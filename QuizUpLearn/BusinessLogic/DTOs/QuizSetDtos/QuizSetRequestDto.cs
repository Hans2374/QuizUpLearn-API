using BusinessLogic.DTOs.QuizDtos;

namespace BusinessLogic.DTOs.QuizSetDtos
{
    public class QuizSetRequestDto
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string QuizType { get; set; } = string.Empty;
        public string SkillType { get; set; } = string.Empty;
        public string DifficultyLevel { get; set; } = string.Empty;
        public int TimeLimit { get; set; } = 1000; // in seconds
        public Guid CreatedBy { get; set; }
        public bool IsAIGenerated { get; set; } = true;
        public bool IsPublished { get; set; } = false;
        public bool IsPremiumOnly { get; set; } = false;
        // optional fields
        public string TOEICPart { get; set; } = string.Empty;
        // navgation property
        public List<QuizRequestDto> Quizzes { get; set; } = new List<QuizRequestDto>();
    }
}
