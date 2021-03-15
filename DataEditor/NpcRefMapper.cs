using System.Collections.Generic;

namespace DispelTools.DataEditor
{
    internal class NpcRefMapper : Mapper
    {
        private readonly int elementStep = 0x2a0;
        private static List<ItemFieldDescriptor> descriptorList;

        protected override int PropertyItemSize => elementStep;

        protected override List<ItemFieldDescriptor> FileDescriptor { get { if (descriptorList == null) { descriptorList = CreateMap(); } return descriptorList; } }
        private List<ItemFieldDescriptor> CreateMap()
        {

            var list = new List<ItemFieldDescriptor>
            {
                createDescriptor(ItemFieldDescriptor.Type.INT32, true),
                createDescriptor("id", ItemFieldDescriptor.Type.INT32, true),
                createDescriptor("ncpId", ItemFieldDescriptor.Type.INT32),
                createDescriptor("name", ItemFieldDescriptor.Type.STRING),
                createDescriptor("text", ItemFieldDescriptor.Type.STRING),


                createDescriptor("party/scriptId",  ItemFieldDescriptor.Type.INT32),
                createDescriptor("showOnEvent?", ItemFieldDescriptor.Type.INT32),
                createDescriptor(ItemFieldDescriptor.Type.INT32),
                createDescriptor("goto1Filled", ItemFieldDescriptor.Type.INT32),
                createDescriptor("goto2Filled", ItemFieldDescriptor.Type.INT32),

                createDescriptor("goto3Filled", ItemFieldDescriptor.Type.INT32),
                createDescriptor("goto4Filled", ItemFieldDescriptor.Type.INT32, "when goto4 not filled its 1, idk why"),
                createDescriptor("goto1X", ItemFieldDescriptor.Type.INT32),
                createDescriptor("goto2X", ItemFieldDescriptor.Type.INT32),
                createDescriptor("goto3X", ItemFieldDescriptor.Type.INT32),

                createDescriptor("goto4X", ItemFieldDescriptor.Type.INT32),
                createDescriptor("goto1Y", ItemFieldDescriptor.Type.INT32),
                createDescriptor("goto2Y", ItemFieldDescriptor.Type.INT32),
                createDescriptor("goto3Y", ItemFieldDescriptor.Type.INT32),
                createDescriptor("goto4Y", ItemFieldDescriptor.Type.INT32),

                createDescriptor(ItemFieldDescriptor.Type.INT32),
                createDescriptor(ItemFieldDescriptor.Type.INT32),
                createDescriptor(ItemFieldDescriptor.Type.INT32),
                createDescriptor(ItemFieldDescriptor.Type.INT32),
                createDescriptor("lookingDirection", ItemFieldDescriptor.Type.INT32, "0 = up, clockwise"),

                createDescriptor(ItemFieldDescriptor.Type.INT32),
                createDescriptor(ItemFieldDescriptor.Type.INT32),
                createDescriptor(ItemFieldDescriptor.Type.INT32),
                createDescriptor(ItemFieldDescriptor.Type.INT32),
                createDescriptor(ItemFieldDescriptor.Type.INT32),

                createDescriptor(ItemFieldDescriptor.Type.INT32),
                createDescriptor(ItemFieldDescriptor.Type.INT32),
                createDescriptor(ItemFieldDescriptor.Type.INT32),
                createDescriptor(ItemFieldDescriptor.Type.INT32),
                createDescriptor(ItemFieldDescriptor.Type.INT32),

                createDescriptor(ItemFieldDescriptor.Type.INT32),
                createDescriptor(ItemFieldDescriptor.Type.INT32),
                createDescriptor(ItemFieldDescriptor.Type.INT32),
                createDescriptor(ItemFieldDescriptor.Type.INT32),
                createDescriptor("dialogId", ItemFieldDescriptor.Type.INT32, "also text for shop")
            };

            return list;
        }
    }
}
