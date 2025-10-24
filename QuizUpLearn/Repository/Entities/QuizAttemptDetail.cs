using Repository.Entities.BaseModelEntity;

namespace Repository.Entities
{
    public class QuizAttemptDetail : BaseEntity
    {
        public Guid AttemptId { get; set; }
        public Guid QuestionId { get; set; }
        public string UserAnswer { get; set; } // Giữ lại để backward compatibility
        public Guid SelectedAnswerOptionId { get; set; } // ID của AnswerOption được chọn
        public bool? IsCorrect { get; set; }
        public int? TimeSpent { get; set; }

        // Foreign key properties to match database schema
        public Guid? QuizAttemptId { get; set; }
        public Guid QuizId { get; set; }

        // Navigation properties
        public virtual QuizAttempt? QuizAttempt { get; set; }
        public virtual Quiz Quiz { get; set; }
        public virtual AnswerOption? SelectedAnswerOption { get; set; }
    }
}
