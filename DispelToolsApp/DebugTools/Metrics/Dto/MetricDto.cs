namespace DispelTools.DebugTools.Metrics.Dto
{
    public interface IMetricDto
    {
        string MetricName { get; }
        MetricType MetricType { get; }
        object MetricValue { get; }
    }
}
