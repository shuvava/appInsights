using System;
using System.Linq;

using Microsoft.Extensions.Options;

using Prometheus;

using Shuvava.Extensions.Metrics.Hosting;
using Shuvava.Extensions.Metrics.Models;


namespace Shuvava.Extensions.Metrics.Prometheus
{
    public class AppMetrics : MetricsCollector
    {
        private readonly Gauge.Child[] _collectionCounts = new Gauge.Child[3];
        private readonly Gauge _cpuGauge;
        private readonly DateTime _epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        private readonly Gauge _gcGenGauge;
        private readonly Gauge _memoryGauge;
        private readonly Gauge _numThreads;
        private readonly Gauge _openHandles;
        private readonly AppMetricsSettings _settings;
        private readonly Gauge _startTime;
        private readonly Gauge _workingSet;


        public AppMetrics(IOptions<AppMetricsSettings> options) : base(options)
        {
            _settings = options.Value;

            _cpuGauge = global::Prometheus.Metrics.CreateGauge(
                GetMetricName(MetricsConstants.CpuUsed), "Application Cpu Used"
            );

            _memoryGauge = global::Prometheus.Metrics.CreateGauge(
                GetMetricName(MetricsConstants.MemoryUsed), "Application Memory Used"
            );

            _startTime = global::Prometheus.Metrics.CreateGauge(GetMetricName("process_start_time_seconds"),
                "Start time of the process since unix epoch in seconds.");

            var collectionCountsParen = global::Prometheus.Metrics.CreateGauge(
                GetMetricName(MetricsConstants.GcGenObjectCount), "Application GC Gen 0 Object Count", "generation");

            foreach (var gen in Enumerable.Range(0, 3))
            {
                _collectionCounts[gen] = collectionCountsParen.Labels(gen.ToString());
            }

            _openHandles = global::Prometheus.Metrics.CreateGauge(GetMetricName("process_open_handles"), "Number of open handles");
            _numThreads = global::Prometheus.Metrics.CreateGauge(GetMetricName("process_num_threads"), "Total number of threads");
            _workingSet = global::Prometheus.Metrics.CreateGauge(GetMetricName("process_working_set_bytes"), "Process working set");
        }


        protected override bool _enableAllocationEvents { get; } = false;


        public string GetMetricName(string metric)
        {
            return $"{_settings.ApplicationName}_{metric}";
        }


        protected override void ProcessAllocationEvent(GCMemoryAllocation eventData)
        {
            // throw new NotImplementedException();
        }


        protected override void ProcessHeapStats(GCHeapStats eventData)
        {
            // throw new NotImplementedException();
        }


        protected override void ProcessSystemUsageMetrics(AppProcessMetrics metrics)
        {
            UpdateMetric(_cpuGauge, metrics.CpuUsed);
            UpdateMetric(_memoryGauge, metrics.MemoryUsed);
            UpdateMetric(_collectionCounts[0], metrics.GcGen0ObjectCount);
            UpdateMetric(_collectionCounts[1], metrics.GcGen1ObjectCount);
            UpdateMetric(_collectionCounts[2], metrics.GcGen2ObjectCount);
            _startTime.Set(metrics.ProcessStartTime.Subtract(_epoch).TotalSeconds);
            UpdateMetric(_openHandles, metrics.HandleCount);
            UpdateMetric(_numThreads, metrics.ThreadCount);
            UpdateMetric(_workingSet, metrics.ProcessWorkingSet);
        }


        public void UpdateMetric(IGauge metric, double value)
        {
            if (metric.Value > value)
            {
                metric.Dec(metric.Value - value);
            }
            else
            {
                metric.Inc(value - metric.Value);
            }
        }
    }
}
