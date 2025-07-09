using Microsoft.AspNetCore.Identity;

namespace HSRP_Management_API.Models
{
    public class ApplicationUserModel 
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string? Email { get; set; }
        public string PhoneNumber { get; set; }
    }
}
