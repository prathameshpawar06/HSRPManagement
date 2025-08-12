namespace HSRP_DAL.Models
{
    public class VerifyOtpResponse
    {
        public bool Success { get; set; }
        public string Reason { get; set; }
        public DateTime? VerifiedAt { get; set; }
    }
}
