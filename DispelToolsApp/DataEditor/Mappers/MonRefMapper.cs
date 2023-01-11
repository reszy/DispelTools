using System.Collections.Generic;
using System.IO.Abstractions;

namespace DispelTools.DataEditor.Mappers
{
    internal class MonRefMapper : Mapper
    {
        public MonRefMapper() { }

        public MonRefMapper(IFileSystem fs) : base(fs) { }

        internal override int PropertyItemSize => 14 * 4;

        protected override List<ItemFieldDescriptor> CreateDescriptors()
        {

            var builder = new FileDescriptorBuilder();
            builder.Add("fileId", ItemFieldDescriptor.AsInt32());
            builder.Add("monId", ItemFieldDescriptor.AsInt32());
            builder.Add("posX", ItemFieldDescriptor.AsInt32());
            builder.Add("posY", ItemFieldDescriptor.AsInt32());
            builder.Add(ItemFieldDescriptor.AsInt32());
            builder.Add(ItemFieldDescriptor.AsInt32());
            builder.Add(ItemFieldDescriptor.AsInt32());
            builder.Add(ItemFieldDescriptor.AsInt32());
            builder.Add(ItemFieldDescriptor.AsInt32());

            builder.Add("LootSlot1 itemId", ItemFieldDescriptor.AsByte());
            builder.Add("LootSlot1 itemType", ItemFieldDescriptor.AsByte());
            builder.Add(ItemFieldDescriptor.AsByte());
            builder.Add(ItemFieldDescriptor.AsByte());

            builder.Add("LootSlot2 itemId", ItemFieldDescriptor.AsByte());
            builder.Add("LootSlot2 itemType", ItemFieldDescriptor.AsByte());
            builder.Add(ItemFieldDescriptor.AsByte());
            builder.Add(ItemFieldDescriptor.AsByte());

            builder.Add("LootSlot3 itemId", ItemFieldDescriptor.AsByte());
            builder.Add("LootSlot3 itemType", ItemFieldDescriptor.AsByte());
            builder.Add(ItemFieldDescriptor.AsByte());
            builder.Add(ItemFieldDescriptor.AsByte());

            builder.Add(ItemFieldDescriptor.AsInt32());
            builder.Add(ItemFieldDescriptor.AsInt32());

            return builder.Build();
        }
    }
}
