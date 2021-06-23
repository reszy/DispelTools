namespace DispelTools.DebugTools.Metrics.Dto
{
    public class GeneralFileMetricDto : IMetricDto
    {
        private readonly string metricName;
        private readonly string filename;
        private readonly decimal value;

        public GeneralFileMetricDto(string metricName, string filename, decimal value)
        {
            this.metricName = metricName;
            this.filename = filename;
            this.value = value;
        }

        public string MetricName => $"FileMetric.{metricName}.{filename}";

        public MetricType MetricType => MetricType.NUMBER;

        public object MetricValue => value;
    }
}
