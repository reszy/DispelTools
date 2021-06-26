using DispelTools.DebugTools.MetricTools.Dto;
using System;
using System.Collections.Generic;

namespace DispelTools.DebugTools.MetricTools
{
    internal class Metric
    {
        internal List<decimal> NumberValues { get; private set; } = new List<decimal>();
        internal List<string> StringValues { get; private set; } = new List<string>();
        internal Dictionary<string, long> CountValues { get; private set; } = new Dictionary<string, long>();
        internal MetricType MetricType { get; private set; }

        internal Metric(MetricDto metricDto)
        {
            MetricType = metricDto.MetricType;
            AddMetric(metricDto);
        }
        internal Metric(List<string> values)
        {
            MetricType = MetricType.STRING;
            StringValues = values;
        }
        internal Metric(List<decimal> values)
        {
            MetricType = MetricType.NUMBER;
            NumberValues = values;
        }
        internal Metric(Dictionary<string, long> values)
        {
            MetricType = MetricType.COUNT;
            CountValues = values;
        }
        internal void AddMetric(MetricDto metricDto)
        {
            switch (MetricType)
            {
                case MetricType.NUMBER:
                    NumberValues.Add(Convert.ToDecimal(metricDto.MetricValue));
                    break;
                case MetricType.STRING:
                    StringValues.Add((string)metricDto.MetricValue);
                    break;
                case MetricType.COUNT:
                    string key = metricDto.MetricSubName.ToString();
                    long sum = (long)metricDto.MetricValue;
                    if (CountValues.TryGetValue(key, out long counter))
                    {
                        sum += counter;
                    }
                    CountValues[key] = sum;
                    break;
            }
        }

        internal void Merge(Metric otherMetric)
        {
            if (MetricType != otherMetric.MetricType)
            {
                throw new ArgumentException("Metrics of wrong type are being merged");
            }
            switch (MetricType)
            {
                case MetricType.NUMBER:
                    NumberValues.AddRange(otherMetric.NumberValues);
                    break;
                case MetricType.STRING:
                    StringValues.AddRange(otherMetric.StringValues);
                    break;
                case MetricType.COUNT:
                    foreach (var newValue in otherMetric.CountValues)
                    {
                        long sum = newValue.Value;
                        if (CountValues.TryGetValue(newValue.Key, out long oldValue))
                        {
                            sum += oldValue;
                        }
                        CountValues[newValue.Key] = sum;
                    }
                    break;
            }
        }
    }
}
