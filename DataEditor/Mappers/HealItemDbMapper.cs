using System.Collections.Generic;

namespace DispelTools.DataEditor.Mappers
{
    internal class HealItemDbMapper : Mapper
    {
        private const byte FILLER = 0x0;
        private const int NAME_STRING_MAX_LENGTH = 30;
        private const int DESCRIPTION_STRING_MAX_LENGTH = 202;
        private static List<ItemFieldDescriptor> descriptorList;
        protected override int PropertyItemSize => 63 * 4;

        protected override List<ItemFieldDescriptor> FileDescriptor { get { if (descriptorList == null) { descriptorList = CreateMap(); } return descriptorList; } }

        private List<ItemFieldDescriptor> CreateMap()
        {
            var list = new List<ItemFieldDescriptor>
            {
                createDescriptor("name", ItemFieldDescriptor.AsFixedString(NAME_STRING_MAX_LENGTH, FILLER)),
                createDescriptor("description", ItemFieldDescriptor.AsFixedString(DESCRIPTION_STRING_MAX_LENGTH, FILLER)),

                createDescriptor("Base price", ItemFieldDescriptor.AsInt16()),
                createDescriptor(ItemFieldDescriptor.AsInt16()),
                createDescriptor(ItemFieldDescriptor.AsInt16()),
                createDescriptor(ItemFieldDescriptor.AsInt16()),
                createDescriptor("PZ", ItemFieldDescriptor.AsInt16()),
                createDescriptor("PM", ItemFieldDescriptor.AsInt16()),
                createDescriptor("Full PZ", ItemFieldDescriptor.AsByte()),
                createDescriptor("Full PM", ItemFieldDescriptor.AsByte()),
                createDescriptor("Poison heal", ItemFieldDescriptor.AsByte()),
                createDescriptor("Petrif heal", ItemFieldDescriptor.AsByte()),
                createDescriptor("Heal polimorph", ItemFieldDescriptor.AsByte()),
                createDescriptor(ItemFieldDescriptor.AsByte()),
                createDescriptor(ItemFieldDescriptor.AsInt16()),

            };

            return list;
        }
    }
}
