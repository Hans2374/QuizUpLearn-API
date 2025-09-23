using Repository.Entities.BaseModelEntity;

namespace Repository.Entities
{
    public class Question : BaseEntity
    {
        public Guid QuizSetId { get; set; }

        public required string QuestionText { get; set; }
        public required string QuestionType { get; set; }
        public string? DifficultyLevel { get; set; }
        public string? Explanation { get; set; }
        public int TimeLimit { get; set; }
        public int Points { get; set; }
        public int OrderIndex { get; set; }
        public bool IsActive { get; set; }
        // Navigation
        public virtual QuizSet? QuizSet { get; set; }
        public virtual ICollection<AnswerOption> AnswerOptions { get; set; } = new List<AnswerOption>();
    }
}
