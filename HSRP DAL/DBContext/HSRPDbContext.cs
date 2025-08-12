using HSRP_DAL.Domains;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HSRP_DAL.DBContext
{
    public class HSRPDbContext : IdentityDbContext<ApplicationUser,ApplicationRole,int>
    {
        public HSRPDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Branches> Branches { get; set; }
        public DbSet<OtpEntry> OtpEntries { get; set; }

    }

}
