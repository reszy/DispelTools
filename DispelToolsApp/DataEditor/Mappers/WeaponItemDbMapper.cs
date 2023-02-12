namespace DispelTools.DataEditor.Mappers
{
    internal class WeaponItemDbMapper : MapperDefinition
    {
        private const byte FILLER = 0x0;
        private const int NAME_STRING_MAX_LENGTH = 30;
        private const int DESCRIPTION_STRING_MAX_LENGTH = 202;

        public override int PropertyItemSize => 71 * 4;
        public override List<ItemFieldDescriptor> CreateDescriptors()
        {
            var builder = new FileDescriptorBuilder();
            builder.Add("name", ItemFieldDescriptor.AsFixedString(NAME_STRING_MAX_LENGTH, FILLER));
            builder.Add("description", ItemFieldDescriptor.AsFixedString(DESCRIPTION_STRING_MAX_LENGTH, FILLER));

            builder.Add("Base price", ItemFieldDescriptor.AsInt16());
            builder.Add(ItemFieldDescriptor.AsInt16());
            builder.Add(ItemFieldDescriptor.AsInt16());
            builder.Add(ItemFieldDescriptor.AsInt16());
            builder.Add("PZ", ItemFieldDescriptor.AsInt16());
            builder.Add("PM", ItemFieldDescriptor.AsInt16());
            builder.Add("SIŁ", ItemFieldDescriptor.AsInt16());
            builder.Add("ZW", ItemFieldDescriptor.AsInt16());
            builder.Add("MM", ItemFieldDescriptor.AsInt16());
            builder.Add("TF", ItemFieldDescriptor.AsInt16());
            builder.Add("UNK", ItemFieldDescriptor.AsInt16());
            builder.Add("TRF", ItemFieldDescriptor.AsInt16());
            builder.Add("ATK", ItemFieldDescriptor.AsInt16());
            builder.Add("OBR", ItemFieldDescriptor.AsInt16());
            builder.Add("MAG", ItemFieldDescriptor.AsInt16());
            builder.Add("WYT", ItemFieldDescriptor.AsInt16());
            builder.Add(ItemFieldDescriptor.AsInt16());
            builder.Add(ItemFieldDescriptor.AsInt16());
            builder.Add("REQ SIŁ", ItemFieldDescriptor.AsInt16());
            builder.Add(ItemFieldDescriptor.AsInt16());
            builder.Add("REQ ZW", ItemFieldDescriptor.AsInt16());
            builder.Add(ItemFieldDescriptor.AsInt16());
            builder.Add("REQ MM", ItemFieldDescriptor.AsInt16());
            builder.Add(ItemFieldDescriptor.AsInt16());
            builder.Add(ItemFieldDescriptor.AsInt16());
            builder.Add(ItemFieldDescriptor.AsInt16());

            return builder.Build();
        }
    }
}
