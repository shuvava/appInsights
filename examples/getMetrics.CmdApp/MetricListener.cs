using System;

using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

using Shuvava.Extensions.Metrics;
using Shuvava.Extensions.Metrics.Models;


namespace getMetrics.CmdApp
{
    public class MetricListener : GcEventListener
    {
        private readonly JsonSerializerSettings _settings;

        public MetricListener(bool enableAllocationEvents = false):base(enableAllocationEvents)
        {
            _settings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore,
                Formatting = Formatting.Indented
            };
        }


        protected override void ProcessAllocationEvent(GCMemoryAllocation eventData)
        {
            var result = JsonConvert.SerializeObject(eventData, _settings);
            Console.WriteLine($"{nameof(ProcessAllocationEvent)}: {result}");
        }


        protected override void ProcessHeapStats(GCHeapStats eventData)
        {
            var result = JsonConvert.SerializeObject(eventData, _settings);
            Console.WriteLine($"{nameof(ProcessHeapStats)}: {result}");
        }


        public void Dispose()
        {

            base.Dispose();
        }
    }
}
