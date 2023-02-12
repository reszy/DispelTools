using DispelTools.Common.DataProcessing;
using DispelTools.DataEditor.Data;
using DispelTools.DataEditor.Mappers;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;
using System.Linq;

namespace DispelTools.DataEditor
{
    public class Mapper
    {
        private readonly IFileSystem fs;
        private readonly List<ItemFieldDescriptor> descriptorList;

        private readonly int propertyItemSize;
        private readonly byte counterSize;

        public Mapper(IFileSystem fs, MapperDefinition mapperDefinition)
        {
            this.fs = fs;
            descriptorList = mapperDefinition.CreateDescriptors();
            propertyItemSize = mapperDefinition.PropertyItemSize;
            counterSize = mapperDefinition.CounterSize;
        }

        public List<PropertyItem> ReadFile(string filename, WorkReporter workReporter)
        {
            int elementStep = PropertyItemSize;
            var list = new List<PropertyItem>();
            using (var reader = new BinaryReader(fs.File.OpenRead(filename)))
            {
                int expectedElements = 0;
                int spaceForElements = (int)Math.Floor((decimal)((reader.BaseStream.Length - counterSize) / elementStep));
                if (counterSize > 0)
                {
                    byte[] fullCount = new byte[] { 0, 0, 0, 0 };
                    var count = reader.ReadBytes(counterSize);
                    count.CopyTo(fullCount, 0);
                    expectedElements = BitConverter.ToInt32(fullCount, 0);
                }
                else
                {
                    expectedElements = spaceForElements;
                }
                if (expectedElements != spaceForElements)
                {
                    var canProceed = workReporter.AskUserIfCanProceed(new($"In file count = {expectedElements}, expected by file size {spaceForElements}"));
                    if (!canProceed) return list;
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
            using (var writer = new BinaryWriter(fs.File.OpenWrite(filename)))
            {
                writer.BaseStream.Position = elementNumber * PropertyItemSize + counterSize;
                WriteElement(writer, element);
            }
        }

        public Mapping CreateMapping(params string[] fieldNames) => new Mapping(this, fieldNames);
        public int PropertyItemSize { get => propertyItemSize; }

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

        public static Mapper? GetMapperForFilename(IFileSystem fs, string filename)
        {
            string filenameWithExtension = fs.Path.GetFileName(filename);
            if (filenameWithExtension.ToUpper().StartsWith("NPC") && filenameWithExtension.ToUpper().EndsWith("REF"))
            {
                return new(fs, new NpcRefMapper());
            }
            if (filenameWithExtension.ToUpper().StartsWith("MON") && filenameWithExtension.ToUpper().EndsWith("REF"))
            {
                return new(fs, new MonRefMapper());
            }
            if (filenameWithExtension.ToUpper().StartsWith("EXT") && filenameWithExtension.ToUpper().EndsWith("REF"))
            {
                return new(fs, new ExtRefMapper());
            }
            if (filenameWithExtension.ToUpper().Equals("EDITITEM.DB"))
            {
                return new(fs, new EditItemDbMapper());
            }
            if (filenameWithExtension.ToUpper().Equals("HEALITEM.DB"))
            {
                return new(fs, new HealItemDbMapper());
            }
            if (filenameWithExtension.ToUpper().Equals("EVENTITEM.DB"))
            {
                return new(fs, new EventItemDbMapper());
            }
            if (filenameWithExtension.ToUpper().Equals("MISCITEM.DB"))
            {
                return new(fs, new MiscItemDbMapper());
            }
            if (filenameWithExtension.ToUpper().Equals("WEAPONITEM.DB"))
            {
                return new(fs, new WeaponItemDbMapper());
            }
            if (filenameWithExtension.ToUpper().Equals("STORE.DB"))
            {
                return new(fs, new StoreDbMapper());
            }
            if (filenameWithExtension.ToUpper().Equals("MONSTER.DB"))
            {
                return new(fs, new MonsterDbMapper());
            }
            if (filenameWithExtension.ToUpper().Equals("MULMONSTER.DB"))
            {
                return new(fs, new MulMonsterDbMapper());
            }

            return null;
        }
    }
}
