using System.ComponentModel.DataAnnotations;

namespace HSRP_DAL.Domains
{
    public class OtpEntry
    {
        [Key]
        public int Id { get; set; }

        // The principal this OTP is for (user id, phone number, email, or combination)
        public string? Subject { get; set; }          // e.g. "+9198xxxx" or "user@example.com"
        public string? Purpose { get; set; }          // e.g. "Login", "Transaction", "PasswordReset"

        // OTP storage: prefer hashed value; see service code below
        public string OtpHash { get; set; }          // hashed OTP
        public string OtpSalt { get; set; }          // optional salt for hashing

        public int OtpLength { get; set; } = 6;
        public DateTime ExpiresAt { get; set; }
        public bool IsUsed { get; set; } = false;
        public int Attempts { get; set; } = 0;       // number of verification attempts
        public int MaxAttempts { get; set; } = 5;

        public string CreatedBy { get; set; }        // optional, who triggered send
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? VerifiedAt { get; set; }
        public string Metadata { get; set; }         // JSON or free text (IP, device, txn id)
    }
}
