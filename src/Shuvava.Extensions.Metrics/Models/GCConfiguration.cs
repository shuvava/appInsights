using System.Runtime;


namespace Shuvava.Extensions.Metrics.Models
{
    // ReSharper disable InconsistentNaming
    public struct GCConfiguration
    {
        public bool IsServerGC { get; set; }
        public GCLargeObjectHeapCompactionMode LargeObjectHeapCompactionMode { get; set; }
    }
}
