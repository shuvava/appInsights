using System;
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
        private static readonly Random getrandom = new Random();
        private static HttpClient _client = new HttpClient();

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
        public async Task<IActionResult> ReusedHttpClient([FromServices] IHttpClientFactory clientFactory)
        {
            var httpClient = clientFactory.CreateClient("github");
            using (var result = await httpClient.GetAsync("https://github.com/shuvava"))
            {
                return Ok(result.StatusCode);
            }
        }


        [HttpGet("HttpClientNoUsingWithExceptions")]
        public async Task<IActionResult> HttpClientNoUsingWithExceptions()
        {
            var ran = getrandom.Next(0, 100);
            var client = new HttpClient();
            var result = await client.GetAsync("https://github.com/shuvava");

            if (ran > 50)
            {
                throw new Exception("test");
            }
            return Ok(result.StatusCode);
        }

        [HttpGet("HttpClientUsingWithExceptions")]
        public async Task<IActionResult> HttpClientUsingWithExceptions()
        {
            var ran = getrandom.Next(0, 100);
            using(var client = new HttpClient())
            using (var result = await client.GetAsync("https://github.com/shuvava"))
            {
                if (ran > 50)
                {
                    throw new Exception("test");
                }
                return Ok(result.StatusCode);
            }
        }

        [HttpGet("ReusedHttpClientUsingWithExceptions")]
        public async Task<IActionResult> ReusedHttpClientUsingWithExceptions([FromServices] IHttpClientFactory clientFactory)
        {
            var ran = getrandom.Next(0, 100);
            using (var client = clientFactory.CreateClient("github"))
            using (var result = await client.GetAsync("https://github.com/shuvava"))
            {
                if (ran > 50)
                {
                    throw new Exception("test");
                }
                return Ok(result.StatusCode);
            }
        }

        [HttpGet("StaticHttpClientUsingWithExceptions")]
        public async Task<IActionResult> StaticHttpClientUsingWithExceptions()
        {
            var ran = getrandom.Next(0, 100);
            using (var result = await _client.GetAsync("https://github.com/shuvava"))
            {
                if (ran > 50)
                {
                    throw new Exception("test");
                }
                return Ok(result.StatusCode);
            }
        }
    }
}
