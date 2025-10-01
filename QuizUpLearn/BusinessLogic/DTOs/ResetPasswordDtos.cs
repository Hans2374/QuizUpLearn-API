namespace BusinessLogic.DTOs
{
    public class ResetPasswordInitiateRequestDto
    {
        public required string Email { get; set; }
    }

    public class ResetPasswordVerifyRequestDto
    {
        public required string Email { get; set; }
        public required string OtpCode { get; set; }
    }

    public class ResetPasswordConfirmRequestDto
    {
        public required string Email { get; set; }
        public required string NewPassword { get; set; }
        public required string OtpCode { get; set; }
    }
}


