using DispelTools.DebugTools.MetricTools.Dto;
using System.Collections.Generic;

namespace DispelTools.DebugTools.MetricTools
{
    internal class MetricsContainer
    {
        public MetricsContainer(string filename, bool overwriteFile = false)
        {
            Filename = filename;
            OverwriteFile = overwriteFile;
        }

        public string Filename { get; set; }
        public bool OverwriteFile { get; set; }

        public Dictionary<string, Metric> Metrics { get; set; } = new Dictionary<string, Metric>();
        public void AddMetric(MetricDto metricDto)
        {
            if (Metrics.TryGetValue(metricDto.MetricName, out var metric))
            {
                metric.AddMetric(metricDto);
            }
            else
            {
                Metrics[metricDto.MetricName] = new Metric(metricDto);
            }
        }
    }
}
