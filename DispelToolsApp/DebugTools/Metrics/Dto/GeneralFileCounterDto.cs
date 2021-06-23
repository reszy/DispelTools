namespace DispelTools.DebugTools.Metrics.Dto
{
    public class GeneralFileCounterDto : IMetricDto
    {
        private readonly string metricName;
        private readonly string filename;

        public GeneralFileCounterDto(string metricName, string filename)
        {
            this.metricName = metricName;
            this.filename = filename;
        }

        public string MetricName => $"FileMetric.{filename}";

        public MetricType MetricType => MetricType.COUNT;

        public object MetricValue => metricName;
    }
}
