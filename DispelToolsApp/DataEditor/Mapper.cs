using DispelTools.Components.CustomPropertyGridControl;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;
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
                int spaceForElements = (int)Math.Floor((decimal)((reader.BaseStream.Length - GetSkipBytesCount()) / elementStep));
                if (HaveCounterOnBeginning)
                {
                    expectedElements = reader.ReadInt32();
                }
                else
                {
                    expectedElements = spaceForElements;
                }
                if (expectedElements != spaceForElements)
                {
                    MessageBox.Show($"In file count = {expectedElements}, counted {spaceForElements}");
                }
                for (int i = 0; i < expectedElements; i++)
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
                writer.BaseStream.Position = elementNumber * PropertyItemSize + GetSkipBytesCount();
                WriteElement(writer, element);
            }
        }
        protected abstract int PropertyItemSize { get; }

        protected virtual bool HaveCounterOnBeginning { get; } = true;
        protected abstract List<ItemFieldDescriptor> CreateDescriptors();

        private int GetSkipBytesCount() => HaveCounterOnBeginning ? 4 : 0;

        private PropertyItem ReadElement(BinaryReader reader)
        {
            var propertyItem = new PropertyItem();
            for (int i = 0; i < descriptorList.Count; i++)
            {
                var fieldDescriptor = descriptorList[i];
                object value = fieldDescriptor.ItemFieldDescriptorType.Read(reader);
                var field = new Field(fieldDescriptor.Name, value, fieldDescriptor.ItemFieldDescriptorType.VisualFieldType, fieldDescriptor.ReadOnly);
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
    }
}
