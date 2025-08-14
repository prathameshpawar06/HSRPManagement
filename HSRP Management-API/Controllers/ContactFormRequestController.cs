using HSRP_DAL.Models;
using HSRP_Management_API.ResponseModels;
using Microsoft.AspNetCore.Mvc;

namespace HSRP_Management_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ContactFormRequestController : ControllerBase
    {
        public async Task CreateContactFormRequest(ContactFormRequestModel request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request), "Contact form request cannot be null.");
            }

            await Task.CompletedTask;



            //return Ok(new HsrpResponse("Contact form request created successfully.", true, null));
        }
    }
}
