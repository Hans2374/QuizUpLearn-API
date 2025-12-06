using Repository.Entities.BaseModelEntity;

namespace Repository.Entities
{
    public class UserWeakPoint : BaseEntity
    {
        public Guid UserId { get; set; }
        public required string WeakPoint { get; set; }
        public required string ToeicPart { get; set; }
        public string? DifficultyLevel { get; set; }
        public string? Advice { get; set; }
        public virtual User? User { get; set; }
        public virtual ICollection<UserMistake>? UserMistakes { get; set; }
    }
}
