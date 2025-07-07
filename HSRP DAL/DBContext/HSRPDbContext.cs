using HSRP_DAL.Domains;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HSRP_DAL.DBContext
{
    public class HSRPDbContext : IdentityDbContext<ApplicationUser>
    {
        public HSRPDbContext(DbContextOptions options) : base(options)
        {
        }

    }

}
