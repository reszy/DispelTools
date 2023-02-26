using DispelTools.Common.DataProcessing;
using DispelTools.DataEditor.Data;
using DispelTools.DataEditor.Mappers;
using System.IO.Abstractions;

namespace DispelTools.DataEditor
{
    public partial class SimpleDataLoader
    {
        private readonly IFileSystem fs;
        private readonly List<ItemFieldDescriptor> descriptorList;

        private readonly int dataItemSize;
        private readonly byte inFileCounterSize;

        public SimpleDataLoader(IFileSystem fs, MapperDefinition mapperDefinition)
        {
            this.fs = fs;
            MapperDefinition = mapperDefinition;
            descriptorList = mapperDefinition.CreateDescriptors();
            dataItemSize = mapperDefinition.ItemSize;
            inFileCounterSize = mapperDefinition.InFileCounterSize;
        }

        public SimpleDataContainer LoadData(string filename, WorkReporter workReporter)
        {
            return new(MapperDefinition, ReadFile(filename, workReporter), filename, fs.Path.GetFileName(filename));
        }
        private List<DataItem> ReadFile(string filename, WorkReporter workReporter)
        {
            int elementStep = DataItemSize;
            var list = new List<DataItem>();
            using (var reader = new BinaryReader(fs.File.OpenRead(filename)))
            {
                int expectedElements = 0;
                int spaceForElements = (int)Math.Floor((decimal)((reader.BaseStream.Length - inFileCounterSize) / elementStep));
                if (inFileCounterSize > 0)
                {
                    byte[] fullCount = new byte[] { 0, 0, 0, 0 };
                    var count = reader.ReadBytes(inFileCounterSize);
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
        public void SaveElement(SimpleDataContainer container, DataItem item)
        {
            var itemIndex = container.IndexOf(item);
            using var writer = new BinaryWriter(fs.File.OpenWrite(container.Path));
            writer.BaseStream.Position = itemIndex * DataItemSize + inFileCounterSize;
            WriteElement(writer, item);
        }
        public int DataItemSize { get => dataItemSize; }
        public MapperDefinition MapperDefinition { get; }

        private DataItem ReadElement(BinaryReader reader)
        {
            var propertyItem = new DataItem();
            for (int i = 0; i < descriptorList.Count; i++)
            {
                var fieldDescriptor = descriptorList[i];
                object value = fieldDescriptor.ItemFieldDescriptorType.Read(reader);
                propertyItem.AddField(CreateField(fieldDescriptor, value));
            }
            return propertyItem;
        }
        private void WriteElement(BinaryWriter writer, DataItem propertyitem)
        {
            for (int i = 0; i < descriptorList.Count; i++)
            {
                var descriptor = descriptorList[i];
                var field = propertyitem[i];
                descriptor.ItemFieldDescriptorType.Write(writer, field);
            }
        }

        private static IField CreateField(ItemFieldDescriptor descriptor, object value)
        {
            if (descriptor.FieldClass == typeof(ByteArrayField)) return new ByteArrayField(descriptor.Name, (byte[])value, descriptor.ItemFieldDescriptorType.VisualFieldType);
            if (descriptor.FieldClass == typeof(TextField)) return new TextField(descriptor.Name, (byte[])value, descriptor.Encoding);
            if (descriptor.FieldClass == typeof(PrimitiveField<byte>)) return new PrimitiveField<byte>(descriptor.Name, (byte)value, descriptor.ItemFieldDescriptorType.VisualFieldType);
            if (descriptor.FieldClass == typeof(PrimitiveField<int>)) return new PrimitiveField<int>(descriptor.Name, (int)value, descriptor.ItemFieldDescriptorType.VisualFieldType);
            if (descriptor.FieldClass == typeof(PrimitiveField<short>)) return new PrimitiveField<short>(descriptor.Name, (short)value, descriptor.ItemFieldDescriptorType.VisualFieldType);
            throw new ArgumentException("Unsupported field type", nameof(descriptor));
        }

        public static SimpleDataLoader? GetMapperForFilename(IFileSystem fs, string filename)
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
