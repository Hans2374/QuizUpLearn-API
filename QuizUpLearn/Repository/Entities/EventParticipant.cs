using Repository.Entities.BaseModelEntity;

namespace Repository.Entities
{
    public class EventParticipant : BaseEntity
    {
        public Guid EventId { get; set; }
        public Guid ParticipantId { get; set; }
        public long Score { get; set; }
        public double Accuracy { get; set; }
        public long Rank { get; set; }
        public DateTime JoinAt { get; set; }
        public DateTime? FinishAt { get; set; }
        // Navigation properties
        public virtual Event? Event { get; set; }
        public virtual User? Participant { get; set; }
    }
}
