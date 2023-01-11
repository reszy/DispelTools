using System.Collections.Generic;

namespace DispelTools.DataEditor.Mappers
{
    internal class HealItemDbMapper : Mapper
    {
        private const byte FILLER = 0x0;
        private const int NAME_STRING_MAX_LENGTH = 30;
        private const int DESCRIPTION_STRING_MAX_LENGTH = 202;
        internal override int PropertyItemSize => 63 * 4;

        protected override List<ItemFieldDescriptor> CreateDescriptors()
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
            builder.Add("Full PZ", ItemFieldDescriptor.AsByte());
            builder.Add("Full PM", ItemFieldDescriptor.AsByte());
            builder.Add("Poison heal", ItemFieldDescriptor.AsByte());
            builder.Add("Petrif heal", ItemFieldDescriptor.AsByte());
            builder.Add("Heal polimorph", ItemFieldDescriptor.AsByte());
            builder.Add(ItemFieldDescriptor.AsByte());
            builder.Add(ItemFieldDescriptor.AsInt16());

            return builder.Build();
        }
    }
}
