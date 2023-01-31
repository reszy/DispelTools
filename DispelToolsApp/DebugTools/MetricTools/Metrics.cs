using DispelTools.Common;
using DispelTools.DebugTools.MetricTools.Dto;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace DispelTools.DebugTools.MetricTools
{
    public static partial class Metrics
    {
        public static bool Enabled { get; set; } = false;
        private static readonly JsonSerializer serializer = CreateSerializer();
        private static readonly Dictionary<MetricFile, MetricsContainer> metrics = CreateMetrics();

        public static void Count(MetricFile metricFile, string metricName, long count = 1) => Count(metricFile, "Not grouped counters", metricName, count);
        public static void Count(MetricFile metricFile, string group, string metricName, long count = 1) => AddMetric(metricFile, new MetricDto(group, metricName, MetricType.COUNT, count));
        public static void Gauge(MetricFile metricFile, string metricName, long value) => AddMetric(metricFile, new MetricDto(metricName, "", MetricType.NUMBER, value));
        public static void List(MetricFile metricFile, string metricName, string value) => AddMetric(metricFile, new MetricDto(metricName, "", MetricType.STRING, value));

        public static void AddMetric(MetricFile metricFile, MetricDto metricDto)
        {
            if (Enabled)
            {
                metrics[metricFile].AddMetric(metricDto);
            }
        }

        public static void DumpMetrics(object? sender, EventArgs args)
        {
            if (Enabled)
            {
                foreach (var entry in metrics)
                {
                    var metricContainer = entry.Value;
                    string metricsFilename = GetMetricsFilePath(metricContainer.Filename);
                    if (!metricContainer.OverwriteFile && Settings.FS.File.Exists(metricsFilename))
                    {
                        using (var reader = Settings.FS.File.OpenText(metricsFilename))
                        {
                            var oldMetrics = serializer.Deserialize(reader, typeof(Dictionary<string, Metric>)) as Dictionary<string, Metric>;
                            if (oldMetrics != null)
                            {
                                Merge(oldMetrics, metricContainer);
                            }
                        }
                    }
                    using (var writer = new StreamWriter(Settings.FS.File.Create(metricsFilename)))
                    {
                        serializer.Serialize(writer, metricContainer.Metrics);
                    }
                    metricContainer.Metrics.Clear();
                }
            }
        }

        private static void Merge(Dictionary<string, Metric> oldMetrics, MetricsContainer existingMetrics)
        {
            foreach (var newerMetric in existingMetrics.Metrics)
            {
                if (oldMetrics.TryGetValue(newerMetric.Key, out var olderMetric))
                {
                    olderMetric.Merge(newerMetric.Value);
                }
                else
                {
                    oldMetrics[newerMetric.Key] = newerMetric.Value;
                }
            }
            existingMetrics.Metrics = oldMetrics;
        }

        private static JsonSerializer CreateSerializer()
        {
            var serializer = JsonSerializer.Create(new JsonSerializerSettings()
            {
                Formatting = Formatting.Indented,
                StringEscapeHandling = StringEscapeHandling.EscapeNonAscii,
                FloatParseHandling = FloatParseHandling.Decimal,
            });
            serializer.Converters.Add(new MetricJsonConverter());
            return serializer;
        }

        private static Dictionary<MetricFile, MetricsContainer> CreateMetrics()
        {
            var dictionary = new Dictionary<MetricFile, MetricsContainer>();
            CreateEntry(MetricFile.SpriteFileMetric, true);
            CreateEntry(MetricFile.SpriteOffsetMetric, true);
            CreateEntry(MetricFile.MapReadMetric, true);
            return dictionary;

            MetricsContainer CreateEntry(MetricFile name, bool overwrite)
            {
                return dictionary[name] = new MetricsContainer(Enum.GetName(typeof(MetricFile), name), overwrite);
            }
        }
        private static string GetMetricsFilePath(string filename)
        {
            string exeDirectory = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            return exeDirectory + $"\\DispelTools_{filename}.json";
        }
    }
}
