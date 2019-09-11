using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AppointmentsController : ControllerBase
    {
        // GET: api/Appointments
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok("something");
        }
    }
}
