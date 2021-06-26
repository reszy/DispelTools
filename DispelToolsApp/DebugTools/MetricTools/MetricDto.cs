namespace DispelTools.DebugTools.MetricTools.Dto
{
    public struct MetricDto
    {
        public MetricDto(string metricName, string metricSubName, MetricType metricType, object metricValue)
        {
            MetricName = metricName;
            MetricSubName = metricSubName;
            MetricType = metricType;
            MetricValue = metricValue;
        }

        public string MetricName { get; private set; }

        public string MetricSubName { get; private set; }

        public MetricType MetricType { get; private set; }

        public object MetricValue { get; private set; }
    }
}
