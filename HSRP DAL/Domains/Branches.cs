using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HSRP_DAL.Domains
{
    public class Branches
    {
        [Key]
        public int BranchId { get; set; }
        public string BranchName { get; set; }
        public string BranchAddress { get; set; }
        public string? BranchContactNumber { get; set; }
        public string? BranchEmail { get; set; }
        public int BranchManagerId { get; set; }
        public bool BranchStatus { get; set; } // Active, Inactive, etc.
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; } = DateTime.UtcNow;
        public int CreatedBy { get; set; } // User ID or name of the creator
        public int? UpdatedBy { get; set; } // User ID or name of the last updater
    }
}
