using System;
using System.Diagnostics.Tracing;


namespace getMetrics.CmdApp
{
    public class GcFinalizersEventListener : EventListener
    {
        // from https://docs.microsoft.com/en-us/dotnet/framework/performance/garbage-collection-etw-events
        private const int GC_KEYWORD =                 0x0000001;
        private const int TYPE_KEYWORD =               0x0080000;
        private const int GCHEAPANDTYPENAMES_KEYWORD = 0x1000000;

        protected override void OnEventSourceCreated(EventSource eventSource)
        {
            Console.WriteLine($"{eventSource.Guid} | {eventSource.Name}");

            // look for .NET Garbage Collection events
            if (eventSource.Name.Equals("Microsoft-Windows-DotNETRuntime"))
            {
                EnableEvents(
                    eventSource,
                    EventLevel.Verbose,
                    (EventKeywords) (GC_KEYWORD | GCHEAPANDTYPENAMES_KEYWORD | TYPE_KEYWORD)
                );
            }
        }

        // from https://blogs.msdn.microsoft.com/dotnet/2018/12/04/announcing-net-core-2-2/
        // Called whenever an event is written.
        protected override void OnEventWritten(EventWrittenEventArgs eventData)
        {
            // Write the contents of the event to the console.
            Console.WriteLine($"ThreadID = {eventData.OSThreadId} ID = {eventData.EventId} Name = {eventData.EventName}");
            for (int i = 0; i < eventData.Payload.Count; i++)
            {
                string payloadString = eventData.Payload[i] != null ? eventData.Payload[i].ToString() : string.Empty;
                Console.WriteLine($"    Name = \"{eventData.PayloadNames[i]}\" Value = \"{payloadString}\"");
            }
            Console.WriteLine("\n");
        }
    }
}
