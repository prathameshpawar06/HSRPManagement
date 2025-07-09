using HSRP_BAL.IServices;
using HSRP_DAL.DBContext;
using HSRP_DAL.Domains;
using Microsoft.EntityFrameworkCore;

namespace HSRP_BAL.Services
{
    public class ApplicationUserServices : IApplicationUserServices
    {

        private readonly HSRPDbContext _context;
        public ApplicationUserServices(HSRPDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Get all registered users in the system.
        /// </summary>
        /// <returns>user list </returns>
        public async Task<List<ApplicationUser>> GetAllUsers()
        {
            var userList = await _context.Users.ToListAsync();
            return userList;
        }
    }
}
