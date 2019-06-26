namespace Shuvava.Extensions.Metrics.Models
{
    // ReSharper disable once InconsistentNaming
    /// <summary>
    /// <seealso cref="https://docs.microsoft.com/en-us/dotnet/framework/performance/garbage-collection-etw-events#gcallocationtick_v2_event"/>
    /// </summary>
    public struct GCMemoryAllocation
    {
        public ulong AllocationAmount { get; set; }
        public string TypeName { get; set; }
    }
}
