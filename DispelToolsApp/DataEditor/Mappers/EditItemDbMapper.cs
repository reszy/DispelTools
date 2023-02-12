namespace DispelTools.DataEditor.Mappers
{
    internal class EditItemDbMapper : MapperDefinition
    {
        private const byte FILLER = 0x0;
        private const int NAME_STRING_MAX_LENGTH = 30;
        private const int DESCRIPTION_STRING_MAX_LENGTH = 202;

        public override int PropertyItemSize => 67 * 4;

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
            builder.Add(ItemFieldDescriptor.AsInt16());
            builder.Add("item destroying power", ItemFieldDescriptor.AsInt16());
            builder.Add(ItemFieldDescriptor.AsByte());
            builder.Add("modifies item", ItemFieldDescriptor.AsByte());
            builder.Add("Additional effect", ItemFieldDescriptor.AsInt16(), "poison or burn or none");

            return builder.Build();
        }
    }
}
