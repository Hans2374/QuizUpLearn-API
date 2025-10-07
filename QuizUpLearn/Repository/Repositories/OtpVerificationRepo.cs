using Microsoft.EntityFrameworkCore;
using Repository.DBContext;
using Repository.Entities;
using Repository.Interfaces;

namespace Repository.Repositories
{
    public class OtpVerificationRepo : IOtpVerificationRepo
    {
        private readonly MyDbContext _context;

        public OtpVerificationRepo(MyDbContext context)
        {
            _context = context;
        }

        public async Task<OtpVerification> CreateAsync(OtpVerification entity)
        {
            await _context.OTPVerifications.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task InvalidateAllActiveOtpsAsync(string email, string purpose)
        {
            var normalized = email.Trim().ToLowerInvariant();
            var now = DateTime.UtcNow;
            var active = await _context.OTPVerifications
                .Where(o => o.Email == normalized && o.Purpose == purpose && !o.IsUsed && o.ExpiresAt > now)
                .ToListAsync();
            foreach (var item in active)
            {
                item.IsUsed = true;
                item.UsedAt = DateTime.UtcNow;
            }
            if (active.Count > 0)
            {
                _context.OTPVerifications.UpdateRange(active);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<OtpVerification?> GetValidOtpAsync(string email, string purpose, string otpCode)
        {
            var normalized = email.Trim().ToLowerInvariant();
            var now = DateTime.UtcNow;
            return await _context.OTPVerifications
                .OrderByDescending(o => o.CreatedAt)
                .FirstOrDefaultAsync(o => o.Email == normalized && o.Purpose == purpose && o.OTPCode == otpCode && !o.IsUsed && o.ExpiresAt > now);
        }

        public async Task MarkUsedAsync(Guid id)
        {
            var entity = await _context.OTPVerifications.FindAsync(id);
            if (entity == null) return;
            entity.IsUsed = true;
            entity.UsedAt = DateTime.UtcNow;
            _context.OTPVerifications.Update(entity);
            await _context.SaveChangesAsync();
        }
    }
}


