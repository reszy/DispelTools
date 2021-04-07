using System;
using System.Linq;
using System.Text;

namespace DispelTools.Components.CustomPropertyGridControl
{
    public class Field
    {
        private byte b_value;
        private short s_value;
        private int i_value;
        private byte[] raw_value;
        private readonly object defaultValue;
        private static readonly Encoding asciiEncoding = Encoding.GetEncoding(437);
        private static readonly Encoding plEncoding = Encoding.GetEncoding(1250);
        private static readonly Encoding korEncoding = Encoding.GetEncoding(51949);

        private bool IsShort = false;
        private bool IsByte = false;
        private bool IsInt = false;
        private bool IsRaw = false;

        public object Value => GetValue();
        public object DefaultValue => defaultValue;
        public string Name { get; set; }
        public string Description { get; set; }
        public bool ReadOnly { get; set; }
        public DisplayType Type { get; set; } = DisplayType.ASCII;
        public enum DisplayType
        {
            HEX, BIN, DEC,
            ASCII, TEXT_KOR, TEXT_PL
        }

        public enum SupportedValues
        {
            SHORT, BYTE, INT, RAW
        }

        public SupportedValues ValueType { get; private set; }
        public bool IsText => Type == DisplayType.ASCII || Type == DisplayType.TEXT_PL || Type == DisplayType.TEXT_KOR;
        public decimal DecimalValue => new decimal(IsByte ? (byte)Value : (IsShort ? (short)Value : (int)Value));
        public int MaxValue => IsByte ? byte.MaxValue : (IsShort ? short.MaxValue : int.MaxValue);
        public int MinValue => IsByte ? byte.MinValue : (IsShort ? short.MinValue : int.MinValue);

        public byte[] ByteArrayValue => raw_value;
        public string EncodedText => GetEncoding().GetString(raw_value);
        public Encoding GetEncoding() => Type == DisplayType.TEXT_KOR ? korEncoding : (Type == DisplayType.TEXT_PL ? plEncoding : asciiEncoding);
        public bool HasChanged => !IsDefaultEqualToValue();

        public Field(string name, object value, DisplayType type = DisplayType.ASCII, bool readOnly = false)
        {
            Name = name;
            ReadOnly = readOnly;
            Type = type;
            IsRaw = value is byte[] || value is string;
            if (value is string)
            {
                raw_value = GetEncoding().GetBytes((string)value);
                ValueType = SupportedValues.RAW;
                defaultValue = raw_value;
            }
            else
            {
                defaultValue = value;
            }
            if (IsShort = value is short)
            {
                s_value = (short)value;
                ValueType = SupportedValues.SHORT;
            }
            if (IsByte = value is byte)
            {
                b_value = (byte)value;
                ValueType = SupportedValues.BYTE;
            }
            if (IsInt = value is int)
            {
                i_value = (int)value;
                ValueType = SupportedValues.INT;
            }
            if (value is byte[])
            {
                raw_value = (byte[])value;
                ValueType = SupportedValues.RAW;
            }
            if (IsText ^ IsRaw)
            {
                throw new ArgumentException("Value has incompatible displayType");
            }
        }
        public void RevertValue() {
            switch (ValueType)
            {
                case SupportedValues.SHORT:
                    s_value = (short)defaultValue;
                    break;
                case SupportedValues.BYTE:
                    b_value = (byte)defaultValue;
                    break;
                case SupportedValues.INT:
                    i_value = (int)defaultValue;
                    break;
                case SupportedValues.RAW:
                    raw_value = (byte[])defaultValue;
                    break;
                default:
                    break;
            }
        }

        public void SetValue(byte[] value)
        {
            if (!IsRaw) { throw new ArgumentException(); }
            raw_value = value;
        }
        public void SetValue(string value)
        {
            if (!IsRaw) { throw new ArgumentException(); }
            raw_value = GetEncoding().GetBytes(value);
        }
        public void SetValue(byte value)
        {
            if (!IsByte) { throw new ArgumentException(); }
            b_value = value;
        }
        public void SetValue(short value)
        {
            if (!IsShort) { throw new ArgumentException(); }
            s_value = value;
        }
        public void SetValue(int value)
        {
            if (!IsInt) { throw new ArgumentException(); }
            i_value = value;
        }
        public void SetValue(decimal value)
        {
            if (IsText) { throw new ArgumentException(); }
            switch (ValueType)
            {
                case SupportedValues.SHORT:
                    s_value = Convert.ToInt16(value);
                    break;
                case SupportedValues.BYTE:
                    b_value = Convert.ToByte(value);
                    break;
                case SupportedValues.INT:
                    i_value = Convert.ToInt32(value);
                    break;
                default:
                    break;
            }
        }
        private object GetValue()
        {
            switch (ValueType)
            {
                case SupportedValues.SHORT:
                    return s_value;
                case SupportedValues.BYTE:
                    return b_value;
                case SupportedValues.INT:
                    return i_value;
                case SupportedValues.RAW:
                    return raw_value;
                default:
                    return null;
            }
        }

        private bool IsDefaultEqualToValue()
        {
            switch (ValueType)
            {
                case SupportedValues.SHORT:
                    return (short)defaultValue == s_value;
                case SupportedValues.BYTE:
                    return (byte)defaultValue == b_value;
                case SupportedValues.INT:
                    return (int)defaultValue == i_value;
                case SupportedValues.RAW:
                    return Enumerable.SequenceEqual((byte[])defaultValue, raw_value);
                default:
                    throw new NotImplementedException();
            }
        }
    }
}
