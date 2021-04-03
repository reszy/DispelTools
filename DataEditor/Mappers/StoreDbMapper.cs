using System.Collections.Generic;

namespace DispelTools.DataEditor.Mappers
{
    internal class StoreDbMapper : Mapper
    {
        private const byte FILLER = 0x0;
        private static List<ItemFieldDescriptor> descriptorList;
        protected override int PropertyItemSize => 237 * 4;

        protected override List<ItemFieldDescriptor> FileDescriptor { get { if (descriptorList == null) { descriptorList = CreateMap(); } return descriptorList; } }

        private List<ItemFieldDescriptor> CreateMap()
        {
            var list = new List<ItemFieldDescriptor>
            {
                createDescriptor("name", ItemFieldDescriptor.AsFixedString(32, FILLER)),
                createDescriptor(ItemFieldDescriptor.AsByteArray(148), true),
                createDescriptor("text", ItemFieldDescriptor.AsFixedString(512, FILLER)),
                createDescriptor("haggle success", ItemFieldDescriptor.AsFixedString(128, FILLER)),
                createDescriptor("haggle fail", ItemFieldDescriptor.AsFixedString(128, FILLER)),

            };

            return list;
        }
    }
}
