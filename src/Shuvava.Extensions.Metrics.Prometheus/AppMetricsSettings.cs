using Shuvava.Extensions.Metrics.Hosting;


namespace Shuvava.Extensions.Metrics.Prometheus
{
    public class AppMetricsSettings: MetricsCollectorSettings
    {
        public string ApplicationName { get; set; }
    }
}
