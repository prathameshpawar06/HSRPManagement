namespace HSRP_DAL.Models
{
    public class SendOtpRequest
    {
        public string Subject { get; set; }      // phone or email
        public string Purpose { get; set; }      // e.g., "Login"
        public int Length { get; set; } = 6;
        public int ValidForSeconds { get; set; } = 300; // 5 mins
        public string Metadata { get; set; }
    }
}
