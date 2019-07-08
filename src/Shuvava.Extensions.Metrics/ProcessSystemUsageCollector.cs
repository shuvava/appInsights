using System;
using System.Diagnostics;

using Shuvava.Extensions.Metrics.Models;


namespace Shuvava.Extensions.Metrics
{
    public class ProcessSystemUsageCollector
    {
        private readonly Process _process = Process.GetCurrentProcess();
        private TimeSpan _lastPrivilegedProcessorTime = TimeSpan.Zero;
        private DateTime _lastTimeStamp;
        private TimeSpan _lastTotalProcessorTime = TimeSpan.Zero;
        private TimeSpan _lastUserProcessorTime = TimeSpan.Zero;


        public ProcessSystemUsage CollectData()
        {
            _process.Refresh();
            var totalCpuTimeUsed = _process.TotalProcessorTime.TotalMilliseconds -
                                   _lastTotalProcessorTime.TotalMilliseconds;

            var privilegedCpuTimeUsed = _process.PrivilegedProcessorTime.TotalMilliseconds -
                                        _lastPrivilegedProcessorTime.TotalMilliseconds;

            var userCpuTimeUsed =
                _process.UserProcessorTime.TotalMilliseconds - _lastUserProcessorTime.TotalMilliseconds;

            _lastTotalProcessorTime = _process.TotalProcessorTime;
            _lastPrivilegedProcessorTime = _process.PrivilegedProcessorTime;
            _lastUserProcessorTime = _process.UserProcessorTime;
            var cpuTimeElapsed = (DateTime.UtcNow - _lastTimeStamp).TotalMilliseconds * Environment.ProcessorCount;
            _lastTimeStamp = DateTime.UtcNow;

            return new ProcessSystemUsage
            {
                TotalCpuUsed = totalCpuTimeUsed * 100 / cpuTimeElapsed,
                PrivilegedCpuUsed = privilegedCpuTimeUsed * 100 / cpuTimeElapsed,
                UserCpuUsed = userCpuTimeUsed * 100 / cpuTimeElapsed,
                TotalCpuUsedInMilliseconds = totalCpuTimeUsed,
                WorkingSet = _process.WorkingSet64,
                NonPagedSystemMemory = _process.NonpagedSystemMemorySize64,
                PagedMemory = _process.PagedMemorySize64,
                PagedSystemMemory = _process.PagedSystemMemorySize64,
                PrivateMemory = _process.PrivateMemorySize64,
                VirtualMemoryMemory = _process.VirtualMemorySize64,
                HandleCount = _process.HandleCount,
                ThreadCount = _process.Threads.Count,
                ProcessStartTime = _process.StartTime,
            };
        }
    }
}
