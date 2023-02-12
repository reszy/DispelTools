namespace DispelTools.DataEditor.Mappers
{
    internal class ExtRefMapper : MapperDefinition
    {
        private const byte FILLER = 0xcd;
        private const int NAME_STRING_MAX_LENGTH = 32;

        public override int PropertyItemSize => 46 * 4;
        public override string GetMapperName() => "Ext*.ref mapper";

        public override List<ItemFieldDescriptor> CreateDescriptors()
        {
            var builder = new FileDescriptorBuilder();
            builder.Add("number in file", ItemFieldDescriptor.AsByte());
            builder.Add(ItemFieldDescriptor.AsByte());
            builder.Add("ExtId", ItemFieldDescriptor.AsByte(), "Id from Extra.ini");

            builder.Add("name", ItemFieldDescriptor.AsFixedString(NAME_STRING_MAX_LENGTH, FILLER));
            builder.Add("type", ItemFieldDescriptor.AsByte(), "7-magic, 6-interactive object, 5-altar, 4-sign, 2-door, 0-chest");

            builder.Add("xPos", ItemFieldDescriptor.AsInt32());
            builder.Add("yPos", ItemFieldDescriptor.AsInt32());

            builder.Add("rotation", ItemFieldDescriptor.AsByte());
            builder.Add(ItemFieldDescriptor.AsByte());
            builder.Add(ItemFieldDescriptor.AsByte());
            builder.Add(ItemFieldDescriptor.AsByte());

            builder.Add(ItemFieldDescriptor.AsInt32());
            builder.Add("closed", ItemFieldDescriptor.AsInt32(), "chest 0-open, 1-closed");

            builder.Add("required item id", ItemFieldDescriptor.AsByte(), "lower bound");
            builder.Add("required item type id", ItemFieldDescriptor.AsByte());
            builder.Add(ItemFieldDescriptor.AsByte());
            builder.Add(ItemFieldDescriptor.AsByte());

            builder.Add("2?required item id", ItemFieldDescriptor.AsByte(), "upper bound");
            builder.Add("2?required item type id", ItemFieldDescriptor.AsByte());
            builder.Add(ItemFieldDescriptor.AsByte());
            builder.Add(ItemFieldDescriptor.AsByte());

            builder.Fill(4, ItemFieldDescriptor.AsInt32());

            builder.Add("gold amount", ItemFieldDescriptor.AsInt32());

            builder.Add("item id", ItemFieldDescriptor.AsByte());
            builder.Add("item group type id", ItemFieldDescriptor.AsByte());
            builder.Add(ItemFieldDescriptor.AsByte());
            builder.Add(ItemFieldDescriptor.AsByte());

            builder.Add("item count", ItemFieldDescriptor.AsInt32());

            builder.Fill(10, ItemFieldDescriptor.AsInt32());

            builder.Add("event id", ItemFieldDescriptor.AsInt32(), "id from event.ini");
            builder.Add("message id", ItemFieldDescriptor.AsInt32(), "id from message.scr for signs");
            builder.Add(ItemFieldDescriptor.AsInt32());
            builder.Add(ItemFieldDescriptor.AsInt32());

            builder.Fill(32, ItemFieldDescriptor.AsByte());
            builder.Add("Visibility?", ItemFieldDescriptor.AsByte());
            builder.Fill(3, ItemFieldDescriptor.AsByte());

            return builder.Build();
        }
    }
}
