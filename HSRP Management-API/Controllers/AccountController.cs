using HSRP_BAL.IServices;
using HSRP_BAL.Services;
using HSRP_DAL.Domains;
using HSRP_DAL.Models;
using HSRP_Management_API.ResponseModels;
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
        private readonly IOtpService _otpService;

        public AccountController(ILogger<AccountController> logger,
            IApplicationUserServices applicationUserServices,
            JwtService jwtService,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IOtpService otpService)
        {
            _logger = logger;
            _applicationUserServices = applicationUserServices;
            _jwtService = jwtService;
            _userManager = userManager;
            _signInManager = signInManager;
            _otpService = otpService;
        }

        [HttpPost("registration")]
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

        [HttpGet("getAll")]
        public async Task<IActionResult> GetAllUsers()
        {
            var list = await _applicationUserServices.GetAllUsers();

            var result = new HsrpResponse("User registration successful.", true, null, list);

            return Ok(result);
        }

        [HttpPost("checkUserByMobileNumber")]
        public async Task<IActionResult> CheckUserByMobileNumber([FromBody] LoginModel model)
        {
            if(string.IsNullOrWhiteSpace(model.PhoneNumber))
            {
                return BadRequest("Mobile number is required.");
            }

            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.PhoneNumber == model.PhoneNumber);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            await _otpService.SendOtpAsync(new SendOtpRequest
            {
                Subject = user.PhoneNumber ?? "",
                Purpose = "Login",
                Length = 4,
                ValidForSeconds = 900, // 5 minutes
                Metadata = user.Id.ToString()
            });

            return Ok(new HsrpResponse("Login Successfull", true, null, user));
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!model.IsContactProvided())
                return BadRequest("You must enter either Email/Username or Phone number.");

            ApplicationUser? user = null;

            // --- OTP LOGIN FLOW ---
            if (!string.IsNullOrEmpty(model.OTP))
            {
                // Identify user by phone number
                user = await _userManager.Users.FirstOrDefaultAsync(x => x.PhoneNumber == model.PhoneNumber);
                if (user == null)
                    return Unauthorized("User not found for the provided phone number.");

                // Verify OTP
                var otpResult = await _otpService.VerifyOtpAsync(new VerifyOtpRequest
                {
                    Subject = model.PhoneNumber ?? "",
                    OtpCode = model.OTP,
                    Purpose = "Login"
                });

                if (!otpResult.Success)
                    return Unauthorized("Invalid or expired OTP.");
            }
            // --- PASSWORD LOGIN FLOW ---
            else
            {
                // Identify user by username or email
                user = await _userManager.FindByNameAsync(model.Username ?? "")
                       ?? await _userManager.Users.FirstOrDefaultAsync(x=>x.PhoneNumber == model.PhoneNumber);

                if (user == null)
                    return Unauthorized("Invalid username or password.");

                // Check password
                var passwordResult = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);
                if (!passwordResult.Succeeded)
                    return Unauthorized("Invalid username or password.");
            }

            // --- JWT GENERATION ---
            var roles = (await _userManager.GetRolesAsync(user)).FirstOrDefault() ?? "EMP";
            var token = _jwtService.GenerateToken(user.Id.ToString(), roles);

            return Ok(new HsrpResponse("Login Successful", true, token, null));
        }

    }
}
