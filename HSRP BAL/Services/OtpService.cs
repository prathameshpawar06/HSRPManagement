using System.Security.Cryptography;
using System.Text;
using HSRP_BAL.IServices;
using HSRP_DAL.DBContext;
using HSRP_DAL.Domains;
using HSRP_DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace HSRP_BAL.Services
{
    public class OtpService : IOtpService
    {
        public  readonly HSRPDbContext _context;

        public OtpService(HSRPDbContext context)
        {
            _context = context;
        }

        public async Task<SendOtpResponse> SendOtpAsync(SendOtpRequest req)
        {
            var otp = GenerateNumericOtp(req.Length);
            var salt = Convert.ToBase64String(RandomNumberGenerator.GetBytes(16));
            var hash = ComputeHmacHash(otp, salt);

            var entry = new OtpEntry
            {
                Subject = req.Subject,
                Purpose = req.Purpose,
                OtpHash = hash,
                OtpSalt = salt,
                OtpLength = req.Length,
                ExpiresAt = DateTime.UtcNow.AddSeconds(req.ValidForSeconds),
                Metadata = req.Metadata,
                CreatedBy = "Admin",
            };

            _context.Set<OtpEntry>().Add(entry);
            await _context.SaveChangesAsync();

            //await _provider.SendAsync(req.Subject, $"Your OTP is: {otp} (valid {req.ValidForSeconds / 60} min)");

            return new SendOtpResponse
            {
                OtpId = entry.Id,
                ExpiresAt = entry.ExpiresAt
            };
        }

        public async Task<VerifyOtpResponse> VerifyOtpAsync(VerifyOtpRequest req)
        {
            OtpEntry entry = null;

            if (req.OtpId > 0)
            {
                entry = await _context.Set<OtpEntry>().FindAsync(req.OtpId);
            }
            else
            {
                entry = await _context.Set<OtpEntry>()
                    .Where(x => x.Subject == req.Subject && x.Purpose == req.Purpose /*&& !x.IsUsed*/)
                    .OrderByDescending(x => x.CreatedAt)
                    .FirstOrDefaultAsync();
            }

            if (entry == null)
                return new VerifyOtpResponse { Success = false, Reason = "NotFound" };

            if (entry.IsUsed)
                return new VerifyOtpResponse { Success = false, Reason = "AlreadyUsed" };

            if (entry.ExpiresAt < DateTime.UtcNow)
                return new VerifyOtpResponse { Success = false, Reason = "Expired" };

            if (entry.Attempts >= entry.MaxAttempts)
                return new VerifyOtpResponse { Success = false, Reason = "MaxAttemptsExceeded" };

            var computed = ComputeHmacHash(req.OtpCode, entry.OtpSalt);
            if (computed == entry.OtpHash)
            {
                entry.IsUsed = true;
                entry.VerifiedAt = DateTime.UtcNow;
                await _context.SaveChangesAsync();
                return new VerifyOtpResponse { Success = true, VerifiedAt = entry.VerifiedAt };
            }
            else
            {
                entry.Attempts += 1;
                await _context.SaveChangesAsync();
                return new VerifyOtpResponse { Success = false, Reason = "Invalid" };
            }
        }

        private static string GenerateNumericOtp(int length)
        {
            var sb = new StringBuilder(length);
            for (int i = 0; i < length; i++)
                sb.Append(RandomNumberGenerator.GetInt32(0, 10));
            return sb.ToString();
        }

        private static string ComputeHmacHash(string value, string salt)
        {
            using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(salt));
            var bytes = hmac.ComputeHash(Encoding.UTF8.GetBytes(value));
            return Convert.ToBase64String(bytes);
        }
    }
}
