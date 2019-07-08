using System.Reflection;

using Microsoft.AspNetCore.Mvc;

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

using Shuvava.Extensions.Metrics;
using Shuvava.Extensions.Metrics.Models;


namespace GetMetrics.WebApp.Controllers
{
    [Route("/app")]
    [ApiController]
    public class VersionController : ControllerBase
    {
        private readonly string _version;
        private readonly string _model;


        public VersionController()
        {
            var settings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore,
                Formatting = Formatting.Indented
            };

            settings.Converters.Add(new StringEnumConverter {NamingStrategy = new CamelCaseNamingStrategy()});
            _version = Assembly.GetExecutingAssembly().GetName().Version.ToString();
            var gcConfig = DotNetGC.GetConfig();

            var model = new VersionModel
            {
                Version = _version,
                GC = gcConfig,
            };

            _model = JsonConvert.SerializeObject(model, settings);
        }


        [HttpGet("/")]
        [HttpGet("version")]
        [ProducesResponseType(typeof(string), 200)]
        public ActionResult<string> Get()
        {
            return _model;
        }


        public class VersionModel
        {
            public string Version { get; set; }


            public GCConfiguration GC { get; set; }
        }
    }
}
