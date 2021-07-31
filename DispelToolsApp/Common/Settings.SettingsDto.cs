using System;
using System.Collections.Generic;
using System.Reflection;

namespace DispelTools.Common
{
    public static partial class Settings
    {
        [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
        private class DebugIgnoreAttribute : Attribute { }
        private class SettingsDto
        {
            private static readonly SettingsDto defaultSettings = Default();
            public string GameRootDir { get; set; }

            public string OutRootDir { get; set; }

            [DebugIgnore]
            public bool DebugFileMetrics { get; set; }
            [DebugIgnore]
            public bool DebugReadOnlyExtractor { get; set; }

            public static SettingsDto Default()
            {
                return new SettingsDto()
                {
                    GameRootDir = "",
                    OutRootDir = "",
                    DebugFileMetrics = false,
                    DebugReadOnlyExtractor = false,
                };
            }

            public void ParseAndSet(string propertyName, object value)
            {
                var property = GetType().GetProperty(propertyName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                if (property != null)
                {
                    property.SetValue(this, Convert.ChangeType(value, property.PropertyType));
                }
            }

            public Dictionary<string, object> SerializeToMap()
            {
                var map = new Dictionary<string, object>();
                var properties = typeof(SettingsDto).GetProperties();
                foreach (var property in properties)
                {
                    if (!Attribute.IsDefined(property, typeof(DebugIgnoreAttribute)) || !property.GetValue(this).Equals(property.GetValue(defaultSettings)))
                    {
                        map[property.Name] = property.GetValue(this);
                    }
                }
                return map;
            }
        }
    }
}
