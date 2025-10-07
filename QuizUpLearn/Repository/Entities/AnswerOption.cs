using Repository.Entities.BaseModelEntity;

namespace Repository.Entities
{
    public class AnswerOption : BaseEntity
    {
        public Guid QuizId { get; set; }
        public string OptionLabel { get; set; }
        public string OptionText { get; set; }
        public int OrderIndex { get; set; }
        public bool IsCorrect { get; set; } = false;
        // Navigation
        public virtual Quiz? Question { get; set; }
    }
}
