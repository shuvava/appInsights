namespace Shuvava.Extensions.Metrics.Models
{
    // ReSharper disable once InconsistentNaming
    /// <summary>
    /// <seealso cref="https://docs.microsoft.com/en-us/dotnet/framework/performance/garbage-collection-etw-events#gcheapstats_v1_event"/>
    /// </summary>
    public struct GCHeapStats
    {
        /// <summary>
        /// The size, in bytes, of generation 0 memory.
        /// </summary>
        public ulong GenerationSize0 { get; set; }
        /// <summary>
        /// The number of bytes that are promoted from generation 0 to generation 1.
        /// </summary>
        public ulong TotalPromotedSize0 { get; set; }
        /// <summary>
        /// The size, in bytes, of generation 1 memory.
        /// </summary>
        public ulong GenerationSize1 { get; set; }
        /// <summary>
        /// The number of bytes that are promoted from generation 1 to generation 2.
        /// </summary>
        public ulong TotalPromotedSize1 { get; set; }
        /// <summary>
        /// The size, in bytes, of generation 2 memory.
        /// </summary>
        public ulong GenerationSize2 { get; set; }
        /// <summary>
        /// The number of bytes that survived in generation 2 after the last collection.
        /// </summary>
        public ulong TotalPromotedSize2 { get; set; }
        /// <summary>
        /// The size, in bytes, of the large object heap.
        /// </summary>
        public ulong GenerationSize3 { get; set; }
        /// <summary>
        /// The number of bytes that survived in the large object heap after the last collection.
        /// </summary>
        public ulong TotalPromotedSize3 { get; set; }
    }
}
