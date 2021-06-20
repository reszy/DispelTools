using DispelTools.Common;
using DispelTools.DebugTools.Metrics.Dto;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace DispelTools.DebugTools.Metrics
{
    public static class FileMetrics
    {
        public static bool Enabled { get; set; } = false;
        private static Dictionary<string, Metric> metrics = new Dictionary<string, Metric>();
        private static string GetMetricsFilePath()
        {
            string exeDirectory = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            return exeDirectory + "\\DispelTools_FileMetrics.json";
        }

        public static void AddMetric(IMetricDto metricDto)
        {
            if (Enabled)
            {
                if (metrics.TryGetValue(metricDto.MetricName, out var metric))
                {
                    metric.AddMetric(metricDto);
                }
                else
                {
                    metrics[metricDto.MetricName] = new Metric(metricDto);
                }
            }
        }

        public static void DumpMetrics(object sender, EventArgs args)
        {
            if (Enabled)
            {
                var serializer = JsonSerializer.Create(new JsonSerializerSettings()
                {
                    Formatting = Formatting.Indented,
                    StringEscapeHandling = StringEscapeHandling.EscapeNonAscii,
                    FloatParseHandling = FloatParseHandling.Decimal,
                });
                serializer.Converters.Add(new MetricConverter());
                string metricsFilename = GetMetricsFilePath();
                if (Settings.FS.File.Exists(metricsFilename))
                {
                    using (var reader = Settings.FS.File.OpenText(metricsFilename))
                    {
                        var metricsLoaded = serializer.Deserialize(reader, typeof(Dictionary<string, Metric>)) as Dictionary<string, Metric>;
                        if (metricsLoaded != null)
                        {
                            Merge(metricsLoaded);
                        }
                    }
                }
                using (var writer = new StreamWriter(Settings.FS.File.Create(metricsFilename)))
                {
                    serializer.Serialize(writer, metrics);
                }
                metrics.Clear();
            }
        }

        private static void Merge(Dictionary<string, Metric> metricsLoaded)
        {
            foreach (var newerMetric in metrics)
            {
                if (metricsLoaded.TryGetValue(newerMetric.Key, out var olderMetric))
                {
                    olderMetric.Merge(newerMetric.Value);
                }
                else
                {
                    metricsLoaded[newerMetric.Key] = newerMetric.Value;
                }
            }
            metrics = metricsLoaded;
        }

        internal class MetricConverter : JsonConverter<Metric>
        {
            public override void WriteJson(JsonWriter writer, Metric value, JsonSerializer serializer)
            {
                if (value.MetricType == MetricType.COUNT)
                {
                    writer.WriteStartObject();
                    foreach(var entry in value.CountValues)
                    {
                        writer.WritePropertyName(entry.Key);
                        writer.WriteValue(entry.Value);
                    }
                    writer.WriteEndObject();
                }
                else
                {
                    writer.WriteStartArray();
                    switch (value.MetricType)
                    {
                        case MetricType.NUMBER:
                            value.NumberValues.ForEach(v => writer.WriteValue(v));
                            break;
                        case MetricType.STRING:
                            value.StringValues.ForEach(v => writer.WriteValue(v));
                            break;
                    }
                    writer.WriteEndArray();
                }
            }

            public override Metric ReadJson(JsonReader reader, Type objectType, Metric existingValue, bool hasExistingValue, JsonSerializer serializer)
            {
                if (reader.TokenType == JsonToken.StartObject)
                {
                    var values = new Dictionary<string, long>();
                    bool finished = false;
                    while (!finished)
                    {
                        reader.Read();
                        if (reader.TokenType == JsonToken.EndObject) {finished = true; break;
                    }
                        var key = (string)reader.Value;
                        var value = long.Parse(reader.ReadAsString());
                        values[key] = value;
                    }
                    return new Metric(values);
                }
                else
                {
                    reader.Read();
                    var type = reader.ValueType;
                    if (type == typeof(decimal))
                    {
                        var values = new List<decimal>();
                        do
                        {
                            values.Add((decimal)reader.Value);
                        } while (reader.ReadAsDecimal() != null);
                        return new Metric(values);
                    }
                    else
                    {
                        var values = new List<string>();
                        do
                        {
                            values.Add((string)reader.Value);
                        } while (reader.ReadAsString() != null);
                        return new Metric(values);
                    }
                }
            }
        }
    }
}
