using Repository.Entities.BaseModelEntity;

namespace Repository.Entities
{
    public class QuizSet : BaseEntity
    {
        public required string Title { get; set; }
        public string Description { get; set; } = string.Empty;

        public Guid CreatorId { get; set; } //UserID

        public string? Category { get; set; }
        public string? DifficultyLevel { get; set; }
        public bool IsPublic { get; set; }
        public string AccessCode { get; set; } = Random.Shared.Next(0001, 9999).ToString();
        public string? ThumbnailUrl { get; set; }
        public int EstimatedTime { get; set; }
        public int TotalQuestions { get; set; }
        public int TotalPlays { get; set; }
        public double AvgScore { get; set; }
        public bool IsActive { get; set; }

        // Navigation
        public virtual User? Creator { get; set; }
        public virtual ICollection<Question> Questions { get; set; } = new List<Question>();
    }
}
