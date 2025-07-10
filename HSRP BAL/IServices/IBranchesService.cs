using HSRP_DAL.Domains;
using HSRP_DAL.Models;

namespace HSRP_BAL.IServices
{
    public interface IBranchesService
    {
        Task<List<BranchesModel>> GetAllBranchesAsync();

        Task<Branches> CreateBranchAsync(BranchesModel branch);

        Task<Branches> UpdateBranchAsync(BranchesModel branch);

        Task<Branches> GetBranchByIdAsync(int branchId);

        Task<bool> DeleteBranchAsync(int branchId);
    }
}
