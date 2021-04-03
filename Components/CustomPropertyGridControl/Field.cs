using System;

namespace DispelTools.Components.CustomPropertyGridControl
{
    public class Field
    {
        private bool ready = false;
        private string str_value;
        private byte b_value;
        private short s_value;
        private int i_value;
        private object defaultValue;

        public Field() { }
        public Field(string name, object value, bool readOnly = false)
        {
            Name = name;
            ReadOnly = readOnly;
            if (value.GetType().IsPrimitive || value is string)
            {
                Value = value;
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
        public object Value
        {
            get => GetValue();
            set => SaveValue(value);
        }
        public object DefaultValue { get => defaultValue; private set { defaultValue = value; Prepare(); } }

        private void Prepare()
        {
            if (ready) { return; }
            if (IsShort = defaultValue is short)
            {
                s_value = (short)defaultValue;
            }
            if (IsByte = defaultValue is byte)
            {
                b_value = (byte)defaultValue;
            }
            if (IsInt = defaultValue is int)
            {
                i_value = (int)defaultValue;
            }
            if (defaultValue is string)
            {
                str_value = (string)defaultValue;
            }
            ready = true;
        }

        public string Name { get; set; }
        public string Description { get; set; }
        public bool ReadOnly { get; set; }
        public FieldType Type { get; set; } = FieldType.ASCII;
        public enum FieldType { HEX, BIN, ASCII, DEC }

        public bool IsShort { get; private set; }
        public bool IsByte { get; private set; }
        public bool IsInt { get; private set; }
        public decimal DecimalValue => new decimal(IsByte ? (byte)Value : (IsShort ? (short)Value : (int)Value));
        public decimal DecimalDefaultValue => new decimal(IsByte ? (byte)DefaultValue : (IsShort ? (short)DefaultValue : (int)DefaultValue));
        public int MaxValue => IsByte ? byte.MaxValue : (IsShort ? short.MaxValue : int.MaxValue);
        public int MinValue => IsByte ? byte.MinValue : (IsShort ? short.MinValue : int.MinValue);

        private void SaveValue(object value)
        {
            if(defaultValue == null)
            {
                DefaultValue = value;
            }
            if (IsShort)
            {
                s_value = Convert.ToInt16(value);
            }
            else if (IsByte)
            {
                b_value = Convert.ToByte(value);
            }
            else if (IsInt)
            {
                i_value = Convert.ToInt32(value);
            }
            else
            {
                str_value = (string)value;
            }
        }
        private object GetValue()
        {
            if (IsShort)
            {
                return s_value;
            }
            else if (IsByte)
            {
                return b_value;
            }
            else if (IsInt)
            {
                return i_value;
            }
            else
            {
                return str_value;
            }
        }
    }
}
