using Repository.Entities.BaseModelEntity;

namespace Repository.Entities
{
    public class AnswerOption : BaseEntity
    {
        public Guid QuestionId { get; set; }

        public required string OptionText { get; set; }
        public bool IsCorrect { get; set; }
        public int OrderIndex { get; set; }

        // Navigation
        public virtual Question? Question { get; set; }
    }
}
