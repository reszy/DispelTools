using DispelTools.Common;
using DispelTools.Components.CustomPropertyGridControl;
using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.IO;

namespace DispelTools.DataEditor
{
    public abstract partial class Mapper
    {
        public List<PropertyItem> ReadFile(string filename)
        {
            int elementStep = PropertyItemSize;
            var list = new List<PropertyItem>();
            using (var reader = new BinaryReader(new FileStream(filename, FileMode.Open, FileAccess.Read)))
            {
                var expectedElements = reader.ReadInt32();
                int spaceForElements = (int)Math.Floor((decimal)((reader.BaseStream.Length - 4) / elementStep));
                if(expectedElements != spaceForElements)
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
            using (var writer = new BinaryWriter(new FileStream(filename, FileMode.Open, FileAccess.Write)))
            {
                writer.BaseStream.Position = elementNumber * PropertyItemSize;
                WriteElement(writer, element);
            }
        }
        protected abstract int PropertyItemSize { get; }

        protected abstract List<ItemFieldDescriptor> FileDescriptor { get; }

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
                var field = new Field()
                {
                    Name = fieldDescriptor.Name,
                    Value = value,
                    ReadOnly = fieldDescriptor.ReadOnly,
                    Type = fieldDescriptor.ItemFieldDescriptorType.VisualFieldType
                };
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
                descriptor.ItemFieldDescriptorType.Write(writer, field.Value);
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
    }
}
