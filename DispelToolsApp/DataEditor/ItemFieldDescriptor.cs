using DispelTools.DataEditor.Data;
using System.Collections.Generic;
using System.IO;

namespace DispelTools.DataEditor
{
    public struct ItemFieldDescriptor
    {
        public ItemFieldDescriptor(string name, bool readOnly, IFieldType itemFieldDescriptorType)
        {
            Name = name;
            ReadOnly = readOnly;
            ItemFieldDescriptorType = itemFieldDescriptorType;
        }

        public string Name { get; set; }
        public string? Description { get; init; }
        public bool ReadOnly { get; set; }
        public IFieldType ItemFieldDescriptorType { get; set; }

        public static IFieldType AsByte() => new ByteField();
        public static IFieldType AsInt32() => new Int32Field();
        public static IFieldType AsInt16() => new Int16Field();
        public static IFieldType AsFixedString(int stringMaxLength, byte filler) => AsFixedString(stringMaxLength, filler, Field.DisplayType.TEXT_PL);
        public static IFieldType AsFixedString(int stringMaxLength, byte filler, Field.DisplayType encoding) => new FixedStringField(stringMaxLength, filler, encoding);
        public static IFieldType AsByteArray(int length) => new ByteArrayField(length);
        public interface IFieldType
        {
            Field.DisplayType VisualFieldType { get; }

            int ByteSize { get; }
            object Read(BinaryReader reader);

            void Write(BinaryWriter writer, object value);
        }
        private class ByteField : IFieldType
        {
            public Field.DisplayType VisualFieldType => Field.DisplayType.HEX;
            public int ByteSize => 1;

            public object Read(BinaryReader reader) => reader.ReadByte();
            public void Write(BinaryWriter writer, object value) => writer.Write((byte)value);
        }
        private class ByteArrayField : IFieldType
        {
            private readonly int length;

            public ByteArrayField(int length)
            {
                this.length = length;
            }

            public Field.DisplayType VisualFieldType => Field.DisplayType.ASCII;
            public int ByteSize => length;

            public object Read(BinaryReader reader) => reader.ReadBytes(length);
            public void Write(BinaryWriter writer, object value) => writer.Write((byte[])value);
        }
        private class Int32Field : IFieldType
        {
            public Field.DisplayType VisualFieldType => Field.DisplayType.DEC;
            public int ByteSize => 4;

            public object Read(BinaryReader reader) => reader.ReadInt32();
            public void Write(BinaryWriter writer, object value) => writer.Write((int)value);
        }
        private class Int16Field : IFieldType
        {
            public Field.DisplayType VisualFieldType => Field.DisplayType.DEC;
            public int ByteSize => 2;

            public object Read(BinaryReader reader) => reader.ReadInt16();
            public void Write(BinaryWriter writer, object value) => writer.Write((short)value);
        }
        private class FixedStringField : IFieldType
        {
            private readonly int stringMaxLength;
            private readonly byte filler;
            private readonly Field.DisplayType encoding;

            public FixedStringField(int stringMaxLength, byte filler, Field.DisplayType encoding)
            {
                this.stringMaxLength = stringMaxLength;
                this.filler = filler;
                if (encoding == Field.DisplayType.ASCII || encoding == Field.DisplayType.TEXT_KOR || encoding == Field.DisplayType.TEXT_PL)
                {
                    this.encoding = encoding;
                }
                else
                {
                    throw new System.ArgumentException($"Argument {encoding} is not encoding");
                }
            }

            public Field.DisplayType VisualFieldType => encoding;
            public int ByteSize => stringMaxLength;

            public object Read(BinaryReader reader)
            {
                var bList = new List<byte>();
                byte[] bytes = reader.ReadBytes(stringMaxLength);
                for (int i = 0; i < stringMaxLength; i++)
                {
                    if (bytes[i] == 0)
                    {
                        break;
                    }
                    bList.Add(bytes[i]);
                }
                return bList.ToArray();
            }

            public void Write(BinaryWriter writer, object value)
            {
                byte[] bytes = (byte[])value;
                int countermax = System.Math.Min(stringMaxLength - 1, bytes.Length);
                for (int i = 0; i < countermax; i++)
                {
                    writer.Write(bytes[i]);
                }
                writer.Write((byte)0);
                for (int i = countermax; i < stringMaxLength - 1; i++)
                {
                    writer.Write(filler);
                }
            }
        }
    }
}
