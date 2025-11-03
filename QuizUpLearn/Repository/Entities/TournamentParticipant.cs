using Repository.Entities.BaseModelEntity;

namespace Repository.Entities
{
    public class TournamentParticipant : BaseEntity
    {
        public Guid TournamentId { get; set; }
        public Guid ParticipantId { get; set; }
        public DateTime JoinAt { get; set; }
        public DateTime? FinishAt { get; set; }
        // Navigation properties
        public virtual Tournament? Tournament { get; set; }
        public virtual User? Participant { get; set; }
    }
}
