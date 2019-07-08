using GetMetrics.WebApp.Models;

using Microsoft.AspNetCore.Mvc;


namespace GetMetrics.WebApp.Controllers.V1
{
    [ApiVersion(ApiVersions.V1)]
    [Route( "api/v{version:apiVersion}/[controller]" )]
    [Route( "api/[controller]" )]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        [HttpGet]
        [ProducesResponseType(typeof(string), 200)]
        public ActionResult<string> Get() => "Hello world!";
    }
}
