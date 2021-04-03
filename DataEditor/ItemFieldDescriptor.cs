using DispelTools.Components.CustomPropertyGridControl;
using System.IO;
using System.Text;

namespace DispelTools.DataEditor
{
    public class ItemFieldDescriptor
    {
        public ItemFieldDescriptor(string name, bool readOnly, FieldType itemFieldDescriptorType)
        {
            Name = name;
            ReadOnly = readOnly;
            ItemFieldDescriptorType = itemFieldDescriptorType;
        }

        public string Name { get; set; }
        public string Description { get; set; }
        public bool ReadOnly { get; set; }
        public FieldType ItemFieldDescriptorType { get; set; }

        public static FieldType AsByte() => new ByteField();
        public static FieldType AsInt32() => new Int32Field();
        public static FieldType AsInt16() => new Int16Field();
        public static FieldType AsFixedString(int stringMaxLength, byte filler) => new FixedStringField(stringMaxLength, filler);
        public static FieldType AsByteArray(int length) => new ByteArrayField(length);
        public interface FieldType
        {
            Field.FieldType VisualFieldType { get; }
            object Read(BinaryReader reader);

            void Write(BinaryWriter writer, object value);
        }
        private class ByteField : FieldType
        {
            public Field.FieldType VisualFieldType => Field.FieldType.HEX;

            public object Read(BinaryReader reader) => reader.ReadByte();
            public void Write(BinaryWriter writer, object value) => writer.Write((byte)value);
        }
        private class ByteArrayField : FieldType
        {
            private readonly int length;

            public ByteArrayField(int length)
            {
                this.length = length;
            }

            public Field.FieldType VisualFieldType => Field.FieldType.ASCII;

            public object Read(BinaryReader reader)
            {
                var buffer = reader.ReadBytes(length);
                return Encoding.UTF8.GetString(buffer, 0, buffer.Length);
            }
            public void Write(BinaryWriter writer, object value)
            {
                var converted = Encoding.UTF8.GetBytes((string)value);
                writer.Write(converted);
            }
        }
        private class Int32Field : FieldType
        {
            public Field.FieldType VisualFieldType => Field.FieldType.DEC;

            public object Read(BinaryReader reader) => reader.ReadInt32();
            public void Write(BinaryWriter writer, object value) => writer.Write((int)value);
        }
        private class Int16Field : FieldType
        {
            public Field.FieldType VisualFieldType => Field.FieldType.DEC;

            public object Read(BinaryReader reader) => reader.ReadInt16();
            public void Write(BinaryWriter writer, object value) => writer.Write((short)value);
        }
        private class FixedStringField : FieldType
        {
            private readonly int stringMaxLength;
            private readonly byte filler;

            public FixedStringField(int stringMaxLength, byte filler)
            {
                this.stringMaxLength = stringMaxLength;
                this.filler = filler;
            }

            public Field.FieldType VisualFieldType => Field.FieldType.ASCII;

            public object Read(BinaryReader reader)
            {
                var sb = new StringBuilder();
                byte[] chars = reader.ReadBytes(stringMaxLength);
                for (int i = 0; i < stringMaxLength; i++)
                {
                    byte character = chars[i];
                    if (character == 0)
                    {
                        break;
                    }
                    sb.Append((char)character);
                }
                return sb.ToString();
            }

            public void Write(BinaryWriter writer, object value)
            {
                var str = (string)value;
                for (int i = 0; i < stringMaxLength; i++)
                {
                    if (str.Length > i)
                    {
                        writer.Write((byte)str[i]);
                    }
                    else if (str.Length == i)
                    {
                        writer.Write((byte)0);
                    }
                    else
                    {
                        writer.Write(filler);
                    }
                }
            }
        }
    }
}
