using HSRP_BAL.IServices;
using HSRP_DAL.Models;
using HSRP_Management_API.ResponseModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HSRP_Management_API.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    [Authorize]
    public class BranchController : ControllerBase
    {
        private readonly IBranchesService _branchesService;

        public BranchController(IBranchesService branchesService)
        {
            _branchesService = branchesService;
        }

        [HttpGet]
        [Route("GetAllBranches")]
        public async Task<IActionResult> GetAllBranches()
        {
            var list = await _branchesService.GetAllBranchesAsync();
            return Ok(new HsrpResponse("List of all branches", true, null, list));
        }

        [HttpPost]
        [Route("CreateBranch")]
        public async Task<IActionResult> CreateBranch([FromBody] BranchesModel branch)
        {
            if (branch == null)
            {
                return BadRequest("Branch data is required.");
            }
            var createdBranch = await _branchesService.CreateBranchAsync(branch);
            return Ok(new HsrpResponse("Branch created successfully.", true, null, createdBranch));
        }

        [HttpPut]
        [Route("UpdateBranch")]
        public async Task<IActionResult> UpdateBranch([FromBody] BranchesModel branch)
        {
            if (branch == null)
            {
                return BadRequest("Branch data is required.");
            }
            var updatedBranch = await _branchesService.UpdateBranchAsync(branch);
            return Ok(new HsrpResponse("Branch updated successfully.", true, null, updatedBranch));
        }

        [HttpGet]
        [Route("GetBranchById/{branchId}")]
        public async Task<IActionResult> GetBranchById(int branchId)
        {
            var branch = await _branchesService.GetBranchByIdAsync(branchId);
            if (branch == null)
            {
                return NotFound("Branch not found.");
            }
            return Ok(new HsrpResponse("Branch details retrieved successfully.", true, null, branch));
        }

        [HttpDelete]
        [Route("DeleteBranch/{branchId}")]
        public async Task<IActionResult> DeleteBranch(int branchId)
        {
            var isDeleted = await _branchesService.DeleteBranchAsync(branchId);
            if (!isDeleted)
            {
                return NotFound("Branch not found or could not be deleted.");
            }
            return Ok(new HsrpResponse("Branch deleted successfully.", true, null));
        }
    }
}
