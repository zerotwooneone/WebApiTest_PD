using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Api.Reload;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReloadController : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public ReloadController(IHttpClientFactory httpClientFactory,
            IHostingEnvironment hostingEnvironment)
        {
            _httpClientFactory = httpClientFactory;
        }
        // POST: api/Reload
        [HttpPost]
        public async Task<IActionResult> Post()
        {
            using (var httpClient = _httpClientFactory.CreateClient())
            {
                try
                {
                    var response = await httpClient.GetAsync("https://sampledata.petdesk.com/api/appointments");
                    if (!response.IsSuccessStatusCode)
                    {
                        return StatusCode((int)HttpStatusCode.InternalServerError, $"server returned: {response.StatusCode}");
                    }

                    var appointments = await response.Content.ReadAsAsync<IEnumerable<AppointmentModel>>();

                    int x = 0; //breakpoint here to test
                }
                catch (HttpRequestException e)
                {
                    return StatusCode((int) HttpStatusCode.InternalServerError, e.ToString());
                }
            }
            return Ok("something");
        }
    }
}
