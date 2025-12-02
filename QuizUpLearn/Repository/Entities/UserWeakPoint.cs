using Repository.Entities.BaseModelEntity;
using System.ComponentModel.DataAnnotations.Schema;

namespace Repository.Entities
{
    public class UserWeakPoint : BaseEntity
    {
        public Guid UserId { get; set; }
        
        [ForeignKey("UserMistake")]
        public Guid UserMistakeId { get; set; }
        
        public required string WeakPoint { get; set; }
        public required string ToeicPart { get; set; }
        public string? DifficultyLevel { get; set; }
        public string? Advice { get; set; }
        
        public virtual User? User { get; set; }
        public virtual UserMistake? UserMistake { get; set; }
    }
}
