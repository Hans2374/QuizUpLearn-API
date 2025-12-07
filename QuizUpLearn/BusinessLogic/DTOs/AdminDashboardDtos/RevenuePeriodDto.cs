namespace BusinessLogic.DTOs.AdminDashboardDtos
{
    public class RevenuePeriodDto
    {
        public string Period { get; set; } = string.Empty;
        public long Amount { get; set; }
        public int TransactionCount { get; set; }
    }
}

