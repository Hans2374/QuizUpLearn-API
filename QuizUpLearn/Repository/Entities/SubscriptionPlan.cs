using Repository.Entities.BaseModelEntity;

namespace Repository.Entities
{
    public class SubscriptionPlan : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public long Price { get; set; } = 0; //in vnd
        public int DurationInDays { get; set; }
    }
}
