using System.Diagnostics.Tracing;

using Shuvava.Extensions.Metrics.Models;


namespace Shuvava.Extensions.Metrics
{
    public abstract class GcEventListener : EventListener
    {
        // ReSharper disable InconsistentNaming
        // from https://docs.microsoft.com/en-us/dotnet/framework/performance/garbage-collection-etw-events
        private const int GC_KEYWORD = 0x0000001;
        // ReSharper enable InconsistentNaming


        protected abstract bool _enableAllocationEvents { get; }


        protected override void OnEventSourceCreated(EventSource eventSource)
        {
            // look for .NET Garbage Collection events
            if (!eventSource.Name.Equals("Microsoft-Windows-DotNETRuntime"))
            {
                return;
            }

            // EventLevel.Verbose enables the AllocationTick events, but also a heap of other stuff
            // and will increase the memory allocation of your application since it's a lot of data to digest.
            // EventLevel.Information is more light weight and is recommended if you don't need the allocation data.
            var eventLevel = _enableAllocationEvents ? EventLevel.Verbose : EventLevel.Informational;

            EnableEvents(
                eventSource,
                eventLevel,
                (EventKeywords) GC_KEYWORD
            );
        }


        // from https://blogs.msdn.microsoft.com/dotnet/2018/12/04/announcing-net-core-2-2/
        // Called whenever an event is written.
        protected override void OnEventWritten(EventWrittenEventArgs eventData)
        {
            switch (eventData.EventName)
            {
                case "GCHeapStats_V1":
                    var heapStatus = new GCHeapStats
                    {
                        GenerationSize0 = (ulong) eventData.Payload[0],
                        TotalPromotedSize0 = (ulong) eventData.Payload[1],
                        GenerationSize1 = (ulong) eventData.Payload[2],
                        TotalPromotedSize1 = (ulong) eventData.Payload[3],
                        GenerationSize2 = (ulong) eventData.Payload[4],
                        TotalPromotedSize2 = (ulong) eventData.Payload[5],
                        GenerationSize3 = (ulong) eventData.Payload[6],
                        TotalPromotedSize3 = (ulong) eventData.Payload[7]
                    };

                    ProcessHeapStats(heapStatus);

                    break;

                case "GCAllocationTick_V3":
                    var memStatus = new GCMemoryAllocation
                    {
                        AllocationAmount = (ulong) eventData.Payload[3],
                        TypeName = (string) eventData.Payload[5]
                    };

                    ProcessAllocationEvent(memStatus);

                    break;
            }
        }


        protected abstract void ProcessAllocationEvent(GCMemoryAllocation eventData);
        protected abstract void ProcessHeapStats(GCHeapStats eventData);
    }
}
