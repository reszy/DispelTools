using System.Collections.Generic;

namespace DispelTools.DataEditor.Mappers
{
    internal class MiscItemDbMapper : Mapper
    {
        private const byte FILLER = 0x0;
        private const int NAME_STRING_MAX_LENGTH = 30;
        private const int DESCRIPTION_STRING_MAX_LENGTH = 202;
        protected override int PropertyItemSize => 64 * 4;

        protected override List<ItemFieldDescriptor> CreateDescriptors()
        {
            var builder = new FileDescriptorBuilder();
            builder.Add("name", ItemFieldDescriptor.AsFixedString(NAME_STRING_MAX_LENGTH, FILLER));
            builder.Add("description", ItemFieldDescriptor.AsFixedString(DESCRIPTION_STRING_MAX_LENGTH, FILLER));

            builder.Add("base price", ItemFieldDescriptor.AsInt32());
            builder.Add(ItemFieldDescriptor.AsInt16());
            builder.Add(ItemFieldDescriptor.AsInt16());

            builder.Add(ItemFieldDescriptor.AsInt16());
            builder.Add(ItemFieldDescriptor.AsInt16());
            builder.Add(ItemFieldDescriptor.AsInt16());
            builder.Add(ItemFieldDescriptor.AsInt16());

            builder.Add(ItemFieldDescriptor.AsInt16());
            builder.Add(ItemFieldDescriptor.AsInt16());
            builder.Add(ItemFieldDescriptor.AsInt16());
            builder.Add(ItemFieldDescriptor.AsInt16());

            return builder.Build();
        }
    }
}
