using Repository.Entities.BaseModelEntity;
using Repository.Enums;

namespace Repository.Entities
{
    public class Transaction : BaseEntity
    {
        public Guid UserId { get; set; }
        public Guid SubscriptionPlanId { get; set; }
        public long Amount { get; set; }
        public DateTime? CompletedDate { get; set; }
        public TransactionStatusEnum Status { get; set; } = TransactionStatusEnum.Pending;
        public string? PaymentGatewayTransactionId { get; set; }
        public virtual User? User { get; set; }
        public virtual SubscriptionPlan? SubscriptionPlan { get; set; }
    }
}
