using HSRP_BAL.IServices;
using HSRP_BAL.Services;
using HSRP_Management_API.Models;
using HSRP_Management_API.ResponseModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HSRP_Management_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly ILogger<AccountController> _logger;
        private readonly IApplicationUserServices _applicationUserServices;

        public AccountController(ILogger<AccountController> logger,
            IApplicationUserServices applicationUserServices)
        {
            _logger = logger;
            _applicationUserServices = applicationUserServices;
        }

        [HttpPost("/registration")]
        public async Task<IActionResult> Registration(ApplicationUser request)
        {
            if (request == null)
            {
                return BadRequest("Invalid user data.");
            }

            var list = await _applicationUserServices.GetAllUsers();

            var result = new HsrpResponse("User registration successful.",true,list);

            return Ok(result);
        }

        [HttpGet("/getAll")]
        public async Task<IActionResult>GetAllUsers()
        {
            var list = await _applicationUserServices.GetAllUsers();

            var result = new HsrpResponse("User registration successful.", true, list);

            return Ok(result);
        }
    }
}
