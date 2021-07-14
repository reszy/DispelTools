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

        protected Mapper(IFileSystem fs)
        {
            this.fs = fs;
        }
        protected Mapper() : this(new FileSystem())
        {
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

        protected abstract List<ItemFieldDescriptor> FileDescriptor { get; }

        protected virtual bool HaveCounterOnBeginning { get; } = true;

        private int GetSkipBytesCount() => HaveCounterOnBeginning ? 4 : 0;

        private PropertyItem ReadElement(BinaryReader reader)
        {
            var propertyItem = new PropertyItem();
            for (int i = 0; i < FileDescriptor.Count; i++)
            {
                var fieldDescriptor = FileDescriptor[i];
                if (fieldDescriptor.Name == "?")
                {
                    fieldDescriptor.Name = GetHexRelativePosition(reader);
                }
                object value = fieldDescriptor.ItemFieldDescriptorType.Read(reader);
                var field = new Field(fieldDescriptor.Name, value, fieldDescriptor.ItemFieldDescriptorType.VisualFieldType, fieldDescriptor.ReadOnly);
                propertyItem.AddField(field);
            }
            return propertyItem;
        }

        private string GetHexRelativePosition(BinaryReader reader)
        {
            decimal relativePosition = reader.BaseStream.Position - Math.Floor((decimal)(reader.BaseStream.Position / PropertyItemSize));
            return "? 0x" + ((int)relativePosition).ToString("X").ToLower();
        }
        private void WriteElement(BinaryWriter writer, PropertyItem propertyitem)
        {
            for (int i = 0; i < FileDescriptor.Count; i++)
            {
                var descriptor = FileDescriptor[i];
                var field = propertyitem[i];
                descriptor.ItemFieldDescriptorType.Write(writer, field.IsText ? field.ByteArrayValue : field.Value);
            }
        }

        protected ItemFieldDescriptor createDescriptor(ItemFieldDescriptor.FieldType type, bool readOnly = false) => new ItemFieldDescriptor("?", readOnly, type);
        protected ItemFieldDescriptor createDescriptor(ItemFieldDescriptor.FieldType type, string description, bool readOnly = false)
        {
            return new ItemFieldDescriptor("?", readOnly, type)
            {

                Description = description
            };
        }
        protected ItemFieldDescriptor createDescriptor(string name, ItemFieldDescriptor.FieldType type, bool readOnly = false) => new ItemFieldDescriptor(name, readOnly, type);
        protected ItemFieldDescriptor createDescriptor(string name, ItemFieldDescriptor.FieldType type, string description, bool readOnly = false)
        {
            return new ItemFieldDescriptor(name, readOnly, type)
            {
                Description = description
            };
        }

        public static void FillWithUnknownBytes(int count, List<ItemFieldDescriptor> list)
        {
            for(int i = 0; i< count;i++)
            {
                list.Add(new ItemFieldDescriptor("?", false, ItemFieldDescriptor.AsByte()));
            }
        }
        public static void FillWithUnknownInts(int count, List<ItemFieldDescriptor> list)
        {
            for(int i = 0; i< count;i++)
            {
                list.Add(new ItemFieldDescriptor("?", false, ItemFieldDescriptor.AsInt32()));
            }
        }
    }
}
