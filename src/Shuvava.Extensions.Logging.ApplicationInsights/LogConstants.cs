namespace Shuvava.Extensions.Logging.ApplicationInsights
{
    public static class LogConstants
    {
        /// <summary>
        /// Gets the name of the key used to store the name of the function.
        /// </summary>
        public const string NameKey = "Name";
        /// <summary>
        /// Gets the event id for a metric event.
        /// </summary>
        public const int MetricEventId = 1;
        /// <summary>
        /// Gets the name of the key used to store a metric sum.
        /// </summary>
        public const string MetricValueKey = "Value";
    }
}
