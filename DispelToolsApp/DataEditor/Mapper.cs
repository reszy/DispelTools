using DispelTools.Components.CustomPropertyGridControl;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;
using System.Linq;
using System.Windows.Forms;

namespace DispelTools.DataEditor
{
    public abstract partial class Mapper
    {
        private readonly IFileSystem fs;
        private readonly List<ItemFieldDescriptor> descriptorList;

        protected Mapper(IFileSystem fs)
        {
            this.fs = fs;
            descriptorList = CreateDescriptors();
        }
        protected Mapper() : this(new FileSystem())
        {
            descriptorList = CreateDescriptors();
        }

        public List<PropertyItem> ReadFile(string filename)
        {
            int elementStep = PropertyItemSize;
            var list = new List<PropertyItem>();
            using (var reader = new BinaryReader(fs.FileStream.Create(filename, FileMode.Open, FileAccess.Read)))
            {
                int expectedElements = 0;
                int spaceForElements = (int)Math.Floor((decimal)((reader.BaseStream.Length - CounterSize) / elementStep));
                if (CounterSize > 0)
                {
                    byte[] fullCount = new byte[] { 0, 0, 0, 0 };
                    var count = reader.ReadBytes(CounterSize);
                    count.CopyTo(fullCount, 0);
                    expectedElements = BitConverter.ToInt32(fullCount, 0);
                }
                else
                {
                    expectedElements = spaceForElements;
                }
                if (expectedElements != spaceForElements)
                {
                    MessageBox.Show($"In file count = {expectedElements}, counted {spaceForElements}");
                }
                for (int i = 0; i < spaceForElements; i++)
                {
                    list.Add(ReadElement(reader));
                }
            }
            return list;
        }
        public void SaveElement(PropertyItem element, int elementNumber, string filename)
        {
            using (var writer = new BinaryWriter(fs.FileStream.Create(filename, FileMode.Open, FileAccess.Write)))
            {
                writer.BaseStream.Position = elementNumber * PropertyItemSize + CounterSize;
                WriteElement(writer, element);
            }
        }

        public Mapping CreateMapping(params string[] fieldNames) => new Mapping(this, fieldNames);
        internal abstract int PropertyItemSize { get; }

        protected virtual byte CounterSize { get; } = 4;
        protected abstract List<ItemFieldDescriptor> CreateDescriptors();

        private PropertyItem ReadElement(BinaryReader reader)
        {
            var propertyItem = new PropertyItem();
            for (int i = 0; i < descriptorList.Count; i++)
            {
                var fieldDescriptor = descriptorList[i];
                object value = fieldDescriptor.ItemFieldDescriptorType.Read(reader);
                var field = new Field(fieldDescriptor.Name, value, fieldDescriptor.ItemFieldDescriptorType.VisualFieldType, fieldDescriptor.ReadOnly)
                {
                    Description = fieldDescriptor.Description
                };
                propertyItem.AddField(field);
            }
            return propertyItem;
        }
        private void WriteElement(BinaryWriter writer, PropertyItem propertyitem)
        {
            for (int i = 0; i < descriptorList.Count; i++)
            {
                var descriptor = descriptorList[i];
                var field = propertyitem[i];
                descriptor.ItemFieldDescriptorType.Write(writer, field.IsText ? field.ByteArrayValue : field.Value);
            }
        }

        public class Mapping
        {
            private int[] mapping;

            public Mapping(Mapper mapper, params string[] fieldNames)
            {
                mapping = fieldNames
                    .Select(name => mapper.descriptorList.Find(descriptor => descriptor.Name.Equals(name, StringComparison.OrdinalIgnoreCase)))
                    .Select(descriptor => mapper.descriptorList.IndexOf(descriptor))
                    .ToArray();
            }

            public object[] Convert(PropertyItem item)
            {
                return mapping
                    .Select(fieldId => item[fieldId].Value)
                    .ToArray();
            }
        }
    }
}
