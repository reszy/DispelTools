using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace DispelTools.DebugTools.MetricTools
{
    internal class MetricJsonConverter : JsonConverter<Metric>
    {
        public override void WriteJson(JsonWriter writer, Metric value, JsonSerializer serializer)
        {
            if (value.MetricType == MetricType.COUNT)
            {
                writer.WriteStartObject();
                foreach (var entry in value.CountValues)
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
                    if (reader.TokenType == JsonToken.EndObject)
                    {
                        finished = true; break;
                    }
                    string key = (string)reader.Value;
                    long value = long.Parse(reader.ReadAsString());
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
