using Repository.Entities.BaseModelEntity;

namespace Repository.Entities
{
    public class User : BaseEntity
    {
        public Guid AccountId { get; set; }

        public required string Username { get; set; }
        public string FullName { get; set; } = string.Empty;
        public required string AvatarUrl { get; set; }
        public string? Bio { get; set; }
        public int LoginStreak { get; set; }
        public DateTime? LastLoginDate { get; set; }
        public int TotalPoints { get; set; }
        public string? PreferredLanguage { get; set; }
        public string? Timezone { get; set; }
        public List<string>? WeakPoints { get; set; } = new List<string>();

        // Navigation
        public virtual Account? Account { get; set; }
        public virtual ICollection<QuizSet> CreatedQuizSets { get; set; } = new List<QuizSet>();
        public virtual ICollection<QuizAttempt> QuizAttempts { get; set; } = new List<QuizAttempt>();
    }
}
