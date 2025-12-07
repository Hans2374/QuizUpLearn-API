namespace BusinessLogic.DTOs.AdminDashboardDtos
{
    public class UserGrowthDto
    {
        public string Month { get; set; } = string.Empty;
        public int UserCount { get; set; }
        public int NewUsers { get; set; }
    }
}

