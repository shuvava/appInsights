using System.Diagnostics.Tracing;


namespace Shuvava.Extensions.Metrics
{
    public abstract class GcEventListener : EventListener
    {
        // ReSharper disable once InconsistentNaming
        // from https://docs.microsoft.com/en-us/dotnet/framework/performance/garbage-collection-etw-events
        private const int GC_KEYWORD = 0x0000001;
        protected EventSource DotNetRuntime;
        private readonly EventLevel _eventLevel;


        public GcEventListener(bool enableAllocationEvents = false)
        {
            _eventLevel = enableAllocationEvents ? EventLevel.Verbose : EventLevel.Informational;
        }


        protected override void OnEventSourceCreated(EventSource eventSource)
        {
            // look for .NET Garbage Collection events
            if (!eventSource.Name.Equals("Microsoft-Windows-DotNETRuntime"))
            {
                return;
            }

            DotNetRuntime = eventSource;
            // EventLevel.Verbose enables the AllocationTick events, but also a heap of other stuff
            // and will increase the memory allocation of your application since it's a lot of data to digest.
            // EventLevel.Information is more light weight and is recommended if you don't need the allocation data.
            EnableEvents(eventSource, _eventLevel, (EventKeywords) GC_KEYWORD);
        }
    }
}
