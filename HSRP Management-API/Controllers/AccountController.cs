using HSRP_BAL.IServices;
using HSRP_BAL.Services;
using HSRP_DAL.Domains;
using HSRP_Management_API.Models;
using HSRP_Management_API.ResponseModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HSRP_Management_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly ILogger<AccountController> _logger;
        private readonly IApplicationUserServices _applicationUserServices;
        private readonly JwtService _jwtService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountController(ILogger<AccountController> logger,
            IApplicationUserServices applicationUserServices,
            JwtService jwtService,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
        {
            _logger = logger;
            _applicationUserServices = applicationUserServices;
            _jwtService = jwtService;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpPost("/registration")]
        public async Task<IActionResult> Registration(ApplicationUserModel request)
        {
            if (request == null)
            {
                return BadRequest("Invalid user data.");
            }

            var user = new ApplicationUser
            {
                UserName = request.UserName,
                Email = request.Email,
                PhoneNumber = request.PhoneNumber
            };

            var result = await _userManager.CreateAsync(user, request.Password);
            if (result.Succeeded)
            {
                var response = new HsrpResponse("User registration successful.", true, null);

                return Ok(response);
            }

            return BadRequest(result.Errors);
        }

        [HttpGet("/getAll")]
        public async Task<IActionResult>GetAllUsers()
        {
            var list = await _applicationUserServices.GetAllUsers();

            var result = new HsrpResponse("User registration successful.", true, null ,list);

            return Ok(result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!model.IsContactProvided())
                return BadRequest("You must enter either Email or Phone number.");

            ApplicationUser? user;
            if(!string.IsNullOrEmpty(model.PhoneNumber))
            {
                user = await _userManager.Users.FirstOrDefaultAsync(x => x.PhoneNumber == model.PhoneNumber);
            }
            else
            {
                user = await _userManager.FindByNameAsync(model.Username ?? "");
            }

            if (user == null)
            {
                return Unauthorized("Invalid username or password.");
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);

            if(!result.Succeeded)
            {
                return Unauthorized("Invalid username or password.");
            }

            var roles = (await _userManager.GetRolesAsync(user)).FirstOrDefault() ?? "EMP";
            var token = _jwtService.GenerateToken(user.Id, roles);

            return Ok(new HsrpResponse("Login Successfull",true, token,null));
        }
    }
}
