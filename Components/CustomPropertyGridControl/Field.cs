namespace DispelTools.Components.CustomPropertyGridControl
{
    public class Field
    {
        public Field() { }
        public Field(string name, object value, bool readOnly = false)
        {
            Name = name;
            ReadOnly = readOnly;
            if (value.GetType().IsPrimitive || value is string)
            {
                Value = value;
                DefaultValue = value;
                if (value is string)
                {
                    Type = FieldType.ASCII;
                }
                else if (value is byte)
                {
                    Type = FieldType.HEX;
                }
                else
                {
                    Type = FieldType.DEC;
                }

            }
            else
            {
                Value = value;
            }
        }
        public object Value { get; set; }
        public object DefaultValue { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool ReadOnly { get; set; }
        public FieldType Type { get; set; } = FieldType.ASCII;
        public enum FieldType { HEX, BIN, ASCII, DEC }
    }
}
