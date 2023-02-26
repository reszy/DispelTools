using DispelTools.DataEditor.Data;

namespace DispelTools.DataEditor
{
    public struct ItemFieldDescriptor
    {
        public ItemFieldDescriptor(string name, bool readOnly, IFieldTypeDescriptor itemFieldDescriptorType)
        {
            Name = name;
            ReadOnly = readOnly;
            ItemFieldDescriptorType = itemFieldDescriptorType;
        }

        public string Name { get; set; }
        public string? Description { get; init; }
        public bool ReadOnly { get; set; }
        public Field.SupportedEncoding Encoding { get; init; }
        public IFieldTypeDescriptor ItemFieldDescriptorType { get; set; }
        public bool IsNameGenerated { get; init; }
        public Type FieldClass => ItemFieldDescriptorType.FieldClass;

        public static IFieldTypeDescriptor AsByte() => new ByteFieldDescriptor();
        public static IFieldTypeDescriptor AsInt32() => new Int32FieldDescriptor();
        public static IFieldTypeDescriptor AsInt16() => new Int16FieldDescriptor();
        public static IFieldTypeDescriptor AsFixedString(int stringMaxLength, byte filler) => new FixedStringFieldDescriptor(stringMaxLength, filler);
        public static IFieldTypeDescriptor AsByteArray(int length) => new ByteArrayFieldDescriptor(length);
        public interface IFieldTypeDescriptor
        {
            Type FieldClass { get; }
            Field.DisplayType VisualFieldType { get; }

            int ByteSize { get; }

            object Read(BinaryReader reader);
            void Write(BinaryWriter writer, IField field);
        }
        private class ByteFieldDescriptor : IFieldTypeDescriptor
        {
            public Type FieldClass => typeof(PrimitiveField<byte>);
            public Field.DisplayType VisualFieldType => Field.DisplayType.HEX;
            public int ByteSize => 1;

            public object Read(BinaryReader reader) => reader.ReadByte();
            public void Write(BinaryWriter writer, IField field)
            {
                if (field is PrimitiveField<byte> byteField)
                    writer.Write(byteField.TValue);
                else
                    throw new ArgumentException("Wrong type", nameof(field));
            }
        }
        private class ByteArrayFieldDescriptor : IFieldTypeDescriptor
        {
            private readonly int length;

            public ByteArrayFieldDescriptor(int length)
            {
                this.length = length;
            }
            public Type FieldClass => typeof(Data.ByteArrayField);
            public Field.DisplayType VisualFieldType => Field.DisplayType.ASCII;
            public int ByteSize => length;

            public object Read(BinaryReader reader) => reader.ReadBytes(length);
            public void Write(BinaryWriter writer, IField field)
            {
                if (field is ByteArrayField byteArrayField)
                    writer.Write(byteArrayField.Bytes);
                else
                    throw new ArgumentException("Wrong type", nameof(field));
            }
        }
        private class Int32FieldDescriptor : IFieldTypeDescriptor
        {
            public Type FieldClass => typeof(PrimitiveField<int>);
            public Field.DisplayType VisualFieldType => Field.DisplayType.DEC;
            public int ByteSize => 4;

            public object Read(BinaryReader reader) => reader.ReadInt32();
            public void Write(BinaryWriter writer, IField field)
            {
                if (field is PrimitiveField<int> byteField)
                    writer.Write(byteField.TValue);
                else
                    throw new ArgumentException("Wrong type", nameof(field));
            }
        }
        private class Int16FieldDescriptor : IFieldTypeDescriptor
        {
            public Type FieldClass => typeof(PrimitiveField<short>);
            public Field.DisplayType VisualFieldType => Field.DisplayType.DEC;
            public int ByteSize => 2;

            public object Read(BinaryReader reader) => reader.ReadInt16();
            public void Write(BinaryWriter writer, IField field)
            {
                if (field is PrimitiveField<short> byteField)
                    writer.Write(byteField.TValue);
                else
                    throw new ArgumentException("Wrong type", nameof(field));
            }
        }
        private class FixedStringFieldDescriptor : IFieldTypeDescriptor
        {
            private readonly int stringMaxLength;
            private readonly byte filler;

            public FixedStringFieldDescriptor(int stringMaxLength, byte filler)
            {
                this.stringMaxLength = stringMaxLength;
                this.filler = filler;
            }
            public Type FieldClass => typeof(TextField);
            public Field.DisplayType VisualFieldType => Field.DisplayType.ASCII;
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

            public void Write(BinaryWriter writer, IField field)
            {
                byte[] bytes = field.GetEncodedText();
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
