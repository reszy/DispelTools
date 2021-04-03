using System.Collections.Generic;

namespace DispelTools.DataEditor.Mappers
{
    internal class MiscItemDbMapper : Mapper
    {
        private const byte FILLER = 0x0;
        private const int NAME_STRING_MAX_LENGTH = 30;
        private const int DESCRIPTION_STRING_MAX_LENGTH = 202;
        private static List<ItemFieldDescriptor> descriptorList;
        protected override int PropertyItemSize => 64 * 4;

        protected override List<ItemFieldDescriptor> FileDescriptor { get { if (descriptorList == null) { descriptorList = CreateMap(); } return descriptorList; } }

        private List<ItemFieldDescriptor> CreateMap()
        {
            var list = new List<ItemFieldDescriptor>
            {
                createDescriptor("name", ItemFieldDescriptor.AsFixedString(NAME_STRING_MAX_LENGTH, FILLER)),
                createDescriptor("description", ItemFieldDescriptor.AsFixedString(DESCRIPTION_STRING_MAX_LENGTH, FILLER)),

                createDescriptor("base price",ItemFieldDescriptor.AsInt32()),
                createDescriptor(ItemFieldDescriptor.AsInt16()),
                createDescriptor(ItemFieldDescriptor.AsInt16()),

                createDescriptor(ItemFieldDescriptor.AsInt16()),
                createDescriptor(ItemFieldDescriptor.AsInt16()),
                createDescriptor(ItemFieldDescriptor.AsInt16()),
                createDescriptor(ItemFieldDescriptor.AsInt16()),

                createDescriptor(ItemFieldDescriptor.AsInt16()),
                createDescriptor(ItemFieldDescriptor.AsInt16()),
                createDescriptor(ItemFieldDescriptor.AsInt16()),
                createDescriptor(ItemFieldDescriptor.AsInt16()),

            };

            return list;
        }
    }
}
