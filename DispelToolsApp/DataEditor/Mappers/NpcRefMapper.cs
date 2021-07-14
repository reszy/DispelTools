using System.Collections.Generic;

namespace DispelTools.DataEditor.Mappers
{
    internal class NpcRefMapper : Mapper
    {
        private const byte FILLER = 0xCD;
        private const int STRING_MAX_LENGTH = 260;

        private readonly int elementStep = 0x2a0;

        protected override int PropertyItemSize => elementStep;

        protected override List<ItemFieldDescriptor> CreateDescriptors()
        {
            var builder = new FileDescriptorBuilder();
            builder.Add("id", ItemFieldDescriptor.AsInt32(), true);
            builder.Add("ncpId", ItemFieldDescriptor.AsInt32());
            builder.Add("name", ItemFieldDescriptor.AsFixedString(STRING_MAX_LENGTH, FILLER));
            builder.Add("text", ItemFieldDescriptor.AsFixedString(STRING_MAX_LENGTH, FILLER));


            builder.Add("party/scriptId", ItemFieldDescriptor.AsInt32());
            builder.Add("showOnEvent?", ItemFieldDescriptor.AsInt32());
            builder.Add(ItemFieldDescriptor.AsInt32());
            builder.Add("goto1Filled", ItemFieldDescriptor.AsInt32());
            builder.Add("goto2Filled", ItemFieldDescriptor.AsInt32());

            builder.Add("goto3Filled", ItemFieldDescriptor.AsInt32());
            builder.Add("goto4Filled", ItemFieldDescriptor.AsInt32(), "when goto4 not filled its 1, idk why");
            builder.Add("goto1X", ItemFieldDescriptor.AsInt32());
            builder.Add("goto2X", ItemFieldDescriptor.AsInt32());
            builder.Add("goto3X", ItemFieldDescriptor.AsInt32());

            builder.Add("goto4X", ItemFieldDescriptor.AsInt32());
            builder.Add("goto1Y", ItemFieldDescriptor.AsInt32());
            builder.Add("goto2Y", ItemFieldDescriptor.AsInt32());
            builder.Add("goto3Y", ItemFieldDescriptor.AsInt32());
            builder.Add("goto4Y", ItemFieldDescriptor.AsInt32());

            builder.Add(ItemFieldDescriptor.AsInt32());
            builder.Add(ItemFieldDescriptor.AsInt32());
            builder.Add(ItemFieldDescriptor.AsInt32());
            builder.Add(ItemFieldDescriptor.AsInt32());
            builder.Add("lookingDirection", ItemFieldDescriptor.AsInt32(), "0 = up, clockwise");

            builder.Add(ItemFieldDescriptor.AsInt32());
            builder.Add(ItemFieldDescriptor.AsInt32());
            builder.Add(ItemFieldDescriptor.AsInt32());
            builder.Add(ItemFieldDescriptor.AsInt32());
            builder.Add(ItemFieldDescriptor.AsInt32());

            builder.Add(ItemFieldDescriptor.AsInt32());
            builder.Add(ItemFieldDescriptor.AsInt32());
            builder.Add(ItemFieldDescriptor.AsInt32());
            builder.Add(ItemFieldDescriptor.AsInt32());
            builder.Add(ItemFieldDescriptor.AsInt32());

            builder.Add(ItemFieldDescriptor.AsInt32());
            builder.Add(ItemFieldDescriptor.AsInt32());
            builder.Add(ItemFieldDescriptor.AsInt32());
            builder.Add(ItemFieldDescriptor.AsInt32());

            builder.Add("dialogId", ItemFieldDescriptor.AsInt32(), "also text for shop");
            builder.Add(ItemFieldDescriptor.AsInt32());


            return builder.Build();
        }
    }
}
