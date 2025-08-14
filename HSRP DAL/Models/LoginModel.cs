using System.ComponentModel.DataAnnotations;

namespace HSRP_DAL.Models
{
    public class LoginModel
    {
        public string? Username { get; set; } = null!;

        [RegularExpression(@"^[6-9]\d{9}$", ErrorMessage = "Phone number must be a valid 10-digit Indian number")]
        public string PhoneNumber { get; set; }

        //[Required(ErrorMessage = "Password is required")]
        public string? Password { get; set; } = null!;

        public string? OTP { get; set; } = null!;
        public bool IsContactProvided()
        {
            return !string.IsNullOrWhiteSpace(Username) || !string.IsNullOrWhiteSpace(PhoneNumber);
        }
    }
}
