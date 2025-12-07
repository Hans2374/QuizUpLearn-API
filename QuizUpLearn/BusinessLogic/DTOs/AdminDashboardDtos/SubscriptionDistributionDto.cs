namespace BusinessLogic.DTOs.AdminDashboardDtos
{
    public class SubscriptionDistributionDto
    {
        public string PlanName { get; set; } = string.Empty;
        public Guid PlanId { get; set; }
        public int ActiveCount { get; set; }
        public double Percentage { get; set; }
        public long TotalRevenue { get; set; }
    }
}

