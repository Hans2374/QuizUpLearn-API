using Repository.Entities.BaseModelEntity;

namespace Repository.Entities
{
    public class Account : BaseEntity
    {
        public required string Email { get; set; }
        public required string PasswordHash { get; set; }

        public Guid UserId { get; set; }
        public Guid RoleId { get; set; }

        public bool IsEmailVerified { get; set; }
        public string? EmailVerificationToken { get; set; }
        public string? PasswordResetToken { get; set; }
        public DateTime? PasswordResetExpires { get; set; }
        public DateTime? LastLoginAt { get; set; }
        public int LoginAttempts { get; set; }
        public DateTime? LockoutUntil { get; set; }
        public bool IsActive { get; set; }
        public bool IsBanned { get; set; }

        // Navigation
        public virtual Role? Role { get; set; }
        public virtual ICollection<OtpVerification> OTPVerifications { get; set; } = new List<OtpVerification>();
        public virtual ICollection<QuizSet> QuizSets { get; set; } = new List<QuizSet>();
        public virtual User? User { get; set; }
    }
}
