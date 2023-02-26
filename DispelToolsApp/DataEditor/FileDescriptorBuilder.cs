using DispelTools.DataEditor.Data;

namespace DispelTools.DataEditor
{
    public class FileDescriptorBuilder
    {
        private readonly List<ItemFieldDescriptor> fields = new List<ItemFieldDescriptor>();
        private int byteCounter = 0;
        public void Add(string name, ItemFieldDescriptor.IFieldTypeDescriptor type, Field.SupportedEncoding encoding, bool readOnly = false)
        {
            fields.Add(new ItemFieldDescriptor(name, readOnly, type)
            {
                Encoding = encoding,
                IsNameGenerated = false
            });
            byteCounter += type.ByteSize;
        }
        public void Add(ItemFieldDescriptor.IFieldTypeDescriptor type, Field.SupportedEncoding encoding, bool readOnly = false)
        {
            fields.Add(new ItemFieldDescriptor(GetUnknownName(), readOnly, type)
            {
                Encoding = encoding,
                IsNameGenerated = true
            });
            byteCounter += type.ByteSize;
        }
        public void Add(ItemFieldDescriptor.IFieldTypeDescriptor type, Field.SupportedEncoding encoding, string description, bool readOnly = false)
        {
            fields.Add(new ItemFieldDescriptor(GetUnknownName(), readOnly, type)
            {
                Description = description,
                Encoding = encoding,
                IsNameGenerated = true
            });
            byteCounter += type.ByteSize;
        }
        public void Add(ItemFieldDescriptor.IFieldTypeDescriptor type, bool readOnly = false)
        {
            fields.Add(new ItemFieldDescriptor(GetUnknownName(), readOnly, type)
            {
                Encoding = Field.SupportedEncoding.PL,
                IsNameGenerated = true
            });
            byteCounter += type.ByteSize;
        }
        public void Add(ItemFieldDescriptor.IFieldTypeDescriptor type, string description, bool readOnly = false)
        {
            fields.Add(new ItemFieldDescriptor(GetUnknownName(), readOnly, type)
            {
                Description = description,
                Encoding = Field.SupportedEncoding.PL,
                IsNameGenerated = true
            });
            byteCounter += type.ByteSize;
        }
        public void Add(string name, ItemFieldDescriptor.IFieldTypeDescriptor type, bool readOnly = false)
        {
            fields.Add(new ItemFieldDescriptor(name, readOnly, type) {
                Encoding = Field.SupportedEncoding.PL,
                IsNameGenerated = false
            });
            byteCounter += type.ByteSize;
        }
        public void Add(string name, ItemFieldDescriptor.IFieldTypeDescriptor type, string description, bool readOnly = false)
        {
            fields.Add(new ItemFieldDescriptor(name, readOnly, type)
            {
                Description = description,
                Encoding = Field.SupportedEncoding.PL,
                IsNameGenerated = false
            });
            byteCounter += type.ByteSize;
        }

        public void Fill(int count, ItemFieldDescriptor.IFieldTypeDescriptor type)
        {
            for (int i = 0; i < count; i++)
            {
                Add(type);
            }
        }

        public List<ItemFieldDescriptor> Build() => fields;

        private string GetUnknownName() => $"? (F:{fields.Count}) 0x{byteCounter.ToString("X").ToLower()}";
    }
}
