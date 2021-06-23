namespace DispelTools.DebugTools.Metrics.Dto
{
    public class GeneralFileStatusDto : IMetricDto
    {
        private readonly string metricName;
        private readonly string filename;
        private readonly string value;

        public GeneralFileStatusDto(string metricName, string filename, string value)
        {
            this.metricName = metricName;
            this.filename = filename;
            this.value = value;
        }

        public string MetricName => $"FileMetric.{metricName}.{filename}";

        public MetricType MetricType => MetricType.STRING;

        public object MetricValue => value;
    }
}
