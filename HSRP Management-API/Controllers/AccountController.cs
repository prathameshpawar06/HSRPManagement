using Microsoft.AspNetCore.Mvc;

namespace HSRP_Management_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        [Route("/registration")]
        public IActionResult Registration()
        {
            return Ok("Account Controller is working!");
        }
    }
}
