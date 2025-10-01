namespace BusinessLogic.DTOs
{
    public class LoginResponseDto
    {
        public required ResponseAccountDto Account { get; set; }
        public required string AccessToken { get; set; }
        public DateTime ExpiresAt { get; set; }
        public required string RefreshToken { get; set; }
        public DateTime RefreshExpiresAt { get; set; }
    }
}


