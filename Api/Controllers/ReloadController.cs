using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReloadController : ControllerBase
    {
        // POST: api/Reload
        [HttpPost]
        public async Task<IActionResult> Post()
        {
            return Ok("something");
        }
    }
}
