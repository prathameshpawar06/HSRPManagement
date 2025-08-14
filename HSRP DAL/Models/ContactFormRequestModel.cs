using System.ComponentModel.DataAnnotations;

namespace HSRP_DAL.Models
{
    public class ContactFormRequestModel
    {
        [Required(ErrorMessage = "First Name is required")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last Name is required")]
        public string LastName { get; set; }

        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string? EmailAddress { get; set; }

        [Phone(ErrorMessage = "Invalid Phone Number")]
        public string PhoneNumber { get; set; }

        public string Message { get; set; }
    }
}
