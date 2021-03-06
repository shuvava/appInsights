using System.Collections.Generic;
using System.Collections.ObjectModel;

using Microsoft.Extensions.Logging;


namespace Shuvava.Extensions.Logging.ApplicationInsights
{
    //https://github.com/Azure/azure-webjobs-sdk/blob/dev/src/Microsoft.Azure.WebJobs.Host/Loggers/Logger/LoggerExtensions.cs
    public static class LoggerExtensions
    {
        /// <summary>
        ///     Logs a metric value.
        /// </summary>
        /// <param name="logger">The ILogger.</param>
        /// <param name="name">The name of the metric.</param>
        /// <param name="value">The value of the metric.</param>
        /// <param name="properties">Named string values for classifying and filtering metrics.</param>
        public static void LogMetric(this ILogger logger, string name, double value,
            IDictionary<string, object> properties = null)
        {
            IDictionary<string, object> state = properties == null
                ? new Dictionary<string, object>()
                : new Dictionary<string, object>(properties);


            state[LogConstants.NameKey] = name;
            state[LogConstants.MetricValueKey] = value;

            IDictionary<string, object> payload = new ReadOnlyDictionary<string, object>(state);
            logger?.Log(LogLevel.Information, LogConstants.MetricEventId, payload, null, (s, e) => null);
        }
    }
}
