using DispelTools.Components.CustomPropertyGridControl;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DispelTools.DataEditor
{
    public abstract class Mapper
    {
        private readonly int stringMaxLength = 260;
        public List<PropertyItem> ReadFile(string filename)
        {
            int elementStep = PropertyItemSize;
            var list = new List<PropertyItem>();
            using (var reader = new BinaryReader(new FileStream(filename, FileMode.Open, FileAccess.Read)))
            {
                int expetedElements = (int)Math.Floor((decimal)(reader.BaseStream.Length / elementStep));
                for (int i = 0; i < expetedElements; i++)
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

        public class ItemFieldDescriptor
        {
            public ItemFieldDescriptor(string name, bool readOnly, Type itemFieldDescriptorType)
            {
                Name = name;
                ReadOnly = readOnly;
                ItemFieldDescriptorType = itemFieldDescriptorType;
            }

            public string Name { get; set; }
            public string Description { get; set; }
            public bool ReadOnly { get; set; }
            public Type ItemFieldDescriptorType { get; set; }
            public enum Type { BYTE, INT32, STRING }
        }

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
                object value = ReadFieldValue(reader, fieldDescriptor.ItemFieldDescriptorType);
                var field = new Field()
                {
                    Name = fieldDescriptor.Name,
                    Value = value,
                    DefaultValue = value,
                    ReadOnly = fieldDescriptor.ReadOnly,
                    Type = MapType(fieldDescriptor.ItemFieldDescriptorType)
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

        private object ReadFieldValue(BinaryReader reader, ItemFieldDescriptor.Type type)
        {
            switch (type)
            {
                case ItemFieldDescriptor.Type.BYTE:
                    return reader.ReadByte();
                case ItemFieldDescriptor.Type.INT32:
                    return reader.ReadInt32(); ;
                case ItemFieldDescriptor.Type.STRING:
                    return ReadString(reader);
                default:
                    return null;
            }
        }

        private Field.FieldType MapType(ItemFieldDescriptor.Type type)
        {
            switch (type)
            {
                case ItemFieldDescriptor.Type.BYTE:
                    return Field.FieldType.HEX;
                case ItemFieldDescriptor.Type.INT32:
                    return Field.FieldType.DEC;
                case ItemFieldDescriptor.Type.STRING:
                    return Field.FieldType.ASCII;
                default:
                    return Field.FieldType.ASCII;
            }
        }

        protected string ReadString(BinaryReader reader)
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

        protected void WriteString(BinaryWriter writer, string str)
        {
            byte filler = 0xCD;
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
        private void WriteElement(BinaryWriter writer, PropertyItem propertyitem)
        {
            for (int i = 0; i < FileDescriptor.Count; i++)
            {
                var descriptor = FileDescriptor[i];
                var field = propertyitem[i];
                WriteFieldValue(writer, descriptor.ItemFieldDescriptorType, field.Value);
            }
        }

        private void WriteFieldValue(BinaryWriter writer, ItemFieldDescriptor.Type type, object value)
        {
            switch (type)
            {
                case ItemFieldDescriptor.Type.BYTE:
                    writer.Write((byte)value); break;
                case ItemFieldDescriptor.Type.INT32:
                    writer.Write((int)value); break;
                case ItemFieldDescriptor.Type.STRING:
                    WriteString(writer, (string)value); break;
                default:
                    break;
            }
        }

        protected ItemFieldDescriptor createDescriptor(ItemFieldDescriptor.Type type, bool readOnly = false) => new ItemFieldDescriptor("?", readOnly, type);
        protected ItemFieldDescriptor createDescriptor(ItemFieldDescriptor.Type type, string description, bool readOnly = false)
        {
            return new ItemFieldDescriptor("?", readOnly, type)
            {

                Description = description
            };
        }
        protected ItemFieldDescriptor createDescriptor(string name, ItemFieldDescriptor.Type type, bool readOnly = false) => new ItemFieldDescriptor(name, readOnly, type);
        protected ItemFieldDescriptor createDescriptor(string name, ItemFieldDescriptor.Type type, string description, bool readOnly = false)
        {
            return new ItemFieldDescriptor(name, readOnly, type)
            {
                Description = description
            };
        }
    }
}
