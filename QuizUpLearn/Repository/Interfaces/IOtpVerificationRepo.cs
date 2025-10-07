using Repository.Entities;

namespace Repository.Interfaces
{
    public interface IOtpVerificationRepo
    {
        Task<OtpVerification> CreateAsync(OtpVerification entity);
        Task<OtpVerification?> GetValidOtpAsync(string email, string purpose, string otpCode);
        Task InvalidateAllActiveOtpsAsync(string email, string purpose);
        Task MarkUsedAsync(Guid id);
    }
}


