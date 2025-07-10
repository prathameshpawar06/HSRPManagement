namespace HSRP_DAL.Models
{
    public class BranchesModel
    {
        public int? BranchId { get; set; } = 0; // Default to 0 if not set
        public string BranchName { get; set; }
        public string BranchAddress { get; set; }
        public string? BranchContactNumber { get; set; }
        public string? BranchEmail { get; set; }
        public int? BranchManagerId { get; set; } = 0; // Default to 0 if not set
        public bool BranchStatus { get; set; } 
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; } = DateTime.UtcNow;
        public int CreatedBy { get; set; } 
        public int? UpdatedBy { get; set; }
    }
}
