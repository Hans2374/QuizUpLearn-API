using Repository.Entities.BaseModelEntity;

namespace Repository.Entities
{
    public class UserMistake : BaseEntity
    {
        public Guid UserId { get; set; }
        public Guid QuizId { get; set; }
        public int TimesAttempted { get; set; }
        public int TimesWrong { get; set; }
        public DateTime LastAttemptedAt { get; set; }
        public bool IsAnalyzed { get; set; } = false;
        // Navigation property
        public virtual User? User { get; set; }
        public virtual Quiz? Quiz { get; set; }
    }
}
