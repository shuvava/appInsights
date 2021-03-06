﻿using System;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;


namespace Shuvava.Extensions.Metrics.Hosting
{
    public abstract class MetricsCollector : GcEventListener, IHostedService
    {
        private readonly ProcessSystemUsageCollector _systemUsageCollector;
        protected readonly MetricsCollectorSettings Settings;
        private Timer _timer;


        public MetricsCollector(IOptions<MetricsCollectorSettings> settings)
        {
            if (settings == null)
            {
                throw new ArgumentException(nameof(settings));
            }

            Settings = settings.Value;
            _systemUsageCollector = new ProcessSystemUsageCollector();
        }


        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(CollectData, null, 0, Settings.MetricsCollectionInterval);

            return Task.CompletedTask;
        }


        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Change(Timeout.Infinite, Timeout.Infinite);

            return Task.CompletedTask;
        }


        private void CollectData(object state)
        {
            var sysInfo = _systemUsageCollector.CollectData();

            var metrics = new AppProcessMetrics
            {
                CpuUsed = sysInfo.TotalCpuUsed,
                MemoryUsed = DotNetGC.GetTotalMemory(),
                ProcessWorkingSet = sysInfo.WorkingSet,
                GcGen0ObjectCount = DotNetGC.CollectionCount(0),
                GcGen1ObjectCount = DotNetGC.CollectionCount(1),
                GcGen2ObjectCount = DotNetGC.CollectionCount(2),
                HandleCount = sysInfo.HandleCount,
                ThreadCount = sysInfo.ThreadCount,
                ProcessStartTime = sysInfo.ProcessStartTime,
            };

            ProcessSystemUsageMetrics(metrics);
        }


        protected abstract void ProcessSystemUsageMetrics(AppProcessMetrics metrics);


        public override void Dispose()
        {
            _timer?.Dispose();
            base.Dispose();
        }
    }
}
