using HSRP_DAL.Domains;

namespace HSRP_BAL.IServices
{
    public interface IApplicationUserServices
    {
        Task<List<ApplicationUser>> GetAllUsers();
    }
}
