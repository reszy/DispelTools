using System.Collections.Generic;

namespace DispelTools.DataEditor
{
    public class FileDescriptorBuilder
    {
        private readonly List<ItemFieldDescriptor> fields = new List<ItemFieldDescriptor>();
        private int byteCounter = 0;

        public void Add(ItemFieldDescriptor.FieldType type, bool readOnly = false)
        {
            fields.Add(new ItemFieldDescriptor(GetUnknownName(), readOnly, type));
            byteCounter += type.ByteSize;
        }
        public void Add(ItemFieldDescriptor.FieldType type, string description, bool readOnly = false)
        {
            fields.Add(new ItemFieldDescriptor(GetUnknownName(), readOnly, type)
            {
                Description = description
            });
            byteCounter += type.ByteSize;
        }
        public void Add(string name, ItemFieldDescriptor.FieldType type, bool readOnly = false)
        {
            fields.Add(new ItemFieldDescriptor(name, readOnly, type));
            byteCounter += type.ByteSize;
        }
        public void Add(string name, ItemFieldDescriptor.FieldType type, string description, bool readOnly = false)
        {
            fields.Add(new ItemFieldDescriptor(name, readOnly, type)
            {
                Description = description
            });
            byteCounter += type.ByteSize;
        }

        public void Fill(int count, ItemFieldDescriptor.FieldType type)
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
