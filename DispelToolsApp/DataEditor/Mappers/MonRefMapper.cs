namespace DispelTools.DataEditor.Mappers
{
    internal class MonRefMapper : MapperDefinition
    {
        public override int ItemSize => 14 * 4;
        public override string GetMapperName() => "Mon*.ref mapper";

        public override List<ItemFieldDescriptor> CreateDescriptors()
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
