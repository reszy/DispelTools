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
            builder.Add("type", AsByte(), "7-magic, 6-interactive object, 5-altar, 4-sign, 2-door, 0-chest");

            builder.Add("xPos", AsInt32());
            builder.Add("yPos", AsInt32());

            builder.Add("rotation", AsByte());
            builder.Add(AsByte());
            builder.Add(AsByte());
            builder.Add(AsByte());

            builder.Add(AsInt32());
            builder.Add("closed", AsInt32(), "chest 0-open, 1-closed");

            builder.Add("required item id", AsByte(), "lower bound");
            builder.Add("required item type id", AsByte());
            builder.Add(AsByte());
            builder.Add(AsByte());

            builder.Add("2?required item id", AsByte(), "upper bound");
            builder.Add("2?required item type id", AsByte());
            builder.Add(AsByte());
            builder.Add(AsByte());

            builder.Fill(4, AsInt32());

            builder.Add("gold amount", AsInt32());

            builder.Add("item id", AsByte());
            builder.Add("item group type id", AsByte());
            builder.Add(AsByte());
            builder.Add(AsByte());

            builder.Add("item count", AsInt32());

            builder.Fill(10, AsInt32());

            builder.Add("event id",AsInt32(), "id from event.ini");
            builder.Add("message id",AsInt32(), "id from message.scr for signs");
            builder.Add(AsInt32());
            builder.Add(AsInt32());

            builder.Fill(32, AsByte());
            builder.Add("Visibility?", AsByte());
            builder.Fill(3, AsByte());

            return builder.Build();
        }
    }
}
