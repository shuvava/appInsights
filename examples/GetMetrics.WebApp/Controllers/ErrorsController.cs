using System.Net.Http;
using System.Threading.Tasks;

using GetMetrics.WebApp.Models;

using Microsoft.AspNetCore.Mvc;


namespace GetMetrics.WebApp.Controllers
{
    [ApiVersion(ApiVersions.V1)]
    [Route( "api/[controller]" )]
    [ApiController]
    public class ErrorsController : ControllerBase
    {
        private readonly HttpClient _httpClient;

        public ErrorsController()
        {
            _httpClient = new HttpClient();
        }

        [HttpGet("NotReusedHttpClient")]
        public async Task<IActionResult> NotReusedHttpClient()
        {
            using (var client = new HttpClient())
            {
                var result = await client.GetAsync("https://github.com/shuvava");

                return Ok(result.StatusCode);
            }
        }

        [HttpGet("ReusedHttpClient")]
        public async Task<IActionResult> ReusedHttpClient()
        {
            using (var result = await _httpClient.GetAsync("https://github.com/shuvava"))
            {
                return Ok(result.StatusCode);
            }
        }
    }
}
