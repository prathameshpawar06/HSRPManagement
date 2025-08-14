using System.ComponentModel.DataAnnotations;

namespace HSRP_DAL.Domains
{
    public enum RequestStatus
    {
        New,
        Reviewed,
        Closed
    }


    public class ContactFormRequest
    {
        [Key]
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? EmailAddress { get; set; }
        public string PhoneNumber { get; set; }
        public string? Message { get; set; }
        public string Status { get; set; } = "New";
    }
}
