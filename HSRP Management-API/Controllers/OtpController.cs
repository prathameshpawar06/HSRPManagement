using HSRP_BAL.IServices;
using HSRP_DAL.Models;
using Microsoft.AspNetCore.Mvc;

namespace HSRP_Management_API.Controllers
{
    [ApiController]
    [Route("api/otp")]
    public class OtpController : Controller
    {
        private readonly IOtpService _otpService;
        public OtpController(IOtpService otpService) => _otpService = otpService;

        [HttpPost("send")]
        public async Task<IActionResult> Send([FromBody] SendOtpRequest req)
        {
            var res = await _otpService.SendOtpAsync(req);
            return Ok(res);
        }

        [HttpPost("verify")]
        public async Task<IActionResult> Verify([FromBody] VerifyOtpRequest req)
        {
            var res = await _otpService.VerifyOtpAsync(req);
            return res.Success ? Ok(res) : BadRequest(res);
        }
    }
}
