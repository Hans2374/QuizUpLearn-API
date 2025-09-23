using Repository.Entities.BaseModelEntity;

namespace Repository.Entities
{
    public class OtpVerification : BaseEntity
    {
        public Guid AccountId { get; set; }

        public required string Email { get; set; }
        public required string OTPCode { get; set; }
        public string? Purpose { get; set; }
        public bool IsUsed { get; set; }
        public DateTime ExpiresAt { get; set; }
        public DateTime? UsedAt { get; set; }

        // Navigation
        public virtual Account? Account { get; set; }
    }
}
