using System.Collections.Generic;

namespace DispelTools.DataEditor.Mappers
{
    internal class NpcRefMapper : Mapper
    {
        private const byte FILLER = 0xCD;
        private const int STRING_MAX_LENGTH = 260;

        private readonly int elementStep = 0x2a0;
        private static List<ItemFieldDescriptor> descriptorList;

        protected override int PropertyItemSize => elementStep;

        protected override List<ItemFieldDescriptor> FileDescriptor { get { if (descriptorList == null) { descriptorList = CreateMap(); } return descriptorList; } }
        private List<ItemFieldDescriptor> CreateMap()
        {

            var list = new List<ItemFieldDescriptor>
            {
                createDescriptor("id", ItemFieldDescriptor.AsInt32(), true),
                createDescriptor("ncpId", ItemFieldDescriptor.AsInt32()),
                createDescriptor("name", ItemFieldDescriptor.AsFixedString(STRING_MAX_LENGTH, FILLER)),
                createDescriptor("text", ItemFieldDescriptor.AsFixedString(STRING_MAX_LENGTH, FILLER)),


                createDescriptor("party/scriptId",  ItemFieldDescriptor.AsInt32()),
                createDescriptor("showOnEvent?", ItemFieldDescriptor.AsInt32()),
                createDescriptor(ItemFieldDescriptor.AsInt32()),
                createDescriptor("goto1Filled", ItemFieldDescriptor.AsInt32()),
                createDescriptor("goto2Filled", ItemFieldDescriptor.AsInt32()),

                createDescriptor("goto3Filled", ItemFieldDescriptor.AsInt32()),
                createDescriptor("goto4Filled", ItemFieldDescriptor.AsInt32(), "when goto4 not filled its 1, idk why"),
                createDescriptor("goto1X", ItemFieldDescriptor.AsInt32()),
                createDescriptor("goto2X", ItemFieldDescriptor.AsInt32()),
                createDescriptor("goto3X", ItemFieldDescriptor.AsInt32()),

                createDescriptor("goto4X", ItemFieldDescriptor.AsInt32()),
                createDescriptor("goto1Y", ItemFieldDescriptor.AsInt32()),
                createDescriptor("goto2Y", ItemFieldDescriptor.AsInt32()),
                createDescriptor("goto3Y", ItemFieldDescriptor.AsInt32()),
                createDescriptor("goto4Y", ItemFieldDescriptor.AsInt32()),

                createDescriptor(ItemFieldDescriptor.AsInt32()),
                createDescriptor(ItemFieldDescriptor.AsInt32()),
                createDescriptor(ItemFieldDescriptor.AsInt32()),
                createDescriptor(ItemFieldDescriptor.AsInt32()),
                createDescriptor("lookingDirection", ItemFieldDescriptor.AsInt32(), "0 = up, clockwise"),

                createDescriptor(ItemFieldDescriptor.AsInt32()),
                createDescriptor(ItemFieldDescriptor.AsInt32()),
                createDescriptor(ItemFieldDescriptor.AsInt32()),
                createDescriptor(ItemFieldDescriptor.AsInt32()),
                createDescriptor(ItemFieldDescriptor.AsInt32()),

                createDescriptor(ItemFieldDescriptor.AsInt32()),
                createDescriptor(ItemFieldDescriptor.AsInt32()),
                createDescriptor(ItemFieldDescriptor.AsInt32()),
                createDescriptor(ItemFieldDescriptor.AsInt32()),
                createDescriptor(ItemFieldDescriptor.AsInt32()),

                createDescriptor(ItemFieldDescriptor.AsInt32()),
                createDescriptor(ItemFieldDescriptor.AsInt32()),
                createDescriptor(ItemFieldDescriptor.AsInt32()),
                createDescriptor(ItemFieldDescriptor.AsInt32()),

                createDescriptor("dialogId", ItemFieldDescriptor.AsInt32(), "also text for shop"),
                createDescriptor(ItemFieldDescriptor.AsInt32())
            };

            return list;
        }
    }
}
