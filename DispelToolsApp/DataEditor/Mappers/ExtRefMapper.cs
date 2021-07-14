using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DispelTools.DataEditor.ItemFieldDescriptor;

namespace DispelTools.DataEditor.Mappers
{
    internal class ExtRefMapper : Mapper
    {
        private const byte FILLER = 0xcd;
        private const int NAME_STRING_MAX_LENGTH = 32;
        private static List<ItemFieldDescriptor> descriptorList;
        protected override int PropertyItemSize => 46 * 4;

        protected override List<ItemFieldDescriptor> FileDescriptor { get { if (descriptorList == null) { descriptorList = CreateMap(); } return descriptorList; } }

        private List<ItemFieldDescriptor> CreateMap()
        {
            var list = new List<ItemFieldDescriptor>
            {
                createDescriptor("number in file", AsByte()),
                createDescriptor(AsByte()),
                createDescriptor("ExtId", AsByte(), "Id from Extra.ini"),

                createDescriptor("name", AsFixedString(NAME_STRING_MAX_LENGTH, FILLER)),
                createDescriptor(AsByte()),//4 ? end of string? EOT

                createDescriptor("xPos", AsInt32()),
                createDescriptor("yPos", AsInt32()),

                createDescriptor(AsFixedString(4, FILLER)),
            };

            FillWithUnknownInts(25, list);
            FillWithUnknownBytes(36, list);

            return list;
        }
    }
}
