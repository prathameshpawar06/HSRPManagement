using HSRP_BAL.IServices;
using HSRP_DAL.DBContext;
using HSRP_DAL.Domains;
using HSRP_DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace HSRP_BAL.Services
{
    public class BranchesService : IBranchesService
    {
        private readonly HSRPDbContext _context;

        public BranchesService(HSRPDbContext context)
        {
            _context = context;
        }

        public async Task<List<BranchesModel>> GetAllBranchesAsync()
        {
            var list = await _context.Branches.Select(b => new BranchesModel
            {
                BranchId = b.BranchId,
                BranchName = b.BranchName,
                BranchAddress = b.BranchAddress,
                BranchContactNumber = b.BranchContactNumber,
                BranchEmail = b.BranchEmail,
                BranchManagerId = b.BranchManagerId,
                BranchStatus = b.BranchStatus,
                CreatedAt = b.CreatedAt,
                UpdatedAt = b.UpdatedAt,
                CreatedBy = b.CreatedBy,
                UpdatedBy = b.UpdatedBy
            }).ToListAsync();

            return list;
        }

        public async Task<Branches> CreateBranchAsync(BranchesModel branch)
        {
            if (branch == null)
            {
                throw new ArgumentNullException(nameof(branch), "Branch cannot be null.");
            }
            var newBranch = new Branches
            {
                BranchName = branch.BranchName,
                BranchAddress = branch.BranchAddress,
                BranchContactNumber = branch.BranchContactNumber,
                BranchEmail = branch.BranchEmail,
                BranchManagerId = branch.BranchManagerId ?? 0,
                BranchStatus = branch.BranchStatus,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = branch.CreatedBy
            };
            _context.Branches.Add(newBranch);
            await _context.SaveChangesAsync();
            return newBranch;
        }

        public async Task<Branches> UpdateBranchAsync(BranchesModel branch)
        {
            if (branch == null)
            {
                throw new ArgumentNullException(nameof(branch), "Branch cannot be null.");
            }
            var existingBranch = await _context.Branches.FindAsync(branch.BranchId);
            if (existingBranch == null)
            {
                throw new KeyNotFoundException($"Branch with ID {branch.BranchId} not found.");
            }
            existingBranch.BranchName = branch.BranchName;
            existingBranch.BranchAddress = branch.BranchAddress;
            existingBranch.BranchContactNumber = branch.BranchContactNumber;
            existingBranch.BranchEmail = branch.BranchEmail;
            existingBranch.BranchManagerId = branch.BranchManagerId ?? 0;
            existingBranch.BranchStatus = branch.BranchStatus;
            existingBranch.UpdatedAt = DateTime.UtcNow;
            existingBranch.UpdatedBy = branch.UpdatedBy;
            _context.Entry(existingBranch).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return existingBranch;
        }

        public async Task<Branches> GetBranchByIdAsync(int branchId)
        {
            var branch = await _context.Branches.FindAsync(branchId);
            if (branch == null)
            {
                throw new KeyNotFoundException($"Branch with ID {branchId} not found.");
            }
            return branch;
        }

        public async Task<bool> DeleteBranchAsync(int branchId)
        {
            var branch = await _context.Branches.FindAsync(branchId);
            if (branch == null)
            {
                throw new KeyNotFoundException($"Branch with ID {branchId} not found.");
            }
            _context.Branches.Remove(branch);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
