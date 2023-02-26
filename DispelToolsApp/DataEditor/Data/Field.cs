using System.Text;
using static DispelTools.DataEditor.Data.Field;

namespace DispelTools.DataEditor.Data
{
    public interface IField
    {
        public bool IsReference { get; }
        public bool IsText { get; }

        public string Name { get; }
        public object Value { get; set; }
        public string? Description { get; init; }
        public bool IsNameGenerated { get; init; }

        public bool ReadOnly { get; init; }
        public bool HasChanged { get; }

        public DisplayType DisplayType { get; }

        public string GetDecodedText();
        public byte[] GetEncodedText();
        public void RevertValue();
    }
    public interface IPrimitiveField
    {
        int MaxValue { get; }
        int MinValue { get; }
    }
    public class PrimitiveField<T> : IField, IPrimitiveField where T : struct
    {
        private T _value;
        private readonly T defaultValue;
        public bool IsReference { get; }
        public bool IsText => false;
        public string Name { get; }
        public object Value { get => _value; set => SetValue(value); }
        public T TValue { get => _value; set => SetValue(value); }
        public string? Description { get; init; }
        public bool IsNameGenerated { get; init; }
        public bool ReadOnly { get; init; }
        public bool HasChanged => !_value.Equals(defaultValue);
        public DisplayType DisplayType { get; }

        public int MaxValue { get; }
        public int MinValue { get; }

        public PrimitiveField(string name, T value, DisplayType displayType)
        {
            if (value is byte[]) throw new ArgumentException("Expected value as number", nameof(value));
            Name = name;
            defaultValue = _value = value;
            DisplayType = displayType;
            if (value is int)
            {
                MaxValue = int.MaxValue;
                MinValue = int.MinValue;
            }
            if (value is short)
            {
                MaxValue = short.MaxValue;
                MinValue = short.MinValue;
            }
            if (value is byte)
            {
                MaxValue = byte.MaxValue;
                MinValue = byte.MinValue;
            }
        }
        public string GetDecodedText() => string.Empty;
        public byte[] GetEncodedText() => Array.Empty<byte>();

        public void RevertValue()
        {
            _value = defaultValue;
        }

        private void SetValue(object value)
        {
            if (value is not T) throw new ArgumentException($"Expected value as {typeof(T).Name}, was {value.GetType().Name}", nameof(value));
            _value = (T)value;
        }
    }

    public class ByteArrayField : IField
    {
        private readonly byte[] defaultValue;
        private byte[] _value;

        public bool IsReference { get; }
        public bool IsText => false;


        public string Name { get; }
        public object Value { get => _value.ToArray(); set => SetValue(value); }
        public byte[] Bytes { get => _value.ToArray(); set => SetValue(value); }
        public string? Description { get; init; }
        public bool IsNameGenerated { get; init; }

        public bool ReadOnly { get; init; }
        public bool HasChanged => ((ReadOnlySpan<byte>)_value).SequenceEqual(defaultValue);

        public DisplayType DisplayType { get; }

        public ByteArrayField(string name, byte[] value, DisplayType displayType)
        {
            Name = name;
            defaultValue = _value = value;
            DisplayType = displayType;
        }

        public string GetDecodedText() => $"0x{Convert.ToHexString(_value)}";

        public byte[] GetEncodedText() => _value.ToArray();

        public void RevertValue() => _value = defaultValue.ToArray();

        private void SetValue(object value)
        {
            if (value is byte[] byteArray) { _value = byteArray.ToArray(); return; }
            if (value is string str) { _value = Convert.FromHexString(str.Replace("0x", "")); return; }
            throw new ArgumentException("Unsupported type", nameof(value));
        }
    }

    public class TextField : IField
    {
        private readonly byte[] defaultValue;
        private byte[] _value;
        private string decodedValue;
        private readonly Encoding encoding;

        public bool IsReference { get; }
        public bool IsText => true;

        public string Name { get; }
        public object Value { get => _value.ToArray(); set => SetValue(value); }
        public string? Description { get; init; }
        public bool IsNameGenerated { get; init; }

        public bool ReadOnly { get; init; }
        public bool HasChanged => ((ReadOnlySpan<byte>)_value).SequenceEqual(defaultValue);

        public DisplayType DisplayType => DisplayType.ASCII;

        public TextField(string name, string value, SupportedEncoding encoding)
        {
            this.encoding = GetEncoding(encoding);
            Name = name;
            defaultValue = _value = this.encoding.GetBytes(value);
            decodedValue = value;
        }

        public TextField(string name, byte[] value, SupportedEncoding encoding)
        {
            this.encoding = GetEncoding(encoding);
            Name = name;
            defaultValue = _value = value;
            decodedValue = this.encoding.GetString(value);
        }
        public static Encoding GetEncoding(SupportedEncoding encoding) => encoding == SupportedEncoding.KOR ? korEncoding : (encoding == SupportedEncoding.PL ? plEncoding : asciiEncoding);

        public string GetDecodedText() => decodedValue;

        public byte[] GetEncodedText() => _value.ToArray();

        private void SetValue(object value)
        {
            if (value is byte[] byteArray) { SetValue(byteArray); return; }
            if (value is string str) { SetValue(encoding.GetBytes(str)); return; }
            throw new ArgumentException("Unsupported type", nameof(value));
        }

        public void RevertValue()
        {
            SetValue(defaultValue);
        }

        private void SetValue(byte[] value)
        {
            _value = value.ToArray();
            decodedValue = encoding.GetString(value);
        }
    }

    public static class Field
    {
        internal static readonly Encoding asciiEncoding = Encoding.GetEncoding(437);
        internal static readonly Encoding plEncoding = Encoding.GetEncoding(1250);
        internal static readonly Encoding korEncoding = Encoding.GetEncoding(51949);

        public enum DisplayType
        {
            HEX_STRING = 1, 
            HEX = HEX_STRING,
            BIN = 2,
            DEC = 3,
            STRING = 4, 
            ASCII = STRING, TEXT_KOR = STRING, TEXT_PL = STRING
        }

        public enum SupportedEncoding
        {
            KOR, PL, ASCII//, UTF8
        }
    }
}
