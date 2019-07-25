using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;


namespace GetMetrics.WebApp.Controllers
{
    [Route("/tasks")]
    [ApiController]
    public class LongRunningTaskController : ControllerBase
    {
        private readonly ILogger<LongRunningTaskController> _logger;


        public LongRunningTaskController(ILogger<LongRunningTaskController> logger)
        {
            _logger = logger;
        }

        // GET
        [HttpGet("run")]
        public async Task<ActionResult<string>> Get()
        {
            await RunLongTask();
            return Ok();
        }


        private async Task RunLongTask()
        {
            foreach (var i in Enumerable.Range(0, 1000))
            {
                _logger.LogInformation($"{Thread.CurrentThread.ManagedThreadId} Do some work: {i}");
                await Task.Delay(TimeSpan.FromMilliseconds(500));
            }

        }
    }
}
