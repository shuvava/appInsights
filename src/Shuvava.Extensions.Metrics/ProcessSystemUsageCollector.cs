using System;
using System.Diagnostics;

using Shuvava.Extensions.Metrics.Models;


namespace Shuvava.Extensions.Metrics
{
    public class ProcessSystemUsageCollector
    {
        private static readonly int processorTotal = Environment.ProcessorCount;
        private readonly Process _process = Process.GetCurrentProcess();
        private double _lastPrivilegedProcessorTime;
        private DateTime _lastTimeStamp;
        private double _lastTotalProcessorTime;
        private double _lastUserProcessorTime;
        private DateTime _newTimeStamp;


        public ProcessSystemUsage CollectData()
        {
            _process.Refresh();
            _newTimeStamp = DateTime.UtcNow;
            var newTotalProcessorTime = _process.TotalProcessorTime.TotalMilliseconds;
            var newPrivilegedProcessorTime = _process.PrivilegedProcessorTime.TotalMilliseconds;
            var newUserProcessorTime = _process.UserProcessorTime.TotalMilliseconds;

            var totalCpuTimeUsed = newTotalProcessorTime - _lastTotalProcessorTime;
            var privilegedCpuTimeUsed = newPrivilegedProcessorTime - _lastPrivilegedProcessorTime;
            var userCpuTimeUsed = newUserProcessorTime - _lastUserProcessorTime;
            var cpuTimeElapsed = _newTimeStamp.Subtract(_lastTimeStamp).TotalMilliseconds * processorTotal;

            _lastTimeStamp = _newTimeStamp;
            _lastTotalProcessorTime = newTotalProcessorTime;
            _lastPrivilegedProcessorTime = newPrivilegedProcessorTime;
            _lastUserProcessorTime = newUserProcessorTime;

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
                ThreadCount = _process.Threads?.Count ?? 0,
                ProcessStartTime = _process.StartTime
            };
        }
    }
}
