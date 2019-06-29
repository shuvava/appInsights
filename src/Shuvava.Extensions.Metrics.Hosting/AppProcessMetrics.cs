namespace Shuvava.Extensions.Metrics.Hosting
{
    public struct AppProcessMetrics
    {
        public double CpuUsed { get; set; }
        public long MemoryUsed { get; set; }
        public int GcGen0ObjectCount { get; set; }
        public int GcGen1ObjectCount { get; set; }
        public int GcGen2ObjectCount { get; set; }
    }
}
