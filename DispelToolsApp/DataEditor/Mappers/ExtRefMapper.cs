using System.Collections.Generic;
using static DispelTools.DataEditor.ItemFieldDescriptor;

namespace DispelTools.DataEditor.Mappers
{
    internal class ExtRefMapper : Mapper
    {
        private const byte FILLER = 0xcd;
        private const int NAME_STRING_MAX_LENGTH = 32;
        protected override int PropertyItemSize => 46 * 4;

        protected override List<ItemFieldDescriptor> CreateDescriptors()
        {
            var builder = new FileDescriptorBuilder();
            builder.Add("number in file", AsByte());
            builder.Add(AsByte());
            builder.Add("ExtId", AsByte(), "Id from Extra.ini");

            builder.Add("name", AsFixedString(NAME_STRING_MAX_LENGTH, FILLER));
            builder.Add(AsByte());//4 ? end of string? EOT

            builder.Add("xPos", AsInt32());
            builder.Add("yPos", AsInt32());

            builder.Add(AsFixedString(4, FILLER));

            builder.Fill(25, AsInt32());
            builder.Fill(36, AsByte());

            return builder.Build();
        }
    }
}
