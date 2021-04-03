using System.Collections.Generic;

namespace DispelTools.DataEditor.Mappers
{
    internal class MonRefMapper : Mapper
    {
        private static List<ItemFieldDescriptor> descriptorList;

        protected override int PropertyItemSize => 14 * 4;

        protected override List<ItemFieldDescriptor> FileDescriptor { get { if (descriptorList == null) { descriptorList = CreateMap(); } return descriptorList; } }

        private List<ItemFieldDescriptor> CreateMap()
        {

            var list = new List<ItemFieldDescriptor>
            {
                createDescriptor("fileId", ItemFieldDescriptor.AsInt32()),
                createDescriptor("monId", ItemFieldDescriptor.AsInt32()),
                createDescriptor("posX", ItemFieldDescriptor.AsInt32()),
                createDescriptor("posY", ItemFieldDescriptor.AsInt32()),
                createDescriptor(ItemFieldDescriptor.AsInt32()),
                createDescriptor(ItemFieldDescriptor.AsInt32()),
                createDescriptor(ItemFieldDescriptor.AsInt32()),
                createDescriptor(ItemFieldDescriptor.AsInt32()),
                createDescriptor(ItemFieldDescriptor.AsInt32()),
                createDescriptor(ItemFieldDescriptor.AsInt32()),
                createDescriptor(ItemFieldDescriptor.AsInt32()),
                createDescriptor(ItemFieldDescriptor.AsInt32()),
                createDescriptor(ItemFieldDescriptor.AsInt32()),
                createDescriptor(ItemFieldDescriptor.AsInt32())
            };

            return list;
        }
    }
}
